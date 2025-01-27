using System.Runtime.InteropServices;
using static Xui.Runtime.IOS.CoreFoundation;
using static Xui.Runtime.IOS.CoreGraphics;
using static Xui.Runtime.IOS.ObjC;

namespace Xui.Runtime.IOS;

public static partial class Foundation
{
    public partial class NSRunLoop : NSObject
    {
        public class Mode : NSString
        {
            public static readonly Mode Common = GetKeyConstant("NSRunLoopCommonModes");

            public static readonly Mode Default = GetKeyConstant("NSDefaultRunLoopMode");

            // Some are missing from AppKit: NSEventTrackingRunLoopMode, NSModalPanelRunLoopMode, UITrackingRunLoopMode

            private static Mode GetKeyConstant(string name) => new Mode(Marshal.ReadIntPtr(NativeLibrary.GetExport(Foundation.Lib, name)));

            private Mode(nint id) : base(id)
            {
            }
        }
    }
}