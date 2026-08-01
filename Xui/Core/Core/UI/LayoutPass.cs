namespace Xui.Core.UI;

/// <summary>Passes that may be requested during a render-surface update.</summary>
[Flags]
public enum LayoutPass : byte
{
    /// <summary>Advance time-based state.</summary>
    Animate = 1 << 0,
    /// <summary>Measure desired size.</summary>
    Measure = 1 << 1,
    /// <summary>Arrange final frame.</summary>
    Arrange = 1 << 2,
    /// <summary>Render the view.</summary>
    Render = 1 << 3,
}
