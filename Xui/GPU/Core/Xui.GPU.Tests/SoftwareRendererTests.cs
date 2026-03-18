using Xunit;
using Xui.GPU.Software;
using Xui.GPU.Shaders;
using Xui.GPU.Shaders.Types;

namespace Xui.GPU.Tests;

public class SoftwareRendererTests
{
    [Fact]
    public unsafe void Framebuffer_CreateAndDispose_ShouldAllocateMemory()
    {
        using var fb = new Framebuffer(100, 100, withDepthBuffer: true);
        
        Assert.Equal(100, fb.Width);
        Assert.Equal(100, fb.Height);
        Assert.True(fb.HasDepthBuffer);
        Assert.True(fb.ColorData != null);
        Assert.True(fb.DepthData != null);
    }

    [Fact]
    public unsafe void Framebuffer_WithoutDepthBuffer_ShouldNotAllocateDepth()
    {
        using var fb = new Framebuffer(50, 50, withDepthBuffer: false);
        
        Assert.Equal(50, fb.Width);
        Assert.Equal(50, fb.Height);
        Assert.False(fb.HasDepthBuffer);
        Assert.True(fb.ColorData != null);
        Assert.True(fb.DepthData == null);
    }

    [Fact]
    public void Framebuffer_InvalidDimensions_ShouldThrow()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => new Framebuffer(0, 100));
        Assert.Throws<ArgumentOutOfRangeException>(() => new Framebuffer(100, 0));
        Assert.Throws<ArgumentOutOfRangeException>(() => new Framebuffer(-1, 100));
    }

    [Fact]
    public void Framebuffer_ClearColor_ShouldSetAllPixels()
    {
        using var fb = new Framebuffer(10, 10);
        fb.ClearColor(0xFF0000FF); // Red

        for (int y = 0; y < 10; y++)
        {
            for (int x = 0; x < 10; x++)
            {
                Assert.Equal(0xFF0000FFu, fb.GetColor(x, y));
            }
        }
    }

    [Fact]
    public void Framebuffer_ClearDepth_ShouldSetAllDepths()
    {
        using var fb = new Framebuffer(10, 10, withDepthBuffer: true);
        fb.ClearDepth(0.5f);

        for (int y = 0; y < 10; y++)
        {
            for (int x = 0; x < 10; x++)
            {
                Assert.Equal(0.5f, fb.GetDepth(x, y), precision: 5);
            }
        }
    }

    [Fact]
    public void Framebuffer_SetGetColor_ShouldRoundTrip()
    {
        using var fb = new Framebuffer(10, 10);
        
        fb.SetColor(5, 5, 0x12345678u);
        Assert.Equal(0x12345678u, fb.GetColor(5, 5));
    }

    [Fact]
    public void Framebuffer_SetGetDepth_ShouldRoundTrip()
    {
        using var fb = new Framebuffer(10, 10, withDepthBuffer: true);
        
        fb.SetDepth(5, 5, 0.75f);
        Assert.Equal(0.75f, fb.GetDepth(5, 5), precision: 5);
    }

    [Fact]
    public void Framebuffer_OutOfBounds_ShouldNotCrash()
    {
        using var fb = new Framebuffer(10, 10);
        
        // Should not throw or crash
        fb.SetColor(-1, 5, 0xFF0000FF);
        fb.SetColor(10, 5, 0xFF0000FF);
        fb.SetColor(5, -1, 0xFF0000FF);
        fb.SetColor(5, 10, 0xFF0000FF);
        
        Assert.Equal(0u, fb.GetColor(-1, 5));
        Assert.Equal(0u, fb.GetColor(10, 5));
    }

    [Fact]
    public void Framebuffer_ToByteArray_ShouldCopyData()
    {
        using var fb = new Framebuffer(2, 2);
        fb.SetColor(0, 0, 0xFF0000AA);
        fb.SetColor(1, 0, 0x00FF00BB);
        fb.SetColor(0, 1, 0x0000FFCC);
        fb.SetColor(1, 1, 0xFFFFFFDD);

        var bytes = fb.ToByteArray();
        
        Assert.Equal(2 * 2 * 4, bytes.Length);
        
        // Check first pixel - stored as uint 0xFF0000AA in little-endian byte order
        // On little-endian: least significant byte first
        Assert.Equal(0xAA, bytes[0]);  // A (least significant byte of 0xFF0000AA)
        Assert.Equal(0x00, bytes[1]);  // B
        Assert.Equal(0x00, bytes[2]);  // G
        Assert.Equal(0xFF, bytes[3]);  // R (most significant byte)
    }

    [Fact]
    public void ColorTarget_ToRgba32_ShouldConvertCorrectly()
    {
        var color = new Color4(new F32(1.0f), new F32(0.5f), new F32(0.0f), new F32(1.0f));
        var rgba32 = ColorTarget.ToRgba32(color);
        
        // Expected: R=255, G=127, B=0, A=255 -> 0xFF7F00FF
        Assert.Equal(0xFF7F00FFu, rgba32);
    }

    [Fact]
    public void ColorTarget_FromRgba32_ShouldConvertCorrectly()
    {
        var color = ColorTarget.FromRgba32(0xFF7F00FF);
        
        Assert.Equal(1.0f, (float)color.R, precision: 2);
        Assert.Equal(0.498f, (float)color.G, precision: 2); // 127/255 ≈ 0.498
        Assert.Equal(0.0f, (float)color.B, precision: 2);
        Assert.Equal(1.0f, (float)color.A, precision: 2);
    }

    [Fact]
    public void ColorTarget_RoundTrip_ShouldPreserveColor()
    {
        var original = new Color4(new F32(0.8f), new F32(0.6f), new F32(0.4f), new F32(0.2f));
        var rgba32 = ColorTarget.ToRgba32(original);
        var converted = ColorTarget.FromRgba32(rgba32);
        
        // Allow small precision loss from 8-bit conversion
        Assert.Equal((float)original.R, (float)converted.R, precision: 2);
        Assert.Equal((float)original.G, (float)converted.G, precision: 2);
        Assert.Equal((float)original.B, (float)converted.B, precision: 2);
        Assert.Equal((float)original.A, (float)converted.A, precision: 2);
    }

    [Fact]
    public void ColorTarget_Blend_ShouldBlendCorrectly()
    {
        var src = new Color4(new F32(1.0f), new F32(0.0f), new F32(0.0f), new F32(0.5f)); // 50% opaque red
        var dst = new Color4(new F32(0.0f), new F32(1.0f), new F32(0.0f), new F32(1.0f)); // Green

        var blended = ColorTarget.Blend(src, dst);

        // Expected: 0.5 * red + 0.5 * green = (0.5, 0.5, 0, ...)
        Assert.Equal(0.5f, (float)blended.R, precision: 2);
        Assert.Equal(0.5f, (float)blended.G, precision: 2);
        Assert.Equal(0.0f, (float)blended.B, precision: 2);
    }

    [Fact]
    public unsafe void VertexSource_FromArray_ShouldCreateSource()
    {
        var vertices = new[] { 1, 2, 3, 4, 5 };
        
        fixed (int* ptr = vertices)
        {
            var source = new VertexSource<int>(ptr, vertices.Length);
            
            Assert.Equal(5, source.Count);
            Assert.Equal(1, source[0]);
            Assert.Equal(3, source[2]);
            Assert.Equal(5, source[4]);
        }
    }

    [Fact]
    public unsafe void VertexSource_OutOfBounds_ShouldThrow()
    {
        var vertices = new[] { 1, 2, 3 };
        
        fixed (int* ptr = vertices)
        {
            var source = new VertexSource<int>(ptr, vertices.Length);
            
            Assert.Throws<IndexOutOfRangeException>(() => source[-1]);
            Assert.Throws<IndexOutOfRangeException>(() => source[3]);
        }
    }

    [Fact]
    public unsafe void VertexSource_AsSpan_ShouldReturnSpan()
    {
        var vertices = new[] { 10, 20, 30 };
        
        fixed (int* ptr = vertices)
        {
            var source = new VertexSource<int>(ptr, vertices.Length);
            var span = source.AsSpan();
            
            Assert.Equal(3, span.Length);
            Assert.Equal(10, span[0]);
            Assert.Equal(30, span[2]);
        }
    }

    [Fact]
    public void Viewport_Creation_ShouldSetProperties()
    {
        var viewport = new Viewport(10, 20, 640, 480, 0.1f, 0.9f);
        
        Assert.Equal(10, viewport.X);
        Assert.Equal(20, viewport.Y);
        Assert.Equal(640, viewport.Width);
        Assert.Equal(480, viewport.Height);
        Assert.Equal(0.1f, viewport.MinDepth);
        Assert.Equal(0.9f, viewport.MaxDepth);
    }

    [Fact]
    public void Viewport_InvalidDimensions_ShouldThrow()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => new Viewport(0, 0, 0, 100));
        Assert.Throws<ArgumentOutOfRangeException>(() => new Viewport(0, 0, 100, 0));
    }

    [Fact]
    public void Viewport_FromFramebuffer_ShouldMatchDimensions()
    {
        using var fb = new Framebuffer(800, 600);
        var viewport = Viewport.FromFramebuffer(fb);
        
        Assert.Equal(0, viewport.X);
        Assert.Equal(0, viewport.Y);
        Assert.Equal(800, viewport.Width);
        Assert.Equal(600, viewport.Height);
    }

    [Fact]
    public void Viewport_Transform_ShouldMapNdcToScreen()
    {
        var viewport = new Viewport(0, 0, 100, 100);
        
        // Center of NDC space (-1 to +1) should map to center of viewport
        viewport.Transform(0.0f, 0.0f, 0.5f, out float x, out float y, out float z);
        
        Assert.Equal(50.0f, x, precision: 1);
        Assert.Equal(50.0f, y, precision: 1);
        Assert.Equal(0.5f, z, precision: 3);
    }

    [Fact]
    public void Viewport_Transform_LeftTopCorner()
    {
        var viewport = new Viewport(0, 0, 100, 100);
        
        viewport.Transform(-1.0f, -1.0f, 0.0f, out float x, out float y, out float z);
        
        Assert.Equal(0.0f, x, precision: 1);
        Assert.Equal(0.0f, y, precision: 1);
        Assert.Equal(0.0f, z, precision: 3);
    }

    [Fact]
    public void Viewport_Transform_RightBottomCorner()
    {
        var viewport = new Viewport(0, 0, 100, 100);
        
        viewport.Transform(1.0f, 1.0f, 1.0f, out float x, out float y, out float z);
        
        Assert.Equal(100.0f, x, precision: 1);
        Assert.Equal(100.0f, y, precision: 1);
        Assert.Equal(1.0f, z, precision: 3);
    }

    [Fact]
    public void RenderContext_Creation_ShouldInitialize()
    {
        using var fb = new Framebuffer(100, 100);
        var ctx = new RenderContext(fb);
        
        Assert.Equal(fb.Width, ctx.Viewport.Width);
        Assert.Equal(fb.Height, ctx.Viewport.Height);
        Assert.Equal(PrimitiveTopology.TriangleList, ctx.Topology);
        Assert.True(ctx.DepthTestEnabled);
        Assert.False(ctx.BlendEnabled);
    }

    [Fact]
    public void RenderContext_ClearColor_ShouldClearFramebuffer()
    {
        using var fb = new Framebuffer(10, 10);
        var ctx = new RenderContext(fb);
        
        var clearColor = new Color4(new F32(1.0f), new F32(0.0f), new F32(0.0f), new F32(1.0f));
        ctx.ClearColor(clearColor);
        
        // Check a few pixels
        var pixel = fb.GetColor(5, 5);
        var color = ColorTarget.FromRgba32(pixel);
        
        Assert.Equal(1.0f, (float)color.R, precision: 2);
        Assert.Equal(0.0f, (float)color.G, precision: 2);
        Assert.Equal(0.0f, (float)color.B, precision: 2);
    }

    [Fact]
    public void RenderContext_ClearDepth_ShouldClearDepthBuffer()
    {
        using var fb = new Framebuffer(10, 10, withDepthBuffer: true);
        var ctx = new RenderContext(fb);
        
        ctx.ClearDepth(0.3f);
        
        Assert.Equal(0.3f, fb.GetDepth(5, 5), precision: 3);
    }

    // Simple test shader implementations
    private struct SimpleVertex
    {
        public Float2 Position;
        public Color4 Color;
    }

    private struct SimpleVarying
    {
        public Float4 Position;
        public Color4 Color;
    }

    private struct EmptyBindings { }

    private struct SimpleVertexShader : IVertexShader<SimpleVertex, SimpleVarying, EmptyBindings>
    {
        public SimpleVarying Execute(SimpleVertex input, in EmptyBindings bindings)
        {
            return new SimpleVarying
            {
                Position = new Float4(input.Position.X, input.Position.Y, new F32(0.5f), new F32(1.0f)),
                Color = input.Color
            };
        }
    }

    private struct SimpleFragmentShader : IFragmentShader<SimpleVarying, FragmentOutput, EmptyBindings>
    {
        public FragmentOutput Execute(SimpleVarying input, in EmptyBindings bindings)
        {
            return new FragmentOutput
            {
                Color = input.Color
            };
        }
    }

    [Fact]
    public unsafe void RenderContext_DrawSimpleTriangle_ShouldRasterize()
    {
        using var fb = new Framebuffer(100, 100);
        var ctx = new RenderContext(fb);
        
        // Clear to black
        ctx.ClearColor(new Color4(new F32(0), new F32(0), new F32(0), new F32(1)));
        
        // Create a simple triangle covering the center
        var vertices = new SimpleVertex[]
        {
            new() { Position = new Float2(new F32(0.0f), new F32(-0.5f)), Color = new Color4(new F32(1), new F32(0), new F32(0), new F32(1)) },
            new() { Position = new Float2(new F32(-0.5f), new F32(0.5f)), Color = new Color4(new F32(0), new F32(1), new F32(0), new F32(1)) },
            new() { Position = new Float2(new F32(0.5f), new F32(0.5f)), Color = new Color4(new F32(0), new F32(0), new F32(1), new F32(1)) }
        };
        
        fixed (SimpleVertex* ptr = vertices)
        {
            var source = new VertexSource<SimpleVertex>(ptr, vertices.Length);
            var vertexShader = new SimpleVertexShader();
            var fragmentShader = new SimpleFragmentShader();
            var bindings = new EmptyBindings();
            
            ctx.Draw(source, vertexShader, fragmentShader, bindings);
        }
        
        // Check that center pixel is not black (triangle should cover it)
        var centerColor = ColorTarget.FromRgba32(fb.GetColor(50, 50));
        var isNotBlack = (float)centerColor.R > 0.01f || (float)centerColor.G > 0.01f || (float)centerColor.B > 0.01f;
        
        Assert.True(isNotBlack, "Center pixel should be colored by the triangle");
    }

    [Fact]
    public unsafe void RenderContext_DepthTest_ShouldRespectDepth()
    {
        using var fb = new Framebuffer(10, 10, withDepthBuffer: true);
        var ctx = new RenderContext(fb);
        ctx.DepthTestEnabled = true;
        
        ctx.ClearColor(new Color4(new F32(0), new F32(0), new F32(0), new F32(1)));
        ctx.ClearDepth(1.0f);
        
        // Draw a triangle - depth should be written
        var vertices = new SimpleVertex[]
        {
            new() { Position = new Float2(new F32(-0.5f), new F32(-0.5f)), Color = new Color4(new F32(1), new F32(0), new F32(0), new F32(1)) },
            new() { Position = new Float2(new F32(0.5f), new F32(-0.5f)), Color = new Color4(new F32(1), new F32(0), new F32(0), new F32(1)) },
            new() { Position = new Float2(new F32(0.0f), new F32(0.5f)), Color = new Color4(new F32(1), new F32(0), new F32(0), new F32(1)) }
        };
        
        fixed (SimpleVertex* ptr = vertices)
        {
            var source = new VertexSource<SimpleVertex>(ptr, vertices.Length);
            ctx.Draw(source, new SimpleVertexShader(), new SimpleFragmentShader(), new EmptyBindings());
        }
        
        // Center should have depth < 1.0 now
        var depth = fb.GetDepth(5, 5);
        Assert.True(depth < 1.0f, $"Depth should be less than 1.0, was {depth}");
    }
}
