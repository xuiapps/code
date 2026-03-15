using Xui.Core.Debug;

namespace Xui.Core.UI;

/// <summary>
/// Passed to <see cref="View.OnAttach"/> when a view is added to the visual tree.
/// Carries tree-wide shared resources (e.g. instrumentation) down the DFS walk so
/// every view in the subtree receives them without per-view service lookups.
/// </summary>
public ref struct AttachEventRef
{
    /// <summary>
    /// Instrumentation sink for the visual tree. Views receive this during attach
    /// and store it so invalidation methods (e.g. <see cref="View.InvalidateRender"/>,
    /// <see cref="View.RequestAnimationFrame"/>) can log consistently with the
    /// layout-pass instrumentation carried by <see cref="LayoutGuide.Instruments"/>.
    /// </summary>
    public InstrumentsAccessor Instruments;

    /// <summary>Initializes a new <see cref="AttachEventRef"/>.</summary>
    public AttachEventRef()
    {
    }
}
