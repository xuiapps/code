using System.Runtime.InteropServices;
using Xui.Core.Canvas;
using Xui.Core.Math2D;
using Xui.Core.UI;

namespace Xui.Apps.BlankApp;

public class Star
{
    public static readonly Star Instance = new Star();

    public void Render(IContext context)
    {
        context.Save();
        context.BeginPath();
        context.LineWidth = 2f;
        context.SetStroke(Xui.Core.Canvas.Colors.Black);

        var r1 = 6f;
        var r2 = 12f;

        var tenthPi = NFloat.Pi * 2f / 10f;
        for(int i = 0; i < 5; i++)
        {
            var phi = i * NFloat.Pi * 2f / 5f;
            var p0 = new Xui.Core.Math2D.Point(0, 0) + r1 * new Vector(NFloat.Sin(phi - tenthPi), -NFloat.Cos(phi - tenthPi));
            var p1 = new Xui.Core.Math2D.Point(0, 0) + r2 * new Vector(NFloat.Sin(phi), -NFloat.Cos(phi));
            var p2 = new Xui.Core.Math2D.Point(0, 0) + r1 * new Vector(NFloat.Sin(phi + tenthPi), -NFloat.Cos(phi + tenthPi));
            var p3 = new Xui.Core.Math2D.Point(0, 0) + r2 * new Vector(NFloat.Sin(phi + tenthPi + tenthPi), -NFloat.Cos(phi + tenthPi + tenthPi));

            if (i == 0)
            {
                context.MoveTo(Xui.Core.Math2D.Point.Lerp(p0, p1, 0.5f));
            }

            context.ArcTo(p1, p2, 2f);
            context.ArcTo(p2, p3, 2f);
        }
        context.ClosePath();
        context.Stroke();

        context.Restore();
    }
}
