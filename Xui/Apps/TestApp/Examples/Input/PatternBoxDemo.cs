using Xui.Core.Canvas;
using Xui.Core.UI;
using Xui.SDK.UI;

namespace Xui.Apps.TestApp.Examples.Input;

/// <summary>
/// Demo page for <see cref="PatternBox"/> — a pattern-masked slot input.
/// Shows boxes for common input patterns.
/// </summary>
public class PatternBoxDemo : Example
{
    public PatternBoxDemo()
    {
        Title = "Pattern Input";
        Content = new VerticalStack
        {
            Content =
            [
                new Label { Text = "6-digit PIN:", FontFamily = ["Inter"], Margin = (4, 4) },
                new PatternBox("######") { Id = "PinBox", Margin = (4, 4) },

                new Label { Text = "Credit card (####-####-####-####):", FontFamily = ["Inter"], Margin = (4, 4) },
                new PatternBox("####-####-####-####") { Id = "CardBox", Margin = (4, 4) },

                new Label { Text = "CCV (###):", FontFamily = ["Inter"], Margin = (4, 4) },
                new PatternBox("###") { Id = "CcvBox", Margin = (4, 4) },
            ]
        };
    }
}
