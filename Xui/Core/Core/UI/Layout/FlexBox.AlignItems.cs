namespace Xui.Core.UI.Layout;

public partial class FlexBox
{
    /// <summary>
    /// Sets the default cross-axis alignment for all flex items on a line.
    /// Corresponds to the CSS <c>align-items</c> property on a flex container.
    /// </summary>
    public enum AlignItems : byte
    {
        /// <summary>Browser default. Acts like <see cref="Stretch"/>.</summary>
        Normal = 0,

        /// <summary>Items are stretched to fill the cross axis.</summary>
        Stretch = 1,

        /// <summary>Items are packed toward the flex-start edge of the cross axis.</summary>
        FlexStart = 2,

        /// <summary>Items are packed toward the flex-end edge of the cross axis.</summary>
        FlexEnd = 3,

        /// <summary>Items are centered on the cross axis.</summary>
        Center = 4,

        /// <summary>Items are aligned along their first baseline.</summary>
        Baseline = 5,

        /// <summary>Items are aligned along their first baseline (same as <see cref="Baseline"/>).</summary>
        FirstBaseline = 6,

        /// <summary>Items are aligned along their last baseline.</summary>
        LastBaseline = 7,
    }
}
