namespace Xui.Runtime.IOS;

public static partial class ObjC
{
    /// <summary>
    /// Abstracts C# interactions with an Objective-C selector.
    /// </summary>
    public struct Super
    {
        public nint receiver;
        public nint super_class;

        public Super(nint receiver, nint super_class)
        {
            this.receiver = receiver;
            this.super_class = super_class;
        }
    }
}