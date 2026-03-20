using System.Runtime.InteropServices;

namespace Xui.GPU.Hardware.D3D11;

/// <summary>
/// Direct3D 11 hardware GPU device implementation.
/// Provides hardware-accelerated shader compilation and rendering on Windows.
/// </summary>
/// <remarks>
/// This backend uses D3D11 for rendering and D3DCompiler for HLSL compilation.
/// Rendering is done off-screen to a texture that can be read back to CPU memory
/// for display via the software/2D canvas pipeline.
/// </remarks>
public sealed class D3D11GpuDevice : IGpuDevice
{
    // D3D11 native handles
    private readonly unsafe void* _device;
    private readonly unsafe void* _deviceContext;
    private bool _disposed;

    /// <inheritdoc/>
    public string BackendName => "D3D11";

    /// <inheritdoc/>
    public bool IsDisposed => _disposed;

    /// <summary>
    /// Creates a new Direct3D 11 GPU device.
    /// </summary>
    /// <exception cref="InvalidOperationException">Thrown if device creation fails.</exception>
    public unsafe D3D11GpuDevice()
    {
        void* device = null;
        void* context = null;

        // Create D3D11 device with hardware driver
        int hr = D3D11Native.D3D11CreateDevice(
            adapter: null,
            driverType: D3D11Native.DriverType.Hardware,
            software: null,
            flags: D3D11Native.DeviceCreationFlags.BgraSupport | D3D11Native.DeviceCreationFlags.SingleThreaded,
            featureLevels: null,
            numFeatureLevels: 0,
            sdkVersion: D3D11Native.D3D11SdkVersion,
            device: &device,
            featureLevel: null,
            immediateContext: &context);

        Marshal.ThrowExceptionForHR(hr);

        if (device == null)
            throw new InvalidOperationException("D3D11CreateDevice returned null device.");
        if (context == null)
            throw new InvalidOperationException("D3D11CreateDevice returned null device context.");

        _device = device;
        _deviceContext = context;
    }

    /// <inheritdoc/>
    public unsafe IGpuVertexShader CompileVertexShader(string source, string entryPoint)
    {
        ObjectDisposedException.ThrowIf(_disposed, this);
        var bytecode = D3DCompiler.Compile(source, entryPoint, "vs_5_0");
        return new D3D11VertexShader(entryPoint, bytecode, _device);
    }

    /// <inheritdoc/>
    public unsafe IGpuFragmentShader CompileFragmentShader(string source, string entryPoint)
    {
        ObjectDisposedException.ThrowIf(_disposed, this);
        var bytecode = D3DCompiler.Compile(source, entryPoint, "ps_5_0");
        return new D3D11FragmentShader(entryPoint, bytecode, _device);
    }

    /// <inheritdoc/>
    public unsafe IGpuRenderTarget CreateRenderTarget(int width, int height)
    {
        ObjectDisposedException.ThrowIf(_disposed, this);
        return new D3D11RenderTarget(width, height, _device, _deviceContext);
    }

    /// <inheritdoc/>
    public unsafe IGpuCommandList CreateCommandList()
    {
        ObjectDisposedException.ThrowIf(_disposed, this);
        return new D3D11CommandList(_device, _deviceContext);
    }

    /// <inheritdoc/>
    public unsafe void Dispose()
    {
        if (_disposed) return;
        _disposed = true;

        // Release device context (ID3D11DeviceContext)
        if (_deviceContext != null)
            D3D11Native.Release(_deviceContext);

        // Release device (ID3D11Device)
        if (_device != null)
            D3D11Native.Release(_device);
    }
}
