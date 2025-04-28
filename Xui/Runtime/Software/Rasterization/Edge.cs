using Xui.Core.Math2D;

namespace Xui.Runtime.Software.Rasterization;

public readonly struct Edge
{
    public readonly nfloat X0, Y0;
    public readonly nfloat X1, Y1;
    public readonly nfloat DxDy;  // slope (delta x / delta y)

    public Edge(Point start, Point end)
    {
        if (start.Y < end.Y)
        {
            X0 = start.X;
            Y0 = start.Y;
            X1 = end.X;
            Y1 = end.Y;
        }
        else
        {
            X0 = end.X;
            Y0 = end.Y;
            X1 = start.X;
            Y1 = start.Y;
        }

        var dy = Y1 - Y0;
        DxDy = dy != 0 ? (X1 - X0) / dy : 0;
    }
}
