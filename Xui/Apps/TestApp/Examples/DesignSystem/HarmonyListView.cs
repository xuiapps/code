using System.Runtime.InteropServices;

using Xui.Core.Canvas;
using Xui.Core.Math2D;
using Xui.Core.UI;
using Xui.Core.UI.Input;
using Xui.DevKit.UI.Design;

namespace Xui.Apps.TestApp.Examples.DesignSystem;

/// <summary>
/// Vertical list of harmony labels, each a clickable row with hover/active states.
/// </summary>
internal class HarmonyListView : View
{
    private readonly DesignSystemDemoView owner;
    private int hoveredIndex = -1;

    private static readonly string[] harmonyNames =
        ["Complementary", "Analogous", "Split Compl.", "Triadic", "Tetradic"];
    private static readonly ColorHarmony[] harmonies =
        [ColorHarmony.Complementary, ColorHarmony.Analogous, ColorHarmony.SplitComplementary, ColorHarmony.Triadic, ColorHarmony.Tetradic];

    private static readonly NFloat RowHeight = 32;

    public HarmonyListView(DesignSystemDemoView owner) => this.owner = owner;

    public override int Count => 0;
    public override View this[int index] => throw new IndexOutOfRangeException();

    private int HitRow(NFloat globalY)
    {
        var idx = (int)((globalY - this.Frame.Y) / RowHeight);
        return idx >= 0 && idx < harmonies.Length ? idx : -1;
    }

    public override void OnPointerEvent(ref PointerEventRef e, EventPhase phase)
    {
        if (phase == EventPhase.Tunnel)
        {
            if (e.Type == PointerEventType.Down)
            {
                var idx = HitRow(e.State.Position.Y);
                if (idx >= 0)
                    owner.SetHarmony(harmonies[idx]);
            }
            else if (e.Type == PointerEventType.Move || e.Type == PointerEventType.Enter)
            {
                var idx = HitRow(e.State.Position.Y);
                if (idx != hoveredIndex)
                {
                    hoveredIndex = idx;
                    this.InvalidateRender();
                }
            }
            else if (e.Type == PointerEventType.Leave)
            {
                if (hoveredIndex != -1)
                {
                    hoveredIndex = -1;
                    this.InvalidateRender();
                }
            }
        }
        base.OnPointerEvent(ref e, phase);
    }

    protected override Size MeasureCore(Size available, IMeasureContext context) => available;

    protected override void RenderCore(IContext context)
    {
        var ds = owner.DesignSystem;
        if (ds == null) return;

        context.SetFont(new() { FontFamily = ["Inter"], FontSize = 13, FontWeight = FontWeight.Medium });
        context.TextBaseline = TextBaseline.Top;

        for (int i = 0; i < harmonies.Length; i++)
        {
            bool isActive = harmonies[i] == owner.Harmony;
            bool isHovered = i == hoveredIndex;
            NFloat y = this.Frame.Y + i * RowHeight;

            var rowRect = new Rect(this.Frame.X + 2, y + 1, this.Frame.Width - 4, RowHeight - 2);
            if (isActive)
            {
                context.BeginPath();
                context.RoundRect(rowRect, 4);
                context.SetFill(ds.Colors.Primary.Container);
                context.Fill(FillRule.NonZero);
                context.SetStroke(ds.Colors.Primary.Background);
                context.LineWidth = 1;
                context.Stroke();
            }
            else if (isHovered)
            {
                context.BeginPath();
                context.RoundRect(rowRect, 4);
                context.SetFill(ds.Colors.Surface.Container);
                context.Fill(FillRule.NonZero);
                context.SetStroke(ds.Colors.Outline);
                context.LineWidth = 1;
                context.Stroke();
            }

            // Hardcoded text colors to avoid NSColorRef crash with OKLCH colors in FillText
            context.SetFill(isActive ? new Color(0x1A3366FF) : new Color(0x444444FF));
            context.FillText(harmonyNames[i], new Point(this.Frame.X + 12, y + 8));
        }
    }
}
