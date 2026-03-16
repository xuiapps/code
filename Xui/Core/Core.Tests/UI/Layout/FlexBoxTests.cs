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

        Assert.Equal(FlexBox.Direction.Row,            toolbar.FlexDirection);
        Assert.Equal(FlexBox.AlignItems.Center,        toolbar.FlexAlignItems);
        Assert.Equal(FlexBox.JustifyContent.FlexStart, toolbar.FlexJustifyContent);
        Assert.Equal((nfloat)8,                        toolbar.ColumnGap);
        Assert.Equal(5,                                toolbar.Count);

        var spacer = (Label)toolbar[3];
        Assert.Equal((nfloat)1, spacer[Grow]);
        Assert.Equal((nfloat)1, spacer[Shrink]);   // default per CSS spec
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

        Assert.Equal(FlexBox.Direction.Column,   card.FlexDirection);
        Assert.Equal(FlexBox.Wrap.NoWrap,        card.FlexWrap);
        Assert.Equal(FlexBox.AlignItems.Stretch, card.FlexAlignItems);
        Assert.Equal(3,                          card.Count);

        var body = (Label)card[1];
        Assert.Equal("Body",    body.Text);
        Assert.Equal((nfloat)1, body[Grow]);
        Assert.Equal((nfloat)1, body[Shrink]);
    }
}
