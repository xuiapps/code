namespace Xui.Core.Actual;

public interface IWindow
{
    public string Title { get; set; }
    public void Show();
    void Invalidate();
}
