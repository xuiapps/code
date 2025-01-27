using System;
using System.Runtime.InteropServices;
using Xui.Core.Canvas;

namespace Xui.Runtime.MacOS;

public static partial class CoreGraphics
{
    public ref partial struct CGGradientRef
    {
        [LibraryImport(CoreGraphicsLib)]
        public static partial void CGGradientRelease(nint self);

        [LibraryImport(CoreGraphicsLib)]
        public static partial nint CGGradientCreateWithColorComponents(nint cgColorSpaceRef, ref NFloat componentsArrPtr, ref NFloat locationsArrPtr, nint count);

        public readonly nint Self;

        public CGGradientRef(nint self)
        {
            if (self == 0)
            {
                throw new ObjCException($"{nameof(CGColorRef)} instantiated with nil self.");
            }

            this.Self = self;
        }

        public CGGradientRef(ReadOnlySpan<GradientStop> gradient) : this(Map(gradient)) {}

        private static nint Map(ReadOnlySpan<GradientStop> gradient)
        {
            using var rgb = CGColorSpaceRef.CreateDeviceRGB();
            Span<NFloat> components = stackalloc NFloat[gradient.Length * 4];
            Span<NFloat> locations = stackalloc NFloat[gradient.Length];
            for(int i = 0; i < gradient.Length; i++)
            {
                var stop = gradient[i];
                var o = i * 4;
                components[o + 0] = stop.Color.Red;
                components[o + 1] = stop.Color.Green;
                components[o + 2] = stop.Color.Blue;
                components[o + 3] = stop.Color.Alpha;
                locations[i] = stop.Offset;
            }
            return CGGradientCreateWithColorComponents(
                rgb,
                ref MemoryMarshal.GetReference(components),
                ref MemoryMarshal.GetReference(locations),
                gradient.Length);
        }

        public void Dispose()
        {
            if (this.Self != 0)
            {
                CGGradientRelease(this.Self);
            }
        }

        public static implicit operator nint(CGGradientRef cfColorRef) => cfColorRef.Self;
    }
}