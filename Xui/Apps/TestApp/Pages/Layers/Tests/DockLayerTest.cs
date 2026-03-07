using System.Runtime.InteropServices;
using Xui.Core.Abstract.Events;
using Xui.Core.Canvas;
using Xui.Core.Math2D;
using Xui.Core.UI;
using Xui.Core.UI.Input;
using Xui.Core.UI.Layer;
using static Xui.Core.Canvas.Colors;

namespace Xui.Apps.TestApp.Pages.Layers.Tests;

/// <summary>
/// Demonstrates <see cref="DockLayer"/> composing <see cref="ButtonLayer"/> with
/// <see cref="TextInputLayer"/> for three common input patterns.
/// </summary>
public class DockLayerTest : View
{
    private readonly Label lblClearable, lblCombo, lblNumeric;
    private readonly ClearableInput clearable;
    private readonly ComboInput combo;
    private readonly NumericInput numeric;

    public override int Count => 6;
    public override View this[int index] => index switch
    {
        0 => lblClearable,
        1 => clearable,
        2 => lblCombo,
        3 => combo,
        4 => lblNumeric,
        5 => numeric,
        _ => throw new IndexOutOfRangeException(),
    };

    public DockLayerTest()
    {
        lblClearable = MakeLabel("Clearable input");
        clearable    = new ClearableInput();

        lblCombo = MakeLabel("Combo / dropdown");
        combo    = new ComboInput();

        lblNumeric = MakeLabel("Numeric stepper");
        numeric    = new NumericInput();

        AddProtectedChild(lblClearable);
        AddProtectedChild(clearable);
        AddProtectedChild(lblCombo);
        AddProtectedChild(combo);
        AddProtectedChild(lblNumeric);
        AddProtectedChild(numeric);
    }

    private static Label MakeLabel(string text) => new Label
    {
        Text       = text,
        FontFamily = ["Inter"],
        FontSize   = 12,
        FontWeight = FontWeight.Normal,
    };

    protected override Size MeasureCore(Size availableSize, IMeasureContext context)
    {
        NFloat w = availableSize.Width - 40;
        foreach (var v in new View[] { lblClearable, clearable, lblCombo, combo, lblNumeric, numeric })
            v.Measure(new Size(w, availableSize.Height), context);
        return availableSize;
    }

    protected override void ArrangeCore(Rect rect, IMeasureContext context)
    {
        NFloat x   = rect.X + 20;
        NFloat w   = rect.Width - 40;
        NFloat y   = rect.Y + 20;
        NFloat gap = 10;

        void Place(View v, NFloat h) { v.Arrange(new Rect(x, y, w, h), context); y += h + gap; }

        Place(lblClearable, 16);
        Place(clearable,    clearable.Frame.Height > 0 ? clearable.Frame.Height : 28);
        y += 8;
        Place(lblCombo, 16);
        Place(combo,    combo.Frame.Height > 0 ? combo.Frame.Height : 28);
        y += 8;
        Place(lblNumeric, 16);
        Place(numeric,    numeric.Frame.Height > 0 ? numeric.Frame.Height : 28);
    }

    protected override void RenderCore(IContext context)
    {
        context.SetFill(new Color(0xF8, 0xF9, 0xFA, 0xFF));
        context.FillRect(Frame);
        base.RenderCore(context);
    }

    // ── Widget 1: text input with a clear "×" button that appears when text is non-empty ──

    private class ClearableInput
        : LayerView<FocusBorderLayer<DockLayer.Dock2<View, TextInputLayer, ButtonLayer>>>
    {
        public override bool Focusable => true;

        public ClearableInput()
        {
            // Outer border
            Layer.BackgroundColor    = White;
            Layer.BorderThickness    = 1;
            Layer.BorderColor        = new Color(0xCC, 0xCC, 0xCC, 0xFF);
            Layer.FocusedBorderColor = new Color(0x00, 0x78, 0xD4, 0xFF);
            Layer.CornerRadius       = new CornerRadius(5);
            Layer.Padding            = 0;

            // Dock alignment
            Layer.Border.Child.Child1.Align = DockLayer.Align.Stretch;
            Layer.Border.Child.Child2.Align = DockLayer.Align.Right;

            // Text layer
            ref var inp = ref Layer.Border.Child.Child1.Child;
            inp.Color                    = Black;
            inp.SelectedColor            = White;
            inp.SelectionBackgroundColor = new Color(0x00, 0x78, 0xD4, 0xFF);
            inp.Padding                  = 3;
            inp.FontFamily               = ["Inter"];
            inp.FontSize                 = 15;
            inp.FontWeight               = FontWeight.Normal;
            inp.SelectAllOnFocus         = true;

            // Clear button (hidden until text is present)
            Layer.Border.Child.Child2.Child = new ButtonLayer
            {
                Label         = "×",
                Margin        = 2,
                CornerRadius  = new CornerRadius(5),
                NormalColor   = new Color(0xE0, 0xE0, 0xE0, 0xFF),
                HoverColor    = new Color(0xC8, 0xC8, 0xC8, 0xFF),
                PressedColor  = new Color(0xA8, 0xA8, 0xA8, 0xFF),
                LabelColor    = new Color(0x44, 0x44, 0x44, 0xFF),
                FontSize      = 14,
                Visible       = false,
                RequestRedraw = InvalidateRender,
                OnClick       = () =>
                {
                    Layer.Border.Child.Child1.Child.Text     = "";
                    Layer.Border.Child.Child2.Child.Visible  = false;
                    InvalidateMeasure();
                },
            };
        }

        public override void OnChar(ref KeyEventRef e)
        {
            base.OnChar(ref e);
            SyncClearButton();
        }

        public override void OnKeyDown(ref KeyEventRef e)
        {
            base.OnKeyDown(ref e);
            SyncClearButton();
        }

        private void SyncClearButton()
        {
            bool hasText   = Layer.Border.Child.Child1.Child.Text.Length > 0;
            bool wasVisible = Layer.Border.Child.Child2.Child.Visible;
            if (hasText != wasVisible)
            {
                Layer.Border.Child.Child2.Child.Visible = hasText;
                InvalidateMeasure();
            }
        }
    }

    // ── Widget 2: combobox-style with a right-aligned dropdown arrow ──

    private class ComboInput
        : LayerView<FocusBorderLayer<DockLayer.Dock2<View, TextInputLayer, ButtonLayer>>>
    {
        public override bool Focusable => true;

        public ComboInput()
        {
            // Border — no right padding so the arrow button is flush to the border edge
            Layer.BackgroundColor    = White;
            Layer.BorderThickness    = 1;
            Layer.BorderColor        = new Color(0xCC, 0xCC, 0xCC, 0xFF);
            Layer.FocusedBorderColor = new Color(0x00, 0x78, 0xD4, 0xFF);
            Layer.CornerRadius       = new CornerRadius(5);
            Layer.Padding            = 0;

            // Dock alignment
            Layer.Border.Child.Child1.Align = DockLayer.Align.Stretch;
            Layer.Border.Child.Child2.Align = DockLayer.Align.Right;

            // Text layer
            ref var inp = ref Layer.Border.Child.Child1.Child;
            inp.Color                    = Black;
            inp.SelectedColor            = White;
            inp.SelectionBackgroundColor = new Color(0x00, 0x78, 0xD4, 0xFF);
            inp.Padding                  = 3;
            inp.FontFamily               = ["Inter"];
            inp.FontSize                 = 15;
            inp.FontWeight               = FontWeight.Normal;
            inp.SelectAllOnFocus         = true;

            // Dropdown arrow button — no margin, right corners rounded to match border
            Layer.Border.Child.Child2.Child = new ButtonLayer
            {
                Label         = "▾",
                Margin        = 0,
                CornerRadius  = new CornerRadius(0, 4, 4, 0),
                NormalColor   = new Color(0xF0, 0xF0, 0xF0, 0xFF),
                HoverColor    = new Color(0xD8, 0xD8, 0xD8, 0xFF),
                PressedColor  = new Color(0xC0, 0xC0, 0xC0, 0xFF),
                LabelColor    = new Color(0x44, 0x44, 0x44, 0xFF),
                FontSize      = 12,
                Visible       = true,
                RequestRedraw = InvalidateRender,
                OnClick       = () => { /* open dropdown */ },
            };
        }
    }

    // ── Widget 3: numeric stepper — "−" | text | "+" ──

    private class NumericInput
        : LayerView<FocusBorderLayer<DockLayer.Dock3<View, ButtonLayer, TextInputLayer, ButtonLayer>>>
    {
        public override bool Focusable => true;

        public NumericInput()
        {
            // Outer border
            Layer.BackgroundColor    = White;
            Layer.BorderThickness    = 1;
            Layer.BorderColor        = new Color(0xCC, 0xCC, 0xCC, 0xFF);
            Layer.FocusedBorderColor = new Color(0x00, 0x78, 0xD4, 0xFF);
            Layer.CornerRadius       = new CornerRadius(5);
            Layer.Padding            = 0;

            // Dock alignment: Left button | Stretch text | Right button
            Layer.Border.Child.Child1.Align = DockLayer.Align.Left;
            Layer.Border.Child.Child2.Align = DockLayer.Align.Stretch;
            Layer.Border.Child.Child3.Align = DockLayer.Align.Right;

            // "−" button (left)
            Layer.Border.Child.Child1.Child = new ButtonLayer
            {
                Label         = "−",
                Margin        = 0,
                CornerRadius  = new CornerRadius(4, 0, 0, 4),
                NormalColor   = new Color(0xF0, 0xF0, 0xF0, 0xFF),
                HoverColor    = new Color(0xD8, 0xD8, 0xD8, 0xFF),
                PressedColor  = new Color(0xC0, 0xC0, 0xC0, 0xFF),
                LabelColor    = new Color(0x22, 0x22, 0x22, 0xFF),
                FontSize      = 16,
                Visible       = true,
                RequestRedraw = InvalidateRender,
                OnClick       = () =>
                {
                    var t = Layer.Border.Child.Child2.Child.Text;
                    if (int.TryParse(t, out int v)) Layer.Border.Child.Child2.Child.Text = (v - 1).ToString();
                    InvalidateRender();
                },
            };

            // Text layer (center, stretch)
            ref var inp = ref Layer.Border.Child.Child2.Child;
            inp.Color                    = Black;
            inp.SelectedColor            = White;
            inp.SelectionBackgroundColor = new Color(0x00, 0x78, 0xD4, 0xFF);
            inp.Padding                  = 3;
            inp.FontFamily               = ["Inter"];
            inp.FontSize                 = 15;
            inp.FontWeight               = FontWeight.Normal;
            inp.SelectAllOnFocus         = true;
            inp.Text                     = "0";

            // "+" button (right)
            Layer.Border.Child.Child3.Child = new ButtonLayer
            {
                Label         = "+",
                Margin        = 0,
                CornerRadius  = new CornerRadius(0, 4, 4, 0),
                NormalColor   = new Color(0xF0, 0xF0, 0xF0, 0xFF),
                HoverColor    = new Color(0xD8, 0xD8, 0xD8, 0xFF),
                PressedColor  = new Color(0xC0, 0xC0, 0xC0, 0xFF),
                LabelColor    = new Color(0x22, 0x22, 0x22, 0xFF),
                FontSize      = 16,
                Visible       = true,
                RequestRedraw = InvalidateRender,
                OnClick       = () =>
                {
                    var t = Layer.Border.Child.Child2.Child.Text;
                    if (int.TryParse(t, out int v)) Layer.Border.Child.Child2.Child.Text = (v + 1).ToString();
                    InvalidateRender();
                },
            };
        }
    }
}
