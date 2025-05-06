# Xui API Reference for AI Consumption

_Generated on 2025-05-06 18:41 UTC_

This document is an automatically generated API reference extracted directly from the Xui source code.
It is optimized for AI models to efficiently understand the framework.

## Assembly: Core

### Abstract/Application.cs

```csharp
using Xui.Core.Actual;

namespace Xui.Core.Abstract;
/// <summary>
/// Represents an abstract base class for Xui applications.
/// This class is paired at runtime with a platform-specific counterpart,
/// which delegates to actual system APIs on macOS, Windows, Android, etc.
///
/// Users should subclass <see cref = "Application"/>, override the <see cref = "Start"/> method,
/// and call <see cref = "Run"/> to launch the application.
/// </summary>
public abstract class Application
{
    /// <summary>
    /// Initializes a new instance of the <see cref = "Application"/> class.
    /// </summary>
    public Application()
    {
    }

    /// <summary>
    /// Starts the main application loop by delegating to the platform-specific run loop.
    /// This method may block until the application exits,
    /// or may return immediately if the platform bootstraps a runtime loop before instantiating the app delegate.
    /// </summary>
    /// <returns>The application’s exit code.</returns>
    public int Run()
    {
    }

    /// <summary>
    /// Called by the runtime after initialization.
    /// Override this method to set up application state and display the initial UI.
    /// </summary>
    public abstract void Start();
}
```

### Abstract/Events/DeleteBackwardsEventRef.cs

```csharp
namespace Xui.Core.Abstract.Events;
/// <summary>
/// Represents a platform-level input event requesting deletion of content
/// positioned logically before the caret or selection range.
/// Typically triggered by a "Backspace" key press or an equivalent gesture.
/// </summary>
/// <remarks>
/// This event originates from the <c>Actual</c> window layer and is forwarded
/// to the <c>Abstract</c> window for dispatching through the view hierarchy.
/// It is intended to be routed to the appropriate focused or editable view.
/// </remarks>
public ref struct DeleteBackwardsEventRef
{
    /// <summary>
    /// Initializes a new instance of the <see cref = "DeleteBackwardsEventRef"/> struct.
    /// </summary>
    public DeleteBackwardsEventRef()
    {
    }
}
```

### Abstract/Events/FrameEventRef.cs

```csharp
namespace Xui.Core.Abstract.Events;
/// <summary>
/// Represents a platform-level event dispatched once per frame,
/// providing timing information used for driving animations and visual updates.
/// </summary>
/// <remarks>
/// This event is emitted from the <c>Actual</c> window and forwarded
/// to the <c>Abstract</c> window, typically during the animation phase
/// of the UI lifecycle. It provides both the previous and next frame times,
/// along with the time delta between them. Consumers can use this data
/// to advance animations or perform time-based layout updates.
/// </remarks>
public ref struct FrameEventRef
{
    /// <summary>
    /// The timestamp of the previous frame.
    /// </summary>
    public TimeSpan Previous;
    /// <summary>
    /// The timestamp of the upcoming frame.
    /// </summary>
    public TimeSpan Next;
    /// <summary>
    /// The time elapsed between the previous and next frames.
    /// </summary>
    public TimeSpan Delta;
    /// <summary>
    /// Initializes a new instance of the <see cref = "FrameEventRef"/> struct
    /// with the given previous and next frame timestamps.
    /// </summary>
    /// <param name = "previous">The timestamp of the previous frame.</param>
    /// <param name = "next">The timestamp of the upcoming frame.</param>
    public FrameEventRef(TimeSpan previous, TimeSpan next)
    {
    }
}
```

### Abstract/Events/InsertTextEventRef.cs

```csharp
namespace Xui.Core.Abstract.Events;
/// <summary>
/// Represents a platform-level input event requesting insertion of text
/// at the current caret or selection position.
/// </summary>
/// <remarks>
/// This event originates from the <c>Actual</c> window and is forwarded
/// to the <c>Abstract</c> layer for dispatch through the view hierarchy.
/// It is typically triggered by user input such as typing characters,
/// pasting from the clipboard, or text input from IMEs.
/// </remarks>
public ref struct InsertTextEventRef
{
    /// <summary>
    /// The text to be inserted into the current input context.
    /// </summary>
    public readonly string Text;
    /// <summary>
    /// Initializes a new instance of the <see cref = "InsertTextEventRef"/> struct
    /// with the specified text to insert.
    /// </summary>
    /// <param name = "text">The string of text to be inserted.</param>
    public InsertTextEventRef(string text)
    {
    }
}
```

### Abstract/Events/MouseButton.cs

```csharp
namespace Xui.Core.Abstract.Events;
/// <summary>
/// Identifies a specific mouse button involved in a pointer event.
/// </summary>
public enum MouseButton
{
    /// <summary>
    /// The left mouse button, typically used for primary actions like selection or dragging.
    /// </summary>
    Left,
    /// <summary>
    /// The right mouse button, typically used for context menus or alternate actions.
    /// </summary>
    Right,
    /// <summary>
    /// Any other mouse button, such as middle-click or additional buttons on advanced mice.
    /// </summary>
    Other
}
```

### Abstract/Events/MouseDownEventRef.cs

```csharp
using Xui.Core.Math2D;

namespace Xui.Core.Abstract.Events;
/// <summary>
/// Represents a platform-level input event indicating that a mouse button
/// was pressed at a given position.
/// </summary>
/// <remarks>
/// This event is dispatched by the <c>Actual</c> window and forwarded
/// to the <c>Abstract</c> layer for routing through the view hierarchy.
/// It may be used to initiate focus, dragging, selection, or other pointer interactions.
/// </remarks>
public ref struct MouseDownEventRef
{
    /// <summary>
    /// The position of the mouse pointer at the time of the event,
    /// in logical window coordinates.
    /// </summary>
    public Point Position;
    /// <summary>
    /// The mouse button that was pressed.
    /// </summary>
    public MouseButton Button;
}
```

### Abstract/Events/MouseMoveEventRef.cs

```csharp
using Xui.Core.Math2D;

namespace Xui.Core.Abstract.Events;
/// <summary>
/// Represents a platform-level input event indicating that the mouse pointer
/// has moved to a new position.
/// </summary>
/// <remarks>
/// This event is dispatched by the <c>Actual</c> window and forwarded
/// to the <c>Abstract</c> layer for routing through the view hierarchy.
/// It is typically used to trigger hover effects, cursor updates,
/// or to track dragging or gesture movement.
/// </remarks>
public ref struct MouseMoveEventRef
{
    /// <summary>
    /// The current position of the mouse pointer in logical window coordinates.
    /// </summary>
    public Point Position;
}
```

### Abstract/Events/MouseUpEventRef.cs

```csharp
using Xui.Core.Math2D;

namespace Xui.Core.Abstract.Events;
/// <summary>
/// Represents a platform-level input event indicating that a mouse button
/// was released at a given position.
/// </summary>
/// <remarks>
/// This event is dispatched by the <c>Actual</c> window and forwarded
/// to the <c>Abstract</c> layer for routing through the view hierarchy.
/// It is typically used to complete interactions such as clicks, drags,
/// or other pointer-driven gestures.
/// </remarks>
public ref struct MouseUpEventRef
{
    /// <summary>
    /// The position of the mouse pointer at the time of the event,
    /// in logical window coordinates.
    /// </summary>
    public Point Position;
    /// <summary>
    /// The mouse button that was released.
    /// </summary>
    public MouseButton Button;
}
```

### Abstract/Events/RenderEventRef.cs

```csharp
using Xui.Core.Math2D;

namespace Xui.Core.Abstract.Events;
/// <summary>
/// Represents a platform-level render event dispatched after a timing frame,
/// indicating that views should perform layout and rendering operations within a given region.
/// </summary>
/// <remarks>
/// This event follows a <see cref = "FrameEventRef"/> and marks the phase during which
/// views are expected to perform their <c>Measure</c>, <c>Arrange</c>, and <c>Render</c> passes.
/// It is emitted by the <c>Actual</c> window and forwarded to the <c>Abstract</c> layer.
/// </remarks>
public ref struct RenderEventRef
{
    /// <summary>
    /// The region of the screen or surface that should be re-rendered.
    /// </summary>
    public Rect Rect;
    /// <summary>
    /// Timing information associated with this frame, typically used for animations.
    /// </summary>
    public FrameEventRef Frame;
    /// <summary>
    /// Initializes a new instance of the <see cref = "RenderEventRef"/> struct
    /// with the given invalidation region and frame timing data.
    /// </summary>
    /// <param name = "rect">The region to be rendered.</param>
    /// <param name = "frame">The timing information for this frame.</param>
    public RenderEventRef(Rect rect, FrameEventRef frame)
    {
    }
}
```

### Abstract/Events/ScrollWheelEventRef.cs

```csharp
using Xui.Core.Math2D;

namespace Xui.Core.Abstract.Events;
/// <summary>
/// Represents a platform-level input event generated by a scroll wheel or trackpad gesture,
/// indicating a change in scroll position.
/// </summary>
/// <remarks>
/// This event is dispatched by the <c>Actual</c> window and forwarded
/// to the <c>Abstract</c> layer for routing through the view hierarchy.
/// It may be used to scroll content, zoom views, or trigger kinetic effects,
/// depending on platform and modifier keys.
/// </remarks>
public ref struct ScrollWheelEventRef
{
    /// <summary>
    /// The scroll delta, typically measured in logical units per axis.
    /// Positive Y values usually indicate upward scrolling.
    /// </summary>
    public Vector Delta;
}
```

### Abstract/Events/Touch.cs

```csharp
using Xui.Core.Math2D;

namespace Xui.Core.Abstract.Events;
/// <summary>
/// Represents a single touch point within a multi-touch input event.
/// </summary>
/// <remarks>
/// This structure contains identifying and positional data for a specific finger
/// or contact point on a touch surface. Multiple <see cref = "Touch"/> instances may be
/// reported in a single event when handling gestures or complex touch interactions.
/// </remarks>
public struct Touch
{
    /// <summary>
    /// A unique index identifying this touch point during its lifetime.
    /// Typically assigned by the platform and reused after release.
    /// </summary>
    public int Index;
    /// <summary>
    /// The current position of the touch in logical window coordinates.
    /// </summary>
    public Point Position;
    /// <summary>
    /// The estimated contact radius of the touch, in logical units.
    /// Used for gesture recognition or pressure emulation.
    /// </summary>
    public nfloat Radius;
    /// <summary>
    /// The current phase of the touch (e.g., began, moved, ended).
    /// </summary>
    public TouchPhase Phase;
}
```

### Abstract/Events/TouchEventRef.cs

```csharp
namespace Xui.Core.Abstract.Events;
/// <summary>
/// Represents a platform-level input event containing one or more touch points,
/// typically originating from a touchscreen or trackpad gesture.
/// </summary>
/// <remarks>
/// This event is dispatched by the <c>Actual</c> window and forwarded
/// to the <c>Abstract</c> layer for routing through the view hierarchy.
/// It includes all active touch points for the current frame, allowing
/// gesture recognition, hit testing, and view interaction logic.
/// </remarks>
public ref struct TouchEventRef
{
    /// <summary>
    /// A span of all current touch points involved in this event.
    /// </summary>
    public readonly ReadOnlySpan<Touch> Touches;
    /// <summary>
    /// Initializes a new instance of the <see cref = "TouchEventRef"/> struct
    /// with the provided set of touch points.
    /// </summary>
    /// <param name = "touches">A span of active touch data.</param>
    public TouchEventRef(ReadOnlySpan<Touch> touches)
    {
    }
}
```

### Abstract/Events/TouchPhase.cs

```csharp
namespace Xui.Core.Abstract.Events;
/// <summary>
/// Describes the phase of an individual touch point during a touch event.
/// </summary>
public enum TouchPhase
{
    /// <summary>
    /// The touch has just started (finger or stylus contacted the surface).
    /// </summary>
    Start,
    /// <summary>
    /// The touch is actively moving across the surface.
    /// </summary>
    Move,
    /// <summary>
    /// The touch has ended (finger or stylus lifted off the surface).
    /// </summary>
    End
}
```

### Abstract/Events/WindowHitTestEventRef.cs

```csharp
using Xui.Core.Math2D;

namespace Xui.Core.Abstract.Events;
/// <summary>
/// Represents a platform-level hit test event that allows the application
/// to define how a point within a custom window frame should be interpreted
/// (e.g., as a draggable title bar, resize border, or transparent region).
/// </summary>
/// <remarks>
/// This event is dispatched when the user interacts with the non-client area
/// of a custom-framed window (such as the emulator window or a styled installer).
/// It is primarily used on platforms like Windows that support fine-grained
/// window frame interaction via hit testing.
///
/// On macOS and other platforms where native support is unavailable,
/// this event may be ignored or implemented through custom code.
/// </remarks>
public ref struct WindowHitTestEventRef
{
    /// <summary>
    /// The location of the hit test, in logical window coordinates.
    /// </summary>
    public Point Point;
    /// <summary>
    /// The bounds of the entire window, in logical coordinates.
    /// This may be used to determine edge proximity for resizing logic.
    /// </summary>
    public Rect Window;
    /// <summary>
    /// The result of the hit test, set by the application to control
    /// how the system should interpret the hit location.
    /// </summary>
    public WindowArea Area;
    /// <summary>
    /// Initializes a new instance of the <see cref = "WindowHitTestEventRef"/> struct.
    /// </summary>
    /// <param name = "point">The location of the pointer input.</param>
    /// <param name = "window">The bounds of the target window.</param>
    public WindowHitTestEventRef(Point point, Rect window)
    {
    }

    /// <summary>
    /// Describes the purpose or function of a region within a window.
    /// </summary>
    public enum WindowArea : uint
    {
        /// <summary>
        /// The area is unspecified; the platform should handle it normally.
        /// </summary>
        Default = 0,
        /// <summary>
        /// The area is transparent to hit testing and should not trigger drag or resize.
        /// </summary>
        Transparent,
        /// <summary>
        /// The area represents the client region of the window (normal content).
        /// </summary>
        Client,
        /// <summary>
        /// The area represents a draggable title bar region.
        /// </summary>
        Title,
        /// <summary>Top-left resize corner.</summary>
        BorderTopLeft,
        /// <summary>Top edge resize border.</summary>
        BorderTop,
        /// <summary>Top-right resize corner.</summary>
        BorderTopRight,
        /// <summary>Right edge resize border.</summary>
        BorderRight,
        /// <summary>Bottom-right resize corner.</summary>
        BorderBottomRight,
        /// <summary>Bottom edge resize border.</summary>
        BorderBottom,
        /// <summary>Bottom-left resize corner.</summary>
        BorderBottomLeft,
        /// <summary>Left edge resize border.</summary>
        BorderLeft
    }
}
```

### Abstract/HotReload.cs

```csharp
using Xui.Core.Actual;

[assembly: System.Reflection.Metadata.MetadataUpdateHandler(typeof(Xui.Core.Abstract.HotReload))]
namespace Xui.Core.Abstract;
/// <summary>
/// Internal integration point for .NET's MetadataUpdateHandler, enabling partial support
/// for Hot Reload during development. This is not a stable API and should not be used
/// by application developers.
/// </summary>
/// <remarks>
/// This type is invoked automatically by the runtime when types are updated via
/// Edit and Continue or Hot Reload. However, Hot Reload support in Xui is limited,
/// and application developers should rely on full rebuilds for consistent behavior.
///
/// The current implementation simply invalidates open windows and clears transient caches,
/// without attempting to rehydrate or diff application state.
/// </remarks>
public static class HotReload
{
    /// <summary>
    /// Called by the runtime to clear any cached data after a hot reload.
    /// Not intended for use by application developers.
    /// </summary>
    /// <param name = "updatedTypes">The list of updated types, if available.</param>
    public static void ClearCache(Type[]? updatedTypes)
    {
    }

    /// <summary>
    /// Posts a request to the main UI dispatcher to refresh application state.
    /// </summary>
    /// <param name = "updatedTypes">The list of updated types, if available.</param>
    public static void UpdateApplication(Type[]? updatedTypes)
    {
    }

    /// <summary>
    /// Performs a synchronous application update on the main thread.
    /// This currently forces all open windows to re-render.
    /// </summary>
    /// <param name = "updatedTypes">The list of updated types, if available.</param>
    public static void MainThreadUpdateApplication(Type[]? updatedTypes)
    {
    }
}
```

### Abstract/IWindow.cs

```csharp
using Xui.Core.Abstract.Events;
using Xui.Core.Math2D;

namespace Xui.Core.Abstract;
/// <summary>
/// Defines the abstract interface for a platform window in Xui.
/// This surface hosts rendering, input handling, and layout updates.
/// </summary>
/// <remarks>
/// Implementations of this interface bridge platform-specific <c>Actual</c> windowing
/// with the framework’s abstract layer. It is used both for physical windows (e.g. desktop apps)
/// and virtual windows (e.g. emulator windows).
/// </remarks>
public partial interface IWindow
{
    /// <summary>
    /// Gets or sets the total visible area of the window, including content that may
    /// be obscured by hardware cutouts, rounded corners, or system UI overlays.
    /// </summary>
    /// <remarks>
    /// Used by the layout system to determine the full available size.
    /// </remarks>
    public Rect DisplayArea { get; set; }
    /// <summary>
    /// Gets or sets the "safe" area of the window, excluding obstructions like notches
    /// or status bars. Important UI elements should be placed within this area.
    /// </summary>
    /// <remarks>
    /// Especially relevant on mobile devices and in emulator scenarios.
    /// </remarks>
    public Rect SafeArea { get; set; }

    /// <summary>
    /// Invoked when the window is closed and cleanup should occur.
    /// </summary>
    void Closed();
    /// <summary>
    /// Invoked before the window closes. Returning <c>false</c> can cancel the closure.
    /// </summary>
    /// <returns><c>true</c> if the window may close; otherwise, <c>false</c>.</returns>
    bool Closing();
    /// <summary>
    /// Invoked once per frame to propagate animation timing information.
    /// </summary>
    /// <param name = "animationFrame">Timing details for the current animation frame.</param>
    void OnAnimationFrame(ref FrameEventRef animationFrame);
    /// <summary>
    /// Invoked when the mouse is moved within the window.
    /// </summary>
    /// <param name = "evRef">The mouse movement event data.</param>
    void OnMouseMove(ref MouseMoveEventRef evRef);
    /// <summary>
    /// Invoked when a mouse button is pressed within the window.
    /// </summary>
    /// <param name = "evRef">The mouse down event data.</param>
    void OnMouseDown(ref MouseDownEventRef evRef);
    /// <summary>
    /// Invoked when a mouse button is released within the window.
    /// </summary>
    /// <param name = "evRef">The mouse up event data.</param>
    void OnMouseUp(ref MouseUpEventRef evRef);
    /// <summary>
    /// Invoked when the scroll wheel is used within the window.
    /// </summary>
    /// <param name = "evRef">The scroll wheel event data.</param>
    void OnScrollWheel(ref ScrollWheelEventRef evRef);
    /// <summary>
    /// Invoked when touch input occurs within the window.
    /// </summary>
    /// <param name = "touchEventRef">The touch event data.</param>
    void OnTouch(ref TouchEventRef touchEventRef);
    /// <summary>
    /// Invoked during the render phase of the UI lifecycle.
    /// Responsible for triggering layout and visual updates.
    /// </summary>
    /// <param name = "render">The render event data, including target rect and frame info.</param>
    void Render(ref RenderEventRef render);
    /// <summary>
    /// Invoked when the system requests a hit test for window interaction.
    /// Allows the app to indicate whether a region is draggable, resizable, etc.
    /// </summary>
    /// <param name = "evRef">The hit test event containing pointer position and window bounds.</param>
    void WindowHitTest(ref WindowHitTestEventRef evRef);
}
```

### Abstract/IWindow.IDesktopStyle.cs

```csharp
using Xui.Core.Math2D;

namespace Xui.Core.Abstract;
public partial interface IWindow
{
    /// <summary>
    /// Provides optional desktop-specific window styling hints for platforms
    /// that support multiple top-level windows (e.g., Windows, macOS, Linux).
    /// </summary>
    /// <remarks>
    /// On mobile platforms, applications typically run in a single full-screen window,
    /// and this interface has no effect. On desktop, implementing this interface allows
    /// apps to influence window chrome, border visibility, and initial sizing.
    /// </remarks>
    public interface IDesktopStyle
    {
        /// <summary>
        /// If <c>true</c>, the window will be created without a system title bar or border.
        /// The entire surface will be treated as a client area, while still maintaining
        /// standard desktop window behaviors (e.g., close/minimize buttons).
        /// </summary>
        public bool Chromeless
        {
            get
            {
            }
        }

        /// <summary>
        /// Optional startup size hint for the window.
        /// If <c>null</c>, the platform will decide the initial size.
        /// </summary>
        public Size? StartupSize
        {
            get
            {
            }
        }
    }
}
```

### Abstract/IWindow.ISoftKeyboard.cs

```csharp
using Xui.Core.Abstract.Events;
using Xui.Core.Math2D;

namespace Xui.Core.Abstract;
public partial interface IWindow
{
    /// <summary>
    /// Represents a handler for software keyboard input.
    /// </summary>
    /// <remarks>
    /// This interface allows focused views to receive input from the system's software keyboard,
    /// typically used on mobile platforms and the emulator. It handles character insertion
    /// and backward deletion commands.
    ///
    /// This is not meant to expose full IME or keyboard layout logic—just basic input dispatch.
    /// </remarks>
    public interface ISoftKeyboard
    {
        /// <summary>
        /// Requests insertion of one or more characters into the current input context.
        /// </summary>
        /// <param name = "eventRef">The input event containing the text to insert.</param>
        public void InsertText(ref InsertTextEventRef eventRef);
        /// <summary>
        /// Requests deletion of content preceding the caret or selection.
        /// </summary>
        /// <param name = "eventRef">The input event representing a backspace action.</param>
        public void DeleteBackwards(ref DeleteBackwardsEventRef eventRef);
    }
}
```

### Abstract/Window.cs

```csharp
using System.Collections.ObjectModel;
using Xui.Core.Abstract.Events;
using Xui.Core.Actual;
using Xui.Core.Math2D;
using Xui.Core.UI;

namespace Xui.Core.Abstract;
/// <summary>
/// Represents an abstract cross-platform application window in Xui.
/// Handles input, rendering, layout, and software keyboard integration.
/// </summary>
/// <remarks>
/// This class connects the abstract UI framework with the underlying platform window,
/// acting as a root container for layout and visual composition. Subclasses may override
/// specific input or rendering behaviors as needed.
/// </remarks>
public abstract class Window : Abstract.IWindow, Abstract.IWindow.ISoftKeyboard
{
    private static IList<Window> openWindows = new List<Window>();
    /// <summary>
    /// Gets a read-only list of all currently open Xui windows.
    /// </summary>
    public static IReadOnlyList<Window> OpenWindows = new ReadOnlyCollection<Window>(openWindows);
    /// <summary>
    /// Gets the underlying platform-specific window instance.
    /// </summary>
    public Actual.IWindow Actual { get; }
    /// <inheritdoc/>
    public virtual Rect DisplayArea { get; set; }
    /// <inheritdoc/>
    public virtual Rect SafeArea { get; set; }
    /// <summary>
    /// The root view of the window's content hierarchy.
    /// </summary>
    public virtual View? Content { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref = "Window"/> class.
    /// This creates the backing platform window.
    /// </summary>
    public Window()
    {
    }

    /// <summary>
    /// Gets or sets the window title (where supported by the platform).
    /// </summary>
    public string Title
    {
        get
        {
        }

        set
        {
        }
    }

    /// <summary>
    /// Requests that the soft keyboard be shown or hidden (on supported platforms).
    /// </summary>
    public bool RequireKeyboard
    {
        get
        {
        }

        set
        {
        }
    }

    /// <summary>
    /// Makes the window visible and adds it to the list of open windows.
    /// </summary>
    public void Show()
    {
    }

    /// <inheritdoc/>
    public virtual void Render(ref RenderEventRef renderEventRef)
    {
    }

    /// <inheritdoc/>
    public virtual void WindowHitTest(ref WindowHitTestEventRef evRef)
    {
    }

    /// <inheritdoc/>
    public virtual bool Closing()
    {
    }

    /// <inheritdoc/>
    public virtual void Closed()
    {
    }

    /// <summary>
    /// Creates the platform-specific window for this abstract window.
    /// </summary>
    /// <returns>The platform implementation of <see cref = "Actual.IWindow"/>.</returns>
    protected virtual Actual.IWindow CreateActualWindow()
    {
    }

    /// <summary>
    /// Requests a visual invalidation/redraw of this window.
    /// </summary>
    public virtual void Invalidate()
    {
    }

    /// <inheritdoc/>
    public virtual void OnMouseDown(ref MouseDownEventRef e)
    {
    }

    /// <inheritdoc/>
    public virtual void OnMouseMove(ref MouseMoveEventRef e)
    {
    }

    /// <inheritdoc/>
    public virtual void OnMouseUp(ref MouseUpEventRef e)
    {
    }

    /// <inheritdoc/>
    public virtual void OnScrollWheel(ref ScrollWheelEventRef e)
    {
    }

    /// <inheritdoc/>
    public virtual void OnTouch(ref TouchEventRef e)
    {
    }

    /// <inheritdoc/>
    public virtual void OnAnimationFrame(ref FrameEventRef e)
    {
    }

    /// <inheritdoc/>
    public virtual void InsertText(ref InsertTextEventRef eventRef)
    {
    }

    /// <inheritdoc/>
    public virtual void DeleteBackwards(ref DeleteBackwardsEventRef eventRef)
    {
    }
}
```

### Actual/IDispatcher.cs

```csharp
namespace Xui.Core.Actual;
/// <summary>
/// Represents a platform-specific dispatcher for marshaling callbacks onto the main UI thread.
/// Used by the Xui runtime to ensure thread-safe execution of UI logic.
///
/// Each platform must provide an implementation that posts callbacks to the appropriate runloop or UI thread.
/// </summary>
public interface IDispatcher
{
    /// <summary>
    /// Posts the specified callback to be executed on the dispatcher's thread (typically the UI thread).
    /// Use this method when calling from a background thread and needing to safely transition
    /// to the main thread for UI updates or layout work.
    /// </summary>
    /// <param name = "callback">The action to execute on the dispatcher's thread.</param>
    void Post(Action callback);
}
```

### Actual/IRunLoop.cs

```csharp
namespace Xui.Core.Actual;
/// <summary>
/// Represents a platform-specific run loop responsible for driving the application's lifecycle.
/// Each platform must provide an implementation that enters the appropriate system event loop
/// and continues running until the application exits.
///
/// The Xui runtime uses this interface to abstract over platform differences in event dispatch and app execution.
/// </summary>
public interface IRunLoop
{
    /// <summary>
    /// Starts the main run loop for the application.
    /// This method may block until the application terminates or exits naturally.
    /// On platforms with built-in UI event loops (e.g., iOS, Android),
    /// this method may return immediately after bootstrapping the application delegate.
    /// </summary>
    /// <returns>The application’s exit code.</returns>
    int Run();
}
```

### Actual/IRuntime.cs

```csharp
using Xui.Core.Canvas;

namespace Xui.Core.Actual;
/// <summary>
/// Provides a platform-specific implementation of the Xui runtime,
/// responsible for creating and connecting abstract application components to their actual counterparts.
///
/// This interface acts as a bridge between the platform-independent core and the underlying OS-specific APIs
/// (e.g., Win32, Cocoa, UIKit), enabling rendering, windowing, and event dispatch.
/// </summary>
public interface IRuntime
{
    /// <summary>
    /// Gets the global drawing context for the current platform.
    /// This typically wraps a native graphics context such as Direct2D (Windows) or CGContext (macOS),
    /// and serves as the entry point for rendering operations.
    /// </summary>
    IContext DrawingContext { get; }

    /// <summary>
    /// Gets the main thread dispatcher for scheduling UI work.
    /// Used to marshal execution onto the main thread for layout, input, and rendering.
    /// </summary>
    IDispatcher MainDispatcher { get; }

    /// <summary>
    /// Creates a platform-specific window that is bound to the given abstract window definition.
    /// </summary>
    /// <param name = "windowAbstract">The abstract window definition provided by user code.</param>
    /// <returns>A concrete window implementation for the current platform.</returns>
    Actual.IWindow CreateWindow(Abstract.IWindow windowAbstract);
    /// <summary>
    /// Creates a platform-specific run loop associated with the given abstract application.
    /// The returned run loop is responsible for managing the application's execution lifecycle.
    /// </summary>
    /// <param name = "applicationAbstract">The abstract application instance defined by user code.</param>
    /// <returns>A platform-specific run loop instance.</returns>
    IRunLoop CreateRunloop(Abstract.Application applicationAbstract);
}
```

### Actual/IWindow.cs

```csharp
namespace Xui.Core.Actual;
/// <summary>
/// Represents a platform-specific window implementation used by the Xui runtime.
/// Each platform (e.g., Windows, macOS, iOS) must provide an implementation of this interface
/// to manage window lifecycle, rendering, and input.
///
/// This interface is typically paired with an abstract window in the Xui framework,
/// and is not used directly by application developers.
/// </summary>
public interface IWindow
{
    /// <summary>
    /// Gets or sets the window title, where supported by the platform (e.g., desktop).
    /// </summary>
    string Title { get; set; }

    /// <summary>
    /// Displays the window to the user. This may include making it visible, entering the main loop,
    /// or attaching it to the application’s view hierarchy, depending on the platform.
    /// </summary>
    void Show();
    /// <summary>
    /// Requests a redraw of the window surface.
    /// The platform should trigger a paint or render callback as soon as possible.
    /// </summary>
    void Invalidate();
    /// <summary>
    /// Gets or sets whether the window currently requires keyboard input focus.
    /// Platforms may use this to show or hide on-screen keyboards.
    /// </summary>
    bool RequireKeyboard { get; set; }
}
```

### Actual/Runtime.cs

```csharp
using Xui.Core.Actual;
using Xui.Core.Canvas;

namespace Xui.Core.Actual;
/// <summary>
/// Provides global access to the platform-specific runtime environment for the current Xui application.
/// 
/// The platform must assign <see cref = "Current"/> at startup with a concrete <see cref = "IRuntime"/> implementation,
/// which wires up platform-specific services like rendering, window creation, and dispatching.
/// </summary>
public static class Runtime
{
    private static IRuntime? current;
    /// <summary>
    /// Gets or sets the current platform runtime instance.
    /// This must be initialized at application startup by platform bootstrap code.
    /// </summary>
    /// <exception cref = "RuntimeNotAvailable">
    /// Thrown if accessed before the runtime has been initialized.
    /// </exception>
    public static IRuntime Current
    {
        get
        {
        }

        set
        {
        }
    }

    /// <summary>
    /// Gets the global drawing context provided by the current platform runtime, if available.
    /// </summary>
    public static IContext? DrawingContext
    {
        get
        {
        }
    }

    /// <summary>
    /// Gets the main dispatcher for scheduling UI work on the platform's main thread, if available.
    /// </summary>
    public static IDispatcher? MainDispatcher
    {
        get
        {
        }
    }

    /// <summary>
    /// Exception thrown when <see cref = "Current"/> is accessed before it has been initialized.
    /// </summary>
    public class RuntimeNotAvailable : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref = "RuntimeNotAvailable"/> exception
        /// with a helpful diagnostic message.
        /// </summary>
        public RuntimeNotAvailable() : base("First thing to do in your app is set Runtime.Current!\nSomehow the execution reached a Runtime.Current call before a runtime was set.")
        {
        }
    }
}
```

### Animation/ConstantDecelerationCurve.cs

```csharp
namespace Xui.Core.Animation;
/// <summary>
/// Represents a 1D motion curve with constant deceleration (e.g., for scroll or fling).
/// Starts at a given position and velocity, then decelerates linearly to a stop.
/// </summary>
/// <remarks>
/// This model is commonly used in UI frameworks to simulate natural-feeling scroll behavior.
/// The acceleration is constant and opposite to the direction of motion, resulting in
/// a predictable, smooth slowdown.
/// </remarks>
public readonly struct ConstantDecelerationCurve
{
    /// <summary>
    /// The time when the motion begins.
    /// </summary>
    public readonly TimeSpan StartTime;
    /// <summary>
    /// The time when the motion stops (velocity reaches zero).
    /// </summary>
    public readonly TimeSpan EndTime;
    /// <summary>
    /// The constant acceleration applied during motion, typically negative.
    /// </summary>
    public readonly nfloat A;
    /// <summary>
    /// The initial velocity at <see cref = "StartTime"/>, in pixels per second.
    /// </summary>
    public readonly nfloat V0;
    /// <summary>
    /// The initial position at <see cref = "StartTime"/>, in pixels.
    /// </summary>
    public readonly nfloat P0;
    /// <summary>
    /// Constructs a motion curve with constant deceleration.
    /// </summary>
    /// <param name = "startTime">The time when motion begins.</param>
    /// <param name = "position">The initial position at <paramref name = "startTime"/>.</param>
    /// <param name = "velocity">The initial velocity in pixels per second.</param>
    /// <param name = "accelerationMagnitude">The magnitude of deceleration (must be positive).</param>
    /// <exception cref = "ArgumentOutOfRangeException">Thrown if <paramref name = "accelerationMagnitude"/> is not positive.</exception>
    public ConstantDecelerationCurve(TimeSpan startTime, nfloat position, nfloat velocity, nfloat accelerationMagnitude)
    {
    }

    /// <summary>
    /// The total duration of the motion in seconds.
    /// </summary>
    public nfloat DurationSeconds
    {
        get
        {
        }
    }

    /// <summary>
    /// Evaluates the position at the specified time.
    /// </summary>
    /// <param name = "time">The absolute time at which to evaluate the position.</param>
    /// <returns>The position in pixels.</returns>
    public nfloat this[TimeSpan time]
    {
        [DebuggerStepThrough]
        get
        {
        }
    }

    /// <summary>
    /// Evaluates the velocity at the specified time.
    /// </summary>
    /// <param name = "time">The absolute time at which to evaluate the velocity.</param>
    /// <returns>The velocity in pixels per second.</returns>
    public nfloat VelocityAt(TimeSpan time)
    {
    }

    private nfloat ClampTime(TimeSpan time)
    {
    }
}
```

### Animation/CubicMotionCurve.cs

```csharp
namespace Xui.Core.Animation;
/// <summary>
/// Represents a 1D cubic motion curve over time, interpolating both position and velocity.
/// Time is expressed using <see cref = "TimeSpan"/>, and calculations are performed in seconds.
/// </summary>
/// <remarks>
/// This curve is defined by a cubic polynomial:
/// <c>f(t) = A·t³ + B·t² + C·t + D</c>, where <c>t</c> is time in seconds since the global epoch.
/// It supports generation from boundary conditions (position and velocity at both ends),
/// as well as curve continuation with seamless velocity transitions.
/// </remarks>
public readonly struct CubicMotionCurve
{
    /// <summary>
    /// The time at which the motion curve starts.
    /// </summary>
    public readonly TimeSpan StartTime;
    /// <summary>
    /// The time at which the motion curve ends.
    /// </summary>
    public readonly TimeSpan EndTime;
    /// <summary>
    /// The cubic coefficient (multiplied by t³).
    /// </summary>
    public readonly nfloat A;
    /// <summary>
    /// The quadratic coefficient (multiplied by t²).
    /// </summary>
    public readonly nfloat B;
    /// <summary>
    /// The linear coefficient (multiplied by t).
    /// </summary>
    public readonly nfloat C;
    /// <summary>
    /// The constant offset term.
    /// </summary>
    public readonly nfloat D;
    /// <summary>
    /// Initializes a new cubic motion curve with the given polynomial coefficients and time range.
    /// </summary>
    public CubicMotionCurve(TimeSpan startTime, TimeSpan endTime, nfloat a, nfloat b, nfloat c, nfloat d)
    {
    }

    /// <summary>
    /// The final position at <see cref = "EndTime"/>.
    /// </summary>
    public nfloat FinalPosition
    {
        get
        {
        }
    }

    /// <summary>
    /// The duration of the motion curve, in seconds.
    /// </summary>
    public nfloat DurationSeconds
    {
        get
        {
        }
    }

    /// <summary>
    /// Continues the current curve by constructing a new cubic motion curve that
    /// begins where this one leaves off, matching position and velocity, and
    /// interpolating to a new position and velocity over the given time range.
    /// </summary>
    public CubicMotionCurve ContinueWithBoundaryConditions(TimeSpan startTime, TimeSpan endTime, nfloat endPosition, nfloat endVelocity)
    {
    }

    /// <summary>
    /// Constructs a cubic motion curve that interpolates position and velocity
    /// between two points in time using Hermite interpolation.
    /// </summary>
    /// <param name = "startTime">The curve's start time.</param>
    /// <param name = "startPosition">The position at <paramref name = "startTime"/>.</param>
    /// <param name = "startVelocity">The velocity at <paramref name = "startTime"/>.</param>
    /// <param name = "endTime">The curve's end time.</param>
    /// <param name = "endPosition">The position at <paramref name = "endTime"/>.</param>
    /// <param name = "endVelocity">The velocity at <paramref name = "endTime"/>.</param>
    /// <returns>A cubic motion curve satisfying the specified boundary conditions.</returns>
    public static CubicMotionCurve FromBoundaryConditions(TimeSpan startTime, nfloat startPosition, nfloat startVelocity, TimeSpan endTime, nfloat endPosition, nfloat endVelocity)
    {
    }

    /// <summary>
    /// Evaluates the position at the specified time.
    /// </summary>
    /// <param name = "time">An absolute time value.</param>
    /// <returns>The position at the given time.</returns>
    public nfloat this[TimeSpan time]
    {
        [DebuggerStepThrough]
        get
        {
        }
    }

    /// <summary>
    /// Evaluates the velocity at the specified time.
    /// </summary>
    /// <param name = "time">An absolute time value.</param>
    /// <returns>The velocity at the given time.</returns>
    public nfloat VelocityAt(TimeSpan time)
    {
    }
}
```

### Animation/Easing.cs

```csharp
namespace Xui.Core.Animation;
/// <summary>
/// Provides a collection of easing functions and smoothing utilities used in animations.
/// All easing methods are normalized to take a parameter <c>t</c> in the range [0, 1].
/// </summary>
public static partial class Easing
{
    private static readonly nfloat c1 = 1.70158f;
    private static readonly nfloat c2 = 1.70158f * 1.525f;
    private static readonly nfloat c4 = (nfloat)(2 * Math.PI) / 3;
    private static readonly nfloat c5 = (nfloat)(2 * Math.PI) / (nfloat)4.5;
    private static readonly nfloat n1 = 7.5625f;
    private static readonly nfloat d1 = 2.75f;
    /// <summary>
    /// Normalizes a value between <paramref name = "min"/> and <paramref name = "max"/> into a [0,1] range.
    /// Returns 0 if <paramref name = "max"/> ≤ <paramref name = "min"/>.
    /// </summary>
    public static nfloat Normalize(nfloat value, nfloat min, nfloat max)
    {
    }

    /// <summary>
    /// Eases in and out with a sine curve. Smoothest at beginning and end.
    /// </summary>
    public static nfloat EaseInOutSine(nfloat t)
    {
    }

    /// <summary>
    /// Accelerates slowly from 0 to 1 with a quadratic curve.
    /// </summary>
    public static nfloat EaseInQuad(nfloat t)
    {
    }

    /// <summary>
    /// Decelerates smoothly from 1 to 0 with a quadratic curve.
    /// </summary>
    public static nfloat EaseOutQuad(nfloat t)
    {
    }

    /// <summary>
    /// Accelerates and decelerates using a quadratic curve.
    /// </summary>
    public static nfloat EaseInOutQuad(nfloat t)
    {
    }

    /// <summary>
    /// Accelerates slowly with a cubic curve.
    /// </summary>
    public static nfloat EaseInCubic(nfloat t)
    {
    }

    /// <summary>
    /// Decelerates slowly with a cubic curve.
    /// </summary>
    public static nfloat EaseOutCubic(nfloat t)
    {
    }

    /// <summary>
    /// Smoothly accelerates and decelerates with a cubic curve.
    /// </summary>
    public static nfloat EaseInOutCubic(nfloat t)
    {
    }

    /// <summary>
    /// Stronger acceleration from 0 using a quartic curve.
    /// </summary>
    public static nfloat EaseInQuart(nfloat t)
    {
    }

    /// <summary>
    /// Stronger deceleration to 1 using a quartic curve.
    /// </summary>
    public static nfloat EaseOutQuart(nfloat t)
    {
    }

    /// <summary>
    /// Strong acceleration and deceleration using a quartic curve.
    /// </summary>
    public static nfloat EaseInOutQuart(nfloat t)
    {
    }

    /// <summary>
    /// Creates an elastic "bounce-in" effect at the beginning of the transition.
    /// </summary>
    public static nfloat EaseInElastic(nfloat t)
    {
    }

    /// <summary>
    /// Creates an elastic "bounce-out" effect at the end of the transition.
    /// </summary>
    public static nfloat EaseOutElastic(nfloat t)
    {
    }

    /// <summary>
    /// Combines EaseInElastic and EaseOutElastic for a spring-like bounce effect at both ends.
    /// </summary>
    public static nfloat EaseInOutElastic(nfloat t)
    {
    }

    /// <summary>
    /// Eases in with a "backwards" overshoot effect before settling forward.
    /// </summary>
    public static nfloat EaseInBack(nfloat t)
    {
    }

    /// <summary>
    /// Eases out with a slight overshoot after reaching the target.
    /// </summary>
    public static nfloat EaseOutBack(nfloat t)
    {
    }

    /// <summary>
    /// Eases in and out with a "backwards" overshoot at both ends.
    /// </summary>
    public static nfloat EaseInOutBack(nfloat t)
    {
    }

    /// <summary>
    /// Creates a bounce effect as if hitting the ground and bouncing back.
    /// </summary>
    public static nfloat EaseOutBounce(nfloat t)
    {
    }

    /// <summary>
    /// Reverses EaseOutBounce to ease in with bounce.
    /// </summary>
    public static nfloat EaseInBounce(nfloat t)
    {
    }

    /// <summary>
    /// Eases in and out with a bounce at both ends.
    /// </summary>
    public static nfloat EaseInOutBounce(nfloat t)
    {
    }

    /// <summary>
    /// Applies a smoothstep-like easing function that produces smooth transitions with zero first and second derivatives at boundaries.
    /// </summary>
    public static nfloat SmootherStep(nfloat t)
    {
    }

    /// <summary>
    /// Smoothly interpolates between two values using a critically damped spring-like function.
    /// </summary>
    /// <param name = "from">The starting value.</param>
    /// <param name = "to">The target value.</param>
    /// <param name = "velocity">Reference to current velocity (will be modified).</param>
    /// <param name = "smoothTime">Time it takes to reach the target value.</param>
    /// <param name = "maxSpeed">Maximum speed during interpolation.</param>
    /// <param name = "deltaTime">Elapsed time since the last update.</param>
    public static nfloat SmoothDamp(nfloat from, nfloat to, ref nfloat velocity, nfloat smoothTime, nfloat maxSpeed, nfloat deltaTime)
    {
    }
}
```

### Animation/Easing.CubicBezier.cs

```csharp
using Xui.Core.Math2D;

namespace Xui.Core.Animation;
public static partial class Easing
{
    /// <summary>
    /// Represents a CSS-style cubic Bézier easing curve constrained between (0,0) and (1,1).
    /// </summary>
    /// <remarks>
    /// These curves are used for timing animations and transitions.
    /// The curve always starts at (0,0) and ends at (1,1), and the control points <see cref = "P1"/> and <see cref = "P2"/>
    /// shape the acceleration and deceleration of the animation.
    /// </remarks>
    public readonly struct CubicBezier
    {
        /// <summary>The first control point (typically near the origin).</summary>
        public readonly Point P1;
        /// <summary>The second control point (typically near the destination).</summary>
        public readonly Point P2;
        /// <summary>
        /// Initializes a new CSS-style cubic Bézier easing curve with the given control points.
        /// Assumes endpoints at (0,0) and (1,1).
        /// </summary>
        public CubicBezier(Point p1, Point p2)
        {
        }

        /// <summary>
        /// Returns the interpolated point on the Bézier curve at a given <paramref name = "t"/> ∈ [0, 1].
        /// </summary>
        public Point Lerp(nfloat t)
        {
        }

        /// <summary>
        /// Approximates the Y output value for a given input X ∈ [0,1] using a 16-step lookup table.
        /// </summary>
        public nfloat Evaluate(nfloat x)
        {
        }

        /// <summary>
        /// Approximates the Y output value for a given input X ∈ [0,1] using binary search to the given precision.
        /// </summary>
        public nfloat Evaluate(nfloat x, nfloat precision)
        {
        }

        /// <summary>
        /// Indexer alias for <see cref = "Evaluate(nfloat)"/>.
        /// </summary>
        public nfloat this[nfloat x]
        {
            get
            {
            }
        }

        private nfloat EvaluateInternal(nfloat xTarget, int? steps = null, nfloat? precision = null)
        {
        }

        /// <summary>
        /// Finds the parameter <c>t</c> ∈ [0, 1] where the curve is closest to the given <paramref name = "target"/> point.
        /// Uses 16-step approximation.
        /// </summary>
        public nfloat ClosestT(Point target)
        {
        }

        /// <summary>
        /// Returns a new cubic Bézier easing curve with a deformation applied near the closest point to <paramref name = "origin"/>, 
        /// shifted by <paramref name = "delta"/>.
        /// </summary>
        public CubicBezier Drag(Point origin, Vector delta)
        {
        }

        /// <summary>
        /// Returns a new cubic Bézier easing curve with a deformation applied at the given parameter <paramref name = "t"/> by <paramref name = "delta"/>.
        /// </summary>
        public CubicBezier DragAt(nfloat t, Vector delta)
        {
        }

        /// <summary>
        /// Converts a full <see cref = "Curves2D.CubicBezier"/> into a CSS-style easing curve.
        /// The curve must begin at (0,0) and end at (1,1).
        /// </summary>
        public static implicit operator CubicBezier(Curves2D.CubicBezier bezier)
        {
        }

        /// <summary>Equivalent to CSS `ease`: cubic-bezier(0.25, 0.1, 0.25, 1.0)</summary>
        public static CubicBezier Ease
        {
            get
            {
            }
        }

        /// <summary>Equivalent to CSS `ease-in`: cubic-bezier(0.42, 0, 1.0, 1.0)</summary>
        public static CubicBezier EaseIn
        {
            get
            {
            }
        }

        /// <summary>Equivalent to CSS `ease-out`: cubic-bezier(0, 0, 0.58, 1.0)</summary>
        public static CubicBezier EaseOut
        {
            get
            {
            }
        }

        /// <summary>Equivalent to CSS `ease-in-out`: cubic-bezier(0.42, 0, 0.58, 1.0)</summary>
        public static CubicBezier EaseInOut
        {
            get
            {
            }
        }
    }
}
```

### Animation/Easing.PolynomialEasing.cs

```csharp
namespace Xui.Core.Animation;
public static partial class Easing
{
    /// <summary>
    /// Represents a 3rd- or 4th-degree polynomial easing function for animation timing curves.
    /// </summary>
    /// <remarks>
    /// This type can be used to approximate Bézier-based easing curves (like <see cref = "Easing.CubicBezier"/>)
    /// using polynomial coefficients, which are faster to evaluate and easier to store or serialize.
    /// </remarks>
    public readonly struct PolynomialEasing
    {
        /// <summary>The coefficient for x⁴ (A = 0 for cubic curves).</summary>
        public readonly nfloat A;
        /// <summary>The coefficient for x³.</summary>
        public readonly nfloat B;
        /// <summary>The coefficient for x².</summary>
        public readonly nfloat C;
        /// <summary>The coefficient for x¹.</summary>
        public readonly nfloat D;
        /// <summary>The constant term.</summary>
        public readonly nfloat E;
        /// <summary>
        /// Constructs a 3rd-degree polynomial: y = B·x³ + C·x² + D·x + E
        /// </summary>
        public PolynomialEasing(nfloat b, nfloat c, nfloat d, nfloat e)
        {
        }

        /// <summary>
        /// Constructs a 4th-degree polynomial: y = A·x⁴ + B·x³ + C·x² + D·x + E
        /// </summary>
        public PolynomialEasing(nfloat a, nfloat b, nfloat c, nfloat d, nfloat e)
        {
        }

        /// <summary>
        /// Approximates a <see cref = "Easing.CubicBezier"/> easing curve using a cubic polynomial.
        /// The result fits y = B·x³ + C·x² + D·x + E over the domain x ∈ [0, 1].
        /// </summary>
        /// <param name = "bezier">The Bézier curve to approximate.</param>
        /// <param name = "samples">Number of sample points to use in least-squares fitting. Default is 16.</param>
        public PolynomialEasing(CubicBezier bezier, int samples = 16)
        {
        }

        /// <summary>
        /// Evaluates the polynomial at a given value of x ∈ [0, 1].
        /// </summary>
        /// <param name = "x">The input value (typically time).</param>
        /// <returns>The eased value at x.</returns>
        public nfloat this[nfloat x]
        {
            [DebuggerStepThrough]
            get
            {
            }
        }

        /// <summary>
        /// Solves a 4x4 linear system using Gaussian elimination.
        /// </summary>
        /// <param name = "m">The coefficient matrix (flattened row-major).</param>
        /// <param name = "b">The right-hand side vector.</param>
        /// <param name = "result">The resulting vector of solved coefficients.</param>
        private static void SolveGaussian(Span<nfloat> m, Span<nfloat> b, Span<nfloat> result)
        {
        }
    }
}
```

### Animation/ExponentialDecayCurve.cs

```csharp
using System;
using System.Diagnostics;

namespace Xui.Core.Animation;
/// <summary>
/// Represents a motion curve where velocity decays exponentially over time.
/// Commonly used to simulate fling or momentum-based motion with smooth slowdown.
/// </summary>
public readonly struct ExponentialDecayCurve
{
    /// <summary>
    /// The time at which the motion begins.
    /// </summary>
    public readonly TimeSpan StartTime;
    /// <summary>
    /// The time at which the motion ends, determined by when velocity falls below a defined threshold.
    /// </summary>
    public readonly TimeSpan EndTime;
    /// <summary>
    /// The decay factor per second. Lower values result in faster deceleration.
    /// </summary>
    public readonly nfloat DecayPerSecond;
    /// <summary>
    /// The initial velocity at the start of the curve.
    /// </summary>
    public readonly nfloat InitialVelocity;
    /// <summary>
    /// The starting position of the motion.
    /// </summary>
    public readonly nfloat StartPosition;
    /// <summary>
    /// A typical decay factor (~0.998 per millisecond). Use for normal fling decay.
    /// </summary>
    public static readonly nfloat Normal = (nfloat)0.135;
    /// <summary>
    /// A faster decay factor (~0.99 per millisecond). Use for snappier motion.
    /// </summary>
    public static readonly nfloat Fast = (nfloat)0.00004317;
    /// <summary>
    /// Initializes an exponential decay motion curve.
    /// </summary>
    /// <param name = "startTime">The time when motion begins.</param>
    /// <param name = "startPosition">The initial position.</param>
    /// <param name = "initialVelocity">The initial velocity (can be negative).</param>
    /// <param name = "decayPerSecond">
    /// The decay multiplier per second. A value less than 1, closer to 0 = faster decay.
    /// Recommended value is <see cref = "Normal"/>.
    /// </param>
    /// <param name = "velocityThreshold">
    /// The velocity below which motion is considered stopped (defaults to 0.5).
    /// </param>
    [DebuggerStepThrough]
    public ExponentialDecayCurve(TimeSpan startTime, nfloat startPosition, nfloat initialVelocity, nfloat decayPerSecond, double velocityThreshold = 0.5)
    {
    }

    /// <summary>
    /// Gets the final position of the motion at <see cref = "EndTime"/>.
    /// </summary>
    public nfloat FinalPosition
    {
        get
        {
        }
    }

    /// <summary>
    /// Evaluates the position of the motion at a given time.
    /// </summary>
    /// <param name = "time">The time to evaluate.</param>
    /// <returns>The interpolated position at the specified time.</returns>
    public nfloat this[TimeSpan time]
    {
        [DebuggerStepThrough]
        get
        {
        }
    }

    /// <summary>
    /// Evaluates the velocity of the motion at a given time.
    /// </summary>
    /// <param name = "time">The time to evaluate.</param>
    /// <returns>The current velocity at the specified time.</returns>
    public nfloat VelocityAt(TimeSpan time)
    {
    }
}
```

### Animation/QuadraticMotionCurve.cs

```csharp
namespace Xui.Core.Animation;
/// <summary>
/// Represents a 1D motion curve under constant acceleration, defined as a quadratic function of time.
/// 
/// This curve is useful for modeling motion that decelerates smoothly to a stop,
/// such as fling or scroll-stop animations.
/// </summary>
public readonly struct QuadraticMotionCurve
{
    /// <summary>
    /// The time at which the motion starts.
    /// </summary>
    public readonly TimeSpan StartTime;
    /// <summary>
    /// The time at which the motion ends (i.e., when velocity reaches zero).
    /// </summary>
    public readonly TimeSpan EndTime;
    /// <summary>
    /// The quadratic coefficient (0.5 * acceleration).
    /// </summary>
    public readonly nfloat A;
    /// <summary>
    /// The linear coefficient (initial velocity minus adjustment).
    /// </summary>
    public readonly nfloat B;
    /// <summary>
    /// The constant coefficient (initial position minus offset).
    /// </summary>
    public readonly nfloat C;
    /// <summary>
    /// Initializes the curve with explicit polynomial coefficients and time range.
    /// </summary>
    /// <param name = "startTime">The time when motion starts.</param>
    /// <param name = "endTime">The time when motion ends.</param>
    /// <param name = "a">The quadratic coefficient.</param>
    /// <param name = "b">The linear coefficient.</param>
    /// <param name = "c">The constant offset.</param>
    [DebuggerStepThrough]
    public QuadraticMotionCurve(TimeSpan startTime, TimeSpan endTime, nfloat a, nfloat b, nfloat c)
    {
    }

    /// <summary>
    /// Constructs a curve from initial position, velocity, and acceleration magnitude.
    /// The acceleration is applied in the opposite direction of velocity to decelerate to zero.
    /// </summary>
    /// <param name = "startTime">The time when motion starts.</param>
    /// <param name = "startPosition">Initial position.</param>
    /// <param name = "startVelocity">Initial velocity (positive or negative).</param>
    /// <param name = "accelerationMagnitude">Positive scalar magnitude of deceleration.</param>
    public QuadraticMotionCurve(TimeSpan startTime, nfloat startPosition, nfloat startVelocity, nfloat accelerationMagnitude)
    {
    }

    /// <summary>
    /// Gets the final position of the motion at <see cref = "EndTime"/>.
    /// </summary>
    public nfloat FinalPosition
    {
        get
        {
        }
    }

    /// <summary>
    /// Evaluates the position of the motion at the specified <paramref name = "time"/>.
    /// </summary>
    /// <param name = "time">The time at which to evaluate the position.</param>
    public nfloat this[TimeSpan time]
    {
        [DebuggerStepThrough]
        get
        {
        }
    }

    /// <summary>
    /// Evaluates the instantaneous velocity at the specified <paramref name = "time"/>.
    /// </summary>
    /// <param name = "time">The time at which to evaluate the velocity.</param>
    /// <returns>The velocity in units per second.</returns>
    public nfloat VelocityAt(TimeSpan time)
    {
    }
}
```

### Canvas/Color.cs

```csharp
using System.Runtime.InteropServices;

namespace Xui.Core.Canvas;
/// <summary>
/// Represents a color using red, green, blue, and alpha components, all normalized to the range [0, 1].
/// </summary>
public struct Color
{
    /// <summary>
    /// Red component of the color, in the range [0, 1].
    /// </summary>
    public nfloat Red;
    /// <summary>
    /// Green component of the color, in the range [0, 1].
    /// </summary>
    public nfloat Green;
    /// <summary>
    /// Blue component of the color, in the range [0, 1].
    /// </summary>
    public nfloat Blue;
    /// <summary>
    /// Alpha (opacity) component of the color, in the range [0, 1].
    /// </summary>
    public nfloat Alpha;
    /// <summary>
    /// Initializes a new <see cref = "Color"/> with the specified red, green, blue, and alpha components.
    /// </summary>
    /// <param name = "red">Red component, normalized [0, 1]</param>
    /// <param name = "green">Green component, normalized [0, 1]</param>
    /// <param name = "blue">Blue component, normalized [0, 1]</param>
    /// <param name = "alpha">Alpha component, normalized [0, 1]</param>
    public Color(nfloat red, nfloat green, nfloat blue, nfloat alpha)
    {
    }

    /// <summary>
    /// Initializes a new <see cref = "Color"/> from a packed 32-bit RGBA value.
    /// The value format is 0xRRGGBBAA, where each component is 8 bits.
    /// </summary>
    /// <param name = "rgba">Packed RGBA value in 0xRRGGBBAA format.</param>
    public Color(uint rgba)
    {
    }

    /// <summary>
    /// Returns true if the color is fully transparent (Alpha = 0).
    /// </summary>
    public readonly bool IsTransparent
    {
        get
        {
        }
    }

    /// <summary>
    /// Implicitly converts a 32-bit RGBA value (0xRRGGBBAA) to a <see cref = "Color"/>.
    /// </summary>
    /// <param name = "rgbaHex">Packed RGBA hex value.</param>
    public static implicit operator Color(uint rgbaHex)
    {
    }
}
```

### Canvas/Colors.cs

```csharp
namespace Xui.Core.Canvas;
/// <summary>
/// Provides web standard named color constants in 32-bit RGBA format (0xRRGGBBAA).
/// These match the named colors from the CSS color specification:
/// https://developer.mozilla.org/en-US/docs/Web/CSS/named-color
/// </summary>
public static class Colors
{
#pragma warning disable CS1591
    public const uint Transparent = 0x00000000;
    public const uint Black = 0x000000FF;
    public const uint White = 0xFFFFFFFF;
    public const uint Red = 0xFF0000FF;
    public const uint Lime = 0x00FF00FF;
    public const uint Blue = 0x0000FFFF;
    public const uint Yellow = 0xFFFF00FF;
    public const uint Fuchsia = 0xFF00FFFF;
    public const uint Aqua = 0x00FFFFFF;
    public const uint Silver = 0xC0C0C0FF;
    public const uint Gray = 0x808080FF;
    public const uint Maroon = 0x800000FF;
    public const uint Purple = 0x800080FF;
    public const uint Green = 0x008000FF;
    public const uint Olive = 0x808000FF;
    public const uint Navy = 0x000080FF;
    public const uint Teal = 0x008080FF;
    public const uint AliceBlue = 0xf0f8ffFF;
    public const uint AntiqueWhite = 0xfaebd7FF;
    public const uint Aquamarine = 0x7fffd4FF;
    public const uint Azure = 0xf0ffffFF;
    public const uint Beige = 0xf5f5dcFF;
    public const uint Bisque = 0xffe4c4FF;
    public const uint BlanchedAlmond = 0xffebcdFF;
    public const uint BlueViolet = 0x8a2be2FF;
    public const uint Brown = 0xa52a2aFF;
    public const uint BurlyWood = 0xdeb887FF;
    public const uint CadetBlue = 0x5f9ea0FF;
    public const uint ChartReuse = 0x7fff00FF;
    public const uint Chocolate = 0xd2691eFF;
    public const uint Coral = 0xff7f50FF;
    public const uint CornflowerBlue = 0x6495edFF;
    public const uint CornSilk = 0xfff8dcFF;
    public const uint Crimson = 0xdc143cFF;
    public const uint Cyan = 0x00ffffFF;
    public const uint DarkBlue = 0x00008bFF;
    public const uint DarkCyan = 0x008b8bFF;
    public const uint DarkGoldenRod = 0xb8860bFF;
    public const uint DarkGray = 0xa9a9a9FF;
    public const uint DarkGreen = 0x006400FF;
    public const uint DarkGrey = 0xa9a9a9FF;
    public const uint DarkKhaki = 0xbdb76bFF;
    public const uint DarkMagenta = 0x8b008bFF;
    public const uint DarkOliveGreen = 0x556b2fFF;
    public const uint DarkOrange = 0xff8c00FF;
    public const uint DarkOrchid = 0x9932ccFF;
    public const uint DarkRed = 0x8b0000FF;
    public const uint DarkSalmon = 0xe9967aFF;
    public const uint DarkSeaGreen = 0x8fbc8fFF;
    public const uint DarkSlateBlue = 0x483d8bFF;
    public const uint DarkSlateGray = 0x2f4f4fFF;
    public const uint DarkSlateGrey = 0x2f4f4fFF;
    public const uint DarkTurquoise = 0x00ced1FF;
    public const uint DarkViolet = 0x9400d3FF;
    public const uint DeepPink = 0xff1493FF;
    public const uint DeepSkyBlue = 0x00bfffFF;
    public const uint DimGray = 0x696969FF;
    public const uint DimGrey = 0x696969FF;
    public const uint DodgerBlue = 0x1e90ffFF;
    public const uint FireBrick = 0xb22222FF;
    public const uint FloralWhite = 0xfffaf0FF;
    public const uint ForestGreen = 0x228b22FF;
    public const uint Gainsboro = 0xdcdcdcFF;
    public const uint GhostWhite = 0xf8f8ffFF;
    public const uint Gold = 0xffd700FF;
    public const uint GoldenRod = 0xdaa520FF;
    public const uint GreenYellow = 0xadff2fFF;
    public const uint Grey = 0x808080FF;
    public const uint Honeydew = 0xf0fff0FF;
    public const uint HotPink = 0xff69b4FF;
    public const uint IndianRed = 0xcd5c5cFF;
    public const uint Indigo = 0x4b0082FF;
    public const uint Ivory = 0xfffff0FF;
    public const uint Khaki = 0xf0e68cFF;
    public const uint Lavender = 0xe6e6faFF;
    public const uint LavenderBlush = 0xfff0f5FF;
    public const uint LawnGreen = 0x7cfc00FF;
    public const uint LemonChiffon = 0xfffacdFF;
    public const uint LightBlue = 0xadd8e6FF;
    public const uint LightCoral = 0xf08080FF;
    public const uint LightCyan = 0xe0ffffFF;
    public const uint LightGoldenRodYellow = 0xfafad2FF;
    public const uint LightGray = 0xd3d3d3FF;
    public const uint LightGreen = 0x90ee90FF;
    public const uint LightGrey = 0xd3d3d3FF;
    public const uint LightPink = 0xffb6c1FF;
    public const uint LightSalmon = 0xffa07aFF;
    public const uint LightSeaGreen = 0x20b2aaFF;
    public const uint LightSkyBlue = 0x87cefaFF;
    public const uint LightSlateGray = 0x778899FF;
    public const uint LightSlateGrey = 0x778899FF;
    public const uint LightSteelBlue = 0xb0c4deFF;
    public const uint LightYellow = 0xffffe0FF;
    public const uint LimeGreen = 0x32cd32FF;
    public const uint Linen = 0xfaf0e6FF;
    public const uint Magenta = 0xff00ffFF;
    public const uint MediumAquamarine = 0x66cdaaFF;
    public const uint MediumBlue = 0x0000cdFF;
    public const uint MediumOrchid = 0xba55d3FF;
    public const uint MediumPurple = 0x9370dbFF;
    public const uint MediumSeaGreen = 0x3cb371FF;
    public const uint MediumSlateBlue = 0x7b68eeFF;
    public const uint MediumSpringGreen = 0x00fa9aFF;
    public const uint MediumTurquoise = 0x48d1ccFF;
    public const uint MediumVioletRed = 0xc71585FF;
    public const uint MidnightBlue = 0x191970FF;
    public const uint MintCream = 0xf5fffaFF;
    public const uint MistyRose = 0xffe4e1FF;
    public const uint Moccasin = 0xffe4b5FF;
    public const uint NavajoWhite = 0xffdeadFF;
    public const uint OldLace = 0xfdf5e6FF;
    public const uint OliveDrab = 0x6b8e23FF;
    public const uint Orange = 0xffa500FF;
    public const uint OrangeRed = 0xff4500FF;
    public const uint Orchid = 0xda70d6FF;
    public const uint PaleGoldenRod = 0xeee8aaFF;
    public const uint PaleGreen = 0x98fb98FF;
    public const uint PaleTurquoise = 0xafeeeeFF;
    public const uint PaleVioletRed = 0xdb7093FF;
    public const uint PapayaWhip = 0xffefd5FF;
    public const uint PeachPuff = 0xffdab9FF;
    public const uint Peru = 0xcd853fFF;
    public const uint Pink = 0xffc0cbFF;
    public const uint Plum = 0xdda0ddFF;
    public const uint PowderBlue = 0xb0e0e6FF;
    public const uint RebeccaPurple = 0x663399FF;
    public const uint RosyBrown = 0xbc8f8fFF;
    public const uint RoyalBlue = 0x4169e1FF;
    public const uint SaddleBrown = 0x8b4513FF;
    public const uint Salmon = 0xfa8072FF;
    public const uint SandyBrown = 0xf4a460FF;
    public const uint SeaGreen = 0x2e8b57FF;
    public const uint Seashell = 0xfff5eeFF;
    public const uint Sienna = 0xa0522dFF;
    public const uint SkyBlue = 0x87ceebFF;
    public const uint SlateBlue = 0x6a5acdFF;
    public const uint SlateGray = 0x708090FF;
    public const uint SlateGrey = 0x708090FF;
    public const uint Snow = 0xfffafaFF;
    public const uint SpringGreen = 0x00ff7fFF;
    public const uint SteelBlue = 0x4682b4FF;
    public const uint Tan = 0xd2b48cFF;
    public const uint Thistle = 0xd8bfd8FF;
    public const uint Tomato = 0xff6347FF;
    public const uint Turquoise = 0x40e0d0FF;
    public const uint Violet = 0xee82eeFF;
    public const uint Wheat = 0xf5deb3FF;
    public const uint WhiteSmoke = 0xf5f5f5FF;
    public const uint YellowGreen = 0x9acd32FF;
}
```

### Canvas/CornerRadius.cs

```csharp
namespace Xui.Core.Canvas;
using System.Diagnostics;

/// <summary>
/// Represents the radius of each corner of a rectangle, allowing for uniform or non-uniform rounding.
/// </summary>
public struct CornerRadius
{
    /// <summary>
    /// A <see cref = "CornerRadius"/> where all corners have a radius of zero.
    /// </summary>
    public static readonly CornerRadius Zero = new CornerRadius();
    /// <summary>
    /// Radius of the top-left corner.
    /// </summary>
    public nfloat TopLeft;
    /// <summary>
    /// Radius of the top-right corner.
    /// </summary>
    public nfloat TopRight;
    /// <summary>
    /// Radius of the bottom-right corner.
    /// </summary>
    public nfloat BottomRight;
    /// <summary>
    /// Radius of the bottom-left corner.
    /// </summary>
    public nfloat BottomLeft;
    /// <summary>
    /// Returns true if all corners have the same radius value.
    /// </summary>
    public bool IsUniform
    {
        get
        {
        }
    }

    /// <summary>
    /// Returns true if all corner radii are zero.
    /// </summary>
    public readonly bool IsZero
    {
        get
        {
        }
    }

    /// <summary>
    /// Initializes a <see cref = "CornerRadius"/> with the same radius applied to all four corners.
    /// </summary>
    /// <param name = "radius">The uniform radius for all corners.</param>
    [DebuggerStepThrough]
    public CornerRadius(nfloat radius) : this()
    {
    }

    /// <summary>
    /// Initializes a <see cref = "CornerRadius"/> with individual values for each corner.
    /// </summary>
    /// <param name = "topLeft">Radius of the top-left corner.</param>
    /// <param name = "topRight">Radius of the top-right corner.</param>
    /// <param name = "bottomRight">Radius of the bottom-right corner.</param>
    /// <param name = "bottomLeft">Radius of the bottom-left corner.</param>
    [DebuggerStepThrough]
    public CornerRadius(nfloat topLeft, nfloat topRight, nfloat bottomRight, nfloat bottomLeft)
    {
    }

    /// <summary>
    /// Implicitly converts a single radius value to a uniform <see cref = "CornerRadius"/>.
    /// </summary>
    /// <param name = "radius">The uniform corner radius.</param>
    public static implicit operator CornerRadius(nfloat radius)
    {
    }

    /// <summary>
    /// Implicitly converts a single integer radius value to a uniform <see cref = "CornerRadius"/>.
    /// </summary>
    /// <param name = "radius">The uniform corner radius.</param>
    public static implicit operator CornerRadius(int radius)
    {
    }

    /// <summary>
    /// Implicitly converts a 4-tuple of radius values to a <see cref = "CornerRadius"/>.
    /// </summary>
    /// <param name = "radii">Tuple representing (TopLeft, TopRight, BottomRight, BottomLeft).</param>
    public static implicit operator CornerRadius(Tuple<nfloat, nfloat, nfloat, nfloat> radii)
    {
    }

    /// <summary>
    /// Adds two <see cref = "CornerRadius"/> values component-wise.
    /// </summary>
    /// <param name = "lhs">The first <see cref = "CornerRadius"/>.</param>
    /// <param name = "rhs">The second <see cref = "CornerRadius"/>.</param>
    /// <returns>A new <see cref = "CornerRadius"/> where each corner is the sum of the corresponding corners.</returns>
    [DebuggerStepThrough]
    public static CornerRadius operator +(CornerRadius lhs, CornerRadius rhs)
    {
    }

    /// <summary>
    /// Subtracts one <see cref = "CornerRadius"/> from another component-wise.
    /// </summary>
    /// <param name = "lhs">The first <see cref = "CornerRadius"/>.</param>
    /// <param name = "rhs">The second <see cref = "CornerRadius"/> to subtract.</param>
    /// <returns>A new <see cref = "CornerRadius"/> where each corner is the difference of the corresponding corners.</returns>
    [DebuggerStepThrough]
    public static CornerRadius operator -(CornerRadius lhs, CornerRadius rhs)
    {
    }

    /// <summary>
    /// Returns a <see cref = "CornerRadius"/> where each corner is the minimum of the corresponding corners of the two inputs.
    /// </summary>
    /// <param name = "a">First <see cref = "CornerRadius"/>.</param>
    /// <param name = "b">Second <see cref = "CornerRadius"/>.</param>
    /// <returns>A new <see cref = "CornerRadius"/> taking the minimum value at each corner.</returns>
    [DebuggerStepThrough]
    public static CornerRadius Min(CornerRadius a, CornerRadius b)
    {
    }

    /// <summary>
    /// Returns a <see cref = "CornerRadius"/> where each corner is the maximum of the corresponding corners of the two inputs.
    /// </summary>
    /// <param name = "a">First <see cref = "CornerRadius"/>.</param>
    /// <param name = "b">Second <see cref = "CornerRadius"/>.</param>
    /// <returns>A new <see cref = "CornerRadius"/> taking the maximum value at each corner.</returns>
    [DebuggerStepThrough]
    public static CornerRadius Max(CornerRadius a, CornerRadius b)
    {
    }

    /// <summary>
    /// Implicitly converts a tuple (horizontal, vertical) into a <see cref = "CornerRadius"/>,
    /// where TopLeft and BottomRight use horizontal, and TopRight and BottomLeft use vertical radius.
    /// </summary>
    /// <param name = "radii">Tuple of two radii (horizontal, vertical).</param>
    [DebuggerStepThrough]
    public static implicit operator CornerRadius((nfloat horizontal, nfloat vertical) radii)
    {
    }

    /// <summary>
    /// Implicitly converts a tuple (topLeft, topRight, bottomRight, bottomLeft) into a <see cref = "CornerRadius"/>.
    /// </summary>
    /// <param name = "radii">Tuple of four radii representing each corner individually.</param>
    [DebuggerStepThrough]
    public static implicit operator CornerRadius((nfloat topLeft, nfloat topRight, nfloat bottomRight, nfloat bottomLeft) radii)
    {
    }
}
```

### Canvas/FillMode.cs

```csharp
namespace Xui.Core.Canvas;
/// <summary>
/// Specifies the algorithm used to determine the "insideness" of a point in a path,
/// controlling how filled shapes are rendered.
/// </summary>
public enum FillRule
{
    /// <summary>
    /// Uses the non-zero winding rule. A point is inside the path if the sum of
    /// path segment windings around it is non-zero. This is the default rule.
    /// </summary>
    NonZero,
    /// <summary>
    /// Uses the even-odd rule. A point is inside the path if a ray drawn from it
    /// to infinity crosses the path segments an odd number of times.
    /// </summary>
    EvenOdd
}
```

### Canvas/Font.cs

```csharp
namespace Xui.Core.Canvas;
/// <summary>
/// Describes a font used for text layout and rendering, similar to the CSS font model.
/// </summary>
/// <remarks>
/// This type mirrors a simplified subset of CSS font properties as documented at:
/// https://developer.mozilla.org/en-US/docs/Web/CSS/font
///
/// <para><b>Key differences:</b></para>
/// <list type="bullet">
/// <item><description><see cref = "FontFamily"/> refers to a single font name only. No fallback list or character substitution is provided.</description></item>
/// <item><description><see cref = "FontSize"/> and <see cref = "LineHeight"/> are specified in user-space units (e.g., pixels or points).</description></item>
/// <item><description>Font weight is numeric and roughly corresponds to CSS values between 100–900.</description></item>
/// </list>
/// </remarks>
public ref struct Font
{
    /// <summary>
    /// The primary font family name. No fallback fonts are supported.
    /// </summary>
    public ReadOnlySpan<string> FontFamily;
    /// <summary>
    /// The font size in user-space units.
    /// </summary>
    public nfloat FontSize;
    /// <summary>
    /// The font style (normal, italic, oblique).
    /// </summary>
    public FontStyle FontStyle;
    /// <summary>
    /// The numeric weight of the font. Matches common web font weights:
    /// <code>
    /// 100 - Thin
    /// 200 - Extra Light
    /// 300 - Light
    /// 400 - Normal
    /// 500 - Medium
    /// 600 - Semi Bold
    /// 700 - Bold
    /// 800 - Extra Bold
    /// 900 - Black
    /// </code>
    /// </summary>
    public nfloat FontWeight;
    /// <summary>
    /// The line height in user-space units. Controls vertical spacing between lines.
    /// </summary>
    public nfloat LineHeight;
}
```

### Canvas/FontStyle.cs

```csharp
namespace Xui.Core.Canvas;
using System.Diagnostics;

/// <summary>
/// Represents a web-like font style, supporting <c>normal</c>, <c>italic</c>, and <c>oblique</c> variations.
/// Global CSS styles like <c>inherit</c>, <c>unset</c>, and <c>initial</c> are not supported.
///
/// Reference: https://developer.mozilla.org/en-US/docs/Web/CSS/font-style
/// </summary>
public struct FontStyle
{
    private readonly bool italic;
    private readonly bool oblique;
    private readonly nfloat obliqueAngle;
    /// <summary>
    /// Initializes a new <see cref = "FontStyle"/> with explicit flags for italic or oblique styling.
    /// </summary>
    /// <param name = "italic">Whether the font is italic.</param>
    /// <param name = "oblique">Whether the font is oblique (slanted).</param>
    /// <param name = "obliqueAngle">Angle for oblique style, in degrees.</param>
    [DebuggerStepThrough]
    public FontStyle(bool italic, bool oblique, nfloat obliqueAngle)
    {
    }

    /// <summary>
    /// Gets a value indicating whether the font style is italic.
    /// </summary>
    public bool IsItalic
    {
        get
        {
        }
    }

    /// <summary>
    /// Gets a value indicating whether the font style is oblique (slanted).
    /// </summary>
    public bool IsOblique
    {
        get
        {
        }
    }

    /// <summary>
    /// Gets the oblique slant angle, in degrees. Only meaningful if <see cref = "IsOblique"/> is true.
    /// Range is typically from -90° to 90°.
    /// </summary>
    public nfloat ObliqueAngle
    {
        get
        {
        }
    }

    /// <summary>
    /// Normal (upright) font style.
    /// </summary>
    public static FontStyle Normal
    {
        get
        {
        }
    }

    /// <summary>
    /// Italic font style.
    /// </summary>
    public static FontStyle Italic
    {
        get
        {
        }
    }

    /// <summary>
    /// Oblique font style using the default slant angle (14°).
    /// </summary>
    public static FontStyle Oblique
    {
        get
        {
        }
    }

    /// <summary>
    /// Creates a custom oblique font style with a specific slant angle.
    /// </summary>
    /// <param name = "obliqueAngle">The angle of the slant, in degrees. Should be between -90 and 90.</param>
    /// <returns>A <see cref = "FontStyle"/> representing the custom oblique angle.</returns>
    [DebuggerStepThrough]
    public static FontStyle CustomOblique(nfloat obliqueAngle)
    {
    }
}
```

### Canvas/GradientStop.cs

```csharp
namespace Xui.Core.Canvas;
/// <summary>
/// Represents a single color stop in a gradient, defined by an offset and a color.
/// Used for both linear and radial gradients.
/// </summary>
public struct GradientStop
{
    /// <summary>
    /// The position of the stop within the gradient, typically in the range [0, 1].
    /// </summary>
    public nfloat Offset;
    /// <summary>
    /// The color at the specified offset.
    /// </summary>
    public Color Color;
    /// <summary>
    /// Initializes a new <see cref = "GradientStop"/> with a specified offset and color.
    /// </summary>
    /// <param name = "offset">Position of the color stop, usually from 0 to 1.</param>
    /// <param name = "color">Color at the given offset.</param>
    public GradientStop(nfloat offset, Color color)
    {
    }

    /// <summary>
    /// Initializes a new <see cref = "GradientStop"/> with a specified offset and packed RGBA color.
    /// </summary>
    /// <param name = "offset">Position of the color stop, usually from 0 to 1.</param>
    /// <param name = "color">Color in packed 0xRRGGBBAA format.</param>
    public GradientStop(nfloat offset, uint color)
    {
    }

    /// <summary>
    /// Implicitly converts a tuple of <see cref = "nfloat"/> and <see cref = "Color"/> to a <see cref = "GradientStop"/>.
    /// </summary>
    /// <param name = "pair">A tuple containing offset and color.</param>
    public static implicit operator GradientStop((nfloat Offset, Color Color) pair)
    {
    }

    /// <summary>
    /// Implicitly converts a tuple of <see cref = "nfloat"/> and <c>uint</c> (0xRRGGBBAA) to a <see cref = "GradientStop"/>.
    /// </summary>
    /// <param name = "pair">A tuple containing offset and packed RGBA color.</param>
    public static implicit operator GradientStop((nfloat Offset, uint Color) pair)
    {
    }
}
```

### Canvas/IContext.cs

```csharp
namespace Xui.Core.Canvas;
/// <summary>
/// Represents a 2D drawing context for the Xui Canvas, modeled after the HTML5 Canvas 2D context API.
/// This interface aggregates all sub-contexts responsible for different aspects of 2D rendering,
/// including state, drawing primitives, text, images, transformations, and resource management.
/// </summary>
public interface IContext : IMeasureContext, IStateContext, // Handles save/restore state stack and global properties
 IPenContext, // Controls stroke/fill styles, line width, caps, joins, etc.
 IPathDrawingContext, // Handles path creation and stroking/filling
 IRectDrawingContext, // Shortcut methods for drawing and clearing rectangles
 ITextDrawingContext, // Text rendering, alignment, font settings
 IImageDrawingContext, // Drawing images, bitmaps, or image-like data
 ITransformContext, // Transform matrix manipulation (translate, scale, rotate, etc.)
 IDisposable // Ensures proper cleanup of native resources
{
}
```

### Canvas/IGlyphPathBuilder.cs

```csharp
using Xui.Core.Math2D;

namespace Xui.Core.Canvas;
/// <summary>
/// A lightweight interface for constructing TrueType glyph paths
/// using move, line, curve, and close commands.
/// </summary>
/// <remarks>
/// Glyphs in TrueType fonts use quadratic Bézier curves with optional
/// off-curve control points. This interface is intended for glyph outlines
/// and font rendering engines.
/// </remarks>
public interface IGlyphPathBuilder
{
    /// <summary>
    /// Begins a new sub-path at the specified point.
    /// </summary>
    void MoveTo(Point to);
    /// <summary>
    /// Draws a straight line to the specified point.
    /// </summary>
    void LineTo(Point to);
    /// <summary>
    /// Draws a quadratic Bézier curve using a control point and end point.
    /// </summary>
    void CurveTo(Point control, Point to);
    /// <summary>
    /// Closes the current path contour.
    /// </summary>
    void ClosePath();
}
```

### Canvas/IImageDrawingContext.cs

```csharp
namespace Xui.Core.Canvas;
/// <summary>
/// Defines methods for drawing image-like content onto the canvas,
/// such as bitmaps, video frames, or decoded image buffers.
/// 
/// Mirrors the image drawing APIs from the HTML5 Canvas 2D context.
/// https://developer.mozilla.org/en-US/docs/Web/API/CanvasRenderingContext2D/drawImage
/// </summary>
public interface IImageDrawingContext
{
// Future members may include:
// void DrawImage(ImageSource image, nfloat dx, nfloat dy);
// void DrawImage(ImageSource image, nfloat dx, nfloat dy, nfloat dWidth, nfloat dHeight);
// void DrawImage(ImageSource image, nfloat sx, nfloat sy, nfloat sWidth, nfloat sHeight,
//                nfloat dx, nfloat dy, nfloat dWidth, nfloat dHeight);
}
```

### Canvas/IMeasureContext.cs

```csharp
namespace Xui.Core.Canvas;
/// <summary>
/// Defines a measurement context that provides access to platform-specific text metrics,
/// supports subpixel snapping for layout precision, and enables accurate text size calculations
/// using the underlying rendering engine's font rasterization and shaping systems.
/// </summary>
public interface IMeasureContext : ITextMeasureContext
{
}
```

### Canvas/IPathBuilder.cs

```csharp
using Xui.Core.Math2D;

namespace Xui.Core.Canvas;
/// <summary>
/// Defines methods for constructing paths,
/// following the HTML5 Canvas path API model.
///
/// Reference: https://developer.mozilla.org/en-US/docs/Web/API/CanvasRenderingContext2D#paths
/// </summary>
public interface IPathBuilder : IGlyphPathBuilder
{
    /// <summary>
    /// Begins a new path by resetting the current path list.
    /// </summary>
    void BeginPath();
    /// <summary>
    /// Draws a cubic Bézier curve from the current point to the specified point,
    /// using two control points.
    /// </summary>
    /// <param name = "cp1">First control point.</param>
    /// <param name = "cp2">Second control point.</param>
    /// <param name = "to">End point.</param>
    void CurveTo(Point cp1, Point cp2, Point to);
    /// <summary>
    /// Adds an arc to the path centered at the specified point.
    /// </summary>
    /// <param name = "center">Center of the arc.</param>
    /// <param name = "radius">Radius of the arc.</param>
    /// <param name = "startAngle">Start angle in radians.</param>
    /// <param name = "endAngle">End angle in radians.</param>
    /// <param name = "winding">Direction in which the arc is drawn.</param>
    void Arc(Point center, nfloat radius, nfloat startAngle, nfloat endAngle, Winding winding = Winding.ClockWise);
    /// <summary>
    /// Adds an arc to the path, connecting two tangents defined by control points.
    /// </summary>
    /// <param name = "cp1">First control point.</param>
    /// <param name = "cp2">Second control point.</param>
    /// <param name = "radius">Arc radius.</param>
    void ArcTo(Point cp1, Point cp2, nfloat radius);
    /// <summary>
    /// Adds an elliptical arc to the path.
    /// </summary>
    /// <param name = "center">Center of the ellipse.</param>
    /// <param name = "radiusX">Horizontal radius.</param>
    /// <param name = "radiusY">Vertical radius.</param>
    /// <param name = "rotation">Rotation of the ellipse, in radians.</param>
    /// <param name = "startAngle">Start angle in radians.</param>
    /// <param name = "endAngle">End angle in radians.</param>
    /// <param name = "winding">Direction in which the arc is drawn.</param>
    void Ellipse(Point center, nfloat radiusX, nfloat radiusY, nfloat rotation, nfloat startAngle, nfloat endAngle, Winding winding = Winding.ClockWise);
    /// <summary>
    /// Adds a rectangle path to the current path.
    /// </summary>
    /// <param name = "rect">The rectangle to add.</param>
    void Rect(Rect rect);
    /// <summary>
    /// Adds a rounded rectangle path with a uniform radius.
    /// </summary>
    /// <param name = "rect">The rectangle to round.</param>
    /// <param name = "radius">Corner radius.</param>
    void RoundRect(Rect rect, nfloat radius);
    /// <summary>
    /// Adds a rounded rectangle path with per-corner radii.
    /// </summary>
    /// <param name = "rect">The rectangle to round.</param>
    /// <param name = "radius">Corner radius object defining each corner.</param>
    void RoundRect(Rect rect, CornerRadius radius);
}
```

### Canvas/IPathClipping.cs

```csharp
namespace Xui.Core.Canvas;
/// <summary>
/// Defines methods for clipping using a path,
/// following the HTML5 Canvas path API model.
///
/// Reference: https://developer.mozilla.org/en-US/docs/Web/API/CanvasRenderingContext2D#paths
/// </summary>
public interface IPathClipping
{
    /// <summary>
    /// Sets the current clipping region to the current path.
    /// Subsequent drawing operations are clipped to this region.
    /// </summary>
    void Clip();
}
```

### Canvas/IPathDrawing.cs

```csharp
namespace Xui.Core.Canvas;
/// <summary>
/// Defines methods for drawing a constructed path,
/// following the HTML5 Canvas path API model.
///
/// Reference: https://developer.mozilla.org/en-US/docs/Web/API/CanvasRenderingContext2D#paths
/// </summary>
public interface IPathDrawing
{
    /// <summary>
    /// Fills the current path using the specified fill rule.
    /// </summary>
    /// <param name = "rule">The fill rule to use (NonZero or EvenOdd).</param>
    void Fill(FillRule rule = FillRule.NonZero);
    /// <summary>
    /// Strokes the current path using the current stroke style.
    /// </summary>
    void Stroke();
}
```

### Canvas/IPathDrawingContext.cs

```csharp
using Xui.Core.Math2D;

namespace Xui.Core.Canvas;
/// <summary>
/// Defines methods for constructing and rendering 2D paths,
/// following the HTML5 Canvas path API model.
///
/// Reference: https://developer.mozilla.org/en-US/docs/Web/API/CanvasRenderingContext2D#paths
/// </summary>
public interface IPathDrawingContext : IPathBuilder, IPathDrawing, IPathClipping
{
}
```

### Canvas/IPenContext.cs

```csharp
namespace Xui.Core.Canvas;
/// <summary>
/// Defines properties and methods for controlling stroke and fill styles in a 2D drawing context,
/// including line caps, joins, width, dashes, and brush settings for fill and stroke.
/// </summary>
public interface IPenContext
{
    /// <summary>
    /// Sets the global alpha value for all drawing operations.
    /// Range: 0.0 (fully transparent) to 1.0 (fully opaque).
    /// </summary>
    nfloat GlobalAlpha { set; }

    /// <summary>
    /// Sets the style of line caps used for strokes.
    /// </summary>
    LineCap LineCap { set; }

    /// <summary>
    /// Sets the style of line joins between segments.
    /// </summary>
    LineJoin LineJoin { set; }

    /// <summary>
    /// Sets the width of stroked lines, in user space units.
    /// </summary>
    nfloat LineWidth { set; }

    /// <summary>
    /// Sets the miter limit ratio for miter joins.
    /// If the ratio of miter length to line width exceeds this value, a bevel join is used instead.
    /// </summary>
    nfloat MiterLimit { set; }

    /// <summary>
    /// Sets the phase offset for the start of the dash pattern.
    /// </summary>
    nfloat LineDashOffset { set; }

    /// <summary>
    /// Sets the dash pattern used for stroking lines.
    /// Each element in the span represents a dash or gap length, alternating.
    /// </summary>
    /// <param name = "segments">A sequence of dash and gap lengths.</param>
    void SetLineDash(ReadOnlySpan<nfloat> segments);
    /// <summary>
    /// Sets the stroke style to a solid color.
    /// </summary>
    /// <param name = "color">The stroke color.</param>
    void SetStroke(Color color);
    /// <summary>
    /// Sets the stroke style to a linear gradient.
    /// </summary>
    /// <param name = "linearGradient">The gradient to use for stroking paths.</param>
    void SetStroke(LinearGradient linearGradient);
    /// <summary>
    /// Sets the stroke style to a radial gradient.
    /// </summary>
    /// <param name = "radialGradient">The gradient to use for stroking paths.</param>
    void SetStroke(RadialGradient radialGradient);
    /// <summary>
    /// Sets the fill style to a solid color.
    /// </summary>
    /// <param name = "color">The fill color.</param>
    void SetFill(Color color);
    /// <summary>
    /// Sets the fill style to a linear gradient.
    /// </summary>
    /// <param name = "linearGradient">The gradient to use for filling shapes.</param>
    void SetFill(LinearGradient linearGradient);
    /// <summary>
    /// Sets the fill style to a radial gradient.
    /// </summary>
    /// <param name = "radialGradient">The gradient to use for filling shapes.</param>
    void SetFill(RadialGradient radialGradient);
}
```

### Canvas/IRectDrawingContext.cs

```csharp
using Xui.Core.Math2D;

namespace Xui.Core.Canvas;
/// <summary>
/// Provides convenience methods for drawing filled and stroked rectangles,
/// similar to the <c>fillRect()</c> and <c>strokeRect()</c> functions in the HTML5 Canvas API.
/// </summary>
public interface IRectDrawingContext
{
    /// <summary>
    /// Draws the outline of the specified rectangle using the current stroke style.
    /// </summary>
    /// <param name = "rect">The rectangle to stroke.</param>
    void StrokeRect(Rect rect);
    /// <summary>
    /// Fills the specified rectangle using the current fill style.
    /// </summary>
    /// <param name = "rect">The rectangle to fill.</param>
    void FillRect(Rect rect);
}
```

### Canvas/IStateContext.cs

```csharp
namespace Xui.Core.Canvas;
/// <summary>
/// Provides methods to save and restore the current drawing state,
/// including styles, transforms, and clipping paths.
/// Mirrors the behavior of the HTML5 Canvas <c>save()</c> and <c>restore()</c> methods.
/// </summary>
public interface IStateContext
{
    /// <summary>
    /// Pushes the current drawing state onto the state stack.
    /// This includes styles, transformations, clipping paths, etc.
    /// </summary>
    void Save();
    /// <summary>
    /// Pops the top state off the state stack and restores it.
    /// Any modifications made since the last <see cref = "Save"/> are discarded.
    /// </summary>
    void Restore();
}
```

### Canvas/ITextDrawingContext.cs

```csharp
using Xui.Core.Math2D;

namespace Xui.Core.Canvas;
/// <summary>
/// Defines methods for drawing text onto the canvas, including alignment and baseline options.
/// Mirrors the HTML5 Canvas 2D text API for <c>fillText()</c>.
/// Also provides access to text measurement via <see cref = "ITextMeasureContext"/>.
/// </summary>
public interface ITextDrawingContext : ITextMeasureContext
{
    /// <summary>
    /// Sets the horizontal alignment of the text relative to the given position.
    /// </summary>
    TextAlign TextAlign { set; }

    /// <summary>
    /// Sets the vertical alignment of the text relative to the given baseline.
    /// </summary>
    TextBaseline TextBaseline { set; }

    /// <summary>
    /// Draws filled text at the specified position using the current fill style and font settings.
    /// </summary>
    /// <param name = "text">The text string to render.</param>
    /// <param name = "pos">The position at which to start rendering the text.</param>
    void FillText(string text, Point pos);
}
```

### Canvas/ITextMeasureContext.cs

```csharp
using Xui.Core.Math2D;

namespace Xui.Core.Canvas;
/// <summary>
/// Provides functionality for measuring text and setting font properties.
/// Mirrors the measurement behavior of the HTML5 Canvas 2D context.
/// </summary>
public interface ITextMeasureContext
{
    /// <summary>
    /// Measures the width and height of the specified text string using the current font.
    /// </summary>
    /// <param name = "text">The text string to measure.</param>
    /// <returns>
    /// A <see cref = "Vector"/> representing the size of the rendered text.
    /// Typically, X is width and Y is line height or baseline ascent.
    /// </returns>
    Vector MeasureText(string text);
    /// <summary>
    /// Sets the font used for all subsequent text drawing and measurement operations.
    /// </summary>
    /// <param name = "font">The font definition to apply.</param>
    void SetFont(Font font);
}
```

### Canvas/ITransformContext.cs

```csharp
using Xui.Core.Math2D;

namespace Xui.Core.Canvas;
/// <summary>
/// Provides methods for manipulating the current transformation matrix of the drawing context.
/// This includes translation, scaling, rotation, and applying affine transformations.
/// Mirrors the transformation API of the HTML5 Canvas 2D context.
/// </summary>
public interface ITransformContext
{
    /// <summary>
    /// Applies a translation by the specified vector.
    /// This offsets all subsequent drawing operations by the given amount.
    /// </summary>
    /// <param name = "vector">The translation vector (dx, dy).</param>
    void Translate(Vector vector);
    /// <summary>
    /// Applies a clockwise rotation to the current transform.
    /// </summary>
    /// <param name = "angle">The rotation angle in radians.</param>
    void Rotate(nfloat angle);
    /// <summary>
    /// Applies a scaling transformation using the specified scaling factors.
    /// </summary>
    /// <param name = "vector">The scaling vector (sx, sy).</param>
    void Scale(Vector vector);
    /// <summary>
    /// Resets the current transformation matrix to the identity matrix,
    /// then replaces it with the specified transform.
    /// Equivalent to <c>ctx.setTransform(...)</c> in HTML5 Canvas.
    /// </summary>
    /// <param name = "transform">The new transformation matrix to apply.</param>
    void SetTransform(AffineTransform transform);
    /// <summary>
    /// Multiplies the current transformation matrix by the specified matrix.
    /// This is equivalent to applying an additional transformation on top of the existing one.
    /// </summary>
    /// <param name = "matrix">The matrix to multiply with the current transform.</param>
    void Transform(AffineTransform matrix);
}
```

### Canvas/LinearGradient.cs

```csharp
using Xui.Core.Math2D;

namespace Xui.Core.Canvas;
/// <summary>
/// Represents a linear gradient brush that transitions between colors along a straight line,
/// following the same behavior as the HTML5 Canvas <c>createLinearGradient()</c>.
/// </summary>
public ref struct LinearGradient
{
    /// <summary>
    /// The starting point of the gradient, in user space coordinates.
    /// </summary>
    public Point StartPoint;
    /// <summary>
    /// The ending point of the gradient, in user space coordinates.
    /// </summary>
    public Point EndPoint;
    /// <summary>
    /// The sequence of gradient stops that define color transitions along the line.
    /// Offsets are typically in the range [0, 1].
    /// </summary>
    public ReadOnlySpan<GradientStop> GradientStops;
    /// <summary>
    /// Creates a new <see cref = "LinearGradient"/> with the given start and end points and gradient stops.
    /// </summary>
    /// <param name = "start">The start point of the gradient.</param>
    /// <param name = "end">The end point of the gradient.</param>
    /// <param name = "gradient">A span of gradient stops sorted by offset.</param>
    public LinearGradient(Point start, Point end, ReadOnlySpan<GradientStop> gradient)
    {
    }
}
```

### Canvas/LineCap.cs

```csharp
namespace Xui.Core.Canvas;
/// <summary>
/// Specifies the shape used at the ends of lines when stroking paths.
/// Mirrors the <c>lineCap</c> property in the HTML5 Canvas API.
/// </summary>
public enum LineCap
{
    /// <summary>
    /// The line ends exactly at the endpoint with no additional extension. This is the default.
    /// </summary>
    Butt = 0,
    /// <summary>
    /// The line ends with a semicircular extension, centered on the endpoint.
    /// </summary>
    Round = 1,
    /// <summary>
    /// The line ends with a square extension equal to half the line width, extending beyond the endpoint.
    /// </summary>
    Square = 2
}
```

### Canvas/LineJoin.cs

```csharp
namespace Xui.Core.Canvas;
/// <summary>
/// Specifies how two connected lines are joined when stroking a path.
/// Mirrors the <c>lineJoin</c> property in the HTML5 Canvas API.
/// </summary>
public enum LineJoin
{
    /// <summary>
    /// Lines are joined with a sharp corner or extended miter, depending on the miter limit.
    /// </summary>
    Miter = 0,
    /// <summary>
    /// Lines are joined with a circular arc, creating a rounded corner.
    /// </summary>
    Round = 1,
    /// <summary>
    /// Lines are joined with a beveled corner by connecting the outer corners of the strokes.
    /// </summary>
    Bevel = 2
}
```

### Canvas/PaintStyle.cs

```csharp
namespace Xui.Core.Canvas;
/// <summary>
/// Specifies the type of brush or fill style used for painting shapes or strokes.
/// Helps distinguish between solid colors and gradient fills.
/// </summary>
public enum PaintStyle
{
    /// <summary>
    /// A single, uniform color.
    /// </summary>
    SolidColor = 0,
    /// <summary>
    /// A linear gradient that transitions colors along a straight line.
    /// </summary>
    LinearGradient = 1,
    /// <summary>
    /// A radial gradient that transitions colors outward in a circular or elliptical shape.
    /// </summary>
    RadialGradient = 2
}
```

### Canvas/Path2D.cs

```csharp
using System.Runtime.CompilerServices;
using Xui.Core.Math2D;

namespace Xui.Core.Canvas;
public class Path2D : IPathBuilder
{
    private byte[] _data;
    private int _length;
    public Path2D(int initialCapacity = 256)
    {
    }

    public void BeginPath()
    {
    }

    public void MoveTo(Point to)
    {
    }

    public void LineTo(Point to)
    {
    }

    public void ClosePath()
    {
    }

    public void CurveTo(Point cp1, Point to)
    {
    }

    public void CurveTo(Point cp1, Point cp2, Point to)
    {
    }

    public void Arc(Point center, nfloat radius, nfloat startAngle, nfloat endAngle, Winding winding = Winding.ClockWise)
    {
    }

    public void ArcTo(Point cp1, Point cp2, nfloat radius)
    {
    }

    public void Ellipse(Point center, nfloat radiusX, nfloat radiusY, nfloat rotation, nfloat startAngle, nfloat endAngle, Winding winding = Winding.ClockWise)
    {
    }

    public void Rect(Rect rect)
    {
    }

    public void RoundRect(Rect rect, nfloat radius)
    {
    }

    public void RoundRect(Rect rect, CornerRadius radius)
    {
    }

    private void WriteCommand(PathCommandType cmd)
    {
    }

    private void WritePoint(Point p)
    {
    }

    private void WriteRect(Rect r)
    {
    }

    private void WriteCornerRadius(CornerRadius r)
    {
    }

    private void WriteNFloat(nfloat value)
    {
    }

    private void WriteByte(byte value)
    {
    }

    private void EnsureCapacity(int sizeHint)
    {
    }

    enum PathCommandType : byte
    {
        MoveTo,
        LineTo,
        ClosePath,
        QuadraticCurveTo,
        CubicCurveTo,
        Arc,
        ArcTo,
        Ellipse,
        Rect,
        RoundRectUniform,
        RoundRectCorners
    }

    public void Visit(IPathBuilder sink)
    {
    }

    private Point ReadPoint(ref int pos)
    {
    }

    private Rect ReadRect(ref int pos)
    {
    }

    private CornerRadius ReadCornerRadius(ref int pos)
    {
    }

    private nfloat ReadNFloat(ref int pos)
    {
    }
}
```

### Canvas/RadialGradient.cs

```csharp
using Xui.Core.Math2D;

namespace Xui.Core.Canvas;
/// <summary>
/// Represents a radial gradient brush that transitions colors outward from one circle to another,
/// similar to <c>createRadialGradient()</c> in the HTML5 Canvas API.
/// </summary>
public ref struct RadialGradient
{
    /// <summary>
    /// The center point of the starting circle (inner circle).
    /// </summary>
    public Point StartCenter;
    /// <summary>
    /// The radius of the starting circle (inner circle), usually 0 for solid centers.
    /// </summary>
    public nfloat StartRadius;
    /// <summary>
    /// The center point of the ending circle (outer circle).
    /// </summary>
    public Point EndCenter;
    /// <summary>
    /// The radius of the ending circle (outer circle), defining the spread of the gradient.
    /// </summary>
    public nfloat EndRadius;
    /// <summary>
    /// A sequence of color stops defining how colors are interpolated from the inner to the outer circle.
    /// Offsets are typically in the range [0, 1].
    /// </summary>
    public ReadOnlySpan<GradientStop> GradientStops;
    /// <summary>
    /// Initializes a radial gradient that transitions between two circles.
    /// </summary>
    /// <param name = "startCenter">Center of the inner circle.</param>
    /// <param name = "startRadius">Radius of the inner circle.</param>
    /// <param name = "endCenter">Center of the outer circle.</param>
    /// <param name = "endRadius">Radius of the outer circle.</param>
    /// <param name = "gradientStops">Span of color stops defining the gradient transition.</param>
    public RadialGradient(Point startCenter, nfloat startRadius, Point endCenter, nfloat endRadius, ReadOnlySpan<GradientStop> gradientStops)
    {
    }

    /// <summary>
    /// Initializes a radial gradient from a single point and radius, fading outward from a solid center.
    /// This is equivalent to setting the start radius to zero and both centers to the same point.
    /// </summary>
    /// <param name = "center">Center point of the gradient.</param>
    /// <param name = "radius">Radius of the gradient spread.</param>
    /// <param name = "gradientStops">Span of color stops defining the gradient transition.</param>
    public RadialGradient(Point center, nfloat radius, ReadOnlySpan<GradientStop> gradientStops)
    {
    }
}
```

### Canvas/SVG/ArcFlag.cs

```csharp
/// <summary>
/// Specifies the size of the arc to be drawn when interpreting an SVG arc command.
/// Used in conjunction with the sweep flag to determine the final arc segment.
///
/// This corresponds to the "large-arc-flag" in the SVG path specification.
/// </summary>
public enum ArcFlag
{
    /// <summary>
    /// Draw the smaller of the two possible arc sweeps (less than or equal to 180 degrees).
    /// </summary>
    Small = 0,
    /// <summary>
    /// Draw the larger of the two possible arc sweeps (greater than 180 degrees).
    /// </summary>
    Large = 1
}
```

### Canvas/SVG/Extensions.cs

```csharp
namespace Xui.Core.Canvas.SVG;
/// <summary>
/// Provides SVG-related extension methods for the <see cref = "IPathDrawingContext"/> interface.
/// </summary>
public static class Extensions
{
    /// <summary>
    /// Creates a <see cref = "PathDataBuilder"/> for the given path drawing context.
    /// 
    /// This enables fluent parsing or construction of SVG path commands targeting the current canvas context.
    /// </summary>
    /// <param name = "this">The path drawing context (e.g., a canvas or drawing surface).</param>
    /// <returns>A <see cref = "PathDataBuilder"/> instance for building or parsing SVG paths.</returns>
    public static PathDataBuilder PathData(this IPathDrawingContext @this)
    {
    }
}
```

### Canvas/SVG/PathDataBuilder.cs

```csharp
using Xui.Core.Math2D;

namespace Xui.Core.Canvas.SVG;
/// <summary>
/// A utility for converting parsed SVG path commands into drawing operations
/// on an <see cref = "IPathDrawingContext"/> target.
/// 
/// This builder supports both absolute (e.g., <c>M</c>, <c>L</c>, <c>C</c>) and relative (e.g., <c>m</c>, <c>l</c>, <c>c</c>) SVG commands,
/// and tracks the current drawing point and control points for smooth curves.
/// </summary>
public ref struct PathDataBuilder
{
    /// <summary>
    /// The target drawing context that receives path commands.
    /// </summary>
    public readonly IPathDrawingContext Sink;
    /// <summary>
    /// The starting point of the current subpath (used for <c>Z</c> command).
    /// </summary>
    public Point? StartPoint;
    /// <summary>
    /// The current position of the pen after the last command.
    /// </summary>
    public Point CurrentPoint;
    /// <summary>
    /// The reflected control point for the next cubic Bézier curve (used in <c>S</c>, <c>s</c>).
    /// </summary>
    public Point NextCubicControlPoint;
    /// <summary>
    /// The reflected control point for the next quadratic Bézier curve (used in <c>T</c>, <c>t</c>).
    /// </summary>
    public Point NextQuadraticControlPoint;
    /// <summary>
    /// Initializes a new <see cref = "PathDataBuilder"/> targeting the specified drawing context.
    /// </summary>
    /// <param name = "sink">The drawing context that will receive the path instructions.</param>
    public PathDataBuilder(IPathDrawingContext sink)
    {
    }

    /// <summary>
    /// Begins a new path on the drawing context.
    /// </summary>
    public PathDataBuilder Begin()
    {
    }

#pragma warning disable CS1591
    public PathDataBuilder M(Point point)
    {
    }

    public PathDataBuilder m(Vector vector)
    {
    }

    public PathDataBuilder L(Point point)
    {
    }

    public PathDataBuilder l(Vector vector)
    {
    }

    public PathDataBuilder H(nfloat h)
    {
    }

    public PathDataBuilder h(nfloat h)
    {
    }

    public PathDataBuilder V(nfloat v)
    {
    }

    public PathDataBuilder v(nfloat v)
    {
    }

    public PathDataBuilder C(Point cp1, Point cp2, Point to)
    {
    }

    public PathDataBuilder c(Vector cv1, Vector cv2, Vector to)
    {
    }

    public PathDataBuilder S(Point cp, Point to)
    {
    }

    public PathDataBuilder s(Vector cv, Vector to)
    {
    }

    public PathDataBuilder Q(Point cp, Point to)
    {
    }

    public PathDataBuilder q(Vector cv, Vector to)
    {
    }

    public PathDataBuilder T(Point to)
    {
    }

    public PathDataBuilder t(Vector to)
    {
    }

    public PathDataBuilder A(Vector sr, nfloat xAxisRotationDeg, ArcFlag largeArcFlag, Winding sweepDirection, Point p2)
    {
    }

    public PathDataBuilder a(Vector v2, nfloat xAxisRotationDeg, ArcFlag largeArcFlag, Winding sweepDirection, Vector sr)
    {
    }

    public PathDataBuilder Z()
    {
    }

    public PathDataBuilder z()
    {
    }
}
```

### Canvas/TextAlign.cs

```csharp
namespace Xui.Core.Canvas;
/// <summary>
/// Specifies the horizontal alignment of text relative to the drawing position.
/// Mirrors the <c>textAlign</c> property in the HTML5 Canvas 2D API.
/// </summary>
public enum TextAlign
{
    /// <summary>
    /// Aligns text to the start of the writing direction (left for LTR, right for RTL).
    /// </summary>
    Start,
    /// <summary>
    /// Aligns text to the end of the writing direction (right for LTR, left for RTL).
    /// </summary>
    End,
    /// <summary>
    /// Aligns text to the left, regardless of writing direction.
    /// </summary>
    Left,
    /// <summary>
    /// Aligns text to the right, regardless of writing direction.
    /// </summary>
    Right,
    /// <summary>
    /// Centers the text horizontally around the drawing position.
    /// </summary>
    Center
}
```

### Canvas/TextBaseline.cs

```csharp
namespace Xui.Core.Canvas;
/// <summary>
/// Specifies the vertical alignment of text relative to the drawing position.
/// Mirrors the <c>textBaseline</c> property in the HTML5 Canvas 2D API.
/// </summary>
public enum TextBaseline
{
    /// <summary>
    /// The top of the em box is aligned with the drawing position.
    /// </summary>
    Top,
    /// <summary>
    /// The hanging baseline is aligned with the drawing position (used in some scripts like Devanagari).
    /// </summary>
    Hanging,
    /// <summary>
    /// The middle of the text (roughly half the em height) is aligned with the drawing position.
    /// </summary>
    Middle,
    /// <summary>
    /// The alphabetic baseline (default for Latin scripts) is aligned with the drawing position.
    /// </summary>
    Alphabetic,
    /// <summary>
    /// The ideographic baseline is aligned with the drawing position (used in scripts like Chinese or Japanese).
    /// </summary>
    Ideographic,
    /// <summary>
    /// The bottom of the em box is aligned with the drawing position.
    /// </summary>
    Bottom
}
```

### Canvas/Winding.cs

```csharp
namespace Xui.Core.Canvas;
/// <summary>
/// Specifies the direction in which arcs or elliptical curves are drawn,
/// affecting fill and stroke behavior in path-based rendering.
/// </summary>
public enum Winding : int
{
    /// <summary>
    /// The path is drawn in a clockwise direction.
    /// This is the default in most canvas and geometry operations.
    /// </summary>
    ClockWise = 1,
    /// <summary>
    /// The path is drawn in a counter-clockwise direction.
    /// </summary>
    CounterClockWise = 0
}
```

### Curves2D/Arc.cs

```csharp
using Xui.Core.Math2D;
using Xui.Core.Canvas;

namespace Xui.Core.Curves2D;
/// <summary>
/// Represents a circular or elliptical arc defined by a center point, radii, rotation, and sweep angles.
/// </summary>
/// <remarks>
/// Arcs are useful for describing elliptical curves in vector graphics, layout paths, and stroke outlines.
/// They interpolate smoothly from <see cref = "StartAngle"/> to <see cref = "EndAngle"/>, optionally applying
/// rotation and winding direction.
/// </remarks>
public readonly struct Arc : ICurve
{
    /// <summary>The center point of the arc's ellipse.</summary>
    public readonly Point Center;
    /// <summary>The horizontal radius of the arc.</summary>
    public readonly nfloat RadiusX;
    /// <summary>The vertical radius of the arc.</summary>
    public readonly nfloat RadiusY;
    /// <summary>The clockwise rotation (in radians) applied to the arc relative to the X axis.</summary>
    public readonly nfloat Rotation;
    /// <summary>The angle (in radians) at which the arc starts, before rotation is applied.</summary>
    public readonly nfloat StartAngle;
    /// <summary>The angle (in radians) at which the arc ends, before rotation is applied.</summary>
    public readonly nfloat EndAngle;
    /// <summary>Specifies whether the arc is drawn clockwise or counter-clockwise.</summary>
    public readonly Winding Winding;
    /// <summary>
    /// Initializes a new <see cref = "Arc"/> with the given center, radii, rotation, angles, and winding direction.
    /// </summary>
    /// <param name = "center">The center point of the arc's ellipse.</param>
    /// <param name = "rx">The horizontal radius.</param>
    /// <param name = "ry">The vertical radius.</param>
    /// <param name = "rotation">The clockwise rotation in radians applied to the ellipse.</param>
    /// <param name = "startAngle">The start angle of the arc in radians (before rotation).</param>
    /// <param name = "endAngle">The end angle of the arc in radians (before rotation).</param>
    /// <param name = "winding">Whether the arc is drawn clockwise or counter-clockwise.</param>
    public Arc(Point center, nfloat rx, nfloat ry, nfloat rotation, nfloat startAngle, nfloat endAngle, Winding winding = Winding.ClockWise)
    {
    }

    /// <summary>
    /// Gets the interpolated point on the arc at the specified parameter <paramref name = "t"/>.
    /// </summary>
    /// <param name = "t">A normalized value from 0 to 1 representing the progression along the arc.</param>
    public Point this[nfloat t]
    {
        get
        {
        }
    }

    /// <summary>
    /// Computes the interpolated point on the arc at a given parameter <paramref name = "t"/>.
    /// </summary>
    /// <param name = "t">A normalized value in the range [0, 1].</param>
    public Point Lerp(nfloat t)
    {
    }

    /// <summary>
    /// Evaluates the arc at a normalized parameter <paramref name = "t"/>, returning the corresponding point.
    /// </summary>
    /// <param name = "t">A normalized value in the range [0, 1].</param>
    /// <returns>The corresponding point on the rotated ellipse arc.</returns>
    public Point Evaluate(nfloat t)
    {
    }

    /// <summary>
    /// Computes the tangent vector at parameter <paramref name = "t"/> on the arc.
    /// </summary>
    /// <param name = "t">A normalized value in the range [0, 1].</param>
    /// <returns>The normalized tangent vector at the evaluated point on the arc.</returns>
    public Vector Tangent(nfloat t)
    {
    }

    /// <summary>
    /// Approximates the arc length using uniform sampling with 16 segments.
    /// </summary>
    /// <returns>The approximate total arc length.</returns>
    public nfloat Length()
    {
    }

    /// <summary>
    /// Approximates the arc length using recursive adaptive subdivision.
    /// </summary>
    /// <param name = "flatness">The maximum allowed error per segment for arc approximation.</param>
    /// <returns>The computed arc length within the specified flatness tolerance.</returns>
    public nfloat Length(nfloat flatness)
    {
    }

    private nfloat LengthRecursive(nfloat t0, nfloat t1, Point p0, Point p1, nfloat tol)
    {
    }

    /// <summary>
    /// Gets the signed sweep angle of the arc, in radians.
    /// Adjusts automatically for the specified <see cref = "Winding"/>.
    /// </summary>
    public nfloat SweepAngle
    {
        get
        {
        }
    }

    /// <summary>
    /// Converts this arc to an endpoint-parameterized <see cref = "ArcEndpoint"/> representation.
    /// </summary>
    /// <remarks>
    /// This method generates a single arc segment between the start and end points of the original arc,
    /// preserving the sweep direction and determining whether the arc is the larger of the two possible segments.
    /// If the arc forms a complete circle (i.e., the start and end points match and the sweep covers 360°),
    /// the endpoint position is nudged slightly to avoid rendering ambiguities in formats that do not support full-circle arcs directly.
    /// </remarks>
    /// <returns>
    /// An <see cref = "ArcEndpoint"/> structure that approximates this arc using endpoint-based parametrization.
    /// </returns>
    public ArcEndpoint ToEndpointArc()
    {
    }

    /// <summary>
    /// Converts this arc to one or two endpoint-parameterized arcs, depending on the arc's sweep angle.
    /// </summary>
    /// <remarks>
    /// If the arc forms a complete or nearly complete circle (i.e., the sweep angle approaches ±360° and the start and end points match),
    /// the arc is split into two half-circle segments to avoid rendering issues in formats that do not support full-circle arcs directly.
    /// Otherwise, a single <see cref = "ArcEndpoint"/> is returned with appropriate flags set for direction and size.
    /// </remarks>
    /// <returns>
    /// A tuple of one or two <see cref = "ArcEndpoint"/> instances that collectively represent the same elliptical arc:
    /// <list type="bullet">
    ///   <item>
    ///     <description>
    ///       <c>Item1</c> – The first arc segment (always non-null).
    ///     </description>
    ///   </item>
    ///   <item>
    ///     <description>
    ///       <c>Item2</c> – The second arc segment if the arc is split, or <c>null</c> if the arc fits in a single segment.
    ///     </description>
    ///   </item>
    /// </list>
    /// </returns>
    public (ArcEndpoint, ArcEndpoint? ) ToEndpointArcs()
    {
    }

    /// <summary>
    /// Evaluates the position on the ellipse at a given angle <paramref name = "θ"/> (in radians),
    /// relative to the unrotated ellipse center.
    /// </summary>
    /// <param name = "θ">
    /// The angle in radians, measured from the X axis of the unrotated ellipse.
    /// This is not a normalized parameter like <c>t</c>, but an absolute angle used in parametric arc equations.
    /// </param>
    /// <returns>
    /// The corresponding <see cref = "Point"/> on the arc's ellipse at the given angle,
    /// after applying the arc's rotation and translating from its center.
    /// </returns>
    /// <remarks>
    /// The point is computed using the parametric equation of an axis-aligned ellipse:
    /// <c>(x, y) = (rx * cos(θ), ry * sin(θ))</c>, then rotated by <see cref = "Rotation"/> and translated to <see cref = "Center"/>.
    /// This method is useful when working directly with angle-based metrics like vertical extrema (e.g., θ = π/2 or 3π/2).
    /// </remarks>
    public Point EvaluateAtAngle(nfloat θ)
    {
    }

    /// <summary>
    /// Splits this arc into a series of <see cref = "Arc3Point"/> segments,
    /// each spanning ≤ 90° and aligned so that one edge is axis-aligned.
    /// </summary>
    /// <param name = "buffer">
    /// A span to receive the output segments. Must have space for up to 4 entries.
    /// </param>
    /// <param name = "count">
    /// The number of segments written to <paramref name = "buffer"/>.
    /// </param>
    /// <remarks>
    /// This method ensures consistent arc tessellation by aligning segment edges with
    /// X or Y axes and bounding each by ≤ 90°, suitable for round rects or shader-compatible curves.
    /// </remarks>
    public void ToArc3Segments(Span<Arc3Point> buffer, out int count)
    {
    }
}
```

### Curves2D/Arc3Point.cs

```csharp
using Xui.Core.Math2D;
using Xui.Core.Canvas;

namespace Xui.Core.Curves2D;
/// <summary>
/// Represents a circular arc defined by three points: start, mid (on-curve), and end.
/// </summary>
/// <remarks>
/// The arc is guaranteed to pass through all three control points <see cref = "P0"/>, <see cref = "P1"/>, and <see cref = "P2"/>.
/// The arc lies on the unique circle that intersects these points and is useful for geometric primitives,
/// shader-based SDF rendering, and polygonal arc approximation.
/// </remarks>
public readonly struct Arc3Point : ICurve
{
    /// <summary>The starting point of the arc.</summary>
    public readonly Point P0;
    /// <summary>A point on the arc between <see cref = "P0"/> and <see cref = "P2"/>.</summary>
    public readonly Point P1;
    /// <summary>The ending point of the arc.</summary>
    public readonly Point P2;
    /// <summary>
    /// Initializes a new <see cref = "Arc3Point"/> defined by three on-curve points.
    /// </summary>
    public Arc3Point(Point p0, Point p1, Point p2)
    {
    }

    /// <summary>
    /// Converts this three-point arc into a center-based <see cref = "Arc"/> if the arc is valid.
    /// </summary>
    /// <returns>
    /// A valid <see cref = "Arc"/> if the input points define a circular segment; otherwise <c>null</c>.
    /// If the result is <c>null</c>, consider falling back to a straight <see cref = "Segment"/> from <see cref = "P0"/> to <see cref = "P2"/>.
    /// </returns>
    public Arc? ToCenterArc()
    {
    }

    /// <summary>
    /// Converts this arc to an endpoint-based <see cref = "ArcEndpoint"/> format, if valid.
    /// </summary>
    public ArcEndpoint? ToEndpointArc()
    {
    }

    /// <inheritdoc/>
    public Point Lerp(nfloat t)
    {
    }

    /// <inheritdoc/>
    public Vector Tangent(nfloat t)
    {
    }

    /// <inheritdoc/>
    public Point this[nfloat t]
    {
        get
        {
        }
    }

    /// <inheritdoc/>
    public nfloat Length()
    {
    }

    /// <inheritdoc/>
    public nfloat Length(nfloat precision)
    {
    }

    /// <summary>
    /// Implicitly converts a tuple of 3 points to an <see cref = "Arc3Point"/>.
    /// </summary>
    public static implicit operator Arc3Point((Point p0, Point p1, Point p2) value)
    {
    }

    private static bool TryLineIntersection(Point p1, Point p2, Point q1, Point q2, out Point intersection)
    {
    }

    private static Winding GetWinding(nfloat θ0, nfloat θm, nfloat θ1)
    {
    }

    /// <summary>
    /// Splits the arc into one or two Y-monotonic <see cref = "Arc3Point"/> segments.
    /// </summary>
    /// <remarks>
    /// This method finds the angle θ where the Y value of the arc reaches an extremum.
    /// If this point lies within the arc, the arc is split at that angle into two Y-monotonic sub-arcs.
    /// If no vertical extremum is found within the arc range, the arc is already monotonic.
    /// </remarks>
    public MonotonicArc3Point SplitIntoYMonotonicCurves()
    {
    }

    private static nfloat NormalizeAngle(nfloat θ)
    {
    }

    private static bool AngleIsBetweenCW(nfloat θ, nfloat from, nfloat to)
    {
    }

    private static bool AngleIsBetweenCCW(nfloat θ, nfloat from, nfloat to)
    {
    }
}
```

### Curves2D/ArcEndpoint.cs

```csharp
using Xui.Core.Math2D;
using Xui.Core.Canvas;

namespace Xui.Core.Curves2D;
/// <summary>
/// Represents a circular or elliptical arc defined by radii, rotation, and endpoint parameters.
/// </summary>
/// <remarks>
/// This arc representation uses start and end points along with size and sweep flags.
/// It is commonly used in vector graphics where the arc shape is inferred from geometry and direction flags.
/// </remarks>
public readonly struct ArcEndpoint : ICurve
{
    /// <summary>The start point of the arc.</summary>
    public readonly Point Start;
    /// <summary>The end point of the arc.</summary>
    public readonly Point End;
    /// <summary>The horizontal radius of the arc.</summary>
    public readonly nfloat RadiusX;
    /// <summary>The vertical radius of the arc.</summary>
    public readonly nfloat RadiusY;
    /// <summary>The clockwise rotation (in radians) applied to the arc's ellipse relative to the X axis.</summary>
    public readonly nfloat Rotation;
    /// <summary>If true, the arc is the longer (greater than 180°) of the two possible arcs between points.</summary>
    public readonly bool LargeArc;
    /// <summary>The direction in which the arc is drawn (clockwise or counter-clockwise).</summary>
    public readonly Winding Winding;
    /// <summary>
    /// Initializes a new <see cref = "ArcEndpoint"/> with the given geometry and flags.
    /// </summary>
    /// <param name = "start">The start point of the arc.</param>
    /// <param name = "end">The end point of the arc.</param>
    /// <param name = "rx">The horizontal radius.</param>
    /// <param name = "ry">The vertical radius.</param>
    /// <param name = "rotation">The clockwise rotation in radians applied to the ellipse.</param>
    /// <param name = "largeArc">Whether the arc should be the larger of the two possible arcs.</param>
    /// <param name = "winding">The sweep direction (clockwise or counter-clockwise).</param>
    public ArcEndpoint(Point start, Point end, nfloat rx, nfloat ry, nfloat rotation, bool largeArc, Winding winding)
    {
    }

    /// <summary>
    /// Gets the interpolated point on the arc at the specified parameter <paramref name = "t"/>.
    /// </summary>
    /// <param name = "t">A normalized value from 0 to 1 representing the progression along the arc.</param>
    public Point this[nfloat t]
    {
        get
        {
        }
    }

    /// <summary>
    /// Computes the interpolated point on the arc at a given parameter <paramref name = "t"/>.
    /// </summary>
    /// <param name = "t">A normalized value in the range [0, 1].</param>
    public Point Lerp(nfloat t)
    {
    }

    /// <summary>
    /// Evaluates the arc at a normalized parameter <paramref name = "t"/>, returning the corresponding point.
    /// </summary>
    /// <param name = "t">A normalized value in the range [0, 1].</param>
    /// <returns>The corresponding point on the arc.</returns>
    public Point Evaluate(nfloat t)
    {
    }

    /// <summary>
    /// Computes the tangent vector at parameter <paramref name = "t"/> on the arc.
    /// </summary>
    /// <param name = "t">A normalized value in the range [0, 1].</param>
    /// <returns>The normalized tangent vector at the evaluated point on the arc.</returns>
    public Vector Tangent(nfloat t)
    {
    }

    /// <summary>
    /// Approximates the arc length using uniform sampling with 16 segments.
    /// </summary>
    /// <returns>The approximate total arc length.</returns>
    public nfloat Length()
    {
    }

    /// <summary>
    /// Approximates the arc length using recursive adaptive subdivision.
    /// </summary>
    /// <param name = "flatness">The maximum allowed error per segment for arc approximation.</param>
    /// <returns>The computed arc length within the specified flatness tolerance.</returns>
    public nfloat Length(nfloat flatness)
    {
    }

    /// <summary>
    /// Converts this endpoint-parameterized arc to a center-based <see cref = "Arc"/>.
    /// </summary>
    /// <returns>A new <see cref = "Arc"/> representing the same shape.</returns>
    public Arc ToCenterArc()
    {
    }

    /// <summary>
    /// Splits this arc into a series of <see cref = "Arc3Point"/> segments,
    /// each spanning ≤ 90° and aligned so that one edge is axis-aligned.
    /// </summary>
    /// <param name = "buffer">
    /// A span to receive the output segments. Must have space for up to 4 entries.
    /// </param>
    /// <param name = "count">
    /// The number of segments written to <paramref name = "buffer"/>.
    /// </param>
    /// <remarks>
    /// This method converts the arc into a center-based <see cref = "Arc"/> representation,
    /// then delegates to its <see cref = "Arc.ToArc3Segments"/> method. Each resulting
    /// <see cref = "Arc3Point"/> segment spans ≤ 90° and is safe for monotonic tessellation.
    /// </remarks>
    public void ToArc3Segments(Span<Arc3Point> buffer, out int count)
    {
    }

    private static nfloat AngleOnUnitEllipse(nfloat ux, nfloat uy, nfloat vx, nfloat vy)
    {
    }
}
```

### Curves2D/Bezier.cs

```csharp
using Xui.Core.Math2D;

namespace Xui.Core.Curves2D;
/// <summary>
/// Static helper methods for constructing Bézier curves.
/// </summary>
public static class Bezier
{
    /// <summary>
    /// Creates a linear Bézier curve from two points.
    /// </summary>
    public static LinearBezier Linear(Point p0, Point p1)
    {
    }

    /// <summary>
    /// Creates a quadratic Bézier curve from three control points.
    /// </summary>
    public static QuadraticBezier Quadratic(Point p0, Point p1, Point p2)
    {
    }

    /// <summary>
    /// Creates a cubic Bézier curve from four control points.
    /// </summary>
    public static CubicBezier Cubic(Point p0, Point p1, Point p2, Point p3)
    {
    }
}
```

### Curves2D/CubicBezier.cs

```csharp
using Xui.Core.Math2D;

namespace Xui.Core.Curves2D;
/// <summary>
/// Represents a cubic Bézier curve defined by four control points.
/// </summary>
/// <remarks>
/// A cubic Bézier curve provides smooth interpolation between <see cref = "P0"/> and <see cref = "P3"/>,
/// influenced by the control points <see cref = "P1"/> and <see cref = "P2"/>. This is commonly used in vector graphics,
/// animation timing, and layout paths.
/// </remarks>
public readonly struct CubicBezier : ICurve
{
    /// <summary>The starting point of the curve.</summary>
    public readonly Point P0;
    /// <summary>The first control point, influencing the curve near <see cref = "P0"/>.</summary>
    public readonly Point P1;
    /// <summary>The second control point, influencing the curve near <see cref = "P3"/>.</summary>
    public readonly Point P2;
    /// <summary>The ending point of the curve.</summary>
    public readonly Point P3;
    /// <summary>
    /// Initializes a new cubic Bézier curve with the specified control points.
    /// </summary>
    public CubicBezier(Point p0, Point p1, Point p2, Point p3)
    {
    }

    /// <summary>
    /// Computes the interpolated point on the curve at parameter <paramref name = "t"/> using De Casteljau’s algorithm.
    /// </summary>
    /// <param name = "t">A normalized parameter in the range [0, 1].</param>
    public Point Lerp(nfloat t)
    {
    }

    public QuadraticBezier QuadraticApproximation
    {
        get
        {
        }
    }

    /// <summary>
    /// Computes the tangent vector of the curve at parameter <paramref name = "t"/>.
    /// </summary>
    /// <param name = "t">A normalized parameter in the range [0, 1].</param>
    public Vector Tangent(nfloat t)
    {
    }

    /// <summary>
    /// Approximates the arc length of the curve using uniform sampling with 16 segments.
    /// </summary>
    public nfloat Length()
    {
    }

    /// <summary>
    /// Approximates the arc length of the curve using recursive adaptive subdivision.
    /// </summary>
    /// <param name = "precision">
    /// The tolerance value used to determine when a segment is flat enough to stop subdividing.
    /// Smaller values result in more accurate results but require more computation.
    /// </param>
    public nfloat Length(nfloat precision)
    {
    }

    private static nfloat Length(Point p0, Point p1, Point p2, Point p3, nfloat tolerance)
    {
    }

    /// <summary>
    /// Gets the interpolated point on the curve at the specified parameter.
    /// </summary>
    /// <param name = "t">A normalized parameter in the range [0, 1].</param>
    public Point this[nfloat t]
    {
        get
        {
        }
    }

    /// <summary>
    /// Implicitly converts a tuple of four points into a <see cref = "CubicBezier"/>.
    /// </summary>
    /// <param name = "value">A tuple representing the start point, two control points, and end point.</param>
    public static implicit operator CubicBezier((Point p0, Point p1, Point p2, Point p3) value)
    {
    }

    /// <summary>
    /// Implicitly converts a <see cref = "QuadraticBezier"/> to a <see cref = "CubicBezier"/>
    /// using interpolation for smooth parameterization.
    /// </summary>
    /// <remarks>
    /// This conversion evaluates the quadratic Bézier at fixed steps (1⁄3 and 2⁄3) to generate
    /// the internal control points of the cubic, resulting in a visually smooth upgrade path.
    /// </remarks>
    public static implicit operator CubicBezier(QuadraticBezier quadratic)
    {
    }

    /// <summary>
    /// Implicitly converts this cubic Catmull–Rom spline to a <see cref = "CubicBezier"/>.
    /// </summary>
    /// <remarks>
    /// Uses the canonical Catmull–Rom to Bézier mapping, interpolating between P1 and P2.
    /// </remarks>
    public static implicit operator CubicBezier(CubicSpline spline)
    {
    }

    /// <summary>
    /// Returns a new <see cref = "CubicBezier"/> curve where the control points <c>P1</c> and <c>P2</c>
    /// are adjusted based on a drag gesture at a given parametric position <paramref name = "t"/>.
    /// </summary>
    /// <param name = "t">
    /// A normalized value (between 0 and 1) representing the position on the curve where the drag occurs.
    /// </param>
    /// <param name = "delta">
    /// The drag vector representing how far the user dragged the point at <paramref name = "t"/>.
    /// </param>
    /// <returns>
    /// A new <see cref = "CubicBezier"/> with <c>P0</c> and <c>P3</c> unchanged,
    /// and <c>P1</c> and <c>P2</c> adjusted proportionally to the drag direction and distance.
    /// </returns>
    /// <remarks>
    /// The influence of the drag on <c>P1</c> and <c>P2</c> is weighted quadratically based on their
    /// distance from <paramref name = "t"/> along the curve:
    /// <c>P1</c> is influenced by <c>(1 - t)²</c> and <c>P2</c> by <c>t²</c>.
    /// This preserves the endpoints while allowing intuitive manipulation of the curve shape.
    /// </remarks>
    public CubicBezier Drag(nfloat t, Vector delta)
    {
    }

    /// <summary>
    /// Returns a new cubic Bézier curve with adjusted control points so the point on the curve
    /// nearest to <paramref name = "origin"/> is moved by <paramref name = "delta"/>.
    /// </summary>
    public CubicBezier DragAt(Point origin, Vector delta)
    {
    }

    /// <summary>
    /// Returns a new cubic Bézier curve with adjusted control points so the point on the curve
    /// nearest to <paramref name = "origin"/> is moved by <paramref name = "delta"/>.
    /// Uses recursive refinement based on the specified <paramref name = "precision"/>.
    /// </summary>
    public CubicBezier DragAt(Point origin, Vector delta, nfloat precision)
    {
    }

    /// <summary>
    /// Returns the parameter <c>t</c> ∈ [0, 1] at which the curve is closest to the specified <paramref name = "target"/> point.
    /// This version uses uniform sampling with 16 segments and is fast enough for interactive use.
    /// </summary>
    /// <param name = "target">The point to compare against the curve.</param>
    /// <returns>The approximate <c>t</c> value where the curve is closest to <paramref name = "target"/>.</returns>
    public nfloat ClosestT(Point target)
    {
    }

    /// <summary>
    /// Returns the parameter <c>t</c> ∈ [0, 1] at which the curve is closest to the specified <paramref name = "target"/> point,
    /// using recursive ternary subdivision until the difference in <c>t</c> range is less than <paramref name = "precision"/>.
    /// </summary>
    /// <param name = "target">The point to compare against the curve.</param>
    /// <param name = "precision">The minimum resolution for <c>t</c> convergence. Smaller values yield more accurate results.</param>
    /// <returns>The <c>t</c> value where the curve is closest to <paramref name = "target"/> within the specified <paramref name = "precision"/>.</returns>
    public nfloat ClosestT(Point target, nfloat precision)
    {
    }

    public MonotonicCubicBezier SplitIntoYMonotonicCurves()
    {
    }

    /// <summary>
    /// Subdivides this cubic Bézier curve at parameter t, returning two curves.
    /// </summary>
    public (CubicBezier, CubicBezier) Subdivide(nfloat t)
    {
    }

    private static nfloat ClosestTRecursive(CubicBezier curve, Point target, nfloat t0, nfloat t1, nfloat precision)
    {
    }

    /// <summary>
    /// Converts this cubic Bézier curve into a sequence of Y-monotonic quadratic Bézier segments,
    /// each approximating a portion of the original curve within the specified flatness precision.
    /// </summary>
    /// <param name = "buffer">
    /// A span to receive the quadratic segments. Must be large enough to hold the result.
    /// A typical size is between 4 and 32. Must be at least 3 to accommodate the initial monotonic split.
    /// </param>
    /// <param name = "precision">
    /// The maximum allowed distance between the original cubic curve and its quadratic approximation.
    /// Lower values yield more accurate approximations but may require more output segments.
    /// </param>
    /// <param name = "count">
    /// The number of segments written to <paramref name = "buffer"/>.
    /// </param>
    /// <remarks>
    /// This method uses an adaptive, flatness-aware subdivision strategy. It begins by splitting the
    /// cubic curve into up to 3 Y-monotonic segments. Each is then approximated with a quadratic Bézier,
    /// and the segments with the highest deviation are recursively subdivided until the total number of
    /// segments fits within <paramref name = "buffer"/> and all segments meet the precision requirement.
    /// 
    /// The output segments are returned in order, forming a continuous piecewise-curve that follows the
    /// original cubic. Suitable for SDF tessellation or scanline-based rasterization.
    /// </remarks>
    /// <exception cref = "ArgumentException">
    /// Thrown if <paramref name = "buffer"/> has fewer than 3 elements.
    /// </exception>
    public void ToQuadratics(Span<QuadraticBezier> buffer, nfloat precision, out int count)
    {
    }

    public struct SubcurveNode
    {
        public CubicBezier Segment;
        public QuadraticBezier QuadraticBezierApproximation;
        public nfloat Precision;
        public ushort NextIndex;
        /// <summary>
        /// Initializes a new node representing a quadratic approximation of a cubic Bézier segment in a linked list.
        /// </summary>
        /// <param name = "segment">The original cubic Bézier segment.</param>
        /// <param name = "nextIndex">Index of the next node in the chain. Use 0 if this is the last node.</param>
        public SubcurveNode(CubicBezier segment, ushort nextIndex = 0)
        {
        }
    }
}
```

### Curves2D/CubicSpline.cs

```csharp
using Xui.Core.Math2D;

namespace Xui.Core.Curves2D;
/// <summary>
/// Represents a 4-point spline segment using Catmull–Rom interpolation.
/// The curve interpolates smoothly between <see cref = "P1"/> and <see cref = "P2"/>.
/// </summary>
/// <remarks>
/// Catmull–Rom splines are useful for generating smooth curves through a sequence of points
/// with minimal configuration. This implementation produces a C¹-continuous cubic curve,
/// influenced by neighboring control points <see cref = "P0"/> and <see cref = "P3"/>.
/// The segment can also be converted into a <see cref = "CubicBezier"/> for drawing or compatibility.
/// </remarks>
public readonly struct CubicSpline : ICurve
{
    /// <summary>The control point before the start of the curve.</summary>
    public readonly Point P0;
    /// <summary>The starting point of the interpolated segment.</summary>
    public readonly Point P1;
    /// <summary>The ending point of the interpolated segment.</summary>
    public readonly Point P2;
    /// <summary>The control point after the end of the curve.</summary>
    public readonly Point P3;
    /// <summary>
    /// Initializes a new Catmull–Rom spline segment from four control points.
    /// </summary>
    public CubicSpline(Point p0, Point p1, Point p2, Point p3)
    {
    }

    /// <summary>
    /// Evaluates the point on the spline at the given parameter <paramref name = "t"/> using the Catmull–Rom formula.
    /// </summary>
    /// <param name = "t">A normalized parameter in the range [0, 1].</param>
    public Point Lerp(nfloat t)
    {
    }

    /// <summary>
    /// Computes the tangent vector at parameter <paramref name = "t"/> by differentiating the spline.
    /// </summary>
    /// <param name = "t">A normalized parameter in the range [0, 1].</param>
    public Vector Tangent(nfloat t)
    {
    }

    /// <summary>
    /// Approximates the arc length of the curve using uniform sampling with 16 segments.
    /// </summary>
    public nfloat Length()
    {
    }

    /// <summary>
    /// Approximates the arc length using recursive adaptive subdivision.
    /// </summary>
    /// <param name = "precision">The tolerance used to decide subdivision depth.</param>
    public nfloat Length(nfloat precision)
    {
    }

    private static nfloat Length(Point p0, Point p1, Point p2, Point p3, nfloat tolerance)
    {
    }

    /// <summary>
    /// Indexer-style access to evaluate the curve at a given parameter.
    /// </summary>
    public Point this[nfloat t]
    {
        get
        {
        }
    }

    /// <summary>
    /// Converts this Catmull–Rom spline segment to an equivalent <see cref = "CubicBezier"/>.
    /// The resulting curve interpolates between <see cref = "P1"/> and <see cref = "P2"/>.
    /// </summary>
    public static implicit operator CubicBezier(CubicSpline s)
    {
    }
}
```

### Curves2D/ICurve.cs

```csharp
using Xui.Core.Math2D;

namespace Xui.Core.Curves2D;
/// <summary>
/// Represents a common interface for evaluable 2D curves.
/// </summary>
public interface ICurve
{
    /// <summary>
    /// Evaluates the curve at the given normalized parameter t in [0, 1].
    /// </summary>
    Point Lerp(nfloat t);
    /// <summary>
    /// Computes the tangent vector at the specified t.
    /// </summary>
    Vector Tangent(nfloat t);
    /// <summary>
    /// Provides indexer access as an alias for <see cref = "Lerp"/>.
    /// </summary>
    Point this[nfloat t] { get; }

    /// <summary>
    /// Computes an approximate arc length using 16 steps.
    /// </summary>
    nfloat Length();
    /// <summary>
    /// Computes a refined approximation of arc length with a specified precision.
    /// </summary>
    nfloat Length(nfloat precision);
}
```

### Curves2D/LinearBezier.cs

```csharp
using Xui.Core.Math2D;

namespace Xui.Core.Curves2D;
/// <summary>
/// Represents a linear Bézier curve, equivalent to a straight line between two points.
/// </summary>
/// <remarks>
/// This is the simplest form of Bézier curve, defined by two points <see cref = "P0"/> and <see cref = "P1"/>.
/// It is often used for edges of polygonal paths or degenerate forms of higher-order Béziers.
/// </remarks>
public readonly struct LinearBezier : ICurve
{
    /// <summary>The start point of the curve.</summary>
    public readonly Point P0;
    /// <summary>The end point of the curve.</summary>
    public readonly Point P1;
    /// <summary>
    /// Initializes a new <see cref = "LinearBezier"/> curve with the given start and end points.
    /// </summary>
    /// <param name = "p0">The starting point.</param>
    /// <param name = "p1">The ending point.</param>
    public LinearBezier(Point p0, Point p1)
    {
    }

    /// <summary>
    /// Computes the point on the line at parameter <paramref name = "t"/> using linear interpolation.
    /// </summary>
    /// <param name = "t">A normalized parameter in the range [0, 1].</param>
    public Point Lerp(nfloat t)
    {
    }

    /// <summary>
    /// Returns the constant tangent vector of the line segment.
    /// </summary>
    /// <param name = "t">The curve parameter (ignored since the tangent is constant).</param>
    public Vector Tangent(nfloat t)
    {
    }

    /// <summary>
    /// Returns the exact length of the line segment.
    /// </summary>
    public nfloat Length()
    {
    }

    /// <summary>
    /// Returns the length of the line segment. This overload ignores the <paramref name = "precision"/> parameter.
    /// </summary>
    public nfloat Length(nfloat precision)
    {
    }

    /// <summary>
    /// Indexer-style access to the interpolated point at parameter <paramref name = "t"/>.
    /// </summary>
    public Point this[nfloat t]
    {
        get
        {
        }
    }

    /// <summary>
    /// Implicitly converts a tuple of two points into a <see cref = "LinearBezier"/>.
    /// </summary>
    public static implicit operator LinearBezier((Point p0, Point p1) value)
    {
    }

    /// <summary>
    /// Implicitly converts this <see cref = "LinearBezier"/> to a <see cref = "QuadraticBezier"/> curve.
    /// The intermediate point is set to the midpoint for visual smoothness.
    /// </summary>
    public static implicit operator QuadraticBezier(LinearBezier linear)
    {
    }

    /// <summary>
    /// Implicitly converts this <see cref = "LinearBezier"/> to a <see cref = "CubicBezier"/> curve.
    /// Intermediate control points are interpolated at 1/3 and 2/3 for smooth spacing.
    /// </summary>
    public static implicit operator CubicBezier(LinearBezier linear)
    {
    }

    /// <summary>
    /// Implicitly converts a <see cref = "LinearSpline"/> to a <see cref = "LinearBezier"/>.
    /// </summary>
    public static implicit operator LinearBezier(LinearSpline spline)
    {
    }
}
```

### Curves2D/LinearSpline.cs

```csharp
using Xui.Core.Math2D;

namespace Xui.Core.Curves2D;
/// <summary>
/// Represents a linear spline segment between two points.
/// </summary>
/// <remarks>
/// A linear spline defines a straight-line interpolation between <see cref = "P0"/> and <see cref = "P1"/>.
/// It is the simplest form of spline, useful for polyline paths, control handles, and degenerate Bézier cases.
/// </remarks>
public readonly struct LinearSpline : ICurve
{
    /// <summary>
    /// The starting point of the line segment.
    /// </summary>
    public readonly Point P0;
    /// <summary>
    /// The ending point of the line segment.
    /// </summary>
    public readonly Point P1;
    /// <summary>
    /// Initializes a new <see cref = "LinearSpline"/> segment between two points.
    /// </summary>
    /// <param name = "p0">The starting point of the spline.</param>
    /// <param name = "p1">The ending point of the spline.</param>
    public LinearSpline(Point p0, Point p1)
    {
    }

    /// <summary>
    /// Interpolates a point on the segment at the specified parameter.
    /// </summary>
    /// <param name = "t">A normalized parameter in the range [0, 1].</param>
    /// <returns>The interpolated point on the line.</returns>
    public Point Lerp(nfloat t)
    {
    }

    /// <summary>
    /// Gets the constant tangent vector of the line segment.
    /// </summary>
    /// <param name = "t">A normalized parameter (unused, since the tangent is constant).</param>
    public Vector Tangent(nfloat t)
    {
    }

    /// <summary>
    /// Evaluates the point on the spline at the specified parameter.
    /// </summary>
    public Point this[nfloat t]
    {
        get
        {
        }
    }

    /// <summary>
    /// Returns the length of the segment.
    /// </summary>
    public nfloat Length()
    {
    }

    /// <summary>
    /// Returns the length of the segment. Precision is ignored for linear splines.
    /// </summary>
    public nfloat Length(nfloat precision)
    {
    }

    /// <summary>
    /// Converts this linear spline to a <see cref = "LinearBezier"/>.
    /// </summary>
    public static implicit operator LinearBezier(LinearSpline s)
    {
    }
}
```

### Curves2D/MonotonicArc3Point.cs

```csharp
namespace Xui.Core.Curves2D;
/// <summary>
/// Represents one or two <see cref = "Arc3Point"/> segments that are each monotonic in the Y direction.
/// </summary>
public readonly struct MonotonicArc3Point
{
    /// <summary>The first monotonic segment.</summary>
    public readonly Arc3Point First;
    /// <summary>The optional second segment, if the arc was split.</summary>
    public readonly Arc3Point? Second;
    /// <summary>Creates a single-segment monotonic arc.</summary>
    public MonotonicArc3Point(Arc3Point only)
    {
    }

    /// <summary>Creates a two-segment Y-monotonic arc.</summary>
    public MonotonicArc3Point(Arc3Point first, Arc3Point second)
    {
    }

    /// <summary>True if the arc was split into two segments.</summary>
    public bool IsSplit
    {
        get
        {
        }
    }
}
```

### Curves2D/MonotonicCubicBezier.cs

```csharp
namespace Xui.Core.Curves2D;
public readonly struct MonotonicCubicBezier
{
    public readonly CubicBezier First;
    public readonly CubicBezier? Second;
    public readonly CubicBezier? Third;
    public MonotonicCubicBezier(CubicBezier first, CubicBezier? second = null, CubicBezier? third = null)
    {
    }
}
```

### Curves2D/MonotonicQuadraticBezier.cs

```csharp
namespace Xui.Core.Curves2D;
public readonly struct MonotonicQuadraticBezier
{
    public readonly QuadraticBezier First;
    public readonly QuadraticBezier? Second;
    public MonotonicQuadraticBezier(QuadraticBezier first, QuadraticBezier? second = null)
    {
    }
}
```

### Curves2D/QuadraticBezier.cs

```csharp
using Xui.Core.Math2D;

namespace Xui.Core.Curves2D;
/// <summary>
/// Represents a quadratic Bézier curve defined by three control points.
/// </summary>
/// <remarks>
/// A quadratic Bézier curve smoothly interpolates between <see cref = "P0"/> and <see cref = "P2"/>.
/// The curve is influenced by a single control point <see cref = "P1"/>.
/// Quadratic Béziers are widely used in vector graphics, font rendering, and easing functions.
/// </remarks>
public readonly struct QuadraticBezier : ICurve
{
    /// <summary>The starting point of the curve.</summary>
    public readonly Point P0;
    /// <summary>The control point that influences the curvature of the segment.</summary>
    public readonly Point P1;
    /// <summary>The ending point of the curve.</summary>
    public readonly Point P2;
    /// <summary>
    /// Initializes a new quadratic Bézier curve with the specified control points.
    /// </summary>
    public QuadraticBezier(Point p0, Point p1, Point p2)
    {
    }

    /// <summary>
    /// Evaluates the curve at parameter <paramref name = "t"/> using De Casteljau’s algorithm.
    /// </summary>
    /// <param name = "t">A normalized parameter in the range [0, 1].</param>
    /// <returns>The interpolated point on the curve.</returns>
    public Point Lerp(nfloat t)
    {
    }

    /// <summary>
    /// Computes the tangent vector at parameter <paramref name = "t"/>.
    /// </summary>
    /// <param name = "t">A normalized parameter in the range [0, 1].</param>
    /// <returns>The tangent vector at the specified point.</returns>
    public Vector Tangent(nfloat t)
    {
    }

    /// <summary>
    /// Approximates the arc length using uniform sampling over 16 segments.
    /// </summary>
    /// <returns>The approximate total length of the curve.</returns>
    public nfloat Length()
    {
    }

    /// <summary>
    /// Approximates the arc length using recursive adaptive subdivision.
    /// </summary>
    /// <param name = "precision">The maximum allowed error for curve flatness.</param>
    public nfloat Length(nfloat precision)
    {
    }

    private static nfloat Length(Point a, Point b, Point c, nfloat tolerance)
    {
    }

    /// <summary>
    /// Evaluates the point on the curve at the specified parameter.
    /// </summary>
    public Point this[nfloat t]
    {
        get
        {
        }
    }

    /// <summary>
    /// Converts a tuple of 3 points to a <see cref = "QuadraticBezier"/>.
    /// </summary>
    public static implicit operator QuadraticBezier((Point p0, Point p1, Point p2) value)
    {
    }

    /// <summary>
    /// Converts a <see cref = "QuadraticSpline"/> to a <see cref = "QuadraticBezier"/>.
    /// </summary>
    public static implicit operator QuadraticBezier(QuadraticSpline spline)
    {
    }

    /// <summary>
    /// Adjusts the control point to drag the curve at <paramref name = "t"/> by the given delta.
    /// Keeps <see cref = "P0"/> and <see cref = "P2"/> fixed.
    /// </summary>
    public QuadraticBezier Drag(nfloat t, Vector delta)
    {
    }

    /// <summary>
    /// Adjusts the control point so that the curve is moved near <paramref name = "origin"/> by <paramref name = "delta"/>.
    /// Uses coarse sampling to find the closest point.
    /// </summary>
    public QuadraticBezier DragAt(Point origin, Vector delta)
    {
    }

    /// <summary>
    /// Adjusts the control point so that the curve is moved near <paramref name = "origin"/> by <paramref name = "delta"/>.
    /// Uses adaptive precision to locate the closest point.
    /// </summary>
    public QuadraticBezier DragAt(Point origin, Vector delta, nfloat precision)
    {
    }

    /// <summary>
    /// Finds the <c>t</c> value where the curve is closest to <paramref name = "target"/>.
    /// Uses uniform sampling across 16 points.
    /// </summary>
    public nfloat ClosestT(Point target)
    {
    }

    /// <summary>
    /// Finds the <c>t</c> value where the curve is closest to <paramref name = "target"/>.
    /// Uses recursive ternary subdivision with the specified <paramref name = "precision"/>.
    /// </summary>
    public nfloat ClosestT(Point target, nfloat precision)
    {
    }

    /// <summary>
    /// Splits the current quadratic Bézier curve into two segments if it is not monotonic in the Y direction.
    /// </summary>
    /// <returns>
    /// A <see cref = "MonotonicQuadraticBezier"/> instance containing one or two sub-curves, each guaranteed to be monotonic in Y.
    /// If the curve is already Y-monotonic (i.e., has no vertical extrema within (0,1)), a single-segment result is returned.
    /// </returns>
    /// <remarks>
    /// This method analyzes the derivative of the Y component of the curve to determine if a vertical extremum occurs
    /// within the curve's parameter domain. If a critical point is found in (0,1), the curve is split at that point to ensure
    /// each resulting segment is Y-monotonic. This is useful for scanline rasterization, tessellation, or other geometry processing
    /// that requires monotonic segments.
    /// </remarks>
    public MonotonicQuadraticBezier SplitIntoYMonotonicCurves()
    {
    }

    private static nfloat ClosestTRecursive(QuadraticBezier curve, Point target, nfloat t0, nfloat t1, nfloat precision)
    {
    }

    /// <summary>
    /// Subdivides this quadratic Bézier curve at parameter t, returning two curves.
    /// </summary>
    public (QuadraticBezier, QuadraticBezier) Subdivide(nfloat t)
    {
    }
}
```

### Curves2D/QuadraticSpline.cs

```csharp
using Xui.Core.Math2D;

namespace Xui.Core.Curves2D;
/// <summary>
/// Represents a 3-point quadratic spline segment (parabolic arc) defined by a start point, control point, and end point.
/// </summary>
/// <remarks>
/// This spline uses the De Casteljau algorithm for interpolation and arc length approximation.
/// The curve passes through <see cref = "P0"/> and <see cref = "P2"/>, with <see cref = "P1"/> acting as the control point that defines curvature.
/// </remarks>
public readonly struct QuadraticSpline : ICurve
{
    /// <summary>The start point of the spline.</summary>
    public readonly Point P0;
    /// <summary>The control point that defines the curvature of the arc.</summary>
    public readonly Point P1;
    /// <summary>The end point of the spline.</summary>
    public readonly Point P2;
    /// <summary>
    /// Initializes a new <see cref = "QuadraticSpline"/> with the given start, control, and end points.
    /// </summary>
    public QuadraticSpline(Point p0, Point p1, Point p2)
    {
    }

    /// <summary>
    /// Computes the point on the spline at parameter <paramref name = "t"/> using De Casteljau interpolation.
    /// </summary>
    /// <param name = "t">A normalized parameter between 0 and 1.</param>
    public Point Lerp(nfloat t)
    {
    }

    /// <summary>
    /// Computes the tangent vector at parameter <paramref name = "t"/> on the spline.
    /// </summary>
    /// <param name = "t">A normalized parameter between 0 and 1.</param>
    public Vector Tangent(nfloat t)
    {
    }

    /// <summary>
    /// Approximates the total arc length of the spline using a fixed 16-sample De Casteljau subdivision.
    /// </summary>
    public nfloat Length()
    {
    }

    /// <summary>
    /// Approximates the arc length with adaptive subdivision using the specified precision tolerance.
    /// </summary>
    /// <param name = "precision">The maximum allowed error in the approximation.</param>
    public nfloat Length(nfloat precision)
    {
    }

    private static nfloat Length(Point a, Point b, Point c, nfloat tolerance)
    {
    }

    /// <summary>
    /// Evaluates the point on the spline at parameter <paramref name = "t"/>.
    /// </summary>
    public Point this[nfloat t]
    {
        get
        {
        }
    }

    /// <summary>
    /// Converts this spline to a <see cref = "QuadraticBezier"/> curve with the same control points.
    /// </summary>
    public static implicit operator QuadraticBezier(QuadraticSpline s)
    {
    }
}
```

### Curves2D/Segment.cs

```csharp
using Xui.Core.Math2D;

namespace Xui.Core.Curves2D;
/// <summary>
/// Represents a straight line segment from <see cref = "P0"/> to <see cref = "P1"/>.
/// Implements the <see cref = "ICurve"/> interface for compatibility with other curve types.
/// </summary>
public readonly struct Segment : ICurve
{
    /// <summary>The starting point of the line segment.</summary>
    public readonly Point P0;
    /// <summary>The ending point of the line segment.</summary>
    public readonly Point P1;
    /// <summary>Initializes a new line from <paramref name = "p0"/> to <paramref name = "p1"/>.</summary>
    public Segment(Point p0, Point p1)
    {
    }

    /// <inheritdoc/>
    public Point Lerp(nfloat t)
    {
    }

    /// <inheritdoc/>
    public Vector Tangent(nfloat t)
    {
    }

    /// <inheritdoc/>
    public Point this[nfloat t]
    {
        get
        {
        }
    }

    /// <inheritdoc/>
    public nfloat Length()
    {
    }

    /// <inheritdoc/>
    public nfloat Length(nfloat precision)
    {
    }

    /// <summary>
    /// Converts a tuple to a <see cref = "Segment"/>.
    /// </summary>
    public static implicit operator Segment((Point p0, Point p1) value)
    {
    }
}
```

### Curves2D/Spline.cs

```csharp
using Xui.Core.Math2D;

namespace Xui.Core.Curves2D;
/// <summary>
/// Provides factory methods for creating spline segments of various degrees.
/// </summary>
public static class Spline
{
    /// <summary>
    /// Creates a linear spline (degree 1) between two points.
    /// </summary>
    public static LinearSpline Linear(Point p0, Point p1)
    {
    }

    /// <summary>
    /// Creates a quadratic spline (degree 2) between three points.
    /// </summary>
    public static QuadraticSpline Quadratic(Point p0, Point p1, Point p2)
    {
    }

    /// <summary>
    /// Creates a cubic spline (degree 3) using Catmull–Rom interpolation through P1 and P2.
    /// </summary>
    /// <remarks>
    /// This produces a smooth segment influenced by surrounding control points <paramref name = "p0"/> and <paramref name = "p3"/>.
    /// </remarks>
    public static CubicSpline Cubic(Point p0, Point p1, Point p2, Point p3)
    {
    }
}
```

### GlobalUsings.cs

```csharp
#pragma warning disable CS8981
global using nfloat = System.Runtime.InteropServices.NFloat;
#pragma warning restore
global using System.Diagnostics;
global using System.ComponentModel;
global using static Xui.Core.Math2D.Constants;
```

### Math2D/AffineTransform.cs

```csharp
using System.Diagnostics;

namespace Xui.Core.Math2D;
/// <summary>
/// Represents a 2D affine transformation matrix in the form:
/// <code>
/// | A C Tx |
/// | B D Ty |
/// | 0 0  1 |
/// </code>
/// Used for rotating, scaling, translating, and skewing 2D vectors or points.
/// </summary>
/// <remarks>
/// Transformation is applied as:
/// <code>
/// | x' |   | A C Tx |   | x |
/// | y' | = | B D Ty | * | y |
/// | 1  |   | 0 0  1 |   | 1 |
/// </code>
/// </remarks>
public struct AffineTransform
{
    /// <summary>The matrix component in the first row, first column (scale X / rotate).</summary>
    public nfloat A;
    /// <summary>The matrix component in the second row, first column (shear Y / rotate).</summary>
    public nfloat B;
    /// <summary>The matrix component in the first row, second column (shear X / rotate).</summary>
    public nfloat C;
    /// <summary>The matrix component in the second row, second column (scale Y / rotate).</summary>
    public nfloat D;
    /// <summary>The translation component along the X axis.</summary>
    public nfloat Tx;
    /// <summary>The translation component along the Y axis.</summary>
    public nfloat Ty;
    /// <summary>The identity transform (no scale, rotation, or translation).</summary>
    public static readonly AffineTransform Identity = new AffineTransform(1, 0, 0, 1, 0, 0);
    /// <summary>Returns <c>true</c> if the current transform is the identity matrix.</summary>
    public bool IsIdentity
    {
        get
        {
        }
    }

    /// <summary>
    /// Returns the inverse of this affine transform.
    /// </summary>
    /// <remarks>
    /// This operation assumes the transform is invertible. Division by a zero determinant will cause a runtime exception.
    /// </remarks>
    public AffineTransform Inverse
    {
        [DebuggerStepThrough]
        get
        {
        }
    }

    /// <summary>
    /// Returns the determinant of the 2×2 linear portion of the matrix.
    /// </summary>
    /// <remarks>
    /// This value determines if the matrix is invertible. A value of 0 means the matrix cannot be inverted.
    /// </remarks>
    public nfloat Determinant
    {
        [DebuggerStepThrough]
        get
        {
        }
    }

    /// <summary>
    /// Creates a rotation transform around the origin using the given angle in radians.
    /// </summary>
    [DebuggerStepThrough]
    public static AffineTransform Rotate(nfloat angle)
    {
    }

    /// <summary>
    /// Creates a translation transform using the specified offset vector.
    /// </summary>
    [DebuggerStepThrough]
    public static AffineTransform Translate(Vector v)
    {
    }

    /// <summary>
    /// Creates a scaling transform using the specified scale vector.
    /// </summary>
    [DebuggerStepThrough]
    public static AffineTransform Scale(Vector v)
    {
    }

    /// <summary>
    /// Creates a shear (skew) transformation along the X and Y axes.
    /// </summary>
    /// <param name = "shearX">The shear factor in the X direction.</param>
    /// <param name = "shearY">The shear factor in the Y direction.</param>
    [DebuggerStepThrough]
    public static AffineTransform Shear(nfloat shearX, nfloat shearY)
    {
    }

    /// <summary>
    /// Constructs an affine transform with the specified matrix coefficients.
    /// </summary>
    [DebuggerStepThrough]
    public AffineTransform(nfloat a, nfloat b, nfloat c, nfloat d, nfloat tx, nfloat ty)
    {
    }

    /// <summary>
    /// Applies only the linear portion (scale, rotation, shear) of this transform to a <see cref = "Vector"/>.
    /// </summary>
    /// <remarks>
    /// Translation is not applied. Use this to transform directions, movement deltas, or normals.
    /// </remarks>
    /// <param name = "t">The affine transform.</param>
    /// <param name = "v">The vector to transform.</param>
    /// <returns>A new <see cref = "Vector"/> with the linear transform applied.</returns>
    [DebuggerStepThrough]
    public static Vector operator *(AffineTransform t, Vector v)
    {
    }

    /// <summary>
    /// Applies this transform to a <see cref = "Point"/>, including translation.
    /// </summary>
    /// <remarks>
    /// This operation applies the full affine matrix, including scale, rotation, shear, and translation.
    /// Use this when transforming coordinates in space (e.g., an element's position).
    /// </remarks>
    /// <param name = "t">The affine transform.</param>
    /// <param name = "p">The point to transform.</param>
    /// <returns>A new <see cref = "Point"/> with the transform applied.</returns>
    [DebuggerStepThrough]
    public static Point operator *(AffineTransform t, Point p)
    {
    }

    /// <summary>
    /// Composes two affine transforms using matrix multiplication.
    /// </summary>
    /// <returns>A new transform representing the application of <paramref name = "rhs"/> followed by <paramref name = "lhs"/>.</returns>
    [DebuggerStepThrough]
    public static AffineTransform operator *(AffineTransform lhs, AffineTransform rhs)
    {
    }

    /// <inheritdoc/>
    [DebuggerStepThrough]
    public override string ToString()
    {
    }
}
```

### Math2D/Constants.cs

```csharp
namespace Xui.Core.Math2D
{
    /// <summary>
    /// Common mathematical constants used throughout 2D geometry and curve computations.
    /// </summary>
    public static class Constants
    {
        /// <summary>π (pi): Ratio of a circle’s circumference to its diameter.</summary>
        public static readonly nfloat π = nfloat.Pi;
        /// <summary>τ (tau): One full circle in radians (2π).</summary>
        public static readonly nfloat τ = nfloat.Pi * 2;
        /// <summary>ε (epsilon): A small value used for numerical stability checks.</summary>
        public static readonly nfloat ε = 1e-5f;
        /// <summary>√2 (square root of 2): Diagonal of a unit square.</summary>
        public static readonly nfloat sqrt2 = (nfloat)Math.Sqrt(2);
        /// <summary>ϕ (phi): The golden ratio (≈ 1.618).</summary>
        public static readonly nfloat ϕ = (1 + nfloat.Sqrt(5)) / 2;
    }
}
```

### Math2D/Frame.cs

```csharp
namespace Xui.Core.Math2D;
/// <summary>
/// Represents thickness values for each edge of a rectangular element.
/// </summary>
/// <remarks>
/// A <see cref = "Frame"/> defines spacing from the left, top, right, and bottom edges.
/// It is typically used for layout constructs such as <c>Margin</c>, <c>Padding</c>, or <c>BorderWidth</c>.
/// Unlike <see cref = "Rect"/>, it does not represent position or size—only edge offsets.
/// </remarks>
public struct Frame
{
    /// <summary>The thickness of the top edge.</summary>
    public nfloat Top;
    /// <summary>The thickness of the right edge.</summary>
    public nfloat Right;
    /// <summary>The thickness of the bottom edge.</summary>
    public nfloat Bottom;
    /// <summary>The thickness of the left edge.</summary>
    public nfloat Left;
    /// <summary>
    /// Returns <c>true</c> if all four edges have equal thickness.
    /// </summary>
    public bool IsUniform
    {
        get
        {
        }
    }

    /// <summary>
    /// Returns true if all sides (Left, Top, Right, Bottom) are zero.
    /// </summary>
    public readonly bool IsZero
    {
        get
        {
        }
    }

    /// <summary>
    /// Gets the total horizontal edge thickness (left + right).
    /// </summary>
    public nfloat TotalWidth
    {
        get
        {
        }
    }

    /// <summary>
    /// Gets the total vertical edge thickness (top + bottom).
    /// </summary>
    public nfloat TotalHeight
    {
        get
        {
        }
    }

    /// <summary>
    /// A frame with all edge values set to zero.
    /// </summary>
    public static readonly Frame Zero = new Frame();
    /// <summary>
    /// Initializes a new <see cref = "Frame"/> with all edges set to zero.
    /// </summary>
    [DebuggerStepThrough]
    public Frame()
    {
    }

    /// <summary>
    /// Initializes a new <see cref = "Frame"/> with the specified edge thicknesses.
    /// </summary>
    /// <param name = "top">Top edge thickness.</param>
    /// <param name = "right">Right edge thickness.</param>
    /// <param name = "bottom">Bottom edge thickness.</param>
    /// <param name = "left">Left edge thickness.</param>
    [DebuggerStepThrough]
    public Frame(nfloat top, nfloat right, nfloat bottom, nfloat left)
    {
    }

    /// <summary>
    /// Converts a 4-tuple (top, right, bottom, left) to a <see cref = "Frame"/>.
    /// </summary>
    [DebuggerStepThrough]
    public static implicit operator Frame((nfloat top, nfloat right, nfloat bottom, nfloat left) value)
    {
    }

    /// <summary>
    /// Converts a 2-tuple (horizontal, vertical) to a <see cref = "Frame"/>.
    /// Horizontal is applied to left and right; vertical to top and bottom.
    /// </summary>
    [DebuggerStepThrough]
    public static implicit operator Frame((nfloat horizontal, nfloat vertical) value)
    {
    }

    /// <summary>
    /// Converts a scalar value into a uniform <see cref = "Frame"/> for all sides.
    /// </summary>
    [DebuggerStepThrough]
    public static implicit operator Frame(nfloat value)
    {
    }

    /// <summary>
    /// Implicitly converts an <see cref = "int "/> to a <see cref = "Frame"/>,
    /// applying the same value to all four sides (Left, Top, Right, Bottom).
    /// </summary>
    /// <param name = "value">The integer value to apply uniformly to all sides.</param>
    /// <returns>A <see cref = "Frame"/> with all sides set to <paramref name = "value"/>.</returns>
    [DebuggerStepThrough]
    public static implicit operator Frame(int value)
    {
    }

    /// <summary>
    /// Implicitly converts a <see cref = "double "/> to a <see cref = "Frame"/>,
    /// applying the same value to all four sides (Left, Top, Right, Bottom).
    /// </summary>
    /// <param name = "value">The double value to apply uniformly to all sides.</param>
    /// <returns>A <see cref = "Frame"/> with all sides set to <paramref name = "value"/>.</returns>
    [DebuggerStepThrough]
    public static implicit operator Frame(double value)
    {
    }

    /// <summary>
    /// Implicitly converts a <see cref = "float "/> to a <see cref = "Frame"/>,
    /// applying the same value to all four sides (Left, Top, Right, Bottom).
    /// </summary>
    /// <param name = "value">The float value to apply uniformly to all sides.</param>
    /// <returns>A <see cref = "Frame"/> with all sides set to <paramref name = "value"/>.</returns>
    [DebuggerStepThrough]
    public static implicit operator Frame(float value)
    {
    }

    /// <summary>
    /// Scales all edges of the frame by the specified scalar.
    /// </summary>
    [DebuggerStepThrough]
    public static Frame operator *(Frame lhs, nfloat rhs)
    {
    }

    /// <summary>
    /// Adds the corresponding edge thicknesses of two frames.
    /// </summary>
    [DebuggerStepThrough]
    public static Frame operator +(Frame lhs, Frame rhs)
    {
    }

    /// <summary>
    /// Subtracts the corresponding edge thicknesses of two frames.
    /// </summary>
    [DebuggerStepThrough]
    public static Frame operator -(Frame lhs, Frame rhs)
    {
    }

    /// <summary>
    /// Returns a frame containing the maximum value for each edge from two frames.
    /// </summary>
    [DebuggerStepThrough]
    public static Frame Max(Frame lhs, Frame rhs)
    {
    }

    /// <summary>
    /// Returns a frame containing the minimum value for each edge from two frames.
    /// </summary>
    [DebuggerStepThrough]
    public static Frame Min(Frame lhs, Frame rhs)
    {
    }

    /// <inheritdoc/>
    [DebuggerStepThrough]
    public override string ToString()
    {
    }

    /// <summary>
    /// Returns <c>true</c> if all edge values match exactly.
    /// </summary>
    public static bool operator ==(Frame left, Frame right)
    {
    }

    /// <summary>
    /// Returns <c>true</c> if any edge value differs.
    /// </summary>
    public static bool operator !=(Frame lhs, Frame rhs)
    {
    }

    /// <inheritdoc/>
    public override bool Equals(object? obj)
    {
    }

    /// <inheritdoc/>
    public override int GetHashCode()
    {
    }
}
```

### Math2D/Point.cs

```csharp
using System.Runtime.CompilerServices;

namespace Xui.Core.Math2D;
/// <summary>
/// Represents a point in 2D space, defined by its <see cref = "X"/> and <see cref = "Y"/> coordinates.
/// </summary>
public struct Point
{
    /// <summary>The X-coordinate of the point.</summary>
    public nfloat X;
    /// <summary>The Y-coordinate of the point.</summary>
    public nfloat Y;
    /// <summary>A point at the origin (0, 0).</summary>
    public static readonly Point Zero = (0, 0);
    /// <summary>
    /// A point at (1, 1), useful for normalized coordinates or vector math.
    /// </summary>
    public static readonly Point One = (1, 1);
    /// <summary>
    /// Initializes a new point with the specified coordinates.
    /// </summary>
    [DebuggerStepThrough]
    public Point(nfloat x, nfloat y)
    {
    }

    /// <summary>
    /// Linearly interpolates between two points by <paramref name = "step"/>.
    /// </summary>
    /// <param name = "start">The start point.</param>
    /// <param name = "end">The end point.</param>
    /// <param name = "step">A value from 0 to 1 indicating the interpolation position.</param>
    [DebuggerStepThrough]
    public static Point Lerp(Point start, Point end, nfloat step)
    {
    }

    /// <summary>
    /// Returns the Euclidean distance between two points.
    /// </summary>
    [DebuggerStepThrough]
    public static nfloat Distance(Point a, Point b)
    {
    }

    /// <summary>
    /// Returns the squared Euclidean distance between two points (no square root).
    /// Useful for performance comparisons or ordering by proximity.
    /// </summary>
    [DebuggerStepThrough]
    public static nfloat SquaredDistance(Point a, Point b)
    {
    }

    /// <summary>
    /// Returns the Manhattan (taxicab) distance between two points.
    /// </summary>
    /// <remarks>
    /// The sum of the absolute horizontal and vertical distances. 
    /// Often used in grid-based or discrete movement systems.
    /// </remarks>
    [DebuggerStepThrough]
    public static nfloat TaxicabDistance(Point a, Point b)
    {
    }

    /// <summary>
    /// Returns the vector difference from one point to another.
    /// </summary>
    [DebuggerStepThrough]
    public static Vector operator -(Point lhs, Point rhs)
    {
    }

    /// <summary>
    /// Offsets a point by a vector.
    /// </summary>
    [DebuggerStepThrough]
    public static Point operator +(Point lhs, Vector rhs)
    {
    }

    /// <summary>
    /// Subtracts a vector from a point, offsetting it in the opposite direction.
    /// </summary>
    [DebuggerStepThrough]
    public static Point operator -(Point lhs, Vector rhs)
    {
    }

    /// <summary>
    /// Converts a tuple to a point.
    /// </summary>
    [DebuggerStepThrough]
    public static implicit operator Point(ValueTuple<nfloat, nfloat> tuple)
    {
    }

    /// <summary>
    /// Explicitly converts a vector to a point (drops directional semantics).
    /// </summary>
    [DebuggerStepThrough]
    public static explicit operator Point(Vector vector)
    {
    }

    /// <summary>
    /// Determines whether two points have equal coordinates.
    /// </summary>
    [DebuggerStepThrough]
    public static bool operator ==(Point lhs, Point rhs)
    {
    }

    /// <summary>
    /// Determines whether two points differ in any coordinate.
    /// </summary>
    [DebuggerStepThrough]
    public static bool operator !=(Point lhs, Point rhs)
    {
    }

    /// <summary>
    /// Apply an uniform scale to a point.
    /// </summary>
    [DebuggerStepThrough]
    public static Point operator *(Point v, nfloat f)
    {
    }

    /// <inheritdoc/>
    public override string ToString()
    {
    }

    /// <inheritdoc/>
    public override bool Equals(object? obj)
    {
    }

    /// <summary>
    /// Returns true if this point has the same coordinates as another.
    /// </summary>
    public bool Equals(Point other)
    {
    }

    /// <inheritdoc/>
    public override int GetHashCode()
    {
    }
}
```

### Math2D/Rect.cs

```csharp
using Xui.Core.Set;

namespace Xui.Core.Math2D;
/// <summary>
/// Represents a rectangle in 2D space, defined by its origin (<see cref = "X"/>, <see cref = "Y"/>)
/// and its dimensions (<see cref = "Width"/>, <see cref = "Height"/>).
/// </summary>
/// <remarks>
/// Unlike <see cref = "Frame"/>, which represents edge thicknesses around a box, 
/// <see cref = "Rect"/> defines a positioned, sized area used for layout and rendering.
/// </remarks>
public struct Rect : INonEnumerableSet<Point>
{
    /// <summary>The X-coordinate of the rectangle’s top-left corner.</summary>
    public nfloat X;
    /// <summary>The Y-coordinate of the rectangle’s top-left corner.</summary>
    public nfloat Y;
    /// <summary>The width of the rectangle.</summary>
    public nfloat Width;
    /// <summary>The height of the rectangle.</summary>
    public nfloat Height;
    /// <summary>
    /// Creates a new <see cref = "Rect"/> from position and size.
    /// </summary>
    [DebuggerStepThrough]
    public Rect(nfloat x, nfloat y, nfloat width, nfloat height)
    {
    }

    /// <summary>
    /// Creates a new <see cref = "Rect"/> from a top-left point and a size.
    /// </summary>
    [DebuggerStepThrough]
    public Rect(Point topLeft, Size size)
    {
    }

    /// <summary>
    /// Creates a new <see cref = "Rect"/> from a top-left vector and a size.
    /// </summary>
    [DebuggerStepThrough]
    public Rect(Vector topLeft, Size size)
    {
    }

    /// <summary>Returns the top-left point of the rectangle.</summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public Point TopLeft
    {
        get
        {
        }
    }

    /// <summary>Returns the top-center point of the rectangle.</summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public Point TopCenter
    {
        get
        {
        }
    }

    /// <summary>Returns the top-right point of the rectangle.</summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public Point TopRight
    {
        get
        {
        }
    }

    /// <summary>Returns the bottom-right point of the rectangle.</summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public Point BottomRight
    {
        get
        {
        }
    }

    /// <summary>Returns the bottom-left point of the rectangle.</summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public Point BottomLeft
    {
        get
        {
        }
    }

    /// <summary>Returns the X-coordinate of the left edge.</summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public nfloat Left
    {
        get
        {
        }
    }

    /// <summary>Returns the Y-coordinate of the top edge.</summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public nfloat Top
    {
        get
        {
        }
    }

    /// <summary>Returns the X-coordinate of the right edge.</summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public nfloat Right
    {
        get
        {
        }
    }

    /// <summary>Returns the Y-coordinate of the bottom edge.</summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public nfloat Bottom
    {
        get
        {
        }
    }

    /// <summary>
    /// Returns the center point of the rectangle.
    /// </summary>
    public Point Center
    {
        get
        {
        }
    }

    /// <summary>
    /// Gets or sets the size of the rectangle.
    /// </summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public Size Size
    {
        get
        {
        }

        set
        {
        }
    }

    /// <summary>
    /// Returns a new rectangle that is inset by the same amount on all four sides.
    /// </summary>
    [DebuggerStepThrough]
    public Rect Inset(nfloat inset)
    {
    }

    /// <summary>
    /// Returns true if the specified <paramref name = "point"/> lies within this rectangle.
    /// </summary>
    [DebuggerStepThrough]
    public bool Contains(Point point)
    {
    }

    /// <summary>
    /// Returns a rectangle expanded horizontally and vertically by the specified amounts.
    /// </summary>
    [DebuggerStepThrough]
    public Rect Expand(nfloat h, nfloat v)
    {
    }

    /// <summary>
    /// Returns a rectangle expanded uniformly in all directions.
    /// </summary>
    [DebuggerStepThrough]
    public Rect Expand(nfloat expand)
    {
    }

    /// <summary>
    /// Implicit conversion from a 4-tuple to a rectangle.
    /// </summary>
    [DebuggerStepThrough]
    public static implicit operator Rect(ValueTuple<nfloat, nfloat, nfloat, nfloat> tuple)
    {
    }

    /// <summary>
    /// Returns a rectangle expanded outward by a <see cref = "Frame"/>.
    /// </summary>
    [DebuggerStepThrough]
    public static Rect operator +(Rect rect, Frame frame)
    {
    }

    /// <summary>
    /// Returns a rectangle inset inward by a <see cref = "Frame"/>.
    /// </summary>
    [DebuggerStepThrough]
    public static Rect operator -(Rect rect, Frame frame)
    {
    }

    /// <summary>
    /// Returns true if two rectangles are equal in position and size.
    /// </summary>
    [DebuggerStepThrough]
    public static bool operator ==(Rect lhs, Rect rhs)
    {
    }

    /// <summary>
    /// Returns true if any of the rectangle fields are different.
    /// </summary>
    [DebuggerStepThrough]
    public static bool operator !=(Rect lhs, Rect rhs)
    {
    }

    /// <inheritdoc/>
    public override bool Equals(object? obj)
    {
    }

    /// <summary>
    /// Returns true if this rectangle is equal to another rectangle in position and size.
    /// </summary>
    public bool Equals(Rect other)
    {
    }

    /// <inheritdoc/>
    public override int GetHashCode()
    {
    }

    /// <inheritdoc/>
    [DebuggerStepThrough]
    public override string ToString()
    {
    }
}
```

### Math2D/Size.cs

```csharp
namespace Xui.Core.Math2D;
/// <summary>
/// Represents a two-dimensional size with <see cref = "Width"/> and <see cref = "Height"/> components.
/// </summary>
/// <remarks>
/// A <see cref = "Size"/> specifies how much space an element occupies, without defining its position.
/// It is commonly used in layout systems to describe constraints or measured results.
/// </remarks>
public struct Size
{
    /// <summary>The width of the element.</summary>
    public nfloat Width;
    /// <summary>The height of the element.</summary>
    public nfloat Height;
    /// <summary>A size with zero width and height.</summary>
    public static readonly Size Empty = new Size(0, 0);
    /// <summary>A size with infinite width and height.</summary>
    public static readonly Size Infinity = new Size(nfloat.PositiveInfinity, nfloat.PositiveInfinity);
    /// <summary>
    /// Initializes a new instance of the <see cref = "Size"/> struct with zero dimensions.
    /// </summary>
    [DebuggerStepThrough]
    public Size()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref = "Size"/> struct with the given width and height.
    /// </summary>
    /// <param name = "width">The width of the size.</param>
    /// <param name = "height">The height of the size.</param>
    [DebuggerStepThrough]
    public Size(nfloat width, nfloat height)
    {
    }

    /// <summary>
    /// Implicitly converts a tuple to a <see cref = "Size"/>.
    /// </summary>
    [DebuggerStepThrough]
    public static implicit operator Size(ValueTuple<nfloat, nfloat> tuple)
    {
    }

    /// <summary>
    /// Returns the size of a square.
    /// </summary>
    public static implicit operator Size(int uniform)
    {
    }

    /// <summary>
    /// Explicitly converts a <see cref = "Vector"/> to a <see cref = "Size"/>.
    /// </summary>
    [DebuggerStepThrough]
    public static explicit operator Size(Vector vector)
    {
    }

    /// <summary>
    /// Adds edge spacing from a <see cref = "Frame"/> to a <see cref = "Size"/>, increasing its dimensions.
    /// </summary>
    [DebuggerStepThrough]
    public static Size operator +(Size size, Frame frame)
    {
    }

    /// <summary>
    /// Subtracts edge spacing from a <see cref = "Size"/>, reducing its dimensions.
    /// </summary>
    [DebuggerStepThrough]
    public static Size operator -(Size size, Frame frame)
    {
    }

    /// <summary>
    /// Adds two sizes together component-wise.
    /// </summary>
    [DebuggerStepThrough]
    public static Size operator +(Size lhs, Size rhs)
    {
    }

    /// <inheritdoc/>
    [DebuggerStepThrough]
    public override string ToString()
    {
    }

    /// <summary>
    /// Returns <c>true</c> if the two sizes have the same dimensions.
    /// </summary>
    [DebuggerStepThrough]
    public static bool operator ==(Size lhs, Size rhs)
    {
    }

    /// <summary>
    /// Returns <c>true</c> if the two sizes have different dimensions.
    /// </summary>
    [DebuggerStepThrough]
    public static bool operator !=(Size lhs, Size rhs)
    {
    }

    /// <inheritdoc/>
    [DebuggerStepThrough]
    public override bool Equals(object? obj)
    {
    }

    /// <summary>
    /// Returns <c>true</c> if this size is equal to the specified <paramref name = "other"/>.
    /// </summary>
    public bool Equals(Size other)
    {
    }

    /// <inheritdoc/>
    [DebuggerStepThrough]
    public override int GetHashCode()
    {
    }

    /// <summary>
    /// Returns the minimum of two sizes, using the smaller width and height from each.
    /// </summary>
    public static Size Min(Size lhs, Size rhs)
    {
    }

    /// <summary>
    /// Returns the maximum of two sizes, using the larger width and height from each.
    /// </summary>
    public static Size Max(Size lhs, Size rhs)
    {
    }
}
```

### Math2D/Vector.cs

```csharp
namespace Xui.Core.Math2D;
/// <summary>
/// Represents a 2D vector with X and Y components, commonly used for geometric operations,
/// layout math, and movement in 2D space.
/// </summary>
public struct Vector
{
    /// <summary>The horizontal component of the vector.</summary>
    public nfloat X;
    /// <summary>The vertical component of the vector.</summary>
    public nfloat Y;
    /// <summary>
    /// Initializes a new instance of the <see cref = "Vector"/> struct
    /// with the specified X and Y components.
    /// </summary>
    /// <param name = "x">The horizontal component of the vector.</param>
    /// <param name = "y">The vertical component of the vector.</param>
    [DebuggerStepThrough]
    public Vector(nfloat x, nfloat y)
    {
    }

    /// <summary>A zero vector (0, 0).</summary>
    public static readonly Vector Zero = (0, 0);
    /// <summary>A unit vector (1, 1).</summary>
    public static readonly Vector One = (1, 1);
    /// <summary>A unit vector pointing left (-1, 0).</summary>
    public static readonly Vector Left = (-1, 0);
    /// <summary>A unit vector pointing up (0, -1).</summary>
    public static readonly Vector Up = (0, -1);
    /// <summary>A unit vector pointing right (1, 0).</summary>
    public static readonly Vector Right = (1, 0);
    /// <summary>A unit vector pointing down (0, 1).</summary>
    public static readonly Vector Down = (0, 1);
    /// <summary>
    /// Returns a normalized (unit length) version of this vector.
    /// </summary>
    public Vector Normalized
    {
        get
        {
        }
    }

    /// <summary>
    /// Returns the vector rotated 90° counter-clockwise (CCW).
    /// </summary>
    public Vector PerpendicularCCW
    {
        get
        {
        }
    }

    /// <summary>
    /// Returns the vector rotated 90° clockwise (CW).
    /// </summary>
    public Vector PerpendicularCW
    {
        get
        {
        }
    }

    /// <summary>
    /// Returns the magnitude (length) of the vector.
    /// </summary>
    public nfloat Magnitude
    {
        get
        {
        }
    }

    /// <summary>
    /// Returns the squared magnitude (length squared) of the vector.
    /// This avoids the square root computation used in <see cref = "Magnitude"/>.
    /// </summary>
    public nfloat MagnitudeSquared
    {
        get
        {
        }
    }

    /// <summary>
    /// Gets the angle in radians between the vector and the positive X-axis,
    /// measured counter-clockwise in the range [-π, π].
    /// </summary>
    /// <remarks>
    /// This is equivalent to calling <c>Atan2(Y, X)</c> and is commonly used to compute
    /// polar angles from directional vectors, such as when determining the angular position
    /// of a point on a circle or ellipse.
    /// </remarks>
    /// <returns>
    /// The angle in radians between this vector and the X-axis.
    /// </returns>
    public nfloat ArcAngle
    {
        get
        {
        }
    }

    /// <summary>
    /// Rotates the vector counterclockwise by the given angle (in degrees).
    /// </summary>
    public Vector Rotate(nfloat degrees)
    {
    }

    /// <summary>
    /// Returns the dot product of two vectors.
    /// </summary>
    [DebuggerStepThrough]
    public static nfloat Dot(Vector lhs, Vector rhs)
    {
    }

    /// <summary>
    /// Projects <paramref name = "lhs"/> onto <paramref name = "rhs"/>.
    /// </summary>
    [DebuggerStepThrough]
    public static Vector Project(Vector lhs, Vector rhs)
    {
    }

    /// <summary>
    /// Returns the 2D cross product (scalar) of two vectors.
    /// </summary>
    [DebuggerStepThrough]
    public static nfloat Cross(Vector lhs, Vector rhs)
    {
    }

    /// <summary>
    /// Returns a new vector with its magnitude limited to the specified maximum.
    /// </summary>
    [DebuggerStepThrough]
    public Vector Clamp(nfloat max)
    {
    }

    /// <summary>
    /// Returns the clockwise angle (in radians) from the upward direction (0, 1) to the given vector.
    /// </summary>
    [DebuggerStepThrough]
    public static nfloat Angle(Vector v)
    {
    }

    /// <summary>
    /// Returns a unit vector rotated clockwise from the upward direction (0, 1) by the given angle (in radians).
    /// </summary>
    [DebuggerStepThrough]
    public static Vector Angle(nfloat radians)
    {
    }

    /// <summary>
    /// Returns the signed angle (in radians) from <paramref name = "lhs"/> to <paramref name = "rhs"/>, clockwise from upward.
    /// </summary>
    [DebuggerStepThrough]
    public static nfloat Angle(Vector lhs, Vector rhs)
    {
    }

    /// <summary>
    /// Linearly interpolates between two vectors by <paramref name = "step"/>.
    /// </summary>
    [DebuggerStepThrough]
    public static Vector Lerp(Vector start, Vector end, nfloat step)
    {
    }

    /// <summary>
    /// Implicitly converts a <see cref = "Point"/> to a <see cref = "Vector"/>.
    /// </summary>
    [DebuggerStepThrough]
    public static implicit operator Vector(Point point)
    {
    }

    /// <summary>
    /// Multiplies a vector by a scalar.
    /// </summary>
    [DebuggerStepThrough]
    public static Vector operator *(Vector v, nfloat f)
    {
    }

    /// <summary>
    /// Divides a vector by a scalar.
    /// </summary>
    [DebuggerStepThrough]
    public static Vector operator /(Vector v, nfloat f)
    {
    }

    /// <summary>
    /// Multiplies a scalar by a vector.
    /// </summary>
    [DebuggerStepThrough]
    public static Vector operator *(nfloat f, Vector v)
    {
    }

    /// <summary>
    /// Adds two vectors.
    /// </summary>
    [DebuggerStepThrough]
    public static Vector operator +(Vector lhs, Vector rhs)
    {
    }

    /// <summary>
    /// Subtracts one vector from another.
    /// </summary>
    [DebuggerStepThrough]
    public static Vector operator -(Vector lhs, Vector rhs)
    {
    }

    /// <summary>
    /// Implicitly converts a tuple to a <see cref = "Vector"/>.
    /// </summary>
    [DebuggerStepThrough]
    public static implicit operator Vector(ValueTuple<nfloat, nfloat> tuple)
    {
    }

    /// <summary>
    /// Returns true if the two vectors have equal components.
    /// </summary>
    [DebuggerStepThrough]
    public static bool operator ==(Vector lhs, Vector rhs)
    {
    }

    /// <summary>
    /// Returns true if any component of the two vectors is different.
    /// </summary>
    [DebuggerStepThrough]
    public static bool operator !=(Vector lhs, Vector rhs)
    {
    }

    /// <inheritdoc/>
    public override bool Equals(object? obj)
    {
    }

    /// <summary>
    /// Returns true if this vector is equal to another vector.
    /// </summary>
    public bool Equals(Vector other)
    {
    }

    /// <inheritdoc/>
    public override int GetHashCode()
    {
    }

    /// <inheritdoc/>
    public override string ToString()
    {
    }
}
```

### obj/Debug/net9.0/.NETCoreApp,Version=v9.0.AssemblyAttributes.cs

```csharp
// <autogenerated />
using System;
using System.Reflection;

[assembly: global::System.Runtime.Versioning.TargetFrameworkAttribute(".NETCoreApp,Version=v9.0", FrameworkDisplayName = ".NET 9.0")]
```

### Set/EmptySet.cs

```csharp
namespace Xui.Core.Set;
/// <summary>
/// Represents an empty set — a set that contains no elements.
/// </summary>
/// <typeparam name = "T">The type of elements the set could (but does not) contain.</typeparam>
/// <remarks>
/// This type always returns <c>false</c> from <see cref = "Contains"/>. It is a useful identity for 
/// set composition and for scenarios like default hit test areas or disabled regions.
/// </remarks>
public sealed class EmptySet<T> : INonEnumerableSet<T>
{
    /// <summary>
    /// A shared singleton instance of the empty set.
    /// </summary>
    public static readonly EmptySet<T> Instance = new();
    // Private constructor to enforce singleton usage.
    private EmptySet()
    {
    }

    /// <summary>
    /// Always returns <c>false</c>, since no element belongs to the empty set.
    /// </summary>
    /// <param name = "obj">The object to test.</param>
    /// <returns><c>false</c> for all inputs.</returns>
    public bool Contains(T obj)
    {
    }
}
```

### Set/INonEnumerableSet.cs

```csharp
namespace Xui.Core.Set;
/// <summary>
/// Represents a mathematical set as defined by its membership test: 
/// whether an element belongs to the set.
/// </summary>
/// <typeparam name = "T">The type of elements being tested for membership.</typeparam>
/// <remarks>
/// This interface does not imply enumeration, mutation, or collection semantics. 
/// It aligns with the pure set-theoretic notion where a set is characterized solely 
/// by the ability to determine if a value is a member of the set (i.e., <c>x ∈ S</c>).
/// </remarks>
public interface INonEnumerableSet<T>
{
    /// <summary>
    /// Determines whether the specified element is a member of this set.
    /// </summary>
    /// <param name = "obj">The element to test for membership.</param>
    /// <returns><c>true</c> if the element belongs to the set; otherwise, <c>false</c>.</returns>
    public bool Contains(T obj);
}
```

### UI/Border.cs

```csharp
using Xui.Core.Math2D;
using Xui.Core.Canvas;

namespace Xui.Core.UI
{
    /// <summary>
    /// A view that draws a background, border, and padding around a single child content view.
    /// </summary>
    public class Border : View
    {
        /// <summary>
        /// Gets or sets the content view displayed inside the border.
        /// </summary>
        public View? Content { get; set; }
        /// <summary>
        /// Gets or sets the thickness of the border on each side.
        /// </summary>
        public Frame BorderThickness { get; set; } = Math2D.Frame.Zero;
        /// <summary>
        /// Gets or sets the corner radius used to round the corners of the border and background.
        /// </summary>
        public CornerRadius CornerRadius { get; set; } = 0;
        /// <summary>
        /// Gets or sets the background color inside the border.
        /// </summary>
        public Color BackgroundColor { get; set; } = Colors.Transparent;
        /// <summary>
        /// Gets or sets the color of the border stroke.
        /// </summary>
        public Color BorderColor { get; set; } = Colors.Transparent;
        /// <summary>
        /// Gets or sets the padding between the border and the content.
        /// </summary>
        public Frame Padding { get; set; } = Math2D.Frame.Zero;

        /// <inheritdoc/>
        protected override Size MeasureCore(Size constraints, IMeasureContext context)
        {
        }

        /// <inheritdoc/>
        protected override void ArrangeCore(Rect rect, IMeasureContext context)
        {
        }

        /// <inheritdoc/>
        protected override void RenderCore(IContext context)
        {
        }
    }
}
```

### UI/Direction.cs

```csharp
namespace Xui.Core.UI;
/// <summary>
/// Indicates the semantic inline direction of content, such as left-to-right or right-to-left.
/// Used to resolve text alignment, layout flow, and mirroring behavior.
/// </summary>
public enum Direction : byte
{
    /// <summary>
    /// Inherit direction from the parent view.
    /// </summary>
    Inherit = 0,
    /// <summary>
    /// Left-to-right flow (default for Western languages).
    /// </summary>
    LeftToRight = 1,
    /// <summary>
    /// Right-to-left flow (used in Arabic, Hebrew, etc).
    /// </summary>
    RightToLeft = 2
}
```

### UI/Flow.cs

```csharp
namespace Xui.Core.UI;
/// <summary>
/// Controls whether layout and rendering should respect directionality and writing mode.
/// Used to suppress mirroring and bidi-aware behavior for diagrams, graphs, and non-linguistic views.
/// </summary>
public enum Flow : byte
{
    /// <summary>
    /// Inherit flow behavior from the parent view.
    /// </summary>
    Inherit = 0,
    /// <summary>
    /// Enable direction-aware and writing-mode-aware behavior.
    /// </summary>
    Aware = 1,
    /// <summary>
    /// Disable all direction-aware layout and rendering. Used for charts, maps, etc.
    /// </summary>
    Unaware = 2
}
```

### UI/FlowDirection.cs

```csharp
namespace Xui.Core.UI;
/// <summary>
/// Represents the resolved physical direction of layout flow along an axis.
/// This is derived from <see cref = "WritingMode"/> and <see cref = "Direction"/>,
/// and is used to control stacking, alignment, and layout flow along block or inline axes.
/// </summary>
public enum FlowDirection : byte
{
    /// <summary>
    /// Content flows from left to right.
    /// </summary>
    LeftToRight = 0,
    /// <summary>
    /// Content flows from right to left.
    /// </summary>
    RightToLeft = 1,
    /// <summary>
    /// Content flows from top to bottom.
    /// </summary>
    TopToBottom = 2,
    /// <summary>
    /// Content flows from bottom to top.
    /// </summary>
    BottomToTop = 3
}
```

### UI/HorizontalAlignment.cs

```csharp
namespace Xui.Core.UI;
/// <summary>
/// Specifies how a view should be aligned horizontally within its layout bounds.
/// Used by parent containers during layout to position the view along the inline axis.
/// </summary>
public enum HorizontalAlignment : byte
{
    /// <summary>
    /// Stretch to fill the full available horizontal space.
    /// </summary>
    Stretch = 0,
    /// <summary>
    /// Align to the left edge (or start edge in LTR layouts).
    /// </summary>
    Left = 1,
    /// <summary>
    /// Center horizontally within the available space.
    /// </summary>
    Center = 2,
    /// <summary>
    /// Align to the right edge (or start edge in RTL layouts).
    /// </summary>
    Right = 3
}
```

### UI/HorizontalStack.cs

```csharp
using Xui.Core.Math2D;
using Xui.Core.Canvas;

namespace Xui.Core.UI;
/// <summary>
/// A layout container that arranges its children horizontally from left to right.
/// 
/// Each child is measured with unconstrained width and a constrained height.
/// The container expands to fit the combined width of all children.
/// </summary>
public class HorizontalStack : ViewCollection
{
    /// <inheritdoc/>
    protected override Size MeasureCore(Size availableBorderEdgeSize, IMeasureContext context)
    {
    }

    /// <inheritdoc/>
    protected override void ArrangeCore(Rect rect, IMeasureContext context)
    {
    }
}
```

### UI/HorizontalUniformStack.cs

```csharp
using Xui.Core.Math2D;
using Xui.Core.Canvas;

namespace Xui.Core.UI;
/// <summary>
/// A layout container that arranges its children in a horizontal stack,
/// assigning each child the same width.
/// </summary>
/// <remarks>
/// <para>
/// If the parent provides a constrained width, the container divides
/// the available width equally among all children.
/// </para>
/// <para>
/// If the width is unconstrained (infinite), the container measures each child
/// to determine the maximum width, and assigns that uniform width to all columns.
/// </para>
/// <para>
/// The height of the container is based on the tallest child.
/// </para>
/// </remarks>
public class HorizontalUniformStack : ViewCollection
{
    /// <summary>
    /// Measures the desired size of this layout container and its children,
    /// based on the available space provided by the parent.
    /// </summary>
    /// <param name = "availableBorderEdgeSize">
    /// The space available for layout, excluding padding and borders.
    /// </param>
    /// <returns>
    /// The desired size of this container based on its layout strategy.
    /// </returns>
    protected override Size MeasureCore(Size availableBorderEdgeSize, IMeasureContext context)
    {
    }

    /// <inheritdoc/>
    protected override void ArrangeCore(Rect rect, IMeasureContext context)
    {
    }
}
```

### UI/Input/EventPhase.cs

```csharp
namespace Xui.Core.UI.Input
{
    /// <summary>
    /// Defines the phase of event delivery through the view hierarchy.
    /// </summary>
    public enum EventPhase
    {
        /// <summary>
        /// Event is tunneling from the root down toward the target view.
        /// </summary>
        Tunnel,
        /// <summary>
        /// Event is bubbling from the target view up toward the root.
        /// </summary>
        Bubble
    }
}
```

### UI/Input/EventRouter.cs

```csharp
using Xui.Core.Math2D;
using Xui.Core.Abstract.Events;

namespace Xui.Core.UI.Input
{
    /// <summary>
    /// Routes platform-level input events through the view hierarchy, translating them into abstract pointer events.
    /// Handles pointer capture, enter/leave events, and event phase delivery (tunneling and bubbling).
    /// </summary>
    public class EventRouter
    {
        private readonly View _rootView;
        // Tracks capture, previous hit target, and last known position per pointer ID
        private readonly Dictionary<int, PointerTracking> _pointerTracking = new();
        /// <summary>
        /// Initializes a new instance of the <see cref = "EventRouter"/> class, responsible for translating platform input events
        /// and dispatching them through the view hierarchy starting from the specified root view.
        /// </summary>
        /// <param name = "rootView">The root view of the window hierarchy that will receive routed pointer events.</param>
        public EventRouter(View rootView)
        {
        }

        /// <summary>
        /// Dispatches a touch event, normalizing it into abstract pointer events.
        /// </summary>
        /// <param name = "touchEvent">The touch event to dispatch.</param>
        public void Dispatch(ref TouchEventRef touchEvent)
        {
        }

        /// <summary>
        /// Dispatches a normalized pointer event into the view hierarchy.
        /// Handles capture, hit-testing, and event phase routing.
        /// </summary>
        private void DispatchPointer(ref PointerEventRef e)
        {
        }

        /// <summary>
        /// Recursively performs hit-testing starting from the given view,
        /// returning the deepest visible view under the specified position.
        /// </summary>
        /// <param name = "view">The view to start hit-testing from.</param>
        /// <param name = "position">The global position to test.</param>
        /// <returns>The deepest view under the point, or null if none hit.</returns>
        private View? HitTest(View view, Point position)
        {
        }

        /// <summary>
        /// Builds the route from the root to the target view.
        /// </summary>
        private List<View> BuildRoute(View target)
        {
        }

        /// <summary>
        /// Tracks information about a specific active pointer (touch, mouse, pen) for routing and event management.
        /// </summary>
        private struct PointerTracking
        {
            /// <summary>
            /// The view that captured this pointer, if any.
            /// </summary>
            public View? Captured;
            /// <summary>
            /// The last view hit by this pointer, used for enter/leave tracking.
            /// </summary>
            public View? PreviousTarget;
            /// <summary>
            /// The last known position of the pointer in global window coordinates.
            /// </summary>
            public Point LastPosition;
        }
    }
}
```

### UI/Input/PointerButton.cs

```csharp
namespace Xui.Core.UI.Input
{
    /// <summary>
    /// Defines the mouse or pointer button associated with a pointer event, based on the W3C Pointer Events specification.
    /// </summary>
    public enum PointerButton : short
    {
        /// <summary>
        /// No button or touch/pen contact change occurred (-1).
        /// </summary>
        None = -1,
        /// <summary>
        /// Left mouse button, touch contact, or pen contact (0).
        /// </summary>
        Left = 0,
        /// <summary>
        /// Middle mouse button (usually the scroll wheel button) (1).
        /// </summary>
        Middle = 1,
        /// <summary>
        /// Right mouse button or pen barrel button (2).
        /// </summary>
        Right = 2,
        /// <summary>
        /// X1 (back) mouse button (3).
        /// </summary>
        X1 = 3,
        /// <summary>
        /// X2 (forward) mouse button (4).
        /// </summary>
        X2 = 4,
        /// <summary>
        /// Pen eraser button (5).
        /// </summary>
        Eraser = 5,
    }
}
```

### UI/Input/PointerButtons.cs

```csharp
namespace Xui.Core.UI.Input
{
    /// <summary>
    /// Defines the set of mouse or pointer buttons currently pressed, based on the W3C Pointer Events specification.
    /// </summary>
    [Flags]
    public enum PointerButtons : short
    {
        /// <summary>
        /// No buttons are currently pressed.
        /// </summary>
        None = 0,
        /// <summary>
        /// Left mouse button, touch contact, or pen contact.
        /// </summary>
        Left = 1 << 0,
        /// <summary>
        /// Right mouse button or pen barrel button.
        /// </summary>
        Right = 1 << 1,
        /// <summary>
        /// Middle mouse button.
        /// </summary>
        Middle = 1 << 2,
        /// <summary>
        /// X1 (back) mouse button.
        /// </summary>
        X1 = 1 << 3,
        /// <summary>
        /// X2 (forward) mouse button.
        /// </summary>
        X2 = 1 << 4,
        /// <summary>
        /// Pen eraser button.
        /// </summary>
        Eraser = 1 << 5,
    }
}
```

### UI/Input/PointerEventRef.cs

```csharp
namespace Xui.Core.UI.Input
{
    /// <summary>
    /// Represents a reference to a pointer event routed to a view, containing identification and pointer state information.
    /// </summary>
    public ref struct PointerEventRef
    {
        /// <summary>
        /// Gets the type of pointer event (e.g., Down, Move, Up, Enter, Leave).
        /// </summary>
        public readonly PointerEventType Type;
        /// <summary>
        /// Gets the unique ID of the pointer. Used to distinguish different active pointers (e.g., different fingers or pen tips).
        /// </summary>
        public readonly int PointerId;
        /// <summary>
        /// Gets the persistent device ID, if known. Identifies the underlying hardware across multiple pointer sessions.
        /// </summary>
        public readonly long PersistentDeviceId;
        /// <summary>
        /// Gets a value indicating whether this pointer is the primary pointer in a multi-pointer interaction (e.g., first touch).
        /// </summary>
        public readonly bool IsPrimary;
        /// <summary>
        /// Gets the current physical state of the pointer (position, pressure, tilt, etc.).
        /// </summary>
        public readonly PointerState State;
        /// <summary>
        /// Gets high-frequency coalesced samples recorded between the last and current events.
        /// </summary>
        public readonly ReadOnlySpan<PointerState> CoalescedStates;
        /// <summary>
        /// Gets future-predicted samples estimated from the current pointer movement.
        /// </summary>
        public readonly ReadOnlySpan<PointerState> PredictedStates;
        /// <summary>
        /// Initializes a new instance of the <see cref = "PointerEventRef"/> struct.
        /// </summary>
        /// <param name = "type">The type of pointer event (Down, Move, Up, etc.).</param>
        /// <param name = "pointerId">The unique ID of the pointer.</param>
        /// <param name = "persistentDeviceId">The persistent ID of the physical device, if available.</param>
        /// <param name = "isPrimary">Indicates whether this pointer is the primary pointer.</param>
        /// <param name = "state">The current physical state of the pointer.</param>
        /// <param name = "coalescedStates">High-frequency samples coalesced since the last event.</param>
        /// <param name = "predictedStates">Future-predicted samples for smoothing or latency compensation.</param>
        public PointerEventRef(PointerEventType type, int pointerId, long persistentDeviceId, bool isPrimary, PointerState state, ReadOnlySpan<PointerState> coalescedStates, ReadOnlySpan<PointerState> predictedStates)
        {
        }
    }
}
```

### UI/Input/PointerEventType.cs

```csharp
namespace Xui.Core.UI.Input
{
    /// <summary>
    /// Defines the type of action associated with a pointer event, based on the W3C Pointer Events specification.
    /// </summary>
    public enum PointerEventType
    {
        /// <summary>
        /// The pointer has moved onto the element's hit region.
        /// Corresponds to the "pointerover" event.
        /// </summary>
        Over,
        /// <summary>
        /// The pointer has entered the element or one of its descendants.
        /// Corresponds to the "pointerenter" event.
        /// </summary>
        Enter,
        /// <summary>
        /// A pointer button has been pressed.
        /// Corresponds to the "pointerdown" event.
        /// </summary>
        Down,
        /// <summary>
        /// The pointer has moved while being active.
        /// Corresponds to the "pointermove" event.
        /// </summary>
        Move,
        /// <summary>
        /// A pointer button has been released.
        /// Corresponds to the "pointerup" event.
        /// </summary>
        Up,
        /// <summary>
        /// The pointer input was canceled by the system.
        /// Corresponds to the "pointercancel" event.
        /// </summary>
        Cancel,
        /// <summary>
        /// The pointer has moved off of the element's hit region.
        /// Corresponds to the "pointerout" event.
        /// </summary>
        Out,
        /// <summary>
        /// The pointer has left the element and all its descendants.
        /// Corresponds to the "pointerleave" event.
        /// </summary>
        Leave,
        /// <summary>
        /// The pointer has been captured by the element.
        /// Corresponds to the "gotpointercapture" event.
        /// </summary>
        GotCapture,
        /// <summary>
        /// The pointer capture has been released from the element.
        /// Corresponds to the "lostpointercapture" event.
        /// </summary>
        LostCapture,
    }
}
```

### UI/Input/PointerState.cs

```csharp
using Xui.Core.Math2D;

namespace Xui.Core.UI.Input
{
    /// <summary>
    /// Describes the physical state of a pointer at a specific moment in time, including position, pressure, tilt, and button information.
    /// </summary>
    public readonly struct PointerState
    {
        /// <summary>
        /// Gets the position of the pointer in global (window) coordinates.
        /// </summary>
        public readonly Point Position;
        /// <summary>
        /// Gets the size of the pointer's contact geometry, such as the area covered by a finger or stylus tip.
        /// </summary>
        public readonly Size ContactSize;
        /// <summary>
        /// Gets the normalized pressure applied by the pointer (range: 0.0 to 1.0).
        /// </summary>
        public readonly nfloat Pressure;
        /// <summary>
        /// Gets the normalized tangential (barrel) pressure applied by the pointer (range: -1.0 to 1.0).
        /// </summary>
        public readonly nfloat TangentialPressure;
        /// <summary>
        /// Gets the tilt of the pointer relative to the X and Y axes (in degrees).
        /// </summary>
        public readonly Vector Tilt;
        /// <summary>
        /// Gets the clockwise twist (rotation) of the pointer around its major axis, in degrees (0.0 to 359.9).
        /// </summary>
        public readonly nfloat Twist;
        /// <summary>
        /// Gets the altitude angle of the pointer relative to the surface (0 = horizontal, π/2 = vertical), in radians.
        /// </summary>
        public readonly nfloat AltitudeAngle;
        /// <summary>
        /// Gets the azimuth angle (compass direction) of the pointer around the vertical axis, in radians.
        /// </summary>
        public readonly nfloat AzimuthAngle;
        /// <summary>
        /// Gets the type of device generating the pointer input (mouse, touch, pen, etc.).
        /// </summary>
        public readonly PointerType PointerType;
        /// <summary>
        /// Gets which button was responsible for triggering this pointer state change (left, right, middle, eraser, etc.).
        /// </summary>
        public readonly PointerButton Button;
        /// <summary>
        /// Gets the set of all currently pressed buttons on the device.
        /// </summary>
        public readonly PointerButtons Buttons;
        /// <summary>
        /// Initializes a new instance of the <see cref = "PointerState"/> struct.
        /// </summary>
        /// <param name = "position">The global position of the pointer.</param>
        /// <param name = "contactSize">The size of the contact area.</param>
        /// <param name = "pressure">The normalized pressure applied.</param>
        /// <param name = "tangentialPressure">The normalized tangential (barrel) pressure applied.</param>
        /// <param name = "tilt">The tilt vector of the pointer.</param>
        /// <param name = "twist">The clockwise twist rotation of the pointer.</param>
        /// <param name = "altitudeAngle">The altitude angle relative to the surface.</param>
        /// <param name = "azimuthAngle">The azimuth angle (compass direction).</param>
        /// <param name = "pointerType">The type of input device.</param>
        /// <param name = "button">The button that triggered the event.</param>
        /// <param name = "buttons">The set of currently pressed buttons.</param>
        public PointerState(Point position, Size contactSize, nfloat pressure, nfloat tangentialPressure, Vector tilt, nfloat twist, nfloat altitudeAngle, nfloat azimuthAngle, PointerType pointerType, PointerButton button, PointerButtons buttons)
        {
        }
    }
}
```

### UI/Input/PointerType.cs

```csharp
namespace Xui.Core.UI.Input
{
    /// <summary>
    /// Defines the type of input device associated with a pointer event.
    /// </summary>
    public enum PointerType
    {
        /// <summary>
        /// A mouse device generated the pointer event.
        /// </summary>
        Mouse,
        /// <summary>
        /// A touch contact (e.g., finger or capacitive touch) generated the pointer event.
        /// </summary>
        Touch,
        /// <summary>
        /// A pen, stylus, or similar fine-point device generated the pointer event.
        /// </summary>
        Pen,
        /// <summary>
        /// The pointer device type could not be determined.
        /// </summary>
        Unknown,
    }
}
```

### UI/Label.cs

```csharp
using Xui.Core.Math2D;
using Xui.Core.Canvas;

namespace Xui.Core.UI
{
    /// <summary>
    /// A view that displays a single line of styled text.
    /// </summary>
    public class Label : View
    {
        /// <summary>
        /// Gets or sets the text content displayed by the label.
        /// </summary>
        public string Text { get; set; } = "";
        /// <summary>
        /// Gets or sets the color used to fill the text.
        /// </summary>
        public Color TextColor { get; set; } = Colors.Black;
        /// <summary>
        /// Gets or sets the font family used for rendering the text.
        /// </summary>
        public string[] FontFamily { get; set; } = ["Verdana"];
        /// <summary>
        /// Gets or sets the font size in points.
        /// </summary>
        public nfloat FontSize { get; set; } = 15;
        /// <summary>
        /// Gets or sets the font style (e.g., normal, italic, oblique).
        /// </summary>
        public FontStyle FontStyle { get; set; } = FontStyle.Italic;
        /// <summary>
        /// Gets or sets the font weight (e.g., normal, bold, numeric weight).
        /// </summary>
        public int FontWeight { get; set; } = 600;
        /// <summary>
        /// Gets or sets the line height of the text.
        /// </summary>
        public nfloat LineHeight { get; set; } = 18;

        /// <inheritdoc/>
        protected override Size MeasureCore(Size availableBorderEdgeSize, IMeasureContext context)
        {
        }

        /// <inheritdoc/>
        protected override void RenderCore(IContext context)
        {
        }
    }
}
```

### UI/LayoutGuide.cs

```csharp
using Xui.Core.Canvas;
using Xui.Core.Math2D;

namespace Xui.Core.UI;
/// <summary>
/// Encapsulates the parameters and results of a layout pass (Measure, Arrange, Render) for a view.
/// </summary>
public struct LayoutGuide
{
    /// <summary>
    /// Indicates the type of layout pass being performed: Measure, Arrange, or Render.
    /// </summary>
    public LayoutPass Pass;
    // Measure spec
    /// <summary>
    /// The available space for measuring this view's margin box. Used during the Measure pass.
    /// </summary>
    public Size AvailableSize;
    /// <summary>
    /// How the view should size itself horizontally during measurement (exact or at-most).
    /// </summary>
    public SizeTo XSize;
    /// <summary>
    /// How the view should size itself vertically during measurement (exact or at-most).
    /// </summary>
    public SizeTo YSize;
    /// <summary>
    /// Optional measurement context providing access to platform-specific text metrics
    /// and precise size calculations during the Measure pass.
    /// If set, text and layout measurements can use font shaping and pixel snapping
    /// consistent with the underlying rendering system.
    /// </summary>
    public IMeasureContext? MeasureContext;
    /// <summary>
    /// The desired size of the view's margin box, produced during the Measure pass.
    /// </summary>
    public Size DesiredSize;
    // Arrange spec
    /// <summary>
    /// The anchor point that defines the alignment constraint for layout.
    /// This point serves as a reference for positioning the view based on alignment.
    /// For example, if alignment is set to <see cref = "Align.End"/>, the anchor represents the bottom-right constraint.
    /// If alignment is <see cref = "Align.Start"/>, it represents the top-left constraint.
    /// </summary>
    public Point Anchor;
    /// <summary>
    /// The horizontal alignment of the view within its allocated space.
    /// </summary>
    public Align XAlign;
    /// <summary>
    /// The vertical alignment of the view within its allocated space.
    /// </summary>
    public Align YAlign;
    /// <summary>
    /// The final rectangle occupied by the view's border edge box after the Arrange pass.
    /// </summary>
    public Rect ArrangedRect;
    // Render spec
    /// <summary>
    /// Optional rendering context for drawing during the Render pass.
    /// </summary>
    public IContext? RenderContext;
    /// <summary>
    /// Returns true if this guide represents a Measure pass.
    /// </summary>
    public bool IsMeasure
    {
        get
        {
        }
    }

    /// <summary>
    /// Returns true if this guide represents an Arrange pass.
    /// </summary>
    public bool IsArrange
    {
        get
        {
        }
    }

    /// <summary>
    /// Returns true if this guide represents a Render pass.
    /// </summary>
    public bool IsRender
    {
        get
        {
        }
    }

    /// <summary>
    /// Flags indicating which type of layout pass is being performed.
    /// Multiple passes may be combined (e.g., Measure | Render).
    /// </summary>
    [Flags]
    public enum LayoutPass : byte
    {
        /// <summary>
        /// Indicates a Measure pass to determine desired size.
        /// </summary>
        Measure = 1 << 0,
        /// <summary>
        /// Indicates an Arrange pass to finalize layout position and size.
        /// </summary>
        Arrange = 1 << 1,
        /// <summary>
        /// Indicates a Render pass to draw the view's content.
        /// </summary>
        Render = 1 << 2,
    }

    /// <summary>
    /// Defines how the view should interpret the size constraints during measurement.
    /// </summary>
    public enum SizeTo : byte
    {
        /// <summary>
        /// The view must exactly match the given size constraints.
        /// </summary>
        Exact,
        /// <summary>
        /// The view may size to its content, but must not exceed the given constraints.
        /// </summary>
        AtMost
    }

    /// <summary>
    /// Defines alignment of a view within a layout axis.
    /// </summary>
    public enum Align : byte
    {
        /// <summary>
        /// Align to the start (top or left).
        /// </summary>
        Start = 0,
        /// <summary>
        /// Align to the center.
        /// </summary>
        Center = 1,
        /// <summary>
        /// Align to the end (bottom or right).
        /// </summary>
        End = 2
    }
}
```

### UI/VerticalAlignment.cs

```csharp
namespace Xui.Core.UI;
/// <summary>
/// Specifies how a view should be aligned vertically within its layout bounds.
/// Used by parent containers to control vertical positioning along the block axis.
/// </summary>
public enum VerticalAlignment : byte
{
    /// <summary>
    /// Stretch to fill the full available vertical space.
    /// </summary>
    Stretch = 0,
    /// <summary>
    /// Align to the top edge (or start edge in top-down layouts).
    /// </summary>
    Top = 1,
    /// <summary>
    /// Center vertically within the available space.
    /// </summary>
    Middle = 2,
    /// <summary>
    /// Align to the bottom edge (or start edge in bottom-up layouts).
    /// </summary>
    Bottom = 3
}
```

### UI/VerticalStack.cs

```csharp
using Xui.Core.Math2D;
using Xui.Core.Canvas;

namespace Xui.Core.UI;
/// <summary>
/// A layout container that arranges its children vertically from top to bottom.
/// 
/// Each child is measured with an unconstrained height and is allowed to take up as much vertical space as needed.
/// The container expands to fit the combined height of all children.
/// </summary>
public class VerticalStack : ViewCollection
{
    /// <inheritdoc/>
    protected override Size MeasureCore(Size availableBorderEdgeSize, IMeasureContext context)
    {
    }

    /// <inheritdoc/>
    protected override void ArrangeCore(Rect rect, IMeasureContext context)
    {
    }
}
```

### UI/VerticalUniformStack.cs

```csharp
using Xui.Core.Math2D;
using Xui.Core.Canvas;

namespace Xui.Core.UI;
/// <summary>
/// A layout container that arranges its children in a vertical stack,
/// giving each child the same height.
/// </summary>
/// <remarks>
/// <para>
/// If the parent provides a constrained height, the container divides
/// the available height equally among all children.
/// </para>
/// <para>
/// If the height is unconstrained (infinite), the container measures each child
/// to determine the maximum height, and uses that height for all rows.
/// </para>
/// </remarks>
public class VerticalUniformStack : ViewCollection
{
    /// <inheritdoc/>
    protected override Size MeasureCore(Size availableBorderEdgeSize, IMeasureContext context)
    {
    }

    /// <inheritdoc/>
    protected override void ArrangeCore(Rect rect, IMeasureContext context)
    {
    }
}
```

### UI/View.cs

```csharp
using Xui.Core.Canvas;
using Xui.Core.Math2D;

namespace Xui.Core.UI;
/// <summary>
/// Base class for all UI elements in the Xui layout engine.
/// A view participates in layout, rendering, and input hit testing, and may contain child views.
/// </summary>
public abstract partial class View
{
    /// <summary>
    /// The parent view in the visual hierarchy. This is set automatically when the view is added to a container.
    /// </summary>
    public View? Parent { get; internal set; }
    /// <summary>
    /// The border edge of this view in global coordinates relative to the top-left of the window.
    /// </summary>
    public Rect Frame { get; protected set; }
    /// <summary>
    /// The margin around this view. Margins participate in collapsed margin logic during layout,
    /// and are external spacing relative to the parent or surrounding siblings.
    /// </summary>
    public Frame Margin { get; set; } = (0, 0);
    /// <summary>
    /// The horizontal alignment of this view inside its layout anchor region.
    /// Used during layout when the view has remaining space within its container.
    /// </summary>
    public HorizontalAlignment HorizontalAlignment { get; set; } = HorizontalAlignment.Stretch;
    /// <summary>
    /// The vertical alignment of this view inside its layout anchor region.
    /// Used during layout when the view has remaining space within its container.
    /// </summary>
    public VerticalAlignment VerticalAlignment { get; set; } = VerticalAlignment.Stretch;
    /// <summary>
    /// The writing direction of this view, which determines the block or inline flow direction.
    /// Inherited from the parent flow context if set to <see cref = "Direction.Inherit"/>.
    /// </summary>
    public Direction Direction { get; set; } = Direction.Inherit;
    /// <summary>
    /// The writing mode of this view (e.g. horizontal top-to-bottom or vertical right-to-left).
    /// Inherited from the parent if set to <see cref = "WritingMode.Inherit"/>.
    /// </summary>
    public WritingMode WritingMode { get; set; } = WritingMode.HorizontalTB;
    /// <summary>
    /// Controls how the layout system treats this view's children.
    /// Can be inherited or explicitly overridden for advanced layout containers.
    /// </summary>
    public Flow Flow { get; set; } = Flow.Aware;
    /// <summary>
    /// The minimum width of the border edge box.
    /// </summary>
    public nfloat MinimumWidth { get; set; } = 0;
    /// <summary>
    /// The minimum height of the border edge box.
    /// </summary>
    public nfloat MinimumHeight { get; set; } = 0;
    /// <summary>
    /// The maximum width of the border edge box.
    /// </summary>
    public nfloat MaximumWidth { get; set; } = nfloat.PositiveInfinity;
    /// <summary>
    /// The maximum height of the border edge box.
    /// </summary>
    public nfloat MaximumHeight { get; set; } = nfloat.PositiveInfinity;
    /// <summary>
    /// Returns the number of child views. Used by layout containers and traversal logic.
    /// Leaf views should return 0.
    /// </summary>
    public virtual int Count { get; } = 0;

    /// <summary>
    /// Indexer to access child views by index.
    /// Layout containers should implement this to expose their children.
    /// </summary>
    public virtual View this[int index]
    {
        get
        {
        }
    }

    /// <summary>
    /// Determines whether the given point (in local coordinates) hits this view’s visual bounds.
    /// Used for input dispatch and hit testing.
    /// </summary>
    /// <param name = "point">The point to test, relative to this view’s coordinate space.</param>
    /// <returns><c>true</c> if the point is inside the view’s frame; otherwise <c>false</c>.</returns>
    public virtual bool HitTest(Point point)
    {
    }

    /// <summary>
    /// Performs a full layout pass for a view - measure, arrange and render.
    /// 
    /// Flags can limit to a subset of the layout passes, in case a container needs to measure children multiple times,
    /// or in case a container can rush forward without forking the layout pass into multiple sub-passes.
    /// 
    /// The layout method will delegate parts of the execution to <see cref = "MeasureCore"/>, <see cref = "ArrangeCore"/> and <see cref = "RenderCore"/>.
    /// 
    /// If a container needs to call multiple times methods for a child,
    /// either call the <see cref = "Measure"/>, <see cref = "Arrange"/> and <see cref = "Render"/>,
    /// or construct a <see cref = "LayoutGuide"/> with the specific flags and pass it to <see cref = "Update"/>.
    /// 
    /// Some containers may override and implement a Layout in a way, that it compacts the flow and avoids fork,
    /// like a VerticalStack that is placed on fullscreen (with fixed width),
    /// can arrange children top to bottom calling their Layout directly - eventually going foreach-layout without splitting into foreach-measure, foreach-arrange cycles.
    /// VerticalStack however, when centered, while it can layout children vertically in a single pass, it can't render, because it needs its height to figure out its position,
    /// so in these cases it may foreach-layout (measure and arrange) resolve the stack Y position and then foreach-layout (for render).
    /// </summary>
    /// <param name = "guide"></param>
    /// <returns></returns>
    public virtual LayoutGuide Update(LayoutGuide guide)
    {
    }

    /// <summary>
    /// Measures the view using the specified available size, returning the desired size
    /// calculated during the layout pass.
    /// </summary>
    /// <param name = "availableSize">The maximum space available for the view to occupy.</param>
    /// <param name = "context"></param>
    /// <returns>The size that the view desires to occupy within the constraints.</returns>
    public Size Measure(Size availableSize, IMeasureContext context)
    {
    }

    /// <summary>
    /// Arranges the view within the specified rectangle, finalizing its layout position and size.
    /// </summary>
    /// <param name = "rect">The rectangle defining the position and exact size for the view.</param>
    /// <returns>The rectangle occupied by the arranged view.</returns>
    public Rect Arrange(Rect rect, IMeasureContext context)
    {
    }

    /// <summary>
    /// Renders the view using the given rendering context. This should be called after layout is complete.
    /// </summary>
    /// <param name = "context">The rendering context used to draw the view.</param>
    public void Render(IContext context)
    {
    }

    /// <summary>
    /// Determines the minimum size that this view's border edge box requires,
    /// given the maximum available size. Margin is not part of this size.
    /// </summary>
    /// <param name = "availableBorderEdgeSize">
    /// The maximum size available for the view’s border edge box. 
    /// This size excludes margins, which are handled by the parent layout.
    /// </param>
    /// <param name = "context">
    /// The layout metrics context providing access to platform-specific measurements,
    /// text sizing, and pixel snapping utilities.
    /// </param>
    /// <returns>
    /// The desired size of the border edge box based on content and layout logic.
    /// </returns>
    protected virtual Size MeasureCore(Size availableBorderEdgeSize, IMeasureContext context)
    {
    }

    /// <summary>
    /// Performs the layout pass by arranging content and children within the view's
    /// border edge box, using the provided rectangle.
    /// </summary>
    /// <param name = "rect">
    /// The final rectangle (position and size) allocated to this view's border edge box.
    /// </param>
    /// <param name = "context">
    /// The layout metrics context providing access to platform-specific measurements,
    /// text sizing, and pixel snapping utilities.
    /// </param>
    protected virtual void ArrangeCore(Rect rect, IMeasureContext context)
    {
    }

    /// <summary>
    /// Renders the content and children of this view using the provided rendering context.
    /// </summary>
    /// <param name = "context">
    /// The drawing context used for rendering visual content to the output surface.
    /// </param>
    protected virtual void RenderCore(IContext context)
    {
    }
}
```

### UI/View.Input.cs

```csharp
using Xui.Core.UI.Input;

namespace Xui.Core.UI;
public abstract partial class View
{
    /// <summary>
    /// Called during event dispatch to handle a pointer event in a specific event phase.
    /// </summary>
    public virtual void RaisePointerEvent(ref PointerEventRef e, EventPhase phase)
    {
    }
}
```

### UI/ViewCollection.cs

```csharp
using Xui.Core.Canvas;
using Xui.Core.Math2D;

namespace Xui.Core.UI;
/// <summary>
/// A base class for container views that hold and manage a list of child views.
/// Provides methods for adding, removing, rendering, and hit testing children.
/// </summary>
public abstract class ViewCollection : View
{
    /// <summary>
    /// The internal list of child views contained within this view.
    /// </summary>
    protected readonly List<View> children = new();
    /// <summary>
    /// Gets the number of child views in this collection.
    /// </summary>
    public override int Count
    {
        get
        {
        }
    }

    /// <summary>
    /// Gets the child view at the specified index.
    /// </summary>
    /// <param name = "index">The index of the child to retrieve.</param>
    /// <returns>The child view at the given index.</returns>
    public override View this[int index]
    {
        get
        {
        }
    }

    /// <summary>
    /// Adds the provided views to this container during object initialization.
    /// This property is intended for use with C# object initializers and will not clear existing children.
    /// </summary>
    public ReadOnlySpan<View> Content
    {
        init
        {
        }
    }

    /// <summary>
    /// Adds a view to this container.
    /// </summary>
    /// <param name = "child">The view to add.</param>
    /// <exception cref = "InvalidOperationException">Thrown if the view already has a parent.</exception>
    public virtual void Add(View child)
    {
    }

    /// <summary>
    /// Removes a view from this container.
    /// </summary>
    /// <param name = "child">The view to remove.</param>
    public virtual void Remove(View child)
    {
    }

    /// <summary>
    /// Renders all child views using the provided rendering context.
    /// </summary>
    /// <param name = "context">The rendering context used for drawing visual content.</param>
    protected override void RenderCore(IContext context)
    {
    }

    /// <summary>
    /// Performs hit testing against the child views in reverse order (top-most first).
    /// </summary>
    /// <param name = "point">The point to test, in the coordinate space of this view.</param>
    /// <returns>True if any child was hit; otherwise, falls back to base implementation.</returns>
    public override bool HitTest(Point point)
    {
    }
}
```

### UI/WritingMode.cs

```csharp
namespace Xui.Core.UI;
/// <summary>
/// Specifies the orientation and flow direction of text and block layout.
/// Affects which axis is considered "block" and "inline".
/// </summary>
public enum WritingMode : byte
{
    /// <summary>
    /// Inherit direction from the parent view.
    /// </summary>
    Inherit,
    /// <summary>
    /// Horizontal writing mode. Text flows left-to-right, lines stack top-to-bottom.
    /// </summary>
    HorizontalTB,
    /// <summary>
    /// Vertical writing mode. Lines stack right-to-left, text flows top-to-bottom.
    /// </summary>
    VerticalRL,
    /// <summary>
    /// Vertical writing mode. Lines stack left-to-right, text flows top-to-bottom.
    /// </summary>
    VerticalLR,
    /// <summary>
    /// Sideways vertical mode. Lines stack right-to-left, but glyphs are rotated to remain horizontal.
    /// </summary>
    SidewaysRL,
    /// <summary>
    /// Sideways vertical mode. Lines stack left-to-right, with horizontal glyph orientation.
    /// </summary>
    SidewaysLR
}
```

