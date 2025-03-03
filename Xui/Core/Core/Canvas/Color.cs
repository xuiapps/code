using System.Runtime.InteropServices;

namespace Xui.Core.Canvas;

public struct Color
{
    public nfloat Red;
    public nfloat Green;
    public nfloat Blue;
    public nfloat Alpha;

    public Color(nfloat red, nfloat green, nfloat blue, nfloat alpha)
    {
        this.Red = red;
        this.Green = green;
        this.Blue = blue;
        this.Alpha = alpha;
    }

    public Color(uint rgba)
    {
        this.Red = ((rgba & 0xFF000000) >> 24) / 255.0f;
        this.Green = ((rgba & 0x00FF0000) >> 16) / 255.0f;
        this.Blue = ((rgba & 0x0000FF00) >> 8)  / 255.0f;
        this.Alpha = ((rgba & 0x000000FF) >> 0)  / 255.0f;
    }

    public static implicit operator Color(uint rgbaHex) => new Color(rgbaHex);
}