namespace Xui.Core.UI.Layout;

public partial class FlexBox
{
    /// <summary>
    /// Controls the order in which the item appears within the flex container, overriding source order.
    /// Corresponds to CSS <c>order</c>.
    /// </summary>
    public static readonly View.Extra<nint> Order = new(0);

    /// <summary>
    /// How much the item will grow relative to other flex items when free space is available.
    /// Corresponds to CSS <c>flex-grow</c>. Default is <c>0</c> (does not grow).
    /// </summary>
    public static readonly View.Extra<nfloat> Grow = new(0);

    /// <summary>
    /// How much the item will shrink relative to other flex items when space is insufficient.
    /// Corresponds to CSS <c>flex-shrink</c>. Default is <c>1</c> (shrinks evenly).
    /// </summary>
    public static readonly View.Extra<nfloat> Shrink = new(1);

    /// <summary>
    /// The initial size of the item along the main axis before free space is distributed.
    /// <c>nfloat.NaN</c> means <c>auto</c> (use the item's width or height).
    /// Corresponds to CSS <c>flex-basis</c>.
    /// </summary>
    public static readonly View.Extra<nfloat> Basis = new(nfloat.NaN);
}
