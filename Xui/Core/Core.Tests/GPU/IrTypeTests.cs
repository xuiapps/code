using Xunit;
using Xui.GPU.IR;

namespace Xui.GPU.IR.Tests;

/// <summary>
/// Tests for IR type system.
/// </summary>
public class IrTypeTests
{
    [Fact]
    public void ScalarType_F32_HasCorrectName()
    {
        var f32 = new IrScalarType(ScalarKind.F32);
        Assert.Equal("F32", f32.Name);
        Assert.Equal(ScalarKind.F32, f32.ScalarKind);
    }

    [Fact]
    public void ScalarType_I32_HasCorrectName()
    {
        var i32 = new IrScalarType(ScalarKind.I32);
        Assert.Equal("I32", i32.Name);
        Assert.Equal(ScalarKind.I32, i32.ScalarKind);
    }

    [Fact]
    public void VectorType_Float2_HasCorrectName()
    {
        var f32 = new IrScalarType(ScalarKind.F32);
        var float2 = new IrVectorType(f32, 2);
        
        Assert.Equal("F322", float2.Name);
        Assert.Equal(2, float2.Dimension);
        Assert.Equal(f32, float2.ElementType);
    }

    [Fact]
    public void VectorType_Float4_HasCorrectName()
    {
        var f32 = new IrScalarType(ScalarKind.F32);
        var float4 = new IrVectorType(f32, 4);
        
        Assert.Equal("F324", float4.Name);
        Assert.Equal(4, float4.Dimension);
    }

    [Fact]
    public void MatrixType_Float4x4_HasCorrectName()
    {
        var f32 = new IrScalarType(ScalarKind.F32);
        var matrix = new IrMatrixType(f32, 4, 4);
        
        Assert.Equal("F324x4", matrix.Name);
        Assert.Equal(4, matrix.Rows);
        Assert.Equal(4, matrix.Columns);
    }

    [Fact]
    public void StructType_CanAddFields()
    {
        var structType = new IrStructType("MyStruct");
        var f32 = new IrScalarType(ScalarKind.F32);
        
        structType.Fields.Add(new IrStructField 
        { 
            Name = "X", 
            Type = f32 
        });
        
        Assert.Equal("MyStruct", structType.Name);
        Assert.Single(structType.Fields);
        Assert.Equal("X", structType.Fields[0].Name);
    }

    [Fact]
    public void TextureType_HasCorrectName()
    {
        var f32 = new IrScalarType(ScalarKind.F32);
        var float4 = new IrVectorType(f32, 4);
        var texture = new IrTextureType(float4);
        
        Assert.Equal("Texture2D<F324>", texture.Name);
    }

    [Fact]
    public void SamplerType_IsCorrect()
    {
        var sampler = IrSamplerType.Instance;
        
        Assert.Equal("Sampler", sampler.Name);
        Assert.Equal(IrNodeKind.SamplerType, sampler.Kind);
    }
}
