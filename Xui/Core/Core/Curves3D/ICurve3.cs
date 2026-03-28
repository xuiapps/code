using Xui.Core.Math3D;

namespace Xui.Core.Curves3D;

/// <summary>
/// Defines a parametric curve in 3D space.
/// </summary>
/// <remarks>
/// The parameter <c>t</c> ranges from 0 to 1, where 0 is the start and 1 is the end of the curve.
/// </remarks>
public interface ICurve3
{
    /// <summary>
    /// Evaluates the curve at the given parameter <paramref name="t"/> and returns the
    /// corresponding point in 3D space.
    /// </summary>
    /// <param name="t">A normalized parameter in the range [0, 1].</param>
    Point3 Lerp(float t);

    /// <summary>
    /// Returns the tangent (first derivative) vector of the curve at parameter <paramref name="t"/>.
    /// </summary>
    /// <param name="t">A normalized parameter in the range [0, 1].</param>
    Vector3 Tangent(float t);

    /// <summary>
    /// Evaluates the curve at the given parameter <paramref name="t"/>.
    /// Alias for <see cref="Lerp(float)"/>.
    /// </summary>
    Point3 this[float t] { get; }

    /// <summary>
    /// Approximates the arc length of the curve using 16 uniform samples.
    /// </summary>
    float Length();

    /// <summary>
    /// Approximates the arc length of the curve using adaptive subdivision
    /// until the error is within <paramref name="precision"/>.
    /// </summary>
    /// <param name="precision">The maximum allowed error per segment.</param>
    float Length(float precision);
}
