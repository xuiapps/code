using System;
using System.Text;
using Xui.Core.Abstract.Events;
using Xui.Core.Canvas;
using Xui.Core.Math1D;
using Xui.Core.Math2D;
using Xui.Core.UI.Input;

namespace Xui.Core.UI;

/// <summary>
/// A minimal single-line text input view.
/// Supports typing, backspace, selection, focus/blur, and password masking.
/// </summary>
public class TextBox : View
{
    private static readonly TimeSpan CaretBlinkInterval = TimeSpan.FromMilliseconds(530);
    private static readonly nfloat TextPadding = 4;

    public override bool Focusable => true;

    private readonly StringBuilder textBuffer = new();
    private Interval<uint>.ClosedOpen selection;
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
            this.selection = new Interval<uint>.ClosedOpen((uint)value.Length, (uint)value.Length);
            this.InvalidateRender();
            this.InvalidateMeasure();
        }
    }

    /// <summary>
    /// Gets or sets the current selection range as a half-open interval [Start, End).
    /// When empty (Start == End), represents the caret position.
    /// </summary>
    public Interval<uint>.ClosedOpen Selection
    {
        get => this.selection;
        set => this.selection = value;
    }

    /// <summary>
    /// When true, displays masking characters instead of the actual text.
    /// </summary>
    public bool IsPassword { get; set; }

    /// <summary>
    /// When true, all text is selected when the TextBox receives focus.
    /// </summary>
    public bool SelectAllOnFocus { get; set; } = true;

    /// <summary>
    /// Optional filter that determines whether a character is accepted.
    /// Return true to accept, false to reject.
    /// </summary>
    public Func<char, bool>? InputFilter { get; set; }

    /// <summary>
    /// Gets or sets the text color.
    /// </summary>
    public Color Color { get; set; } = Colors.Black;

    /// <summary>
    /// Gets or sets the text color for selected text.
    /// </summary>
    public Color SelectedColor { get; set; } = Colors.White;

    /// <summary>
    /// Gets or sets the background color drawn behind selected text.
    /// </summary>
    public Color SelectionBackgroundColor { get; set; } = Colors.Blue;

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
        if (this.SelectAllOnFocus)
            this.selection = new Interval<uint>.ClosedOpen(0, (uint)this.textBuffer.Length);

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
        if (e.Key == VirtualKey.Back)
        {
            var sel = this.selection;
            if (!sel.IsEmpty)
            {
                this.textBuffer.Remove((int)sel.Start, (int)(sel.End - sel.Start));
                this.selection = new Interval<uint>.ClosedOpen(sel.Start, sel.Start);
            }
            else if (sel.Start > 0)
            {
                this.textBuffer.Remove((int)sel.Start - 1, 1);
                this.selection = new Interval<uint>.ClosedOpen(sel.Start - 1, sel.Start - 1);
            }

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

        var sel = this.selection;
        if (!sel.IsEmpty)
            this.textBuffer.Remove((int)sel.Start, (int)(sel.End - sel.Start));

        this.textBuffer.Insert((int)sel.Start, e.Character);
        this.selection = new Interval<uint>.ClosedOpen(sel.Start + 1, sel.Start + 1);
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

        // Text & Selection
        context.SetFont(this.GetFont());
        context.TextBaseline = TextBaseline.Top;
        context.TextAlign = TextAlign.Left;

        var displayText = this.GetDisplayText();
        var sel = this.selection;
        var textX = frame.X + TextPadding;
        var textY = frame.Y + TextPadding;

        if (!sel.IsEmpty && focused)
        {
            var selStart = (int)sel.Start;
            var selEnd = (int)sel.End;

            var beforeSelWidth = selStart > 0
                ? context.MeasureText(displayText[..selStart]).Size.Width
                : (nfloat)0;
            var toSelEndWidth = context.MeasureText(displayText[..selEnd]).Size.Width;

            // Selection background
            context.SetFill(this.SelectionBackgroundColor);
            context.FillRect(new Rect(
                textX + beforeSelWidth,
                frame.Y + TextPadding,
                toSelEndWidth - beforeSelWidth,
                frame.Height - TextPadding * 2));

            // Text before selection
            if (selStart > 0)
            {
                context.SetFill(this.Color);
                context.FillText(displayText[..selStart], new Point(textX, textY));
            }

            // Selected text
            context.SetFill(this.SelectedColor);
            context.FillText(displayText[selStart..selEnd], new Point(textX + beforeSelWidth, textY));

            // Text after selection
            if (selEnd < displayText.Length)
            {
                context.SetFill(this.Color);
                context.FillText(displayText[selEnd..], new Point(textX + toSelEndWidth, textY));
            }
        }
        else
        {
            // No selection — draw all text with normal color
            context.SetFill(this.Color);
            context.FillText(displayText, new Point(textX, textY));
        }

        // Caret (only when no selection active)
        if (focused && this.caretVisible && sel.IsEmpty)
        {
            var cursorPos = (int)sel.Start;
            var caretOffset = cursorPos > 0
                ? context.MeasureText(displayText[..cursorPos]).Size.Width
                : (nfloat)0;
            var caretX = textX + caretOffset;
            context.SetFill(this.Color);
            context.FillRect(new Rect(caretX, frame.Y + TextPadding, 1, frame.Height - TextPadding * 2));
        }
    }
}
