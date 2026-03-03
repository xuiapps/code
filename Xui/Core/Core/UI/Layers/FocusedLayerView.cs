namespace Xui.Core.UI.Layers;

/// <summary>
/// A <see cref="LayerView{T}"/> that owns focus and drives a blinking caret inside
/// its layer tree. Subclasses implement <see cref="GetCaretRef"/> to return a
/// reference to whichever <c>CaretVisible</c> field the layer tree exposes.
/// </summary>
/// <typeparam name="T">Root layer struct type.</typeparam>
public abstract class FocusedLayerView<T> : LayerView<T>
    where T : struct, ILayer
{
    private static readonly TimeSpan CaretBlinkInterval = TimeSpan.FromMilliseconds(530);

    private TimeSpan caretToggleTime;

    public override bool Focusable => true;

    /// <summary>
    /// Returns a managed reference to the <c>CaretVisible</c> field in the layer tree
    /// that should be toggled by the blink animation.
    /// </summary>
    protected abstract ref bool GetCaretRef();

    /// <summary>
    /// Resets the blink timer and immediately shows the caret.
    /// Call after any user action that should restart the blink cycle.
    /// </summary>
    protected void ResetCaretBlink()
    {
        GetCaretRef() = true;
        caretToggleTime = TimeSpan.Zero;
    }

    protected override void AnimateCore(TimeSpan previousTime, TimeSpan currentTime)
    {
        if (!IsFocused)
        {
            base.AnimateCore(previousTime, currentTime);
            return;
        }

        if (caretToggleTime == TimeSpan.Zero)
            caretToggleTime = currentTime + CaretBlinkInterval;

        if (currentTime >= caretToggleTime)
        {
            GetCaretRef() = !GetCaretRef();
            caretToggleTime = currentTime + CaretBlinkInterval;
            InvalidateRender();
        }

        RequestAnimationFrame();
        base.AnimateCore(previousTime, currentTime);
    }
}
