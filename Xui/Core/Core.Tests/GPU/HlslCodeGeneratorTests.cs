using Xunit;
using Xui.GPU.Backends.Hlsl;

namespace Xui.GPU.IR.Tests;

/// <summary>
/// Tests for HLSL code generation.
/// </summary>
public class HlslCodeGeneratorTests
{
    [Fact]
    public void HlslGenerator_HasCorrectName()
    {
        var generator = new HlslCodeGenerator();
        Assert.Equal("HLSL", generator.Name);
    }

    [Fact]
    public void HlslGenerator_TriangleShader_GeneratesValidStructs()
    {
        var module = IrBuilder.CreateTriangleShaderModule();
        var generator = new HlslCodeGenerator();
        
        var hlsl = generator.GenerateCode(module);
        
        // Verify struct declarations
        Assert.Contains("struct TriangleVertex", hlsl);
        Assert.Contains("struct TriangleVaryings", hlsl);
        Assert.Contains("struct FragmentOutput", hlsl);
    }

    [Fact]
    public void HlslGenerator_TriangleShader_GeneratesVertexShader()
    {
        var module = IrBuilder.CreateTriangleShaderModule();
        var generator = new HlslCodeGenerator();
        
        var hlsl = generator.GenerateCode(module);
        
        // Verify vertex shader
        Assert.Contains("TriangleVaryings VSMain(TriangleVertex input)", hlsl);
        Assert.Contains("float4(input.Position, 0.0f, 1.0f)", hlsl);
    }

    [Fact]
    public void HlslGenerator_TriangleShader_GeneratesFragmentShader()
    {
        var module = IrBuilder.CreateTriangleShaderModule();
        var generator = new HlslCodeGenerator();
        
        var hlsl = generator.GenerateCode(module);
        
        // Verify pixel (fragment) shader
        Assert.Contains("FragmentOutput PSMain(TriangleVaryings input)", hlsl);
        Assert.Contains("output.Color = input.Color", hlsl);
    }

    [Fact]
    public void HlslGenerator_TriangleShader_GeneratesSemantics()
    {
        var module = IrBuilder.CreateTriangleShaderModule();
        var generator = new HlslCodeGenerator();
        
        var hlsl = generator.GenerateCode(module);
        
        // Verify HLSL semantics
        Assert.Contains(": SV_POSITION", hlsl);
        Assert.Contains(": TEXCOORD0", hlsl);
        Assert.Contains(": TEXCOORD1", hlsl);
    }

    [Fact]
    public void HlslGenerator_TriangleShader_MapsTypesCorrectly()
    {
        var module = IrBuilder.CreateTriangleShaderModule();
        var generator = new HlslCodeGenerator();
        
        var hlsl = generator.GenerateCode(module);
        
        // Verify type mappings
        Assert.Contains("float2", hlsl);
        Assert.Contains("float4", hlsl);
    }

    [Fact]
    public void HlslGenerator_GeneratesModuleComment()
    {
        var module = IrBuilder.CreateTriangleShaderModule();
        var generator = new HlslCodeGenerator();
        
        var hlsl = generator.GenerateCode(module);
        
        // Verify header
        Assert.Contains("// Generated HLSL shader code", hlsl);
        Assert.Contains("// Module: TriangleShader", hlsl);
    }
}
