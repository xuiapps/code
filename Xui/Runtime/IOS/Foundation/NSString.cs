using System.Runtime.InteropServices;
using static Xui.Runtime.IOS.CoreFoundation;
using static Xui.Runtime.IOS.CoreGraphics;
using static Xui.Runtime.IOS.ObjC;

namespace Xui.Runtime.IOS;

public static partial class Foundation
{
    public partial class NSString : NSObject
    {
        public static new readonly Class Class = new Class(Lib, "NSString");

        public static readonly Sel DrawAtPointWithAttributesSel = new Sel("drawAtPoint:withAttributes:");

        public static readonly Sel SizeWithAttributesSel = new Sel("sizeWithAttributes:");

        [LibraryImport(LibObjCLib, EntryPoint = "objc_msgSend")]
        public static partial void objc_msgSend(nint nsString, nint sel, CGPoint position, nint attributes);
    
        public NSString(nint id) : base(id)
        {
        }

        public NSString(string text) : base(new CFStringRef(text))
        {
        }
    }
}