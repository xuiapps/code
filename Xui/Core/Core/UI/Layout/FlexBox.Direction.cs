namespace Xui.Core.UI.Layout;

public partial class FlexBox
{
    /// <summary>
    /// Specifies the direction of the main axis along which flex items are placed.
    /// Corresponds to the CSS <c>flex-direction</c> property.
    /// </summary>
    public new enum Direction : byte
    {
        /// <summary>Items are placed left to right (inline-start to inline-end).</summary>
        Row = 0,

        /// <summary>Items are placed right to left (inline-end to inline-start).</summary>
        RowReverse = 1,

        /// <summary>Items are placed top to bottom (block-start to block-end).</summary>
        Column = 2,

        /// <summary>Items are placed bottom to top (block-end to block-start).</summary>
        ColumnReverse = 3,
    }
}
