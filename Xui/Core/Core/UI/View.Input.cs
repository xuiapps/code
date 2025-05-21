using Xui.Core.UI.Input;

namespace Xui.Core.UI;

public partial class View
{
    /// <summary>
    /// Called during event dispatch to handle a pointer event in a specific event phase.
    /// </summary>
    public virtual void OnPointerEvent(ref PointerEventRef e, EventPhase phase)
    {
        // override in child classes
    }
}
