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

    [Fact]
    public void F32_ComparisonOperators_Work()
    {
        var a = new F32(5.0f);
        var b = new F32(3.0f);
        var c = new F32(5.0f);

        Assert.True((bool)(a == c));
        Assert.True((bool)(a != b));
        Assert.True((bool)(a > b));
        Assert.True((bool)(b < a));
        Assert.True((bool)(a >= c));
        Assert.True((bool)(a <= c));
    }

    [Fact]
    public void F32_Negation_Works()
    {
        var a = new F32(5.0f);
        var result = -a;
        Assert.Equal(-5.0f, (float)result);
    }

    [Fact]
    public void I32_ArithmeticOperators_Work()
    {
        var a = new I32(10);
        var b = new I32(3);

        Assert.Equal(13, (int)(a + b));
        Assert.Equal(7, (int)(a - b));
        Assert.Equal(30, (int)(a * b));
        Assert.Equal(3, (int)(a / b));
        Assert.Equal(-10, (int)(-a));
    }

    [Fact]
    public void I32_ComparisonOperators_Work()
    {
        var a = new I32(10);
        var b = new I32(5);
        var c = new I32(10);

        Assert.True((bool)(a == c));
        Assert.True((bool)(a != b));
        Assert.True((bool)(a > b));
        Assert.True((bool)(b < a));
        Assert.True((bool)(a >= c));
        Assert.True((bool)(a <= c));
    }

    [Fact]
    public void I32_BitwiseOperators_Work()
    {
        var a = new I32(0b1010); // 10
        var b = new I32(0b1100); // 12

        Assert.Equal(0b1000, (int)(a & b)); // AND = 8
        Assert.Equal(0b1110, (int)(a | b)); // OR = 14
        Assert.Equal(0b0110, (int)(a ^ b)); // XOR = 6
        Assert.Equal(unchecked((int)0xFFFFFFF5), (int)(~a)); // NOT
        Assert.Equal(0b10100, (int)(a << 1)); // Left shift = 20
        Assert.Equal(0b0101, (int)(a >> 1)); // Right shift = 5
    }

    [Fact]
    public void U32_ArithmeticOperators_Work()
    {
        var a = new U32(10);
        var b = new U32(3);

        Assert.Equal(13u, (uint)(a + b));
        Assert.Equal(7u, (uint)(a - b));
        Assert.Equal(30u, (uint)(a * b));
        Assert.Equal(3u, (uint)(a / b));
    }

    [Fact]
    public void U32_ComparisonOperators_Work()
    {
        var a = new U32(10);
        var b = new U32(5);
        var c = new U32(10);

        Assert.True((bool)(a == c));
        Assert.True((bool)(a != b));
        Assert.True((bool)(a > b));
        Assert.True((bool)(b < a));
        Assert.True((bool)(a >= c));
        Assert.True((bool)(a <= c));
    }

    [Fact]
    public void U32_BitwiseOperators_Work()
    {
        var a = new U32(0b1010u); // 10
        var b = new U32(0b1100u); // 12

        Assert.Equal(0b1000u, (uint)(a & b)); // AND = 8
        Assert.Equal(0b1110u, (uint)(a | b)); // OR = 14
        Assert.Equal(0b0110u, (uint)(a ^ b)); // XOR = 6
        Assert.Equal(0xFFFFFFF5u, (uint)(~a)); // NOT
        Assert.Equal(0b10100u, (uint)(a << 1)); // Left shift = 20
        Assert.Equal(0b0101u, (uint)(a >> 1)); // Right shift = 5
    }

    [Fact]
    public void Bool_LogicalOperators_Work()
    {
        var t = Bool.True;
        var f = Bool.False;

        Assert.True((bool)(t & t));
        Assert.False((bool)(t & f));
        Assert.True((bool)(t | f));
        Assert.False((bool)(f | f));
        Assert.False((bool)(!t));
        Assert.True((bool)(!f));
        Assert.True((bool)(t == t));
        Assert.True((bool)(t != f));
    }

    [Fact]
    public void Float2_Division_Works()
    {
        var a = new Float2(new F32(10.0f), new F32(20.0f));
        var b = new Float2(new F32(2.0f), new F32(4.0f));
        var scalar = new F32(2.0f);

        var result1 = a / b;
        var result2 = a / scalar;

        Assert.Equal(5.0f, (float)result1.X);
        Assert.Equal(5.0f, (float)result1.Y);
        Assert.Equal(5.0f, (float)result2.X);
        Assert.Equal(10.0f, (float)result2.Y);
    }

    [Fact]
    public void Float2_Negation_Works()
    {
        var vec = new Float2(new F32(1.0f), new F32(-2.0f));
        var result = -vec;

        Assert.Equal(-1.0f, (float)result.X);
        Assert.Equal(2.0f, (float)result.Y);
    }

    [Fact]
    public void Float3_Division_Works()
    {
        var a = new Float3(new F32(10.0f), new F32(20.0f), new F32(30.0f));
        var b = new Float3(new F32(2.0f), new F32(4.0f), new F32(5.0f));
        var scalar = new F32(2.0f);

        var result1 = a / b;
        var result2 = a / scalar;

        Assert.Equal(5.0f, (float)result1.X);
        Assert.Equal(5.0f, (float)result1.Y);
        Assert.Equal(6.0f, (float)result1.Z);
        Assert.Equal(5.0f, (float)result2.X);
        Assert.Equal(10.0f, (float)result2.Y);
        Assert.Equal(15.0f, (float)result2.Z);
    }

    [Fact]
    public void Float3_Negation_Works()
    {
        var vec = new Float3(new F32(1.0f), new F32(-2.0f), new F32(3.0f));
        var result = -vec;

        Assert.Equal(-1.0f, (float)result.X);
        Assert.Equal(2.0f, (float)result.Y);
        Assert.Equal(-3.0f, (float)result.Z);
    }

    [Fact]
    public void Float4_Division_Works()
    {
        var a = new Float4(new F32(10.0f), new F32(20.0f), new F32(30.0f), new F32(40.0f));
        var b = new Float4(new F32(2.0f), new F32(4.0f), new F32(5.0f), new F32(8.0f));
        var scalar = new F32(2.0f);

        var result1 = a / b;
        var result2 = a / scalar;

        Assert.Equal(5.0f, (float)result1.X);
        Assert.Equal(5.0f, (float)result1.Y);
        Assert.Equal(6.0f, (float)result1.Z);
        Assert.Equal(5.0f, (float)result1.W);
        Assert.Equal(5.0f, (float)result2.X);
        Assert.Equal(10.0f, (float)result2.Y);
        Assert.Equal(15.0f, (float)result2.Z);
        Assert.Equal(20.0f, (float)result2.W);
    }

    [Fact]
    public void Float4_Negation_Works()
    {
        var vec = new Float4(new F32(1.0f), new F32(-2.0f), new F32(3.0f), new F32(-4.0f));
        var result = -vec;

        Assert.Equal(-1.0f, (float)result.X);
        Assert.Equal(2.0f, (float)result.Y);
        Assert.Equal(-3.0f, (float)result.Z);
        Assert.Equal(4.0f, (float)result.W);
    }

    [Fact]
    public void Float4x4_MatrixMultiplication_Works()
    {
        var identity = Float4x4.Identity;
        var scale2 = new Float4x4(
            new Float4(new F32(2.0f), F32.Zero, F32.Zero, F32.Zero),
            new Float4(F32.Zero, new F32(2.0f), F32.Zero, F32.Zero),
            new Float4(F32.Zero, F32.Zero, new F32(2.0f), F32.Zero),
            new Float4(F32.Zero, F32.Zero, F32.Zero, F32.One)
        );

        var result = identity * scale2;

        Assert.Equal(2.0f, (float)result.Row0.X);
        Assert.Equal(0.0f, (float)result.Row0.Y);
        Assert.Equal(2.0f, (float)result.Row1.Y);
        Assert.Equal(2.0f, (float)result.Row2.Z);
        Assert.Equal(1.0f, (float)result.Row3.W);
    }

    [Fact]
    public void Float4x4_CreateTranslation_Works()
    {
        var translation = Float4x4.CreateTranslation(new F32(1.0f), new F32(2.0f), new F32(3.0f));
        var point = new Float4(F32.Zero, F32.Zero, F32.Zero, F32.One);
        
        var result = translation * point;
        
        Assert.Equal(1.0f, (float)result.X);
        Assert.Equal(2.0f, (float)result.Y);
        Assert.Equal(3.0f, (float)result.Z);
        Assert.Equal(1.0f, (float)result.W);
    }

    [Fact]
    public void Float4x4_CreateRotationY_Works()
    {
        // Rotate 90 degrees around Y axis
        var rotation = Float4x4.CreateRotationY(new F32(MathF.PI / 2.0f));
        var point = new Float4(F32.One, F32.Zero, F32.Zero, F32.One);
        
        var result = rotation * point;
        
        // Point (1,0,0) should rotate to approximately (0,0,1)
        Assert.Equal(0.0f, (float)result.X, 5);
        Assert.Equal(0.0f, (float)result.Y, 5);
        Assert.Equal(1.0f, (float)result.Z, 5);
        Assert.Equal(1.0f, (float)result.W, 5);
    }

    [Fact]
    public void Float4x4_CreatePerspective_Works()
    {
        var perspective = Float4x4.CreatePerspective(
            new F32(MathF.PI / 4.0f),  // 45 degree FOV
            new F32(1.0f),              // 1:1 aspect ratio
            new F32(0.1f),              // Near plane
            new F32(100.0f)             // Far plane
        );
        
        // Check that the matrix is not identity
        Assert.NotEqual(1.0f, (float)perspective.Row0.X);
        Assert.NotEqual(0.0f, (float)perspective.Row0.X);
        Assert.NotEqual(1.0f, (float)perspective.Row1.Y);
        Assert.NotEqual(0.0f, (float)perspective.Row1.Y);
        
        // Check perspective projection properties ([0,1] depth range, DirectX-style)
        // Row 3, column 2 (Z component) should be -1 for perspective divide
        Assert.Equal(-1.0f, (float)perspective.Row3.Z);
        // Row 2 should have depth mapping coefficients
        Assert.NotEqual(0.0f, (float)perspective.Row2.Z);
        Assert.NotEqual(0.0f, (float)perspective.Row2.W);
    }

    [Fact]
    public void Float4_Float3Constructor_Works()
    {
        var vec3 = new Float3(new F32(1.0f), new F32(2.0f), new F32(3.0f));
        var vec4 = new Float4(vec3, new F32(4.0f));
        
        Assert.Equal(1.0f, (float)vec4.X);
        Assert.Equal(2.0f, (float)vec4.Y);
        Assert.Equal(3.0f, (float)vec4.Z);
        Assert.Equal(4.0f, (float)vec4.W);
    }

    [Fact]
    public void Float3_Cross_Works()
    {
        var xAxis = new Float3(F32.One, F32.Zero, F32.Zero);
        var yAxis = new Float3(F32.Zero, F32.One, F32.Zero);
        
        var zAxis = Float3.Cross(xAxis, yAxis);
        
        Assert.Equal(0.0f, (float)zAxis.X);
        Assert.Equal(0.0f, (float)zAxis.Y);
        Assert.Equal(1.0f, (float)zAxis.Z);
    }

    [Fact]
    public void Float3_Normalize_Works()
    {
        var vec = new Float3(new F32(3.0f), new F32(4.0f), F32.Zero);
        var normalized = Float3.Normalize(vec);
        
        // Length should be 1
        float lengthSquared = (float)(normalized.X * normalized.X + normalized.Y * normalized.Y + normalized.Z * normalized.Z);
        Assert.Equal(1.0f, MathF.Sqrt(lengthSquared), 5);
        
        // Direction should be preserved
        Assert.Equal(0.6f, (float)normalized.X, 5);
        Assert.Equal(0.8f, (float)normalized.Y, 5);
    }
}
