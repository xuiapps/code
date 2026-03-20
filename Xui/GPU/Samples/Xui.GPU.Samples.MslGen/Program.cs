using Xui.GPU.Backends.Metal;
using Xui.GPU.Backends.Hlsl;

namespace Xui.GPU.Samples.MslGen;

/// <summary>
/// Sample program demonstrating Metal Shading Language (MSL) shader generation from IR.
/// Also demonstrates side-by-side HLSL vs MSL output for the same shader.
/// This shows Phase 15a (HLSL backend) and Phase 15b (MSL backend) working together.
/// </summary>
class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Xui.GPU MSL Code Generation Sample");
        Console.WriteLine("===================================");
        Console.WriteLine();
        Console.WriteLine("Generating MSL and HLSL shader code from the same Triangle shader IR...");
        Console.WriteLine();

        // Create the triangle shader IR module
        var module = IrBuilder.CreateTriangleShaderModule();

        // Generate MSL (Metal Shading Language) code
        var mslGenerator = new MslCodeGenerator();
        var mslCode = mslGenerator.GenerateCode(module);

        // Generate HLSL for comparison
        var hlslGenerator = new HlslCodeGenerator();
        var hlslCode = hlslGenerator.GenerateCode(module);

        // Display generated MSL
        Console.WriteLine("=== Generated MSL Code (Apple Metal) ===");
        Console.WriteLine(mslCode);

        // Display generated HLSL
        Console.WriteLine("=== Generated HLSL Code (DirectX) ===");
        Console.WriteLine(hlslCode);

        // Save to files
        var mslOutputPath = "triangle_shader.metal";
        File.WriteAllText(mslOutputPath, mslCode);
        Console.WriteLine($"MSL code saved to: {Path.GetFullPath(mslOutputPath)}");

        var hlslOutputPath = "triangle_shader.hlsl";
        File.WriteAllText(hlslOutputPath, hlslCode);
        Console.WriteLine($"HLSL code saved to: {Path.GetFullPath(hlslOutputPath)}");

        Console.WriteLine();
        Console.WriteLine("Key differences between MSL and HLSL:");
        Console.WriteLine("  • MSL requires #include <metal_stdlib>");
        Console.WriteLine("  • MSL uses [[vertex]] and [[fragment]] attributes");
        Console.WriteLine("  • MSL uses [[stage_in]] for vertex input");
        Console.WriteLine("  • MSL uses [[position]] and [[user(locnN)]] instead of HLSL semantics");
        Console.WriteLine("  • MSL uses 'mix()' where HLSL uses 'lerp()'");
        Console.WriteLine("  • MSL uses 'dfdx/dfdy' where HLSL uses 'ddx/ddy'");
        Console.WriteLine("  • MSL uses 'fract()' where HLSL uses 'frac()'");
        Console.WriteLine();
        Console.WriteLine("Success! Both HLSL and MSL backends are working.");
    }
}
