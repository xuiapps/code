using System.Runtime.InteropServices;
using Xui.Core.Canvas;

namespace Xui.Apps.BlankApp;

public class Home : Xui.Core.UI.IDrawable
{
    public static readonly Home Instance = new Home();

    public NFloat Selected { get; set; }
    
    public Color Color { get; set; }
    public Color SelectedColor { get; set; }

    public void Render(IContext context)
    {
        context.Save();
        context.LineWidth = NFloat.Lerp(1.25f, 2.25f, this.Selected);

        var stroke = new Color(
            NFloat.Lerp(this.Color.Red, this.SelectedColor.Red, this.Selected),
            NFloat.Lerp(this.Color.Green, this.SelectedColor.Green, this.Selected),
            NFloat.Lerp(this.Color.Blue, this.SelectedColor.Blue, this.Selected),
            NFloat.Lerp(this.Color.Alpha, this.SelectedColor.Alpha, this.Selected)
        );
        var fill = new Color(stroke.Red, stroke.Green, stroke.Blue, NFloat.Lerp(0f, stroke.Alpha, this.Selected));

        NFloat six = NFloat.Lerp(6, 8, this.Selected);
        NFloat two = NFloat.Lerp(2, 3, this.Selected);

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
