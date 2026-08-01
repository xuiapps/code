using Xui.Core.Math2D;

namespace Xui.Core.UI;

/// <summary>Immutable input to a measurement operation.</summary>
public readonly struct MeasureConstraints
{
    /// <summary>Initializes measurement constraints.</summary>
    public MeasureConstraints(Size availableSize, LayoutSizeMode xSize = LayoutSizeMode.AtMost, LayoutSizeMode ySize = LayoutSizeMode.AtMost)
    {
        AvailableSize = availableSize;
        XSize = xSize;
        YSize = ySize;
    }

    /// <summary>Available margin-box size.</summary>
    public Size AvailableSize { get; }
    /// <summary>Horizontal sizing mode.</summary>
    public LayoutSizeMode XSize { get; }
    /// <summary>Vertical sizing mode.</summary>
    public LayoutSizeMode YSize { get; }
}
