using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Xui.Runtime.Software;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct RGBA
{
    public byte Red;
    public byte Green;
    public byte Blue;
    public byte Alpha;

    public RGBA(byte r, byte g, byte b, byte a)
    {
        Red = r;
        Green = g;
        Blue = b;
        Alpha = a;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static byte BlendByte(byte dst, byte src, float alpha)
    {
        return (byte)(dst * (1 - alpha) + src * alpha);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static RGBA Blend(RGBA dst, RGBA src, float alphaFactor)
    {
        return new RGBA(
            BlendByte(dst.Red,   src.Red,   alphaFactor),
            BlendByte(dst.Green, src.Green, alphaFactor),
            BlendByte(dst.Blue,  src.Blue,  alphaFactor),
            BlendByte(dst.Alpha, src.Alpha, alphaFactor)
        );
    }
}
