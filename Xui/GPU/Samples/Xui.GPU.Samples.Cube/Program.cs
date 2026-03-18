using Xui.GPU.Shaders;
using Xui.GPU.Shaders.Types;
using Xui.GPU.Software;
using Xui.Runtime.Software;

namespace Xui.GPU.Samples.Cube;

/// <summary>
/// Demonstration: Render a rotating 3D cube to an RGBA bitmap using CPU software rendering with depth testing.
/// This extends the triangle sample to show 3D transformations and proper depth ordering.
/// </summary>
class Program
{
    static unsafe void Main(string[] args)
    {
        Console.WriteLine("Xui.GPU Cube Sample - 3D Rendering with Depth Testing");
        Console.WriteLine("======================================================");
        Console.WriteLine();
        
        const int width = 512;
        const int height = 512;
        
        // Create framebuffer with depth buffer (essential for 3D rendering)
        using var framebuffer = new Framebuffer(width, height, withDepthBuffer: true);
        var context = new RenderContext(framebuffer);
        
        // Enable depth testing for proper 3D rendering
        context.DepthTestEnabled = true;
        
        // Clear to dark background
        Console.WriteLine($"Creating {width}x{height} framebuffer with depth buffer...");
        context.ClearColor(new Color4(new F32(0.15f), new F32(0.15f), new F32(0.2f), new F32(1.0f)));
        context.ClearDepth(1.0f);
        
        // Create cube vertices (8 corners of a unit cube centered at origin)
        var cubeVertices = CreateCubeVertices();
        
        Console.WriteLine($"Rendering cube with {cubeVertices.Length / 3} triangles...");
        
        // Setup transformation matrices
        float rotationAngle = MathF.PI / 6.0f; // 30 degrees
        const float cubeScale = 0.6f; // Scale for reasonable screen coverage
        
        // Model matrix: Scale down, then rotate around Y and X axes
        var scaleMatrix = new Float4x4(
            new Float4(new F32(cubeScale), F32.Zero, F32.Zero, F32.Zero),
            new Float4(F32.Zero, new F32(cubeScale), F32.Zero, F32.Zero),
            new Float4(F32.Zero, F32.Zero, new F32(cubeScale), F32.Zero),
            new Float4(F32.Zero, F32.Zero, F32.Zero, F32.One)
        );
        
        var modelMatrix = Float4x4.CreateRotationY(new F32(rotationAngle)) * 
                         Float4x4.CreateRotationX(new F32(rotationAngle * 0.5f)) *
                         scaleMatrix;
        
        // View matrix: Camera at (0, 0, 5) looking at origin
        var viewMatrix = Float4x4.CreateLookAt(
            new Float3(F32.Zero, F32.Zero, new F32(5.0f)),  // Eye position
            new Float3(F32.Zero, F32.Zero, F32.Zero),       // Look at target
            new Float3(F32.Zero, F32.One, F32.Zero)         // Up vector
        );
        
        // Projection matrix: Perspective projection
        var projectionMatrix = Float4x4.CreatePerspective(
            new F32(MathF.PI / 4.0f),  // 45 degree FOV
            new F32((float)width / height),  // Aspect ratio
            new F32(0.1f),  // Near plane
            new F32(100.0f)  // Far plane
        );
        
        // Combine transformations: MVP = Projection * View * Model
        var mvpMatrix = projectionMatrix * viewMatrix * modelMatrix;
        
        // Create bindings with the MVP matrix
        var bindings = new CubeBindings { MVP = mvpMatrix };
        
        // Pin vertex array and render
        fixed (CubeVertex* ptr = cubeVertices)
        {
            var vertexSource = new VertexSource<CubeVertex>(ptr, cubeVertices.Length);
            var vertexShader = new CubeVertexShader();
            var fragmentShader = new CubeFragmentShader();
            
            // Draw the cube (36 vertices = 12 triangles = 6 faces)
            context.Draw(vertexSource, vertexShader, fragmentShader, bindings);
        }
        
        Console.WriteLine("Cube rendered successfully!");
        
        // Convert framebuffer to RGBA byte array and save as PNG
        Console.WriteLine("Converting to PNG format...");
        
        byte[] rgbaPixels = new byte[width * height * 4];
        int index = 0;
        
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
                
                rgbaPixels[index++] = r;
                rgbaPixels[index++] = g;
                rgbaPixels[index++] = b;
                rgbaPixels[index++] = a;
            }
        }
        
        var outputPath = "cube_output.png";
        PngEncoder.SaveRGBA(outputPath, rgbaPixels, width, height);
        Console.WriteLine($"Output saved to: {Path.GetFullPath(outputPath)}");
        
        Console.WriteLine();
        Console.WriteLine("Success! The cube has been rendered using:");
        Console.WriteLine("  ✓ 3D vertex transformations (Model-View-Projection)");
        Console.WriteLine("  ✓ Perspective projection");
        Console.WriteLine("  ✓ CPU software rasterization with depth testing");
        Console.WriteLine("  ✓ Proper depth ordering of overlapping faces");
        Console.WriteLine("  ✓ Per-face color interpolation");
        Console.WriteLine();
        Console.WriteLine("This demonstrates a complete 3D rendering pipeline!");
    }
    
    /// <summary>
    /// Creates vertices for a unit cube with 6 colored faces.
    /// Each face has 2 triangles (6 vertices per face, 36 total).
    /// </summary>
    static CubeVertex[] CreateCubeVertices()
    {
        // Define 8 corner positions of a unit cube
        var positions = new Float3[]
        {
            new Float3(new F32(-1.0f), new F32(-1.0f), new F32(-1.0f)), // 0: left-bottom-back
            new Float3(new F32( 1.0f), new F32(-1.0f), new F32(-1.0f)), // 1: right-bottom-back
            new Float3(new F32( 1.0f), new F32( 1.0f), new F32(-1.0f)), // 2: right-top-back
            new Float3(new F32(-1.0f), new F32( 1.0f), new F32(-1.0f)), // 3: left-top-back
            new Float3(new F32(-1.0f), new F32(-1.0f), new F32( 1.0f)), // 4: left-bottom-front
            new Float3(new F32( 1.0f), new F32(-1.0f), new F32( 1.0f)), // 5: right-bottom-front
            new Float3(new F32( 1.0f), new F32( 1.0f), new F32( 1.0f)), // 6: right-top-front
            new Float3(new F32(-1.0f), new F32( 1.0f), new F32( 1.0f)), // 7: left-top-front
        };
        
        // Define colors for each face
        var red = new Color4(new F32(1.0f), new F32(0.0f), new F32(0.0f), new F32(1.0f));
        var green = new Color4(new F32(0.0f), new F32(1.0f), new F32(0.0f), new F32(1.0f));
        var blue = new Color4(new F32(0.0f), new F32(0.0f), new F32(1.0f), new F32(1.0f));
        var yellow = new Color4(new F32(1.0f), new F32(1.0f), new F32(0.0f), new F32(1.0f));
        var magenta = new Color4(new F32(1.0f), new F32(0.0f), new F32(1.0f), new F32(1.0f));
        var cyan = new Color4(new F32(0.0f), new F32(1.0f), new F32(1.0f), new F32(1.0f));
        
        // Build vertices for each face (2 triangles per face)
        var vertices = new List<CubeVertex>();
        
        // Front face (z = 1) - Red
        AddQuad(vertices, positions[4], positions[5], positions[6], positions[7], red);
        
        // Back face (z = -1) - Green
        AddQuad(vertices, positions[1], positions[0], positions[3], positions[2], green);
        
        // Top face (y = 1) - Blue
        AddQuad(vertices, positions[3], positions[7], positions[6], positions[2], blue);
        
        // Bottom face (y = -1) - Yellow
        AddQuad(vertices, positions[4], positions[0], positions[1], positions[5], yellow);
        
        // Right face (x = 1) - Magenta
        AddQuad(vertices, positions[5], positions[1], positions[2], positions[6], magenta);
        
        // Left face (x = -1) - Cyan
        AddQuad(vertices, positions[0], positions[4], positions[7], positions[3], cyan);
        
        return vertices.ToArray();
    }
    
    /// <summary>
    /// Adds two triangles to form a quad.
    /// </summary>
    static void AddQuad(List<CubeVertex> vertices, Float3 p0, Float3 p1, Float3 p2, Float3 p3, Color4 color)
    {
        // Triangle 1: p0, p1, p2
        vertices.Add(new CubeVertex { Position = p0, Color = color });
        vertices.Add(new CubeVertex { Position = p1, Color = color });
        vertices.Add(new CubeVertex { Position = p2, Color = color });
        
        // Triangle 2: p0, p2, p3
        vertices.Add(new CubeVertex { Position = p0, Color = color });
        vertices.Add(new CubeVertex { Position = p2, Color = color });
        vertices.Add(new CubeVertex { Position = p3, Color = color });
    }
}

// === Shader Definitions ===

/// <summary>
/// Vertex input structure for the cube.
/// </summary>
public struct CubeVertex
{
    public Float3 Position;
    public Color4 Color;
}

/// <summary>
/// Varyings passed from vertex to fragment shader.
/// </summary>
public struct CubeVaryings
{
    public Float4 Position;
    public Color4 Color;
}

/// <summary>
/// Bindings containing transformation matrices.
/// </summary>
public struct CubeBindings
{
    public Float4x4 MVP;
}

/// <summary>
/// Vertex shader: transforms 3D positions to clip space.
/// </summary>
public readonly struct CubeVertexShader
    : IVertexShader<CubeVertex, CubeVaryings, CubeBindings>
{
    public CubeVaryings Execute(CubeVertex input, in CubeBindings bindings)
    {
        // Transform position from local space to clip space using MVP matrix
        var position4 = new Float4(input.Position, F32.One);
        var clipPosition = bindings.MVP * position4;
        
        return new CubeVaryings
        {
            Position = clipPosition,
            Color = input.Color
        };
    }
}

/// <summary>
/// Fragment shader: outputs interpolated colors.
/// </summary>
public readonly struct CubeFragmentShader
    : IFragmentShader<CubeVaryings, FragmentOutput, CubeBindings>
{
    public FragmentOutput Execute(CubeVaryings input, in CubeBindings bindings)
    {
        return new FragmentOutput
        {
            Color = input.Color
        };
    }
}
