using Xui.Core.Math2D;

namespace Xui.Core.Curves2D;

/// <summary>
/// Represents a cubic Bézier curve defined by four control points.
/// </summary>
/// <remarks>
/// A cubic Bézier curve provides smooth interpolation between <see cref="P0"/> and <see cref="P3"/>,
/// influenced by the control points <see cref="P1"/> and <see cref="P2"/>. This is commonly used in vector graphics,
/// animation timing, and layout paths.
/// </remarks>
public readonly struct CubicBezier : ICurve
{
    /// <summary>The starting point of the curve.</summary>
    public readonly Point P0;

    /// <summary>The first control point, influencing the curve near <see cref="P0"/>.</summary>
    public readonly Point P1;

    /// <summary>The second control point, influencing the curve near <see cref="P3"/>.</summary>
    public readonly Point P2;

    /// <summary>The ending point of the curve.</summary>
    public readonly Point P3;

    /// <summary>
    /// Initializes a new cubic Bézier curve with the specified control points.
    /// </summary>
    public CubicBezier(Point p0, Point p1, Point p2, Point p3)
    {
        this.P0 = p0;
        this.P1 = p1;
        this.P2 = p2;
        this.P3 = p3;
    }

    /// <summary>
    /// Computes the interpolated point on the curve at parameter <paramref name="t"/> using De Casteljau’s algorithm.
    /// </summary>
    /// <param name="t">A normalized parameter in the range [0, 1].</param>
    public Point Lerp(nfloat t) =>
        Point.Lerp(
            Point.Lerp(
                Point.Lerp(P0, P1, t),
                Point.Lerp(P1, P2, t),
                t),
            Point.Lerp(
                Point.Lerp(P1, P2, t),
                Point.Lerp(P2, P3, t),
                t),
            t
        );

    public QuadraticBezier QuadraticApproximation
    {
        get
        {
            var p0 = this.P0;
            var p3 = this.P3;

            var v0 = (Vector)p0;
            var v3 = (Vector)p3;
            var v1 = (Vector)this.P1;
            var v2 = (Vector)this.P2;

            // Tangents at endpoints (scaled)
            var tangentStart = 3 * (v1 - v0);
            var tangentEnd = 3 * (v3 - v2);

            // Average the two tangents and add to the midpoint between endpoints
            var avgTangent = (tangentStart + tangentEnd) * 0.25f;
            var midpoint = Point.Lerp(p0, p3, 0.5f);
            var approxControl = midpoint + avgTangent;

            return new QuadraticBezier(p0, approxControl, p3);
        }
    }

    /// <summary>
    /// Computes the tangent vector of the curve at parameter <paramref name="t"/>.
    /// </summary>
    /// <param name="t">A normalized parameter in the range [0, 1].</param>
    public Vector Tangent(nfloat t) =>
        Vector.Lerp(
            Vector.Lerp(P1 - P0, P2 - P1, t),
            Vector.Lerp(P2 - P1, P3 - P2, t),
            t
        ) * 3;

    /// <summary>
    /// Approximates the arc length of the curve using uniform sampling with 16 segments.
    /// </summary>
    public nfloat Length()
    {
        nfloat length = 0;
        var previous = this[0];
        for (int i = 1; i <= 16; i++)
        {
            var t = (nfloat)i / 16;
            var current = this[t];
            length += Point.Distance(previous, current);
            previous = current;
        }
        return length;
    }

    /// <summary>
    /// Approximates the arc length of the curve using recursive adaptive subdivision.
    /// </summary>
    /// <param name="precision">
    /// The tolerance value used to determine when a segment is flat enough to stop subdividing.
    /// Smaller values result in more accurate results but require more computation.
    /// </param>
    public nfloat Length(nfloat precision) =>
        Length(P0, P1, P2, P3, precision);

    private static nfloat Length(Point p0, Point p1, Point p2, Point p3, nfloat tolerance)
    {
        var chord = Point.Distance(p0, p3);
        var control = Point.Distance(p0, p1) + Point.Distance(p1, p2) + Point.Distance(p2, p3);

        if (control - chord <= tolerance)
            return (chord + control) / 2;

        // Subdivide using De Casteljau
        var ab = Point.Lerp(p0, p1, 0.5f);
        var bc = Point.Lerp(p1, p2, 0.5f);
        var cd = Point.Lerp(p2, p3, 0.5f);

        var abbc = Point.Lerp(ab, bc, 0.5f);
        var bccd = Point.Lerp(bc, cd, 0.5f);
        var mid = Point.Lerp(abbc, bccd, 0.5f);

        return
            Length(p0, ab, abbc, mid, tolerance) +
            Length(mid, bccd, cd, p3, tolerance);
    }

    /// <summary>
    /// Gets the interpolated point on the curve at the specified parameter.
    /// </summary>
    /// <param name="t">A normalized parameter in the range [0, 1].</param>
    public Point this[nfloat t] => Lerp(t);

    /// <summary>
    /// Implicitly converts a tuple of four points into a <see cref="CubicBezier"/>.
    /// </summary>
    /// <param name="value">A tuple representing the start point, two control points, and end point.</param>
    public static implicit operator CubicBezier((Point p0, Point p1, Point p2, Point p3) value) =>
        new(value.p0, value.p1, value.p2, value.p3);

    /// <summary>
    /// Implicitly converts a <see cref="QuadraticBezier"/> to a <see cref="CubicBezier"/>
    /// using interpolation for smooth parameterization.
    /// </summary>
    /// <remarks>
    /// This conversion evaluates the quadratic Bézier at fixed steps (1⁄3 and 2⁄3) to generate
    /// the internal control points of the cubic, resulting in a visually smooth upgrade path.
    /// </remarks>
    public static implicit operator CubicBezier(QuadraticBezier quadratic) =>
        new CubicBezier(
            quadratic.P0,
            quadratic[1f / 3f],
            quadratic[2f / 3f],
            quadratic.P2
        );

    /// <summary>
    /// Implicitly converts this cubic Catmull–Rom spline to a <see cref="CubicBezier"/>.
    /// </summary>
    /// <remarks>
    /// Uses the canonical Catmull–Rom to Bézier mapping, interpolating between P1 and P2.
    /// </remarks>
    public static implicit operator CubicBezier(CubicSpline spline)
    {
        var c1 = spline.P1 + (spline.P2 - spline.P0) * (1f / 6f);
        var c2 = spline.P2 - (spline.P3 - spline.P1) * (1f / 6f);
        return new CubicBezier(spline.P1, c1, c2, spline.P2);
    }

    /// <summary>
    /// Returns a new <see cref="CubicBezier"/> curve where the control points <c>P1</c> and <c>P2</c>
    /// are adjusted based on a drag gesture at a given parametric position <paramref name="t"/>.
    /// </summary>
    /// <param name="t">
    /// A normalized value (between 0 and 1) representing the position on the curve where the drag occurs.
    /// </param>
    /// <param name="delta">
    /// The drag vector representing how far the user dragged the point at <paramref name="t"/>.
    /// </param>
    /// <returns>
    /// A new <see cref="CubicBezier"/> with <c>P0</c> and <c>P3</c> unchanged,
    /// and <c>P1</c> and <c>P2</c> adjusted proportionally to the drag direction and distance.
    /// </returns>
    /// <remarks>
    /// The influence of the drag on <c>P1</c> and <c>P2</c> is weighted quadratically based on their
    /// distance from <paramref name="t"/> along the curve:
    /// <c>P1</c> is influenced by <c>(1 - t)²</c> and <c>P2</c> by <c>t²</c>.
    /// This preserves the endpoints while allowing intuitive manipulation of the curve shape.
    /// </remarks>
    public CubicBezier Drag(nfloat t, Vector delta)
    {
        // Lock P0 and P3. Adjust P1 and P2 proportionally.
        var w1 = (1 - t);
        var w2 = t;

        return new CubicBezier(
            P0,
            P1 + delta * (w1 * w1), // influence is quadratic
            P2 + delta * (w2 * w2),
            P3
        );
    }

    /// <summary>
    /// Returns a new cubic Bézier curve with adjusted control points so the point on the curve
    /// nearest to <paramref name="origin"/> is moved by <paramref name="delta"/>.
    /// </summary>
    public CubicBezier DragAt(Point origin, Vector delta)
    {
        var t = ClosestT(origin);
        return Drag(t, delta);
    }

    /// <summary>
    /// Returns a new cubic Bézier curve with adjusted control points so the point on the curve
    /// nearest to <paramref name="origin"/> is moved by <paramref name="delta"/>.
    /// Uses recursive refinement based on the specified <paramref name="precision"/>.
    /// </summary>
    public CubicBezier DragAt(Point origin, Vector delta, nfloat precision)
    {
        var t = ClosestT(origin, precision);
        return Drag(t, delta);
    }

    /// <summary>
    /// Returns the parameter <c>t</c> ∈ [0, 1] at which the curve is closest to the specified <paramref name="target"/> point.
    /// This version uses uniform sampling with 16 segments and is fast enough for interactive use.
    /// </summary>
    /// <param name="target">The point to compare against the curve.</param>
    /// <returns>The approximate <c>t</c> value where the curve is closest to <paramref name="target"/>.</returns>
    public nfloat ClosestT(Point target)
    {
        nfloat bestT = 0;
        nfloat bestDist = nfloat.MaxValue;

        for (int i = 0; i <= 16; i++)
        {
            var t = (nfloat)i / 16;
            var pt = this[t];
            var dist = Point.SquaredDistance(pt, target);
            if (dist < bestDist)
            {
                bestDist = dist;
                bestT = t;
            }
        }

        return bestT;
    }

    /// <summary>
    /// Returns the parameter <c>t</c> ∈ [0, 1] at which the curve is closest to the specified <paramref name="target"/> point,
    /// using recursive ternary subdivision until the difference in <c>t</c> range is less than <paramref name="precision"/>.
    /// </summary>
    /// <param name="target">The point to compare against the curve.</param>
    /// <param name="precision">The minimum resolution for <c>t</c> convergence. Smaller values yield more accurate results.</param>
    /// <returns>The <c>t</c> value where the curve is closest to <paramref name="target"/> within the specified <paramref name="precision"/>.</returns>
    public nfloat ClosestT(Point target, nfloat precision)
    {
        return ClosestTRecursive(this, target, 0, 1, precision);
    }

    public MonotonicCubicBezier SplitIntoYMonotonicCurves()
    {
        // Compute the derivative coefficients (for Y)
        var a = -P0.Y + 3 * P1.Y - 3 * P2.Y + P3.Y;
        var b = 2 * (P0.Y - 2 * P1.Y + P2.Y);
        var c = -P0.Y + P1.Y;

        var discriminant = b * b - 4 * a * c;

        Span<nfloat> tValues = stackalloc nfloat[2];
        int count = 0;

        if (Math.Abs(a) < nfloat.Epsilon) // Degenerate (quadratic)
        {
            if (Math.Abs(b) > nfloat.Epsilon)
            {
                var t = -c / b;
                if (t > 0 && t < 1)
                    tValues[count++] = t;
            }
        }
        else if (discriminant >= 0)
        {
            var sqrtDisc = (nfloat)Math.Sqrt(discriminant);
            var t1 = (-b - sqrtDisc) / (2 * a);
            var t2 = (-b + sqrtDisc) / (2 * a);

            if (t1 > 0 && t1 < 1)
                tValues[count++] = t1;
            if (t2 > 0 && t2 < 1)
                tValues[count++] = t2;

            if (count == 2 && tValues[0] > tValues[1])
                (tValues[0], tValues[1]) = (tValues[1], tValues[0]);
        }

        // No splits required
        if (count == 0)
            return new MonotonicCubicBezier(this);

        // One split required
        if (count == 1)
        {
            var split = Subdivide(tValues[0]);
            return new MonotonicCubicBezier(split.Item1, split.Item2);
        }

        // Two splits required
        var splitFirst = Subdivide(tValues[0]);
        // Adjust t-value for second split to the second segment’s parameterization
        var adjustedT = (tValues[1] - tValues[0]) / (1 - tValues[0]);
        var splitSecond = splitFirst.Item2.Subdivide(adjustedT);

        return new MonotonicCubicBezier(splitFirst.Item1, splitSecond.Item1, splitSecond.Item2);
    }

    /// <summary>
    /// Subdivides this cubic Bézier curve at parameter t, returning two curves.
    /// </summary>
    public (CubicBezier, CubicBezier) Subdivide(nfloat t)
    {
        var p01 = Point.Lerp(P0, P1, t);
        var p12 = Point.Lerp(P1, P2, t);
        var p23 = Point.Lerp(P2, P3, t);

        var p012 = Point.Lerp(p01, p12, t);
        var p123 = Point.Lerp(p12, p23, t);

        var p0123 = Point.Lerp(p012, p123, t);

        return (
            new CubicBezier(P0, p01, p012, p0123),
            new CubicBezier(p0123, p123, p23, P3)
        );
    }

    private static nfloat ClosestTRecursive(CubicBezier curve, Point target, nfloat t0, nfloat t1, nfloat precision)
    {
        var m0 = t0 + (t1 - t0) / 3;
        var m1 = t1 - (t1 - t0) / 3;

        var p0 = curve[m0];
        var p1 = curve[m1];

        var d0 = Point.SquaredDistance(p0, target);
        var d1 = Point.SquaredDistance(p1, target);

        if ((t1 - t0) < precision)
            return (d0 < d1) ? m0 : m1;

        return (d0 < d1)
            ? ClosestTRecursive(curve, target, t0, m1, precision)
            : ClosestTRecursive(curve, target, m0, t1, precision);
    }

    /// <summary>
    /// Converts this cubic Bézier curve into a sequence of Y-monotonic quadratic Bézier segments,
    /// each approximating a portion of the original curve within the specified flatness precision.
    /// </summary>
    /// <param name="buffer">
    /// A span to receive the quadratic segments. Must be large enough to hold the result.
    /// A typical size is between 4 and 32. Must be at least 3 to accommodate the initial monotonic split.
    /// </param>
    /// <param name="precision">
    /// The maximum allowed distance between the original cubic curve and its quadratic approximation.
    /// Lower values yield more accurate approximations but may require more output segments.
    /// </param>
    /// <param name="count">
    /// The number of segments written to <paramref name="buffer"/>.
    /// </param>
    /// <remarks>
    /// This method uses an adaptive, flatness-aware subdivision strategy. It begins by splitting the
    /// cubic curve into up to 3 Y-monotonic segments. Each is then approximated with a quadratic Bézier,
    /// and the segments with the highest deviation are recursively subdivided until the total number of
    /// segments fits within <paramref name="buffer"/> and all segments meet the precision requirement.
    /// 
    /// The output segments are returned in order, forming a continuous piecewise-curve that follows the
    /// original cubic. Suitable for SDF tessellation or scanline-based rasterization.
    /// </remarks>
    /// <exception cref="ArgumentException">
    /// Thrown if <paramref name="buffer"/> has fewer than 3 elements.
    /// </exception>
    public void ToQuadratics(Span<QuadraticBezier> buffer, nfloat precision, out int count)
    {
        if (buffer.Length < 3)
            throw new ArgumentException("Requires buffer length ≥ 3", nameof(buffer));

        Span<SubcurveNode> nodes = stackalloc SubcurveNode[buffer.Length];
        int nodeCount = 0;
        ushort first = 0;

        // Seed monotonic segments
        var mono = SplitIntoYMonotonicCurves();
        nodes[0] = new SubcurveNode(mono.First);
        nodeCount = 1;

        if (mono.Second.HasValue)
        {
            nodes[1] = new SubcurveNode(mono.Second.Value);
            nodes[0].NextIndex = 1;
            nodeCount = 2;
        }

        if (mono.Third.HasValue)
        {
            nodes[2] = new SubcurveNode(mono.Third.Value);
            nodes[1].NextIndex = 2;
            nodeCount = 3;
        }

        // Adaptive subdivision loop
        while (nodeCount < buffer.Length)
        {
            // Find worst-fitting node (max error > precision)
            int worst = 0;
            nfloat worstError = nodes[0].Precision;

            for (int i = 1; i < nodeCount; i++)
            {
                if (nodes[i].Precision > worstError)
                {
                    worst = i;
                    worstError = nodes[i].Precision;
                }
            }

            if (worstError <= precision)
                break; // All are good enough

            // Subdivide the worst-fitting cubic segment
            var (left, right) = nodes[worst].Segment.Subdivide(0.5f);

            // Allocate the right node at the end of the buffer
            ushort rightIndex = (ushort)nodeCount++;
            nodes[rightIndex] = new SubcurveNode(right, nodes[worst].NextIndex);

            // Replace the left node in-place and point it to the right
            nodes[worst] = new SubcurveNode(left, rightIndex);
        }

        // Emit chain
        count = 0;
        ushort index = first;
        while (true)
        {
            ref var node = ref nodes[index];
            buffer[count++] = node.QuadraticBezierApproximation;

            if (node.NextIndex == 0)
                break;
            index = node.NextIndex;
        }
    }

    public struct SubcurveNode
    {
        public CubicBezier Segment;
        public QuadraticBezier QuadraticBezierApproximation;
        public nfloat Precision;
        public ushort NextIndex;

        /// <summary>
        /// Initializes a new node representing a quadratic approximation of a cubic Bézier segment in a linked list.
        /// </summary>
        /// <param name="segment">The original cubic Bézier segment.</param>
        /// <param name="nextIndex">Index of the next node in the chain. Use 0 if this is the last node.</param>
        public SubcurveNode(CubicBezier segment, ushort nextIndex = 0)
        {
            Segment = segment;
            NextIndex = nextIndex;

            QuadraticBezierApproximation = segment.QuadraticApproximation;

            // Compute precision using deviation from P0–P3 baseline
            var line = Segment.P3 - Segment.P0;
            var length = line.Magnitude;

            if (length == 0)
            {
                // Degenerate curve: use max distance from control points to P0
                Precision = nfloat.Max(
                    Point.Distance(Segment.P0, Segment.P1),
                    Point.Distance(Segment.P0, Segment.P2)
                );
            }
            else
            {
                // Signed distance from control points to baseline
                var d1 = Vector.Cross(line, Segment.P1 - Segment.P0) / length;
                var d2 = Vector.Cross(line, Segment.P2 - Segment.P0) / length;
                Precision = nfloat.Max(nfloat.Abs(d1), nfloat.Abs(d2));
            }
        }
    }
}
