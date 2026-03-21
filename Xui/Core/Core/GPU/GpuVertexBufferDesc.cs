namespace Xui.GPU.Hardware;

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
