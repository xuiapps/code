using System;

namespace Xui.Runtime.MacOS;

public partial class Block
{
    [Flags]
    public enum BlockFlags : int {
        IsNoEscape = 1 << 23,
        HasCopyDispose = 1 << 25,
        HasCtor = 1 << 26,
        IsGlobal = 1 << 28,
        HasStret = 1 << 29,
        HasSignature = 1 << 30,
    };
}