namespace Xui.Core.Debug;

/// <summary>Identifies the subsystem or category of an instrumentation event.</summary>
[Flags]
public enum Scope : uint
{
    /// <summary>Application lifecycle events (startup, shutdown, window open/close).</summary>
    Application      = 1 << 0,
    /// <summary>Input event processing (pointer, keyboard, touch).</summary>
    Input            = 1 << 1,
    /// <summary>Top-level rendering pipeline events.</summary>
    Rendering        = 1 << 2,
    /// <summary>View attach, detach, activate, and deactivate events.</summary>
    ViewLifecycle    = 1 << 3,
    /// <summary>View animation frame callbacks.</summary>
    ViewAnimation    = 1 << 4,
    /// <summary>View measure pass events.</summary>
    ViewMeasure      = 1 << 5,
    /// <summary>View arrange pass events.</summary>
    ViewArrange      = 1 << 6,
    /// <summary>View render pass events.</summary>
    ViewRendering    = 1 << 7,
    /// <summary>View state changes (invalidation, focus, flags).</summary>
    ViewState        = 1 << 8,
}
