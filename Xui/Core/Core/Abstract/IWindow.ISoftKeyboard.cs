using Xui.Core.Abstract.Events;
using Xui.Core.Math2D;

namespace Xui.Core.Abstract;

public partial interface IWindow
{
    public interface ISoftKeyboard
    {
        public void InsertText(ref InsertTextEventRef eventRef);

        public void DeleteBackwards(ref DeleteBackwardsEventRef eventRef);
    }
}
