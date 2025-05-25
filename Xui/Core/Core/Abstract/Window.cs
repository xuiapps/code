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
public class Window : Abstract.IWindow, Abstract.IWindow.ISoftKeyboard
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

    public RootView RootView { get; }

    public View Content { init => this.RootView.Content = value; }

    /// <summary>
    /// Initializes a new instance of the <see cref="Window"/> class.
    /// This creates the backing platform window.
    /// </summary>
    public Window()
    {
        this.Actual = this.CreateActualWindow();
        this.RootView = new RootView(this);
    }

    /// <summary>
    /// Gets or sets the window title (where supported by the platform).
    /// </summary>
    public string Title
    {
        get => this.Actual.Title;
        set => this.Actual.Title = value;
    }

    /// <summary>
    /// Requests that the soft keyboard be shown or hidden (on supported platforms).
    /// </summary>
    public bool RequireKeyboard
    {
        get => this.Actual.RequireKeyboard;
        set => this.Actual.RequireKeyboard = value;
    }

    /// <summary>
    /// Makes the window visible and adds it to the list of open windows.
    /// </summary>
    public void Show()
    {
        this.Actual.Show();
        openWindows.Add(this);
    }

    /// <inheritdoc/>
    public virtual void Render(ref RenderEventRef renderEventRef)
    {
        using var context = Runtime.Current.DrawingContext;
        ((IContent)this.RootView).Update(renderEventRef.Rect, context);
    }

    /// <inheritdoc/>
    public virtual void WindowHitTest(ref WindowHitTestEventRef evRef)
    {
        // Subclasses may override to support custom window chrome or drag regions.
    }

    /// <inheritdoc/>
    public virtual bool Closing() => true;

    /// <inheritdoc/>
    public virtual void Closed()
    {
        openWindows.Remove(this);
    }

    /// <summary>
    /// Creates the platform-specific window for this abstract window.
    /// </summary>
    /// <returns>The platform implementation of <see cref="Actual.IWindow"/>.</returns>
    protected virtual Actual.IWindow CreateActualWindow() => Runtime.Current!.CreateWindow(this);

    /// <summary>
    /// Requests a visual invalidation/redraw of this window.
    /// </summary>
    public virtual void Invalidate() => this.Actual.Invalidate();

    /// <inheritdoc/>
    public virtual void OnMouseDown(ref MouseDownEventRef e)
    {
        ((IContent)this.RootView).OnMouseDown(ref e);
    }

    /// <inheritdoc/>
    public virtual void OnMouseMove(ref MouseMoveEventRef e)
    {
        ((IContent)this.RootView).OnMouseMove(ref e);
    }

    /// <inheritdoc/>
    public virtual void OnMouseUp(ref MouseUpEventRef e)
    {
        ((IContent)this.RootView).OnMouseUp(ref e);
    }

    /// <inheritdoc/>
    public virtual void OnScrollWheel(ref ScrollWheelEventRef e)
    {
    }

    /// <inheritdoc/>
    public virtual void OnTouch(ref TouchEventRef e)
    {
        ((IContent)this.RootView).OnTouch(ref e);
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
