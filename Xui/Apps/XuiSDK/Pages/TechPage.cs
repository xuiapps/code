using Xui.Core.Canvas;
using Xui.Core.Math2D;
using Xui.Core.UI;

namespace Xui.Apps.XuiSDK.Pages;

public class TechPage : View
{
    protected override Size MeasureCore(Size availableBorderEdgeSize, IMeasureContext context)
    {
        return availableBorderEdgeSize;
    }

    protected override void RenderCore(IContext context)
    {
        var rect = this.Frame;

        context.SetFont(new Font(32, ["Segoe UI"], fontWeight: FontWeight.Light));
        context.TextBaseline = TextBaseline.Top;
        context.TextAlign = TextAlign.Left;
        context.SetFill(new Color(0x1A1A1AFF));
        context.FillText("Technology", new Point(rect.X + 40, rect.Y + 40));

        context.SetFont(new Font(14, ["Segoe UI"]));
        context.SetFill(new Color(0x606060FF));
        context.FillText("Core technologies powering Xui", new Point(rect.X + 40, rect.Y + 82));

        nfloat itemY = rect.Y + 130;
        string[] items =
        [
            ".NET 10 \u2014 Latest runtime with AOT compilation support",
            "2D Canvas API \u2014 HTML5 Canvas-style drawing primitives",
            "Layout Engine \u2014 Measure/Arrange/Render pipeline",
            "Pointer Events \u2014 W3C-compliant input handling",
            "Platform Backends \u2014 Win32, Cocoa, UIKit, Android, Browser",
        ];

        context.SetFont(new Font(14, ["Segoe UI"]));
        foreach (var item in items)
        {
            // Bullet
            context.SetFill(new Color(0x0078D4FF));
            context.BeginPath();
            context.Arc(new Point(rect.X + 52, itemY + 9), 4, 0, nfloat.Pi * 2);
            context.Fill();

            // Text
            context.SetFill(new Color(0x333333FF));
            context.TextBaseline = TextBaseline.Top;
            context.TextAlign = TextAlign.Left;
            context.FillText(item, new Point(rect.X + 68, itemY));
            itemY += 36;
        }
    }
}
