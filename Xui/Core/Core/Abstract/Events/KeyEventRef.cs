namespace Xui.Core.Abstract.Events;

/// <summary>
/// Represents a keyboard input event.
/// Used for both key down (Key is set) and character input (Character is set) events.
/// </summary>
public ref struct KeyEventRef
{
    /// <summary>
    /// The virtual key code for key down/up events.
    /// </summary>
    public VirtualKey Key;

    /// <summary>
    /// The Unicode character for character input events (e.g. WM_CHAR).
    /// Zero when this is a key down/up event without character translation.
    /// </summary>
    public char Character;

    /// <summary>
    /// True if the Shift key is held during this event.
    /// </summary>
    public bool Shift;

    /// <summary>
    /// True if this is an auto-repeat event (key held down).
    /// </summary>
    public bool IsRepeat;

    /// <summary>
    /// Set to true by the handler to indicate the event was consumed.
    /// </summary>
    public bool Handled;
}
