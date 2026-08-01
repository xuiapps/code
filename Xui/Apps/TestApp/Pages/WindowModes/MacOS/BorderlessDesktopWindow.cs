using System.Runtime.InteropServices;
using Xui.Core.Abstract;
using Xui.Core.Abstract.Events;
using Xui.Core.Canvas;
using Xui.Core.Math2D;
using Xui.Core.UI;
using static Xui.Core.UI.Layout.Grid;
using static Xui.Core.UI.Layout.Grid.TrackSize;

namespace Xui.Apps.TestApp.Pages.WindowModes.MacOS;

/// <summary>
/// Shared application-drawn frame for transparent borderless tool windows.
/// Unlike <see cref="TransparentWindow"/>, this window defines the title and
/// resize regions itself.
/// </summary>
public abstract class BorderlessWindow : Window
{
    private static readonly NFloat HeaderGap = 8;
    private static readonly NFloat BodyCornerRadius = 64;
    private static readonly NFloat BodyBorderWidth = 3;
    private static readonly NFloat ResizeHitSize = 48;
    private readonly NFloat headerHeight;

    protected BorderlessWindow(
        IServiceProvider context,
        string heading,
        string description,
        MacOSWindowTitleHeight titleHeight) : base(context)
    {
        this.headerHeight = titleHeight switch
        {
            MacOSWindowTitleHeight.Default => 28,
            MacOSWindowTitleHeight.Medium => 38,
            MacOSWindowTitleHeight.Large => 52,
            _ => 28,
        };

        Content = new Border
        {
            Content = new Xui.Core.UI.Layout.Grid
            {
                TemplateColumns = [Fr(1)],
                TemplateRows = [Px(this.headerHeight), Px(HeaderGap), Fr(1)],
                Content = [
                    new Border
                    {
                        CornerRadius = 8,
                        BorderThickness = 1,
                        BorderColor = Colors.DarkGray,
                        BackgroundColor = Colors.Gray,
                        Content = new Label
                        {
                            Text = heading,
                            Font = new Font(14, "Inter", FontWeight.SemiBold),
                            HorizontalAlignment = HorizontalAlignment.Center,
                            VerticalAlignment = VerticalAlignment.Middle,
                        }
                    },
                    new Border
                    {
                        BorderColor = Colors.Gray,
                        BackgroundColor = Colors.LightGray,
                        BorderThickness = 3,
                        CornerRadius = 64,
                        [RowStart] = 3,
                    }
                ]
            }
        };
    }

    /// <summary>
    /// Defines custom drag and resize regions for the transparent window.
    /// </summary>
    public override void WindowHitTest(ref WindowHitTestEventRef e)
    {
        var point = e.Point;
        var window = e.Window;

        // The first row is the application-drawn title bar. It is draggable,
        // but never a resize affordance.
        var header = new Rect(window.Left, window.Top, window.Width, this.headerHeight);
        if (header.Contains(point))
        {
            e.Area = WindowHitTestEventRef.WindowArea.Title;
            return;
        }

        // The third grid row is the phone-like rounded surface. Its visible
        // border, rather than the transparent outer window bounds, resizes.
        var body = new Rect(
            window.Left,
            window.Top + this.headerHeight + HeaderGap,
            window.Width,
            window.Height - this.headerHeight - HeaderGap);
        var topLeftCenter = body.TopLeft + (BodyCornerRadius, BodyCornerRadius);
        var topRightCenter = body.TopRight + (-BodyCornerRadius, BodyCornerRadius);
        var bottomLeftCenter = body.BottomLeft + (BodyCornerRadius, -BodyCornerRadius);
        var bottomRightCenter = body.BottomRight + (-BodyCornerRadius, -BodyCornerRadius);

        NFloat signedBorderDistance;
        if (point.X < topLeftCenter.X && point.Y < topLeftCenter.Y)
            signedBorderDistance = (point - topLeftCenter).Magnitude - BodyCornerRadius;
        else if (point.X > topRightCenter.X && point.Y < topRightCenter.Y)
            signedBorderDistance = (point - topRightCenter).Magnitude - BodyCornerRadius;
        else if (point.X < bottomLeftCenter.X && point.Y > bottomLeftCenter.Y)
            signedBorderDistance = (point - bottomLeftCenter).Magnitude - BodyCornerRadius;
        else if (point.X > bottomRightCenter.X && point.Y > bottomRightCenter.Y)
            signedBorderDistance = (point - bottomRightCenter).Magnitude - BodyCornerRadius;
        else
            signedBorderDistance = NFloat.Max(
                NFloat.Max(body.Left - point.X, point.X - body.Right),
                NFloat.Max(body.Top - point.Y, point.Y - body.Bottom));

        if (-BodyBorderWidth <= signedBorderDistance && signedBorderDistance <= 0)
        {
            if (point.X <= body.Left + ResizeHitSize && point.Y <= body.Top + ResizeHitSize)
                e.Area = WindowHitTestEventRef.WindowArea.BorderTopLeft;
            else if (point.X >= body.Right - ResizeHitSize && point.Y <= body.Top + ResizeHitSize)
                e.Area = WindowHitTestEventRef.WindowArea.BorderTopRight;
            else if (point.X >= body.Right - ResizeHitSize && point.Y >= body.Bottom - ResizeHitSize)
                e.Area = WindowHitTestEventRef.WindowArea.BorderBottomRight;
            else if (point.X <= body.Left + ResizeHitSize && point.Y >= body.Bottom - ResizeHitSize)
                e.Area = WindowHitTestEventRef.WindowArea.BorderBottomLeft;
            else if (point.Y <= body.Top + ResizeHitSize)
                e.Area = WindowHitTestEventRef.WindowArea.BorderTop;
            else if (point.Y >= body.Bottom - ResizeHitSize)
                e.Area = WindowHitTestEventRef.WindowArea.BorderBottom;
            else if (point.X <= body.Left + ResizeHitSize)
                e.Area = WindowHitTestEventRef.WindowArea.BorderLeft;
            else if (point.X >= body.Right - ResizeHitSize)
                e.Area = WindowHitTestEventRef.WindowArea.BorderRight;
            else
                e.Area = WindowHitTestEventRef.WindowArea.Client;
        }
        else if (signedBorderDistance <= 0)
        {
            e.Area = WindowHitTestEventRef.WindowArea.Client;
        }
        else
        {
            e.Area = WindowHitTestEventRef.WindowArea.Transparent;
        }
    }
}

/// <summary>A custom-drawn, transparent borderless tool window.</summary>
public sealed class BorderlessDesktopWindow : BorderlessWindow, IWindow.IDesktopStyle, IMacOSWindowStyle
{
    MacOSWindowTitle IMacOSWindowStyle.Title => MacOSWindowTitle.Hidden;
    MacOSWindowBackground IMacOSWindowStyle.Background => MacOSWindowBackground.Borderless;
    Size? IWindow.IDesktopStyle.StartupSize => new(600, 420);

    public BorderlessDesktopWindow(IServiceProvider context)
        : base(context, "Borderless tool", "Xui draws the entire frame and supplies the drag and resize hit testing.", MacOSWindowTitleHeight.Default)
    {
        Title = "Borderless Tool";
    }
}

/// <summary>A borderless tool window with a compact unified toolbar.</summary>
public sealed class BorderlessUnifiedCompactDesktopWindow : BorderlessWindow, IWindow.IDesktopStyle, IMacOSWindowStyle
{
    MacOSWindowTitle IMacOSWindowStyle.Title => MacOSWindowTitle.Hidden;
    MacOSWindowTitleHeight IMacOSWindowStyle.TitleHeight => MacOSWindowTitleHeight.Medium;
    MacOSWindowBackground IMacOSWindowStyle.Background => MacOSWindowBackground.Borderless;
    Size? IWindow.IDesktopStyle.StartupSize => new(600, 420);

    public BorderlessUnifiedCompactDesktopWindow(IServiceProvider context)
        : base(context, "Borderless compact tool", "The compact native toolbar positions the traffic lights while Xui owns the visible chrome.", MacOSWindowTitleHeight.Medium)
    {
        Title = "Borderless Compact Tool";
    }
}

/// <summary>A borderless tool window with a unified toolbar.</summary>
public sealed class BorderlessUnifiedDesktopWindow : BorderlessWindow, IWindow.IDesktopStyle, IMacOSWindowStyle
{
    MacOSWindowTitle IMacOSWindowStyle.Title => MacOSWindowTitle.Hidden;
    MacOSWindowTitleHeight IMacOSWindowStyle.TitleHeight => MacOSWindowTitleHeight.Large;
    MacOSWindowBackground IMacOSWindowStyle.Background => MacOSWindowBackground.Borderless;
    Size? IWindow.IDesktopStyle.StartupSize => new(600, 420);

    public BorderlessUnifiedDesktopWindow(IServiceProvider context)
        : base(context, "Borderless unified tool", "The large native toolbar moves the traffic lights and drops down in full screen; Xui owns drag and resize.", MacOSWindowTitleHeight.Large)
    {
        Title = "Borderless Unified Tool";
    }
}
