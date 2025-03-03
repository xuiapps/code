using System.Collections.ObjectModel;
using Xui.Core.Abstract.Events;
using Xui.Core.Actual;
using Xui.Core.Math2D;

namespace Xui.Core.Abstract;

public abstract class Window : Abstract.IWindow, Abstract.IWindow.ISoftKeyboard
{
    private static IList<Window> openWindows = new List<Window>();
    public static IReadOnlyList<Window> OpenWindows = new ReadOnlyCollection<Window>(openWindows);

    public Actual.IWindow Actual { get; }

    public virtual Rect DisplayArea { get; set; }

    public virtual Rect SafeArea { get; set; }

    public Window()
    {
        this.Actual = this.CreateActualWindow();
    }

    public string Title
    {
        get => this.Actual.Title;
        set => this.Actual.Title = value;
    }

    public bool RequireKeyboard
    {
        get => this.Actual.RequireKeyboard;
        set => this.Actual.RequireKeyboard = value;
    }

    public void Show()
    {
        this.Actual.Show();
        openWindows.Add(this);
    }

    public virtual void Render(ref RenderEventRef rect)
    {
    }

    public virtual void WindowHitTest(ref WindowHitTestEventRef evRef)
    {
    }

    public virtual bool Closing() => true;

    public virtual void Closed()
    {
        openWindows.Remove(this);
    }

    protected virtual Actual.IWindow CreateActualWindow() => Runtime.Current!.CreateWindow(this);

    public virtual void Invalidate() => this.Actual.Invalidate();

    public virtual void OnMouseDown(ref MouseDownEventRef e)
    {
    }

    public virtual void OnMouseMove(ref MouseMoveEventRef e)
    {
    }

    public virtual void OnMouseUp(ref MouseUpEventRef e)
    {
    }

    public virtual void OnScrollWheel(ref ScrollWheelEventRef e)
    {
    }

    public virtual void OnTouch(ref TouchEventRef e)
    {
    }

    public virtual void OnAnimationFrame(ref FrameEventRef e)
    {
    }

    public virtual void InsertText(ref InsertTextEventRef eventRef)
    {
    }

    public virtual void DeleteBackwards(ref DeleteBackwardsEventRef eventRef)
    {
    }
}
