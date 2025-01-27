using System.Runtime.InteropServices;

namespace Xui.Runtime.MacOS;

public static partial class CoreAnimation
{
    public sealed class CAShapeLayerLineCap
    {
        public readonly nint Self;

        private CAShapeLayerLineCap(nint self)
        {
            this.Self = self;
        }

        public static readonly CAShapeLayerLineCap Butt = new CAShapeLayerLineCap(Marshal.ReadIntPtr(NativeLibrary.GetExport(CoreAnimation.Lib, "kCALineCapButt")));

        public static readonly CAShapeLayerLineCap Round = new CAShapeLayerLineCap(Marshal.ReadIntPtr(NativeLibrary.GetExport(CoreAnimation.Lib, "kCALineCapRound")));

        public static readonly CAShapeLayerLineCap Square = new CAShapeLayerLineCap(Marshal.ReadIntPtr(NativeLibrary.GetExport(CoreAnimation.Lib, "kCALineCapSquare")));

        public static implicit operator nint(CAShapeLayerLineCap fillRule) => fillRule.Self;
    }
}