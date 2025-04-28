namespace Xui.Core.Curves2D;

public readonly struct MonotonicCubicBezier
{
    public readonly CubicBezier First;
    public readonly CubicBezier? Second;
    public readonly CubicBezier? Third;

    public MonotonicCubicBezier(CubicBezier first, CubicBezier? second = null, CubicBezier? third = null)
    {
        First = first;
        Second = second;
        Third = third;
    }
}
