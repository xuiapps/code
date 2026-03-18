using Xunit;
using Xui.GPU.Shaders.Types;
using Xui.GPU.Shaders.Intrinsics;
using static Xui.GPU.Shaders.Intrinsics.Shader;

namespace Xui.GPU.Tests;

/// <summary>
/// Tests for shader intrinsic functions.
/// </summary>
public class ShaderIntrinsicsTests
{
    private const float Epsilon = 0.0001f;

    [Fact]
    public void Clamp_ScalarValues_Work()
    {
        var value = new F32(5.0f);
        var result1 = Clamp(value, new F32(0.0f), new F32(10.0f));
        var result2 = Clamp(value, new F32(6.0f), new F32(10.0f));
        var result3 = Clamp(value, new F32(0.0f), new F32(4.0f));

        Assert.Equal(5.0f, (float)result1);
        Assert.Equal(6.0f, (float)result2);
        Assert.Equal(4.0f, (float)result3);
    }

    [Fact]
    public void Saturate_ScalarValues_Work()
    {
        Assert.Equal(0.5f, (float)Saturate(new F32(0.5f)));
        Assert.Equal(0.0f, (float)Saturate(new F32(-0.5f)));
        Assert.Equal(1.0f, (float)Saturate(new F32(1.5f)));
    }

    [Fact]
    public void Lerp_ScalarValues_Work()
    {
        var a = new F32(0.0f);
        var b = new F32(10.0f);
        
        Assert.Equal(0.0f, (float)Lerp(a, b, new F32(0.0f)));
        Assert.Equal(5.0f, (float)Lerp(a, b, new F32(0.5f)));
        Assert.Equal(10.0f, (float)Lerp(a, b, new F32(1.0f)));
    }

    [Fact]
    public void MinMax_ScalarValues_Work()
    {
        var a = new F32(3.0f);
        var b = new F32(7.0f);

        Assert.Equal(3.0f, (float)Min(a, b));
        Assert.Equal(7.0f, (float)Max(a, b));
    }

    [Fact]
    public void Abs_ScalarValues_Work()
    {
        Assert.Equal(5.0f, (float)Abs(new F32(5.0f)));
        Assert.Equal(5.0f, (float)Abs(new F32(-5.0f)));
    }

    [Fact]
    public void TrigonometricFunctions_Work()
    {
        var angle = new F32(MathF.PI / 4.0f); // 45 degrees

        float sin = Sin(angle);
        float cos = Cos(angle);
        float tan = Tan(angle);

        Assert.Equal(MathF.Sqrt(2.0f) / 2.0f, sin, Epsilon);
        Assert.Equal(MathF.Sqrt(2.0f) / 2.0f, cos, Epsilon);
        Assert.Equal(1.0f, tan, Epsilon);
    }

    [Fact]
    public void InverseTrigonometricFunctions_Work()
    {
        var value = new F32(0.5f);

        float asin = Asin(value);
        float acos = Acos(value);
        float atan = Atan(value);

        Assert.Equal(MathF.Asin(0.5f), asin, Epsilon);
        Assert.Equal(MathF.Acos(0.5f), acos, Epsilon);
        Assert.Equal(MathF.Atan(0.5f), atan, Epsilon);
    }

    [Fact]
    public void Atan2_Works()
    {
        var y = new F32(1.0f);
        var x = new F32(1.0f);
        
        float result = Atan2(y, x);
        Assert.Equal(MathF.PI / 4.0f, result, Epsilon);
    }

    [Fact]
    public void PowerFunctions_Work()
    {
        var value = new F32(2.0f);
        var exponent = new F32(3.0f);

        Assert.Equal(8.0f, (float)Pow(value, exponent), Epsilon);
        Assert.Equal(MathF.Exp(2.0f), (float)Exp(value), Epsilon);
        Assert.Equal(MathF.Log(2.0f), (float)Log(value), Epsilon);
        Assert.Equal(MathF.Log2(2.0f), (float)Log2(value), Epsilon);
    }

    [Fact]
    public void Sqrt_Works()
    {
        var value = new F32(16.0f);
        Assert.Equal(4.0f, (float)Sqrt(value));
    }

    [Fact]
    public void Rsqrt_Works()
    {
        var value = new F32(16.0f);
        Assert.Equal(0.25f, (float)Rsqrt(value), Epsilon);
    }

    [Fact]
    public void RoundingFunctions_Work()
    {
        var value = new F32(3.7f);

        Assert.Equal(3.0f, (float)Floor(value));
        Assert.Equal(4.0f, (float)Ceil(value));
        Assert.Equal(0.7f, (float)Fract(value), Epsilon);
        Assert.Equal(4.0f, (float)Round(value));
    }

    [Fact]
    public void Dot_Float2_Works()
    {
        var a = new Float2(new F32(1.0f), new F32(2.0f));
        var b = new Float2(new F32(3.0f), new F32(4.0f));

        var result = Dot(a, b);
        Assert.Equal(11.0f, (float)result); // 1*3 + 2*4 = 11
    }

    [Fact]
    public void Dot_Float3_Works()
    {
        var a = new Float3(new F32(1.0f), new F32(2.0f), new F32(3.0f));
        var b = new Float3(new F32(4.0f), new F32(5.0f), new F32(6.0f));

        var result = Dot(a, b);
        Assert.Equal(32.0f, (float)result); // 1*4 + 2*5 + 3*6 = 32
    }

    [Fact]
    public void Dot_Float4_Works()
    {
        var a = new Float4(new F32(1.0f), new F32(2.0f), new F32(3.0f), new F32(4.0f));
        var b = new Float4(new F32(5.0f), new F32(6.0f), new F32(7.0f), new F32(8.0f));

        var result = Dot(a, b);
        Assert.Equal(70.0f, (float)result); // 1*5 + 2*6 + 3*7 + 4*8 = 70
    }

    [Fact]
    public void Length_Float2_Works()
    {
        var v = new Float2(new F32(3.0f), new F32(4.0f));
        var result = Length(v);
        Assert.Equal(5.0f, (float)result); // sqrt(3^2 + 4^2) = 5
    }

    [Fact]
    public void Length_Float3_Works()
    {
        var v = new Float3(new F32(1.0f), new F32(2.0f), new F32(2.0f));
        var result = Length(v);
        Assert.Equal(3.0f, (float)result, Epsilon); // sqrt(1 + 4 + 4) = 3
    }

    [Fact]
    public void Distance_Float2_Works()
    {
        var a = new Float2(new F32(0.0f), new F32(0.0f));
        var b = new Float2(new F32(3.0f), new F32(4.0f));

        var result = Distance(a, b);
        Assert.Equal(5.0f, (float)result);
    }

    [Fact]
    public void Normalize_Float2_Works()
    {
        var v = new Float2(new F32(3.0f), new F32(4.0f));
        var result = Normalize(v);

        Assert.Equal(0.6f, (float)result.X, Epsilon);
        Assert.Equal(0.8f, (float)result.Y, Epsilon);
        Assert.Equal(1.0f, (float)Length(result), Epsilon);
    }

    [Fact]
    public void Normalize_Float3_Works()
    {
        var v = new Float3(new F32(1.0f), new F32(2.0f), new F32(2.0f));
        var result = Normalize(v);

        var len = Length(result);
        Assert.Equal(1.0f, (float)len, Epsilon);
    }

    [Fact]
    public void Cross_Float3_Works()
    {
        var a = new Float3(new F32(1.0f), new F32(0.0f), new F32(0.0f));
        var b = new Float3(new F32(0.0f), new F32(1.0f), new F32(0.0f));

        var result = Cross(a, b);

        Assert.Equal(0.0f, (float)result.X);
        Assert.Equal(0.0f, (float)result.Y);
        Assert.Equal(1.0f, (float)result.Z);
    }

    [Fact]
    public void Lerp_Float2_Works()
    {
        var a = new Float2(new F32(0.0f), new F32(0.0f));
        var b = new Float2(new F32(10.0f), new F32(20.0f));
        var t = new F32(0.5f);

        var result = Lerp(a, b, t);

        Assert.Equal(5.0f, (float)result.X);
        Assert.Equal(10.0f, (float)result.Y);
    }

    [Fact]
    public void Lerp_Float3_Works()
    {
        var a = new Float3(new F32(0.0f), new F32(0.0f), new F32(0.0f));
        var b = new Float3(new F32(10.0f), new F32(20.0f), new F32(30.0f));
        var t = new F32(0.5f);

        var result = Lerp(a, b, t);

        Assert.Equal(5.0f, (float)result.X);
        Assert.Equal(10.0f, (float)result.Y);
        Assert.Equal(15.0f, (float)result.Z);
    }

    [Fact]
    public void Min_Float2_Works()
    {
        var a = new Float2(new F32(1.0f), new F32(5.0f));
        var b = new Float2(new F32(3.0f), new F32(2.0f));

        var result = Min(a, b);

        Assert.Equal(1.0f, (float)result.X);
        Assert.Equal(2.0f, (float)result.Y);
    }

    [Fact]
    public void Max_Float3_Works()
    {
        var a = new Float3(new F32(1.0f), new F32(5.0f), new F32(3.0f));
        var b = new Float3(new F32(3.0f), new F32(2.0f), new F32(4.0f));

        var result = Max(a, b);

        Assert.Equal(3.0f, (float)result.X);
        Assert.Equal(5.0f, (float)result.Y);
        Assert.Equal(4.0f, (float)result.Z);
    }

    [Fact]
    public void Clamp_Float2_Works()
    {
        var value = new Float2(new F32(-1.0f), new F32(2.0f));
        var min = new Float2(new F32(0.0f), new F32(0.0f));
        var max = new Float2(new F32(1.0f), new F32(1.0f));

        var result = Clamp(value, min, max);

        Assert.Equal(0.0f, (float)result.X);
        Assert.Equal(1.0f, (float)result.Y);
    }

    [Fact]
    public void Saturate_Float3_Works()
    {
        var value = new Float3(new F32(-0.5f), new F32(0.5f), new F32(1.5f));
        var result = Saturate(value);

        Assert.Equal(0.0f, (float)result.X);
        Assert.Equal(0.5f, (float)result.Y);
        Assert.Equal(1.0f, (float)result.Z);
    }

    [Fact]
    public void Ddx_ThrowsNotImplementedException()
    {
        var value = new F32(1.0f);
        Assert.Throws<NotImplementedException>(() => Ddx(value));
    }

    [Fact]
    public void Ddy_ThrowsNotImplementedException()
    {
        var value = new F32(1.0f);
        Assert.Throws<NotImplementedException>(() => Ddy(value));
    }

    [Fact]
    public void Fwidth_ThrowsNotImplementedException()
    {
        var value = new F32(1.0f);
        Assert.Throws<NotImplementedException>(() => Fwidth(value));
    }
}
