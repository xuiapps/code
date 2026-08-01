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

    /// <summary>Raised when any design token has changed (e.g. dark mode toggle, font scale change).</summary>
    event Action? Changed;
}

/// <summary>
/// Receives a design-token update after a view has resolved its <see cref="IDesignSystem"/>.
/// Implementations should copy every token they use into local fields from the cached provider.
/// </summary>
public interface IDesignSystemChangeNotifications
{
    /// <summary>Copies the latest design tokens into the view's local state.</summary>
    void OnDesignTokenChange();
}
