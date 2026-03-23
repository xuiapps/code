using Xui.Core.Canvas;

namespace Xui.DevKit.UI.Design;

/// <summary>
/// Provides color roles derived from a seed palette, grouped into semantic
/// <see cref="ColorGroup"/> bundles and a set of neutral/structural colors.
/// </summary>
public interface IColorSystem
{
    /// <summary>
    /// The application canvas color group.
    /// Background = window/screen fill, Foreground = body text,
    /// Container = card/panel fill, OnContainer = card text.
    /// </summary>
    ColorGroup Application { get; }

    /// <summary>
    /// The surface (card/panel) color group, slightly elevated from Application.
    /// Background = card fill, Foreground = card body text,
    /// Container = alternate surface (e.g. alternating table row, hover bg),
    /// OnContainer = secondary text on alternate surface.
    /// </summary>
    ColorGroup Surface { get; }

    /// <summary>Neutral border/divider color (mid-tone).</summary>
    Color Outline { get; }

    /// <summary>Lighter neutral border/divider variant.</summary>
    Color OutlineVariant { get; }

    /// <summary>Brand / primary action group (from the Primary tonal ramp).</summary>
    ColorGroup Primary { get; }

    /// <summary>Supporting / secondary action group (from the Secondary tonal ramp).</summary>
    ColorGroup Secondary { get; }

    /// <summary>Tertiary highlight / accent group (from the Accent tonal ramp).</summary>
    ColorGroup Accent { get; }

    /// <summary>Error / destructive state group (from the Error tonal ramp).</summary>
    ColorGroup Error { get; }

    /// <summary>Focus ring color (typically Accent.Background at full opacity).</summary>
    Color FocusRing { get; }

    /// <summary>
    /// Returns a full tonal ramp for any hue/chroma combination.
    /// </summary>
    OklchRamp GetTonalRamp(nfloat hueDegrees, nfloat chroma);

    /// <summary>True if the current effective color scheme is dark.</summary>
    bool IsDark { get; }
}
