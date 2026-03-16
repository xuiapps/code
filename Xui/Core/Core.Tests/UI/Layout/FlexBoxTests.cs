using Xui.Core.UI;
using Xui.Core.UI.Layout;
using static Xui.Core.UI.Layout.FlexBox;

namespace Xui.Core.UI.Tests.Layout;

public class FlexBoxTests
{
    [Fact]
    public void FlexBox_Toolbar_RowWithGrowingSpacer()
    {
        var toolbar = new FlexBox
        {
            FlexDirection      = FlexBox.Direction.Row,
            FlexAlignItems     = FlexBox.AlignItems.Center,
            FlexJustifyContent = FlexBox.JustifyContent.FlexStart,
            ColumnGap = 8,
            Content =
            [
                new Label { Text = "File"   },
                new Label { Text = "Edit"   },
                new Label { Text = "View"   },
                new Label { Text = "spacer", [Grow] = 1 },
                new Label { Text = "Help"   },
            ]
        };
    }

    [Fact]
    public void FlexBox_Card_ColumnWithShrinkingBody()
    {
        var card = new FlexBox
        {
            FlexDirection  = FlexBox.Direction.Column,
            FlexWrap       = FlexBox.Wrap.NoWrap,
            FlexAlignItems = FlexBox.AlignItems.Stretch,
            RowGap = 4,
            Content =
            [
                new Label { Text = "Title"  },
                new Label { Text = "Body", [Grow] = 1, [Shrink] = 1 },
                new Label { Text = "Footer" },
            ]
        };
    }
}
