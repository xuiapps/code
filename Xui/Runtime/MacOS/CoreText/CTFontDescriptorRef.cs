using System;
using System.Runtime.InteropServices;
using static Xui.Runtime.MacOS.CoreFoundation;
using static Xui.Runtime.MacOS.CoreGraphics;

namespace Xui.Runtime.MacOS;

public static partial class CoreText
{
    public ref partial struct CTFontDescriptorRef : IDisposable
    {
        public readonly nint Self;

        [LibraryImport(CoreTextLib)]
        public static partial nint CTFontDescriptorCreateWithAttributes(nint cfDictionaryRefAttributes);

        [LibraryImport(CoreTextLib)]
        public static partial nint CTFontDescriptorCreateWithNameAndSize(nint cfStringRefName, NFloat size);

        public CTFontDescriptorRef(nint self)
        {
            if (self == 0)
            {
                throw new ObjCException($"{nameof(CTFontDescriptorRef)} instantiated with nil self.");
            }

            this.Self = self;
        }

        public CTFontDescriptorRef(string name) : this(WithName(name))
        {
        }

        public static CTFontDescriptorRef WithAttributes(nint attributes) =>
            new CTFontDescriptorRef(CTFontDescriptorCreateWithAttributes(attributes));
        
        private static nint WithName(string name)
        {
            using var cfName = new CFStringRef(name);
            return CTFontDescriptorCreateWithNameAndSize(cfName, 0);
        }

        public void Dispose()
        {
            if (this.Self != 0)
            {
                CFRelease(this.Self);
            }
        }

        public static implicit operator nint(CTFontDescriptorRef ctFontRef) => ctFontRef.Self;
    }
}