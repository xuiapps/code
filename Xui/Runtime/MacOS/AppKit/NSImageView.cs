using System;
using System.Dynamic;
using static Xui.Runtime.MacOS.ObjC;

namespace Xui.Runtime.MacOS;

public static partial class AppKit
{
    public class NSImageView : NSControl
    {
        public static new readonly Class Class = new Class(AppKit.Lib, "NSImageView");

        private static readonly Prop ImageProp = new Prop("image", "setImage:");

        private static readonly Sel InitWithImageSel = new Sel("initWithImage:");

        public NSImageView(nint id) : base(id)
        {
        }

        public NSImageView() : base(Class.New())
        {
        }

        public NSImage Image
        {
            // TODO: marshalling for C# created objects...
            get => throw new NotImplementedException();
            set => objc_msgSend(this, ImageProp.SetSel, value);
        }

        public override NSImageView Autorelease() => (NSImageView)base.Autorelease();
    }
}