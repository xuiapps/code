using Xui.GPU.Shaders;
using Xui.GPU.Shaders.Types;

namespace Xui.GPU.Samples.Triangle;

/// <summary>
/// MVP demonstration: Render a triangle to an RGBA bitmap using CPU software rendering.
/// This will be the first working end-to-end example of the Xui.GPU pipeline.
/// </summary>
class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Xui.GPU Triangle Sample - MVP");
        Console.WriteLine("==============================");
        Console.WriteLine();
        Console.WriteLine("This sample will render a triangle using:");
        Console.WriteLine("- C# authored vertex and fragment shaders");
        Console.WriteLine("- CPU software rasterization");
        Console.WriteLine("- RGBA bitmap output with depth buffer");
        Console.WriteLine();
        Console.WriteLine("TODO: Implementation will be completed in subsequent phases:");
        Console.WriteLine("  Phase 1-4: Type system and shader interfaces");
        Console.WriteLine("  Phase 8-10: Software renderer");
        Console.WriteLine("  Phase 11: Triangle demo implementation");
        Console.WriteLine();
        Console.WriteLine("Current status: Project structure created ✓");
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
