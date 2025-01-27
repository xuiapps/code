using static Xui.Runtime.IOS.CoreFoundation;
using static Xui.Runtime.IOS.ObjC;
using static Xui.Runtime.IOS.UIKit.UIEvent;

namespace Xui.Runtime.IOS;

public static partial class UIKit
{
    public ref partial struct UIEventRef
    {
        public readonly nint Self;

        public UIEventRef(nint self)
        {
            if (self == 0)
            {
                throw new ObjCException($"{nameof(UIEventRef)} instantiated with nil self.");
            }

            this.Self = self;
        }

        public EventType Type => (EventType)TypeProp.Get(this);

        public EventSubtype Subtype => (EventSubtype)SubtypeProp.Get(this);

        public CFSetRef AllTouches => new CFSetRef(AllTouchesProp.Get(this));

        public void Dispose()
        {
            if (this.Self != 0)
            {
                CoreFoundation.CFRelease(this.Self);
            }
        }

        public static implicit operator nint(UIEventRef uiEventRef) => uiEventRef.Self;
    }
}