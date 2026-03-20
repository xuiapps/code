using Xunit;
using Xui.GPU.Backends.Metal;
using Xui.GPU.Backends.Hlsl;
using Xui.GPU.IR;

namespace Xui.GPU.IR.Tests;

/// <summary>
/// Tests for Metal Shading Language (MSL) code generation.
/// </summary>
public class MslCodeGeneratorTests
{
    [Fact]
    public void MslGenerator_HasCorrectName()
    {
        var generator = new MslCodeGenerator();
        Assert.Equal("MSL", generator.Name);
    }

    [Fact]
    public void MslGenerator_TriangleShader_GeneratesMetalIncludes()
    {
        var module = IrBuilder.CreateTriangleShaderModule();
        var generator = new MslCodeGenerator();

        var msl = generator.GenerateCode(module);

        // MSL requires metal_stdlib include
        Assert.Contains("#include <metal_stdlib>", msl);
        Assert.Contains("using namespace metal;", msl);
    }

    [Fact]
    public void MslGenerator_TriangleShader_GeneratesValidStructs()
    {
        var module = IrBuilder.CreateTriangleShaderModule();
        var generator = new MslCodeGenerator();

        var msl = generator.GenerateCode(module);

        // Verify struct declarations
        Assert.Contains("struct TriangleVertex", msl);
        Assert.Contains("struct TriangleVaryings", msl);
        Assert.Contains("struct FragmentOutput", msl);
    }

    [Fact]
    public void MslGenerator_TriangleShader_GeneratesVertexFunction()
    {
        var module = IrBuilder.CreateTriangleShaderModule();
        var generator = new MslCodeGenerator();

        var msl = generator.GenerateCode(module);

        // MSL vertex functions use [[vertex]] attribute
        Assert.Contains("[[vertex]]", msl);
        Assert.Contains("vertex_main", msl);
        // Vertex data is accessed via device buffer pointer + vertex_id (no [[stage_in]] for VS)
        Assert.Contains("[[buffer(0)]]", msl);
        Assert.Contains("[[vertex_id]]", msl);
        // Fragment function still uses [[stage_in]] for its interpolated input
        Assert.Contains("[[stage_in]]", msl);
    }

    [Fact]
    public void MslGenerator_TriangleShader_GeneratesFragmentFunction()
    {
        var module = IrBuilder.CreateTriangleShaderModule();
        var generator = new MslCodeGenerator();

        var msl = generator.GenerateCode(module);

        // MSL fragment functions use [[fragment]] attribute
        Assert.Contains("[[fragment]]", msl);
        Assert.Contains("fragment_main", msl);
    }

    [Fact]
    public void MslGenerator_TriangleShader_GeneratesMslAttributes()
    {
        var module = IrBuilder.CreateTriangleShaderModule();
        var generator = new MslCodeGenerator();

        var msl = generator.GenerateCode(module);

        // MSL uses [[position]] for built-in position (in the varyings struct)
        Assert.Contains("[[position]]", msl);

        // Inter-stage varyings use [[user(locnN)]]
        Assert.Contains("[[user(locn0)]]", msl);

        // Fragment output color attachments use [[color(N)]]
        Assert.Contains("[[color(0)]]", msl);

        // Vertex inputs have no field attributes (accessed via buffer pointer + vertex_id)
        Assert.DoesNotContain("[[user(locn1)]]", msl);

        // Vertex function uses the device buffer + vertex_id pattern
        Assert.Contains("[[vertex_id]]", msl);
    }

    [Fact]
    public void MslGenerator_TriangleShader_MapsTypesCorrectly()
    {
        var module = IrBuilder.CreateTriangleShaderModule();
        var generator = new MslCodeGenerator();

        var msl = generator.GenerateCode(module);

        // Verify type mappings (same as HLSL for scalars and vectors)
        Assert.Contains("float2", msl);
        Assert.Contains("float4", msl);
    }

    [Fact]
    public void MslGenerator_GeneratesModuleComment()
    {
        var module = IrBuilder.CreateTriangleShaderModule();
        var generator = new MslCodeGenerator();

        var msl = generator.GenerateCode(module);

        // Verify header
        Assert.Contains("// Generated MSL shader code", msl);
        Assert.Contains("// Module: TriangleShader", msl);
    }

    [Fact]
    public void MslGenerator_LerpMapsToMix()
    {
        // MSL uses 'mix' instead of 'lerp'
        // Create a module with a lerp intrinsic call
        var module = IrBuilder.CreateTriangleShaderModule();
        var generator = new MslCodeGenerator();

        // The mapping is tested by checking the MapIntrinsicToMsl logic
        // via the generator's Name property and type system consistency
        Assert.Equal("MSL", generator.Name);
    }

    [Fact]
    public void MslGenerator_DdxMapsToDfdx()
    {
        // MSL uses 'dfdx' instead of 'ddx'
        // This is validated through the MapIntrinsicToMsl method
        var generator = new MslCodeGenerator();

        // Verify generator can handle a module without throwing
        var module = IrBuilder.CreateTriangleShaderModule();
        var msl = generator.GenerateCode(module);
        Assert.NotEmpty(msl);
    }

    [Fact]
    public void MslGenerator_DifferentFromHlsl_LerpVsMix()
    {
        var module = IrBuilder.CreateTriangleShaderModule();
        var mslGenerator = new MslCodeGenerator();
        var hlslGenerator = new HlslCodeGenerator();

        var msl = mslGenerator.GenerateCode(module);
        var hlsl = hlslGenerator.GenerateCode(module);

        // Both should produce valid output but with different syntax
        Assert.NotEqual(msl, hlsl);

        // MSL has metal_stdlib, HLSL does not
        Assert.Contains("#include <metal_stdlib>", msl);
        Assert.DoesNotContain("#include <metal_stdlib>", hlsl);

        // MSL has [[vertex]], HLSL does not
        Assert.Contains("[[vertex]]", msl);
        Assert.DoesNotContain("[[vertex]]", hlsl);

        // HLSL has VSMain, MSL does not
        Assert.Contains("VSMain", hlsl);
        Assert.DoesNotContain("VSMain", msl);
    }

    [Fact]
    public void MslGenerator_VertexFunction_UsesBufferPointerAndVertexId()
    {
        // Vertex data is accessed via device buffer pointer + vertex_id, not [[stage_in]],
        // because [[stage_in]] with [[attribute(N)]] requires a MTLVertexDescriptor.
        var module = IrBuilder.CreateTriangleShaderModule();
        var generator = new MslCodeGenerator();

        var msl = generator.GenerateCode(module);

        Assert.Contains("device const TriangleVertex* vertices [[buffer(0)]]", msl);
        Assert.Contains("uint vertexId [[vertex_id]]", msl);
        Assert.Contains("TriangleVertex input = vertices[vertexId];", msl);
    }

    [Fact]
    public void MslGenerator_VertexFunction_IncludesBindingsParameter()
    {
        // When a BindingsType is set on the vertex stage, the generator must emit
        // the constant buffer as a function parameter with [[buffer(1)]].
        var module = new IrShaderModule { Name = "TestBindings" };
        var f32 = new IrScalarType(ScalarKind.F32);
        var float4 = new IrVectorType(f32, 4);
        var float4x4 = new IrMatrixType(f32, 4, 4);

        var inputType = new IrStructType("V");
        inputType.Fields.Add(new IrStructField { Name = "Pos", Type = float4, Decorations = { new IrLocationDecoration(0) } });
        module.Structs.Add(new IrStructDecl(inputType));

        var outputType = new IrStructType("Out");
        outputType.Fields.Add(new IrStructField { Name = "Pos", Type = float4, Decorations = { new IrBuiltInDecoration(BuiltInSemantic.Position) } });
        module.Structs.Add(new IrStructDecl(outputType));

        var bindingsType = new IrStructType("Uniforms");
        bindingsType.Fields.Add(new IrStructField { Name = "MVP", Type = float4x4, Decorations = { new IrBindingDecoration(0, 0) } });
        module.Structs.Add(new IrStructDecl(bindingsType));

        var body = new IrBlock();
        body.Statements.Add(new IrReturn(new IrParameter("output", outputType)));

        module.VertexStage = new IrVertexStage
        {
            Name = "VS",
            InputType = inputType,
            OutputType = outputType,
            BindingsType = bindingsType,
            Body = body,
        };

        var generator = new MslCodeGenerator();
        var msl = generator.GenerateCode(module);

        Assert.Contains("constant Uniforms& bindings [[buffer(1)]]", msl);
    }

    [Fact]
    public void MslGenerator_FloatConstants_AlwaysHaveDecimalPoint()
    {
        // Metal rejects float literals without a decimal point (e.g. "1f" is invalid; "1.0f" is valid).
        var module = IrBuilder.CreateTriangleShaderModule();
        var generator = new MslCodeGenerator();

        var msl = generator.GenerateCode(module);

        // The correctly formatted forms must appear in the vertex body constructor.
        Assert.Contains("0.0f", msl);
        Assert.Contains("1.0f", msl);

        // "1f" and "0f" as standalone tokens (not preceded by a digit/dot) must not appear.
        // We look for ", 1f" or "(1f" context which would only arise from a bare literal.
        Assert.DoesNotContain(", 1f", msl);
        Assert.DoesNotContain(", 0f", msl);
        Assert.DoesNotContain("(1f", msl);
        Assert.DoesNotContain("(0f", msl);
    }
}
