using Xui.Core.Canvas;
using Xui.Core.Math2D;
using Xui.Core.UI;

namespace Xui.Apps.XuiSDK.Pages;

public class ContactPage : View
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
        context.FillText("Contact", new Point(rect.X + 40, rect.Y + 40));

        context.SetFont(new Font(14, ["Segoe UI"]));
        context.SetFill(new Color(0x606060FF));
        context.FillText("Get in touch with the Xui team", new Point(rect.X + 40, rect.Y + 82));

        nfloat itemY = rect.Y + 140;
        (string label, string value)[] contacts =
        [
            ("GitHub", "github.com/nicknash/xui"),
            ("License", "MIT License"),
            ("Platform", ".NET 10+"),
        ];

        foreach (var (label, value) in contacts)
        {
            // Label
            context.SetFont(new Font(13, ["Segoe UI"], fontWeight: FontWeight.SemiBold));
            context.TextBaseline = TextBaseline.Top;
            context.TextAlign = TextAlign.Left;
            context.SetFill(new Color(0x0078D4FF));
            context.FillText(label, new Point(rect.X + 56, itemY));

            // Value
            context.SetFont(new Font(13, ["Segoe UI"]));
            context.SetFill(new Color(0x333333FF));
            context.FillText(value, new Point(rect.X + 160, itemY));

            itemY += 32;
        }
    }
}
