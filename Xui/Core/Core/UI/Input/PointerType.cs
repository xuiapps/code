namespace Xui.Core.UI.Input
{
    /// <summary>
    /// Defines the type of input device associated with a pointer event.
    /// </summary>
    public enum PointerType
    {
        /// <summary>
        /// A mouse device generated the pointer event.
        /// </summary>
        Mouse,

        /// <summary>
        /// A touch contact (e.g., finger or capacitive touch) generated the pointer event.
        /// </summary>
        Touch,

        /// <summary>
        /// A pen, stylus, or similar fine-point device generated the pointer event.
        /// </summary>
        Pen,

        /// <summary>
        /// The pointer device type could not be determined.
        /// </summary>
        Unknown,
    }
}
