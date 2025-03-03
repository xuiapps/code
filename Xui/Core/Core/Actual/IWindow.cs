namespace Xui.Core.Actual;

public interface IWindow
{
    public string Title { get; set; }
    public void Show();
    public void Invalidate();

    public bool RequireKeyboard { get; set; }
}
