using static Xui.Runtime.MacOS.ObjC;

namespace Xui.Runtime.MacOS;

public static partial class Foundation
{
    public sealed class NSAutoreleasePool : Ref
    {
        public static readonly Class Class = new Class(Foundation.Lib, "NSAutoreleasePool");

        public NSAutoreleasePool() : base(Class.New())
        {
        }
    }
}