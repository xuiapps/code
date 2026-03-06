using Xui.Core.Abstract.Events;
using Xui.Core.Canvas;
using Xui.Core.Math2D;
using Xui.Core.UI.Input;

namespace Xui.Core.UI.Layer;

/// <summary>
/// A <see cref="View"/> that owns a single <typeparamref name="TLayer"/> struct and delegates
/// every layout, rendering, and input override to it.
/// <para>
/// Because the compiler resolves <typeparamref name="TLayer"/> statically, all delegation is
/// devirtualised and inlined by the JIT — zero overhead compared to writing the overrides by hand.
/// </para>
/// <para>
/// <b>Child views:</b> <see cref="LayerView{TLayer}"/> is a leaf by default (Count = 0).
/// Subclasses that host child <see cref="View"/> instances should override <see cref="View.Count"/>
/// and <see cref="View.this[int]"/>, and manage attachment via <see cref="View.SetProtectedChild{T}"/>.
/// Use <see cref="ContentLayer"/> to bridge those views back into the layer tree.
/// </para>
/// </summary>
public class LayerView<TLayer> : View
    where TLayer : struct, ILayer<View>
{
    /// <summary>The layer struct that owns all layout and rendering logic for this view.</summary>
    protected TLayer Layer;

    /// <inheritdoc/>
    protected override void AnimateCore(TimeSpan previousTime, TimeSpan currentTime)
        => Layer.Animate(this, previousTime, currentTime);

    /// <inheritdoc/>
    protected override Size MeasureCore(Size availableSize, IMeasureContext context)
        => Layer.Measure(this, availableSize, context);

    /// <inheritdoc/>
    protected override void ArrangeCore(Rect rect, IMeasureContext context)
        => Layer.Arrange(this, rect, context);

    /// <inheritdoc/>
    protected override void RenderCore(IContext context)
        => Layer.Render(this, context);

    /// <inheritdoc/>
    public override void OnPointerEvent(ref PointerEventRef e, EventPhase phase)
        => Layer.OnPointerEvent(this, ref e, phase);

    /// <inheritdoc/>
    public override void OnKeyDown(ref KeyEventRef e)
        => Layer.OnKeyDown(this, ref e);

    /// <inheritdoc/>
    public override void OnChar(ref KeyEventRef e)
        => Layer.OnChar(this, ref e);

    /// <inheritdoc/>
    protected internal override void OnFocus()
        => Layer.OnFocus(this);

    /// <inheritdoc/>
    protected internal override void OnBlur()
        => Layer.OnBlur(this);
}
