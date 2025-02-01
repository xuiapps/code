using static Xui.Runtime.MacOS.ObjC;

namespace Xui.Runtime.MacOS;

public static partial class AppKit
{
    public class NSVisualEffectView : NSView
    {
        public static new readonly Class Class = new Class(AppKit.Lib, "NSVisualEffectView");

        private static readonly Prop.NInt MaterialProp = new Prop.NInt("material", "setMaterial:");

        private static readonly Prop.NInt BlendingModeProp = new Prop.NInt("blendingMode", "setBlendingMode:");
        private static readonly Prop.NInt StateProp = new Prop.NInt("state", "setState:");

        public NSVisualEffectView() : base(Class.New())
        {
        }

        public NSVisualEffectMaterial Material
        {
            get => (NSVisualEffectMaterial)MaterialProp.Get(this);
            set => MaterialProp.Set(this, (nint)value);
        }

        public NSVisualEffectBlendingMode BlendingMode
        {
            get => (NSVisualEffectBlendingMode)BlendingModeProp.Get(this);
            set => BlendingModeProp.Set(this, (nint)value);
        }

        public NSVisualEffectState State
        {
            get => (NSVisualEffectState)StateProp.Get(this);
            set => StateProp.Set(this, (nint)value);
        }

        public override NSView Autorelease() => (NSView)base.Autorelease();
    }
}