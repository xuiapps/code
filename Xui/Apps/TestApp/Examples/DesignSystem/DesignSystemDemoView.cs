using System.Runtime.InteropServices;

using Xui.Core.DI;
using Xui.Core.UI.Layout;
using Xui.DevKit.UI.Design;
using static Xui.Core.UI.Layout.Grid;
using static Xui.Core.UI.Layout.Grid.TrackSize;

namespace Xui.Apps.TestApp.Examples.DesignSystem;

/// <summary>
/// Two-column Grid layout: left panel (color wheel + palettes), right panel (widget preview).
/// </summary>
internal class DesignSystemDemoView : Grid
{
    private NFloat primaryHue = 240;
    private ColorHarmony harmony = ColorHarmony.SplitComplementary;
    private XuiDesignSystem? designSystem;

    private readonly ColorPickerPanel leftPanel;
    private readonly WidgetPreviewPanel rightPanel;

    public DesignSystemDemoView()
    {
        TemplateColumns = [Px(460), Fr(1)];
        TemplateRows = [Fr(1)];

        leftPanel = new ColorPickerPanel(this) { [ColumnStart] = 1, [RowStart] = 1 };
        rightPanel = new WidgetPreviewPanel(this) { [ColumnStart] = 2, [RowStart] = 1 };

        this.Add(leftPanel);
        this.Add(rightPanel);
    }

    public XuiDesignSystem? DesignSystem => designSystem;
    public NFloat PrimaryHue => primaryHue;
    public ColorHarmony Harmony => harmony;

    /// <summary>
    /// Provides IDesignSystem to all child views via the parent-chain DI.
    /// </summary>
    public override object? GetService(Type serviceType)
    {
        if (serviceType == typeof(IDesignSystem) && designSystem != null)
            return designSystem;
        return base.GetService(serviceType);
    }

    protected override void OnActivate()
    {
        base.OnActivate();
        RebuildDesignSystem();
    }

    public void SetHue(NFloat hue)
    {
        primaryHue = hue;
        RebuildDesignSystem();
        this.InvalidateRender();
    }

    public void SetHarmony(ColorHarmony h)
    {
        harmony = h;
        RebuildDesignSystem();
        this.InvalidateRender();
    }

    private void RebuildDesignSystem()
    {
        var device = this.GetService(typeof(IDeviceInfo)) as IDeviceInfo;
        if (device == null) return;

        designSystem = new XuiDesignSystem(
            new XuiDesignSystemOptions
            {
                PrimaryHue = primaryHue,
                Harmony = harmony,
                Chroma = 0.15f,
            },
            device
        );
    }
}
