using Xui.GPU.Backends.Hlsl;

namespace Xui.GPU.Samples.HlslGen;

/// <summary>
/// Sample program demonstrating HLSL shader generation from IR.
/// This shows Phase 5 (IR) and Phase 12 (HLSL Backend) working together.
/// </summary>
class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Xui.GPU HLSL Code Generation Sample");
        Console.WriteLine("====================================");
        Console.WriteLine();
        Console.WriteLine("Generating HLSL shader code from Triangle shader IR...");
        Console.WriteLine();
        
        // Create the triangle shader IR module
        var module = IrBuilder.CreateTriangleShaderModule();
        
        // Create HLSL code generator
        var hlslGenerator = new HlslCodeGenerator();
        
        // Generate HLSL code
        var hlslCode = hlslGenerator.GenerateCode(module);
        
        // Display the generated HLSL
        Console.WriteLine("Generated HLSL Code:");
        Console.WriteLine("--------------------");
        Console.WriteLine(hlslCode);
        
        // Save to file
        var outputPath = "triangle_shader.hlsl";
        File.WriteAllText(outputPath, hlslCode);
        
        Console.WriteLine($"HLSL code saved to: {Path.GetFullPath(outputPath)}");
        Console.WriteLine();
        Console.WriteLine("Success! HLSL backend is working.");
        Console.WriteLine();
        Console.WriteLine("Next steps:");
        Console.WriteLine("  • Compile with fxc.exe or dxc.exe");
        Console.WriteLine("  • Integrate with D3D12 hardware backend");
        Console.WriteLine("  • Add Roslyn analyzer for shader validation");
    }
}
