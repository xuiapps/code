using System.Runtime.InteropServices;
using Xui.Core.Canvas;

namespace Xui.Apps.BlankApp;

public class Wallet
{
    public static readonly Wallet Instance = new Wallet();

    public NFloat Selected { get; set; }
    
    public Xui.Core.Canvas.Color Color { get; set; }
    public Xui.Core.Canvas.Color SelectedColor { get; set; }

    public void Render(IContext context)
    {
        context.Save();

        context.BeginPath();
        context.LineWidth = NFloat.Lerp(1.25f, 2.25f, Selected);

        var stroke = new Color(
            NFloat.Lerp(Color.Red, SelectedColor.Red, Selected),
            NFloat.Lerp(Color.Green, SelectedColor.Green, Selected),
            NFloat.Lerp(Color.Blue, SelectedColor.Blue, Selected),
            NFloat.Lerp(Color.Alpha, SelectedColor.Alpha, Selected)
        );
        var fill = new Color(stroke.Red, stroke.Green, stroke.Blue, NFloat.Lerp(0f, stroke.Alpha, Selected));

        NFloat seven = NFloat.Lerp(7, 9, Selected);
        NFloat five = NFloat.Lerp(5, 6, Selected);

        NFloat oneTop = NFloat.Lerp(-2f, -4f, Selected);
        NFloat oneBottom = NFloat.Lerp(-2f, -0.5f, Selected);

        context.MoveTo((-seven, oneTop));
        context.ArcTo((-seven, -five), (seven, -five), 2);
        context.ArcTo((seven, -five), (seven, oneTop), 2);
        context.LineTo((seven, oneTop));
        context.ClosePath();

        context.MoveTo((-seven, oneBottom));
        context.LineTo((seven, oneBottom));
        context.ArcTo((seven, five), (-seven, five), 2);
        context.ArcTo((-seven, five), (-seven, oneBottom), 2);
        context.LineTo((-seven, oneBottom));
        context.ClosePath();

        context.SetStroke(stroke);
        context.Stroke();

        context.MoveTo((-seven, oneTop));
        context.ArcTo((-seven, -five), (seven, -five), 2);
        context.ArcTo((seven, -five), (seven, oneTop), 2);
        context.LineTo((seven, oneTop));
        context.ClosePath();

        context.Rect((NFloat.Lerp(-5, -7, Selected), NFloat.Lerp(2, 1, Selected), NFloat.Lerp(4, 8, Selected), NFloat.Lerp(0, 4, Selected)));
        context.ClosePath();

        context.SetFill(fill);
        context.Fill(FillRule.EvenOdd);

        context.Restore();
    }
}
