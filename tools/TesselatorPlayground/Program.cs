using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Globalization;
using Xui.Core.Canvas;
using Xui.Core.Math2D;
using Xui.Runtime.Software.Tessellate;
using System.Runtime.InteropServices;

namespace TesselatorPlayground
{
    internal class Program
    {
        static void Main()
        {
            // FlattenToSVG(BuildHeartPath(), FillRule.NonZero, "tessellation_output_heart_nonzero.svg");
            // FlattenToSVG(BuildHeartPath(), FillRule.EvenOdd, "tessellation_output_heart_evenodd.svg");

            // FlattenToSVG(BuildOPath(), FillRule.NonZero, "tessellation_output_o_nonzero.svg");
            // FlattenToSVG(BuildOPath(), FillRule.EvenOdd, "tessellation_output_o_evenodd.svg");
        }

        private static Path2D BuildHeartPath()
        {
            var path = new Path2D();
            path.MoveTo(new Point(60, 20));
            path.CurveTo(new Point(100, 0), new Point(100, 60), new Point(60, 80));
            path.CurveTo(new Point(20, 60), new Point(20, 0), new Point(60, 20));
            path.ClosePath();
            return path;
        }

        private static Path2D BuildOPath()
        {
            var path = new Path2D();

            // Outer loop (clockwise)
            path.MoveTo(new Point(60, 20));
            path.CurveTo(new Point(100, 20), new Point(100, 80), new Point(60, 80));
            path.CurveTo(new Point(20, 80), new Point(20, 20), new Point(60, 20));
            path.ClosePath();

            // Inner hole (clockwise — to ensure it's a separate loop with distinct winding)
            path.MoveTo(new Point(60, 35));
            path.CurveTo(new Point(80, 35), new Point(80, 65), new Point(60, 65));
            path.CurveTo(new Point(40, 65), new Point(40, 35), new Point(60, 35));
            path.ClosePath();

            return path;
        }

        // private static void FlattenToSVG(Path2D path, FillRule fillRule, string pathToFile)
        // {
        //     var flattener = new PathFlattener(fillRule, flatness: 0.5f);
        //     path.Visit(flattener);
        //     var triangles = flattener.FlattenToTriangles();
        //     var svg = GenerateSvg(triangles, 120, 100);
        //     File.WriteAllText(pathToFile, svg);
        //     Console.WriteLine("Saved SVG to: " + pathToFile);
        // }

        // static string GenerateSvg(List<Triangle> triangles, int width, int height)
        // {
        //     var sb = new StringBuilder();
        //     sb.AppendLine("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
        //     sb.AppendLine($"<svg xmlns=\"http://www.w3.org/2000/svg\" width=\"{width}\" height=\"{height}\" viewBox=\"0 0 {width} {height}\">");

        //     foreach (var tri in triangles)
        //     {
        //         string points = $"{F(tri.A.X)},{F(tri.A.Y)} {F(tri.B.X)},{F(tri.B.Y)} {F(tri.C.X)},{F(tri.C.Y)}";
        //         sb.AppendLine($"  <polygon points=\"{points}\" fill=\"#80c0ff\" stroke=\"#003366\" stroke-width=\"1\" />");
        //     }

        //     sb.AppendLine("</svg>");
        //     return sb.ToString();
        // }

        // static string F(NFloat value) => value.ToString(CultureInfo.InvariantCulture);
    }
}
