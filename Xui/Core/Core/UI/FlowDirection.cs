namespace Xui.Core.UI;

/// <summary>
/// Represents the resolved physical direction of layout flow along an axis.
/// This is derived from <see cref="WritingMode"/> and <see cref="Direction"/>,
/// and is used to control stacking, alignment, and layout flow along block or inline axes.
/// </summary>
public enum FlowDirection : byte
{
    /// <summary>
    /// Content flows from left to right.
    /// </summary>
    LeftToRight = 0,

    /// <summary>
    /// Content flows from right to left.
    /// </summary>
    RightToLeft = 1,

    /// <summary>
    /// Content flows from top to bottom.
    /// </summary>
    TopToBottom = 2,

    /// <summary>
    /// Content flows from bottom to top.
    /// </summary>
    BottomToTop = 3
}
