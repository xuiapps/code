using Xunit;
using Xui.GPU.Shaders.Types;

namespace Xui.GPU.Tests;

/// <summary>
/// Tests for vector swizzle operations on Float2, Float3, and Float4.
/// </summary>
public class SwizzleTests
{
    [Fact]
    public void Float2_Swizzle_XY_ReturnsOriginalOrder()
    {
        var v = new Float2(1.0f, 2.0f);
        var result = v.XY;
        Assert.Equal(1.0f, (float)result.X);
        Assert.Equal(2.0f, (float)result.Y);
    }

    [Fact]
    public void Float2_Swizzle_YX_ReversesComponents()
    {
        var v = new Float2(1.0f, 2.0f);
        var result = v.YX;
        Assert.Equal(2.0f, (float)result.X);
        Assert.Equal(1.0f, (float)result.Y);
    }

    [Fact]
    public void Float2_Swizzle_XX_DuplicatesX()
    {
        var v = new Float2(1.0f, 2.0f);
        var result = v.XX;
        Assert.Equal(1.0f, (float)result.X);
        Assert.Equal(1.0f, (float)result.Y);
    }

    [Fact]
    public void Float2_Swizzle_YY_DuplicatesY()
    {
        var v = new Float2(1.0f, 2.0f);
        var result = v.YY;
        Assert.Equal(2.0f, (float)result.X);
        Assert.Equal(2.0f, (float)result.Y);
    }

    [Fact]
    public void Float3_Swizzle2_ReturnsCorrectPairs()
    {
        var v = new Float3(1.0f, 2.0f, 3.0f);

        Assert.Equal(1.0f, (float)v.XY.X); Assert.Equal(2.0f, (float)v.XY.Y);
        Assert.Equal(1.0f, (float)v.XZ.X); Assert.Equal(3.0f, (float)v.XZ.Y);
        Assert.Equal(2.0f, (float)v.YX.X); Assert.Equal(1.0f, (float)v.YX.Y);
        Assert.Equal(2.0f, (float)v.YZ.X); Assert.Equal(3.0f, (float)v.YZ.Y);
        Assert.Equal(3.0f, (float)v.ZX.X); Assert.Equal(1.0f, (float)v.ZX.Y);
        Assert.Equal(3.0f, (float)v.ZY.X); Assert.Equal(2.0f, (float)v.ZY.Y);
    }

    [Fact]
    public void Float3_Swizzle3_Permutations()
    {
        var v = new Float3(1.0f, 2.0f, 3.0f);

        var xyz = v.XYZ; Assert.Equal(1.0f, (float)xyz.X); Assert.Equal(2.0f, (float)xyz.Y); Assert.Equal(3.0f, (float)xyz.Z);
        var xzy = v.XZY; Assert.Equal(1.0f, (float)xzy.X); Assert.Equal(3.0f, (float)xzy.Y); Assert.Equal(2.0f, (float)xzy.Z);
        var yxz = v.YXZ; Assert.Equal(2.0f, (float)yxz.X); Assert.Equal(1.0f, (float)yxz.Y); Assert.Equal(3.0f, (float)yxz.Z);
        var yzx = v.YZX; Assert.Equal(2.0f, (float)yzx.X); Assert.Equal(3.0f, (float)yzx.Y); Assert.Equal(1.0f, (float)yzx.Z);
        var zxy = v.ZXY; Assert.Equal(3.0f, (float)zxy.X); Assert.Equal(1.0f, (float)zxy.Y); Assert.Equal(2.0f, (float)zxy.Z);
        var zyx = v.ZYX; Assert.Equal(3.0f, (float)zyx.X); Assert.Equal(2.0f, (float)zyx.Y); Assert.Equal(1.0f, (float)zyx.Z);
    }

    [Fact]
    public void Float4_Swizzle2_CommonPairs()
    {
        var v = new Float4(1.0f, 2.0f, 3.0f, 4.0f);

        Assert.Equal(1.0f, (float)v.XY.X); Assert.Equal(2.0f, (float)v.XY.Y);
        Assert.Equal(1.0f, (float)v.XZ.X); Assert.Equal(3.0f, (float)v.XZ.Y);
        Assert.Equal(1.0f, (float)v.XW.X); Assert.Equal(4.0f, (float)v.XW.Y);
        Assert.Equal(2.0f, (float)v.YZ.X); Assert.Equal(3.0f, (float)v.YZ.Y);
        Assert.Equal(2.0f, (float)v.YW.X); Assert.Equal(4.0f, (float)v.YW.Y);
        Assert.Equal(3.0f, (float)v.ZW.X); Assert.Equal(4.0f, (float)v.ZW.Y);
    }

    [Fact]
    public void Float4_Swizzle3_CommonTriples()
    {
        var v = new Float4(1.0f, 2.0f, 3.0f, 4.0f);

        var xyz = v.XYZ; Assert.Equal(1.0f, (float)xyz.X); Assert.Equal(2.0f, (float)xyz.Y); Assert.Equal(3.0f, (float)xyz.Z);
        var xyw = v.XYW; Assert.Equal(1.0f, (float)xyw.X); Assert.Equal(2.0f, (float)xyw.Y); Assert.Equal(4.0f, (float)xyw.Z);
        var xzw = v.XZW; Assert.Equal(1.0f, (float)xzw.X); Assert.Equal(3.0f, (float)xzw.Y); Assert.Equal(4.0f, (float)xzw.Z);
        var yzw = v.YZW; Assert.Equal(2.0f, (float)yzw.X); Assert.Equal(3.0f, (float)yzw.Y); Assert.Equal(4.0f, (float)yzw.Z);
    }

    [Fact]
    public void Float4_Swizzle_XYZW_Identity()
    {
        var v = new Float4(1.0f, 2.0f, 3.0f, 4.0f);
        var result = v.XYZW;
        Assert.Equal(1.0f, (float)result.X);
        Assert.Equal(2.0f, (float)result.Y);
        Assert.Equal(3.0f, (float)result.Z);
        Assert.Equal(4.0f, (float)result.W);
    }

    [Fact]
    public void Float4_Swizzle_RGBA_AliasMatchesXYZW()
    {
        var v = new Float4(1.0f, 2.0f, 3.0f, 4.0f);
        var rgba = v.RGBA;
        var rgb = v.RGB;

        Assert.Equal((float)v.X, (float)rgba.X);
        Assert.Equal((float)v.Y, (float)rgba.Y);
        Assert.Equal((float)v.Z, (float)rgba.Z);
        Assert.Equal((float)v.W, (float)rgba.W);

        Assert.Equal((float)v.X, (float)rgb.X);
        Assert.Equal((float)v.Y, (float)rgb.Y);
        Assert.Equal((float)v.Z, (float)rgb.Z);
    }
}
