using Xui.Core.Math2D;

namespace Xui.Core.Canvas;

public interface IRectDrawingContext
{
    public void StrokeRect(Rect rect);
    public void FillRect(Rect rect);
}