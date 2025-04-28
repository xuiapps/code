using System;
using Xui.Core.Canvas;
using Xui.Runtime.Software.Rasterization;

namespace Xui.Runtime.Software;

public partial class SoftwareContext : IPathDrawing
{
    public void Fill(FillRule rule = FillRule.NonZero)
    {
        // Step 1: Clear the stencil buffer (important!)
        stencil.Clear();

        // Step 2: Create a rasterizer and replay the path into it
        var rasterizer = new Rasterizer();
        path.Visit(rasterizer);

        // Step 3: Rasterize into the stencil with antialiasing
        rasterizer.Rasterize(stencil, rule);

        // Step 4: Composite the stencil onto the bitmap
        CompositeFill();
    }

    public void Stroke()
    {
        throw new NotImplementedException();
    }

    private void CompositeFill()
    {
        for (uint y = 0; y < bitmap.Height; y++)
        {
            for (uint x = 0; x < bitmap.Width; x++)
            {
                ushort coverage = stencil[x, y].Gray; // logical 0..256
                if (coverage > 0)
                {
                    byte alpha8 = (byte)Math.Min((ushort)255, coverage);
                    var color = new RGBA(255, 0, 255, alpha8);
                    bitmap.Blend(x, y, color);
                }
            }
        }
    }
}
