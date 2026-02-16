using Xui.Core.Canvas;
using Xui.Core.Math2D;
using Xui.Core.UI;

namespace Xui.Apps.XuiSDK.Pages;

public class ArchitecturePage : View
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
        context.FillText("Architecture", new Point(rect.X + 40, rect.Y + 40));

        context.SetFont(new Font(14, ["Segoe UI"]));
        context.SetFill(new Color(0x606060FF));
        context.FillText("Layered architecture for maximum portability", new Point(rect.X + 40, rect.Y + 82));

        // Architecture layer boxes
        DrawLayerBox(context, rect.X + 40, rect.Y + 130, 560, 50,
            "Application Layer", "Your App, Views, and Business Logic", new Color(0x0078D4FF));
        DrawLayerBox(context, rect.X + 40, rect.Y + 195, 560, 50,
            "Xui.Core", "Abstract Window, View, Layout, Canvas, Input", new Color(0x107C10FF));
        DrawLayerBox(context, rect.X + 40, rect.Y + 260, 560, 50,
            "Xui.Runtime.*", "Win32, macOS (Cocoa), iOS, Android, Browser", new Color(0x8764B8FF));
        DrawLayerBox(context, rect.X + 40, rect.Y + 325, 560, 50,
            "Platform APIs", "Direct2D, CoreGraphics, Skia, WebGL", new Color(0xD83B01FF));

        // Arrows between layers
        for (int i = 0; i < 3; i++)
        {
            nfloat arrowY = rect.Y + 182 + i * 65;
            context.SetStroke(new Color(0xB0B0B0FF));
            context.LineWidth = 1;
            context.BeginPath();
            context.MoveTo(new Point(rect.X + 320, arrowY));
            context.LineTo(new Point(rect.X + 320, arrowY + 11));
            context.Stroke();
        }
    }

    private static void DrawLayerBox(IContext context, nfloat x, nfloat y, nfloat w, nfloat h,
        string title, string subtitle, Color accentColor)
    {
        // Background
        context.SetFill(new Color(0xFFFFFF80));
        context.BeginPath();
        context.RoundRect(new Rect(x, y, w, h), 6);
        context.Fill();

        // Border
        context.SetStroke(new Color(0x00000015));
        context.LineWidth = 1;
        context.BeginPath();
        context.RoundRect(new Rect(x, y, w, h), 6);
        context.Stroke();

        // Left accent bar
        context.SetFill(accentColor);
        context.BeginPath();
        context.RoundRect(new Rect(x, y + 4, 4, h - 8), 2);
        context.Fill();

        // Title
        context.SetFont(new Font(14, ["Segoe UI"], fontWeight: FontWeight.SemiBold));
        context.TextBaseline = TextBaseline.Top;
        context.TextAlign = TextAlign.Left;
        context.SetFill(new Color(0x1A1A1AFF));
        context.FillText(title, new Point(x + 20, y + 8));

        // Subtitle
        context.SetFont(new Font(12, ["Segoe UI"]));
        context.SetFill(new Color(0x606060FF));
        context.FillText(subtitle, new Point(x + 20, y + 28));
    }
}
