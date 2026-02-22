namespace Xui.Core.Canvas;

/// <summary>
/// Provides access to platform bitmap resources — image loading and GPU upload.
/// Obtain an instance from the platform window at view attach time.
/// </summary>
public interface IBitmapContext
{
    /// <summary>
    /// Decodes the image at <paramref name="uri"/>, uploads it to the GPU once,
    /// and returns a <see cref="Bitmap"/> handle that can be used with
    /// <see cref="IImageDrawingContext"/> or <see cref="IPenContext.SetFill(Bitmap)"/>.
    /// Results are cached by URI — repeated calls with the same path return the same object.
    /// </summary>
    Bitmap LoadBitmap(string uri);
}
