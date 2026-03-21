namespace Xui.Runtime.Windows;

public static partial class D3D11
{
    /// <summary>
    /// Identifies a resource to be accessed for reading and/or writing by the CPU.
    /// Mirrors <c>D3D11_MAP</c>.
    /// </summary>
    public enum Map : uint
    {
        Read = 1,
        Write = 2,
        ReadWrite = 3,
        WriteDiscard = 4,
        WriteNoOverwrite = 5,
    }
}
