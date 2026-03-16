namespace Xui.Core.UI.Layout;

/// <summary>
/// A container view that arranges children along a main axis with flexible sizing and wrapping.
/// Container-level properties mirror the CSS Flex Box spec.
/// </summary>
public partial class FlexBox : ViewCollection
{
    /// <summary>
    /// Sets the main axis direction along which flex items are placed.
    /// Corresponds to <c>flex-direction</c>. Default is <see cref="Direction.Row"/>.
    /// </summary>
    public Direction FlexDirection { get; set; } = Direction.Row;

    /// <summary>
    /// Controls whether flex items wrap onto multiple lines.
    /// Corresponds to <c>flex-wrap</c>. Default is <see cref="Wrap.NoWrap"/>.
    /// </summary>
    public Wrap FlexWrap { get; set; } = Wrap.NoWrap;

    /// <summary>
    /// Aligns flex items along the main axis when there is free space.
    /// Corresponds to <c>justify-content</c>. Default is <see cref="JustifyContent.FlexStart"/>.
    /// </summary>
    public JustifyContent FlexJustifyContent { get; set; } = JustifyContent.FlexStart;

    /// <summary>
    /// Sets the default cross-axis alignment for all flex items on a line.
    /// Corresponds to <c>align-items</c>. Default is <see cref="AlignItems.Stretch"/>.
    /// </summary>
    public AlignItems FlexAlignItems { get; set; } = AlignItems.Stretch;

    /// <summary>
    /// Aligns flex lines along the cross axis when there is extra space (only applies when wrapping).
    /// Corresponds to <c>align-content</c>. Default is <see cref="AlignContent.Normal"/>.
    /// </summary>
    public AlignContent FlexAlignContent { get; set; } = AlignContent.Normal;

    /// <summary>
    /// Space between rows of flex items.
    /// Corresponds to <c>row-gap</c>. Default is <c>0</c>.
    /// </summary>
    public nfloat RowGap { get; set; } = 0;

    /// <summary>
    /// Space between columns of flex items.
    /// Corresponds to <c>column-gap</c>. Default is <c>0</c>.
    /// </summary>
    public nfloat ColumnGap { get; set; } = 0;
}
