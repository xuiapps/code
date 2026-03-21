namespace Xui.GPU.Hardware;

/// <summary>
/// Specifies which triangle faces to cull during rasterization.
/// </summary>
public enum GpuCullMode
{
    /// <summary>No culling — all triangles are rendered.</summary>
    None = 0,
    /// <summary>Back-facing triangles are culled (default).</summary>
    Back = 1,
    /// <summary>Front-facing triangles are culled.</summary>
    Front = 2,
}
