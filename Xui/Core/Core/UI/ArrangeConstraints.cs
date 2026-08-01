using Xui.Core.Math2D;

namespace Xui.Core.UI;

/// <summary>Immutable input to an arrangement operation.</summary>
public readonly struct ArrangeConstraints
{
    /// <summary>Initializes arrangement constraints.</summary>
    public ArrangeConstraints(Size availableSize, Size desiredSize, Point anchor, LayoutSizeMode xSize = LayoutSizeMode.Exact, LayoutSizeMode ySize = LayoutSizeMode.Exact, LayoutAlign xAlign = LayoutAlign.Start, LayoutAlign yAlign = LayoutAlign.Start)
    {
        AvailableSize = availableSize;
        DesiredSize = desiredSize;
        Anchor = anchor;
        XSize = xSize;
        YSize = ySize;
        XAlign = xAlign;
        YAlign = yAlign;
    }

    /// <summary>Available margin-box size.</summary>
    public Size AvailableSize { get; }
    /// <summary>Desired margin-box size from measurement.</summary>
    public Size DesiredSize { get; }
    /// <summary>Reference point for alignment.</summary>
    public Point Anchor { get; }
    /// <summary>Horizontal sizing mode.</summary>
    public LayoutSizeMode XSize { get; }
    /// <summary>Vertical sizing mode.</summary>
    public LayoutSizeMode YSize { get; }
    /// <summary>Horizontal anchor alignment.</summary>
    public LayoutAlign XAlign { get; }
    /// <summary>Vertical anchor alignment.</summary>
    public LayoutAlign YAlign { get; }
}
