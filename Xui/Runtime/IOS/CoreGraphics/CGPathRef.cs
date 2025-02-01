using System.Runtime.InteropServices;

namespace Xui.Runtime.IOS;

public static partial class CoreGraphics
{
    public ref partial struct CGPathRef
    {
        public readonly nint Self;

        [LibraryImport(CoreGraphicsLib)]
        public static partial nint CGPathCreateWithRoundedRect(CGRect rect, NFloat cornerWidth, NFloat cornerHeight, nint cgAffineTransformPtr = 0);

        [LibraryImport(CoreGraphicsLib)]
        public static partial nint CGPathCreateWithRoundedRect(CGRect rect, NFloat cornerWidth, NFloat cornerHeight, ref CGAffineTransform transform);

        public static CGPathRef CreateWithRoundedRect(CGRect rect, NFloat radius) =>
            new CGPathRef(CGPathCreateWithRoundedRect(rect, radius, radius));

        public CGPathRef(nint self)
        {
            if (self == 0)
            {
                throw new ObjCException($"{nameof(CGPathRef)} instantiated with nil self.");
            }

            this.Self = self;
        }

        public void Dispose()
        {
            if (this.Self != 0)
            {
                CGPathRelease(this.Self);
            }
        }

        public static implicit operator nint(CGPathRef cfColorRef) => cfColorRef.Self;
    }
}