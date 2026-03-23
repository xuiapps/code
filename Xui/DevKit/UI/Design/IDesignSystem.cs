namespace Xui.DevKit.UI.Design;

/// <summary>
/// Root interface for the Xui Design System.
/// Resolved from the parent-chain service provider via GetService&lt;IDesignSystem&gt;().
/// </summary>
public interface IDesignSystem
{
    /// <summary>Color tokens and palette math.</summary>
    IColorSystem Colors { get; }

    /// <summary>Typography scale.</summary>
    ITypographySystem Typography { get; }

    /// <summary>Spacing tokens derived from the 4-pt grid.</summary>
    ISpacingSystem Spacing { get; }

    /// <summary>Shape / corner-radius tokens.</summary>
    IShapeSystem Shape { get; }

    /// <summary>Motion tokens (curves and springs).</summary>
    IMotionSystem Motion { get; }

    /// <summary>
    /// Minimum hit-test radius in points. A design choice derived from the device's pointer model.
    /// Touch = 22 pt, Stylus = 10 pt, Mouse = 4 pt.
    /// </summary>
    nfloat MinimumHitTestRadius { get; }

    /// <summary>Raised when any design token has changed (e.g. dark mode toggle, font scale change).</summary>
    event Action? Changed;
}
