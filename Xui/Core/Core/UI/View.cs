using Xui.Core.Canvas;
using Xui.Core.Math2D;

namespace Xui.Core.UI;

/// <summary>
/// Base class for all UI elements in the Xui layout engine.
/// A view participates in layout, rendering, and input hit testing, and may contain child views.
/// </summary>
public abstract class View
{
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
    public Frame Margin { get; set; } = (0, 0);

    /// <summary>
    /// The horizontal alignment of this view inside its layout anchor region.
    /// Used during layout when the view has remaining space within its container.
    /// </summary>
    public HorizontalAlignment HorizontalAlignment { get; set; } = HorizontalAlignment.Stretch;

    /// <summary>
    /// The vertical alignment of this view inside its layout anchor region.
    /// Used during layout when the view has remaining space within its container.
    /// </summary>
    public VerticalAlignment VerticalAlignment { get; set; } = VerticalAlignment.Stretch;

    /// <summary>
    /// The writing direction of this view, which determines the block or inline flow direction.
    /// Inherited from the parent flow context if set to <see cref="Direction.Inherit"/>.
    /// </summary>
    public Direction Direction { get; set; } = Direction.Inherit;

    /// <summary>
    /// The writing mode of this view (e.g. horizontal top-to-bottom or vertical right-to-left).
    /// Inherited from the parent if set to <see cref="WritingMode.Inherit"/>.
    /// </summary>
    public WritingMode WritingMode { get; set; } = WritingMode.HorizontalTB;

    /// <summary>
    /// Controls how the layout system treats this view's children.
    /// Can be inherited or explicitly overridden for advanced layout containers.
    /// </summary>
    public Flow Flow { get; set; } = Flow.Aware;

    /// <summary>
    /// The minimum width of the border edge box.
    /// </summary>
    public nfloat MinimumWidth { get; set; } = 0;

    /// <summary>
    /// The minimum height of the border edge box.
    /// </summary>
    public nfloat MinimumHeight { get; set; } = 0;

    /// <summary>
    /// The maximum width of the border edge box.
    /// </summary>
    public nfloat MaximumWidth { get; set; } = nfloat.PositiveInfinity;

    /// <summary>
    /// The maximum height of the border edge box.
    /// </summary>
    public nfloat MaximumHeight { get; set; } = nfloat.PositiveInfinity;

    /// <summary>
    /// Returns the number of child views. Used by layout containers and traversal logic.
    /// Leaf views should return 0.
    /// </summary>
    public virtual int Count { get; } = 0;

    /// <summary>
    /// Indexer to access child views by index.
    /// Layout containers should implement this to expose their children.
    /// </summary>
    public virtual View this[int index] { get => throw new IndexOutOfRangeException(); }

    /// <summary>
    /// Determines whether the given point (in local coordinates) hits this view’s visual bounds.
    /// Used for input dispatch and hit testing.
    /// </summary>
    /// <param name="point">The point to test, relative to this view’s coordinate space.</param>
    /// <returns><c>true</c> if the point is inside the view’s frame; otherwise <c>false</c>.</returns>
    public virtual bool HitTest(Point point) => false;

    /// <summary>
    /// Performs a full layout pass for a view - measure, arrange and render.
    /// 
    /// Flags can limit to a subset of the layout passes, in case a container needs to measure children multiple times,
    /// or in case a container can rush forward without forking the layout pass into multiple sub-passes.
    /// 
    /// The layout method will delegate parts of the execution to <see cref="MeasureCore"/>, <see cref="ArrangeCore"/> and <see cref="RenderCore"/>.
    /// 
    /// If a container needs to call multiple times methods for a child,
    /// either call the <see cref="Measure"/>, <see cref="Arrange"/> and <see cref="Render"/>,
    /// or construct a <see cref="LayoutGuide"/> with the specific flags and pass it to <see cref="Update"/>.
    /// 
    /// Some containers may override and implement a Layout in a way, that it compacts the flow and avoids fork,
    /// like a VerticalStack that is placed on fullscreen (with fixed width),
    /// can arrange children top to bottom calling their Layout directly - eventually going foreach-layout without splitting into foreach-measure, foreach-arrange cycles.
    /// VerticalStack however, when centered, while it can layout children vertically in a single pass, it can't render, because it needs its height to figure out its position,
    /// so in these cases it may foreach-layout (measure and arrange) resolve the stack Y position and then foreach-layout (for render).
    /// </summary>
    /// <param name="guide"></param>
    /// <returns></returns>
    public virtual LayoutGuide Update(LayoutGuide guide)
    {
        if (guide.IsMeasure)
        {
            Size availableMarginBoxSize = Size.Max((0, 0), guide.AvailableSize);

            Size desiredBorderEdgeBoxSize;

            bool fixedWidth =
                guide.XSize == LayoutGuide.SizeTo.Exact &&
                nfloat.IsFinite(guide.AvailableSize.Width) &&
                this.HorizontalAlignment == HorizontalAlignment.Stretch;

            bool fixedHeight = 
                guide.YSize == LayoutGuide.SizeTo.Exact &&
                nfloat.IsFinite(guide.AvailableSize.Height) &&
                this.VerticalAlignment == VerticalAlignment.Stretch;

            if (fixedWidth && fixedHeight)
            {
                desiredBorderEdgeBoxSize = availableMarginBoxSize - this.Margin;
            }
            else
            {
                desiredBorderEdgeBoxSize = this.MeasureCore(availableMarginBoxSize - this.Margin, guide.MeasureContext!);
                if (fixedWidth)
                {
                    desiredBorderEdgeBoxSize.Width = guide.AvailableSize.Width;
                }
                if (fixedHeight)
                {
                    desiredBorderEdgeBoxSize.Height = guide.AvailableSize.Height;
                }
            }
            desiredBorderEdgeBoxSize = (
                nfloat.Clamp(desiredBorderEdgeBoxSize.Width, this.MinimumWidth, this.MaximumWidth),
                nfloat.Clamp(desiredBorderEdgeBoxSize.Height, this.MinimumHeight, this.MaximumHeight)
            );

            guide.DesiredSize = desiredBorderEdgeBoxSize + this.Margin;
        }

        if (guide.IsArrange)
        {
            nfloat x = guide.Anchor.X - guide.DesiredSize.Width * (int)guide.XAlign * (nfloat).5;
            nfloat y = guide.Anchor.Y - guide.DesiredSize.Height * (int)guide.XAlign * (nfloat).5;
            nfloat width = guide.DesiredSize.Width;
            nfloat height = guide.DesiredSize.Height;

            if (guide.XSize == LayoutGuide.SizeTo.Exact &&
                this.HorizontalAlignment != HorizontalAlignment.Stretch &&
                nfloat.IsFinite(guide.AvailableSize.Width))
            {
                nfloat xViewAlignment = ((int)this.HorizontalAlignment - 1 - (int)guide.XAlign) * (nfloat).5;
                nfloat remainingRealignmentSpace = guide.AvailableSize.Width - guide.DesiredSize.Width;
                x += remainingRealignmentSpace * xViewAlignment;
            }

            if (guide.YSize == LayoutGuide.SizeTo.Exact &&
                this.VerticalAlignment != VerticalAlignment.Stretch &&
                nfloat.IsFinite(guide.AvailableSize.Height))
            {
                nfloat yViewAlignment = ((int)this.VerticalAlignment - 1 - (int)guide.YAlign) * (nfloat).5;
                nfloat remainingRealignmentSpace = guide.AvailableSize.Height - guide.DesiredSize.Height;
                y += remainingRealignmentSpace * yViewAlignment;
            }

            guide.ArrangedRect = new Rect(x, y, width, height) - this.Margin;
            this.Frame = guide.ArrangedRect;
            this.ArrangeCore(guide.ArrangedRect, guide.MeasureContext!);
        }

        if (guide.IsRender)
        {
            // Emit render commands that would draw this view within guide.ArrangeRect
            // The context is required within measure and arrange phases to measure text
            // And to limit layout forks, it can be used to emit the display list commands.
            this.RenderCore(guide.RenderContext!);
        }

        return guide;
    }

    /// <summary>
    /// Measures the view using the specified available size, returning the desired size
    /// calculated during the layout pass.
    /// </summary>
    /// <param name="availableSize">The maximum space available for the view to occupy.</param>
    /// <param name="context"></param>
    /// <returns>The size that the view desires to occupy within the constraints.</returns>
    public Size Measure(Size availableSize, IMeasureContext context) =>
        this.Update(
            new LayoutGuide()
            {
                Pass = LayoutGuide.LayoutPass.Measure,
                AvailableSize = availableSize,
                XSize = LayoutGuide.SizeTo.AtMost,
                YSize = LayoutGuide.SizeTo.AtMost,
                MeasureContext = context
            }).DesiredSize;

    /// <summary>
    /// Arranges the view within the specified rectangle, finalizing its layout position and size.
    /// </summary>
    /// <param name="rect">The rectangle defining the position and exact size for the view.</param>
    /// <returns>The rectangle occupied by the arranged view.</returns>
    public Rect Arrange(Rect rect, IMeasureContext context) =>
        this.Update(
            new LayoutGuide()
            {
                Pass = LayoutGuide.LayoutPass.Arrange,
                DesiredSize = rect.Size,
                XSize = LayoutGuide.SizeTo.Exact,
                YSize = LayoutGuide.SizeTo.Exact,
                MeasureContext = context,
                Anchor = rect.TopLeft
            }
        ).ArrangedRect;

    /// <summary>
    /// Renders the view using the given rendering context. This should be called after layout is complete.
    /// </summary>
    /// <param name="context">The rendering context used to draw the view.</param>
    public void Render(IContext context) =>
        this.Update(
            new LayoutGuide()
            {
                Pass = LayoutGuide.LayoutPass.Render,
                MeasureContext = context,
                RenderContext = context
            }
        );

    /// <summary>
    /// Determines the minimum size that this view's border edge box requires,
    /// given the maximum available size. Margin is not part of this size.
    /// </summary>
    /// <param name="availableBorderEdgeSize">
    /// The maximum size available for the view’s border edge box. 
    /// This size excludes margins, which are handled by the parent layout.
    /// </param>
    /// <param name="context">
    /// The layout metrics context providing access to platform-specific measurements,
    /// text sizing, and pixel snapping utilities.
    /// </param>
    /// <returns>
    /// The desired size of the border edge box based on content and layout logic.
    /// </returns>
    protected virtual Size MeasureCore(Size availableBorderEdgeSize, IMeasureContext context)
    {
        // Measure content or each child...
        return (0, 0);
    }

    /// <summary>
    /// Performs the layout pass by arranging content and children within the view's
    /// border edge box, using the provided rectangle.
    /// </summary>
    /// <param name="rect">
    /// The final rectangle (position and size) allocated to this view's border edge box.
    /// </param>
    /// <param name="context">
    /// The layout metrics context providing access to platform-specific measurements,
    /// text sizing, and pixel snapping utilities.
    /// </param>
    protected virtual void ArrangeCore(Rect rect, IMeasureContext context)
    {
        // Arrange each child...
    }

    /// <summary>
    /// Renders the content and children of this view using the provided rendering context.
    /// </summary>
    /// <param name="context">
    /// The drawing context used for rendering visual content to the output surface.
    /// </param>
    protected virtual void RenderCore(IContext context)
    {
        // Render each child...
    }
}
