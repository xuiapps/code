using Xunit;
using Xui.GPU.Shaders.Attributes;

namespace Xui.GPU.Tests;

/// <summary>
/// Tests for interpolation attribute and mode enum.
/// </summary>
public class InterpolationAttributeTests
{
    [Theory]
    [InlineData(InterpolationMode.Perspective)]
    [InlineData(InterpolationMode.Linear)]
    [InlineData(InterpolationMode.Flat)]
    public void InterpolationAttribute_StoresMode(InterpolationMode mode)
    {
        var attr = new InterpolationAttribute(mode);
        Assert.Equal(mode, attr.Mode);
    }

    [Fact]
    public void InterpolationMode_AllValuesAreDefined()
    {
        Assert.True(Enum.IsDefined(typeof(InterpolationMode), InterpolationMode.Perspective));
        Assert.True(Enum.IsDefined(typeof(InterpolationMode), InterpolationMode.Linear));
        Assert.True(Enum.IsDefined(typeof(InterpolationMode), InterpolationMode.Flat));
    }
}
