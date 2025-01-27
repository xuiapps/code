using System.Runtime.InteropServices;
using Xui.Core.Math2D;

namespace Xui.Runtime.IOS;

public static partial class CoreGraphics
{
    /// <summary>
    /// Affine transformation matrix in the form:
    /// <code>
    /// | A  B  0 |
    /// | C  D  0 |
    /// | Tx Ty 1 |
    /// </code>
    /// Transforming a vector:
    /// <code>
    ///                       | A  B  0 |
    /// [x' y' 1] = [x y 1] * | C  D  0 |
    ///                       | Tx Ty 1 |
    /// </code>
    /// </summary>
    public partial struct CGAffineTransform
    {
        [LibraryImport(CoreGraphicsLib)]
        public static partial CGAffineTransform CGAffineTransformInvert(CGAffineTransform t);

        public static readonly CGAffineTransform Identity = new CGAffineTransform(1, 0, 0, 1, 0, 0);

        public NFloat A, B, C, D, Tx, Ty;

        public CGAffineTransform(NFloat a, NFloat b, NFloat c, NFloat d, NFloat tx, NFloat ty)
        {
            this.A = a;
            this.B = b;
            this.C = c;
            this.D = d;
            this.Tx = tx;
            this.Ty = ty;
        }

        public CGAffineTransform Invert => CGAffineTransformInvert(this);

        public static implicit operator CGAffineTransform(Xui.Core.Math2D.AffineTransform t) =>
            new CGAffineTransform(t.A, t.B, t.C, t.D, t.Tx, t.Ty);

        public static implicit operator Xui.Core.Math2D.AffineTransform(CGAffineTransform t) =>
            new Xui.Core.Math2D.AffineTransform(t.A, t.B, t.C, t.D, t.Tx, t.Ty);
    }
}
