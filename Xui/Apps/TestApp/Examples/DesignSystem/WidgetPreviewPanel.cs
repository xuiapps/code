using Xui.Core.Canvas;
using Xui.Core.Math2D;
using Xui.Core.UI;

namespace Xui.Apps.TestApp.Examples.DesignSystem;

/// <summary>
/// Right panel: widget preview area (placeholder for now).
/// </summary>
internal class WidgetPreviewPanel : View
{
    private readonly DesignSystemDemoView owner;

    public WidgetPreviewPanel(DesignSystemDemoView owner) => this.owner = owner;

    public override int Count => 0;
    public override View this[int index] => throw new IndexOutOfRangeException();

    protected override Size MeasureCore(Size available, IMeasureContext context)
    {
        return available;
    }

    protected override void RenderCore(IContext context)
    {
        var ds = owner.DesignSystem;
        if (ds == null) return;

        context.SetFill(ds.Colors.Surface.Background);
        context.FillRect(this.Frame);

        context.SetFont(new() { FontFamily = ["Inter"], FontSize = 14 });
        context.TextBaseline = TextBaseline.Top;
        context.SetFill(new Color(0x999999FF));
        context.FillText("Widget Preview", new Point(this.Frame.X + 24, this.Frame.Y + 24));
    }
}
