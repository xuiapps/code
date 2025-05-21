using System.Globalization;
using System.Text;
using Xui.Core.Curves2D;

namespace Xui.Core.Math2D;

public class CubicBezierTest
{
    [Fact]
    public void ToQuadratics32()
    {
        // A gentle S-curve with some curvature
        var cubic = new CubicBezier(
            new Point(0, 0),
            new Point(25, 100),
            new Point(75, -100),
            new Point(100, 0)
        );

        Span<QuadraticBezier> quadratics = stackalloc QuadraticBezier[32];
        cubic.ToQuadratics(quadratics, 0.25f, out int count);

        StringBuilder sb = new StringBuilder();
        sb.AppendLine($"Generated {count} quadratic segments:");

        var fmt = "0.00";
        var ci = CultureInfo.InvariantCulture;

        for (int i = 0; i < count; i++)
        {
            var q = quadratics[i];
            string p0 = $"({q.P0.X.ToString(fmt, ci)}, {q.P0.Y.ToString(fmt, ci)})";
            string p1 = $"({q.P1.X.ToString(fmt, ci)}, {q.P1.Y.ToString(fmt, ci)})";
            string p2 = $"({q.P2.X.ToString(fmt, ci)}, {q.P2.Y.ToString(fmt, ci)})";

            sb.AppendLine($"  {i + 1}. Segment {p0}, {p1}, {p2}");
        }

        var actual = sb.ToString();

        var expected = @"Generated 28 quadratic segments:
  1. Segment (0.00, 0.00), (2.03, 7.31), (2.03, 7.31)
  2. Segment (2.03, 7.31), (4.16, 13.43), (4.16, 13.43)
  3. Segment (4.16, 13.43), (6.39, 18.42), (6.39, 18.42)
  4. Segment (6.39, 18.42), (8.70, 22.36), (8.70, 22.36)
  5. Segment (8.70, 22.36), (11.10, 25.31), (11.10, 25.31)
  6. Segment (11.10, 25.31), (13.57, 27.33), (13.57, 27.33)
  7. Segment (13.57, 27.33), (16.12, 28.50), (16.12, 28.49)
  8. Segment (16.12, 28.49), (18.73, 28.87), (18.73, 28.87)
  9. Segment (18.73, 28.87), (22.39, 28.23), (22.39, 28.22)
  10. Segment (22.39, 28.22), (26.15, 26.39), (26.15, 26.39)
  11. Segment (26.15, 26.39), (30.00, 23.55), (30.00, 23.54)
  12. Segment (30.00, 23.54), (33.91, 19.85), (33.91, 19.85)
  13. Segment (33.91, 19.85), (41.90, 10.66), (41.90, 10.60)
  14. Segment (41.90, 10.60), (50.00, 0.06), (50.00, 0.00)
  15. Segment (50.00, 0.00), (58.10, -10.54), (58.10, -10.60)
  16. Segment (58.10, -10.60), (66.08, -19.79), (66.09, -19.85)
  17. Segment (66.09, -19.85), (70.00, -23.53), (70.00, -23.54)
  18. Segment (70.00, -23.54), (73.85, -26.38), (73.85, -26.39)
  19. Segment (73.85, -26.39), (77.61, -28.21), (77.61, -28.22)
  20. Segment (77.61, -28.22), (81.27, -28.86), (81.27, -28.87)
  21. Segment (81.27, -28.87), (83.88, -28.49), (83.88, -28.49)
  22. Segment (83.88, -28.49), (86.43, -27.33), (86.43, -27.33)
  23. Segment (86.43, -27.33), (88.90, -25.30), (88.90, -25.31)
  24. Segment (88.90, -25.31), (91.30, -22.36), (91.30, -22.36)
  25. Segment (91.30, -22.36), (93.61, -18.42), (93.61, -18.42)
  26. Segment (93.61, -18.42), (95.84, -13.42), (95.84, -13.43)
  27. Segment (95.84, -13.43), (97.97, -7.30), (97.97, -7.31)
  28. Segment (97.97, -7.31), (100.00, 0.00), (100.00, 0.00)
";

        Assert.InRange(count, 1, 32);

        Assert.Equal(expected, actual);
    }
}
