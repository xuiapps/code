namespace Xui.Core.UI.Layout;

public partial class FlexBox
{
    /// <summary>
    /// Aligns flex items along the main axis when there is free space.
    /// Corresponds to the CSS <c>justify-content</c> property on a flex container.
    /// </summary>
    public enum JustifyContent : byte
    {
        /// <summary>Browser default. Acts like <see cref="FlexStart"/>.</summary>
        Normal = 0,

        /// <summary>Items are packed toward the flex-start edge of the main axis.</summary>
        FlexStart = 1,

        /// <summary>Items are packed toward the flex-end edge of the main axis.</summary>
        FlexEnd = 2,

        /// <summary>Items are centered along the main axis.</summary>
        Center = 3,

        /// <summary>Items are evenly distributed; first item at start, last at end.</summary>
        SpaceBetween = 4,

        /// <summary>Items are evenly distributed with equal space around each item.</summary>
        SpaceAround = 5,

        /// <summary>Items are evenly distributed with equal space between all items including edges.</summary>
        SpaceEvenly = 6,

        /// <summary>Items are packed toward the start of the main axis.</summary>
        Start = 7,

        /// <summary>Items are packed toward the end of the main axis.</summary>
        End = 8,
    }
}
