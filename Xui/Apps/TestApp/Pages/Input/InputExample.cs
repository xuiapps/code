using System.Runtime.InteropServices;
using Xui.Apps.TestApp.Examples.Input;
using Xui.Core.Canvas;
using Xui.Core.Math2D;
using Xui.Core.UI;
using Xui.Core.UI.Input;
using static Xui.Core.Canvas.Colors;
using static Xui.Core.Canvas.FontWeight;

namespace Xui.Apps.TestApp.Pages.Input;

public class InputExample : Example
{
    public InputExample()
    {
        Title = "Input Widgets";
        Content = new InputPanel();
    }

    public class InputPanel : View
    {
        private VerticalStack list;
        private View? demo;

        public override int Count =>
            (list is not null ? 1 : 0) + (demo is not null ? 1 : 0);

        public override View this[int index] => index switch
        {
            0 => list,
            1 when demo is not null => demo,
            _ => throw new IndexOutOfRangeException()
        };

        public View? Demo
        {
            get => demo;
            set => this.SetProtectedChild(ref demo, value);
        }

        public InputPanel()
        {
            list = new VerticalStack();
            this.AddProtectedChild(list);

            AddDemo<NumberBoxDemo>("Number Input");
            AddDemo<PatternBoxDemo>("Pattern Input");
            AddDemo<CurrencyBoxDemo>("Currency Input");

            // Select first demo by default
            Demo = new NumberBoxDemo();
        }

        public void AddDemo<T>(string name) where T : View, new()
        {
            list.Add(new InputDemoButton(() =>
            {
                Demo = new T();
            })
            {
                Id = name.Replace(" ", ""),
                Margin = 3,
                Text = name,
                FontFamily = ["Inter"]
            });
        }

        protected override Size MeasureCore(Size availableBorderEdgeSize, IMeasureContext context)
        {
            NFloat listWidth = 180;

            list.Measure(new Size(listWidth, availableBorderEdgeSize.Height), context);
            demo?.Measure(new Size(availableBorderEdgeSize.Width - listWidth, availableBorderEdgeSize.Height), context);

            return availableBorderEdgeSize;
        }

        protected override void ArrangeCore(Rect rect, IMeasureContext context)
        {
            NFloat listWidth = 180;

            list.Arrange(new Rect(rect.X, rect.Y, listWidth, rect.Height), context);
            demo?.Arrange(new Rect(rect.X + listWidth, rect.Y, rect.Width - listWidth, rect.Height), context);
        }

        protected override void RenderCore(IContext context)
        {
            NFloat listWidth = 180;

            context.SetFill(new Color(0xF0, 0xF0, 0xF0, 0xFF));
            context.FillRect(new Rect(
                Frame.X + listWidth,
                Frame.Y,
                Frame.Width - listWidth,
                Frame.Height));

            context.Save();
            base.RenderCore(context);
            context.Restore();
        }
    }

    public class InputDemoButton : Label
    {
        private readonly Action onClick;
        private bool hover;
        private bool pressed;

        public InputDemoButton(Action onClick)
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
                    this.InvalidateRender();
                }
                else if (e.Type == PointerEventType.Leave)
                {
                    hover = false;
                    this.InvalidateRender();
                }
                else if (phase == EventPhase.Tunnel && e.Type == PointerEventType.Down)
                {
                    this.CapturePointer(e.PointerId);
                    pressed = true;
                    this.InvalidateRender();
                }
                else if (phase == EventPhase.Tunnel && e.Type == PointerEventType.Up)
                {
                    this.ReleasePointer(e.PointerId);
                    this.onClick();
                    pressed = false;
                    this.InvalidateRender();
                }
            }

            base.OnPointerEvent(ref e, phase);
        }

        protected override void RenderCore(IContext context)
        {
            if (pressed)
            {
                context.SetFill(Yellow);
                context.FillRect(this.Frame);
            }
            else if (hover)
            {
                context.SetFill(LightGray);
                context.FillRect(this.Frame);
            }

            base.RenderCore(context);
        }
    }
}
