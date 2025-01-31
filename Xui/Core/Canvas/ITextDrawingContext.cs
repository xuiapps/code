using Xui.Core.Math2D;

namespace Xui.Core.Canvas;

public interface ITextDrawingContext
{
    void FillText(string text, Point pos);

    Vector MeasureText(string text);

    void SetFont(Font font);
}
