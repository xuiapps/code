namespace Xui.Core.Canvas;

public struct GradientStop
{
    public nfloat Offset;
    public Color Color;

    public GradientStop(nfloat offset, Color color)
    {
        this.Offset = offset;
        this.Color = color;
    }

    public GradientStop(nfloat offset, uint color)
    {
        this.Offset = offset;
        this.Color = new Color(color);
    }
}