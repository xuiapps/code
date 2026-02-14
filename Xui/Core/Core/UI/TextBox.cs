using System;
using System.Text;
using Xui.Core.Abstract.Events;
using Xui.Core.Canvas;
using Xui.Core.Math2D;
using Xui.Core.UI.Input;

namespace Xui.Core.UI;

/// <summary>
/// A minimal single-line text input view.
/// Supports typing, backspace, focus/blur, and password masking.
/// </summary>
public class TextBox : View
{
    private static readonly TimeSpan CaretBlinkInterval = TimeSpan.FromMilliseconds(530);

    public override bool Focusable => true;

    private readonly StringBuilder textBuffer = new();
    private bool caretVisible;
    private TimeSpan caretToggleTime;

    /// <summary>
    /// Gets or sets the text content of the TextBox.
    /// </summary>
    public string Text
    {
        get => this.textBuffer.ToString();
        set
        {
            this.textBuffer.Clear();
            this.textBuffer.Append(value);
            this.InvalidateRender();
            this.InvalidateMeasure();
        }
    }

    /// <summary>
    /// When true, displays masking characters instead of the actual text.
    /// </summary>
    public bool IsPassword { get; set; }

    /// <summary>
    /// Optional filter that determines whether a character is accepted.
    /// Return true to accept, false to reject.
    /// </summary>
    public Func<char, bool>? InputFilter { get; set; }

    /// <summary>
    /// Gets or sets the font family used for rendering the text.
    /// </summary>
    public string[] FontFamily { get; set; } = ["Verdana"];

    /// <summary>
    /// Gets or sets the font size in points.
    /// </summary>
    public nfloat FontSize { get; set; } = 15;

    /// <summary>
    /// Gets or sets the font style (e.g., normal, italic, oblique).
    /// </summary>
    public FontStyle FontStyle { get; set; } = FontStyle.Normal;

    /// <summary>
    /// Gets or sets the font weight (e.g., normal, bold, numeric weight).
    /// </summary>
    public FontWeight FontWeight { get; set; } = FontWeight.Normal;

    /// <summary>
    /// Gets or sets the font stretch (e.g., condensed, semi-expanded etc.).
    /// </summary>
    public FontStretch FontStretch { get; set; } = FontStretch.Normal;

    private static readonly nfloat TextPadding = 4;

    private Font GetFont() => new Font
    {
        FontFamily = this.FontFamily,
        FontSize = this.FontSize,
        FontWeight = this.FontWeight,
        FontStretch = this.FontStretch,
        FontStyle = this.FontStyle,
    };

    private string GetDisplayText() =>
        this.IsPassword ? new string('\u2022', this.textBuffer.Length) : this.Text;

    protected internal override void OnFocus()
    {
        this.caretVisible = true;
        this.caretToggleTime = TimeSpan.Zero;
        this.RequestAnimationFrame();
    }

    protected internal override void OnBlur()
    {
        this.caretVisible = false;
        this.InvalidateRender();
    }

    protected override void AnimateCore(TimeSpan previousTime, TimeSpan currentTime)
    {
        if (!this.IsFocused)
            return;

        if (this.caretToggleTime == TimeSpan.Zero)
        {
            this.caretToggleTime = currentTime + CaretBlinkInterval;
        }

        if (currentTime >= this.caretToggleTime)
        {
            this.caretVisible = !this.caretVisible;
            this.caretToggleTime = currentTime + CaretBlinkInterval;
            this.InvalidateRender();
        }

        this.RequestAnimationFrame();
    }

    public override void OnPointerEvent(ref PointerEventRef e, EventPhase phase)
    {
        if (phase == EventPhase.Bubble && e.Type == PointerEventType.Down)
        {
            this.Focus();
        }
    }

    public override void OnKeyDown(ref KeyEventRef e)
    {
        if (e.Key == VirtualKey.Back && this.textBuffer.Length > 0)
        {
            this.textBuffer.Remove(this.textBuffer.Length - 1, 1);
            this.ResetCaretBlink();
            this.InvalidateRender();
            this.InvalidateMeasure();
            e.Handled = true;
        }
    }

    public override void OnChar(ref KeyEventRef e)
    {
        if (char.IsControl(e.Character))
            return;

        if (this.InputFilter != null && !this.InputFilter(e.Character))
            return;

        this.textBuffer.Append(e.Character);
        this.ResetCaretBlink();
        this.InvalidateRender();
        this.InvalidateMeasure();
        e.Handled = true;
    }

    private void ResetCaretBlink()
    {
        this.caretVisible = true;
        this.caretToggleTime = TimeSpan.Zero;
    }

    protected override Size MeasureCore(Size availableBorderEdgeSize, IMeasureContext context)
    {
        context.SetFont(this.GetFont());
        var displayText = this.GetDisplayText();
        var textMetrics = context.MeasureText(displayText.Length > 0 ? displayText : "X");
        var height = textMetrics.Size.Height + TextPadding * 2;
        // Use available width for the TextBox, but ensure a minimum
        var width = nfloat.Max(availableBorderEdgeSize.Width, 100);
        return new Size(width, height);
    }

    protected override void RenderCore(IContext context)
    {
        var frame = this.Frame;
        var focused = this.IsFocused;

        // Background
        context.SetFill((Color)Colors.White);
        context.FillRect(frame);

        // Border
        context.LineWidth = 1;
        context.SetStroke(focused ? (Color)Colors.Blue : (Color)Colors.Gray);
        context.StrokeRect(frame);

        // Text
        context.SetFont(this.GetFont());
        context.TextBaseline = TextBaseline.Top;
        context.TextAlign = TextAlign.Left;
        context.SetFill((Color)Colors.Black);
        var displayText = this.GetDisplayText();
        context.FillText(displayText, new Point(frame.X + TextPadding, frame.Y + TextPadding));

        // Caret
        if (focused && this.caretVisible)
        {
            var metrics = context.MeasureText(displayText);
            var caretX = frame.X + TextPadding + metrics.Size.Width;
            context.SetFill((Color)Colors.Black);
            context.FillRect(new Rect(caretX, frame.Y + TextPadding, 1, frame.Height - TextPadding * 2));
        }
    }
}
