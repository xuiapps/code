namespace Xui.GPU.Hardware;

/// <summary>
/// Represents a compiled GPU shader ready for use in a hardware pipeline.
/// </summary>
public interface IGpuShader : IDisposable
{
    /// <summary>Gets the name of this shader.</summary>
    string Name { get; }
}

/// <summary>
/// Represents a compiled vertex shader.
/// </summary>
public interface IGpuVertexShader : IGpuShader { }

/// <summary>
/// Represents a compiled fragment (pixel) shader.
/// </summary>
public interface IGpuFragmentShader : IGpuShader { }

/// <summary>
/// Represents a GPU texture that can be used as a render target.
/// </summary>
public interface IGpuTexture : IDisposable
{
    /// <summary>Gets the width of the texture in pixels.</summary>
    int Width { get; }

    /// <summary>Gets the height of the texture in pixels.</summary>
    int Height { get; }
}

/// <summary>
/// Represents a GPU render target backed by a texture.
/// Rendered output can be read back to CPU for display.
/// </summary>
public interface IGpuRenderTarget : IDisposable
{
    /// <summary>Gets the underlying texture.</summary>
    IGpuTexture Texture { get; }

    /// <summary>Gets the width of the render target in pixels.</summary>
    int Width { get; }

    /// <summary>Gets the height of the render target in pixels.</summary>
    int Height { get; }

    /// <summary>
    /// Reads back the rendered pixels to a CPU byte array.
    /// The pixel format is BGRA (Blue, Green, Red, Alpha), 4 bytes per pixel.
    /// </summary>
    /// <param name="destination">
    /// A byte array of size Width * Height * 4 to receive the pixels.
    /// </param>
    void ReadbackPixelsBgra(byte[] destination);
}

/// <summary>
/// Describes the vertex buffer layout for a GPU draw call.
/// </summary>
public sealed class GpuVertexBufferDesc
{
    /// <summary>Gets or sets the stride (bytes per vertex).</summary>
    public int Stride { get; set; }

    /// <summary>Gets or sets the vertex count.</summary>
    public int VertexCount { get; set; }
}

/// <summary>
/// Describes a hardware GPU render pipeline, binding shaders and state together.
/// </summary>
public sealed class GpuPipelineDesc
{
    /// <summary>Gets or sets the compiled vertex shader.</summary>
    public IGpuVertexShader? VertexShader { get; set; }

    /// <summary>Gets or sets the compiled fragment (pixel) shader.</summary>
    public IGpuFragmentShader? FragmentShader { get; set; }

    /// <summary>Gets or sets whether depth testing is enabled.</summary>
    public bool DepthTestEnabled { get; set; } = true;

    /// <summary>Gets or sets whether depth writing is enabled.</summary>
    public bool DepthWriteEnabled { get; set; } = true;
}

/// <summary>
/// Represents a GPU command list / draw context for recording rendering commands.
/// </summary>
public interface IGpuCommandList : IDisposable
{
    /// <summary>
    /// Begins a render pass targeting the given render target.
    /// </summary>
    void BeginRenderPass(IGpuRenderTarget renderTarget, GpuClearColor clearColor);

    /// <summary>
    /// Ends the current render pass.
    /// </summary>
    void EndRenderPass();

    /// <summary>
    /// Sets the pipeline state for subsequent draw calls.
    /// </summary>
    void SetPipeline(GpuPipelineDesc pipeline);

    /// <summary>
    /// Sets the vertex buffer for subsequent draw calls.
    /// </summary>
    /// <param name="data">Pointer to vertex data.</param>
    /// <param name="desc">Vertex buffer description.</param>
    unsafe void SetVertexBuffer(void* data, GpuVertexBufferDesc desc);

    /// <summary>
    /// Sets the uniform/constant buffer data.
    /// </summary>
    /// <param name="data">Pointer to constant data.</param>
    /// <param name="sizeInBytes">Size of the constant data in bytes.</param>
    unsafe void SetConstantBuffer(void* data, int sizeInBytes);

    /// <summary>
    /// Draws primitives using the current pipeline and vertex buffer.
    /// </summary>
    /// <param name="vertexCount">Number of vertices to draw.</param>
    void Draw(int vertexCount);

    /// <summary>
    /// Submits all recorded commands for execution and waits for completion.
    /// </summary>
    void Commit();
}

/// <summary>
/// Clear color used when beginning a render pass.
/// </summary>
public readonly struct GpuClearColor
{
    /// <summary>Red component (0.0 to 1.0).</summary>
    public readonly float R;
    /// <summary>Green component (0.0 to 1.0).</summary>
    public readonly float G;
    /// <summary>Blue component (0.0 to 1.0).</summary>
    public readonly float B;
    /// <summary>Alpha component (0.0 to 1.0).</summary>
    public readonly float A;

    /// <summary>Creates a new GpuClearColor.</summary>
    public GpuClearColor(float r, float g, float b, float a)
    {
        R = r;
        G = g;
        B = b;
        A = a;
    }

    /// <summary>Transparent black.</summary>
    public static GpuClearColor Black => new(0f, 0f, 0f, 1f);
}

/// <summary>
/// Abstract interface for a GPU hardware device.
/// Implementations exist for Direct3D 11 (Windows) and Metal (macOS/iOS).
/// </summary>
public interface IGpuDevice : IDisposable
{
    /// <summary>Gets the name of this GPU backend (e.g., "D3D11", "Metal").</summary>
    string BackendName { get; }

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
