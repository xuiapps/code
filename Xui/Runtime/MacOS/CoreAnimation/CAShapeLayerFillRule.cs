using System.Runtime.InteropServices;

namespace Xui.Runtime.MacOS;

public static partial class CoreAnimation
{
    public sealed class CAShapeLayerFillRule
    {
        public readonly nint Self;

        private CAShapeLayerFillRule(nint self)
        {
            this.Self = self;
        }

        public static readonly CAShapeLayerFillRule EvenOdd = new CAShapeLayerFillRule(Marshal.ReadIntPtr(NativeLibrary.GetExport(CoreAnimation.Lib, "kCAFillRuleEvenOdd")));

        public static readonly CAShapeLayerFillRule NonZero = new CAShapeLayerFillRule(Marshal.ReadIntPtr(NativeLibrary.GetExport(CoreAnimation.Lib, "kCAFillRuleNonZero")));

        public static implicit operator nint(CAShapeLayerFillRule fillRule) => fillRule.Self;
    }
}