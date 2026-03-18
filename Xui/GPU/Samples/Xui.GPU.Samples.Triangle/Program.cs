using Xui.GPU.Shaders;
using Xui.GPU.Shaders.Types;
using Xui.GPU.Software;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace Xui.GPU.Samples.Triangle;

/// <summary>
/// MVP demonstration: Render a triangle to an RGBA bitmap using CPU software rendering.
/// This is the first working end-to-end example of the Xui.GPU pipeline.
/// </summary>
class Program
{
    static unsafe void Main(string[] args)
    {
        Console.WriteLine("Xui.GPU Triangle Sample - MVP");
        Console.WriteLine("==============================");
        Console.WriteLine();
        
        const int width = 512;
        const int height = 512;
        
        // Create framebuffer with depth buffer
        using var framebuffer = new Framebuffer(width, height, withDepthBuffer: true);
        var context = new RenderContext(framebuffer);
        
        // Clear to dark gray background
        Console.WriteLine($"Creating {width}x{height} framebuffer with depth buffer...");
        context.ClearColor(new Color4(new F32(0.1f), new F32(0.1f), new F32(0.1f), new F32(1.0f)));
        context.ClearDepth(1.0f);
        
        // Define triangle vertices (in normalized device coordinates)
        var vertices = new TriangleVertex[]
        {
            // Top vertex - Red
            new TriangleVertex 
            { 
                Position = new Float2(new F32(0.0f), new F32(-0.6f)),
                Color = new Color4(new F32(1.0f), new F32(0.0f), new F32(0.0f), new F32(1.0f))
            },
            // Bottom left - Green
            new TriangleVertex 
            { 
                Position = new Float2(new F32(-0.6f), new F32(0.6f)),
                Color = new Color4(new F32(0.0f), new F32(1.0f), new F32(0.0f), new F32(1.0f))
            },
            // Bottom right - Blue
            new TriangleVertex 
            { 
                Position = new Float2(new F32(0.6f), new F32(0.6f)),
                Color = new Color4(new F32(0.0f), new F32(0.0f), new F32(1.0f), new F32(1.0f))
            }
        };
        
        Console.WriteLine("Rendering triangle with color interpolation...");
        
        // Pin vertex array and create vertex source
        fixed (TriangleVertex* ptr = vertices)
        {
            var vertexSource = new VertexSource<TriangleVertex>(ptr, vertices.Length);
            var vertexShader = new TriangleVertexShader();
            var fragmentShader = new TriangleFragmentShader();
            var bindings = new EmptyBindings();
            
            // Draw the triangle
            context.Draw(vertexSource, vertexShader, fragmentShader, bindings);
        }
        
        Console.WriteLine("Triangle rendered successfully!");
        
        // Convert framebuffer to ImageSharp format and save as PNG
        Console.WriteLine("Converting to PNG format...");
        
        using (var image = new Image<Rgba32>(width, height))
        {
            // Copy framebuffer data directly to image (no double conversion)
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    var pixelRgba32 = framebuffer.GetColor(x, y);
                    
                    // Extract RGBA bytes from packed uint (little-endian: ABGR in memory)
                    byte a = (byte)(pixelRgba32 & 0xFF);
                    byte b = (byte)((pixelRgba32 >> 8) & 0xFF);
                    byte g = (byte)((pixelRgba32 >> 16) & 0xFF);
                    byte r = (byte)((pixelRgba32 >> 24) & 0xFF);
                    
                    image[x, y] = new Rgba32(r, g, b, a);
                }
            }
            
            var outputPath = "triangle_output.png";
            image.SaveAsPng(outputPath);
            Console.WriteLine($"Output saved to: {Path.GetFullPath(outputPath)}");
        }
        
        Console.WriteLine();
        Console.WriteLine("Success! The triangle has been rendered using:");
        Console.WriteLine("  ✓ C# authored vertex and fragment shaders");
        Console.WriteLine("  ✓ CPU software rasterization with depth testing");
        Console.WriteLine("  ✓ Barycentric coordinate interpolation");
        Console.WriteLine("  ✓ RGBA color output");
        Console.WriteLine();
        Console.WriteLine("This demonstrates a complete working Xui.GPU pipeline!");
    }
}

// === Shader Definitions (will be implemented in Phase 2-4) ===

/// <summary>
/// Vertex input structure for the triangle.
/// </summary>
public struct TriangleVertex
{
    // [Location(0)]
    public Float2 Position;
    
    // [Location(1)]
    public Color4 Color;
}

/// <summary>
/// Varyings passed from vertex to fragment shader.
/// </summary>
public struct TriangleVaryings
{
    // [BuiltIn(BuiltIn.Position)]
    public Float4 Position;
    
    // [Location(0)]
    public Color4 Color;
}

/// <summary>
/// Vertex shader: transforms vertex positions.
/// </summary>
public readonly struct TriangleVertexShader
    : IVertexShader<TriangleVertex, TriangleVaryings, EmptyBindings>
{
    public TriangleVaryings Execute(TriangleVertex input, in EmptyBindings bindings)
    {
        return new TriangleVaryings
        {
            Position = new Float4(input.Position, F32.Zero, F32.One),
            Color = input.Color
        };
    }
}

/// <summary>
/// Fragment shader: outputs interpolated colors.
/// </summary>
public readonly struct TriangleFragmentShader
    : IFragmentShader<TriangleVaryings, FragmentOutput, EmptyBindings>
{
    public FragmentOutput Execute(TriangleVaryings input, in EmptyBindings bindings)
    {
        return new FragmentOutput
        {
            Color = input.Color
        };
    }
}

/// <summary>
/// Placeholder for empty bindings.
/// </summary>
public struct EmptyBindings
{
}
