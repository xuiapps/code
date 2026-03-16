namespace Xui.Core.UI.Layout;

/// <summary>
/// A container view that arranges children in a grid of rows and columns.
/// Container-level properties mirror the CSS Grid spec.
/// </summary>
public partial class Grid : ViewCollection
{
    /// <summary>
    /// Defines the track sizes for explicitly defined columns.
    /// Corresponds to <c>grid-template-columns</c>. Default is an empty array (no explicit columns).
    /// </summary>
    public TrackSize[] TemplateColumns { get; set; } = [];

    /// <summary>
    /// Defines the track sizes for explicitly defined rows.
    /// Corresponds to <c>grid-template-rows</c>. Default is an empty array (no explicit rows).
    /// </summary>
    public TrackSize[] TemplateRows { get; set; } = [];

    /// <summary>
    /// Defines named grid areas. Each string in the array defines one row; space-separated cell names
    /// define columns within that row. A dot (<c>.</c>) denotes an unnamed cell.
    /// Corresponds to <c>grid-template-areas</c>. Default is an empty array.
    /// </summary>
    public string[] TemplateAreas { get; set; } = [];

    /// <summary>
    /// Sizing for implicitly created columns (those outside <see cref="TemplateColumns"/>).
    /// Corresponds to <c>grid-auto-columns</c>. Default is <see cref="TrackSize.Auto"/>.
    /// </summary>
    public TrackSize AutoColumns { get; set; } = TrackSize.Auto;

    /// <summary>
    /// Sizing for implicitly created rows (those outside <see cref="TemplateRows"/>).
    /// Corresponds to <c>grid-auto-rows</c>. Default is <see cref="TrackSize.Auto"/>.
    /// </summary>
    public TrackSize AutoRows { get; set; } = TrackSize.Auto;

    /// <summary>
    /// Controls how auto-placed items flow into the grid.
    /// Corresponds to <c>grid-auto-flow</c>. Default is <see cref="AutoFlow.Row"/>.
    /// </summary>
    public AutoFlow GridAutoFlow { get; set; } = AutoFlow.Row;

    /// <summary>
    /// Sets the default inline-axis alignment for all grid items within their grid areas.
    /// Corresponds to <c>justify-items</c>. Default is <see cref="JustifyItems.Stretch"/>.
    /// </summary>
    public JustifyItems GridJustifyItems { get; set; } = JustifyItems.Stretch;

    /// <summary>
    /// Sets the default block-axis alignment for all grid items within their grid areas.
    /// Corresponds to <c>align-items</c>. Default is <see cref="AlignItems.Stretch"/>.
    /// </summary>
    public AlignItems GridAlignItems { get; set; } = AlignItems.Stretch;

    /// <summary>
    /// Aligns the grid along the inline axis within the container when the grid is smaller than the container.
    /// Corresponds to <c>justify-content</c>. Default is <see cref="JustifyContent.Start"/>.
    /// </summary>
    public JustifyContent GridJustifyContent { get; set; } = JustifyContent.Start;

    /// <summary>
    /// Aligns the grid along the block axis within the container when the grid is smaller than the container.
    /// Corresponds to <c>align-content</c>. Default is <see cref="AlignContent.Start"/>.
    /// </summary>
    public AlignContent GridAlignContent { get; set; } = AlignContent.Start;

    /// <summary>
    /// Space between grid rows.
    /// Corresponds to <c>row-gap</c>. Default is <c>0</c>.
    /// </summary>
    public nfloat RowGap { get; set; } = 0;

    /// <summary>
    /// Space between grid columns.
    /// Corresponds to <c>column-gap</c>. Default is <c>0</c>.
    /// </summary>
    public nfloat ColumnGap { get; set; } = 0;
}
