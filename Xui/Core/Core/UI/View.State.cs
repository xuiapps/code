namespace Xui.Core.UI;

public partial class View
{
    /// <summary>
    /// Flags used internally by a <see cref="View"/> to track invalidation state across
    /// measure, layout, and rendering phases. These flags allow the framework to selectively
    /// recompute only dirty subtrees during recursive passes.
    /// </summary>
    [Flags]
    private enum ViewFlags
    {
    }

    private ViewFlags flags;

    protected void InvalidateRender()
    {
        this.Parent?.OnChildInvalidRender(this);
    }

    protected virtual void OnChildInvalidRender(View child)
    {
        this.Parent?.OnChildInvalidRender(this);
    }
    
    public void CapturePointer(int pointerId)
    {
        if (this.TryFindParent<RootView>(out var rootView))
        {
            rootView.EventRouter.CapturePointer(this, pointerId);
        }
    }

    public void ReleasePointer(int pointerId)
    {
        if (this.TryFindParent<RootView>(out var rootView))
        {
            rootView.EventRouter.ReleasePointer(this, pointerId);
        }
    }
}
