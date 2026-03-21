using System.Runtime.InteropServices;
using Xui.Runtime.Windows;
using static Xui.Runtime.Windows.D3D11;

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
public sealed class DirectXGpuDevice : IGpuDevice
{
    // D3D11 wrapper handles
    private readonly Device _device;
    private readonly DeviceContext _deviceContext;
    private bool _disposed;

    /// <inheritdoc/>
    public string BackendName => "D3D11";

    /// <inheritdoc/>
    public bool IsDisposed => _disposed;

    /// <summary>
    /// Creates a new Direct3D 11 GPU device.
    /// </summary>
    /// <exception cref="InvalidOperationException">Thrown if device creation fails.</exception>
    public unsafe DirectXGpuDevice()
    {
        Xui.Runtime.Windows.D3D11.CreateDevice(out var device, out _, out var context);
        _device = device;
        _deviceContext = context;
    }

    /// <inheritdoc/>
    public unsafe IGpuVertexShader CompileVertexShader(string source, string entryPoint)
    {
        ObjectDisposedException.ThrowIf(_disposed, this);
        var bytecode = D3DCompiler.Compile(source, entryPoint, "vs_5_0");
        return new DirectXVertexShader(entryPoint, bytecode, _device);
    }

    /// <inheritdoc/>
    public unsafe IGpuFragmentShader CompileFragmentShader(string source, string entryPoint)
    {
        ObjectDisposedException.ThrowIf(_disposed, this);
        var bytecode = D3DCompiler.Compile(source, entryPoint, "ps_5_0");
        return new DirectXFragmentShader(entryPoint, bytecode, _device);
    }

    /// <inheritdoc/>
    public unsafe IGpuRenderTarget CreateRenderTarget(int width, int height)
    {
        ObjectDisposedException.ThrowIf(_disposed, this);
        return new DirectXRenderTarget(width, height, _device, _deviceContext);
    }

    /// <inheritdoc/>
    public unsafe IGpuCommandList CreateCommandList()
    {
        ObjectDisposedException.ThrowIf(_disposed, this);
        return new DirectXCommandList(_device, _deviceContext);
    }

    /// <inheritdoc/>
    public unsafe void Dispose()
    {
        if (_disposed) return;
        _disposed = true;

        _deviceContext?.Dispose();
        _device?.Dispose();
    }
}
