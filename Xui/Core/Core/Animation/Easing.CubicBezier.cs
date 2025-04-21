using Xui.Core.Math2D;

namespace Xui.Core.Animation;

public static partial class Easing
{
    /// <summary>
    /// Represents a CSS-style cubic Bézier easing curve constrained between (0,0) and (1,1).
    /// </summary>
    /// <remarks>
    /// These curves are used for timing animations and transitions.
    /// The curve always starts at (0,0) and ends at (1,1), and the control points <see cref="P1"/> and <see cref="P2"/>
    /// shape the acceleration and deceleration of the animation.
    /// </remarks>
    public readonly struct CubicBezier
    {
        /// <summary>The first control point (typically near the origin).</summary>
        public readonly Point P1;

        /// <summary>The second control point (typically near the destination).</summary>
        public readonly Point P2;

        /// <summary>
        /// Initializes a new CSS-style cubic Bézier easing curve with the given control points.
        /// Assumes endpoints at (0,0) and (1,1).
        /// </summary>
        public CubicBezier(Point p1, Point p2)
        {
            P1 = p1;
            P2 = p2;
        }

        /// <summary>
        /// Returns the interpolated point on the Bézier curve at a given <paramref name="t"/> ∈ [0, 1].
        /// </summary>
        public Point Lerp(nfloat t) =>
            Point.Lerp(
                Point.Lerp(
                    Point.Lerp(Point.Zero, P1, t),
                    Point.Lerp(P1, P2, t),
                    t),
                Point.Lerp(
                    Point.Lerp(P1, P2, t),
                    Point.Lerp(P2, Point.One, t),
                    t),
                t
            );

        /// <summary>
        /// Approximates the Y output value for a given input X ∈ [0,1] using a 16-step lookup table.
        /// </summary>
        public nfloat Evaluate(nfloat x) => EvaluateInternal(x, steps: 16);

        /// <summary>
        /// Approximates the Y output value for a given input X ∈ [0,1] using binary search to the given precision.
        /// </summary>
        public nfloat Evaluate(nfloat x, nfloat precision) => EvaluateInternal(x, precision: precision);

        /// <summary>
        /// Indexer alias for <see cref="Evaluate(nfloat)"/>.
        /// </summary>
        public nfloat this[nfloat x] => Evaluate(x);

        private nfloat EvaluateInternal(nfloat xTarget, int? steps = null, nfloat? precision = null)
        {
            if (steps.HasValue)
            {
                nfloat closestY = 0;
                nfloat closestDx = nfloat.MaxValue;

                for (int i = 0; i <= steps.Value; i++)
                {
                    var t = (nfloat)i / steps.Value;
                    var p = Lerp(t);
                    var dx = nfloat.Abs(p.X - xTarget);
                    if (dx < closestDx)
                    {
                        closestDx = dx;
                        closestY = p.Y;
                    }
                }

                return closestY;
            }
            else if (precision.HasValue)
            {
                nfloat low = 0;
                nfloat high = 1;
                while (high - low > precision.Value)
                {
                    var mid = (low + high) / 2f;
                    var p = Lerp(mid);
                    if (p.X < xTarget)
                        low = mid;
                    else
                        high = mid;
                }
                return Lerp((low + high) / 2f).Y;
            }

            return 0;
        }

        /// <summary>
        /// Finds the parameter <c>t</c> ∈ [0, 1] where the curve is closest to the given <paramref name="target"/> point.
        /// Uses 16-step approximation.
        /// </summary>
        public nfloat ClosestT(Point target)
        {
            nfloat closestT = 0;
            nfloat closestDistance = nfloat.MaxValue;

            for (int i = 0; i <= 16; i++)
            {
                var t = (nfloat)i / 16;
                var pt = Lerp(t);
                var d = Point.SquaredDistance(pt, target);
                if (d < closestDistance)
                {
                    closestDistance = d;
                    closestT = t;
                }
            }

            return closestT;
        }

        /// <summary>
        /// Returns a new cubic Bézier easing curve with a deformation applied near the closest point to <paramref name="origin"/>, 
        /// shifted by <paramref name="delta"/>.
        /// </summary>
        public CubicBezier Drag(Point origin, Vector delta)
        {
            var t = ClosestT(origin);
            return DragAt(t, delta);
        }

        /// <summary>
        /// Returns a new cubic Bézier easing curve with a deformation applied at the given parameter <paramref name="t"/> by <paramref name="delta"/>.
        /// </summary>
        public CubicBezier DragAt(nfloat t, Vector delta)
        {
            var a = Point.Zero;
            var b = P1;
            var c = P2;
            var d = Point.One;

            var ab = Point.Lerp(a, b, t);
            var bc = Point.Lerp(b, c, t);
            var cd = Point.Lerp(c, d, t);

            var abbc = Point.Lerp(ab, bc, t);
            var bccd = Point.Lerp(bc, cd, t);
            var mid = Point.Lerp(abbc, bccd, t);
            var target = mid + delta;

            var newB = b + delta * (1 - t);
            var newC = c + delta * t;

            return new CubicBezier(newB, newC);
        }

        /// <summary>
        /// Converts a full <see cref="Curves2D.CubicBezier"/> into a CSS-style easing curve.
        /// The curve must begin at (0,0) and end at (1,1).
        /// </summary>
        public static implicit operator CubicBezier(Curves2D.CubicBezier bezier)
        {
            if (bezier.P0 != Point.Zero || bezier.P3 != Point.One)
                throw new ArgumentException("Only cubic Bézier curves with P0=(0,0) and P3=(1,1) are valid for easing.");

            return new CubicBezier(bezier.P1, bezier.P2);
        }

        /// <summary>Equivalent to CSS `ease`: cubic-bezier(0.25, 0.1, 0.25, 1.0)</summary>
        public static CubicBezier Ease => new((0.25f, 0.1f), (0.25f, 1f));

        /// <summary>Equivalent to CSS `ease-in`: cubic-bezier(0.42, 0, 1.0, 1.0)</summary>
        public static CubicBezier EaseIn => new((0.42f, 0f), (1f, 1f));

        /// <summary>Equivalent to CSS `ease-out`: cubic-bezier(0, 0, 0.58, 1.0)</summary>
        public static CubicBezier EaseOut => new((0f, 0f), (0.58f, 1f));

        /// <summary>Equivalent to CSS `ease-in-out`: cubic-bezier(0.42, 0, 0.58, 1.0)</summary>
        public static CubicBezier EaseInOut => new((0.42f, 0f), (0.58f, 1f));
    }
}
