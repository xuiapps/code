namespace Xui.Core.Canvas;

/// <summary>
/// Represents a decoded, GPU-resident image that can be drawn via
/// <see cref="IImageDrawingContext"/> or used as a fill pattern via
/// <see cref="IPenContext.SetFill(Bitmap)"/>.
/// Acquire instances through <see cref="IBitmapContext.LoadBitmap"/>.
/// </summary>
public abstract class Bitmap
{
    /// <summary>Width of the bitmap in pixels.</summary>
    public abstract uint Width { get; }

    /// <summary>Height of the bitmap in pixels.</summary>
    public abstract uint Height { get; }
}
