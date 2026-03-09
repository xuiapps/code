using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Xui.Core.Actual;
using Xui.Core.Canvas;
using static Xui.Runtime.MacOS.CoreFoundation;
using static Xui.Runtime.MacOS.CoreGraphics;
using static Xui.Runtime.MacOS.ImageIO;

namespace Xui.Runtime.MacOS.Actual;

/// <summary>
/// Window-level image catalog: decodes via ImageIO, stores as CGImageRef, caches by URI.
/// Implements <see cref="IImagePipeline"/> so the service chain can vend <see cref="IImage"/>
/// handles to views without exposing platform types.
/// The CGImageRef is drawable by CoreGraphics today and the resource struct is designed
/// to accommodate an MTLTexture handle for Metal rendering in the future.
/// </summary>
internal sealed class MacOSImageFactory : IImagePipeline, IDisposable
{
    private readonly Dictionary<string, MacOSImageResource> cache = new();

    // ── IImagePipeline ───────────────────────────────────────────────────────

    public IImage CreateImage() => new MacOSImage(this);

    // ── Internal catalog ─────────────────────────────────────────────────────

    internal MacOSImageResource? GetOrLoad(string uri)
    {
        if (cache.TryGetValue(uri, out var cached))
            return cached;

        string resolved = Resolve(uri);
        try
        {
            var resource = Decode(resolved);
            cache[uri] = resource;
            return resource;
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"MacOSImageFactory: failed to load '{resolved}': {ex.Message}");
            return null;
        }
    }

    internal Task<MacOSImageResource?> GetOrLoadAsync(string uri) =>
        Task.Run(() => GetOrLoad(uri));

    // ── Internals ─────────────────────────────────────────────────────────────

    private static string Resolve(string uri) =>
        System.IO.Path.IsPathRooted(uri)
            ? uri
            : System.IO.Path.Combine(AppContext.BaseDirectory, uri);

    private static MacOSImageResource Decode(string resolvedPath)
    {
        using var pathString = new CFStringRef(resolvedPath);
        using var url = CFURLRef.CreateWithFileSystemPath(pathString);
        using var source = CGImageSourceRef.CreateWithURL(url);
        using var cgImage = source.CreateImageAtIndex(0);

        var width  = (uint)cgImage.Width;
        var height = (uint)cgImage.Height;

        // Transfer ownership of the CGImageRef out of the ref-struct without releasing.
        // MacOSImageResource takes ownership and will call CGImageRelease on Dispose.
        nint retained = CFRetain(cgImage);
        return new MacOSImageResource(retained, width, height);
    }

    public void Dispose()
    {
        foreach (var resource in cache.Values)
            resource.Dispose();
        cache.Clear();
    }
}
