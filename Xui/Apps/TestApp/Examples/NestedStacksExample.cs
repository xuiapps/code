using Xui.Core.UI;
using static Xui.Core.Canvas.Colors;

namespace Xui.Apps.TestApp.Examples;

public class NestedStacksExample : Example
{
    public NestedStacksExample()
    {
        this.Title = "Nested Stacks";

        this.Content = new VerticalStack
        {
            Content =
            [
                CreateRow("Row 1:", ["A", "B", "C"]),
                CreateRow("Row 2:", ["One", "Two"]),
                CreateRow("Row 3:", ["Left", "Center", "Right", "Stretch"])
            ]
        };
    }

    private static View CreateRow(string label, string[] items)
    {
        return new Border {
            Margin = 8,
            Padding = 8,
            CornerRadius = 8,
            BorderThickness = 1,
            BorderColor = Gray,
            BackgroundColor = White,
            Content = new HorizontalStack
            {
                Content =
                    new List<View>() {
                        new Label {
                            Text = label,
                            Margin = (11, 4, 8, 8),
                            FontWeight = Xui.Core.Canvas.FontWeight.Bold
                        }
                    }.Concat(items.Select(text =>
                        new Border {
                            Margin = 4,
                            Padding = 6,
                            BorderThickness = 1,
                            CornerRadius = 4,
                            BorderColor = Black,
                            BackgroundColor = LightGray,
                            Content = new Label {
                                Text = text
                            }
                        }
                    )).ToArray()
            }
        };
    }
}
