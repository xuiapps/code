using Xui.Core.Math2D;

namespace Xui.Core.UI;

/// <summary>
/// Base class for all UI elements in the Xui layout engine.
/// A view participates in layout, rendering, and input hit testing, and may contain child views.
/// </summary>
public partial class View
{
    /// <summary>
    /// An optional unique identifier for this view, used for lookup via
    /// <see cref="ViewExtensions.FindViewById"/>.
    /// </summary>
    public string? Id { get; set; }

    /// <summary>
    /// The set of class names assigned to this view, used for lookup via
    /// <see cref="ViewExtensions.FindViewsByClass"/>.
    /// </summary>
    public ClassNameCollection ClassName;

    /// <summary>
    /// The parent view in the visual hierarchy. This is set automatically when the view is added to a container.
    /// </summary>
    public View? Parent { get; internal set; }

    /// <summary>
    /// The border edge of this view in global coordinates relative to the top-left of the window.
    /// </summary>
    public Rect Frame { get; protected set; }

    /// <summary>
    /// The margin around this view. Margins participate in collapsed margin logic during layout,
    /// and are external spacing relative to the parent or surrounding siblings.
    /// </summary>
    private Frame margin = (0, 0);
    /// <summary>Gets or sets the external spacing around the view.</summary>
    public Frame Margin
    {
        get => margin;
        set
        {
            if (margin == value) return;
            margin = value;
            this.Invalidate();
        }
    }

    /// <summary>
    /// The horizontal alignment of this view inside its layout anchor region.
    /// Used during layout when the view has remaining space within its container.
    /// </summary>
    private HorizontalAlignment horizontalAlignment = HorizontalAlignment.Stretch;
    /// <summary>Gets or sets the horizontal alignment within the layout anchor region.</summary>
    public HorizontalAlignment HorizontalAlignment
    {
        get => horizontalAlignment;
        set
        {
            if (horizontalAlignment == value) return;
            horizontalAlignment = value;
            this.Invalidate();
        }
    }

    /// <summary>
    /// The vertical alignment of this view inside its layout anchor region.
    /// Used during layout when the view has remaining space within its container.
    /// </summary>
    private VerticalAlignment verticalAlignment = VerticalAlignment.Stretch;
    /// <summary>Gets or sets the vertical alignment within the layout anchor region.</summary>
    public VerticalAlignment VerticalAlignment
    {
        get => verticalAlignment;
        set
        {
            if (verticalAlignment == value) return;
            verticalAlignment = value;
            this.Invalidate();
        }
    }

    /// <summary>
    /// The writing direction of this view, which determines the block or inline flow direction.
    /// Inherited from the parent flow context if set to <see cref="Direction.Inherit"/>.
    /// </summary>
    private Direction direction = Direction.Inherit;
    /// <summary>Gets or sets the writing direction.</summary>
    public Direction Direction
    {
        get => direction;
        set
        {
            if (direction == value) return;
            direction = value;
            this.Invalidate();
        }
    }

    /// <summary>
    /// The writing mode of this view (e.g. horizontal top-to-bottom or vertical right-to-left).
    /// Inherited from the parent if set to <see cref="WritingMode.Inherit"/>.
    /// </summary>
    private WritingMode writingMode = WritingMode.HorizontalTB;
    /// <summary>Gets or sets the writing mode.</summary>
    public WritingMode WritingMode
    {
        get => writingMode;
        set
        {
            if (writingMode == value) return;
            writingMode = value;
            this.Invalidate();
        }
    }

    /// <summary>
    /// Controls how the layout system treats this view's children.
    /// Can be inherited or explicitly overridden for advanced layout containers.
    /// </summary>
    private Flow flow = Flow.Aware;
    /// <summary>Gets or sets the child flow behavior.</summary>
    public Flow Flow
    {
        get => flow;
        set
        {
            if (flow == value) return;
            flow = value;
            this.Invalidate();
        }
    }

    /// <summary>
    /// The minimum width of the border edge box.
    /// </summary>
    private nfloat minimumWidth = 0;
    /// <summary>Gets or sets the minimum border-edge width.</summary>
    public nfloat MinimumWidth
    {
        get => minimumWidth;
        set
        {
            if (minimumWidth == value) return;
            minimumWidth = value;
            this.Invalidate();
        }
    }

    /// <summary>
    /// The minimum height of the border edge box.
    /// </summary>
    private nfloat minimumHeight = 0;
    /// <summary>Gets or sets the minimum border-edge height.</summary>
    public nfloat MinimumHeight
    {
        get => minimumHeight;
        set
        {
            if (minimumHeight == value) return;
            minimumHeight = value;
            this.Invalidate();
        }
    }

    /// <summary>
    /// The maximum width of the border edge box.
    /// </summary>
    private nfloat maximumWidth = nfloat.PositiveInfinity;
    /// <summary>Gets or sets the maximum border-edge width.</summary>
    public nfloat MaximumWidth
    {
        get => maximumWidth;
        set
        {
            if (maximumWidth == value) return;
            maximumWidth = value;
            this.Invalidate();
        }
    }

    /// <summary>
    /// The maximum height of the border edge box.
    /// </summary>
    private nfloat maximumHeight = nfloat.PositiveInfinity;
    /// <summary>Gets or sets the maximum border-edge height.</summary>
    public nfloat MaximumHeight
    {
        get => maximumHeight;
        set
        {
            if (maximumHeight == value) return;
            maximumHeight = value;
            this.Invalidate();
        }
    }

    /// <summary>
    /// Determines whether the given point (in this view's local coordinates) hits this view's visual bounds.
    /// Used for input dispatch and hit testing.
    /// </summary>
    /// <param name="point">The point to test, relative to this view's coordinate space.</param>
    /// <returns><c>true</c> if the point is inside the view's frame; otherwise <c>false</c>.</returns>
    public virtual bool HitTest(Point point)
    {
        if (!this.Frame.Contains(point))
            return false;

        for (int i = this.Count - 1; i >= 0; i--)
            if (this[i].HitTest(this[i].TransformPoint(point)))
                return true;

        return true;
    }
}
