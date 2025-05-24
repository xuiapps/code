using Xui.Apps.TestApp.Examples;
using Xui.Core.Canvas;
using Xui.Core.Math2D;
using Xui.Core.UI;
using static Xui.Core.Canvas.Colors;

namespace Xui.Apps.TestApp.Pages;

public class SdkExamplePage<T> : ViewCollection
    where T : Example, new()
{
    private T content;
    private View header;

    public SdkExamplePage()
    {
        this.content = new T();

        this.header = new Border()
        {
            BorderThickness = (0, 0, 0, 0),
            BorderColor = Red,
            BackgroundColor = White,
            Content = new HorizontalStack()
            {
                Content =
                [
                    new SdkExampleBackButton(),
                    new Label()
                    {
                        Margin = (0, 0, 0, 24),
                        Text = this.content.Title
                    }
                ]
            }
        };

        this.Add(this.content);
        this.Add(this.header);
    }

    protected override Size MeasureCore(Size availableBorderEdgeSize, IMeasureContext context)
    {
        this.header.Measure((availableBorderEdgeSize.Width, 24), context);
        return this.content.Measure(availableBorderEdgeSize - (0, 24), context) + new Size(0, 24);
    }

    protected override void ArrangeCore(Rect rect, IMeasureContext context)
    {
        this.header.Arrange((0, 0, rect.Width, 24), context);
        this.content.Arrange((0, 24, rect.Width, rect.Height - 24), context);
    }
}