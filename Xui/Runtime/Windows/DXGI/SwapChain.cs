using System;
using System.Runtime.InteropServices;
using static Xui.Runtime.Windows.COM;

namespace Xui.Runtime.Windows;

public static partial class DXGI
{
    public unsafe class SwapChain : Unknown
    {
        public static new readonly Guid IID = new Guid("310d36a0-d2e7-4c0a-aa04-6a9d23b8886a");

        public static unsafe nint GetBuffer(nint dxgiSwapChainPtr, uint buffer, in Guid IID)
        {
            // TODO: Make sure the assumption that the dxgiSwapChainPtr and dxgiSwapChainPtr+9 are the this* and +9 is the offset in vtable
            nint ppv;
            fixed (Guid* refIdPtr = &IID)
            {
                Marshal.ThrowExceptionForHR(((delegate* unmanaged<nint, uint, Guid*, nint*, int>)(*(*(void***)dxgiSwapChainPtr + 9)))(dxgiSwapChainPtr, buffer, refIdPtr, &ppv));
            }
            return ppv;
        }

        public SwapChain(void* ptr) : base(ptr)
        {
        }

        public void Present(uint syncInterval = 0, Present flags = 0) =>
            Marshal.ThrowExceptionForHR(((delegate* unmanaged[MemberFunction]<void*, uint, Present, int>)this[8])(this, syncInterval, flags));

        public Surface GetBufferAsSurface(uint buffer = 0)
        {
            void* ppv;
            fixed (Guid* refIdPtr = &Surface.IID)
            {
                Marshal.ThrowExceptionForHR(((delegate* unmanaged[MemberFunction]<void*, uint, Guid*, void**, int>)this[9])(this, buffer, refIdPtr, &ppv));
            }
            return new Surface(ppv);
        }

        public void ResizeBuffers(uint bufferCount, uint width, uint height, Format newFormat, SwapChainFlags swapChainFlags) =>
            Marshal.ThrowExceptionForHR(((delegate* unmanaged[MemberFunction]<void*, uint, uint, uint, Format, SwapChainFlags, int> )this[13])(this, bufferCount, width, height, newFormat, swapChainFlags));

        public FrameStatistics GetFrameStatistics()
        {
            FrameStatistics frameStatistics;
            Marshal.ThrowExceptionForHR(((delegate* unmanaged[MemberFunction]<void*, FrameStatistics*, int> )this[16])(this, &frameStatistics));
            return frameStatistics;
        }
    }
}