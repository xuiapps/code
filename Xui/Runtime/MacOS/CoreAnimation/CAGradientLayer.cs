using System;
using System.Runtime.InteropServices;
using static Xui.Runtime.MacOS.CoreGraphics;
using static Xui.Runtime.MacOS.ObjC;

namespace Xui.Runtime.MacOS;

public static partial class CoreAnimation
{
    /// <summary>
    /// https://developer.apple.com/documentation/quartzcore/cashapelayer?language=objc
    /// </summary>
    public partial class CAGradientLayer : CALayer
    {
        public static new readonly Class Class = new Class(CoreAnimation.Lib, "CAGradientLayer");

        private static readonly Prop.NInt ColorsProp = new Prop.NInt("colors", "setColors:");

        private static readonly Prop.NInt LocationsProp = new Prop.NInt("locations", "setLocations:");

        private static readonly Prop StartPointProp = new Prop("startPoint", "setStartPoint:");

        private static readonly Prop EndPointProp = new Prop("endPoint", "setEndPoint:");

        private static readonly Prop.NInt TypeProp = new Prop.NInt("type", "setType:");

        [LibraryImport(LibObjCLib, EntryPoint = "objc_msgSend")]
        private static partial CGPoint objc_msgSend_retCGPoint(nint obj, nint sel);

        [LibraryImport(LibObjCLib, EntryPoint = "objc_msgSend")]
        private static partial CGPoint objc_msgSend(nint obj, nint sel, CGPoint cgPoint);

        public CAGradientLayer() : base(Class.New())
        {
        }

        public CAGradientLayer(nint id) : base(id)
        {
        }

        // TODO: CFArray of CGColorRef
        public nint Colors
        {
            get => ColorsProp.Get(this);
            set => ColorsProp.Set(this, value);
        }

        // TODO: CFArray of CFNumberRef
        public nint Locations
        {
            get => LocationsProp.Get(this);
            set => LocationsProp.Set(this, value);
        }

        public CGPoint StartPoint
        {
            get => objc_msgSend_retCGPoint(this, StartPointProp.GetSel);
            set => objc_msgSend(this, StartPointProp.SetSel, value);
        }

        public CGPoint EndPoint
        {
            get => objc_msgSend_retCGPoint(this, EndPointProp.GetSel);
            set => objc_msgSend(this, EndPointProp.SetSel, value);
        }

        public CAGradientLayerType Type
        {
            get => throw new NotImplementedException();
            set => TypeProp.Set(this, value);
        }
    }
}
