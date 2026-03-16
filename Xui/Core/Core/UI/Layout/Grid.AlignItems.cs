namespace Xui.Core.UI.Layout;

public partial class Grid
{
    /// <summary>
    /// Sets the default block-axis alignment for all grid items within their grid areas.
    /// Corresponds to the CSS <c>align-items</c> property on a grid container.
    /// </summary>
    public enum AlignItems : byte
    {
        /// <summary>Browser default. Acts like <see cref="Stretch"/>.</summary>
        Normal = 0,

        /// <summary>Items are stretched to fill the grid area block size.</summary>
        Stretch = 1,

        /// <summary>Items are packed toward the start of the block axis.</summary>
        Start = 2,

        /// <summary>Items are packed toward the end of the block axis.</summary>
        End = 3,

        /// <summary>Items are centered on the block axis.</summary>
        Center = 4,

        /// <summary>Items are aligned along their first baseline.</summary>
        Baseline = 5,

        /// <summary>Items are aligned along their first baseline (same as <see cref="Baseline"/>).</summary>
        FirstBaseline = 6,

        /// <summary>Items are aligned along their last baseline.</summary>
        LastBaseline = 7,
    }
}
