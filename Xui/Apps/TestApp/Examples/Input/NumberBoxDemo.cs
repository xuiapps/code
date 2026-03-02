using Xui.Core.Canvas;
using Xui.Core.UI;
using Xui.SDK.UI;

namespace Xui.Apps.TestApp.Examples.Input;

/// <summary>
/// Demo page for <see cref="NumberBox"/> — a numeric stepper input.
/// Shows boxes with different Min/Max/Step/Format configurations.
/// </summary>
public class NumberBoxDemo : Example
{
    public NumberBoxDemo()
    {
        Title = "Number Input";
        Content = new VerticalStack
        {
            Content =
            [
                new Label { Text = "Default (step=1):", FontFamily = ["Inter"], Margin = (4, 4) },
                new NumberBox
                {
                    Id = "DefaultNumberBox",
                    Margin = (4, 4),
                    Value = 0, Min = -100, Max = 100, Step = 1,
                },

                new Label { Text = "Large step (step=10, 0..1000):", FontFamily = ["Inter"], Margin = (4, 4) },
                new NumberBox
                {
                    Id = "LargeStepBox",
                    Margin = (4, 4),
                    Value = 100, Min = 0, Max = 1000, Step = 10,
                },

                new Label { Text = "Decimal (step=0.5, format=F1):", FontFamily = ["Inter"], Margin = (4, 4) },
                new NumberBox
                {
                    Id = "DecimalBox",
                    Margin = (4, 4),
                    Value = 0.0, Min = -10.0, Max = 10.0, Step = 0.5, Format = "F1",
                },

                new Label { Text = "Bounded (−5..5, at min):", FontFamily = ["Inter"], Margin = (4, 4) },
                new NumberBox
                {
                    Id = "BoundedAtMinBox",
                    Margin = (4, 4),
                    Value = -5, Min = -5, Max = 5, Step = 1,
                },
            ]
        };
    }
}
