using Xui.Core.Math2D;

namespace Xui.Core.UI;

public class ViewCollection : View
{
    private List<View> children = new List<View>();

    public int Count => this.children.Count;

    public View this[int index] { get => this.children[index]; }
}