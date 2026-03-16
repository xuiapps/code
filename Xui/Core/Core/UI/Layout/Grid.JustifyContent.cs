namespace Xui.Core.UI.Layout;

public partial class Grid
{
    /// <summary>
    /// Aligns the grid along the inline axis within the container when the grid is smaller than the container.
    /// Corresponds to the CSS <c>justify-content</c> property on a grid container.
    /// </summary>
    public enum JustifyContent : byte
    {
        /// <summary>Browser default. Acts like <see cref="Start"/>.</summary>
        Normal = 0,

        /// <summary>Grid is packed toward the start of the inline axis.</summary>
        Start = 1,

        /// <summary>Grid is packed toward the end of the inline axis.</summary>
        End = 2,

        /// <summary>Grid is centered along the inline axis.</summary>
        Center = 3,

        /// <summary>Grid is packed toward the left end of the inline axis.</summary>
        Left = 4,

        /// <summary>Grid is packed toward the right end of the inline axis.</summary>
        Right = 5,

        /// <summary>Columns are evenly distributed; first column at start, last at end.</summary>
        SpaceBetween = 6,

        /// <summary>Columns are evenly distributed with equal space around each column.</summary>
        SpaceAround = 7,

        /// <summary>Columns are evenly distributed with equal space between all columns including edges.</summary>
        SpaceEvenly = 8,

        /// <summary>Columns are stretched to fill the container inline size.</summary>
        Stretch = 9,
    }
}
