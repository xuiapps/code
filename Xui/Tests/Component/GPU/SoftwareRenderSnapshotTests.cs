using System.Runtime.CompilerServices;
using Xui.GPU.Shaders;
using Xui.GPU.Shaders.Types;
using Xui.GPU.Software;
using Xui.Runtime.Software;

namespace Xui.Tests.Component.GPU;

/// <summary>
/// Snapshot tests for the GPU software renderer.
/// Renders known scenes and compares pixel output against reference PNG snapshots.
/// To regenerate snapshots, delete the reference PNGs and re-run the tests.
/// </summary>
public class SoftwareRenderSnapshotTests
{
    const int Size = 128;

    [Fact]
    public unsafe void Triangle_MatchesSnapshot()
    {
        using var fb = new Framebuffer(Size, Size);
        var ctx = new RenderContext(fb);
        ctx.CullMode = CullMode.None;

        ctx.ClearColor(new Color4(new F32(0.1f), new F32(0.1f), new F32(0.1f), F32.One));

        var vertices = new TriVertex[]
        {
            new() { Position = new Float2(new F32( 0.0f), new F32(-0.8f)), Color = new Color4(new F32(1), F32.Zero, F32.Zero, F32.One) },
            new() { Position = new Float2(new F32(-0.8f), new F32( 0.8f)), Color = new Color4(F32.Zero, new F32(1), F32.Zero, F32.One) },
            new() { Position = new Float2(new F32( 0.8f), new F32( 0.8f)), Color = new Color4(F32.Zero, F32.Zero, new F32(1), F32.One) },
        };

        fixed (TriVertex* ptr = vertices)
        {
            var source = new VertexSource<TriVertex>(ptr, vertices.Length);
            ctx.Draw(source, new TriVS(), new TriFS(), new NoBindings());
        }

        AssertSnapshot(fb, "Triangle");
    }

    [Fact]
    public unsafe void Cube_MatchesSnapshot()
    {
        using var fb = new Framebuffer(Size, Size, withDepthBuffer: true);
        var ctx = new RenderContext(fb);
        ctx.DepthTestEnabled = true;

        ctx.ClearColor(new Color4(new F32(0.5f), new F32(0.5f), new F32(0.5f), F32.One));
        ctx.ClearDepth(1.0f);

        var vertices = CreateCubeVertices();
        var mvp = ComputeCubeMvp();

        fixed (CubeVertex* ptr = vertices)
        {
            var source = new VertexSource<CubeVertex>(ptr, vertices.Length);
            ctx.Draw(source, new CubeVS(), new CubeFS(), new CubeBindings { MVP = mvp });
        }

        AssertSnapshot(fb, "Cube");
    }

    #region Snapshot comparison

    static void AssertSnapshot(Framebuffer fb, string name, [CallerFilePath] string filePath = "")
    {
        var snapshotDir = Path.Combine(Path.GetDirectoryName(filePath)!, "Snapshots");
        Directory.CreateDirectory(snapshotDir);

        var referencePath = Path.Combine(snapshotDir, $"{name}.png");
        var actualPath = Path.Combine(snapshotDir, $"{name}.Actual.png");

        var actualPixels = FramebufferToRgba(fb);

        // Always save the actual output for visual inspection
        PngEncoder.SaveRGBA(actualPath, actualPixels, fb.Width, fb.Height);

        if (!File.Exists(referencePath))
        {
            // First run — generate the reference snapshot
            PngEncoder.SaveRGBA(referencePath, actualPixels, fb.Width, fb.Height);
            Assert.Fail($"Reference snapshot '{name}.png' did not exist and was generated. " +
                        "Verify it visually and re-run the test.");
            return;
        }

        // Load reference and compare
        var referencePixels = PngDecoder.LoadRGBA(referencePath, out int refW, out int refH);

        Assert.Equal(fb.Width, refW);
        Assert.Equal(fb.Height, refH);

        int diffCount = 0;
        int tolerance = 2; // Allow ±2 per channel for rounding
        for (int i = 0; i < actualPixels.Length; i++)
        {
            if (Math.Abs(actualPixels[i] - referencePixels[i]) > tolerance)
                diffCount++;
        }

        if (diffCount > 0)
        {
            double diffPercent = 100.0 * diffCount / (fb.Width * fb.Height * 4);
            Assert.Fail($"Snapshot '{name}' differs in {diffCount} bytes ({diffPercent:F2}%). " +
                        $"Actual saved to '{actualPath}'. Delete reference and re-run to update.");
        }

        // Clean up actual file on success
        File.Delete(actualPath);
    }

    static unsafe byte[] FramebufferToRgba(Framebuffer fb)
    {
        var rgba = new byte[fb.Width * fb.Height * 4];
        uint* src = fb.ColorData;

        for (int i = 0; i < fb.Width * fb.Height; i++)
        {
            uint px = src[i];
            rgba[i * 4 + 0] = (byte)(px >> 24); // R
            rgba[i * 4 + 1] = (byte)(px >> 16); // G
            rgba[i * 4 + 2] = (byte)(px >> 8);  // B
            rgba[i * 4 + 3] = (byte)(px);       // A
        }

        return rgba;
    }

    #endregion

    #region Cube mesh and MVP

    static Float4x4 ComputeCubeMvp()
    {
        const float cubeScale = 0.6f;
        var scale = new Float4x4(
            new Float4(new F32(cubeScale), F32.Zero, F32.Zero, F32.Zero),
            new Float4(F32.Zero, new F32(cubeScale), F32.Zero, F32.Zero),
            new Float4(F32.Zero, F32.Zero, new F32(cubeScale), F32.Zero),
            new Float4(F32.Zero, F32.Zero, F32.Zero, F32.One));

        var model = Float4x4.CreateRotationY(new F32(0.6f))
                  * Float4x4.CreateRotationX(new F32(0.3f))
                  * scale;

        var view = Float4x4.CreateLookAt(
            new Float3(F32.Zero, F32.Zero, new F32(5.0f)),
            new Float3(F32.Zero, F32.Zero, F32.Zero),
            new Float3(F32.Zero, F32.One, F32.Zero));

        var proj = Float4x4.CreatePerspective(
            new F32(MathF.PI / 4.0f), new F32(1.0f), new F32(0.1f), new F32(100.0f));

        return proj * view * model;
    }

    static CubeVertex[] CreateCubeVertices()
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

        Color4 red = 0xFF0000FF, green = 0x008000FF, blue = 0x0000FFFF;
        Color4 yellow = 0xFFFF00FF, magenta = 0xFF00FFFF, cyan = 0x00FFFFFF;

        var v = new List<CubeVertex>();
        Quad(v, p[4], p[5], p[6], p[7], red);
        Quad(v, p[1], p[0], p[3], p[2], green);
        Quad(v, p[3], p[7], p[6], p[2], blue);
        Quad(v, p[4], p[0], p[1], p[5], yellow);
        Quad(v, p[5], p[1], p[2], p[6], magenta);
        Quad(v, p[0], p[4], p[7], p[3], cyan);
        return v.ToArray();
    }

    static void Quad(List<CubeVertex> v, Float3 p0, Float3 p1, Float3 p2, Float3 p3, Color4 c)
    {
        v.Add(new() { Position = p0, Color = c });
        v.Add(new() { Position = p1, Color = c });
        v.Add(new() { Position = p2, Color = c });
        v.Add(new() { Position = p0, Color = c });
        v.Add(new() { Position = p2, Color = c });
        v.Add(new() { Position = p3, Color = c });
    }

    #endregion

    #region Shader types

    struct TriVertex { public Float2 Position; public Color4 Color; }
    struct TriVarying { public Float4 Position; public Color4 Color; }
    struct CubeVertex { public Float3 Position; public Color4 Color; }
    struct CubeVarying { public Float4 Position; public Color4 Color; }
    struct CubeBindings { public Float4x4 MVP; }
    struct NoBindings { }

    readonly struct TriVS : IVertexShader<TriVertex, TriVarying, NoBindings>
    {
        public TriVarying Execute(TriVertex input, in NoBindings bindings) => new()
        {
            Position = new Float4(input.Position, F32.Zero, F32.One),
            Color = input.Color,
        };
    }

    readonly struct TriFS : IFragmentShader<TriVarying, FragmentOutput, NoBindings>
    {
        public FragmentOutput Execute(TriVarying input, in NoBindings bindings) =>
            new() { Color = input.Color };
    }

    readonly struct CubeVS : IVertexShader<CubeVertex, CubeVarying, CubeBindings>
    {
        public CubeVarying Execute(CubeVertex input, in CubeBindings bindings) => new()
        {
            Position = bindings.MVP * new Float4(input.Position, F32.One),
            Color = input.Color,
        };
    }

    readonly struct CubeFS : IFragmentShader<CubeVarying, FragmentOutput, CubeBindings>
    {
        public FragmentOutput Execute(CubeVarying input, in CubeBindings bindings) =>
            new() { Color = input.Color };
    }

    #endregion
}
