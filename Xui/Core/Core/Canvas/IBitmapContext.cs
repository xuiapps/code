namespace Xui.Core.Canvas;

/// <summary>
/// Provides access to platform bitmap resources — factories, image loaders, and brush creation.
/// This interface is a companion to <see cref="ITextMeasureContext"/> and will later be backed
/// by Direct2D image factories and brush caches.
/// For now it serves as a typed placeholder for the attach lifecycle.
/// </summary>
public interface IBitmapContext
{
    // Future members may include:
    // IBitmap LoadBitmap(string uri);
    // IBrush CreateSolidBrush(Color color);
}
