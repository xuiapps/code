// File: Xui/SDK/UI/CurrencyBox.cs
using Xui.Core.Abstract.Events;
using Xui.Core.Canvas;
using Xui.Core.Math2D;
using Xui.Core.UI;
using Xui.Core.UI.Input;
using Xui.Core.UI.Layers;
using Xui.SDK.UI.Layers;

using CurrencyBoxLayer = Xui.Core.UI.Layers.FocusBorderLayer<Xui.SDK.UI.Layers.CurrencyLayer>;

namespace Xui.SDK.UI;

/// <summary>
/// A currency-formatted input control.
/// Tab/click selects all (shows full value highlighted). Typing clears the selection
/// and builds a new value from scratch. The formatted value updates in real time
/// (via <see cref="decimal.TryParse"/>); a blinking caret sits at the right end of
/// the displayed text while editing. Overflow values show a mask (e.g. <c>$#,###.##</c>).
/// </summary>
public class CurrencyBox : LayerView<CurrencyBoxLayer>
{
    private static readonly TimeSpan CaretBlinkInterval = TimeSpan.FromMilliseconds(530);

    private nfloat BorderThickness = 1;

    private decimal _value;
    private string _editBuffer = string.Empty;
    private bool _isEditing;
    private bool _selectAll;
    private TimeSpan _caretToggleTime;

    public CurrencyBox()
    {
        Layer = new CurrencyBoxLayer
        {
            BorderThickness = BorderThickness,
            BackgroundColor = Colors.White,
            NormalBorderColor = Colors.Gray,
            FocusBorderColor = Colors.Blue,
            Child = new CurrencyLayer
            {
                Value = 0m,
                CurrencySymbol = "$",
                SymbolPrefix = true,
                Format = "N2",
                Padding = 4,
                FontFamily = ["Verdana"],
                FontSize = 14,
                TextColor = Colors.Black,
                SelectionBackgroundColor = Colors.Blue,
                SelectedTextColor = Colors.White,
            },
        };
    }

    // --- Properties ---

    public decimal Value
    {
        get => _value;
        set
        {
            _value = value;
            Layer.Child.Value = value;
            InvalidateRender();
        }
    }

    public string CurrencySymbol
    {
        get => Layer.Child.CurrencySymbol;
        set { Layer.Child.CurrencySymbol = value; InvalidateRender(); }
    }

    public bool SymbolPrefix
    {
        get => Layer.Child.SymbolPrefix;
        set { Layer.Child.SymbolPrefix = value; InvalidateRender(); }
    }

    public string Format
    {
        get => Layer.Child.Format;
        set { Layer.Child.Format = value; InvalidateRender(); }
    }

    public override bool Focusable => true;

    // --- Focus / Blur ---

    protected override void OnFocus()
    {
        Layer.IsFocused = true;
        Layer.Child.IsFocused = true;
        _isEditing = true;
        _selectAll = true;
        _editBuffer = string.Empty;
        Layer.Child.SelectAll = true;
        ResetCaretBlink();
        RequestAnimationFrame();
        InvalidateRender();
    }

    protected override void OnBlur()
    {
        Layer.IsFocused = false;
        Layer.Child.IsFocused = false;
        Layer.Child.CaretVisible = false;
        Layer.Child.SelectAll = false;
        _isEditing = false;
        _selectAll = false;
        CommitEdit();
        InvalidateRender();
    }

    // --- Animation (caret blink) ---

    protected override void AnimateCore(TimeSpan previousTime, TimeSpan currentTime)
    {
        if (!IsFocused)
            return;

        if (_caretToggleTime == TimeSpan.Zero)
            _caretToggleTime = currentTime + CaretBlinkInterval;

        if (currentTime >= _caretToggleTime)
        {
            Layer.Child.CaretVisible = !Layer.Child.CaretVisible;
            _caretToggleTime = currentTime + CaretBlinkInterval;
            InvalidateRender();
        }

        RequestAnimationFrame();
        base.AnimateCore(previousTime, currentTime);
    }

    // --- Pointer ---

    public override void OnPointerEvent(ref PointerEventRef e, EventPhase phase)
    {
        if (e.State.PointerType != PointerType.Mouse || phase != EventPhase.Tunnel)
            return;

        if (e.Type == PointerEventType.Down)
            Focus();
    }

    // --- Keyboard ---

    public override void OnKeyDown(ref KeyEventRef e)
    {
        switch (e.Key)
        {
            case VirtualKey.Back:
                if (_selectAll)
                {
                    // Select-all + backspace: clear everything
                    _selectAll = false;
                    Layer.Child.SelectAll = false;
                    _editBuffer = string.Empty;
                    SyncPreview();
                }
                else if (_editBuffer.Length > 0)
                {
                    _editBuffer = _editBuffer[..^1];
                    SyncPreview();
                }
                ResetCaretBlink();
                e.Handled = true;
                break;

            case VirtualKey.Return:
                CommitEdit();
                ResetCaretBlink();
                e.Handled = true;
                break;
        }
    }

    public override void OnChar(ref KeyEventRef e)
    {
        if (char.IsAsciiDigit(e.Character) || e.Character == '.')
        {
            if (_selectAll)
            {
                // First character after select-all replaces everything
                _selectAll = false;
                Layer.Child.SelectAll = false;
                _editBuffer = string.Empty;
            }

            if (e.Character == '.' && _editBuffer.Contains('.')) return;
            _editBuffer += e.Character;
            SyncPreview();
            ResetCaretBlink();
            e.Handled = true;
        }
    }

    // --- Helpers ---

    private void SyncPreview()
    {
        if (decimal.TryParse(_editBuffer, out var preview))
            Layer.Child.Value = preview;
        else if (_editBuffer.Length == 0)
            Layer.Child.Value = 0m;
        InvalidateRender();
    }

    private void CommitEdit()
    {
        if (_editBuffer.Length > 0 && decimal.TryParse(_editBuffer, out var parsed))
            _value = parsed;
        Layer.Child.Value = _value;
        _editBuffer = string.Empty;
        InvalidateRender();
    }

    private void ResetCaretBlink()
    {
        Layer.Child.CaretVisible = true;
        _caretToggleTime = TimeSpan.Zero;
    }
}
