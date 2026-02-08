namespace Xui.Runtime.Test.Actual;

public class TestWindow : Xui.Core.Actual.IWindow
{
    internal Xui.Core.Abstract.IWindow Abstract { get; }
    internal bool Invalid { get; set; }

    public TestWindow(Xui.Core.Abstract.IWindow abstractWindow)
    {
        this.Abstract = abstractWindow;
    }

    public string Title { get; set; } = "";

    public bool RequireKeyboard { get; set; }

    public void Show()
    {
    }

    public void Invalidate()
    {
        Invalid = true;
    }
}
