using System.Text;
using Xui.Core.Abstract.Events;
using Xui.Core.Canvas;
using Xui.Core.Math1D;
using Xui.Core.Math2D;
using Xui.Core.UI.Input;
using Xui.Core.UI.Layers;

using TextBoxLayer = Xui.Core.UI.Layers.FocusBorderLayer<Xui.Core.UI.Layers.TextInputLayer>;

namespace Xui.Core.UI;

/// <summary>
/// A single-line text input view backed by a <see cref="FocusBorderLayer{TChild}"/> +
/// <see cref="TextInputLayer"/> layer pair.
/// Supports typing, backspace, selection, focus/blur, and password masking.
/// </summary>
public class TextBox : FocusedLayerView<TextBoxLayer>
{
    private readonly StringBuilder textBuffer = new();
    private uint anchor;
    private bool isMouseSelecting;

    public TextBox()
    {
        Layer = new TextBoxLayer
        {
            BorderThickness = 1,
            BackgroundColor = Colors.White,
            NormalBorderColor = Colors.Gray,
            FocusBorderColor = Colors.Blue,
            Child = new TextInputLayer
            {
                Padding = 4,
                TextColor = Colors.Black,
                SelectedColor = Colors.White,
                SelectionBackgroundColor = Colors.Blue,
                FontFamily = ["Verdana"],
                FontSize = 15,
                FontStyle = FontStyle.Normal,
                FontWeight = FontWeight.Normal,
                FontStretch = FontStretch.Normal,
            },
        };
    }

    // --- Properties ---

    /// <summary>Gets or sets the text content of the TextBox.</summary>
    public string Text
    {
        get => textBuffer.ToString();
        set
        {
            textBuffer.Clear();
            textBuffer.Append(value);
            SyncText();
            Layer.Child.Selection = new Interval<uint>.ClosedOpen((uint)value.Length, (uint)value.Length);
            InvalidateRender();
            InvalidateMeasure();
        }
    }

    /// <summary>
    /// Gets or sets the current selection range as a half-open interval [Start, End).
    /// When empty (Start == End), represents the caret position.
    /// </summary>
    public Interval<uint>.ClosedOpen Selection
    {
        get => Layer.Child.Selection;
        set => Layer.Child.Selection = value;
    }

    /// <summary>When true, displays masking characters instead of the actual text.</summary>
    public bool IsPassword
    {
        get => Layer.Child.IsPassword;
        set { Layer.Child.IsPassword = value; InvalidateRender(); }
    }

    /// <summary>When true, all text is selected when the TextBox receives focus.</summary>
    public bool SelectAllOnFocus { get; set; } = true;

    /// <summary>
    /// Optional filter that determines whether a typed character is accepted.
    /// Return true to accept, false to reject.
    /// </summary>
    public Func<char, bool>? InputFilter { get; set; }

    /// <summary>Gets or sets the text color.</summary>
    public Color Color
    {
        get => Layer.Child.TextColor;
        set { Layer.Child.TextColor = value; InvalidateRender(); }
    }

    /// <summary>Gets or sets the text color for selected text.</summary>
    public Color SelectedColor
    {
        get => Layer.Child.SelectedColor;
        set { Layer.Child.SelectedColor = value; InvalidateRender(); }
    }

    /// <summary>Gets or sets the background color drawn behind selected text.</summary>
    public Color SelectionBackgroundColor
    {
        get => Layer.Child.SelectionBackgroundColor;
        set { Layer.Child.SelectionBackgroundColor = value; InvalidateRender(); }
    }

    /// <summary>Gets or sets the font family used for rendering the text.</summary>
    public string[] FontFamily
    {
        get => Layer.Child.FontFamily ?? ["Verdana"];
        set { Layer.Child.FontFamily = value; InvalidateMeasure(); }
    }

    /// <summary>Gets or sets the font size in points.</summary>
    public nfloat FontSize
    {
        get => Layer.Child.FontSize;
        set { Layer.Child.FontSize = value; InvalidateMeasure(); }
    }

    /// <summary>Gets or sets the font style (normal, italic, oblique).</summary>
    public FontStyle FontStyle
    {
        get => Layer.Child.FontStyle;
        set { Layer.Child.FontStyle = value; InvalidateMeasure(); }
    }

    /// <summary>Gets or sets the font weight (normal, bold, etc.).</summary>
    public FontWeight FontWeight
    {
        get => Layer.Child.FontWeight;
        set { Layer.Child.FontWeight = value; InvalidateMeasure(); }
    }

    /// <summary>Gets or sets the font stretch (condensed, normal, expanded, etc.).</summary>
    public FontStretch FontStretch
    {
        get => Layer.Child.FontStretch;
        set { Layer.Child.FontStretch = value; InvalidateMeasure(); }
    }

    // --- Focus / Blur ---

    protected override ref bool GetCaretRef() => ref Layer.Child.CaretVisible;

    protected internal override void OnFocus()
    {
        if (SelectAllOnFocus)
        {
            anchor = 0;
            Layer.Child.Selection = new Interval<uint>.ClosedOpen(0, (uint)textBuffer.Length);
        }

        Layer.IsFocused = true;
        Layer.Child.IsFocused = true;
        ResetCaretBlink();
        RequestAnimationFrame();
    }

    protected internal override void OnBlur()
    {
        Layer.IsFocused = false;
        Layer.Child.IsFocused = false;
        Layer.Child.CaretVisible = false;
        InvalidateRender();
    }

    // --- Pointer ---

    public override void OnPointerEvent(ref PointerEventRef e, EventPhase phase)
    {
        if (phase != EventPhase.Bubble)
            return;

        if (e.Type == PointerEventType.Down)
        {
            var wasFocused = IsFocused;
            Focus();

            var cursorPos = HitTestCursor(e.State.Position, e.TextMeasure);
            if (cursorPos.HasValue)
            {
                anchor = cursorPos.Value;
                Layer.Child.Selection = new Interval<uint>.ClosedOpen(cursorPos.Value, cursorPos.Value);
                isMouseSelecting = true;
                CapturePointer(e.PointerId);
            }
            else if (!wasFocused && SelectAllOnFocus)
            {
                anchor = 0;
                Layer.Child.Selection = new Interval<uint>.ClosedOpen(0, (uint)textBuffer.Length);
            }

            ResetCaretBlink();
            InvalidateRender();
        }
        else if (e.Type == PointerEventType.Move && isMouseSelecting)
        {
            var cursorPos = HitTestCursor(e.State.Position, e.TextMeasure);
            if (cursorPos.HasValue)
            {
                var pos = cursorPos.Value;
                Layer.Child.Selection = anchor <= pos
                    ? new Interval<uint>.ClosedOpen(anchor, pos)
                    : new Interval<uint>.ClosedOpen(pos, anchor);
                ResetCaretBlink();
                InvalidateRender();
            }
        }
        else if (e.Type == PointerEventType.Up)
        {
            if (isMouseSelecting)
            {
                isMouseSelecting = false;
                ReleasePointer(e.PointerId);
            }
        }
    }

    // --- Keyboard ---

    public override void OnKeyDown(ref KeyEventRef e)
    {
        var sel = Layer.Child.Selection;
        var len = (uint)textBuffer.Length;

        switch (e.Key)
        {
            case VirtualKey.Back:
                if (!sel.IsEmpty)
                {
                    textBuffer.Remove((int)sel.Start, (int)(sel.End - sel.Start));
                    SyncText();
                    SetCursor(sel.Start);
                }
                else if (sel.Start > 0)
                {
                    textBuffer.Remove((int)sel.Start - 1, 1);
                    SyncText();
                    SetCursor(sel.Start - 1);
                }
                ResetCaretBlink();
                InvalidateRender();
                InvalidateMeasure();
                e.Handled = true;
                break;

            case VirtualKey.Delete:
                if (!sel.IsEmpty)
                {
                    textBuffer.Remove((int)sel.Start, (int)(sel.End - sel.Start));
                    SyncText();
                    SetCursor(sel.Start);
                }
                else if (sel.Start < len)
                {
                    textBuffer.Remove((int)sel.Start, 1);
                    SyncText();
                    SetCursor(sel.Start);
                }
                ResetCaretBlink();
                InvalidateRender();
                InvalidateMeasure();
                e.Handled = true;
                break;

            case VirtualKey.Left:
                if (e.Shift)
                {
                    var cursor = GetCursor();
                    if (cursor > 0) MoveCursor(cursor - 1);
                }
                else if (!sel.IsEmpty)
                    SetCursor(sel.Start);
                else if (sel.Start > 0)
                    SetCursor(sel.Start - 1);
                ResetCaretBlink();
                InvalidateRender();
                e.Handled = true;
                break;

            case VirtualKey.Right:
                if (e.Shift)
                {
                    var cursor = GetCursor();
                    if (cursor < len) MoveCursor(cursor + 1);
                }
                else if (!sel.IsEmpty)
                    SetCursor(sel.End);
                else if (sel.Start < len)
                    SetCursor(sel.Start + 1);
                ResetCaretBlink();
                InvalidateRender();
                e.Handled = true;
                break;

            case VirtualKey.Home:
                if (e.Shift) MoveCursor(0); else SetCursor(0);
                ResetCaretBlink();
                InvalidateRender();
                e.Handled = true;
                break;

            case VirtualKey.End:
                if (e.Shift) MoveCursor(len); else SetCursor(len);
                ResetCaretBlink();
                InvalidateRender();
                e.Handled = true;
                break;
        }
    }

    public override void OnChar(ref KeyEventRef e)
    {
        if (char.IsControl(e.Character))
            return;

        if (InputFilter != null && !InputFilter(e.Character))
            return;

        var sel = Layer.Child.Selection;
        if (!sel.IsEmpty)
            textBuffer.Remove((int)sel.Start, (int)(sel.End - sel.Start));

        textBuffer.Insert((int)sel.Start, e.Character);
        SyncText();
        SetCursor(sel.Start + 1);
        ResetCaretBlink();
        InvalidateRender();
        InvalidateMeasure();
        e.Handled = true;
    }

    // --- Helpers ---

    private void SyncText() => Layer.Child.Text = textBuffer.ToString();

    private void SetCursor(uint pos)
    {
        anchor = pos;
        Layer.Child.Selection = new Interval<uint>.ClosedOpen(pos, pos);
    }

    private void MoveCursor(uint pos)
    {
        if (anchor <= pos)
            Layer.Child.Selection = new Interval<uint>.ClosedOpen(anchor, pos);
        else
            Layer.Child.Selection = new Interval<uint>.ClosedOpen(pos, anchor);
    }

    private uint GetCursor()
    {
        var sel = Layer.Child.Selection;
        return sel.End == anchor ? sel.Start : sel.End;
    }

    private uint? HitTestCursor(Point pointerPosition, ITextMeasureContext? textMeasure)
    {
        if (textMeasure == null)
            return null;

        // Text starts at frame.X + border thickness + padding
        var textX = Frame.X + Layer.BorderThickness + Layer.Child.Padding;
        var clickX = pointerPosition.X - textX;

        var displayText = Layer.Child.IsPassword
            ? new string('\u2022', textBuffer.Length)
            : textBuffer.ToString();

        textMeasure.SetFont(new Font
        {
            FontFamily = Layer.Child.FontFamily,
            FontSize = Layer.Child.FontSize,
            FontStyle = Layer.Child.FontStyle,
            FontWeight = Layer.Child.FontWeight,
            FontStretch = Layer.Child.FontStretch,
        });

        return (uint)textMeasure.HitTestTextPosition(displayText, clickX);
    }

}

