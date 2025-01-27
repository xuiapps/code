using Xui.Core.Math2D;

namespace Xui.Core.Canvas;

public interface ITextDrawingContext
{
    TextAlign TextAlign { set; }

    TextBaseline TextBaseline { set; }

    void FillText(string text, Point pos);

    Vector MeasureText(string text);

    void SetFont(Font font);
}
