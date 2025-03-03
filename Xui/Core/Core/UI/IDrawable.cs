using Xui.Core.Canvas;

namespace Xui.Core.UI;

public interface IDrawable
{
    public virtual void Render(IContext context)
    {
    }
}