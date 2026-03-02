using Xui.Core.Canvas;
using Xui.Core.UI;
using Xui.SDK.UI;

namespace Xui.Apps.TestApp.Examples.Input;

/// <summary>
/// Demo page for <see cref="CurrencyBox"/> — a currency-formatted input.
/// Shows USD, EUR, and an overflow scenario.
/// </summary>
public class CurrencyBoxDemo : Example
{
    public CurrencyBoxDemo()
    {
        Title = "Currency Input";
        Content = new VerticalStack
        {
            Content =
            [
                new Label { Text = "USD (default $):", FontFamily = ["Inter"], Margin = (4, 4) },
                new CurrencyBox
                {
                    Id = "UsdBox",
                    Margin = (4, 4),
                    Value = 1234.56m,
                    CurrencySymbol = "$",
                    SymbolPrefix = true,
                    Format = "N2",
                },

                new Label { Text = "EUR (suffix):", FontFamily = ["Inter"], Margin = (4, 4) },
                new CurrencyBox
                {
                    Id = "EurBox",
                    Margin = (4, 4),
                    Value = 9999.99m,
                    CurrencySymbol = " €",
                    SymbolPrefix = false,
                    Format = "N2",
                },

                new Label { Text = "Large value (overflow hint):", FontFamily = ["Inter"], Margin = (4, 4) },
                new CurrencyBox
                {
                    Id = "LargeBox",
                    Margin = (4, 4),
                    Value = 1_000_000_000.00m,
                    CurrencySymbol = "$",
                    SymbolPrefix = true,
                    Format = "N2",
                },

                new Label { Text = "Zero:", FontFamily = ["Inter"], Margin = (4, 4) },
                new CurrencyBox
                {
                    Id = "ZeroBox",
                    Margin = (4, 4),
                    Value = 0m,
                },
            ]
        };
    }
}
