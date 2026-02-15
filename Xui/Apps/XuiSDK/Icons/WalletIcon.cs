using Xui.Core.Canvas;

namespace Xui.Apps.XuiSDK.Icons;

public class WalletIcon : INavIcon
{
    public nfloat Selected { get; set; }
    public Color Color { get; set; }
    public Color SelectedColor { get; set; }

    public void Render(IContext context)
    {
        context.Save();
        context.LineWidth = nfloat.Lerp(1.25f, 2.25f, Selected);

        var stroke = new Color(
            nfloat.Lerp(Color.Red, SelectedColor.Red, Selected),
            nfloat.Lerp(Color.Green, SelectedColor.Green, Selected),
            nfloat.Lerp(Color.Blue, SelectedColor.Blue, Selected),
            nfloat.Lerp(Color.Alpha, SelectedColor.Alpha, Selected)
        );
        var fill = new Color(stroke.Red, stroke.Green, stroke.Blue, nfloat.Lerp(0f, stroke.Alpha, Selected));

        nfloat seven = nfloat.Lerp(7, 9, Selected);
        nfloat five = nfloat.Lerp(5, 6, Selected);

        nfloat oneTop = nfloat.Lerp(-2f, -4f, Selected);
        nfloat oneBottom = nfloat.Lerp(-2f, -0.5f, Selected);

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

        context.Rect((nfloat.Lerp(-5, -7, Selected), nfloat.Lerp(2, 1, Selected), nfloat.Lerp(4, 8, Selected), nfloat.Lerp(0, 4, Selected)));
        context.ClosePath();

        context.SetStroke(stroke);
        context.Stroke();

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

        context.Rect((nfloat.Lerp(-5, -7, Selected), nfloat.Lerp(2, 1, Selected), nfloat.Lerp(4, 8, Selected), nfloat.Lerp(0, 4, Selected)));
        context.ClosePath();

        context.SetFill(fill);
        context.Fill(FillRule.EvenOdd);

        context.Restore();
    }
}
