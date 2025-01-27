using Xui.Core.Canvas;

namespace Xui.Core.UI;

public static class Extensions
{
    public static void Render(this IContext @this, IDrawable element) => element.Render(@this);
}