using System;

namespace Xui.Runtime.Windows;

public static unsafe partial class D3D11
{
    
    public struct DeviceAndSwapChain : IDisposable
    {
        public DXGI.SwapChain? DxgiSwapChain;
        public D3D11.Device? D3D11Device;
        public D3D11.FeatureLevel D3d11FeatureLevel;
        public D3D11.DeviceContext? D3D11ImmediateContext;
        public DXGI.Device? DxgiDevice;

        public void Dispose()
        {
            if (this.DxgiDevice != null)
            {
                this.DxgiDevice.Dispose();
                this.DxgiDevice = null;
            }

            if (this.DxgiSwapChain != null)
            {
                this.DxgiSwapChain.Dispose();
                this.DxgiSwapChain = null;
            }

            if (this.D3D11Device != null)
            {
                this.D3D11Device.Dispose();
                this.D3D11Device = null;
            }

            if (this.D3D11ImmediateContext != null)
            {
                this.D3D11ImmediateContext.Dispose();
                this.D3D11ImmediateContext = null;
            }
        }
    }
}