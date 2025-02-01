using static Xui.Runtime.MacOS.ObjC;

namespace Xui.Runtime.MacOS;

public static partial class CoreFoundation
{
    public partial class CFDictionary : NSObject
    {
        public struct ValueCallBacks
        {
            // callbacks version, current version is 0
            public nint Version;

            // typedef const void *(*CFDictionaryRetainCallBack)(CFAllocatorRef allocator, const void *value);
            public nint Retain;

            // typedef void (*CFDictionaryReleaseCallBack)(CFAllocatorRef allocator, const void *value);
            public nint Release;

            // typedef CFStringRef (*CFDictionaryCopyDescriptionCallBack)(const void *value);
            public nint CopyDescription;

            // typedef Boolean (*CFDictionaryEqualCallBack)(const void *value1, const void *value2);
            public nint Equal;
        }
    }
}