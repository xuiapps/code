---
title: View — Layers
description: ILayer structs — composable, zero-allocation building blocks for small input widgets.
---

# View — Layers

Small input widgets — text fields with clear buttons, numeric steppers, combo boxes, color pickers — share a lot of drawing and input logic, yet they are too specialised to fit cleanly into a general `View` subclass hierarchy. Creating a full heap-allocated `View` per visual element leads to large object graphs, heavyweight attachment/detachment lifecycles, and virtual-dispatch overhead on every frame.

**Layers** solve this. A layer is a plain C# `struct` that owns one concern of a widget: drawing a border, handling text input, routing a button click. Layers compose at the type level — `FocusBorderLayer<TView, TextInputLayer>` is a single generic struct, resolved statically by the compiler, with all delegation devirtualised and inlined by the JIT. No heap allocation, no virtual dispatch, no view tree entries beyond the single host view.

---

## ILayer\<TView\>

```csharp
public interface ILayer<in TView> where TView : ILayerHost
{
    void Update(TView view, ref LayoutGuide guide);

    void Animate(TView view, TimeSpan previous, TimeSpan current);
    Size Measure(TView view, Size available, IMeasureContext ctx);
    void Arrange(TView view, Rect rect, IMeasureContext ctx);
    void Render(TView view, IContext ctx);

    void OnPointerEvent(TView view, ref PointerEventRef e, EventPhase phase);
    void OnKeyDown(TView view, ref KeyEventRef e);
    void OnChar(TView view, ref KeyEventRef e);
    void OnFocus(TView view);
    void OnBlur(TView view);
}
```

Each method mirrors the corresponding `View` override, except every method receives the host view as an explicit first parameter — the layer carries its own state but calls back to the host for invalidation, focus, and service resolution.

`Update` is the single-pass entry point (see [Layout — LuminarFlow](layout.md)). The default implementation in each layer dispatches to the four individual methods based on the guide's flags. Override `Update` only when a layer can resolve all four stages in one traversal.

---

## ILayerHost

`ILayerHost` is the contract a host view must satisfy. `View` already implements it.

```csharp
public interface ILayerHost : IServiceProvider
{
    Rect Frame { get; }
    bool IsFocused { get; }
    bool Focus();
    void InvalidateRender();
    void InvalidateMeasure();
    void RequestAnimationFrame();
    void CapturePointer(int pointerId);
    void ReleasePointer(int pointerId);
}
```

`ILayerHost` extends `IServiceProvider`, giving layers access to the full [service resolution chain](di.md) — text measure context, bitmap factory, platform drawing context — without importing any concrete type.

---

## LayerView\<TView, TLayer\>

`LayerView<TView, TLayer>` is a `View` subclass that owns a single layer struct and forwards every virtual override to it:

```csharp
public class LayerView<TView, TLayer> : View
    where TView  : View
    where TLayer : struct, ILayer<TView>
{
    protected TLayer Layer;
    private TView Self => (TView)(object)this;

    protected override Size MeasureCore(Size available, IMeasureContext ctx)
        => Layer.Measure(Self, available, ctx);

    protected override void ArrangeCore(Rect rect, IMeasureContext ctx)
        => Layer.Arrange(Self, rect, ctx);

    protected override void RenderCore(IContext ctx)
        => Layer.Render(Self, ctx);

    // ... Animate, OnPointerEvent, OnKeyDown, OnChar, OnFocus, OnBlur
}
```

Because `TLayer` is a static type parameter, the JIT resolves every `Layer.Measure(...)` call at compile/JIT time and inlines the struct method body directly. The `Self` double-cast (`(TView)(object)this`) is a standard C# generic trick that compiles to a single reference check.

Pass `View` as `TView` when the layer does not need access to any host-specific API beyond `ILayerHost`:

```csharp
// No host-specific API needed — use View.
public class TextBox : LayerView<View, FocusBorderLayer<View, TextInputLayer>> { ... }

// Host-specific: ClearableInput.Clear() must be callable from ButtonLayer.
private class ClearableInput
    : LayerView<ClearableInput,
        FocusBorderLayer<ClearableInput,
            DockLayer.Dock2<ClearableInput, TextInputLayer, ButtonLayer<ClearableInput, ClearAction>>>>
{ ... }
```

---

## Composing layers

Layers compose as nested generics. The outer layer wraps the inner; all types are resolved at compile time:

```
FocusBorderLayer<TView, TChild>
  └── BorderLayer<TView, TChild>
        └── TChild  (e.g. TextInputLayer, DockLayer.Dock2<...>, ...)
```

### BorderLayer

Draws a background fill, border stroke, and padding, then delegates to its child:

```csharp
public struct BorderLayer<TView, TChild> : ILayer<TView>
{
    public TChild     Child;
    public Frame      BorderThickness;
    public Frame      Padding;
    public CornerRadius CornerRadius;
    public Color      BackgroundColor;
    public Color      BorderColor;
}
```

### FocusBorderLayer

Wraps `BorderLayer` and swaps the border color when the host has focus:

```csharp
public struct FocusBorderLayer<TView, TChild> : ILayer<TView>
{
    public BorderLayer<TView, TChild> Border;
    public Color FocusedBorderColor;

    // Forwarding properties: BorderThickness, Padding, CornerRadius, ...
    // (delegate to Border.*)
}
```

`OnFocus` / `OnBlur` both call `view.InvalidateRender()` so the color change is immediate.

### TextInputLayer

A leaf layer implementing full single-line text input: selection, caret blink, password masking, cursor positioning, and character filtering. It holds a `StringBuilder` for the text buffer and stores all visual state (caret position, selection range, scroll offset) as struct fields.

```csharp
public struct TextInputLayer : ILayer<View>
{
    public string  Text         { get; set; }   // get/set on StringBuilder
    public bool    IsPassword;
    public Func<char, bool>? InputFilter;       // null = accept all
    public bool    SelectAllOnFocus;

    public string[]?  FontFamily;
    public NFloat     FontSize;
    public FontWeight FontWeight;
    public FontStretch FontStretch;
    public FontStyle   FontStyle;

    public Color Color;
    public Color SelectedColor;
    public Color SelectionBackgroundColor;
    public Frame Padding;
}
```

`TextInputLayer` is the canonical reusable input primitive. `TextBox` wraps it in `FocusBorderLayer`; `DockLayer`-based widgets embed it inline.

---

## DockLayer — layout composition

`DockLayer` contains Dock2, Dock3, and Dock4 variants that arrange child layers in a horizontal strip, each slot tagged as `Left`, `Right`, or `Stretch`:

```csharp
public static class DockLayer
{
    public enum Align { Left, Stretch, Right }

    public struct Dock2<TView, T1, T2> : ILayer<TView> { ... }
    public struct Dock3<TView, T1, T2, T3> : ILayer<TView> { ... }
    public struct Dock4<TView, T1, T2, T3, T4> : ILayer<TView> { ... }
}
```

Assign alignment before layout:

```csharp
// Numeric stepper: [−] | text | [+]
Layer.Border.Child.Child1.Align = DockLayer.Align.Left;     // − button
Layer.Border.Child.Child2.Align = DockLayer.Align.Stretch;  // text input
Layer.Border.Child.Child3.Align = DockLayer.Align.Right;    // + button
```

Fixed-size slots (`Left` / `Right`) are measured first; the `Stretch` slot receives the remaining width.

**Keyboard events fan out to all children.** DockLayer has no concept of a focused child — all children receive `OnKeyDown` and `OnChar`. This is correct when only one child handles keyboard input (e.g. `ButtonLayer` no-ops both). If two children both handle keyboard input, both will receive every key event; design the widget accordingly.

---

## IButtonAction — zero-allocation callbacks

Rather than a delegate (which would allocate a closure), button actions are implemented as a nested struct:

```csharp
public interface IButtonAction<in THost> where THost : ILayerHost
{
    void Execute(THost host);
}
```

Declare the action as a private nested struct inside the owning view. `Execute` receives the fully-typed host, so it can call any view method without boxing or capturing:

```csharp
private class ClearableInput : LayerView<ClearableInput, ...>
{
    internal void Clear() { /* ... */ }

    internal struct ClearAction : IButtonAction<ClearableInput>
    {
        public void Execute(ClearableInput host) => host.Clear();
    }
}
```

Assign the action to the button layer at construction time:

```csharp
Layer.Border.Child.Child2.Child = new ButtonLayer<ClearableInput, ClearAction>
{
    Label = "×",
    Visible = false,
    // ...
};
```

---

## ContentLayer — bridging back to View

When a layer tree needs to embed a child `View` (rather than another layer struct), use `ContentLayer`:

```csharp
public struct ContentLayer : ILayer<View>
{
    public View? Child;
}
```

`ContentLayer` measures, arranges, and renders the child view through the normal `View` methods. Input is not forwarded — the event router hit-tests the child view directly.

`LayerView` is a leaf by default (`Count = 0`). To host child views, override `Count` and the indexer, manage attachment via `AddProtectedChild` / `RemoveProtectedChild`, and embed a `ContentLayer` in the layer tree to delegate layout to those children.

---

## Putting it together: a clearable text input

```csharp
private class ClearableInput
    : LayerView<ClearableInput,
        FocusBorderLayer<ClearableInput,
            DockLayer.Dock2<ClearableInput,
                TextInputLayer,
                ButtonLayer<ClearableInput, ClearableInput.ClearAction>>>>
{
    public override bool Focusable => true;

    public ClearableInput()
    {
        // Border
        Layer.BackgroundColor    = White;
        Layer.BorderColor        = new Color(0xCC, 0xCC, 0xCC, 0xFF);
        Layer.FocusedBorderColor = new Color(0x00, 0x78, 0xD4, 0xFF);
        Layer.BorderThickness    = 1;
        Layer.CornerRadius       = new CornerRadius(5);

        // Dock: text stretches, button anchors right
        Layer.Border.Child.Child1.Align = DockLayer.Align.Stretch;
        Layer.Border.Child.Child2.Align = DockLayer.Align.Right;

        // Text input
        ref var inp = ref Layer.Border.Child.Child1.Child;
        inp.FontFamily = ["Inter"];
        inp.FontSize   = 15;
        inp.FontWeight = FontWeight.Normal;
        inp.Padding    = 3;
        inp.SelectAllOnFocus = true;

        // Clear button — initially hidden (zero width when Visible=false)
        Layer.Border.Child.Child2.Child = new ButtonLayer<ClearableInput, ClearAction>
        {
            Label   = "×",
            Visible = false,
        };
    }

    internal void Clear()
    {
        Layer.Border.Child.Child1.Child.Text    = "";
        Layer.Border.Child.Child2.Child.Visible = false;
        InvalidateMeasure();
    }

    public override void OnChar(ref KeyEventRef e)
    {
        base.OnChar(ref e);
        SyncClearButton();
    }

    private void SyncClearButton()
    {
        bool hasText = Layer.Border.Child.Child1.Child.Text.Length > 0;
        if (Layer.Border.Child.Child2.Child.Visible != hasText)
        {
            Layer.Border.Child.Child2.Child.Visible = hasText;
            InvalidateMeasure();
        }
    }

    internal struct ClearAction : IButtonAction<ClearableInput>
    {
        public void Execute(ClearableInput host) => host.Clear();
    }
}
```

The entire widget is one heap object (`ClearableInput`), one `StringBuilder`, and a flat struct holding all visual and input state. No child view instances, no event delegates.

---

## When to use layers vs View subclasses

| Use layers when… | Use View subclasses when… |
|---|---|
| The widget is a self-contained input control (text field, button, stepper, picker) | The widget manages a list of dynamic child views |
| Drawing and input handling are tightly coupled and shared across variants | The widget needs the full lifecycle (`OnAttach`, `OnActivate`, …) |
| You want zero allocation in the layout/render hot path | The widget is a container that participates in hit testing as a subtree |
| The widget's children are fixed at compile time | Children are added/removed at runtime |

Layers and views interoperate freely: a `LayerView` is a first-class `View`, participates in the parent chain, and is resolved by the service and focus systems exactly like any other view.
