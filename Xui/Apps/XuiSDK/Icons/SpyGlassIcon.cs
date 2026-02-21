using Xui.Core.Canvas;

namespace Xui.Apps.XuiSDK.Icons;

public class SpyGlassIcon : INavIcon
{
    public nfloat Selected { get; set; }
    public Color Color { get; set; }
    public Color SelectedColor { get; set; }

    public void Render(IContext context)
    {
        context.Save();

        context.BeginPath();
        var stroke = new Color(
            nfloat.Lerp(Color.Red, SelectedColor.Red, Selected),
            nfloat.Lerp(Color.Green, SelectedColor.Green, Selected),
            nfloat.Lerp(Color.Blue, SelectedColor.Blue, Selected),
            nfloat.Lerp(Color.Alpha, SelectedColor.Alpha, Selected)
        );
        var fill = new Color(stroke.Red, stroke.Green, stroke.Blue, nfloat.Lerp(0f, stroke.Alpha * 0.2f, Selected));

        nfloat r = nfloat.Lerp(6, 7, Selected);
        nfloat handleLen = nfloat.Lerp(3.5f, 4.5f, Selected);

        context.LineWidth = nfloat.Lerp(1.5f, 2.5f, Selected);

        // Glass circle
        context.BeginPath();
        context.Ellipse((-2, -2), r, r, 0, 0, nfloat.Pi * 2, Winding.ClockWise);
        context.SetStroke(stroke);
        context.Stroke();

        // Fill on select
        context.BeginPath();
        context.Ellipse((-2, -2), r, r, 0, 0, nfloat.Pi * 2, Winding.ClockWise);
        context.SetFill(fill);
        context.Fill();

        // Handle
        context.LineWidth = nfloat.Lerp(2.5f, 3.5f, Selected);
        context.LineCap = LineCap.Round;
        context.BeginPath();
        nfloat hx = -2 + r * 0.707f;
        nfloat hy = -2 + r * 0.707f;
        context.MoveTo((hx, hy));
        context.LineTo((hx + handleLen, hy + handleLen));
        context.SetStroke(stroke);
        context.Stroke();

        context.Restore();
    }
}
