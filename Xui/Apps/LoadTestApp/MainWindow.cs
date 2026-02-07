using System.Runtime.InteropServices;
using Xui.Core.Abstract.Events;
using Xui.Core.Canvas;
using Xui.Core.Math2D;
using Window = Xui.Core.Abstract.Window;

namespace Xui.Apps.LoadTestApp;

public class MainWindow : Window
{
    private NFloat CellWidth = 80;
    private NFloat CellHeight = 22;
    private NFloat ScrollY = 0;

    public override void Render(ref RenderEventRef render)
    {
        long allocBefore = GC.GetAllocatedBytesForCurrentThread();

        using var ctx = Xui.Core.Actual.Runtime.DrawingContext!;

        // Clear background
        ctx.SetFill(Colors.Black);
        ctx.FillRect(render.Rect);

        // Calculate scroll offset based on time (infinite vertical scroll)
        this.ScrollY += 2;

        // Calculate visible grid range
        int startRow = (int)Math.Floor((double)(this.ScrollY / CellHeight));
        int endRow = (int)Math.Ceiling((double)((this.ScrollY + render.Rect.Height) / CellHeight));
        int startCol = 0;
        int endCol = (int)Math.Ceiling((double)(render.Rect.Width / CellWidth));

        // Set up font for cell text
        ctx.SetFont(new Font
        {
            FontFamily = ["Inter", "sans-serif"],
            FontSize = 11,
            FontWeight = 400
        });
        ctx.TextAlign = TextAlign.Center;
        ctx.TextBaseline = TextBaseline.Middle;

        // Draw visible cells
        for (int row = startRow; row <= endRow; row++)
        {
            for (int col = startCol; col <= endCol; col++)
            {
                // Calculate cell position (offset by scroll)
                NFloat x = col * CellWidth;
                NFloat y = row * CellHeight - this.ScrollY;

                // Create RGB from position using sine waves for smooth color transitions (0-1 range)
                NFloat r = (NFloat)(0.5 + 0.5 * Math.Sin(col * 0.15 + row * 0.02));
                NFloat g = (NFloat)(0.5 + 0.5 * Math.Sin(col * 0.1 + row * 0.03 + 2));
                NFloat b = (NFloat)(0.5 + 0.5 * Math.Sin(col * 0.12 + row * 0.025 + 4));

                // Check if mouse is over this cell
                var cellRect = new Rect(x, y, CellWidth - 1, CellHeight - 1);

                ctx.SetFill(new Color(r, g, b, 1));
                ctx.FillRect(cellRect);

                // Draw cell text (coordinates)
                // Use contrasting color for text
                NFloat brightness = (r + g + b) / 3;
                ctx.SetFill(brightness > 0.5 ? Colors.Black : Colors.White);
                ctx.FillText($"{col},{row}", (x + CellWidth / 2, y + CellHeight / 2));
            }
        }

        // Allocation overlay
        long allocAfter = GC.GetAllocatedBytesForCurrentThread();
        long allocated = allocAfter - allocBefore;

        ctx.SetFont(new Font
        {
            FontFamily = ["Inter", "sans-serif"],
            FontSize = 14,
            FontWeight = 700
        });
        ctx.TextAlign = TextAlign.Left;
        ctx.TextBaseline = TextBaseline.Top;

        ctx.SetFill(new Color(0, 0, 0, 0.7f));
        ctx.FillRect(new Rect(8, 8, 220, 24));
        ctx.SetFill(Colors.Lime);
        ctx.FillText($"Alloc: {allocated} bytes", (12, 12));

        // Request continuous animation
        Invalidate();

        base.Render(ref render);
    }
}
