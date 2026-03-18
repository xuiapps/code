namespace Xui.GPU.Software;

/// <summary>
/// Defines the primitive topology for vertex assembly.
/// </summary>
public enum PrimitiveTopology
{
    /// <summary>
    /// Vertices are assembled into triangles.
    /// Each group of 3 vertices forms a triangle.
    /// </summary>
    TriangleList = 0,

    /// <summary>
    /// Vertices form a strip of triangles where each vertex (after the first two)
    /// forms a triangle with the two preceding vertices.
    /// </summary>
    TriangleStrip = 1,

    /// <summary>
    /// Individual line segments. Each group of 2 vertices forms a line.
    /// </summary>
    LineList = 2,

    /// <summary>
    /// Connected line segments where each vertex (after the first)
    /// forms a line with the preceding vertex.
    /// </summary>
    LineStrip = 3,

    /// <summary>
    /// Individual points. Each vertex is rendered as a point.
    /// </summary>
    PointList = 4,
}
