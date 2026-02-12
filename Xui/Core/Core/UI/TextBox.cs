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
    public override bool Focusable => true;

    private readonly StringBuilder textBuffer = new();

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

    public string[] FontFamily { get; set; } = ["Verdana"];

    public nfloat FontSize { get; set; } = 15;

    private static readonly nfloat TextPadding = 4;

    private Font GetFont() => new Font
    {
        FontFamily = this.FontFamily,
        FontSize = this.FontSize,
        FontWeight = FontWeight.Normal,
        FontStretch = FontStretch.Normal,
        FontStyle = FontStyle.Normal,
    };

    private string GetDisplayText() =>
        this.IsPassword ? new string('\u2022', this.textBuffer.Length) : this.Text;

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
            this.InvalidateRender();
            this.InvalidateMeasure();
            e.Handled = true;
        }
    }

    public override void OnChar(ref KeyEventRef e)
    {
        if (char.IsControl(e.Character))
            return;

        this.textBuffer.Append(e.Character);
        this.InvalidateRender();
        this.InvalidateMeasure();
        e.Handled = true;
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
        if (focused)
        {
            var metrics = context.MeasureText(displayText);
            var caretX = frame.X + TextPadding + metrics.Size.Width;
            context.SetFill((Color)Colors.Black);
            context.FillRect(new Rect(caretX, frame.Y + TextPadding, 1, frame.Height - TextPadding * 2));
        }
    }
}
