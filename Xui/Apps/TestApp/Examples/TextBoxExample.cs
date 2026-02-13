using Xui.Core.UI;

namespace Xui.Apps.TestApp.Examples;

public class TextBoxExample : Example
{
    public TextBoxExample()
    {
        Title = "TextBox MVP";
        Content = new VerticalStack
        {
            Content =
            [
                new Label { Text = "Name:", FontFamily = ["Inter"], Margin = (4, 2) },
                new TextBox { Id = "NameBox", Text = "", FontFamily = ["Inter"], Margin = (4, 2) },

                new Label { Text = "Password:", FontFamily = ["Inter"], Margin = (4, 2) },
                new TextBox { Id = "PasswordBox", Text = "", IsPassword = true, FontFamily = ["Inter"], Margin = (4, 2) },

                new Label { Text = "Number:", FontFamily = ["Inter"], Margin = (4, 2) },
                new TextBox { Id = "NumberBox", Text = "0", FontFamily = ["Inter"], Margin = (4, 2), InputFilter = char.IsAsciiDigit },

                new Label { Text = "Color (hex):", FontFamily = ["Inter"], Margin = (4, 2) },
                new TextBox { Id = "ColorBox", Text = "#FF0000", FontFamily = ["Inter"], Margin = (4, 2), InputFilter = c => char.IsAsciiHexDigit(c) || c == '#' },
            ]
        };
    }
}
