using System.Runtime.InteropServices;

namespace Xui.Runtime.MacOS;

public static partial class CoreAnimation
{
    public sealed class CAGradientLayerType
    {
        public readonly nint Self;

        private CAGradientLayerType(nint self)
        {
            this.Self = self;
        }

        public static readonly CAGradientLayerType Axial = new CAGradientLayerType(Marshal.ReadIntPtr(NativeLibrary.GetExport(CoreAnimation.Lib, "kCAGradientLayerAxial")));

        public static readonly CAGradientLayerType Conic = new CAGradientLayerType(Marshal.ReadIntPtr(NativeLibrary.GetExport(CoreAnimation.Lib, "kCAGradientLayerConic")));

        public static readonly CAGradientLayerType Radial = new CAGradientLayerType(Marshal.ReadIntPtr(NativeLibrary.GetExport(CoreAnimation.Lib, "kCAGradientLayerRadial")));

        public static implicit operator nint(CAGradientLayerType type) => type.Self;
    }
}
