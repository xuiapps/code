using Xui.Core.UI;

namespace Xui.Apps.TestApp.Examples;

public class Example : View
{
    public string Title { init; get; } = "Example";

    public View? Content { init; get; }

    public override int Count => this.Content is null ? 0 : 1;

    public override View this[int index] => index == 0 && this.Content is not null ? this.Content : throw new IndexOutOfRangeException();
}
