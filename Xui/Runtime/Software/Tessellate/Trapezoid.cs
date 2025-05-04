using System.Collections;
using System.Collections.Generic;
using Xui.Core.Math2D;

namespace Xui.Runtime.Software.Tessellate;

public readonly struct Trapezoid : IEnumerable<Polygon>
{
    public readonly Point A; // top-left
    public readonly Point B; // top-right
    public readonly Point C; // bottom-right
    public readonly Point D; // bottom-left

    public Trapezoid(Point a, Point b, Point c, Point d)
    {
        A = a; B = b; C = c; D = d;
    }

    public Polygon Polygon1 => new Polygon(A, D, B); // Upper triangle
    public Polygon Polygon2 => new Polygon(B, D, C); // Lower triangle

    public IEnumerator<Polygon> GetEnumerator()
    {
        yield return Polygon1;
        yield return Polygon2;
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        yield return Polygon1;
        yield return Polygon2;
    }
}
