namespace Xui.Core.Abstract.Events;

/// <summary>
/// Platform-agnostic virtual key codes for keyboard input.
/// Values intentionally match Win32 VK_* codes for zero-cost mapping on Windows.
/// </summary>
public enum VirtualKey : ushort
{
    /// <summary>No key pressed.</summary>
    None = 0x00,

    /// <summary>Backspace key.</summary>
    Back = 0x08,
    /// <summary>Tab key.</summary>
    Tab = 0x09,
    /// <summary>Enter/Return key.</summary>
    Return = 0x0D,
    /// <summary>Shift modifier key.</summary>
    Shift = 0x10,
    /// <summary>Control modifier key.</summary>
    Control = 0x11,
    /// <summary>Alt modifier key.</summary>
    Alt = 0x12,
    /// <summary>Escape key.</summary>
    Escape = 0x1B,
    /// <summary>Space bar.</summary>
    Space = 0x20,

    /// <summary>Page Up key.</summary>
    Prior = 0x21,
    /// <summary>Page Down key.</summary>
    Next = 0x22,
    /// <summary>End key.</summary>
    End = 0x23,
    /// <summary>Home key.</summary>
    Home = 0x24,

    /// <summary>Left arrow key.</summary>
    Left = 0x25,
    /// <summary>Up arrow key.</summary>
    Up = 0x26,
    /// <summary>Right arrow key.</summary>
    Right = 0x27,
    /// <summary>Down arrow key.</summary>
    Down = 0x28,

    /// <summary>Insert key.</summary>
    Insert = 0x2D,
    /// <summary>Delete key.</summary>
    Delete = 0x2E,

    /// <summary>Digit key 0.</summary>
    D0 = 0x30,
    /// <summary>Digit key 1.</summary>
    D1 = 0x31,
    /// <summary>Digit key 2.</summary>
    D2 = 0x32,
    /// <summary>Digit key 3.</summary>
    D3 = 0x33,
    /// <summary>Digit key 4.</summary>
    D4 = 0x34,
    /// <summary>Digit key 5.</summary>
    D5 = 0x35,
    /// <summary>Digit key 6.</summary>
    D6 = 0x36,
    /// <summary>Digit key 7.</summary>
    D7 = 0x37,
    /// <summary>Digit key 8.</summary>
    D8 = 0x38,
    /// <summary>Digit key 9.</summary>
    D9 = 0x39,

    /// <summary>Letter key A.</summary>
    A = 0x41,
    /// <summary>Letter key B.</summary>
    B = 0x42,
    /// <summary>Letter key C.</summary>
    C = 0x43,
    /// <summary>Letter key D.</summary>
    D = 0x44,
    /// <summary>Letter key E.</summary>
    E = 0x45,
    /// <summary>Letter key F.</summary>
    F = 0x46,
    /// <summary>Letter key G.</summary>
    G = 0x47,
    /// <summary>Letter key H.</summary>
    H = 0x48,
    /// <summary>Letter key I.</summary>
    I = 0x49,
    /// <summary>Letter key J.</summary>
    J = 0x4A,
    /// <summary>Letter key K.</summary>
    K = 0x4B,
    /// <summary>Letter key L.</summary>
    L = 0x4C,
    /// <summary>Letter key M.</summary>
    M = 0x4D,
    /// <summary>Letter key N.</summary>
    N = 0x4E,
    /// <summary>Letter key O.</summary>
    O = 0x4F,
    /// <summary>Letter key P.</summary>
    P = 0x50,
    /// <summary>Letter key Q.</summary>
    Q = 0x51,
    /// <summary>Letter key R.</summary>
    R = 0x52,
    /// <summary>Letter key S.</summary>
    S = 0x53,
    /// <summary>Letter key T.</summary>
    T = 0x54,
    /// <summary>Letter key U.</summary>
    U = 0x55,
    /// <summary>Letter key V.</summary>
    V = 0x56,
    /// <summary>Letter key W.</summary>
    W = 0x57,
    /// <summary>Letter key X.</summary>
    X = 0x58,
    /// <summary>Letter key Y.</summary>
    Y = 0x59,
    /// <summary>Letter key Z.</summary>
    Z = 0x5A,
}
