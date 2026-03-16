namespace Xui.Core.UI.Layout;

public partial class FlexBox
{
    /// <summary>
    /// Aligns flex lines along the cross axis when there is extra space (only applies when wrapping).
    /// Corresponds to the CSS <c>align-content</c> property on a flex container.
    /// </summary>
    public enum AlignContent : byte
    {
        /// <summary>Browser default. Acts like <see cref="Stretch"/>.</summary>
        Normal = 0,

        /// <summary>Lines are packed toward the flex-start edge of the cross axis.</summary>
        FlexStart = 1,

        /// <summary>Lines are packed toward the flex-end edge of the cross axis.</summary>
        FlexEnd = 2,

        /// <summary>Lines are centered on the cross axis.</summary>
        Center = 3,

        /// <summary>Lines are evenly distributed; first line at start, last at end.</summary>
        SpaceBetween = 4,

        /// <summary>Lines are evenly distributed with equal space around each line.</summary>
        SpaceAround = 5,

        /// <summary>Lines are evenly distributed with equal space between all lines including edges.</summary>
        SpaceEvenly = 6,

        /// <summary>Lines are stretched to fill the container.</summary>
        Stretch = 7,

        /// <summary>Lines are aligned along their first baseline.</summary>
        FirstBaseline = 8,

        /// <summary>Lines are aligned along their last baseline.</summary>
        LastBaseline = 9,
    }
}
