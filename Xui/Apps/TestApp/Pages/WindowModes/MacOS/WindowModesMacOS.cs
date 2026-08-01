using Xui.Apps.TestApp.Examples;
using Xui.Core.Canvas;
using Xui.Core.Math2D;
using Xui.Core.UI;
using Xui.Core.UI.Input;
using static Xui.Core.Canvas.Colors;
using static Xui.Core.UI.Layout.Grid;
using static Xui.Core.UI.Layout.Grid.TrackSize;

namespace Xui.Apps.TestApp.Pages.WindowModes.MacOS;

/// <summary>
/// Demonstrates top-level desktop window modes. These modes intentionally do not
/// apply to mobile, browser, or emulator targets.
/// </summary>
public sealed class WindowModesMacOS : Example
{
    public WindowModesMacOS()
    {
        Title = "Window Modes (macOS)";
        Content = new Xui.Core.UI.Layout.Grid
        {
            TemplateColumns = [Auto, Auto, Auto, Auto],
            TemplateRows = [Auto, Auto, Auto, Auto, Auto, Auto, Auto, Auto],
            ColumnGap = 4,
            RowGap = 4,
            Margin = 16,
            Content =
            [
                new Label
                {
                    Text = "Desktop Window Modes (macOS)",
                    Font = new Font(20, "Inter", FontWeight.Bold),
                    Margin = (0, 0, 8, 0),
                    [ColumnStart] = 1,
                    [RowStart] = 1,
                    [ColumnSpan] = 4,
                },
                GridLabel("Titled:", 2),
                GridButton("Regular", OpenStandardWindow, 2, 2),
                GridButton("Medium", OpenTitledUnifiedCompactWindow, 2, 3),
                GridButton("Large", OpenTitledUnifiedWindow, 2, 4),
                GridLabel("Untitled:", 3),
                GridButton("Regular", OpenUntitledWindow, 3, 2),
                GridButton("Medium", OpenUntitledUnifiedCompactWindow, 3, 3),
                GridButton("Large", OpenUntitledUnifiedWindow, 3, 4),
                GridLabel("Acrylic:", 4),
                GridButton("Titled Medium", OpenTitledUnifiedCompactAcrylicWindow, 4, 2),
                GridButton("Untitled Large", OpenUntitledUnifiedAcrylicWindow, 4, 3),
                GridLabel("Glass:", 5),
                GridButton("Regular", OpenUntitledGlassWindow, 5, 2),
                GridButton("Medium", OpenUntitledUnifiedCompactGlassWindow, 5, 3),
                GridButton("Large", OpenUntitledUnifiedGlassWindow, 5, 4),
                GridButton("XuiSDK style", OpenXuiSDKStyleWindow, 6, 1, 4),
                GridLabel("Transparent:", 7),
                GridButton("Regular", OpenTransparentWindow, 7, 2),
                GridButton("Medium", OpenTransparentUnifiedCompactWindow, 7, 3),
                GridButton("Large", OpenTransparentUnifiedWindow, 7, 4),
                GridLabel("Borderless:", 8),
                GridButton("Regular", OpenBorderlessWindow, 8, 2),
                GridButton("Medium", OpenBorderlessUnifiedCompactWindow, 8, 3),
                GridButton("Large", OpenBorderlessUnifiedWindow, 8, 4),
            ],
        };
    }

    private void OpenStandardWindow()
    {
#if DESKTOP
        if (GetService(typeof(StandardDesktopWindow)) is StandardDesktopWindow window)
            window.Show();
#endif
    }

    private void OpenUntitledWindow()
    {
#if DESKTOP
        if (GetService(typeof(UntitledDesktopWindow)) is UntitledDesktopWindow window)
            window.Show();
#endif
    }

    private void OpenTitledUnifiedCompactWindow()
    {
#if DESKTOP
        if (GetService(typeof(TitledUnifiedCompactDesktopWindow)) is TitledUnifiedCompactDesktopWindow window)
            window.Show();
#endif
    }

    private void OpenTitledUnifiedWindow()
    {
#if DESKTOP
        if (GetService(typeof(TitledUnifiedDesktopWindow)) is TitledUnifiedDesktopWindow window)
            window.Show();
#endif
    }

    private void OpenUntitledUnifiedWindow()
    {
#if DESKTOP
        if (GetService(typeof(UntitledUnifiedDesktopWindow)) is UntitledUnifiedDesktopWindow window)
            window.Show();
#endif
    }

    private void OpenUntitledUnifiedCompactWindow()
    {
#if DESKTOP
        if (GetService(typeof(UntitledUnifiedCompactDesktopWindow)) is UntitledUnifiedCompactDesktopWindow window)
            window.Show();
#endif
    }

    private void OpenTitledUnifiedCompactAcrylicWindow()
    {
#if DESKTOP
        if (GetService(typeof(TitledUnifiedCompactAcrylicDesktopWindow)) is TitledUnifiedCompactAcrylicDesktopWindow window)
            window.Show();
#endif
    }

    private void OpenUntitledUnifiedAcrylicWindow()
    {
#if DESKTOP
        if (GetService(typeof(UntitledUnifiedAcrylicDesktopWindow)) is UntitledUnifiedAcrylicDesktopWindow window)
            window.Show();
#endif
    }

    private void OpenXuiSDKStyleWindow()
    {
#if DESKTOP
        if (GetService(typeof(XuiSDKStyleDesktopWindow)) is XuiSDKStyleDesktopWindow window)
            window.Show();
#endif
    }

    private void OpenUntitledGlassWindow()
    {
#if DESKTOP
        if (GetService(typeof(UntitledGlassDesktopWindow)) is UntitledGlassDesktopWindow window)
            window.Show();
#endif
    }

    private void OpenUntitledUnifiedCompactGlassWindow()
    {
#if DESKTOP
        if (GetService(typeof(UntitledUnifiedCompactGlassDesktopWindow)) is UntitledUnifiedCompactGlassDesktopWindow window)
            window.Show();
#endif
    }

    private void OpenUntitledUnifiedGlassWindow()
    {
#if DESKTOP
        if (GetService(typeof(UntitledUnifiedGlassDesktopWindow)) is UntitledUnifiedGlassDesktopWindow window)
            window.Show();
#endif
    }

    private void OpenTransparentWindow()
    {
#if DESKTOP
        if (GetService(typeof(TransparentDesktopWindow)) is TransparentDesktopWindow window)
            window.Show();
#endif
    }

    private void OpenTransparentUnifiedCompactWindow()
    {
#if DESKTOP
        if (GetService(typeof(TransparentUnifiedCompactDesktopWindow)) is TransparentUnifiedCompactDesktopWindow window)
            window.Show();
#endif
    }

    private void OpenTransparentUnifiedWindow()
    {
#if DESKTOP
        if (GetService(typeof(TransparentUnifiedDesktopWindow)) is TransparentUnifiedDesktopWindow window)
            window.Show();
#endif
    }

    private void OpenBorderlessWindow()
    {
#if DESKTOP
        if (GetService(typeof(BorderlessDesktopWindow)) is BorderlessDesktopWindow window)
            window.Show();
#endif
    }

    private void OpenBorderlessUnifiedCompactWindow()
    {
#if DESKTOP
        if (GetService(typeof(BorderlessUnifiedCompactDesktopWindow)) is BorderlessUnifiedCompactDesktopWindow window)
            window.Show();
#endif
    }

    private void OpenBorderlessUnifiedWindow()
    {
#if DESKTOP
        if (GetService(typeof(BorderlessUnifiedDesktopWindow)) is BorderlessUnifiedDesktopWindow window)
            window.Show();
#endif
    }

    private static Label GridLabel(string text, int row) => new()
    {
        Text = text,
        FontFamily = "Inter",
        VerticalAlignment = VerticalAlignment.Middle,
        [ColumnStart] = 1,
        [RowStart] = row,
    };

    private static OpenWindowButton GridButton(string text, Action action, int row, int column, int columnSpan = 1) => new(action)
    {
        Text = text,
        TextColor = White,
        Blue = true,
        MinimumWidth = 120,
        MinimumHeight = 32,
        HorizontalAlignment = HorizontalAlignment.Center,
        VerticalAlignment = VerticalAlignment.Middle,
        Margin = 4,
        [ColumnStart] = column,
        [RowStart] = row,
        [ColumnSpan] = columnSpan,
    };

    private sealed class OpenWindowButton : Label
    {
        private readonly Action action;
        private bool hovered;
        private bool pressed;

        public bool Blue { get; init; }

        public OpenWindowButton(Action action)
        {
            this.action = action;
            FontFamily = "Inter";
            Margin = (0, 0, 0, 0);
        }

        public override void OnPointerEvent(ref PointerEventRef e, EventPhase phase)
        {
            if (e.Type == PointerEventType.Enter)
            {
                hovered = true;
                InvalidateRender();
            }
            else if (e.Type == PointerEventType.Leave)
            {
                hovered = false;
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
                var wasPressed = pressed;
                pressed = false;
                ReleasePointer(e.PointerId);
                if (wasPressed && Frame.Contains(e.State.Position))
                    action();
                InvalidateRender();
            }
            else if (e.Type == PointerEventType.LostCapture)
            {
                pressed = false;
                hovered = false;
                InvalidateRender();
            }

            base.OnPointerEvent(ref e, phase);
        }

        protected override void RenderCore(IContext context)
        {
            if (Blue)
            {
                context.BeginPath();
                context.RoundRect(Frame, 6);
                context.SetFill(pressed
                    ? new Color(0x004C99FF)
                    : hovered
                        ? new Color(0x0070E0FF)
                        : new Color(0x0A84FFFF));
                context.Fill(FillRule.NonZero);
            }
            else if (pressed)
            {
                context.SetFill(Yellow);
                context.FillRect(Frame);
            }
            else if (hovered)
            {
                context.SetFill(LightGray);
                context.FillRect(Frame);
            }

            context.SetFont(Font);
            context.TextAlign = TextAlign.Center;
            context.TextBaseline = TextBaseline.Middle;
            context.SetFill(TextColor);
            context.FillText(Text, Frame.Center);
        }
    }
}
