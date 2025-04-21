namespace Xui.Core.UI;

/// <summary>
/// Controls whether layout and rendering should respect directionality and writing mode.
/// Used to suppress mirroring and bidi-aware behavior for diagrams, graphs, and non-linguistic views.
/// </summary>
public enum Flow : byte
{
    /// <summary>
    /// Inherit flow behavior from the parent view.
    /// </summary>
    Inherit = 0,

    /// <summary>
    /// Enable direction-aware and writing-mode-aware behavior.
    /// </summary>
    Aware = 1,

    /// <summary>
    /// Disable all direction-aware layout and rendering. Used for charts, maps, etc.
    /// </summary>
    Unaware = 2
}
