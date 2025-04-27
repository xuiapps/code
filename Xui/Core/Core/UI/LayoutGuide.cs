using Xui.Core.Canvas;
using Xui.Core.Math2D;

namespace Xui.Core.UI;

/// <summary>
/// Encapsulates the parameters and results of a layout pass (Measure, Arrange, Render) for a view.
/// </summary>
public struct LayoutGuide
{
    /// <summary>
    /// Indicates the type of layout pass being performed: Measure, Arrange, or Render.
    /// </summary>
    public LayoutPass Pass;

    // Measure spec

    /// <summary>
    /// The available space for measuring this view's margin box. Used during the Measure pass.
    /// </summary>
    public Size AvailableSize;

    /// <summary>
    /// How the view should size itself horizontally during measurement (exact or at-most).
    /// </summary>
    public SizeTo XSize;

    /// <summary>
    /// How the view should size itself vertically during measurement (exact or at-most).
    /// </summary>
    public SizeTo YSize;

    /// <summary>
    /// Optional measurement context providing access to platform-specific text metrics
    /// and precise size calculations during the Measure pass.
    /// If set, text and layout measurements can use font shaping and pixel snapping
    /// consistent with the underlying rendering system.
    /// </summary>
    public IMeasureContext? MeasureContext;

    /// <summary>
    /// The desired size of the view's margin box, produced during the Measure pass.
    /// </summary>
    public Size DesiredSize;

    // Arrange spec

    /// <summary>
    /// The anchor point that defines the alignment constraint for layout.
    /// This point serves as a reference for positioning the view based on alignment.
    /// For example, if alignment is set to <see cref="Align.End"/>, the anchor represents the bottom-right constraint.
    /// If alignment is <see cref="Align.Start"/>, it represents the top-left constraint.
    /// </summary>
    public Point Anchor;

    /// <summary>
    /// The horizontal alignment of the view within its allocated space.
    /// </summary>
    public Align XAlign;

    /// <summary>
    /// The vertical alignment of the view within its allocated space.
    /// </summary>
    public Align YAlign;

    /// <summary>
    /// The final rectangle occupied by the view's border edge box after the Arrange pass.
    /// </summary>
    public Rect ArrangedRect;

    // Render spec

    /// <summary>
    /// Optional rendering context for drawing during the Render pass.
    /// </summary>
    public IContext? RenderContext;

    /// <summary>
    /// Returns true if this guide represents a Measure pass.
    /// </summary>
    public bool IsMeasure => (this.Pass & LayoutPass.Measure) == LayoutPass.Measure;

    /// <summary>
    /// Returns true if this guide represents an Arrange pass.
    /// </summary>
    public bool IsArrange => (this.Pass & LayoutPass.Arrange) == LayoutPass.Arrange;

    /// <summary>
    /// Returns true if this guide represents a Render pass.
    /// </summary>
    public bool IsRender => (this.Pass & LayoutPass.Render) == LayoutPass.Render;

    /// <summary>
    /// Flags indicating which type of layout pass is being performed.
    /// Multiple passes may be combined (e.g., Measure | Render).
    /// </summary>
    [Flags]
    public enum LayoutPass : byte
    {
        /// <summary>
        /// Indicates a Measure pass to determine desired size.
        /// </summary>
        Measure = 1 << 0,

        /// <summary>
        /// Indicates an Arrange pass to finalize layout position and size.
        /// </summary>
        Arrange = 1 << 1,

        /// <summary>
        /// Indicates a Render pass to draw the view's content.
        /// </summary>
        Render = 1 << 2,
    }

    /// <summary>
    /// Defines how the view should interpret the size constraints during measurement.
    /// </summary>
    public enum SizeTo : byte
    {
        /// <summary>
        /// The view must exactly match the given size constraints.
        /// </summary>
        Exact,

        /// <summary>
        /// The view may size to its content, but must not exceed the given constraints.
        /// </summary>
        AtMost
    }

    /// <summary>
    /// Defines alignment of a view within a layout axis.
    /// </summary>
    public enum Align : byte
    {
        /// <summary>
        /// Align to the start (top or left).
        /// </summary>
        Start = 0,

        /// <summary>
        /// Align to the center.
        /// </summary>
        Center = 1,

        /// <summary>
        /// Align to the end (bottom or right).
        /// </summary>
        End = 2
    }
}
