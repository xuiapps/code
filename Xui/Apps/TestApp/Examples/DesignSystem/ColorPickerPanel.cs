using System.Runtime.InteropServices;

using Xui.Core.Canvas;
using Xui.Core.Math2D;
using Xui.Core.UI;
using Xui.DevKit.UI.Design;

namespace Xui.Apps.TestApp.Examples.DesignSystem;

/// <summary>
/// Left panel: color wheel on left, harmony list on right, palette swatches below.
/// </summary>
internal class ColorPickerPanel : View
{
    private readonly DesignSystemDemoView owner;
    private readonly ColorWheelView wheel;
    private readonly HarmonyListView harmonyList;

    public ColorPickerPanel(DesignSystemDemoView owner)
    {
        this.owner = owner;
        wheel = new ColorWheelView(owner);
        harmonyList = new HarmonyListView(owner);
        this.AddProtectedChild(wheel);
        this.AddProtectedChild(harmonyList);
    }

    public override int Count => 2;
    public override View this[int index] => index == 0 ? wheel : harmonyList;

    protected override Size MeasureCore(Size available, IMeasureContext context)
    {
        NFloat wheelSide = 246;
        wheel.Measure(new Size(wheelSide, wheelSide), context);
        harmonyList.Measure(new Size(available.Width - wheelSide, wheelSide), context);
        return available;
    }

    protected override void ArrangeCore(Rect rect, IMeasureContext context)
    {
        NFloat wheelSide = 246;
        wheel.Arrange(new Rect(rect.X, rect.Y, wheelSide, wheelSide), context);
        harmonyList.Arrange(new Rect(rect.X + wheelSide, rect.Y, rect.Width - wheelSide, wheelSide), context);
    }

    protected override void RenderCore(IContext context)
    {
        var ds = owner.DesignSystem;
        if (ds == null) return;

        context.SetFill(ds.Colors.Application.Background);
        context.FillRect(this.Frame);

        base.RenderCore(context);

        // Palette swatches below
        NFloat swatchY = this.Frame.Y + 260;
        NFloat swatchX = this.Frame.X + 16;
        NFloat size = 36;
        NFloat gap = 4;
        NFloat groupGap = 16;

        DrawGroup(context, "Primary", ds.Colors.Primary, swatchX, swatchY, size, gap);
        DrawGroup(context, "Secondary", ds.Colors.Secondary, swatchX + (size * 2 + gap) + groupGap, swatchY, size, gap);
        DrawGroup(context, "Accent", ds.Colors.Accent, swatchX + ((size * 2 + gap) + groupGap) * 2, swatchY, size, gap);
        DrawGroup(context, "Error", ds.Colors.Error, swatchX + ((size * 2 + gap) + groupGap) * 3, swatchY, size, gap);

        NFloat row2Y = swatchY + size * 2 + gap + 24;
        DrawGroup(context, "Application", ds.Colors.Application, swatchX, row2Y, size, gap);
        DrawGroup(context, "Surface", ds.Colors.Surface, swatchX + (size * 2 + gap) + groupGap, row2Y, size, gap);
    }

    private static void DrawGroup(IContext context, string label, ColorGroup group,
        NFloat x, NFloat y, NFloat size, NFloat gap)
    {
        context.SetFont(new() { FontFamily = ["Inter"], FontSize = 11 });
        context.TextBaseline = TextBaseline.Top;
        context.SetFill(new Color(0x666666FF));
        context.FillText(label, new Point(x, y));

        y += 18;

        context.SetFill(group.Background);
        context.FillRect(new Rect(x, y, size, size));

        context.SetFill(group.Foreground);
        context.FillRect(new Rect(x + size + gap, y, size, size));

        context.SetFill(group.Container);
        context.FillRect(new Rect(x, y + size + gap, size, size));

        context.SetFill(group.OnContainer);
        context.FillRect(new Rect(x + size + gap, y + size + gap, size, size));
    }
}
