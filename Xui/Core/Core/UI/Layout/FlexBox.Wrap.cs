namespace Xui.Core.UI.Layout;

public partial class FlexBox
{
    /// <summary>
    /// Controls whether flex items are forced onto a single line or can wrap onto multiple lines.
    /// Corresponds to the CSS <c>flex-wrap</c> property.
    /// </summary>
    public enum Wrap : byte
    {
        /// <summary>All flex items are on a single line.</summary>
        NoWrap = 0,

        /// <summary>Flex items wrap onto additional lines in the block-start direction.</summary>
        Wrap = 1,

        /// <summary>Flex items wrap onto additional lines in the block-end direction.</summary>
        WrapReverse = 2,
    }
}
