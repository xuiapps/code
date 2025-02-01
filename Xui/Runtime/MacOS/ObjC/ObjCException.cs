using System;

namespace Xui.Runtime.MacOS;

public class ObjCException : Exception
{
    public ObjCException(string? message) : base(message)
    {
    }
}