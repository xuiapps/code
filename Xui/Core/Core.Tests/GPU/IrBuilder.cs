using Xui.GPU.IR;

namespace Xui.GPU.Backends.Hlsl;

/// <summary>
/// Helper class to build IR shader modules from C# shader implementations.
/// This is a simplified builder for demonstration purposes.
/// </summary>
public static class IrBuilder
{
    /// <summary>
    /// Creates a simple triangle shader module for demonstration.
    /// </summary>
    public static IrShaderModule CreateTriangleShaderModule()
    {
        var module = new IrShaderModule { Name = "TriangleShader" };
        
        // Define F32 scalar type
        var f32 = new IrScalarType(ScalarKind.F32);
        
        // Define Float2 vector type
        var float2 = new IrVectorType(f32, 2);
        
        // Define Float4 vector type
        var float4 = new IrVectorType(f32, 4);
        
        // Define Color4 as Float4 (RGBA)
        var color4 = float4;
        
        // Define TriangleVertex struct (input to vertex shader)
        var triangleVertex = new IrStructType("TriangleVertex");
        triangleVertex.Fields.Add(new IrStructField 
        { 
            Name = "Position", 
            Type = float2,
            Decorations = { new IrLocationDecoration(0) }
        });
        triangleVertex.Fields.Add(new IrStructField 
        { 
            Name = "Color", 
            Type = color4,
            Decorations = { new IrLocationDecoration(1) }
        });
        module.Structs.Add(new IrStructDecl(triangleVertex));
        
        // Define TriangleVaryings struct (output from VS, input to FS)
        var triangleVaryings = new IrStructType("TriangleVaryings");
        triangleVaryings.Fields.Add(new IrStructField 
        { 
            Name = "Position", 
            Type = float4,
            Decorations = { new IrBuiltInDecoration(BuiltInSemantic.Position) }
        });
        triangleVaryings.Fields.Add(new IrStructField 
        { 
            Name = "Color", 
            Type = color4,
            Decorations = { new IrLocationDecoration(0) }
        });
        module.Structs.Add(new IrStructDecl(triangleVaryings));
        
        // Define FragmentOutput struct (output from fragment shader)
        var fragmentOutput = new IrStructType("FragmentOutput");
        fragmentOutput.Fields.Add(new IrStructField 
        { 
            Name = "Color", 
            Type = color4,
            Decorations = { new IrLocationDecoration(0) }
        });
        module.Structs.Add(new IrStructDecl(fragmentOutput));
        
        // Create vertex shader stage
        var vertexStage = new IrVertexStage
        {
            Name = "TriangleVertexShader",
            InputType = triangleVertex,
            OutputType = triangleVaryings,
            Body = CreateVertexShaderBody()
        };
        module.VertexStage = vertexStage;
        
        // Create fragment shader stage
        var fragmentStage = new IrFragmentStage
        {
            Name = "TriangleFragmentShader",
            InputType = triangleVaryings,
            OutputType = fragmentOutput,
            Body = CreateFragmentShaderBody()
        };
        module.FragmentStage = fragmentStage;
        
        return module;
    }
    
    private static IrBlock CreateVertexShaderBody()
    {
        var f32 = new IrScalarType(ScalarKind.F32);
        var float2 = new IrVectorType(f32, 2);
        var float4 = new IrVectorType(f32, 4);
        
        var block = new IrBlock();
        
        // Create output variable
        var triangleVaryings = new IrStructType("TriangleVaryings");
        var outputVar = new IrVarDecl("output", triangleVaryings);
        block.Statements.Add(outputVar);
        
        // output.Position = Float4(input.Position, 0.0f, 1.0f)
        var inputParam = new IrParameter("input", new IrStructType("TriangleVertex"));
        var inputPosition = new IrFieldAccess(inputParam, "Position", float2);
        var zero = new IrConstant(f32, 0.0f);
        var one = new IrConstant(f32, 1.0f);
        var positionCtor = new IrConstructor(float4, new List<IrExpression> { inputPosition, zero, one });
        
        var outputParam = new IrParameter("output", triangleVaryings);
        var outputPosition = new IrFieldAccess(outputParam, "Position", float4);
        block.Statements.Add(new IrAssignment(outputPosition, positionCtor));
        
        // output.Color = input.Color
        var inputColor = new IrFieldAccess(inputParam, "Color", float4);
        var outputColor = new IrFieldAccess(outputParam, "Color", float4);
        block.Statements.Add(new IrAssignment(outputColor, inputColor));
        
        // return output
        block.Statements.Add(new IrReturn(outputParam));
        
        return block;
    }
    
    private static IrBlock CreateFragmentShaderBody()
    {
        var f32 = new IrScalarType(ScalarKind.F32);
        var float4 = new IrVectorType(f32, 4);
        
        var block = new IrBlock();
        
        // Create output variable
        var fragmentOutput = new IrStructType("FragmentOutput");
        var outputVar = new IrVarDecl("output", fragmentOutput);
        block.Statements.Add(outputVar);
        
        // output.Color = input.Color
        var inputParam = new IrParameter("input", new IrStructType("TriangleVaryings"));
        var inputColor = new IrFieldAccess(inputParam, "Color", float4);
        
        var outputParam = new IrParameter("output", fragmentOutput);
        var outputColor = new IrFieldAccess(outputParam, "Color", float4);
        block.Statements.Add(new IrAssignment(outputColor, inputColor));
        
        // return output
        block.Statements.Add(new IrReturn(outputParam));
        
        return block;
    }
}
