using nfloat = System.Runtime.InteropServices.NFloat;
#pragma warning restore CS8981
using System.Runtime.InteropServices;
using Xui.Apps.TestApp.Pages.FlexBox.Tests;
using Xui.Core.Canvas;
using Xui.Core.Math2D;
using Xui.Core.UI;
using static Xui.Core.Canvas.Colors;

namespace Xui.Apps.TestApp.Pages.FlexBox;

public class FlexBoxExample : Example
{
    public FlexBoxExample()
    {
        Title = "FlexBox Layout";
        Content = new FlexBoxExamplePanel();
    }

    public class FlexBoxExamplePanel : View
    {
        private readonly VerticalStack list;
        private View? preview;

        public override int Count =>
            1 + (preview is not null ? 1 : 0);

        public override View this[int index] => index switch
        {
            0 => list,
            1 when preview is not null => preview,
            _ => throw new IndexOutOfRangeException(),
        };

        public View? Preview
        {
            get => preview;
            set => SetProtectedChild(ref preview, value);
        }

        public FlexBoxExamplePanel()
        {
            list = new VerticalStack();
            AddProtectedChild(list);

            list.Add(new Label { Text = "Basic row", Margin = 3 });
            list.Add(new Label { Text = "Basic column", Margin = 3 });
            list.Add(new Label { Text = "Grow & shrink", Margin = 3 });
            list.Add(new Label { Text = "Wrapping", Margin = 3 });
            list.Add(new Label { Text = "Justify content", Margin = 3 });
            list.Add(new Label { Text = "Align items", Margin = 3 });
            list.Add(new Label { Text = "Align content", Margin = 3 });
            list.Add(new Label { Text = "Gaps", Margin = 3 });
            list.Add(new Label { Text = "Reverse direction", Margin = 3 });
            list.Add(new Label { Text = "Nested flex", Margin = 3 });
            list.Add(new Label { Text = "Responsive layout", Margin = 3 });
            list.Add(new Label { Text = "Mixed sizing", Margin = 3 });

            Preview = new BasicRowTest();
        }

        protected override Size MeasureCore(Size availableSize, IMeasureContext context)
        {
            nfloat listWidth = 200;

            list.Measure(new Size(listWidth, availableSize.Height), context);
            preview?.Measure(new Size(availableSize.Width - listWidth, availableSize.Height), context);

            return availableSize;
        }

        protected override void ArrangeCore(Rect rect, IMeasureContext context)
        {
            nfloat listWidth = 200;

            list.Arrange(new Rect(rect.X, rect.Y, listWidth, rect.Height), context);
            preview?.Arrange(new Rect(rect.X + listWidth, rect.Y, rect.Width - listWidth, rect.Height), context);
        }

        protected override void RenderCore(IContext context)
        {
            context.SetFill(Gray9);
            context.FillRect(Frame);
            base.RenderCore(context);
        }
    }
}
