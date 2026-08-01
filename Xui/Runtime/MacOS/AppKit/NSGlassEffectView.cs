using static Xui.Runtime.MacOS.ObjC;
using NFloat = System.Runtime.InteropServices.NFloat;

namespace Xui.Runtime.MacOS;

public static partial class AppKit
{
    /// <summary>
    /// A view that embeds its content in the system's dynamic glass material.
    /// Available on newer macOS releases only.
    /// </summary>
    public class NSGlassEffectView : NSView
    {
        public static new readonly Class Class = new Class(AppKit.Lib, "NSGlassEffectView");

        private static readonly Prop.NInt ContentViewProp = new Prop.NInt("contentView", "setContentView:");
        private static readonly Prop CornerRadiusProp = new Prop("cornerRadius", "setCornerRadius:");
        private static readonly Prop.Bool EffectIsInteractiveProp = new Prop.Bool("effectIsInteractive", "setEffectIsInteractive:");
        private static readonly Prop.NInt StyleProp = new Prop.NInt("style", "setStyle:");
        private static readonly Prop.NInt TintColorProp = new Prop.NInt("tintColor", "setTintColor:");

        /// <summary>Whether this macOS release provides NSGlassEffectView.</summary>
        public static bool IsAvailable => objc_getClass("NSGlassEffectView") != 0;

        public NSGlassEffectView() : base(Class.New())
        {
        }

        /// <summary>The view embedded in the glass material.</summary>
        public NSView ContentView
        {
            get => new NSView(ContentViewProp.Get(this));
            set => ContentViewProp.Set(this, value);
        }

        /// <summary>The curvature applied to every glass corner.</summary>
        public NFloat CornerRadius
        {
            get => objc_msgSend_retNFloat(this, CornerRadiusProp.GetSel);
            set => objc_msgSend(this, CornerRadiusProp.SetSel, value);
        }

        /// <summary>Enables the system's interactive glass response.</summary>
        public bool EffectIsInteractive
        {
            get => EffectIsInteractiveProp.Get(this);
            set => EffectIsInteractiveProp.Set(this, value);
        }

        /// <summary>The glass visual treatment.</summary>
        public NSGlassEffectViewStyle Style
        {
            get => (NSGlassEffectViewStyle)StyleProp.Get(this);
            set => StyleProp.Set(this, (nint)value);
        }

        /// <summary>The color used to tint the glass effect.</summary>
        public NSColorRef TintColor
        {
            get => new NSColorRef(TintColorProp.Get(this));
            set => TintColorProp.Set(this, value);
        }

        /// <summary>Clears any tint from the glass effect.</summary>
        public void ClearTintColor() => TintColorProp.Set(this, 0);

        public override NSView Autorelease() => (NSView)base.Autorelease();
    }
}
