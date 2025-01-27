using System;
using System.Runtime.InteropServices;
using static Xui.Runtime.Windows.COM;
using static Xui.Runtime.Windows.Win32.Types;
using static Xui.Runtime.Windows.Win32.User32.Types;

namespace Xui.Runtime.Windows;

public static partial class DComp
{
    public unsafe class Device : Unknown
    {
        public static new readonly Guid IID = new Guid("C37EA93A-E7AA-450D-B16F-9746CB0407F3");

        public static Device Create(DXGI.Device dxgiDevice)
        {
            void* dcompositionDevice;
            DCompositionCreateDevice(dxgiDevice, in IID, out dcompositionDevice);
            return new Device(dcompositionDevice);
        }

        public Device(void* ptr) : base(ptr)
        {
        }

        public void Commit() =>
            Marshal.ThrowExceptionForHR(((delegate* unmanaged[MemberFunction]<void*, int>)this[3])(this));

        public FrameStatistics GetFrameStatistics()
        {
            FrameStatistics frameStatistics;
            Marshal.ThrowExceptionForHR(((delegate* unmanaged[MemberFunction]<void*, FrameStatistics*, int>)this[5])(this, &frameStatistics));
            return frameStatistics;
        }

        public Target CreateTargetForHwnd(HWND hwnd, bool topmost = false)
        {
            void* ppTarget;
            Marshal.ThrowExceptionForHR(((delegate* unmanaged[MemberFunction]<void*, HWND, BOOL, void**, int>)this[6])(this, hwnd, topmost, &ppTarget));
            return new Target(ppTarget);
        }

        public Visual CreateVisual()
        {
            void* ppVisual;
            ((delegate* unmanaged[MemberFunction]<void*, void**, int> )this[7])(this, &ppVisual);
            return new Visual(ppVisual);
        }
    }
}
