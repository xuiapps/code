namespace Xui.GPU.Hardware.Metal;

/// <summary>
/// Apple Metal hardware GPU device implementation.
/// Provides hardware-accelerated shader compilation and rendering on macOS and iOS.
/// </summary>
/// <remarks>
/// This backend uses the Metal framework for rendering and the Metal shader compiler
/// for MSL shader compilation. Rendering is done off-screen to a texture that can be
/// read back to CPU memory for display via the 2D canvas pipeline.
/// </remarks>
public sealed class MetalGpuDevice : IGpuDevice
{
    private readonly nint _device;  // id<MTLDevice>
    private readonly nint _commandQueue;  // id<MTLCommandQueue>
    private bool _disposed;

    /// <inheritdoc/>
    public string BackendName => "Metal";

    /// <inheritdoc/>
    public bool IsDisposed => _disposed;

    /// <summary>
    /// Creates a new Metal GPU device using the system default device.
    /// </summary>
    /// <exception cref="InvalidOperationException">Thrown if no Metal device is available.</exception>
    public MetalGpuDevice()
    {
        _device = MetalNative.MTLCreateSystemDefaultDevice();
        if (_device == 0)
            throw new InvalidOperationException("Failed to create Metal device. Metal may not be supported on this system.");

        _commandQueue = MetalNative.Device_NewCommandQueue(_device);
        if (_commandQueue == 0)
            throw new InvalidOperationException("Failed to create Metal command queue.");
    }

    /// <inheritdoc/>
    public IGpuVertexShader CompileVertexShader(string source, string entryPoint)
    {
        ObjectDisposedException.ThrowIf(_disposed, this);
        var library = CompileLibrary(source);
        return new MetalVertexShader(entryPoint, library, entryPoint);
    }

    /// <inheritdoc/>
    public IGpuFragmentShader CompileFragmentShader(string source, string entryPoint)
    {
        ObjectDisposedException.ThrowIf(_disposed, this);
        var library = CompileLibrary(source);
        return new MetalFragmentShader(entryPoint, library, entryPoint);
    }

    /// <inheritdoc/>
    public IGpuRenderTarget CreateRenderTarget(int width, int height)
    {
        ObjectDisposedException.ThrowIf(_disposed, this);
        return new MetalRenderTarget(width, height, _device);
    }

    /// <inheritdoc/>
    public IGpuCommandList CreateCommandList()
    {
        ObjectDisposedException.ThrowIf(_disposed, this);
        return new MetalCommandList(_device, _commandQueue);
    }

    private nint CompileLibrary(string mslSource)
    {
        var sourceStr = MetalNative.NSStringFromString(mslSource);
        var library = MetalNative.Device_NewLibraryWithSource(_device, sourceStr, 0, out nint error);

        if (library == 0 || error != 0)
        {
            var errorDesc = MetalNative.GetErrorDescription(error);
            throw new InvalidOperationException($"MSL shader compilation failed:\n{errorDesc}");
        }

        return library;
    }

    /// <inheritdoc/>
    public void Dispose()
    {
        if (_disposed) return;
        _disposed = true;

        MetalNative.Release(_commandQueue);
        MetalNative.Release(_device);
    }
}
