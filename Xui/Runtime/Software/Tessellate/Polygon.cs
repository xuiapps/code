using System.Collections;
using System.Collections.Generic;
using Xui.Core.Math2D;

namespace Xui.Runtime.Software.Tessellate;

/// <summary>
/// Represents a triangle polygon used during path tessellation.
/// </summary>
public readonly struct Polygon : IEnumerable<Point>
{
    public readonly Point A;
    public readonly Point B;
    public readonly Point C;

    public Polygon(Point a, Point b, Point c)
    {
        A = a;
        B = b;
        C = c;
    }

    public IEnumerator<Point> GetEnumerator()
    {
        yield return A;
        yield return B;
        yield return C;
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        yield return A;
        yield return B;
        yield return C;
    }
}
