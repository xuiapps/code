using System;

namespace Xui.Runtime.MacOS.Actual;

/// <summary>
/// Holds the decoded, platform-resident image data for a single URI.
/// Stores a CGImage for CoreGraphics drawing today and is structured to
/// accommodate an MTLTexture handle for Metal rendering in the future.
/// </summary>
internal sealed class MacOSImageResource : IDisposable
{
    /// <summary>CGImage — used by CoreGraphics drawing today.</summary>
    internal CoreGraphics.CGImage? CGImage;

    // internal nint MTLTexture; // MTLTextureRef — Metal future

    internal uint Width => CGImage?.Width ?? 0;
    internal uint Height => CGImage?.Height ?? 0;

    internal MacOSImageResource(CoreGraphics.CGImage cgImage)
    {
        CGImage = cgImage;
    }

    public void Dispose()
    {
        CGImage?.Dispose();
        CGImage = null;
    }
}
