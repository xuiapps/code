namespace Xui.Core.Canvas;

public interface IPenContext
{
    public nfloat GlobalAlpha { set; }

    public LineCap LineCap { set; }

    public LineJoin LineJoin { set; }

    public nfloat LineWidth { set; }

    public nfloat MiterLimit { set; }

    public nfloat LineDashOffset { set; }

    public void SetLineDash(ReadOnlySpan<nfloat> segments);

    public void SetStroke(Color color);

    public void SetStroke(LinearGradient linearGradient);

    public void SetStroke(RadialGradient radialGradient);

    public void SetFill(Color color);

    public void SetFill(LinearGradient linearGradient);

    public void SetFill(RadialGradient radialGradient);
}