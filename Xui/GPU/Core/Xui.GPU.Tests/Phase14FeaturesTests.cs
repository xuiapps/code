using Xunit;
using Xui.GPU.Shaders.Types;
using Xui.GPU.Shaders.Attributes;
using Xui.GPU.Software;

namespace Xui.GPU.Tests;

/// <summary>
/// Tests for Phase 14: Extended Math and Intrinsics features.
/// Includes swizzle support, interpolation attributes, and extended blend modes.
/// </summary>
public class Phase14FeaturesTests
{
    #region Swizzle Tests

    [Fact]
    public void Float2_Swizzles_Work()
    {
        var v = new Float2(new F32(1.0f), new F32(2.0f));

        // Test 2-component swizzles
        var xy = v.XY;
        var yx = v.YX;
        var xx = v.XX;
        var yy = v.YY;

        Assert.Equal(1.0f, (float)xy.X);
        Assert.Equal(2.0f, (float)xy.Y);

        Assert.Equal(2.0f, (float)yx.X);
        Assert.Equal(1.0f, (float)yx.Y);

        Assert.Equal(1.0f, (float)xx.X);
        Assert.Equal(1.0f, (float)xx.Y);

        Assert.Equal(2.0f, (float)yy.X);
        Assert.Equal(2.0f, (float)yy.Y);
    }

    [Fact]
    public void Float3_Swizzles_2Component_Work()
    {
        var v = new Float3(new F32(1.0f), new F32(2.0f), new F32(3.0f));

        // Test 2-component swizzles
        var xy = v.XY;
        var xz = v.XZ;
        var yx = v.YX;
        var yz = v.YZ;
        var zx = v.ZX;
        var zy = v.ZY;

        Assert.Equal(1.0f, (float)xy.X);
        Assert.Equal(2.0f, (float)xy.Y);

        Assert.Equal(1.0f, (float)xz.X);
        Assert.Equal(3.0f, (float)xz.Y);

        Assert.Equal(2.0f, (float)yx.X);
        Assert.Equal(1.0f, (float)yx.Y);

        Assert.Equal(2.0f, (float)yz.X);
        Assert.Equal(3.0f, (float)yz.Y);

        Assert.Equal(3.0f, (float)zx.X);
        Assert.Equal(1.0f, (float)zx.Y);

        Assert.Equal(3.0f, (float)zy.X);
        Assert.Equal(2.0f, (float)zy.Y);
    }

    [Fact]
    public void Float3_Swizzles_3Component_Work()
    {
        var v = new Float3(new F32(1.0f), new F32(2.0f), new F32(3.0f));

        // Test 3-component swizzles
        var xyz = v.XYZ;
        var xzy = v.XZY;
        var yxz = v.YXZ;
        var yzx = v.YZX;
        var zxy = v.ZXY;
        var zyx = v.ZYX;

        Assert.Equal(1.0f, (float)xyz.X);
        Assert.Equal(2.0f, (float)xyz.Y);
        Assert.Equal(3.0f, (float)xyz.Z);

        Assert.Equal(1.0f, (float)xzy.X);
        Assert.Equal(3.0f, (float)xzy.Y);
        Assert.Equal(2.0f, (float)xzy.Z);

        Assert.Equal(2.0f, (float)yxz.X);
        Assert.Equal(1.0f, (float)yxz.Y);
        Assert.Equal(3.0f, (float)yxz.Z);

        Assert.Equal(2.0f, (float)yzx.X);
        Assert.Equal(3.0f, (float)yzx.Y);
        Assert.Equal(1.0f, (float)yzx.Z);

        Assert.Equal(3.0f, (float)zxy.X);
        Assert.Equal(1.0f, (float)zxy.Y);
        Assert.Equal(2.0f, (float)zxy.Z);

        Assert.Equal(3.0f, (float)zyx.X);
        Assert.Equal(2.0f, (float)zyx.Y);
        Assert.Equal(1.0f, (float)zyx.Z);
    }

    [Fact]
    public void Float4_Swizzles_2Component_Work()
    {
        var v = new Float4(new F32(1.0f), new F32(2.0f), new F32(3.0f), new F32(4.0f));

        // Test common 2-component swizzles
        var xy = v.XY;
        var xz = v.XZ;
        var xw = v.XW;
        var yz = v.YZ;
        var yw = v.YW;
        var zw = v.ZW;

        Assert.Equal(1.0f, (float)xy.X);
        Assert.Equal(2.0f, (float)xy.Y);

        Assert.Equal(1.0f, (float)xz.X);
        Assert.Equal(3.0f, (float)xz.Y);

        Assert.Equal(1.0f, (float)xw.X);
        Assert.Equal(4.0f, (float)xw.Y);

        Assert.Equal(2.0f, (float)yz.X);
        Assert.Equal(3.0f, (float)yz.Y);

        Assert.Equal(2.0f, (float)yw.X);
        Assert.Equal(4.0f, (float)yw.Y);

        Assert.Equal(3.0f, (float)zw.X);
        Assert.Equal(4.0f, (float)zw.Y);
    }

    [Fact]
    public void Float4_Swizzles_3Component_Work()
    {
        var v = new Float4(new F32(1.0f), new F32(2.0f), new F32(3.0f), new F32(4.0f));

        // Test common 3-component swizzles
        var xyz = v.XYZ;
        var xyw = v.XYW;
        var xzw = v.XZW;
        var yzw = v.YZW;
        var rgb = v.RGB;

        Assert.Equal(1.0f, (float)xyz.X);
        Assert.Equal(2.0f, (float)xyz.Y);
        Assert.Equal(3.0f, (float)xyz.Z);

        Assert.Equal(1.0f, (float)xyw.X);
        Assert.Equal(2.0f, (float)xyw.Y);
        Assert.Equal(4.0f, (float)xyw.Z);

        Assert.Equal(1.0f, (float)xzw.X);
        Assert.Equal(3.0f, (float)xzw.Y);
        Assert.Equal(4.0f, (float)xzw.Z);

        Assert.Equal(2.0f, (float)yzw.X);
        Assert.Equal(3.0f, (float)yzw.Y);
        Assert.Equal(4.0f, (float)yzw.Z);

        // Test RGB alias
        Assert.Equal(1.0f, (float)rgb.X);
        Assert.Equal(2.0f, (float)rgb.Y);
        Assert.Equal(3.0f, (float)rgb.Z);
    }

    [Fact]
    public void Float4_Swizzles_4Component_Work()
    {
        var v = new Float4(new F32(1.0f), new F32(2.0f), new F32(3.0f), new F32(4.0f));

        // Test 4-component swizzles
        var xyzw = v.XYZW;
        var rgba = v.RGBA;

        Assert.Equal(1.0f, (float)xyzw.X);
        Assert.Equal(2.0f, (float)xyzw.Y);
        Assert.Equal(3.0f, (float)xyzw.Z);
        Assert.Equal(4.0f, (float)xyzw.W);

        // Test RGBA alias
        Assert.Equal(1.0f, (float)rgba.X);
        Assert.Equal(2.0f, (float)rgba.Y);
        Assert.Equal(3.0f, (float)rgba.Z);
        Assert.Equal(4.0f, (float)rgba.W);
    }

    #endregion

    #region Interpolation Attribute Tests

    [Fact]
    public void InterpolationAttribute_CanBeCreated()
    {
        var perspectiveAttr = new InterpolationAttribute(InterpolationMode.Perspective);
        var linearAttr = new InterpolationAttribute(InterpolationMode.Linear);
        var flatAttr = new InterpolationAttribute(InterpolationMode.Flat);

        Assert.Equal(InterpolationMode.Perspective, perspectiveAttr.Mode);
        Assert.Equal(InterpolationMode.Linear, linearAttr.Mode);
        Assert.Equal(InterpolationMode.Flat, flatAttr.Mode);
    }

    [Fact]
    public void InterpolationMode_EnumValues_AreCorrect()
    {
        // Verify enum values exist
        Assert.True(Enum.IsDefined(typeof(InterpolationMode), InterpolationMode.Perspective));
        Assert.True(Enum.IsDefined(typeof(InterpolationMode), InterpolationMode.Linear));
        Assert.True(Enum.IsDefined(typeof(InterpolationMode), InterpolationMode.Flat));
    }

    #endregion

    #region Blend Mode Tests

    [Fact]
    public void BlendMode_Add_AddsColors()
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
    public void BlendMode_Add_ClampsTooHigh()
    {
        var src = new Color4(0.8f, 0.9f, 1.0f, 1.0f);
        var dst = new Color4(0.5f, 0.6f, 0.7f, 0.5f);

        var result = ColorTarget.Add(src, dst);

        // Values should be clamped to 1.0
        Assert.Equal(1.0f, result.R, precision: 5);
        Assert.Equal(1.0f, result.G, precision: 5);
        Assert.Equal(1.0f, result.B, precision: 5);
        Assert.Equal(1.0f, result.A, precision: 5);
    }

    [Fact]
    public void BlendMode_Multiply_MultipliesColors()
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
    public void BlendMode_Screen_ProducesLighterResult()
    {
        var src = new Color4(0.5f, 0.6f, 0.7f, 1.0f);
        var dst = new Color4(0.4f, 0.5f, 0.6f, 0.8f);

        var result = ColorTarget.Screen(src, dst);

        // Screen blend should lighten
        Assert.True(result.R >= src.R);
        Assert.True(result.G >= src.G);
        Assert.True(result.B >= src.B);
    }

    [Fact]
    public void BlendMode_Overlay_CombinesMultiplyAndScreen()
    {
        var src = new Color4(0.7f, 0.6f, 0.5f, 1.0f);
        var dst = new Color4(0.3f, 0.5f, 0.8f, 1.0f);

        var result = ColorTarget.Overlay(src, dst);

        // Overlay should darken dark values (dst.R < 0.5)
        Assert.True(result.R < 0.7f);
        // And lighten light values (dst.B >= 0.5)
        Assert.True(result.B > 0.5f);
        // Alpha should match source
        Assert.Equal(1.0f, result.A, precision: 5);
    }

    [Fact]
    public void BlendWith_Alpha_UsesAlphaBlending()
    {
        var src = new Color4(1.0f, 0.0f, 0.0f, 0.5f);
        var dst = new Color4(0.0f, 1.0f, 0.0f, 1.0f);

        var result = ColorTarget.BlendWith(src, dst, BlendMode.Alpha);

        // Should be equivalent to Blend()
        var expected = ColorTarget.Blend(src, dst);
        Assert.Equal((float)expected.R, (float)result.R, precision: 5);
        Assert.Equal((float)expected.G, (float)result.G, precision: 5);
        Assert.Equal((float)expected.B, (float)result.B, precision: 5);
        Assert.Equal((float)expected.A, (float)result.A, precision: 5);
    }

    [Fact]
    public void BlendWith_Add_UsesAdditiveBlending()
    {
        var src = new Color4(0.3f, 0.4f, 0.5f, 1.0f);
        var dst = new Color4(0.2f, 0.3f, 0.4f, 0.0f);

        var result = ColorTarget.BlendWith(src, dst, BlendMode.Add);

        Assert.Equal(0.5f, result.R, precision: 5);
        Assert.Equal(0.7f, result.G, precision: 5);
        Assert.Equal(0.9f, result.B, precision: 5);
    }

    [Fact]
    public void BlendWith_Multiply_UsesMultiplicativeBlending()
    {
        var src = new Color4(0.5f, 0.6f, 0.8f, 1.0f);
        var dst = new Color4(0.4f, 0.5f, 0.5f, 0.8f);

        var result = ColorTarget.BlendWith(src, dst, BlendMode.Multiply);

        Assert.Equal(0.2f, result.R, precision: 5);
        Assert.Equal(0.3f, result.G, precision: 5);
        Assert.Equal(0.4f, result.B, precision: 5);
    }

    [Fact]
    public void BlendWith_Screen_UsesScreenBlending()
    {
        var src = new Color4(0.5f, 0.6f, 0.7f, 1.0f);
        var dst = new Color4(0.4f, 0.5f, 0.6f, 0.8f);

        var result = ColorTarget.BlendWith(src, dst, BlendMode.Screen);

        // Verify it's lighter than multiply
        var multiply = ColorTarget.Multiply(src, dst);
        Assert.True(result.R > multiply.R);
        Assert.True(result.G > multiply.G);
        Assert.True(result.B > multiply.B);
    }

    [Fact]
    public void BlendWith_Overlay_UsesOverlayBlending()
    {
        var src = new Color4(0.7f, 0.6f, 0.5f, 1.0f);
        var dst = new Color4(0.3f, 0.5f, 0.8f, 1.0f);

        var result = ColorTarget.BlendWith(src, dst, BlendMode.Overlay);

        // Alpha should match source
        Assert.Equal(1.0f, result.A, precision: 5);
    }

    [Fact]
    public void BlendWith_Replace_ReplacesWithSource()
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
    public void BlendMode_EnumValues_AreAllDefined()
    {
        // Verify all enum values exist
        Assert.True(Enum.IsDefined(typeof(BlendMode), BlendMode.Alpha));
        Assert.True(Enum.IsDefined(typeof(BlendMode), BlendMode.Add));
        Assert.True(Enum.IsDefined(typeof(BlendMode), BlendMode.Multiply));
        Assert.True(Enum.IsDefined(typeof(BlendMode), BlendMode.Screen));
        Assert.True(Enum.IsDefined(typeof(BlendMode), BlendMode.Overlay));
        Assert.True(Enum.IsDefined(typeof(BlendMode), BlendMode.Replace));
    }

    #endregion
}
