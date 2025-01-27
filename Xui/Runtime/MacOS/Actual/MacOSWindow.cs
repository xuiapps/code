using System;
using Xui.Core.Abstract.Events;
using Xui.Core.Math2D;
using static Xui.Runtime.MacOS.AppKit;
using static Xui.Runtime.MacOS.AppKit.NSEventRef;
using static Xui.Runtime.MacOS.CoreAnimation;
using static Xui.Runtime.MacOS.Foundation;
using static Xui.Runtime.MacOS.ObjC;

namespace Xui.Runtime.MacOS.Actual;

public class MacOSWindow : NSWindow, Xui.Core.Actual.IWindow
{
    public static readonly Sel CloseSel = new Sel("close");

    public static Sel AnimationFrameSel = new Sel("animationFrame:");

    protected static unsafe new readonly Class Class = NSWindow.Class
        .Extend("XUIMacOSWindow")
        .AddMethod("sendEvent:", SendEvent)
        .AddMethod("animationFrame:", AnimationFrame)
        .AddMethod("close", Close)
        .Register();
    
    protected static void SendEvent(nint self, nint sel, nint e) =>
        Marshalling.Get<MacOSWindow>(self)
            .SendEvent(sel, new NSEventRef(e));

    public static void AnimationFrame(nint self, nint sel, nint caDisplayLink) =>
        Marshalling.Get<MacOSWindow>(self).AnimationFrame(caDisplayLink);

    protected static void Close(nint self, nint sel) =>
        Marshalling.Get<MacOSWindow>(self)
            .Close();

    private CADisplayLink displayLink;
    private TimeSpan previousFrameTime;
    private TimeSpan nextFrameTime;

    public static nint InitWithAbstract(Xui.Core.Abstract.IWindow @abstract)
    {
        NSWindowStyleMask mask = 
            NSWindowStyleMask.Titled |
            NSWindowStyleMask.Closable |
            NSWindowStyleMask.Miniaturizable |
            NSWindowStyleMask.Resizable;
        
        Rect rect = new Rect(200, 200, 600, 400);
        
        if (@abstract is Xui.Core.Abstract.IWindow.IDesktopStyle dws)
        {
            if (dws.Chromeless)
            {
                mask = 
                    // NSWindowStyleMask.Titled |
                    // NSWindowStyleMask.Closable |
                    // NSWindowStyleMask.Miniaturizable |
                    // NSWindowStyleMask.Resizable;
                    NSWindowStyleMask.FullSizeContentView |
                    NSWindowStyleMask.Borderless;
            }

            if (dws.StartupSize.HasValue)
            {
                rect.Size = dws.StartupSize.Value;
            }
        }

        return InitWithContentRectStyleMaskBackingDefer(
            Class.Alloc(),
            rect: rect,
            nswindowstylemask: mask,
            nsbackingstoretype: NSBackingStoreType.Buffered,
            defer: false
        );
    }

    public MacOSWindow(Xui.Core.Abstract.IWindow @abstract) : base(InitWithAbstract(@abstract))
    {
        this.ContentView = new MacOSWindowRootView(this);

        this.Abstract = @abstract;
        this.Title = "";
        this.Delegate = new MacOSWindowDelegate(this);

        this.IsReleasedWhenClosed = false;
        this.AcceptsMouseMovedEvents = true;

        if (@abstract is Xui.Core.Abstract.IWindow.IDesktopStyle dws && dws.Chromeless)
        {
            using var transparent = new NSColorRef(0, 0, 0, 0);
            this.BackgroundColor = transparent;
            this.TitleVisibility = NSWindowTitleVisibility.Hidden;
            this.TitlebarAppearsTransparent = true;
        }

        this.displayLink = CADisplayLink.DisplayLink(this, AnimationFrameSel);
        this.displayLink.AddToRunLoopForMode(NSRunLoop.MainRunLoop, NSRunLoop.Mode.Common);
    }

    protected internal Xui.Core.Abstract.IWindow Abstract { get; }

    string Xui.Core.Actual.IWindow.Title
    {
        get => this.Title!;
        set => this.Title = value;
    }

    void Xui.Core.Actual.IWindow.Show() => this.MakeKeyAndOrderFront();

    protected void SendEvent(nint sel, NSEventRef e)
    {
        var type = e.Type;
        // System.Diagnostics.Debug.WriteLine("XuiWindow sendEvent " + type);
        if (type == NSEventType.AppKitDefined)
        {
        }
        else if (type == NSEventType.MouseEntered)
        {
            var rect =this.Rect;
            WindowHitTestEventRef eventRef = new ()
            {
                Area = WindowHitTestEventRef.WindowArea.Default,
                Point = e.LocationInWindow,
                Window = rect
            };
            this.Abstract.WindowHitTest(ref eventRef);
            if (eventRef.Area != WindowHitTestEventRef.WindowArea.Default)
            {
                // TODO: Set mouse cursor and organize drag and resize...
                // return;
            }
        }
        else if (type == NSEventType.KeyDown || type == NSEventType.KeyUp)
        {
            ushort keyCode = e.KeyCode;
            string? characters = e.Characters;
            // Debug.WriteLine("XuiWindow sendEvent " + type + " " + keyCode + " " + characters);
        }
        else if (
            type == NSEventType.MouseMoved ||
            type == NSEventType.LeftMouseDragged ||
            type == NSEventType.RightMouseDragged ||
            type == NSEventType.OtherMouseDragged)
        {
            var rect = this.Rect;
            var position = this.ContentView!.ConvertPointFromView(e.LocationInWindow, null);

            WindowHitTestEventRef eventRef = new ()
            {
                Area = WindowHitTestEventRef.WindowArea.Default,
                Point = position,
                Window = new Rect(0, 0, rect.Size.width, rect.Size.height)
            };
            this.Abstract.WindowHitTest(ref eventRef);

            var evRef = new MouseMoveEventRef()
            {
                Position = position
            };
            this.Abstract.OnMouseMove(ref evRef);
            // TODO: Consider the position reported by mouse move to update the cursor...

            if (eventRef.Area != WindowHitTestEventRef.WindowArea.Default)
            {
                switch(eventRef.Area)
                {
                    case WindowHitTestEventRef.WindowArea.BorderTopLeft:
                    case WindowHitTestEventRef.WindowArea.BorderBottomRight:
                        NSCursor._WindowResizeNorthWestSouthEastCursor.Set();
                        break;
                    case WindowHitTestEventRef.WindowArea.BorderTopRight:
                    case WindowHitTestEventRef.WindowArea.BorderBottomLeft:
                        NSCursor._WindowResizeNorthEastSouthWestCursor.Set();
                        break;
                    case WindowHitTestEventRef.WindowArea.BorderTop:
                    case WindowHitTestEventRef.WindowArea.BorderBottom:
                        NSCursor._WindowResizeNorthSouthCursor.Set();
                        break;
                    case WindowHitTestEventRef.WindowArea.BorderLeft:
                    case WindowHitTestEventRef.WindowArea.BorderRight:
                        NSCursor._WindowResizeEastWestCursor.Set();
                        break;
                    default:
                        NSCursor.ArrowCursor.Set();
                        break;
                }

                // TODO: Organize drag and resize...
                // return;
            }
            // Debug.WriteLine("XuiWindow sendEvent " + type + " " + point.x + " : " + point.y);
        }
        else if (type == NSEventType.ScrollWheel)
        {
            var evRef = new ScrollWheelEventRef()
            {
                Delta = e.ScrollingDelta
            };
            this.Abstract.OnScrollWheel(ref evRef);
        }
        else if (type == NSEventType.LeftMouseDown || type == NSEventType.RightMouseDown || type == NSEventType.OtherMouseDown)
        {
            var rect = this.Rect;
            var position = this.ContentView!.ConvertPointFromView(e.LocationInWindow, null);

            WindowHitTestEventRef eventRef = new ()
            {
                Area = WindowHitTestEventRef.WindowArea.Default,
                Point = position,
                Window = new Rect(0, 0, rect.Size.width, rect.Size.height)
            };
            this.Abstract.WindowHitTest(ref eventRef);

            var evRef = new MouseDownEventRef()
            {
                Position = position
            };
            switch(type)
            {
                case NSEventType.LeftMouseDown:
                    evRef.Button = MouseButton.Left;
                    break;
                case NSEventType.RightMouseDown:
                    evRef.Button = MouseButton.Right;
                    break;
                case NSEventType.OtherMouseDown:
                    evRef.Button = MouseButton.Other;
                    break;
            }
            this.Abstract.OnMouseDown(ref evRef);
        }
        else if (type == NSEventType.LeftMouseUp || type == NSEventType.RightMouseUp || type == NSEventType.OtherMouseUp)
        {
            var rect = this.Rect;
            var position = this.ContentView!.ConvertPointFromView(e.LocationInWindow, null);

            WindowHitTestEventRef eventRef = new ()
            {
                Area = WindowHitTestEventRef.WindowArea.Default,
                Point = position,
                Window = new Rect(0, 0, rect.Size.width, rect.Size.height)
            };
            this.Abstract.WindowHitTest(ref eventRef);

            var evRef = new MouseUpEventRef()
            {
                Position = position
            };
            switch(type)
            {
                case NSEventType.LeftMouseDown:
                    evRef.Button = MouseButton.Left;
                    break;
                case NSEventType.RightMouseDown:
                    evRef.Button = MouseButton.Right;
                    break;
                case NSEventType.OtherMouseDown:
                    evRef.Button = MouseButton.Other;
                    break;
            }
            this.Abstract.OnMouseUp(ref evRef);
        }
        else if (type == NSEventType.Pressure)
        {
            // Debug.WriteLine("Pressure: " + e.Pressure);
        }
        else if (type == NSEventRef.NSEventType.AppKitDefined)
        {
            var subtype = e.Subtype;
            // Debug.WriteLine("XuiWindow sendEvent " + type + " " + subtype);
        }
        else
        {
            // Debug.WriteLine("XuiWindow sendEvent " + type);
        }

        // Note you don't have to call super, and if you don't native UI wont receive events... even paint..
        Super super = new Super(this, NSWindow.Class);
        ObjC.objc_msgSendSuper(ref super, sel, e);
    }

    protected void Close()
    {
        this.Abstract.Closed();
        Super super = new Super(this, NSWindow.Class);
        objc_msgSendSuper(ref super, CloseSel);
    }

    internal bool Closing() => this.Abstract.Closing();

    private void AnimationFrame(nint caDisplayLink)
    {
        this.previousFrameTime = this.displayLink.Timestamp;
        this.nextFrameTime = this.displayLink.TargetTimestamp;
        var animationFrame = new FrameEventRef(this.previousFrameTime, this.nextFrameTime);
        this.Abstract.OnAnimationFrame(ref animationFrame);
    }

    public void Invalidate() => this.ContentView!.NeedsDisplay = true;

    internal void Render(NSRect rect)
    {
        FrameEventRef frame = new (this.previousFrameTime, this.nextFrameTime);
        RenderEventRef render = new (rect, frame);

        this.Abstract.Render(ref render);
    }
}
