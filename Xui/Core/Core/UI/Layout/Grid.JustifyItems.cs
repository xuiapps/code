namespace Xui.Core.UI.Layout;

public partial class Grid
{
    /// <summary>
    /// Sets the default <c>justify-self</c> for all items, aligning them along the inline axis within their grid area.
    /// Corresponds to the CSS <c>justify-items</c> property.
    /// </summary>
    public enum JustifyItems : byte
    {
        /// <summary>Browser default. Acts like <see cref="Stretch"/>.</summary>
        Normal = 0,

        /// <summary>Items are stretched to fill the grid area inline size.</summary>
        Stretch = 1,

        /// <summary>Items are packed toward the start of the inline axis.</summary>
        Start = 2,

        /// <summary>Items are packed toward the end of the inline axis.</summary>
        End = 3,

        /// <summary>Items are centered along the inline axis.</summary>
        Center = 4,

        /// <summary>Items are packed toward the left end of the inline axis.</summary>
        Left = 5,

        /// <summary>Items are packed toward the right end of the inline axis.</summary>
        Right = 6,

        /// <summary>Items are aligned along their first baseline.</summary>
        Baseline = 7,

        /// <summary>Items are aligned along their first baseline (same as <see cref="Baseline"/>).</summary>
        FirstBaseline = 8,

        /// <summary>Items are aligned along their last baseline.</summary>
        LastBaseline = 9,
    }
}
