using Xui.Core.Canvas;

namespace Xui.Apps.XuiSDK.Icons;

public class HomeIcon : INavIcon
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

        nfloat six = nfloat.Lerp(6, 8, Selected);
        nfloat two = nfloat.Lerp(2, 3, Selected);

        context.BeginPath();
        context.MoveTo((-two, six));
        context.ArcTo((-six, six), (-six, 0), 2);
        context.ArcTo((-six, 0), (-six, -two), 2);
        context.ArcTo((-six, -two), (0, -six), 2);
        context.ArcTo((0, -six), (six, -two), 2);
        context.ArcTo((six, -two), (six, 0), 2);
        context.ArcTo((six, 0), (six, six), 2);
        context.ArcTo((six, six), (two, six), 2);
        context.LineTo((two, six));
        context.ArcTo((two, 0), (-two, 0), 2);
        context.ArcTo((-two, 0), (-two, six), 2);
        context.ClosePath();

        context.SetStroke(stroke);
        context.Stroke();

        context.BeginPath();
        context.MoveTo((-two, six));
        context.ArcTo((-six, six), (-six, 0), 2);
        context.ArcTo((-six, 0), (-six, -two), 2);
        context.ArcTo((-six, -two), (0, -six), 2);
        context.ArcTo((0, -six), (six, -two), 2);
        context.ArcTo((six, -two), (six, 0), 2);
        context.ArcTo((six, 0), (six, six), 2);
        context.ArcTo((six, six), (two, six), 2);
        context.LineTo((two, six));
        context.ArcTo((two, 0), (-two, 0), 2);
        context.ArcTo((-two, 0), (-two, six), 2);
        context.ClosePath();

        context.SetFill(fill);
        context.Fill();

        context.Restore();
    }
}
