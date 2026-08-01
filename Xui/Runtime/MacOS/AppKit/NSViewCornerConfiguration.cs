using static Xui.Runtime.MacOS.ObjC;
using NFloat = System.Runtime.InteropServices.NFloat;

namespace Xui.Runtime.MacOS;

public static partial class AppKit
{
    /// <summary>
    /// Factories for AppKit's adaptive view corner configuration API.
    /// Available on newer macOS releases only.
    /// </summary>
    public static class NSViewCornerConfiguration
    {
        private static readonly Class RadiusClass = new(AppKit.Lib, "NSViewCornerRadius");
        private static readonly Class ConfigurationClass = new(AppKit.Lib, "NSViewCornerConfiguration");
        private static readonly Sel ContainerConcentricRadiusWithMinimumSel = new("containerConcentricRadiusWithMinimum:");
        private static readonly Sel UniformCornersWithRadiusSel = new("uniformCornersWithRadius:");

        public static bool IsAvailable =>
            objc_getClass("NSViewCornerRadius") != 0 &&
            objc_getClass("NSViewCornerConfiguration") != 0;

        /// <summary>
        /// Creates a uniform configuration whose radius follows the containing
        /// view's corner geometry, while staying at least <paramref name="minimumRadius"/>.
        /// </summary>
        public static nint CreateContainerConcentric(NFloat minimumRadius)
        {
            var radius = objc_msgSend_retIntPtr(
                RadiusClass,
                ContainerConcentricRadiusWithMinimumSel,
                minimumRadius);
            return objc_msgSend_retIntPtr(ConfigurationClass, UniformCornersWithRadiusSel, radius);
        }
    }
}
