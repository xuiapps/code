using System.Runtime.InteropServices;
using static Xui.Runtime.MacOS.ObjC;

namespace Xui.Runtime.MacOS;

public static partial class CoreFoundation
{
    public partial class CFArray : NSObject
    {
        public struct CallBacks
        {
            // callbacks version, current version is 0
            public nint Version;

            // typedef const void *(*CFArrayRetainCallBack)(CFAllocatorRef allocator, const void *value);
            public nint Retain;

            // typedef void (*CFArrayReleaseCallBack)(CFAllocatorRef allocator, const void *value);
            public nint Release;

            // typedef CFStringRef (*CFArrayCopyDescriptionCallBack)(const void *value);
            public nint CopyDescription;

            // typedef Boolean (*CFArrayEqualCallBack)(const void *value1, const void *value2);
            public nint Equal;
        }
    }
}