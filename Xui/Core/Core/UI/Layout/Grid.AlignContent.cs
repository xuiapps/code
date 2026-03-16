namespace Xui.Core.UI.Layout;

public partial class Grid
{
    /// <summary>
    /// Aligns the grid along the block axis within the container when the grid is smaller than the container.
    /// Corresponds to the CSS <c>align-content</c> property on a grid container.
    /// </summary>
    public enum AlignContent : byte
    {
        /// <summary>Browser default. Acts like <see cref="Stretch"/>.</summary>
        Normal = 0,

        /// <summary>Rows are packed toward the start of the block axis.</summary>
        Start = 1,

        /// <summary>Rows are packed toward the end of the block axis.</summary>
        End = 2,

        /// <summary>Rows are centered on the block axis.</summary>
        Center = 3,

        /// <summary>Rows are evenly distributed; first row at start, last at end.</summary>
        SpaceBetween = 4,

        /// <summary>Rows are evenly distributed with equal space around each row.</summary>
        SpaceAround = 5,

        /// <summary>Rows are evenly distributed with equal space between all rows including edges.</summary>
        SpaceEvenly = 6,

        /// <summary>Rows are stretched to fill the container block size.</summary>
        Stretch = 7,

        /// <summary>Rows are aligned along their first baseline.</summary>
        FirstBaseline = 8,

        /// <summary>Rows are aligned along their last baseline.</summary>
        LastBaseline = 9,
    }
}
