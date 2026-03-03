using Xui.Core.Abstract.Events;
using Xui.Core.Canvas;
using Xui.Core.Math1D;
using Xui.Core.Math2D;
using Xui.Core.UI;
using Xui.Core.UI.Input;
using Xui.Core.UI.Layers;
using Xui.SDK.UI.Layers;

// Layout: FocusBorderLayer wraps [ − | TextInputLayer | + ]
using NumberInner = Xui.Core.UI.Layers.DockLeft<
    Xui.SDK.UI.Layers.StepButton,
    Xui.Core.UI.Layers.DockRight<
        Xui.Core.UI.Layers.TextInputLayer,
        Xui.SDK.UI.Layers.StepButton>>;

using NumberBoxLayer = Xui.Core.UI.Layers.FocusBorderLayer<
    Xui.Core.UI.Layers.DockLeft<
        Xui.SDK.UI.Layers.StepButton,
        Xui.Core.UI.Layers.DockRight<
            Xui.Core.UI.Layers.TextInputLayer,
            Xui.SDK.UI.Layers.StepButton>>>;

namespace Xui.SDK.UI;

/// <summary>
/// A numeric stepper input control laid out as [ − | value | + ].
/// Supports pointer clicks on the step buttons and keyboard arrow / digit input.
/// Clicking or tabbing into the center value area selects all; typing replaces the value.
/// </summary>
public class NumberBox : FocusedLayerView<NumberBoxLayer>
{
    private nfloat ButtonWidth = 32;
    private nfloat BorderThickness = 1;

    private double _value;
    private string _editBuffer = string.Empty;
    private bool _isEditing;
    private bool _selectAll;
    private int _pressedButton = -1; // 0 = decrement, 1 = increment

    // Shorthand ref to the center text layer to avoid deep paths throughout.
    private ref TextInputLayer Center => ref Layer.Child.Right.Left;

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
                Right = new DockRight<TextInputLayer, StepButton>
                {
                    Left = new TextInputLayer
                    {
                        Text = "0",
                        TextAlign = TextAlign.Center,
                        FontFamily = ["Verdana"],
                        FontSize = 14,
                        TextColor = Colors.Black,
                        SelectedColor = Colors.White,
                        SelectionBackgroundColor = Colors.Blue,
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

    protected override ref bool GetCaretRef() => ref Center.CaretVisible;

    // --- Focus / Blur ---

    protected override void OnFocus()
    {
        Layer.IsFocused = true;
        Center.IsFocused = true;
        _isEditing = true;
        _selectAll = true;
        _editBuffer = _value.ToString(Format);
        SelectAll();
        ResetCaretBlink();
        RequestAnimationFrame();
        InvalidateRender();
    }

    protected override void OnBlur()
    {
        Layer.IsFocused = false;
        Center.IsFocused = false;
        Center.CaretVisible = false;
        Center.Selection = default;
        _isEditing = false;
        _selectAll = false;
        CommitEdit();
        InvalidateRender();
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
                _editBuffer = string.Empty;
            }
            _editBuffer += e.Character;
            SyncLabelFromEdit();
            ResetCaretBlink();
            e.Handled = true;
        }
    }

    // --- Helpers ---

    private void SelectAll()
    {
        var len = (uint)(Center.Text?.Length ?? 0);
        Center.Selection = new Interval<uint>.ClosedOpen(0, len);
    }

    private void SetCaretAtEnd()
    {
        var len = (uint)(Center.Text?.Length ?? 0);
        Center.Selection = new Interval<uint>.ClosedOpen(len, len);
    }

    private void ApplyStep(double delta)
    {
        Value = Math.Clamp(_value + delta, Min, Max);
        _editBuffer = _value.ToString(Format);
        SyncLabel();
        _selectAll = true;
        SelectAll();
        ResetCaretBlink();
    }

    private void SyncLabel()
    {
        Center.Text = _value.ToString(Format);
        if (_selectAll) SelectAll();
        else SetCaretAtEnd();
    }

    private void SyncLabelFromEdit()
    {
        Center.Text = _editBuffer;
        SetCaretAtEnd();
        InvalidateRender();
    }

    private void CommitEdit()
    {
        if (double.TryParse(_editBuffer, out var parsed))
            _value = Math.Clamp(parsed, Min, Max);
        Center.Text = _value.ToString(Format);
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
}
