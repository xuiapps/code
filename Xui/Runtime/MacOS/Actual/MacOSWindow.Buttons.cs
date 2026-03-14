using System.Runtime.InteropServices;
using static Xui.Runtime.MacOS.AppKit;
using static Xui.Runtime.MacOS.Foundation;

namespace Xui.Runtime.MacOS.Actual;

public partial class MacOSWindow
{
    internal bool IsExitingFullScreen { get; set; }

    public void HideTitleButtons()
    {
        for (int i = 0; i <= 2; i++)
        {
            nint button = StandardWindowButton((NSWindowButton)i);
            if (button != 0)
                NSView.SetHidden(button, true);
        }
    }

    /// <summary>
    /// Repositions the macOS traffic-light buttons according to
    /// <see cref="Xui.Core.Abstract.IWindow.IDesktopStyle.MacOSWindowSystemButtonsOffset"/>.
    /// During a fullscreen-exit transition the container is hidden to avoid visual glitches;
    /// call this again once the transition completes to restore them.
    /// </summary>
    internal void PositionSystemButtons()
    {
        if (Abstract is not Xui.Core.Abstract.IWindow.IDesktopStyle dws)
            return;
        var offset = dws.MacOSWindowSystemButtonsOffset;
        if (offset is null)
            return;

        nint closeBtn       = StandardWindowButton(NSWindowButton.CloseButton);
        nint miniaturizeBtn = StandardWindowButton(NSWindowButton.MiniaturizeButton);
        nint zoomBtn        = StandardWindowButton(NSWindowButton.ZoomButton);
        if (closeBtn == 0)
            return;

        // Walk up: button → groupView → titleBarContainerView
        nint containerView = NSView.GetSuperview(NSView.GetSuperview(closeBtn));
        if (containerView == 0)
            return;

        if (IsExitingFullScreen)
        {
            NSView.SetHidden(containerView, true);
            return;
        }

        NSView.SetHidden(containerView, false);

        NSRect closeBtnFrame       = NSView.GetFrame(closeBtn);
        NSRect miniaturizeBtnFrame = NSView.GetFrame(miniaturizeBtn);

        NFloat buttonHeight  = closeBtnFrame.Size.height;
        NFloat spaceBetween  = miniaturizeBtnFrame.Origin.x - closeBtnFrame.Origin.x;
        NFloat titleBarHeight = buttonHeight + (NFloat)offset.Value.Y;

        // Resize the container and anchor it to the window top (AppKit Y is bottom-up).
        NSRect windowRect = Rect;
        NSRect containerRect = NSView.GetFrame(containerView);
        containerRect.Size.height  = titleBarHeight;
        containerRect.Origin.y     = windowRect.Size.height - titleBarHeight;
        NSView.SetFrame(containerView, containerRect);

        // Reposition each button, preserving original inter-button spacing.
        nint[] buttons = [closeBtn, miniaturizeBtn, zoomBtn];
        for (int i = 0; i < buttons.Length; i++)
        {
            NSRect btnRect = NSView.GetFrame(buttons[i]);
            btnRect.Origin.x = (NFloat)offset.Value.X + i * spaceBetween;
            btnRect.Origin.y = (titleBarHeight - btnRect.Size.height) / 2;
            NSView.SetFrame(buttons[i], btnRect);
        }
    }
}
