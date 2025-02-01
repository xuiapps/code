using static Xui.Runtime.MacOS.ObjC;

namespace Xui.Runtime.MacOS;

public static partial class AppKit
{
    public class NSToolbar : NSObject
    {
        public static new readonly Class Class = new Class(AppKit.Lib, "NSToolbar");

        private static readonly Prop.Bool ShowsBaselineSeparatorProp = new Prop.Bool("showsBaselineSeparator", "setShowsBaselineSeparator:");

        private static readonly Prop.Bool VisibleProp = new Prop.Bool("isVisible", "setVisible:");

        public NSToolbar(nint id) : base(id)
        {
        }

        public NSToolbar() : base(Class.New())
        {
        }

        public bool ShowsBaselineSeparator
        {
            get => ShowsBaselineSeparatorProp.Get(this);
            set => ShowsBaselineSeparatorProp.Set(this, value);
        }

        public bool Visible
        {
            get => VisibleProp.Get(this);
            set => VisibleProp.Set(this, value);
        }
    }
}