namespace Xui.Core.Abstract.Events;

public ref struct InsertTextEventRef
{
    public readonly string Text;

    public InsertTextEventRef(string text)
    {
        this.Text = text;
    }
}