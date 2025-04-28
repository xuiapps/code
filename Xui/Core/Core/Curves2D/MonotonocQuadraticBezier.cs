namespace Xui.Core.Curves2D;

public readonly struct MonotonicQuadraticBezier
{
    public readonly QuadraticBezier First;
    public readonly QuadraticBezier? Second;

    public MonotonicQuadraticBezier(QuadraticBezier first, QuadraticBezier? second = null)
    {
        First = first;
        Second = second;
    }
}
