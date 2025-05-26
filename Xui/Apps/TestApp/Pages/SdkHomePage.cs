using Xui.Apps.TestApp.Examples;
using Xui.Core.UI;
using static Xui.Core.Canvas.FontWeight;

namespace Xui.Apps.TestApp.Pages;

public class SdkHomePage : VerticalStack
{
    public SdkHomePage()
    {
        this.Add(new Label
        {
            Text = "Xui SDK Examples ",
            FontSize = 24,
            FontWeight = Bold
        });
        this.Add(new SdkExampleButton<TextMetricsExample>()
        {
            Margin = 3,
            Text = "TextMetrics",
        });
        this.Add(new SdkExampleButton<TextLayoutExample>()
        {
            Margin = 3,
            Text = "Text Layout",
        });
        this.Add(new SdkExampleButton<NestedStacksExample>()
        {
            Margin = 3,
            Text = "Nested Stacks"
        });
        this.Add(new SdkExampleButton<ViewCollectionAlignmentExample>() {
            Margin = 3,
            Text = "ViewCollection Alignment"
        });
    }
}