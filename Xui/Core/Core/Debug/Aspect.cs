namespace Xui.Core.Debug;

/// <summary>
/// Aspect flags for categorizing instrumentation events.
/// </summary>
[Flags]
public enum Aspect : uint
{
    /// <summary>Application and OS-level events.</summary>
    ApplicationOS = 1 << 0,

    /// <summary>Input events (mouse, keyboard, touch).</summary>
    InputEvents = 1 << 1,

    /// <summary>Window rendering events.</summary>
    WindowRendering = 1 << 2,

    /// <summary>View lifecycle events.</summary>
    ViewLifecycle = 1 << 3,

    /// <summary>View animation timers.</summary>
    ViewAnimationTimers = 1 << 4,

    /// <summary>View measure operations.</summary>
    ViewMeasure = 1 << 5,

    /// <summary>View arrange operations.</summary>
    ViewArrange = 1 << 6,

    /// <summary>View rendering operations.</summary>
    ViewRendering = 1 << 7,

    /// <summary>View state changes.</summary>
    ViewState = 1 << 8,
}
