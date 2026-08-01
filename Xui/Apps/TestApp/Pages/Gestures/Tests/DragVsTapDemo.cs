using System.Runtime.InteropServices;
using Xui.Core.Canvas;
using Xui.Core.Math2D;
using Xui.Core.UI;
using Xui.DevKit.UI.Widgets;
using static Xui.Core.Canvas.FontWeight;

namespace Xui.Apps.TestApp.Pages.Gestures.Tests;

/// <summary>
/// Vertical ScrollView with a mix of <see cref="Button"/> (captures ITap; the
/// ScrollView steals after 8pt of motion) and <see cref="ScratchPad"/> widgets
/// (capture IDrag; the ScrollView stands down for the whole gesture). Drag
/// across a button → the page scrolls. Drag across a ScratchPad → a stroke is
/// drawn and the page does not scroll.
/// </summary>
public class DragVsTapDemo : View
{
    private readonly ScrollView scroller;

    public DragVsTapDemo()
    {
        var stack = new VerticalStack
        {
            Margin = 16,
            Content =
            [
                new Label { Text = "Drag vs Tap", FontFamily = "Inter", FontSize = 18, FontWeight = SemiBold, Margin = (0, 0, 8, 0) },
                new Label { Text = "Buttons capture ITap → ScrollView steals after 8pt. ScratchPads capture IDrag → ScrollView stands down.", FontFamily = "Inter", FontSize = 13, Margin = (0, 0, 12, 0) },
            ]
        };

        for (int i = 0; i < 6; i++)
        {
            stack.Add(new Button { Text = $"Button #{i + 1}  (drag me — page scrolls)", Margin = (0, 0, 8, 0) });
            stack.Add(new ScratchPad { Margin = (0, 0, 16, 0) });
        }

        scroller = new ScrollView
        {
            Direction = ScrollDirection.Vertical,
            Content = stack,
        };
        AddProtectedChild(scroller);
    }

    public override int Count => 1;
    public override View this[int index] => index == 0 ? scroller : throw new IndexOutOfRangeException();

    protected override Size MeasureCore(Size available, IMeasureContext context)
    {
        scroller.Measure(available, context);
        return available;
    }

    protected override void ArrangeCore(Rect rect, IMeasureContext context)
    {
        scroller.Arrange(rect, context);
    }
}
