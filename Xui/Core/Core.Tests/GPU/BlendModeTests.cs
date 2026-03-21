using Xunit;
using Xui.GPU.Shaders.Types;
using Xui.GPU.Software;

namespace Xui.GPU.Tests;

/// <summary>
/// Tests for color blending operations in the software renderer.
/// </summary>
public class BlendModeTests
{
    [Fact]
    public void Add_SumsComponents()
    {
        var src = new Color4(0.3f, 0.4f, 0.5f, 1.0f);
        var dst = new Color4(0.2f, 0.3f, 0.4f, 0.0f);
        var result = ColorTarget.Add(src, dst);
        Assert.Equal(0.5f, result.R, precision: 5);
        Assert.Equal(0.7f, result.G, precision: 5);
        Assert.Equal(0.9f, result.B, precision: 5);
        Assert.Equal(1.0f, result.A, precision: 5);
    }

    [Fact]
    public void Add_ClampsToOne()
    {
        var src = new Color4(0.8f, 0.9f, 1.0f, 1.0f);
        var dst = new Color4(0.5f, 0.6f, 0.7f, 0.5f);
        var result = ColorTarget.Add(src, dst);
        Assert.Equal(1.0f, result.R, precision: 5);
        Assert.Equal(1.0f, result.G, precision: 5);
        Assert.Equal(1.0f, result.B, precision: 5);
        Assert.Equal(1.0f, result.A, precision: 5);
    }

    [Fact]
    public void Multiply_MultipliesComponentwise()
    {
        var src = new Color4(0.5f, 0.6f, 0.8f, 1.0f);
        var dst = new Color4(0.4f, 0.5f, 0.5f, 0.8f);
        var result = ColorTarget.Multiply(src, dst);
        Assert.Equal(0.2f, result.R, precision: 5);
        Assert.Equal(0.3f, result.G, precision: 5);
        Assert.Equal(0.4f, result.B, precision: 5);
        Assert.Equal(0.8f, result.A, precision: 5);
    }

    [Fact]
    public void Screen_ProducesLighterResult()
    {
        var src = new Color4(0.5f, 0.6f, 0.7f, 1.0f);
        var dst = new Color4(0.4f, 0.5f, 0.6f, 0.8f);
        var result = ColorTarget.Screen(src, dst);
        Assert.True(result.R >= src.R);
        Assert.True(result.G >= src.G);
        Assert.True(result.B >= src.B);
    }

    [Fact]
    public void Overlay_DarkensDarkValues_LightensLightValues()
    {
        var src = new Color4(0.7f, 0.6f, 0.5f, 1.0f);
        var dst = new Color4(0.3f, 0.5f, 0.8f, 1.0f);
        var result = ColorTarget.Overlay(src, dst);
        Assert.True(result.R < 0.7f); // darkens where dst < 0.5
        Assert.True(result.B > 0.5f); // lightens where dst >= 0.5
        Assert.Equal(1.0f, result.A, precision: 5);
    }

    [Fact]
    public void Replace_IgnoresDestination()
    {
        var src = new Color4(1.0f, 0.0f, 0.0f, 0.5f);
        var dst = new Color4(0.0f, 1.0f, 0.0f, 1.0f);
        var result = ColorTarget.BlendWith(src, dst, BlendMode.Replace);
        Assert.Equal(1.0f, result.R, precision: 5);
        Assert.Equal(0.0f, result.G, precision: 5);
        Assert.Equal(0.0f, result.B, precision: 5);
        Assert.Equal(0.5f, result.A, precision: 5);
    }

    [Fact]
    public void BlendWith_Alpha_MatchesDirectBlendCall()
    {
        var src = new Color4(1.0f, 0.0f, 0.0f, 0.5f);
        var dst = new Color4(0.0f, 1.0f, 0.0f, 1.0f);
        var result = ColorTarget.BlendWith(src, dst, BlendMode.Alpha);
        var expected = ColorTarget.Blend(src, dst);
        Assert.Equal((float)expected.R, (float)result.R, precision: 5);
        Assert.Equal((float)expected.G, (float)result.G, precision: 5);
        Assert.Equal((float)expected.B, (float)result.B, precision: 5);
        Assert.Equal((float)expected.A, (float)result.A, precision: 5);
    }

    [Fact]
    public void BlendWith_AllModes_AreDefined()
    {
        Assert.True(Enum.IsDefined(typeof(BlendMode), BlendMode.Alpha));
        Assert.True(Enum.IsDefined(typeof(BlendMode), BlendMode.Add));
        Assert.True(Enum.IsDefined(typeof(BlendMode), BlendMode.Multiply));
        Assert.True(Enum.IsDefined(typeof(BlendMode), BlendMode.Screen));
        Assert.True(Enum.IsDefined(typeof(BlendMode), BlendMode.Overlay));
        Assert.True(Enum.IsDefined(typeof(BlendMode), BlendMode.Replace));
    }
}
