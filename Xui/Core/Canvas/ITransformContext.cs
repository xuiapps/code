using Xui.Core.Math2D;

namespace Xui.Core.Canvas;

public interface ITransformContext
{
    public void Translate(Vector vector);

    public void Rotate(nfloat angle);

    public void Scale(Vector vector);

    /// <summary>
    /// Resets the current context transformation to the identity matrix, and then invokes <see cref="Transform"/>.
    /// </summary>
    /// <param name="transform"></param>
    public void SetTransform(AffineTransform transform);

    /// <summary>
    /// Multiplies the current context transformation with the <paramref name="matrix"/>.
    /// </summary>
    /// <param name="matrix"></param>
    public void Transform(AffineTransform matrix);
}