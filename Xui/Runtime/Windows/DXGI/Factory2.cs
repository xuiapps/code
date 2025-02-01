using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using static Xui.Runtime.Windows.Win32.Types;

namespace Xui.Runtime.Windows;

public static partial class DXGI
{
    public unsafe partial class Factory2 : Factory1
    {
        public static new readonly Guid IID = new Guid("50c83a1c-e072-4c48-87b0-3630fa36a6d0");

        [LibraryImport(DXGILib)]
        private static unsafe partial HRESULT CreateDXGIFactory2(uint Flags, in Guid riid, void* ppFactory);

        public static Factory2 Create()
        {
            void* ppFactory;
            CreateDXGIFactory2(0, in Factory2.IID, &ppFactory);
            return new Factory2(ppFactory);
        }

        public Factory2(void* ptr) : base(ptr)
        {
        }

        public SwapChain1 CreateSwapChainForComposition(DXGI.Device device, in SwapChainDesc1 desc, Output? restrictToOutput = null)
        {
            void* ppDxgiSwapChain1;
            fixed(SwapChainDesc1* pDesc = &desc)
            {
                Marshal.ThrowExceptionForHR(((delegate* unmanaged[MemberFunction]<void*, void*, SwapChainDesc1*, void*, void**, int>)this[24])(this, device, pDesc, restrictToOutput, &ppDxgiSwapChain1));
            }
            return new SwapChain1(ppDxgiSwapChain1);
        }
    }
}