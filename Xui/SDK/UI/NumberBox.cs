using Xui.Core.Abstract.Events;
using Xui.Core.Canvas;
using Xui.Core.Math2D;
using Xui.Core.UI;
using Xui.Core.UI.Input;
using Xui.Core.UI.Layers;
using Xui.SDK.UI.Layers;

// Layout: DockLeft pins − on the left; DockRight pins + on the right; NumberCenterLayer fills center.
using NumberInner = Xui.Core.UI.Layers.DockLeft<
    Xui.SDK.UI.Layers.StepButton,
    Xui.Core.UI.Layers.DockRight<
        Xui.SDK.UI.Layers.NumberCenterLayer,
        Xui.SDK.UI.Layers.StepButton>>;

using NumberBoxLayer = Xui.Core.UI.Layers.FocusBorderLayer<
    Xui.Core.UI.Layers.DockLeft<
        Xui.SDK.UI.Layers.StepButton,
        Xui.Core.UI.Layers.DockRight<
            Xui.SDK.UI.Layers.NumberCenterLayer,
            Xui.SDK.UI.Layers.StepButton>>>;

namespace Xui.SDK.UI;

/// <summary>
/// A numeric stepper input control laid out as [ − | value | + ].
/// Supports pointer clicks on the step buttons and keyboard arrow / digit input.
/// Clicking or tabbing into the center value area selects all; typing replaces the value.
/// </summary>
public class NumberBox : LayerView<NumberBoxLayer>
{
    private static readonly TimeSpan CaretBlinkInterval = TimeSpan.FromMilliseconds(530);

    private nfloat ButtonWidth = 32;
    private nfloat BorderThickness = 1;

    private double _value;
    private string _editBuffer = string.Empty;
    private bool _isEditing;
    private bool _selectAll;
    private int _pressedButton = -1; // 0 = decrement, 1 = increment
    private TimeSpan _caretToggleTime;

    public NumberBox()
    {
        Layer = new NumberBoxLayer
        {
            BorderThickness = BorderThickness,
            BackgroundColor = Colors.White,
            NormalBorderColor = Colors.Gray,
            FocusBorderColor = Colors.Blue,
            Child = new NumberInner
            {
                Left = new StepButton
                {
                    Label = "\u2212", // −
                    PreferredWidth = ButtonWidth,
                    CornerRadius = 0,
                    FontSize = 16,
                    FontFamily = ["Verdana"],
                    NormalColor = new Color(0xF0, 0xF0, 0xF0, 0xFF),
                    PressedColor = new Color(0xCC, 0xCC, 0xCC, 0xFF),
                    DisabledColor = new Color(0xF8, 0xF8, 0xF8, 0xFF),
                    TextColor = Colors.Black,
                    DisabledTextColor = Colors.Gray,
                },
                Right = new DockRight<NumberCenterLayer, StepButton>
                {
                    Left = new NumberCenterLayer
                    {
                        Text = "0",
                        FontFamily = ["Verdana"],
                        FontSize = 14,
                        TextColor = Colors.Black,
                        SelectionBackgroundColor = Colors.Blue,
                        SelectedTextColor = Colors.White,
                    },
                    Right = new StepButton
                    {
                        Label = "+",
                        PreferredWidth = ButtonWidth,
                        CornerRadius = 0,
                        FontSize = 16,
                        FontFamily = ["Verdana"],
                        NormalColor = new Color(0xF0, 0xF0, 0xF0, 0xFF),
                        PressedColor = new Color(0xCC, 0xCC, 0xCC, 0xFF),
                        DisabledColor = new Color(0xF8, 0xF8, 0xF8, 0xFF),
                        TextColor = Colors.Black,
                        DisabledTextColor = Colors.Gray,
                    },
                },
            },
        };
    }

    // --- Properties ---

    public double Value
    {
        get => _value;
        set
        {
            var clamped = Math.Clamp(value, Min, Max);
            if (_value == clamped) return;
            _value = clamped;
            SyncLabel();
            UpdateStepButtonStates();
            InvalidateRender();
        }
    }

    public double Min { get; set; } = double.MinValue;
    public double Max { get; set; } = double.MaxValue;
    public double Step { get; set; } = 1.0;
    public string Format { get; set; } = "G";

    public override bool Focusable => true;

    // --- Focus / Blur ---

    protected override void OnFocus()
    {
        Layer.IsFocused = true;
        Layer.Child.Right.Left.IsFocused = true;
        _isEditing = true;
        _selectAll = true;
        _editBuffer = _value.ToString(Format);
        Layer.Child.Right.Left.SelectAll = true;
        ResetCaretBlink();
        RequestAnimationFrame();
        InvalidateRender();
    }

    protected override void OnBlur()
    {
        Layer.IsFocused = false;
        Layer.Child.Right.Left.IsFocused = false;
        Layer.Child.Right.Left.CaretVisible = false;
        Layer.Child.Right.Left.SelectAll = false;
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
            Layer.Child.Right.Left.CaretVisible = !Layer.Child.Right.Left.CaretVisible;
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
        {
            var btn = HitButton(e.State.Position.X);
            if (btn >= 0)
            {
                _pressedButton = btn;
                SetButtonPressed(btn, true);
                CapturePointer(e.PointerId);
                Focus();
                InvalidateRender();
            }
            else
            {
                Focus();
            }
        }
        else if (e.Type == PointerEventType.Up && _pressedButton >= 0)
        {
            var btn = HitButton(e.State.Position.X);
            if (btn == _pressedButton)
                ApplyStep(_pressedButton == 0 ? -Step : Step);

            SetButtonPressed(_pressedButton, false);
            ReleasePointer(e.PointerId);
            _pressedButton = -1;
            InvalidateRender();
        }
    }

    // --- Keyboard ---

    public override void OnKeyDown(ref KeyEventRef e)
    {
        switch (e.Key)
        {
            case VirtualKey.Up:
                ApplyStep(Step);
                e.Handled = true;
                break;
            case VirtualKey.Down:
                ApplyStep(-Step);
                e.Handled = true;
                break;
            case VirtualKey.Back:
                if (_selectAll)
                {
                    _selectAll = false;
                    Layer.Child.Right.Left.SelectAll = false;
                    _editBuffer = string.Empty;
                    SyncLabelFromEdit();
                }
                else if (_editBuffer.Length > 0)
                {
                    _editBuffer = _editBuffer[..^1];
                    SyncLabelFromEdit();
                }
                ResetCaretBlink();
                e.Handled = true;
                break;
        }
    }

    public override void OnChar(ref KeyEventRef e)
    {
        if (char.IsAsciiDigit(e.Character) || e.Character == '-' || e.Character == '.')
        {
            if (_selectAll)
            {
                _selectAll = false;
                Layer.Child.Right.Left.SelectAll = false;
                _editBuffer = string.Empty;
            }
            _editBuffer += e.Character;
            SyncLabelFromEdit();
            ResetCaretBlink();
            e.Handled = true;
        }
    }

    // --- Helpers ---

    private void ApplyStep(double delta)
    {
        Value = Math.Clamp(_value + delta, Min, Max);
        _editBuffer = _value.ToString(Format);
        SyncLabel();
        _selectAll = true;
        Layer.Child.Right.Left.SelectAll = true;
        ResetCaretBlink();
    }

    private void SyncLabel()
    {
        Layer.Child.Right.Left.Text = _value.ToString(Format);
    }

    private void SyncLabelFromEdit()
    {
        Layer.Child.Right.Left.Text = _editBuffer;
        InvalidateRender();
    }

    private void CommitEdit()
    {
        if (double.TryParse(_editBuffer, out var parsed))
            _value = Math.Clamp(parsed, Min, Max);
        SyncLabel();
        UpdateStepButtonStates();
    }

    private void UpdateStepButtonStates()
    {
        Layer.Child.Left.Disabled = _value <= Min;
        Layer.Child.Right.Right.Disabled = _value >= Max;
    }

    private void SetButtonPressed(int btn, bool pressed)
    {
        if (btn == 0) Layer.Child.Left.Pressed = pressed;
        else Layer.Child.Right.Right.Pressed = pressed;
    }

    private int HitButton(nfloat px)
    {
        var left = Frame.X + BorderThickness;
        var right = Frame.Right - BorderThickness;

        if (px >= left && px < left + ButtonWidth) return 0;
        if (px >= right - ButtonWidth && px < right) return 1;
        return -1;
    }

    private void ResetCaretBlink()
    {
        Layer.Child.Right.Left.CaretVisible = true;
        _caretToggleTime = TimeSpan.Zero;
    }
}
