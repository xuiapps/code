namespace Xui.Runtime.Windows;

public static partial class D3D11
{
    /// <summary>
    /// Identifies expected resource use during rendering.
    /// Mirrors <c>D3D11_USAGE</c>.
    /// </summary>
    public enum Usage : uint
    {
        Default = 0,
        Immutable = 1,
        Dynamic = 2,
        Staging = 3,
    }
}
