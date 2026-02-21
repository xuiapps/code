using System.Runtime.InteropServices;
using Xui.Core.Canvas;

namespace Xui.Apps.BlankApp;

public class LocationPin
{
    public static readonly LocationPin Instance = new LocationPin();

    public NFloat Selected { get; set; }
    
    public Xui.Core.Canvas.Color Color { get; set; }
    public Xui.Core.Canvas.Color SelectedColor { get; set; }

    public void Render(IContext context)
    {
        context.Save();
        context.BeginPath();
        context.LineWidth = NFloat.Lerp(1.25f, 2.25f, this.Selected);

        var stroke = new Xui.Core.Canvas.Color(
            NFloat.Lerp(this.Color.Red, this.SelectedColor.Red, this.Selected),
            NFloat.Lerp(this.Color.Green, this.SelectedColor.Green, this.Selected),
            NFloat.Lerp(this.Color.Blue, this.SelectedColor.Blue, this.Selected),
            NFloat.Lerp(this.Color.Alpha, this.SelectedColor.Alpha, this.Selected)
        );
        var fill = new Xui.Core.Canvas.Color(stroke.Red, stroke.Green, stroke.Blue, NFloat.Lerp(0f, stroke.Alpha, this.Selected));

        NFloat side = NFloat.Lerp(18, 24, this.Selected);
        NFloat top = NFloat.Lerp(12, 17, this.Selected);
        NFloat bottom = NFloat.Lerp(6, 8, this.Selected);

        NFloat r = NFloat.Lerp(2, 4, this.Selected);
        NFloat y = NFloat.Lerp(-2, -4, this.Selected);

        context.BeginPath();
        context.MoveTo((0, bottom));
        context.CurveTo((-side, -top), (side, -top), (0, bottom));
        context.ClosePath();

        context.MoveTo((-r, y));
        context.Ellipse((0, y), r, r, 0, -NFloat.Pi, NFloat.Pi, Winding.ClockWise);
        context.ClosePath();

        context.SetStroke(stroke);
        context.Stroke();

        context.BeginPath();
        context.MoveTo((0, bottom));
        context.CurveTo((-side, -top), (side, -top), (0, bottom));
        context.ClosePath();

        context.MoveTo((-r, y));
        context.Ellipse((0, y), r, r, 0, -NFloat.Pi, NFloat.Pi, Winding.ClockWise);
        context.ClosePath();

        context.SetFill(fill);
        context.Fill(FillRule.EvenOdd);

        context.Restore();

    }
}
