using System;

namespace Xui.Runtime.Windows;

public class Win32Exception : Exception
{
    public Win32Exception(string? message) : base(message)
    {
    }
}