using System.Threading.Tasks;
using Xui.Core.Math2D;

namespace Xui.Core.Canvas;

/// <summary>
/// Represents a decoded, GPU-resident image. Acts as a self-loading resource handle —
/// analogous to <c>HTMLImageElement</c> in the browser.
/// <para>
/// Acquire instances via <c>GetService(typeof(IImage))</c> on any <see cref="Xui.Core.UI.View"/>.
/// Then set the source with <see cref="Load"/> (sync-if-cached) or <see cref="LoadAsync"/>
/// for first-time remote or large assets.
/// </para>
/// </summary>
public interface IImage
{
    /// <summary>
    /// Intrinsic size of the image in points. Returns <see cref="Size.Empty"/> until loaded.
    /// </summary>
    Size Size { get; }

    /// <summary>
    /// Loads the image from <paramref name="uri"/>.
    /// Returns immediately if the image is already in the platform catalog.
    /// For local packaged assets this may block on first call; prefer <see cref="LoadAsync"/>
    /// for anything that could be slow.
    /// </summary>
    void Load(string uri);

    /// <summary>
    /// Loads the image from <paramref name="uri"/> on a background thread.
    /// The returned task completes once the image is decoded and GPU-resident.
    /// Subsequent <see cref="Load"/> calls with the same URI return instantly from cache.
    /// </summary>
    Task LoadAsync(string uri);

    /// <summary>
    /// Uploads raw CPU pixel data into this image, replacing any previously loaded content.
    /// <paramref name="bgra32Data"/> must contain <c>width * height * 4</c> bytes in
    /// BGRA order (B at the lowest address), matching <c>DXGI_FORMAT_B8G8R8A8_UNORM</c>.
    /// Default implementation is a no-op on platforms that do not support pixel upload.
    /// </summary>
    void LoadPixels(int width, int height, ReadOnlySpan<byte> bgra32Data) { }
}
