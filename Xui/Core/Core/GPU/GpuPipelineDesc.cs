namespace Xui.GPU.Hardware;

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

    /// <summary>Gets or sets the triangle cull mode.</summary>
    public GpuCullMode CullMode { get; set; } = GpuCullMode.Back;
}
