using nfloat = System.Runtime.InteropServices.NFloat;
#pragma warning restore CS8981
using System.Runtime.InteropServices;
using Xui.Apps.TestApp.Pages.FlexBox.Tests;
using Xui.Core.Canvas;
using Xui.Core.Math2D;
using Xui.Core.UI;
using Xui.Core.UI.Input;
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

            AddTest<BasicRowTest>("Basic row");
            AddTest<BasicColumnTest>("Basic column");
            AddTest<GrowAndShrinkTest>("Grow & shrink");
            AddTest<WrapTest>("Wrapping");
            AddTest<JustifyContentTest>("Justify content");
            AddTest<AlignItemsTest>("Align items");
            AddTest<AlignContentTest>("Align content");
            AddTest<GapsTest>("Gaps");
            AddTest<ReverseDirectionTest>("Reverse direction");
            AddTest<NestedFlexTest>("Nested flex");
            AddTest<ResponsiveLayoutTest>("Responsive layout");
            AddTest<MixedSizingTest>("Mixed sizing");

            Preview = new BasicRowTest();
        }

        public void AddTest<T>(string name) where T : View, new()
        {
            list.Add(new FlexBoxNavButton(() => Preview = new T())
            {
                Id = name,
                Margin = 3,
                Text = name,
                FontFamily = ["Inter"],
            });
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
    }

    public class FlexBoxNavButton : Label
    {
        private readonly Action onClick;
        private bool hover;
        private bool pressed;

        public FlexBoxNavButton(Action onClick)
        {
            this.onClick = onClick;
        }

        public override void OnPointerEvent(ref PointerEventRef e, EventPhase phase)
        {
            if (e.State.PointerType == PointerType.Mouse)
            {
                if (e.Type == PointerEventType.Enter)
                {
                    hover = true;
                    InvalidateRender();
                }
                else if (e.Type == PointerEventType.Leave)
                {
                    hover = false;
                    InvalidateRender();
                }
                else if (phase == EventPhase.Tunnel && e.Type == PointerEventType.Down)
                {
                    CapturePointer(e.PointerId);
                    pressed = true;
                    InvalidateRender();
                }
                else if (phase == EventPhase.Tunnel && e.Type == PointerEventType.Up)
                {
                    ReleasePointer(e.PointerId);
                    onClick();
                    pressed = false;
                    InvalidateRender();
                }
            }

            base.OnPointerEvent(ref e, phase);
        }

        protected override void RenderCore(IContext context)
        {
            if (pressed)
            {
                context.SetFill(Yellow);
                context.FillRect(Frame);
            }
            else if (hover)
            {
                context.SetFill(LightGray);
                context.FillRect(Frame);
            }

            base.RenderCore(context);
        }
    }
}
