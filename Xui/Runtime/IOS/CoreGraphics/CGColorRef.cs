using System.Runtime.InteropServices;

namespace Xui.Runtime.IOS;

public static partial class CoreGraphics
{
    public ref partial struct CGColorRef
    {
        [LibraryImport(CoreGraphicsLib)]
        private static partial nint CGColorCreateGenericRGB(NFloat red, NFloat green, NFloat blue, NFloat alpha);

        [LibraryImport(CoreGraphicsLib)]
        private static partial void CGColorRelease(nint self);

        public readonly nint Self;

        public CGColorRef(nint self)
        {
            if (self == 0)
            {
                throw new ObjCException($"{nameof(CGColorRef)} instantiated with nil self.");
            }

            this.Self = self;
        }

        public static CGColorRef RGBA(NFloat red, NFloat green, NFloat blue, NFloat alpha) => new CGColorRef(CGColorCreateGenericRGB(red, green, blue, alpha));

        public void Dispose()
        {
            if (this.Self != 0)
            {
                CGColorRelease(this.Self);
            }
        }

        public static implicit operator nint(CGColorRef cfColorRef) => cfColorRef.Self;
    }
}