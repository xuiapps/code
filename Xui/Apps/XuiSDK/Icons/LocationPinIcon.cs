using Xui.Core.Canvas;

namespace Xui.Apps.XuiSDK.Icons;

public class LocationPinIcon : INavIcon
{
    public nfloat Selected { get; set; }
    public Color Color { get; set; }
    public Color SelectedColor { get; set; }

    public void Render(IContext context)
    {
        context.Save();
        context.BeginPath();
        context.LineWidth = nfloat.Lerp(1.25f, 2.25f, Selected);

        var stroke = new Color(
            nfloat.Lerp(Color.Red, SelectedColor.Red, Selected),
            nfloat.Lerp(Color.Green, SelectedColor.Green, Selected),
            nfloat.Lerp(Color.Blue, SelectedColor.Blue, Selected),
            nfloat.Lerp(Color.Alpha, SelectedColor.Alpha, Selected)
        );
        var fill = new Color(stroke.Red, stroke.Green, stroke.Blue, nfloat.Lerp(0f, stroke.Alpha, Selected));

        nfloat side = nfloat.Lerp(18, 24, Selected);
        nfloat top = nfloat.Lerp(12, 17, Selected);
        nfloat bottom = nfloat.Lerp(6, 8, Selected);

        nfloat r = nfloat.Lerp(2, 4, Selected);
        nfloat y = nfloat.Lerp(-2, -4, Selected);

        context.BeginPath();
        context.MoveTo((0, bottom));
        context.CurveTo((-side, -top), (side, -top), (0, bottom));
        context.ClosePath();

        context.MoveTo((-r, y));
        context.Ellipse((0, y), r, r, 0, -nfloat.Pi, nfloat.Pi, Winding.ClockWise);
        context.ClosePath();

        context.SetStroke(stroke);
        context.Stroke();

        context.BeginPath();
        context.MoveTo((0, bottom));
        context.CurveTo((-side, -top), (side, -top), (0, bottom));
        context.ClosePath();

        context.MoveTo((-r, y));
        context.Ellipse((0, y), r, r, 0, -nfloat.Pi, nfloat.Pi, Winding.ClockWise);
        context.ClosePath();

        context.SetFill(fill);
        context.Fill(FillRule.EvenOdd);

        context.Restore();
    }
}
