using Xui.Core.Canvas;
using Xui.Core.UI;

namespace Xui.Apps.BlankApp;

public class Hamburger : Xui.Core.UI.IDrawable
{
    public static readonly Hamburger Instance = new Hamburger();

    public void Render(IContext context)
    {
        context.Save();
        context.LineWidth = 3f;
        context.LineCap = Xui.Core.Canvas.LineCap.Round;
        context.SetStroke(Xui.Core.Canvas.Colors.Black);
        context.BeginPath();
        context.MoveTo((-8, -6));
        context.LineTo((8, -6));
        context.MoveTo((-8, 0));
        context.LineTo((8, 0));
        context.MoveTo((-8, 6));
        context.LineTo((8, 6));
        context.Stroke();
        context.Restore();
    }
}
