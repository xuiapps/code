namespace Xui.GPU.Hardware;

/// <summary>
/// Abstract interface for a GPU hardware device.
/// Implementations exist for Direct3D 11 (Windows) and Metal (macOS/iOS).
/// </summary>
public interface IGpuDevice : IDisposable
{
    /// <summary>Gets the name of this GPU backend (e.g., "D3D11", "Metal").</summary>
    string BackendName { get; }

    /// <summary>Gets whether this device has been disposed.</summary>
    bool IsDisposed { get; }

    /// <summary>
    /// Compiles a vertex shader from native shader source code.
    /// </summary>
    /// <param name="source">The shader source (HLSL for D3D11, MSL for Metal).</param>
    /// <param name="entryPoint">The name of the entry point function.</param>
    /// <returns>A compiled vertex shader.</returns>
    IGpuVertexShader CompileVertexShader(string source, string entryPoint);

    /// <summary>
    /// Compiles a fragment (pixel) shader from native shader source code.
    /// </summary>
    /// <param name="source">The shader source (HLSL for D3D11, MSL for Metal).</param>
    /// <param name="entryPoint">The name of the entry point function.</param>
    /// <returns>A compiled fragment shader.</returns>
    IGpuFragmentShader CompileFragmentShader(string source, string entryPoint);

    /// <summary>
    /// Creates a render target texture.
    /// </summary>
    /// <param name="width">Width in pixels.</param>
    /// <param name="height">Height in pixels.</param>
    /// <returns>A render target that can be used with a command list.</returns>
    IGpuRenderTarget CreateRenderTarget(int width, int height);

    /// <summary>
    /// Creates a command list for recording rendering commands.
    /// </summary>
    IGpuCommandList CreateCommandList();
}
