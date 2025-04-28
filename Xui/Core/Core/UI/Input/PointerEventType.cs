namespace Xui.Core.UI.Input
{
    /// <summary>
    /// Defines the type of action associated with a pointer event, based on the W3C Pointer Events specification.
    /// </summary>
    public enum PointerEventType
    {
        /// <summary>
        /// The pointer has moved onto the element's hit region.
        /// Corresponds to the "pointerover" event.
        /// </summary>
        Over,

        /// <summary>
        /// The pointer has entered the element or one of its descendants.
        /// Corresponds to the "pointerenter" event.
        /// </summary>
        Enter,

        /// <summary>
        /// A pointer button has been pressed.
        /// Corresponds to the "pointerdown" event.
        /// </summary>
        Down,

        /// <summary>
        /// The pointer has moved while being active.
        /// Corresponds to the "pointermove" event.
        /// </summary>
        Move,

        /// <summary>
        /// A pointer button has been released.
        /// Corresponds to the "pointerup" event.
        /// </summary>
        Up,

        /// <summary>
        /// The pointer input was canceled by the system.
        /// Corresponds to the "pointercancel" event.
        /// </summary>
        Cancel,

        /// <summary>
        /// The pointer has moved off of the element's hit region.
        /// Corresponds to the "pointerout" event.
        /// </summary>
        Out,

        /// <summary>
        /// The pointer has left the element and all its descendants.
        /// Corresponds to the "pointerleave" event.
        /// </summary>
        Leave,

        /// <summary>
        /// The pointer has been captured by the element.
        /// Corresponds to the "gotpointercapture" event.
        /// </summary>
        GotCapture,

        /// <summary>
        /// The pointer capture has been released from the element.
        /// Corresponds to the "lostpointercapture" event.
        /// </summary>
        LostCapture,
    }
}
