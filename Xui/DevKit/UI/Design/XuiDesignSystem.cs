using Xui.Core.DI;

namespace Xui.DevKit.UI.Design;

/// <summary>
/// Concrete implementation of <see cref="IDesignSystem"/> that derives a complete set of
/// design tokens from a small number of seed properties and the current <see cref="IDeviceInfo"/>.
/// </summary>
public class XuiDesignSystem : IDesignSystem
{
    private readonly XuiColorSystem colors;
    private readonly XuiTypographySystem typography;
    private readonly XuiSpacingSystem spacing;
    private readonly XuiShapeSystem shape;
    private readonly XuiMotionSystem motion;

    /// <summary>
    /// Creates a new design system from the given options and device info.
    /// </summary>
    public XuiDesignSystem(XuiDesignSystemOptions options, IDeviceInfo device)
    {
        colors = new XuiColorSystem(options, device);
        typography = new XuiTypographySystem(options, device);
        spacing = new XuiSpacingSystem();
        shape = new XuiShapeSystem(options);
        motion = new XuiMotionSystem(options, device);

        MinimumHitTestRadius = device.PointerModel switch
        {
            PointerModel.Touch      => 22,
            PointerModel.Stylus     => 10,
            PointerModel.Mouse      => 4,
            PointerModel.Controller => 0,
            PointerModel.Eye        => 0,
            _                       => 4,
        };
    }

    /// <inheritdoc/>
    public IColorSystem Colors => colors;

    /// <inheritdoc/>
    public ITypographySystem Typography => typography;

    /// <inheritdoc/>
    public ISpacingSystem Spacing => spacing;

    /// <inheritdoc/>
    public IShapeSystem Shape => shape;

    /// <inheritdoc/>
    public IMotionSystem Motion => motion;

    /// <inheritdoc/>
    public nfloat MinimumHitTestRadius { get; }

    /// <inheritdoc/>
    public event Action? Changed;

    /// <summary>
    /// Raises the <see cref="Changed"/> event, causing widgets to re-query tokens.
    /// </summary>
    public void NotifyChanged() => Changed?.Invoke();
}

/// <summary>
/// Options for constructing a <see cref="XuiDesignSystem"/>.
/// </summary>
public class XuiDesignSystemOptions
{
    /// <summary>Primary brand hue in degrees (0–360). Required.</summary>
    public nfloat PrimaryHue { get; init; }

    /// <summary>Optional secondary hue override. If null, derived from <see cref="Harmony"/>.</summary>
    public nfloat? SecondaryHue { get; init; }

    /// <summary>Optional tertiary/accent hue override. If null, derived from <see cref="Harmony"/>.</summary>
    public nfloat? TertiaryHue { get; init; }

    /// <summary>Optional neutral hue override. If null, derived as primary hue desaturated.</summary>
    public nfloat? NeutralHue { get; init; }

    /// <summary>Color harmony used to derive secondary/tertiary hues. Default: SplitComplementary.</summary>
    public ColorHarmony Harmony { get; init; } = ColorHarmony.SplitComplementary;

    /// <summary>Target chroma for the primary palette (typical range 0.10–0.35). Default: 0.15.</summary>
    public nfloat Chroma { get; init; } = (nfloat)0.15;

    /// <summary>Global corner-radius multiplier (0.0 = square, 1.0 = default, 2.0 = very round). Default: 1.0.</summary>
    public nfloat RoundnessFactor { get; init; } = 1;

    /// <summary>Preferred animation style. Default: Curve.</summary>
    public MotionPreference MotionPreference { get; init; } = MotionPreference.Curve;

    /// <summary>Default font family for all text styles. Default: system-ui.</summary>
    public string DefaultFontFamily { get; init; } = "Inter";
}
