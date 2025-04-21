using Xui.Core.Math2D;

namespace Xui.Core.Curves2D;

/// <summary>
/// Represents a common interface for evaluable 2D curves.
/// </summary>
public interface ICurve
{
    /// <summary>
    /// Evaluates the curve at the given normalized parameter t in [0, 1].
    /// </summary>
    Point Lerp(nfloat t);

    /// <summary>
    /// Computes the tangent vector at the specified t.
    /// </summary>
    Vector Tangent(nfloat t);

    /// <summary>
    /// Provides indexer access as an alias for <see cref="Lerp"/>.
    /// </summary>
    Point this[nfloat t] { get; }

    /// <summary>
    /// Computes an approximate arc length using 16 steps.
    /// </summary>
    nfloat Length();

    /// <summary>
    /// Computes a refined approximation of arc length with a specified precision.
    /// </summary>
    nfloat Length(nfloat precision);
}
