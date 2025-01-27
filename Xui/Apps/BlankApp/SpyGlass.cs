using System.Runtime.InteropServices;
using Xui.Core.Canvas;
using Xui.Core.UI;

namespace Xui.Apps.BlankApp;

public class SpyGlass : IDrawable
{
    public static readonly SpyGlass Instance = new SpyGlass();

    public void Render(IContext context)
    {
        context.Save();
        context.LineWidth = 2f;

        context.Ellipse((-2.5f, -2.5f), 6, 6, 0, 0, NFloat.Pi * 2, Winding.ClockWise);
        context.SetStroke(Colors.Black);
        context.Stroke();

        context.LineWidth = 3f;
        context.LineCap = LineCap.Round;
        context.BeginPath();
        context.MoveTo((4.5f, 4.5f));
        context.LineTo((8, 8));
        context.SetStroke(Colors.Black);
        context.Stroke();

        context.Restore();
    }
}
