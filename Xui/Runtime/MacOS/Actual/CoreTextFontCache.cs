using System;
using Xui.Core.Canvas;
using static Xui.Runtime.MacOS.CoreFoundation;

namespace Xui.Runtime.MacOS.Actual;

/// <summary>
/// Owns a bounded set of CoreText fonts resolved for one drawing context.
/// </summary>
/// <remarks>
/// Entries own their Core Foundation references. Handles returned by <see cref="TryGet"/>
/// are borrowed and remain valid until the next font switch on the owning context.
/// </remarks>
internal sealed class CoreTextFontCache : FontCache<nint>, IDisposable
{
    internal const int Capacity = 64;

    public CoreTextFontCache() : base(Capacity) { }

    /// <summary>Releases the owned Core Foundation font handle.</summary>
    protected override void OnEvicted(nint ctFont)
    {
        if (ctFont != 0)
            CFRelease(ctFont);
    }

    public void Dispose() => Clear();
}
