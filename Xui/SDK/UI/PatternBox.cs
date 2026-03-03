// File: Xui/SDK/UI/PatternBox.cs
using Xui.Core.Abstract.Events;
using Xui.Core.Canvas;
using Xui.Core.Math2D;
using Xui.Core.UI;
using Xui.Core.UI.Input;
using Xui.Core.UI.Layers;
using Xui.SDK.UI.Layers;

namespace Xui.SDK.UI;

/// <summary>
/// A pattern-masked input control. The pattern string uses <c>#</c> as input slot
/// placeholders and any other character as a visual separator
/// (e.g. <c>"####-####-####-####"</c> for a credit-card number).
/// Each slot is an individual bordered box; clicking a slot positions the cursor there.
/// The active slot shows a blinking caret (when empty) or a blinking selection rect
/// (when filled, styled like TextBox selection).
/// </summary>
public class PatternBox : FocusedLayerView<PatternInputLayer>
{
    private readonly int _slotCount;

    public PatternBox(string pattern)
    {
        int count = 0;
        foreach (char c in pattern)
            if (c == '#') count++;
        _slotCount = count;

        Layer = new PatternInputLayer
        {
            Pattern = pattern,
            Value = new char[count],

            SlotWidth = 28,
            SlotHeight = 32,
            SlotGap = 4,
            SeparatorGap = 2,

            SlotBackground = Colors.White,
            SlotBorderColor = Colors.Gray,
            FocusSlotBorderColor = Colors.Blue,
            TextColor = Colors.Black,
            CursorColor = Colors.Black,
            SelectionBackgroundColor = Colors.Blue,
            SelectedTextColor = Colors.White,

            FontSize = 14,
            FontFamily = ["Verdana"],
        };
    }

    // --- Properties ---

    public string? Pattern => Layer.Pattern;

    public string Value
    {
        get
        {
            if (Layer.Value is null) return string.Empty;
            return new string(Layer.Value).TrimEnd('\0');
        }
    }

    protected override ref bool GetCaretRef() => ref Layer.CaretVisible;

    // --- Focus / Blur ---

    protected override void OnFocus()
    {
        Layer.IsFocused = true;
        Layer.CursorSlot = FirstEmptySlot();
        ResetCaretBlink();
        RequestAnimationFrame();
        InvalidateRender();
    }

    protected override void OnBlur()
    {
        Layer.IsFocused = false;
        Layer.CaretVisible = false;
        InvalidateRender();
    }

    // --- Pointer ---

    public override void OnPointerEvent(ref PointerEventRef e, EventPhase phase)
    {
        if (e.State.PointerType != PointerType.Mouse || phase != EventPhase.Tunnel)
            return;

        if (e.Type == PointerEventType.Down)
        {
            nfloat localX = e.State.Position.X - Frame.X;
            int slot = Layer.HitSlot(localX);
            if (slot >= 0)
                Layer.CursorSlot = slot;
            Focus();
            ResetCaretBlink();
            InvalidateRender();
        }
    }

    // --- Keyboard ---

    public override void OnKeyDown(ref KeyEventRef e)
    {
        switch (e.Key)
        {
            case VirtualKey.Back:
                ClearCurrentSlot();
                if (Layer.CursorSlot > 0)
                    Layer.CursorSlot--;
                e.Handled = true;
                break;

            case VirtualKey.Delete:
                ClearCurrentSlot();
                e.Handled = true;
                break;

            case VirtualKey.Left:
                if (Layer.CursorSlot > 0)
                    Layer.CursorSlot--;
                e.Handled = true;
                break;

            case VirtualKey.Right:
                if (Layer.CursorSlot < _slotCount - 1)
                    Layer.CursorSlot++;
                e.Handled = true;
                break;
        }

        ResetCaretBlink();
        InvalidateRender();
    }

    public override void OnChar(ref KeyEventRef e)
    {
        if (_slotCount == 0 || Layer.Value is null) return;
        if (Layer.CursorSlot >= _slotCount) return;

        bool isLastSlot = Layer.CursorSlot == _slotCount - 1;
        Layer.Value[Layer.CursorSlot] = e.Character;

        if (isLastSlot)
            (this.GetService(typeof(IFocus)) as IFocus)?.Next();
        else
            Layer.CursorSlot++;

        e.Handled = true;
        ResetCaretBlink();
        InvalidateRender();
    }

    // --- Helpers ---

    private void ClearCurrentSlot()
    {
        if (Layer.Value is null || Layer.CursorSlot >= _slotCount) return;
        Layer.Value[Layer.CursorSlot] = '\0';
    }

    private int FirstEmptySlot()
    {
        if (Layer.Value is null) return 0;
        for (int i = 0; i < _slotCount; i++)
            if (Layer.Value[i] == '\0') return i;
        return _slotCount - 1;
    }
}
