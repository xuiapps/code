using Xui.Core.Canvas;
using Xui.Core.Math2D;
using Xui.Core.Curves2D;
using System;
using System.Collections.Generic;

namespace Xui.Runtime.Software.Rasterization;

public sealed class Rasterizer : IPathBuilder
{
    private readonly EdgeList _edges = new();
    private Point _current;
    private bool _hasCurrent;

    public Rasterizer()
    {
    }

    public void BeginPath()
    {
        _edges.Clear();
        _hasCurrent = false;
    }

    public void MoveTo(Point to)
    {
        _current = to;
        _hasCurrent = true;
    }

    public void LineTo(Point to)
    {
        if (_hasCurrent)
        {
            _edges.Add(new Edge(_current, to));
            _current = to;
        }
    }

    public void ClosePath()
    {
        // TODO: If you want automatic closing later
    }

    public void CurveTo(Point cp1, Point to)
    {
        if (_hasCurrent)
        {
            var quad = new QuadraticBezier(_current, cp1, to);
            AddQuadratic(quad, (nfloat)0.25);
            _current = to;
        }
    }

    public void CurveTo(Point cp1, Point cp2, Point to)
    {
        if (_hasCurrent)
        {
            var cubic = new CubicBezier(_current, cp1, cp2, to);
            AddCubic(cubic, (nfloat)0.25);
            _current = to;
        }
    }

    public void Arc(Point center, nfloat radius, nfloat startAngle, nfloat endAngle, Winding winding = Winding.ClockWise)
    {
        // TODO: flatten arcs to lines
    }

    public void ArcTo(Point cp1, Point cp2, nfloat radius)
    {
        // TODO: flatten arcTo
    }

    public void Ellipse(Point center, nfloat radiusX, nfloat radiusY, nfloat rotation, nfloat startAngle, nfloat endAngle, Winding winding = Winding.ClockWise)
    {
        // TODO: flatten ellipse
    }

    public void Rect(Rect rect)
    {
        MoveTo(new Point(rect.X, rect.Y));
        LineTo(new Point(rect.X + rect.Width, rect.Y));
        LineTo(new Point(rect.X + rect.Width, rect.Y + rect.Height));
        LineTo(new Point(rect.X, rect.Y + rect.Height));
        LineTo(new Point(rect.X, rect.Y)); // Close
    }

    public void RoundRect(Rect rect, nfloat radius)
    {
        // TODO: flatten rounded corners
    }

    public void RoundRect(Rect rect, CornerRadius radius)
    {
        // TODO: flatten rounded corners
    }

    private void AddQuadratic(QuadraticBezier curve, nfloat tolerance)
    {
        FlattenQuadratic(curve, tolerance, _edges);
    }

    private static void FlattenQuadratic(QuadraticBezier curve, nfloat tolerance, EdgeList edges)
    {
        if (IsFlatEnough(curve, tolerance))
        {
            edges.Add(new Edge(curve.P0, curve.P2));
        }
        else
        {
            var (left, right) = curve.Subdivide(0.5f);
            FlattenQuadratic(left, tolerance, edges);
            FlattenQuadratic(right, tolerance, edges);
        }
    }

    private static bool IsFlatEnough(QuadraticBezier q, nfloat tolerance)
    {
        return DistancePointToLine(q.P1, q.P0, q.P2) <= tolerance;
    }

    private static nfloat DistancePointToLine(Point p, Point a, Point b)
    {
        var ab = b - a;
        var ap = p - a;
        var area = nfloat.Abs(ab.X * ap.Y - ab.Y * ap.X);
        var baseLength = Point.Distance(a, b);
        if (baseLength == 0) return Point.Distance(p, a);
        return area / baseLength;
    }

    private void AddCubic(CubicBezier curve, nfloat pixelSize)
    {
        FlattenCubic(curve, pixelSize / 4, _edges);
    }

    private static void FlattenCubic(CubicBezier curve, nfloat tolerance, EdgeList edges)
    {
        if (IsFlatEnough(curve, tolerance))
        {
            edges.Add(new Edge(curve.P0, curve.P3));
        }
        else
        {
            var (left, right) = curve.Subdivide(0.5f);
            FlattenCubic(left, tolerance, edges);
            FlattenCubic(right, tolerance, edges);
        }
    }

    private static bool IsFlatEnough(CubicBezier c, nfloat tolerance)
    {
        // Compute the maximum distance from the control points to the straight line P0-P3
        var d1 = DistancePointToLine(c.P1, c.P0, c.P3);
        var d2 = DistancePointToLine(c.P2, c.P0, c.P3);
        return Math.Max(d1, d2) <= tolerance;
    }

    // --- Main Fill ---
    public void Rasterize(G16Stencil stencil, FillRule rule)
    {
        _edges.SortByY();
        var edges = _edges.Edges;
        List<ActiveEdge> activeEdges = new();

        int edgeIndex = 0;
        int minY = 0;
        int maxY = (int)stencil.Height;

        if (edges.Count > 0)
        {
            minY = Math.Max(0, (int)Math.Floor(edges[0].Y0));
            maxY = Math.Min((int)stencil.Height, (int)Math.Ceiling(edges[^1].Y1));
        }

        for (int y = minY; y < maxY; y++)
        {
            nfloat yTop = y;
            nfloat yBottom = y + 1;

            // Add edges entering this scanline
            while (edgeIndex < edges.Count && edges[edgeIndex].Y0 <= yTop)
            {
                var e = edges[edgeIndex++];
                if (e.Y1 > yTop)
                    activeEdges.Add(new ActiveEdge(e));
            }

            // Remove edges that ended before this scanline
            activeEdges.RemoveAll(ae => ae.Y1 <= yTop);

            if (activeEdges.Count == 0)
                continue;

            activeEdges.Sort((a, b) => a.XAt(yTop).CompareTo(b.XAt(yTop)));

            // --- Real 4x4 Supersampling ---
            for (int subY = 0; subY < 4; subY++)
            {
                nfloat subPixelY = y + (subY * 0.25f + 0.125f);

                // Build crossings for this subpixel Y
                List<nfloat> crossings = new();
                foreach (var ae in activeEdges)
                {
                    if (subPixelY >= ae.Y0 && subPixelY < ae.Y1)
                    {
                        var x = ae.XAt(subPixelY);
                        crossings.Add(x);
                    }
                }

                crossings.Sort();

                for (int i = 0; i + 1 < crossings.Count; i += 2)
                {
                    var xStart = crossings[i];
                    var xEnd = crossings[i + 1];

                    if (xEnd > xStart)
                    {
                        int baseX = (int)Math.Floor(xStart);
                        int limitX = (int)Math.Floor(xEnd);

                        for (int x = baseX; x <= limitX; x++)
                        {
                            if (x >= 0 && x < (int)stencil.Width)
                            {
                                // Now supersample horizontally too!
                                for (int subX = 0; subX < 4; subX++)
                                {
                                    nfloat subPixelX = x + (subX * 0.25f + 0.125f);

                                    if (subPixelX >= xStart && subPixelX < xEnd)
                                    {
                                        // 4x4 - 16 subpixels, each contributes to 1/16 of the coverage
                                        stencil.Add((uint)x, (uint)y, 16);
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    }


    private struct ActiveEdge
    {
        public nfloat X0, Y0, X1, Y1, DxDy;

        public ActiveEdge(Edge edge)
        {
            X0 = edge.X0;
            Y0 = edge.Y0;
            X1 = edge.X1;
            Y1 = edge.Y1;
            DxDy = edge.DxDy;
        }

        public nfloat XAt(nfloat y)
        {
            return X0 + (y - Y0) * DxDy;
        }
    }
}
