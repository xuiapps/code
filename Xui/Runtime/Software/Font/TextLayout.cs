using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
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

    public IReadOnlyList<GlyphPosition> Glyphs => _glyphs;

    /// <summary>
    /// The horizontal and vertical bounding metrics of the laid-out text.
    /// </summary>
    public readonly LineMetrics LineMetrics;

    public TextLayout(TrueTypeFont font, string text) : this(font, text, 16)
    {
    }

    public TextLayout(TrueTypeFont font, string text, nfloat fontSize)
    {
        _font = font ?? throw new ArgumentNullException(nameof(font));

        if (font.Head == null || font.Cmap == null || font.Hmtx == null)
            throw new InvalidOperationException("Font is missing required tables (head, cmap, hmtx).");

        _scale = fontSize / font.Head.UnitsPerEm;

        NFloat x = 0;
        NFloat boundingBoxLeft = 0;
        NFloat boundingBoxRight = 0;
        NFloat boundingBoxAscent = 0;
        NFloat boundingBoxDescent = 0;

        char? prevChar = null;

        foreach (char ch in text)
        {
            int? glyphId = font.GetGlyphIndexFromChar(ch);
            if (glyphId is not int id)
                continue;

            if (prevChar is char prev)
            {
                var kern = font.Kerning[prev, ch];
                x += (kern.XAdvance ?? 0) * _scale;
            }

            var metric = font.Hmtx.GetMetric(id);
            _glyphs.Add(new GlyphPosition(id, x, 0));

            if (font.TryGetGlyph(id, out var glyph))
            {
                var bounds = glyph.Bounds;
                var scaledLeft   = x + bounds.Left * _scale;
                var scaledRight  = x + bounds.Right * _scale;
                var scaledAscent = -bounds.Top * _scale;
                var scaledDescent = bounds.Bottom * _scale;

                boundingBoxLeft = nfloat.Min(boundingBoxLeft, scaledLeft);
                boundingBoxRight = nfloat.Max(boundingBoxRight, scaledRight);
                boundingBoxAscent = nfloat.Max(boundingBoxAscent, scaledAscent);
                boundingBoxDescent = nfloat.Max(boundingBoxDescent, scaledDescent);
            }

            x += metric.AdvanceWidth * _scale;
            prevChar = ch;
        }

        LineMetrics = new LineMetrics(x, boundingBoxLeft, boundingBoxRight, boundingBoxAscent, boundingBoxDescent);
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
