using Xui.Core.Canvas;
using Xui.Core.Math2D;
using Xui.Core.UI;
using static Xui.Core.Canvas.Colors;

namespace Xui.Apps.TestApp.Pages.Canvas.Tests;

/// <summary>Demonstrates a horizontal linear gradient used as text fill.</summary>
public sealed class RainbowTextFillTest : View
{
    protected override void RenderCore(IContext context)
    {
        context.SetFill(White);
        context.FillRect(this.Frame);

        context.SetFill(new LinearGradient(
            new Point(this.Frame.X + 50, this.Frame.Center.Y),
            new Point(this.Frame.Right - 50, this.Frame.Center.Y),
            [
                new(0.00f, new Color(0xE6, 0x39, 0x46, 0xFF)),
                new(0.17f, new Color(0xF4, 0xA2, 0x61, 0xFF)),
                new(0.33f, new Color(0xF9, 0xC7, 0x4F, 0xFF)),
                new(0.50f, new Color(0x2A, 0x9D, 0x8F, 0xFF)),
                new(0.67f, new Color(0x45, 0x76, 0xB5, 0xFF)),
                new(0.83f, new Color(0x5E, 0x54, 0xA4, 0xFF)),
                new(1.00f, new Color(0x9B, 0x5D, 0xA5, 0xFF)),
            ]));
        context.SetFont(new Font(112, "Inter", FontWeight.Bold));
        context.TextAlign = TextAlign.Center;
        context.TextBaseline = TextBaseline.Alphabetic;
        context.FillText("Rainbow", (this.Frame.Center.X, this.Frame.Center.Y + 40));
    }
}
