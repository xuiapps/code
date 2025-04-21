namespace Xui.Core.Canvas;

/// <summary>
/// Defines methods for drawing image-like content onto the canvas,
/// such as bitmaps, video frames, or decoded image buffers.
/// 
/// Mirrors the image drawing APIs from the HTML5 Canvas 2D context.
/// https://developer.mozilla.org/en-US/docs/Web/API/CanvasRenderingContext2D/drawImage
/// </summary>
public interface IImageDrawingContext
{
    // Future members may include:
    // void DrawImage(ImageSource image, nfloat dx, nfloat dy);
    // void DrawImage(ImageSource image, nfloat dx, nfloat dy, nfloat dWidth, nfloat dHeight);
    // void DrawImage(ImageSource image, nfloat sx, nfloat sy, nfloat sWidth, nfloat sHeight,
    //                nfloat dx, nfloat dy, nfloat dWidth, nfloat dHeight);
}