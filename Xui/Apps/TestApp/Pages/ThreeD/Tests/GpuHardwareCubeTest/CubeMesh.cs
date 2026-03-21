using Xui.GPU.Shaders.Types;
using static Xui.Core.Canvas.Colors;

namespace Xui.Apps.TestApp.Pages.ThreeD.Tests.GpuHardwareCubeTest;

/// <summary>
/// Cube mesh data: 36 vertices (6 faces × 2 triangles × 3 vertices).
/// </summary>
public static class CubeMesh
{
    public struct Vertex
    {
        public Float3 Position;
        public Color4 Color;
    }

    public static Vertex[] CreateVertices()
    {
        var p = new Float3[]
        {
            new(new F32(-1), new F32(-1), new F32(-1)),
            new(new F32( 1), new F32(-1), new F32(-1)),
            new(new F32( 1), new F32( 1), new F32(-1)),
            new(new F32(-1), new F32( 1), new F32(-1)),
            new(new F32(-1), new F32(-1), new F32( 1)),
            new(new F32( 1), new F32(-1), new F32( 1)),
            new(new F32( 1), new F32( 1), new F32( 1)),
            new(new F32(-1), new F32( 1), new F32( 1)),
        };

        var verts = new List<Vertex>();
        AddQuad(verts, p[4], p[5], p[6], p[7], Red);
        AddQuad(verts, p[1], p[0], p[3], p[2], Green);
        AddQuad(verts, p[3], p[7], p[6], p[2], Blue);
        AddQuad(verts, p[4], p[0], p[1], p[5], Yellow);
        AddQuad(verts, p[5], p[1], p[2], p[6], Magenta);
        AddQuad(verts, p[0], p[4], p[7], p[3], Cyan);
        return verts.ToArray();
    }

    private static void AddQuad(List<Vertex> verts, Float3 p0, Float3 p1, Float3 p2, Float3 p3, Color4 color)
    {
        verts.Add(new Vertex { Position = p0, Color = color });
        verts.Add(new Vertex { Position = p1, Color = color });
        verts.Add(new Vertex { Position = p2, Color = color });
        verts.Add(new Vertex { Position = p0, Color = color });
        verts.Add(new Vertex { Position = p2, Color = color });
        verts.Add(new Vertex { Position = p3, Color = color });
    }
}
