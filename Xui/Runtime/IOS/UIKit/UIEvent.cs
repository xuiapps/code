using static Xui.Runtime.IOS.ObjC;

namespace Xui.Runtime.IOS;

public static partial class UIKit
{
    public partial class UIEvent : NSObject
    {
        public static new readonly Class Class = new Class(Lib, "UIEvent");

        public static readonly Prop.NInt TypeProp = new Prop.NInt("type", "setType:");

        public static readonly Prop.NInt SubtypeProp = new Prop.NInt("subtype", "setSubtype:");

        public static readonly Prop.NInt AllTouchesProp = new Prop.NInt("allTouches", "setAllTouches:");

        public UIEvent(nint id) : base(id)
        {
        }

        public UIEvent() : base(Class.New())
        {
        }

        public EventType Type => (EventType)TypeProp.Get(this);

        public EventSubtype Subtype => (EventSubtype)SubtypeProp.Get(this);
    }
}
