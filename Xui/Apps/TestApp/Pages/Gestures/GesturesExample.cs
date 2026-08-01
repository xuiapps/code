using System.Runtime.InteropServices;
using Xui.Apps.TestApp.Examples;
using Xui.Apps.TestApp.Pages.Gestures.Tests;
using Xui.Core.Canvas;
using Xui.Core.DI;
using Xui.Core.Math2D;
using Xui.Core.UI;
using Xui.Core.UI.Input;
using Xui.DevKit.UI.Design;
using static Xui.Core.Canvas.Colors;

namespace Xui.Apps.TestApp.Pages.Gestures;

public class GesturesExample : Example
{
    private XuiDesignSystem? designSystem;

    public GesturesExample()
    {
        Title = "Gestures";
        Content = new GesturesPanel();
    }

    public override object? GetService(Type serviceType)
    {
        if (serviceType == typeof(IDesignSystem) && designSystem != null)
            return designSystem;
        return base.GetService(serviceType);
    }

    protected override void OnActivate()
    {
        base.OnActivate();

        var device = this.GetService(typeof(IDeviceInfo)) as IDeviceInfo;
        if (device == null) return;

        designSystem = new XuiDesignSystem(
            new XuiDesignSystemOptions
            {
                PrimaryHue = 240,
                Harmony = ColorHarmony.SplitComplementary,
                Chroma = 0.15f,
                Shape = ShapePreset.Soft,
                Sizing = SizingPreset.Mobile,
                NeutralStyle = NeutralStyle.Monochrome,
                Motion = MotionPreset.Normal,
            },
            device);
    }

    public class GesturesPanel : View
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

        public GesturesPanel()
        {
            list = new VerticalStack();
            AddProtectedChild(list);

            AddTest<NestedScrollDemo>("Nested scroll (H in V)");
            AddTest<DragVsTapDemo>("Drag vs Tap");

            Preview = new NestedScrollDemo();
        }

        public void AddTest<T>(string name) where T : View, new()
        {
            list.Add(new GestureTestButton(() => Preview = new T())
            {
                Id = name,
                Margin = 3,
                Text = name,
                FontFamily = "Inter",
            });
        }

        protected override Size MeasureCore(Size availableSize, IMeasureContext context)
        {
            NFloat listWidth = 220;

            list.Measure(new Size(listWidth, availableSize.Height), context);
            preview?.Measure(new Size(availableSize.Width - listWidth, availableSize.Height), context);

            return availableSize;
        }

        protected override void ArrangeCore(Rect rect, IMeasureContext context)
        {
            NFloat listWidth = 220;

            list.Arrange(new Rect(rect.X, rect.Y, listWidth, rect.Height), context);
            preview?.Arrange(new Rect(rect.X + listWidth, rect.Y, rect.Width - listWidth, rect.Height), context);
        }

        protected override void RenderCore(IContext context)
        {
            NFloat listWidth = 220;

            context.SetFill(new Color(0xE0, 0xE0, 0xE0, 0xFF));
            context.FillRect(new Rect(Frame.X + listWidth, Frame.Y, 1, Frame.Height));

            base.RenderCore(context);
        }
    }

    public class GestureTestButton : Label
    {
        private readonly Action onClick;
        private bool hover;
        private bool pressed;

        public GestureTestButton(Action onClick)
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
                    CapturePointer(e.PointerId, PointerGestures.Tap);
                    pressed = true;
                    InvalidateRender();
                }
                else if (phase == EventPhase.Tunnel && e.Type == PointerEventType.Up)
                {
                    // Snapshot before release: ReleasePointer fires LostCapture
                    // synchronously, which clears `pressed` via the handler below.
                    var wasPressed = pressed;
                    pressed = false;
                    ReleasePointer(e.PointerId);
                    if (wasPressed) onClick();
                    InvalidateRender();
                }
                else if (e.Type == PointerEventType.LostCapture)
                {
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
