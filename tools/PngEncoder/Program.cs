using Xui.Core.Math2D;
using Xui.Runtime.Software;

namespace PngEncoder
{
    /// <summary>
    /// Test drive for the Software platform's PngEncoder...
    /// </summary>
    internal class Program
    {
        static void Main()
        {
            const uint width = 256;
            const uint height = 256;

            var ctx = new SoftwareContext(width, height);

            // --- Draw something! ---
            ctx.BeginPath();
            ctx.MoveTo(new Point(50, 50));
            ctx.LineTo(new Point(200, 50));
            ctx.LineTo(new Point(200, 75));
            ctx.LineTo(new Point(50, 50));
            ctx.ClosePath();

            ctx.MoveTo(new Point(50, 200));
            ctx.CurveTo(
                new Point(50, 100),      // Control point 1
                new Point(200, 100),    // Control point 2
                new Point(200, 200)     // End point
            );
            ctx.ClosePath();

            ctx.Fill(); // Fill all paths

            // --- Save output ---
            Xui.Runtime.Software.PngEncoder.SaveRGBA("output.png", ctx.bitmap);

            Console.WriteLine("Saved output.png");

        }
    }
}
