namespace Xui.Core.UI.Input
{
    /// <summary>
    /// Defines the set of mouse or pointer buttons currently pressed, based on the W3C Pointer Events specification.
    /// </summary>
    [Flags]
    public enum PointerButtons : short
    {
        /// <summary>
        /// No buttons are currently pressed.
        /// </summary>
        None = 0,

        /// <summary>
        /// Left mouse button, touch contact, or pen contact.
        /// </summary>
        Left = 1 << 0,

        /// <summary>
        /// Right mouse button or pen barrel button.
        /// </summary>
        Right = 1 << 1,

        /// <summary>
        /// Middle mouse button.
        /// </summary>
        Middle = 1 << 2,

        /// <summary>
        /// X1 (back) mouse button.
        /// </summary>
        X1 = 1 << 3,

        /// <summary>
        /// X2 (forward) mouse button.
        /// </summary>
        X2 = 1 << 4,

        /// <summary>
        /// Pen eraser button.
        /// </summary>
        Eraser = 1 << 5,
    }
}
