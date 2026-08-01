using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using Xui.Core.Canvas;
using Xui.Core.Math2D;

namespace Xui.Runtime.Software.Font;

/// <summary>
/// Lays out a string of text using a TrueType font.
/// Produces positioned glyphs and bounding metrics for rendering and measurement.
/// </summary>
public class TextLayout
{
    private readonly List<GlyphPosition> _glyphs = new();
    private readonly TrueTypeFont _font;
    private readonly NFloat _scale;

    private TextAlign textAlign = TextAlign.Left;
    private TextBaseline textBaseline = TextBaseline.Alphabetic;

    public IReadOnlyList<GlyphPosition> Glyphs => _glyphs;

    /// <summary>
    /// The horizontal and vertical bounding metrics of the laid-out text.
    /// </summary>
    public readonly LineMetrics LineMetrics;

    public TextLayout(TrueTypeFont font, string text) : this(font, text, 16)
    {
    }

    public TextLayout(TrueTypeFont font, string text, nfloat fontSize, TextAlign textAlign = TextAlign.Left, TextBaseline textBaseline = TextBaseline.Alphabetic)
    {
        this.textAlign = textAlign;
        this.textBaseline = textBaseline;

        _font = font ?? throw new ArgumentNullException(nameof(font));

        if (font.Head == null || font.Cmap == null || font.Hmtx == null)
            throw new InvalidOperationException("Font is missing required tables (head, cmap, hmtx).");

        _scale = fontSize / font.Head.UnitsPerEm;

        NFloat x = 0;
        NFloat boundingBoxLeft = 0;
        NFloat boundingBoxRight = 0;
        NFloat boundingBoxTop = 0;
        NFloat boundingBoxBottom = 0;
        bool hasGlyphBounds = false;

        Rune? previous = null;

        foreach (var rune in text.EnumerateRunes())
        {
            int? glyphId = font.GetGlyphIndex(rune);
            if (glyphId is not int id)
                continue;

            if (previous is Rune previousRune)
            {
                var kern = font.Kerning[previousRune, rune];
                x += (kern.XAdvance ?? 0) * _scale;
            }

            var metric = font.Hmtx.GetMetric(id);
            _glyphs.Add(new GlyphPosition(id, x, 0));

            if (font.TryGetGlyph(id, out var glyph))
            {
                var bounds = glyph.Bounds;
                var scaledLeft   = x + bounds.XMin * _scale;
                var scaledRight  = x + bounds.XMax * _scale;
                var scaledTop = bounds.YMax * _scale;
                var scaledBottom = bounds.YMin * _scale;

                if (hasGlyphBounds)
                {
                    boundingBoxLeft = nfloat.Min(boundingBoxLeft, scaledLeft);
                    boundingBoxRight = nfloat.Max(boundingBoxRight, scaledRight);
                    boundingBoxTop = nfloat.Max(boundingBoxTop, scaledTop);
                    boundingBoxBottom = nfloat.Max(boundingBoxBottom, scaledBottom);
                }
                else
                {
                    boundingBoxLeft = scaledLeft;
                    boundingBoxRight = scaledRight;
                    boundingBoxTop = scaledTop;
                    boundingBoxBottom = scaledBottom;
                    hasGlyphBounds = true;
                }
            }

            x += metric.AdvanceWidth * _scale;
            previous = rune;
        }

        // Horizontal alignment offset
        nfloat dx = textAlign switch
        {
            TextAlign.Center => x / 2,
            TextAlign.Right => x,
            _ => 0
        };

        LineMetrics = new LineMetrics(x, dx - boundingBoxLeft, boundingBoxRight - dx, boundingBoxTop, boundingBoxBottom);
    }

    /// <summary>
    /// Visits each glyph path, offset and scaled to its layout position.
    /// </summary>
    public void Visit(IGlyphPathBuilder builder)
    {
        var adapter = new TranslatedScaledGlyphBuilder(builder, _scale);

        foreach (var pos in _glyphs)
        {
            if (!_font.TryGetGlyph(pos.GlyphId, out var glyph))
                continue;

            adapter.Translate = new Vector(pos.X, pos.Y);
            glyph.Visit(adapter);
        }
    }

    public readonly record struct GlyphPosition(int GlyphId, NFloat X, NFloat Y);

    private class TranslatedScaledGlyphBuilder : IGlyphPathBuilder
    {
        private readonly IGlyphPathBuilder _sink;
        private readonly NFloat _scale;

        public Vector Translate;

        public TranslatedScaledGlyphBuilder(IGlyphPathBuilder sink, NFloat scale)
        {
            _sink = sink;
            _scale = scale;
        }

        private Point Transform(Point p) => p * _scale + Translate;

        public void MoveTo(Point to) => _sink.MoveTo(Transform(to));
        public void LineTo(Point to) => _sink.LineTo(Transform(to));
        public void CurveTo(Point control, Point to) =>
            _sink.CurveTo(Transform(control), Transform(to));
        public void ClosePath() => _sink.ClosePath();
    }
}
