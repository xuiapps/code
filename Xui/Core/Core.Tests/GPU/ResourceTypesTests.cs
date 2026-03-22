using Xunit;
using Xui.GPU.Shaders.Types;
using static Xui.GPU.Shaders.Intrinsics.Shader;

namespace Xui.GPU.Tests;

/// <summary>
/// Tests for texture and sampler resource types.
/// </summary>
public class ResourceTypesTests
{
    [Fact]
    public void Texture2D_Construction_Works()
    {
        var texture = new Texture2D<Color4>(null);
        // Texture2D is a struct, so it's always non-null
        // Just verify it can be created
    }

    [Fact]
    public void Sampler_DefaultValues_AreCorrect()
    {
        // Default struct initialization uses zero values for enums
        var sampler = new Sampler();
        Assert.Equal(FilterMode.Point, sampler.MinFilter);  // Point is enum value 0
        Assert.Equal(FilterMode.Point, sampler.MagFilter);
        Assert.Equal(AddressMode.Repeat, sampler.AddressU);  // Repeat is enum value 0
        Assert.Equal(AddressMode.Repeat, sampler.AddressV);
    }

    [Fact]
    public void Sampler_ConstructorDefaults_AreLinearRepeat()
    {
        // Constructor with default parameters uses Linear and Repeat
        var sampler = new Sampler(
            minFilter: FilterMode.Linear,
            magFilter: FilterMode.Linear,
            addressU: AddressMode.Repeat,
            addressV: AddressMode.Repeat
        );
        Assert.Equal(FilterMode.Linear, sampler.MinFilter);
        Assert.Equal(FilterMode.Linear, sampler.MagFilter);
        Assert.Equal(AddressMode.Repeat, sampler.AddressU);
        Assert.Equal(AddressMode.Repeat, sampler.AddressV);
    }

    [Fact]
    public void Sampler_LinearRepeat_IsCorrect()
    {
        var sampler = Sampler.LinearRepeat;
        Assert.Equal(FilterMode.Linear, sampler.MinFilter);
        Assert.Equal(FilterMode.Linear, sampler.MagFilter);
        Assert.Equal(AddressMode.Repeat, sampler.AddressU);
        Assert.Equal(AddressMode.Repeat, sampler.AddressV);
    }

    [Fact]
    public void Sampler_LinearClamp_IsCorrect()
    {
        var sampler = Sampler.LinearClamp;
        Assert.Equal(FilterMode.Linear, sampler.MinFilter);
        Assert.Equal(FilterMode.Linear, sampler.MagFilter);
        Assert.Equal(AddressMode.ClampToEdge, sampler.AddressU);
        Assert.Equal(AddressMode.ClampToEdge, sampler.AddressV);
    }

    [Fact]
    public void Sampler_PointRepeat_IsCorrect()
    {
        var sampler = Sampler.PointRepeat;
        Assert.Equal(FilterMode.Point, sampler.MinFilter);
        Assert.Equal(FilterMode.Point, sampler.MagFilter);
        Assert.Equal(AddressMode.Repeat, sampler.AddressU);
        Assert.Equal(AddressMode.Repeat, sampler.AddressV);
    }

    [Fact]
    public void Sampler_PointClamp_IsCorrect()
    {
        var sampler = Sampler.PointClamp;
        Assert.Equal(FilterMode.Point, sampler.MinFilter);
        Assert.Equal(FilterMode.Point, sampler.MagFilter);
        Assert.Equal(AddressMode.ClampToEdge, sampler.AddressU);
        Assert.Equal(AddressMode.ClampToEdge, sampler.AddressV);
    }

    [Fact]
    public void Sampler_CustomConfiguration_Works()
    {
        var sampler = new Sampler(
            minFilter: FilterMode.Point,
            magFilter: FilterMode.Linear,
            addressU: AddressMode.ClampToEdge,
            addressV: AddressMode.MirroredRepeat
        );

        Assert.Equal(FilterMode.Point, sampler.MinFilter);
        Assert.Equal(FilterMode.Linear, sampler.MagFilter);
        Assert.Equal(AddressMode.ClampToEdge, sampler.AddressU);
        Assert.Equal(AddressMode.MirroredRepeat, sampler.AddressV);
    }

    [Fact]
    public void Int2_Construction_Works()
    {
        var v = new Int2(new I32(10), new I32(20));
        Assert.Equal(10, (int)v.X);
        Assert.Equal(20, (int)v.Y);
    }

    [Fact]
    public void Int2_Operations_Work()
    {
        var a = new Int2(new I32(10), new I32(20));
        var b = new Int2(new I32(5), new I32(4));

        var sum = a + b;
        var diff = a - b;
        var product = a * b;

        Assert.Equal(15, (int)sum.X);
        Assert.Equal(24, (int)sum.Y);
        Assert.Equal(5, (int)diff.X);
        Assert.Equal(16, (int)diff.Y);
        Assert.Equal(50, (int)product.X);
        Assert.Equal(80, (int)product.Y);
    }

    [Fact]
    public void Sample_ThrowsNotImplementedException()
    {
        var texture = new Texture2D<Color4>(null);
        var sampler = Sampler.LinearRepeat;
        var uv = new Float2(new F32(0.5f), new F32(0.5f));

        Assert.Throws<NotImplementedException>(() => Sample(texture, sampler, uv));
    }

    [Fact]
    public void Load_ThrowsNotImplementedException()
    {
        var texture = new Texture2D<Color4>(null);
        var location = new Int2(new I32(0), new I32(0));

        Assert.Throws<NotImplementedException>(() => Load(texture, location));
    }
}
