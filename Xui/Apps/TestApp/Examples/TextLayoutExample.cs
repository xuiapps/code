using Xui.Core.UI;
using static Xui.Core.Canvas.Colors;
using static Xui.Core.Canvas.FontWeight;
using static Xui.Core.Canvas.FontStyle;
using static Xui.Core.UI.HorizontalAlignment;
using static Xui.Core.UI.VerticalAlignment;

namespace Xui.Apps.TestApp.Examples;

public class TextLayoutExample : Example
{
    public TextLayoutExample()
    {
        this.Title = "Text Layout";

        this.Content = new VerticalStack
        {
            Content =
            [
                new Border {
                    Margin = 6,
                    Padding = 6,
                    BorderColor = Gray,
                    BorderThickness = 1,
                    BackgroundColor = LightGray,
                    HorizontalAlignment = Left,
                    Content = new Label {
                        FontFamily = ["Inter"],
                        Text = "Normal text, 14pt",
                        FontSize = 14
                    }
                },
                new Border {
                    Margin = 6,
                    Padding = 6,
                    BorderColor = Gray,
                    BorderThickness = 1,
                    BackgroundColor = LightGray,
                    HorizontalAlignment = Left,
                    Content = new Label {
                        FontFamily = ["Inter"],
                        Text = "Bold text, 16pt",
                        FontSize = 16,
                        FontWeight = Bold
                    }
                },
                new Border {
                    Margin = 6,
                    Padding = 6,
                    BorderColor = Gray,
                    BorderThickness = 1,
                    BackgroundColor = LightGray,
                    HorizontalAlignment = Left,
                    Content = new Label {
                        FontFamily = ["Inter"],
                        Text = "Italic text, 18pt",
                        FontSize = 18,
                        FontStyle = Italic
                    }
                },
                new Border {
                    Margin = 6,
                    Padding = 6,
                    BorderColor = Gray,
                    BorderThickness = 1,
                    BackgroundColor = LightGray,
                    HorizontalAlignment = Left,
                    Content = new Label {
                        FontFamily = ["Inter"],
                        Text = "Bold + Italic, 20pt",
                        FontSize = 20,
                        FontWeight = Bold,
                        FontStyle = Italic
                    }
                },
                new Border {
                    Margin = 6,
                    Padding = 6,
                    BorderColor = Gray,
                    BorderThickness = 1,
                    BackgroundColor = LightGray,
                    HorizontalAlignment = Left,
                    Content = new Label {
                        FontFamily = ["Inter"],
                        Text = "Larger text, 24pt",
                        FontSize = 24,
                        // TODO: The font name of Inter's SemiBold is "Inter SemiBold" and not just "Inter",
                        // so the default FontFaceComparer needs to strip font trait suffixes for this to work.
                        // FontWeight = SemiBold,
                        // FontStyle = Italic
                    }
                }
            ]
        };
    }
}
