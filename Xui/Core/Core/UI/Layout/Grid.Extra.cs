namespace Xui.Core.UI.Layout;

public partial class Grid
{
    /// <summary>
    /// The grid line where the item's row starts (1-indexed). <c>0</c> means <c>auto</c>.
    /// Corresponds to CSS <c>grid-row-start</c>.
    /// </summary>
    public static readonly View.Extra<nint> RowStart = new(0);

    /// <summary>
    /// The grid line where the item's row ends (1-indexed, exclusive). <c>0</c> means <c>auto</c>.
    /// Corresponds to CSS <c>grid-row-end</c>.
    /// </summary>
    public static readonly View.Extra<nint> RowEnd = new(0);

    /// <summary>
    /// The grid line where the item's column starts (1-indexed). <c>0</c> means <c>auto</c>.
    /// Corresponds to CSS <c>grid-column-start</c>.
    /// </summary>
    public static readonly View.Extra<nint> ColumnStart = new(0);

    /// <summary>
    /// The grid line where the item's column ends (1-indexed, exclusive). <c>0</c> means <c>auto</c>.
    /// Corresponds to CSS <c>grid-column-end</c>.
    /// </summary>
    public static readonly View.Extra<nint> ColumnEnd = new(0);

    /// <summary>
    /// Places the item into a named grid area defined by <see cref="TemplateAreas"/>.
    /// Corresponds to CSS <c>grid-area</c>.
    /// </summary>
    public static readonly View.Extra<string?> Area = new(null);

    /// <summary>
    /// Controls the order in which the item appears within the grid, overriding source order.
    /// Corresponds to CSS <c>order</c>.
    /// </summary>
    public static readonly View.Extra<nint> Order = new(0);
}
