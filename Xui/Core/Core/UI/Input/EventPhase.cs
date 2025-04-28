namespace Xui.Core.UI.Input
{
    /// <summary>
    /// Defines the phase of event delivery through the view hierarchy.
    /// </summary>
    public enum EventPhase
    {
        /// <summary>
        /// Event is tunneling from the root down toward the target view.
        /// </summary>
        Tunnel,

        /// <summary>
        /// Event is bubbling from the target view up toward the root.
        /// </summary>
        Bubble
    }
}