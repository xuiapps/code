using System;

namespace Xui.Runtime.IOS;

public class ObjCException : Exception
{
    public ObjCException(string? message) : base(message)
    {
    }
}