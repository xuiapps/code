using Xui.Core.Canvas;

namespace Xui.DevKit.UI.Design;

/// <summary>
/// Concrete shape system with corner radii scaled by a global roundness factor.
/// </summary>
internal class XuiShapeSystem : IShapeSystem
{
    public XuiShapeSystem(XuiDesignSystemOptions options)
    {
        RoundnessFactor = options.RoundnessFactor;
        var f = RoundnessFactor;

        None       = new CornerRadius(0);
        ExtraSmall = new CornerRadius(2 * f);
        Small      = new CornerRadius(4 * f);
        Medium     = new CornerRadius(8 * f);
        Large      = new CornerRadius(12 * f);
        ExtraLarge = new CornerRadius(16 * f);
        Full       = new CornerRadius(9999);
    }

    /// <inheritdoc/>
    public nfloat RoundnessFactor { get; }

    /// <inheritdoc/>
    public CornerRadius None { get; }

    /// <inheritdoc/>
    public CornerRadius ExtraSmall { get; }

    /// <inheritdoc/>
    public CornerRadius Small { get; }

    /// <inheritdoc/>
    public CornerRadius Medium { get; }

    /// <inheritdoc/>
    public CornerRadius Large { get; }

    /// <inheritdoc/>
    public CornerRadius ExtraLarge { get; }

    /// <inheritdoc/>
    public CornerRadius Full { get; }
}
