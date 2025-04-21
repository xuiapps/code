using Xui.Core.Math2D;

namespace Xui.Core.Canvas;

/// <summary>
/// Provides methods for manipulating the current transformation matrix of the drawing context.
/// This includes translation, scaling, rotation, and applying affine transformations.
/// Mirrors the transformation API of the HTML5 Canvas 2D context.
/// </summary>
public interface ITransformContext
{
    /// <summary>
    /// Applies a translation by the specified vector.
    /// This offsets all subsequent drawing operations by the given amount.
    /// </summary>
    /// <param name="vector">The translation vector (dx, dy).</param>
    void Translate(Vector vector);

    /// <summary>
    /// Applies a clockwise rotation to the current transform.
    /// </summary>
    /// <param name="angle">The rotation angle in radians.</param>
    void Rotate(nfloat angle);

    /// <summary>
    /// Applies a scaling transformation using the specified scaling factors.
    /// </summary>
    /// <param name="vector">The scaling vector (sx, sy).</param>
    void Scale(Vector vector);

    /// <summary>
    /// Resets the current transformation matrix to the identity matrix,
    /// then replaces it with the specified transform.
    /// Equivalent to <c>ctx.setTransform(...)</c> in HTML5 Canvas.
    /// </summary>
    /// <param name="transform">The new transformation matrix to apply.</param>
    void SetTransform(AffineTransform transform);

    /// <summary>
    /// Multiplies the current transformation matrix by the specified matrix.
    /// This is equivalent to applying an additional transformation on top of the existing one.
    /// </summary>
    /// <param name="matrix">The matrix to multiply with the current transform.</param>
    void Transform(AffineTransform matrix);
}
