namespace Xui.Core.UI.Input
{
    /// <summary>
    /// Defines the mouse or pointer button associated with a pointer event, based on the W3C Pointer Events specification.
    /// </summary>
    public enum PointerButton : short
    {
        /// <summary>
        /// No button or touch/pen contact change occurred (-1).
        /// </summary>
        None = -1,

        /// <summary>
        /// Left mouse button, touch contact, or pen contact (0).
        /// </summary>
        Left = 0,

        /// <summary>
        /// Middle mouse button (usually the scroll wheel button) (1).
        /// </summary>
        Middle = 1,

        /// <summary>
        /// Right mouse button or pen barrel button (2).
        /// </summary>
        Right = 2,

        /// <summary>
        /// X1 (back) mouse button (3).
        /// </summary>
        X1 = 3,

        /// <summary>
        /// X2 (forward) mouse button (4).
        /// </summary>
        X2 = 4,

        /// <summary>
        /// Pen eraser button (5).
        /// </summary>
        Eraser = 5,
    }
}
