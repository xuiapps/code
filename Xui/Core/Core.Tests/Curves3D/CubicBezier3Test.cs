using Xui.Core.Math3D;
using Xui.Core.Curves3D;

namespace Xui.Core.Curves3D;

public class CubicBezier3Test
{
    [Fact]
    public void CubicBezier3_Lerp_AtEndpoints()
    {
        var curve = new CubicBezier3(
            new Point3(0f, 0f, 0f),
            new Point3(1f, 2f, 0f),
            new Point3(2f, 2f, 0f),
            new Point3(3f, 0f, 0f)
        );

        var start = curve.Lerp(0f);
        Assert.Equal(0f, start.X, 1e-5f);
        Assert.Equal(0f, start.Y, 1e-5f);
        Assert.Equal(0f, start.Z, 1e-5f);

        var end = curve.Lerp(1f);
        Assert.Equal(3f, end.X, 1e-5f);
        Assert.Equal(0f, end.Y, 1e-5f);
        Assert.Equal(0f, end.Z, 1e-5f);
    }

    [Fact]
    public void CubicBezier3_Lerp_Midpoint_SymmetricCurve()
    {
        // Symmetric control points: midpoint should lie on the symmetry axis.
        var curve = new CubicBezier3(
            new Point3(-1f, 0f, 0f),
            new Point3(-1f, 1f, 0f),
            new Point3(1f, 1f, 0f),
            new Point3(1f, 0f, 0f)
        );

        var mid = curve.Lerp(0.5f);
        Assert.Equal(0f, mid.X, 1e-5f);
        Assert.Equal(0.75f, mid.Y, 1e-5f);
        Assert.Equal(0f, mid.Z, 1e-5f);
    }

    [Fact]
    public void CubicBezier3_Indexer_EqualsLerp()
    {
        var curve = new CubicBezier3(
            new Point3(0f, 0f, 0f),
            new Point3(1f, 1f, 0f),
            new Point3(2f, 1f, 0f),
            new Point3(3f, 0f, 0f)
        );

        for (float t = 0f; t <= 1f; t += 0.1f)
        {
            var fromLerp = curve.Lerp(t);
            var fromIndexer = curve[t];
            Assert.Equal(fromLerp, fromIndexer);
        }
    }

    [Fact]
    public void CubicBezier3_Tangent_AtEndpoints()
    {
        var p0 = new Point3(0f, 0f, 0f);
        var p1 = new Point3(1f, 0f, 0f);
        var p2 = new Point3(2f, 0f, 0f);
        var p3 = new Point3(3f, 0f, 0f);
        var straight = new CubicBezier3(p0, p1, p2, p3);

        var t0 = straight.Tangent(0f);
        // Tangent at start: 3*(P1-P0) = (3,0,0)
        Assert.Equal(3f, t0.X, 1e-5f);
        Assert.Equal(0f, t0.Y, 1e-5f);
        Assert.Equal(0f, t0.Z, 1e-5f);

        var t1 = straight.Tangent(1f);
        // Tangent at end: 3*(P3-P2) = (3,0,0)
        Assert.Equal(3f, t1.X, 1e-5f);
        Assert.Equal(0f, t1.Y, 1e-5f);
        Assert.Equal(0f, t1.Z, 1e-5f);
    }

    [Fact]
    public void CubicBezier3_Subdivide_ReconstructsCurve()
    {
        var curve = new CubicBezier3(
            new Point3(0f, 0f, 0f),
            new Point3(1f, 2f, 1f),
            new Point3(2f, 2f, -1f),
            new Point3(3f, 0f, 0f)
        );

        var (left, right) = curve.Subdivide(0.5f);

        // Endpoints of sub-curves match the split point on the original.
        var mid = curve.Lerp(0.5f);
        Assert.Equal(mid.X, left.P3.X, 1e-5f);
        Assert.Equal(mid.Y, left.P3.Y, 1e-5f);
        Assert.Equal(mid.Z, left.P3.Z, 1e-5f);
        Assert.Equal(mid.X, right.P0.X, 1e-5f);
        Assert.Equal(mid.Y, right.P0.Y, 1e-5f);
        Assert.Equal(mid.Z, right.P0.Z, 1e-5f);

        // Outer endpoints match the original.
        Assert.Equal(curve.P0, left.P0);
        Assert.Equal(curve.P3, right.P3);
    }

    [Fact]
    public void CubicBezier3_Length_StraightLine()
    {
        var straight = new CubicBezier3(
            new Point3(0f, 0f, 0f),
            new Point3(1f, 0f, 0f),
            new Point3(2f, 0f, 0f),
            new Point3(3f, 0f, 0f)
        );

        Assert.Equal(3f, straight.Length(), 0.01f);
        Assert.Equal(3f, straight.Length(1e-4f), 0.01f);
    }

    [Fact]
    public void CubicBezier3_QuadraticApproximation_Endpoints()
    {
        var curve = new CubicBezier3(
            new Point3(0f, 0f, 0f),
            new Point3(1f, 2f, 0f),
            new Point3(2f, 2f, 0f),
            new Point3(3f, 0f, 0f)
        );

        var approx = curve.QuadraticApproximation;
        Assert.Equal(curve.P0, approx.P0);
        Assert.Equal(curve.P3, approx.P2);
    }

    [Fact]
    public void Bezier3_Factory_CreatesCorrectTypes()
    {
        var p0 = new Point3(0f, 0f, 0f);
        var p1 = new Point3(1f, 1f, 0f);
        var p2 = new Point3(2f, 1f, 0f);
        var p3 = new Point3(3f, 0f, 0f);

        var linear = Bezier3.Linear(p0, p1);
        var quadratic = Bezier3.Quadratic(p0, p1, p2);
        var cubic = Bezier3.Cubic(p0, p1, p2, p3);

        Assert.Equal(p0, linear.P0);
        Assert.Equal(p0, quadratic.P0);
        Assert.Equal(p0, cubic.P0);
        Assert.Equal(p3, cubic.P3);
    }

    [Fact]
    public void LinearBezier3_LerpEqualsEndpoints()
    {
        var seg = new LinearBezier3(new Point3(0f, 0f, 0f), new Point3(4f, 0f, 0f));
        Assert.Equal(seg.P0, seg.Lerp(0f));
        Assert.Equal(seg.P1, seg.Lerp(1f));

        var mid = seg.Lerp(0.5f);
        Assert.Equal(2f, mid.X, 1e-6f);
    }

    [Fact]
    public void LinearBezier3_Length()
    {
        var seg = new LinearBezier3(new Point3(0f, 0f, 0f), new Point3(3f, 4f, 0f));
        Assert.Equal(5f, seg.Length(), 1e-5f);
    }

    [Fact]
    public void QuadraticBezier3_LerpAtEndpoints()
    {
        var curve = new QuadraticBezier3(
            new Point3(0f, 0f, 0f),
            new Point3(1f, 2f, 0f),
            new Point3(2f, 0f, 0f)
        );
        Assert.Equal(curve.P0, curve.Lerp(0f));
        Assert.Equal(curve.P2, curve.Lerp(1f));
    }

    [Fact]
    public void QuadraticBezier3_Subdivide_ReconstructsCurve()
    {
        var curve = new QuadraticBezier3(
            new Point3(0f, 0f, 0f),
            new Point3(1f, 2f, 0f),
            new Point3(2f, 0f, 0f)
        );
        var (left, right) = curve.Subdivide(0.5f);
        var mid = curve.Lerp(0.5f);
        Assert.Equal(mid.X, left.P2.X, 1e-5f);
        Assert.Equal(mid.Y, left.P2.Y, 1e-5f);
        Assert.Equal(mid.X, right.P0.X, 1e-5f);
        Assert.Equal(curve.P0, left.P0);
        Assert.Equal(curve.P2, right.P2);
    }
}
