namespace Xui.Core.UI.Layout;

public partial class Grid
{
    /// <summary>
    /// Controls how auto-placed items are inserted into the grid.
    /// Corresponds to the CSS <c>grid-auto-flow</c> property.
    /// </summary>
    public enum AutoFlow : byte
    {
        /// <summary>Auto-placed items fill each row in turn, adding new rows as needed.</summary>
        Row = 0,

        /// <summary>Auto-placed items fill each column in turn, adding new columns as needed.</summary>
        Column = 1,

        /// <summary>
        /// Auto-placed items fill each row, using a dense packing algorithm that attempts
        /// to fill in holes earlier in the grid.
        /// </summary>
        RowDense = 2,

        /// <summary>
        /// Auto-placed items fill each column, using a dense packing algorithm that attempts
        /// to fill in holes earlier in the grid.
        /// </summary>
        ColumnDense = 3,
    }
}
