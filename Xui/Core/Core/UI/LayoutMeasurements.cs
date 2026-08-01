using Xui.Core.Math2D;

namespace Xui.Core.UI;

/// <summary>
/// Per-view results written by an update. The caller owns this value and passes it by reference,
/// so sibling updates cannot share or overwrite one another's results.
/// </summary>
public struct LayoutMeasurements
{
    /// <summary>Desired margin-box size produced by measurement.</summary>
    public Size DesiredSize;
    /// <summary>Final border-edge rectangle produced by arrangement.</summary>
    public Rect ArrangedRect;
}
