using Xui.Core.Math2D;

namespace Xui.Core.UI;

public partial class View
{
    /// <summary>
    /// Gets this view's conservative bounds in its parent coordinate space for render culling.
    /// </summary>
    /// <remarks>
    /// A container may override this when its rendered geometry differs from its layout frame.
    /// Computing the union of descendant culling frames is deliberately deferred: it can be
    /// more expensive than rendering for small subtrees.
    /// </remarks>
    public virtual Rect CullingFrame => this.Frame;

    /// <summary>
    /// Maps a point from this view's parent coordinate space into this view's local coordinate space.
    /// </summary>
    /// <remarks>
    /// Layout still supplies frames using the existing coordinate model. The default identity
    /// mapping deliberately preserves that behaviour until a container opts into a local
    /// coordinate transform. Containers such as <see cref="ScrollView"/> can override this
    /// to map input into their content space.
    /// </remarks>
    public virtual Point TransformPoint(Point point) => point;

    /// <summary>
    /// Maps a point from this view's local coordinate space into its parent coordinate space.
    /// </summary>
    /// <remarks>
    /// This must be the inverse of <see cref="TransformPoint"/> for points that can be hit
    /// tested. It is separate so a view can support coordinate mappings that are not yet
    /// represented by the canvas transform API.
    /// </remarks>
    public virtual Point InverseTransformPoint(Point point) => point;

    /// <summary>
    /// Conservatively maps an axis-aligned rectangle from this view's parent coordinate space
    /// into this view's coordinate space.
    /// </summary>
    /// <remarks>
    /// For non-translation transforms, an override must return the smallest parent-axis-aligned
    /// rectangle that encloses every transformed corner.
    /// </remarks>
    public virtual Rect TransformRect(Rect rect) => rect;

    /// <summary>
    /// Conservatively maps an axis-aligned rectangle from this view's coordinate space into its
    /// parent coordinate space.
    /// </summary>
    /// <remarks>
    /// For non-translation transforms, an override must return the smallest parent-axis-aligned
    /// rectangle that encloses every transformed corner.
    /// </remarks>
    public virtual Rect InverseTransformRect(Rect rect) => rect;

    /// <summary>
    /// Maps a point in this view's local coordinate space into root/window coordinate space.
    /// </summary>
    public Point LocalToGlobal(Point point)
    {
        for (View? view = this; view is not null; view = view.Parent)
            point = view.InverseTransformPoint(point);

        return point;
    }

    /// <summary>
    /// Maps a point in root/window coordinate space into this view's local coordinate space.
    /// </summary>
    public Point GlobalToLocal(Point point)
    {
        if (this.Parent is not null)
            point = this.Parent.GlobalToLocal(point);

        return this.TransformPoint(point);
    }
}
