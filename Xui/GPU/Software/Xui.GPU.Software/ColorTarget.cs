using System;
using Xui.GPU.Shaders.Types;

namespace Xui.GPU.Software;

/// <summary>
/// Represents a color attachment target for rendering operations.
/// </summary>
/// <remarks>
/// Provides conversion utilities between shader Color4 types and RGBA32 packed format.
/// </remarks>
public static class ColorTarget
{
    /// <summary>
    /// Converts a Color4 value to packed RGBA32 format (0xRRGGBBAA).
    /// </summary>
    /// <param name="color">Color4 value with components in range [0.0, 1.0].</param>
    /// <returns>Packed RGBA32 color value.</returns>
    /// <remarks>
    /// Color components are clamped to [0.0, 1.0] and converted to 8-bit values.
    /// </remarks>
    public static uint ToRgba32(Color4 color)
    {
        // Clamp and convert to byte range
        byte r = (byte)Math.Clamp((int)(color.R * 255.0f), 0, 255);
        byte g = (byte)Math.Clamp((int)(color.G * 255.0f), 0, 255);
        byte b = (byte)Math.Clamp((int)(color.B * 255.0f), 0, 255);
        byte a = (byte)Math.Clamp((int)(color.A * 255.0f), 0, 255);

        // Pack into RGBA32 format
        return ((uint)r << 24) | ((uint)g << 16) | ((uint)b << 8) | a;
    }

    /// <summary>
    /// Converts packed RGBA32 format to a Color4 value.
    /// </summary>
    /// <param name="rgba32">Packed RGBA32 color value (0xRRGGBBAA).</param>
    /// <returns>Color4 value with components in range [0.0, 1.0].</returns>
    public static Color4 FromRgba32(uint rgba32)
    {
        byte r = (byte)((rgba32 >> 24) & 0xFF);
        byte g = (byte)((rgba32 >> 16) & 0xFF);
        byte b = (byte)((rgba32 >> 8) & 0xFF);
        byte a = (byte)(rgba32 & 0xFF);

        return new Color4(
            r / 255.0f,
            g / 255.0f,
            b / 255.0f,
            a / 255.0f
        );
    }

    /// <summary>
    /// Blends two colors using alpha blending (source over destination).
    /// </summary>
    /// <param name="src">Source color with premultiplied alpha.</param>
    /// <param name="dst">Destination color.</param>
    /// <returns>Blended color.</returns>
    /// <remarks>
    /// Uses standard alpha blending formula:
    /// result = src.rgb * src.a + dst.rgb * (1 - src.a)
    /// </remarks>
    public static Color4 Blend(Color4 src, Color4 dst)
    {
        float srcAlpha = src.A;
        float invSrcAlpha = 1.0f - srcAlpha;

        return new Color4(
            src.R * srcAlpha + dst.R * invSrcAlpha,
            src.G * srcAlpha + dst.G * invSrcAlpha,
            src.B * srcAlpha + dst.B * invSrcAlpha,
            srcAlpha + dst.A * invSrcAlpha
        );
    }

    /// <summary>
    /// Blends a source color onto a destination using packed RGBA32 format.
    /// </summary>
    /// <param name="src">Source RGBA32 color.</param>
    /// <param name="dst">Destination RGBA32 color.</param>
    /// <returns>Blended RGBA32 color.</returns>
    public static uint BlendRgba32(uint src, uint dst)
    {
        var srcColor = FromRgba32(src);
        var dstColor = FromRgba32(dst);
        var blended = Blend(srcColor, dstColor);
        return ToRgba32(blended);
    }
}
