using System;
using static Xui.Runtime.MacOS.CoreGraphics;

namespace Xui.Runtime.MacOS.Actual;

/// <summary>
/// Holds the decoded, platform-resident image data for a single URI.
/// Stores a CGImageRef for CoreGraphics drawing today and is structured to
/// accommodate an MTLTexture handle for Metal rendering in the future.
/// </summary>
internal sealed class MacOSImageResource : IDisposable
{
    /// <summary>CGImageRef — used by CoreGraphics drawing today.</summary>
    internal nint CGImage;

    // internal nint MTLTexture; // MTLTextureRef — Metal future

    internal uint Width;
    internal uint Height;

    internal MacOSImageResource(nint cgImage, uint width, uint height)
    {
        CGImage = cgImage;
        Width   = width;
        Height  = height;
    }

    public void Dispose()
    {
        if (CGImage != 0)
        {
            CGImageRef.CGImageRelease(CGImage);
            CGImage = 0;
        }
    }
}
