using Xunit;
using Xui.GPU.Shaders.Types;

namespace Xui.GPU.Tests;

/// <summary>
/// Tests for the shader type system.
/// </summary>
public class ShaderTypesTests
{
    [Fact]
    public void F32_BasicOperations_Work()
    {
        var a = new F32(5.0f);
        var b = new F32(3.0f);

        var sum = a + b;
        var diff = a - b;
        var product = a * b;
        var quotient = a / b;

        Assert.Equal(8.0f, (float)sum);
        Assert.Equal(2.0f, (float)diff);
        Assert.Equal(15.0f, (float)product);
        Assert.Equal(5.0f / 3.0f, (float)quotient, precision: 5);
    }

    [Fact]
    public void F32_Constants_AreCorrect()
    {
        Assert.Equal(0.0f, (float)F32.Zero);
        Assert.Equal(1.0f, (float)F32.One);
    }

    [Fact]
    public void Float2_BasicOperations_Work()
    {
        var a = new Float2(new F32(1.0f), new F32(2.0f));
        var b = new Float2(new F32(3.0f), new F32(4.0f));

        var sum = a + b;
        var diff = a - b;
        var product = a * b;

        Assert.Equal(4.0f, (float)sum.X);
        Assert.Equal(6.0f, (float)sum.Y);
        Assert.Equal(-2.0f, (float)diff.X);
        Assert.Equal(-2.0f, (float)diff.Y);
        Assert.Equal(3.0f, (float)product.X);
        Assert.Equal(8.0f, (float)product.Y);
    }

    [Fact]
    public void Float2_ScalarMultiplication_Works()
    {
        var vec = new Float2(new F32(2.0f), new F32(3.0f));
        var scalar = new F32(2.0f);

        var result1 = vec * scalar;
        var result2 = scalar * vec;

        Assert.Equal(4.0f, (float)result1.X);
        Assert.Equal(6.0f, (float)result1.Y);
        Assert.Equal(4.0f, (float)result2.X);
        Assert.Equal(6.0f, (float)result2.Y);
    }

    [Fact]
    public void Float4_Construction_Works()
    {
        var vec = new Float4(
            new F32(1.0f),
            new F32(2.0f),
            new F32(3.0f),
            new F32(4.0f)
        );

        Assert.Equal(1.0f, (float)vec.X);
        Assert.Equal(2.0f, (float)vec.Y);
        Assert.Equal(3.0f, (float)vec.Z);
        Assert.Equal(4.0f, (float)vec.W);
    }

    [Fact]
    public void Color4_Construction_Works()
    {
        var white = Color4.White;
        var black = Color4.Black;
        var transparent = Color4.Transparent;

        Assert.Equal(1.0f, (float)white.R);
        Assert.Equal(1.0f, (float)white.A);
        Assert.Equal(0.0f, (float)black.R);
        Assert.Equal(1.0f, (float)black.A);
        Assert.Equal(0.0f, (float)transparent.A);
    }

    [Fact]
    public void Float4x4_Identity_IsCorrect()
    {
        var identity = Float4x4.Identity;

        Assert.Equal(1.0f, (float)identity.Row0.X);
        Assert.Equal(0.0f, (float)identity.Row0.Y);
        Assert.Equal(1.0f, (float)identity.Row1.Y);
        Assert.Equal(1.0f, (float)identity.Row2.Z);
        Assert.Equal(1.0f, (float)identity.Row3.W);
    }

    [Fact]
    public void Float4x4_VectorMultiplication_Works()
    {
        var identity = Float4x4.Identity;
        var vec = new Float4(
            new F32(1.0f),
            new F32(2.0f),
            new F32(3.0f),
            new F32(4.0f)
        );

        var result = identity * vec;

        Assert.Equal(1.0f, (float)result.X);
        Assert.Equal(2.0f, (float)result.Y);
        Assert.Equal(3.0f, (float)result.Z);
        Assert.Equal(4.0f, (float)result.W);
    }
}
