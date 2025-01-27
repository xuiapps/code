using System;
using System.Runtime.InteropServices;
using static Xui.Runtime.Windows.DXGI;
using static Xui.Runtime.Windows.Win32.Types;

namespace Xui.Runtime.Windows;

/// <summary>
/// Code from &lt;d3d11.h&gt; in the d3d11.dll lib.
/// </summary>
public static partial class D3D11
{
    public const string D3D11Lib = "d3d11.dll";

    public static readonly nint Lib = NativeLibrary.Load(D3D11Lib);

    public const uint D3D11_SDK_VERSION = 7;

    [LibraryImport(D3D11Lib, EntryPoint="D3D11CreateDeviceAndSwapChain")]
    private static unsafe partial HRESULT D3D11CreateDeviceAndSwapChain(
        nint adapter,
        DriverType driverType,
        nint software,
        CreteDeviceFlags flags,
        nint featureLevelArrPtr,
        uint featureLevelsCount,
        uint sdkVersion,
        ref SwapChainDesc swapChainDesc,
        out void* swapChain,
        out void* device,
        out FeatureLevel featureLevel,
        out void* immediateContext
    );

    public static unsafe DeviceAndSwapChain CreateDeviceAndSwapChain(nint hWnd, uint width, uint height)
    {
        SwapChainDesc swapChainDesc = new()
        {
            BufferDesc = new ()
            {
                Width = width,
                Height = height,
                Format = Format.B8G8R8A8_UNORM,
                RefreshRate = new ()
                {
                    Numerator = 60,
                    Denominator = 1
                },
            },
            SampleDesc = new ()
            {
                Count = 1,
                Quality = 0
            },
            BufferUsage = Usage.RenderTargetOutput,
            BufferCount = 1,
            OutputWindow = hWnd,
            Windowed = true,
            SwapEffect = SwapEffect.Discard,
            Flags = 0
        };

        Marshal.ThrowExceptionForHR(D3D11.D3D11CreateDeviceAndSwapChain(
            adapter: 0,
            driverType: D3D11.DriverType.Hardware,
            software: 0,
            flags: D3D11.CreteDeviceFlags.SingleThreaded | D3D11.CreteDeviceFlags.BGRASupport,
            featureLevelArrPtr: 0,
            featureLevelsCount: 0,
            sdkVersion: D3D11.D3D11_SDK_VERSION,
            swapChainDesc: ref swapChainDesc,

            swapChain: out var dxgiSwapChainPtr,
            device: out var d3d11DevicePtr,
            featureLevel: out var d3d11FeatureLevel,
            immediateContext: out var d3d11ImmediateContextPtr));

        var dxgiSwapChain = new DXGI.SwapChain(dxgiSwapChainPtr);
        var d3d11Device = new D3D11.Device(d3d11DevicePtr);
        var d3d11ImmediateContext = new D3D11.DeviceContext(d3d11ImmediateContextPtr);

        var dxgiDevicePtr = d3d11Device.QueryInterface(DXGI.Device.IID);
        var dxgiDevice = new DXGI.Device(dxgiDevicePtr);

        return new DeviceAndSwapChain()
        {
            DxgiSwapChain = dxgiSwapChain,
            D3D11Device = d3d11Device,
            D3d11FeatureLevel = d3d11FeatureLevel,
            D3D11ImmediateContext = d3d11ImmediateContext,
            DxgiDevice = dxgiDevice
        };
    }

    [LibraryImport(D3D11Lib, EntryPoint="D3D11CreateDevice")]
    private static unsafe partial HRESULT D3D11CreateDevice(void* pAdapter, DriverType driverType, void* software, uint flags, FeatureLevel* pFeatureLevels, uint featureLevels, uint sdkVersion, void** ppDevice, out FeatureLevel featureLevel, void** ppImmediateContext);

    public static unsafe void CreateDevice(out D3D11.Device device, out FeatureLevel featureLevel, out DeviceContext deviceContext)
    {
        void* ppDevice;
        void* ppImmediateContext;
        uint sdkVersion = 7;
        Marshal.ThrowExceptionForHR(D3D11CreateDevice(null, DriverType.Hardware, null, (uint)CreteDeviceFlags.BGRASupport, null, 0, sdkVersion, &ppDevice, out featureLevel, &ppImmediateContext));
        device = new Device(ppDevice);
        deviceContext = new DeviceContext(ppImmediateContext);
    }
}
