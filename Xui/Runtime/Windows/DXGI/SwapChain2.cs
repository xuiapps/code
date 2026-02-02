using System;
using System.Runtime.InteropServices;
using static Xui.Runtime.Windows.COM;

namespace Xui.Runtime.Windows;

public static partial class DXGI
{
    public unsafe class SwapChain2 : SwapChain1
    {
        // IDXGISwapChain2
        public static new readonly Guid IID = new Guid("A8BE2AC4-199F-4946-B331-79599FB98DE7");

        public SwapChain2(void* ptr) : base(ptr)
        {
        }

        /// <summary>
        /// Limits the number of frames the swap chain can queue for rendering.
        /// Use 1 for tight "render once per frame" pacing.
        /// </summary>
        /// <remarks>
        /// https://learn.microsoft.com/en-us/windows/win32/api/dxgi1_2/nf-dxgi1_2-idxgiswapchain2-setmaximumframelatency
        /// </remarks>
        public void SetMaximumFrameLatency(uint maxLatency) =>
            Marshal.ThrowExceptionForHR(((delegate* unmanaged[MemberFunction]<void*, uint, int>)this[31])(this, maxLatency));

        /// <summary>
        /// Returns a waitable handle that becomes signaled when the swap chain is ready
        /// to accept a new frame (used for frame pacing).
        /// </summary>
        /// <remarks>
        /// Requires the swap chain to be created with <see cref="SwapChainFlags.FrameLatencyWaitableObject"/>.
        /// https://learn.microsoft.com/en-us/windows/win32/api/dxgi1_3/nf-dxgi1_3-idxgiswapchain2-getframelatencywaitableobject
        /// </remarks>
        public nint GetFrameLatencyWaitableObject() =>
            ((delegate* unmanaged[MemberFunction]<void*, nint>)this[33])(this);
    }
}
