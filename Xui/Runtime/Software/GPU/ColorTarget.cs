using System;
using Xui.GPU.Shaders.Types;

namespace Xui.GPU.Software;

/// <summary>
/// Defines blend modes for color blending operations.
/// </summary>
public enum BlendMode
{
    /// <summary>
    /// Standard alpha blending (source over destination).
    /// result = src.rgb * src.a + dst.rgb * (1 - src.a)
    /// </summary>
    Alpha,

    /// <summary>
    /// Additive blending - colors are added together.
    /// result = src.rgb + dst.rgb
    /// </summary>
    Add,

    /// <summary>
    /// Multiplicative blending - colors are multiplied.
    /// result = src.rgb * dst.rgb
    /// </summary>
    Multiply,

    /// <summary>
    /// Screen blending - inverted multiplicative.
    /// result = 1 - (1 - src.rgb) * (1 - dst.rgb)
    /// </summary>
    Screen,

    /// <summary>
    /// Overlay blending - combination of multiply and screen.
    /// Darkens dark colors, lightens light colors.
    /// </summary>
    Overlay,

    /// <summary>
    /// Replace mode - source completely replaces destination.
    /// result = src.rgb
    /// </summary>
    Replace
}

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

    /// <summary>
    /// Blends two colors using the specified blend mode.
    /// </summary>
    /// <param name="src">Source color.</param>
    /// <param name="dst">Destination color.</param>
    /// <param name="mode">The blend mode to use.</param>
    /// <returns>Blended color.</returns>
    public static Color4 BlendWith(Color4 src, Color4 dst, BlendMode mode)
    {
        return mode switch
        {
            BlendMode.Alpha => Blend(src, dst),
            BlendMode.Add => Add(src, dst),
            BlendMode.Multiply => Multiply(src, dst),
            BlendMode.Screen => Screen(src, dst),
            BlendMode.Overlay => Overlay(src, dst),
            BlendMode.Replace => src,
            _ => throw new ArgumentOutOfRangeException(nameof(mode), mode, "Unknown blend mode")
        };
    }

    /// <summary>
    /// Additive blending - adds source and destination colors.
    /// </summary>
    /// <param name="src">Source color.</param>
    /// <param name="dst">Destination color.</param>
    /// <returns>Added color (clamped to [0, 1]).</returns>
    public static Color4 Add(Color4 src, Color4 dst)
    {
        return new Color4(
            Math.Min(src.R + dst.R, 1.0f),
            Math.Min(src.G + dst.G, 1.0f),
            Math.Min(src.B + dst.B, 1.0f),
            Math.Min(src.A + dst.A, 1.0f)
        );
    }

    /// <summary>
    /// Multiplicative blending - multiplies source and destination colors.
    /// </summary>
    /// <param name="src">Source color.</param>
    /// <param name="dst">Destination color.</param>
    /// <returns>Multiplied color.</returns>
    public static Color4 Multiply(Color4 src, Color4 dst)
    {
        return new Color4(
            src.R * dst.R,
            src.G * dst.G,
            src.B * dst.B,
            src.A * dst.A
        );
    }

    /// <summary>
    /// Screen blending - inverted multiplicative blending.
    /// </summary>
    /// <param name="src">Source color.</param>
    /// <param name="dst">Destination color.</param>
    /// <returns>Screen-blended color.</returns>
    public static Color4 Screen(Color4 src, Color4 dst)
    {
        return new Color4(
            1.0f - (1.0f - src.R) * (1.0f - dst.R),
            1.0f - (1.0f - src.G) * (1.0f - dst.G),
            1.0f - (1.0f - src.B) * (1.0f - dst.B),
            1.0f - (1.0f - src.A) * (1.0f - dst.A)
        );
    }

    /// <summary>
    /// Overlay blending - combination of multiply and screen based on destination luminance.
    /// </summary>
    /// <param name="src">Source color.</param>
    /// <param name="dst">Destination color.</param>
    /// <returns>Overlay-blended color.</returns>
    public static Color4 Overlay(Color4 src, Color4 dst)
    {
        static float OverlayChannel(float s, float d)
        {
            return d < 0.5f
                ? 2.0f * s * d
                : 1.0f - 2.0f * (1.0f - s) * (1.0f - d);
        }

        return new Color4(
            OverlayChannel(src.R, dst.R),
            OverlayChannel(src.G, dst.G),
            OverlayChannel(src.B, dst.B),
            src.A  // Overlay doesn't affect alpha
        );
    }
}
