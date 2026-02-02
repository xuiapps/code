using System;
using static Xui.Runtime.Windows.Win32.Types;
using static Xui.Runtime.Windows.Win32.User32;
using static Xui.Runtime.Windows.Win32.User32.Types;

namespace Xui.Runtime.Windows.Actual;

public partial class Win32Window
{
    public abstract class RenderTarget
    {
        protected Win32Window Win32Window { get; }

        public RenderTarget(Win32Window win32Window)
        {
            this.Win32Window = win32Window;
        }

        public virtual bool HandleOnMessage(HWND hWnd, WindowMessage uMsg, WPARAM wParam, LPARAM lParam, out int result)
        {
            result = 0;
            return false;
        }

        public virtual void Render()
        {
        }
    }
}
