namespace Xui.Core.Abstract.Events;

/// <summary>
/// Represents a platform-level input event requesting deletion of content
/// positioned logically before the caret or selection range.
/// Typically triggered by a "Backspace" key press or an equivalent gesture.
/// </summary>
/// <remarks>
/// This event originates from the <c>Actual</c> window layer and is forwarded
/// to the <c>Abstract</c> window for dispatching through the view hierarchy.
/// It is intended to be routed to the appropriate focused or editable view.
/// </remarks>
public ref struct DeleteBackwardsEventRef
{
    /// <summary>
    /// Initializes a new instance of the <see cref="DeleteBackwardsEventRef"/> struct.
    /// </summary>
    public DeleteBackwardsEventRef()
    {
    }
}
