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
                new Label { Text = "Name:", Margin = (4, 2) },
                new TextBox { Id = "NameBox", Text = "", Margin = (4, 2) },

                new Label { Text = "Password:", Margin = (4, 2) },
                new TextBox { Id = "PasswordBox", Text = "", IsPassword = true, Margin = (4, 2) },

                new Label { Text = "Number:", Margin = (4, 2) },
                new TextBox { Id = "NumberBox", Text = "0", Margin = (4, 2) },

                new Label { Text = "Color (hex):", Margin = (4, 2) },
                new TextBox { Id = "ColorBox", Text = "#FF0000", Margin = (4, 2) },
            ]
        };
    }
}
