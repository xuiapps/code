using Xui.Core.Canvas;

namespace Xui.Apps.XuiSDK.Icons;

public interface INavIcon
{
    nfloat Selected { get; set; }
    Color Color { get; set; }
    Color SelectedColor { get; set; }
    void Render(IContext context);
}
