namespace Xui.Core.Abstract;

public partial interface IWindow
{
    public partial interface IDesktopStyle
    {
        public enum WindowClientArea
        {
            /// <summary>System-managed non-client area (title bar + borders behave normally).</summary>
            Default,

            /// <summary>
            /// Extend the client area into the title bar region (app draws background/title area).
            /// </summary>
            Extended,
        }
    }
}
