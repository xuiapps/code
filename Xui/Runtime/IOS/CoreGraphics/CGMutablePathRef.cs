using System.Runtime.InteropServices;

namespace Xui.Runtime.IOS;

public static partial class CoreGraphics
{
    public ref partial struct CGMutablePathRef
    {
        [LibraryImport(CoreGraphicsLib)]
        private static partial nint CGPathCreateMutable();

        /// <summary>
        /// void CGPathMoveToPoint(CGMutablePathRef path, const CGAffineTransform *m, CGFloat x, CGFloat y);
        /// </summary>
        [LibraryImport(CoreGraphicsLib)]
        private static partial void CGPathMoveToPoint(nint path, nint cgAffineTransform, NFloat x, NFloat y);

        /// <summary>
        /// void CGPathAddLineToPoint(CGMutablePathRef path, const CGAffineTransform *m, CGFloat x, CGFloat y);
        /// </summary>
        [LibraryImport(CoreGraphicsLib)]
        private static partial void CGPathAddLineToPoint(nint path, nint cgAffineTransform, NFloat x, NFloat y);

        [LibraryImport(CoreGraphicsLib)]
        private static partial void CGPathCloseSubpath(nint path);

        public readonly nint Self;

        public CGMutablePathRef(nint self)
        {
            if (self == 0)
            {
                throw new ObjCException($"{nameof(CGMutablePathRef)} instantiated with nil self.");
            }

            this.Self = self;
        }

        public CGMutablePathRef() : this(CGPathCreateMutable())
        {
        }

        public void MoveTo(NFloat x, NFloat y)
        {
            CGPathMoveToPoint(this, 0, x, y);
        }

        public void LineTo(NFloat x, NFloat y)
        {
            CGPathAddLineToPoint(this, 0, x, y);
        }

        public void Close()
        {
            CGPathCloseSubpath(this);
        }

        public void Dispose()
        {
            if (this.Self != 0)
            {
                CGPathRelease(this.Self);
            }
        }

        public static implicit operator nint(CGMutablePathRef cfColorRef) => cfColorRef.Self;
    }
}