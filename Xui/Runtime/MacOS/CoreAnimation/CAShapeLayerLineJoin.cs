using System.Runtime.InteropServices;
using static Xui.Runtime.MacOS.CoreGraphics;
using static Xui.Runtime.MacOS.ObjC;

namespace Xui.Runtime.MacOS;

public static partial class CoreAnimation
{
    public sealed class CAShapeLayerLineJoin
    {
        public readonly nint Self;

        private CAShapeLayerLineJoin(nint self)
        {
            this.Self = self;
        }

        public static readonly CAShapeLayerLineJoin Miter = new CAShapeLayerLineJoin(Marshal.ReadIntPtr(NativeLibrary.GetExport(CoreAnimation.Lib, "kCALineJoinMiter")));

        public static readonly CAShapeLayerLineJoin Round = new CAShapeLayerLineJoin(Marshal.ReadIntPtr(NativeLibrary.GetExport(CoreAnimation.Lib, "kCALineJoinRound")));

        public static readonly CAShapeLayerLineJoin Bevel = new CAShapeLayerLineJoin(Marshal.ReadIntPtr(NativeLibrary.GetExport(CoreAnimation.Lib, "kCALineJoinBevel")));

        public static implicit operator nint(CAShapeLayerLineJoin fillRule) => fillRule.Self;
    }
}