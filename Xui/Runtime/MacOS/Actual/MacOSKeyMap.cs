using Xui.Core.Abstract.Events;

namespace Xui.Runtime.MacOS.Actual;

/// <summary>
/// Maps macOS hardware key codes (kVK_* constants from Carbon HIToolbox/Events.h)
/// to the platform-agnostic <see cref="VirtualKey"/> values.
/// </summary>
internal static class MacOSKeyMap
{
    // NSEventModifierFlags bit masks
    internal const nuint ShiftFlag   = 1 << 17; // NSEventModifierFlagShift
    internal const nuint ControlFlag = 1 << 18; // NSEventModifierFlagControl
    internal const nuint OptionFlag  = 1 << 19; // NSEventModifierFlagOption  (Alt)
    internal const nuint CommandFlag = 1 << 20; // NSEventModifierFlagCommand

    private static readonly VirtualKey[] Map = BuildMap();

    private static VirtualKey[] BuildMap()
    {
        var map = new VirtualKey[0x80]; // covers all key codes up to 0x7F

        // ANSI letters (physical key positions, not characters)
        map[0x00] = VirtualKey.A;
        map[0x01] = VirtualKey.S;
        map[0x02] = VirtualKey.D;
        map[0x03] = VirtualKey.F;
        map[0x04] = VirtualKey.H;
        map[0x05] = VirtualKey.G;
        map[0x06] = VirtualKey.Z;
        map[0x07] = VirtualKey.X;
        map[0x08] = VirtualKey.C;
        map[0x09] = VirtualKey.V;
        map[0x0B] = VirtualKey.B;
        map[0x0C] = VirtualKey.Q;
        map[0x0D] = VirtualKey.W;
        map[0x0E] = VirtualKey.E;
        map[0x0F] = VirtualKey.R;
        map[0x10] = VirtualKey.Y;
        map[0x11] = VirtualKey.T;
        map[0x1F] = VirtualKey.O;
        map[0x20] = VirtualKey.U;
        map[0x22] = VirtualKey.I;
        map[0x23] = VirtualKey.P;
        map[0x25] = VirtualKey.L;
        map[0x26] = VirtualKey.J;
        map[0x28] = VirtualKey.K;
        map[0x2D] = VirtualKey.N;
        map[0x2E] = VirtualKey.M;

        // ANSI digits (top row)
        map[0x12] = VirtualKey.D1;
        map[0x13] = VirtualKey.D2;
        map[0x14] = VirtualKey.D3;
        map[0x15] = VirtualKey.D4;
        map[0x16] = VirtualKey.D6;
        map[0x17] = VirtualKey.D5;
        map[0x19] = VirtualKey.D9;
        map[0x1A] = VirtualKey.D7;
        map[0x1C] = VirtualKey.D8;
        map[0x1D] = VirtualKey.D0;

        // Control keys
        map[0x24] = VirtualKey.Return;
        map[0x30] = VirtualKey.Tab;
        map[0x31] = VirtualKey.Space;
        map[0x33] = VirtualKey.Back;      // Backspace (Delete on Apple keyboards)
        map[0x35] = VirtualKey.Escape;
        map[0x75] = VirtualKey.Delete;    // Forward Delete

        // Modifier keys
        map[0x38] = VirtualKey.Shift;     // Left Shift
        map[0x3C] = VirtualKey.Shift;     // Right Shift
        map[0x3B] = VirtualKey.Control;   // Left Control
        map[0x3E] = VirtualKey.Control;   // Right Control
        map[0x3A] = VirtualKey.Alt;       // Left Option
        map[0x3D] = VirtualKey.Alt;       // Right Option

        // Navigation
        map[0x7B] = VirtualKey.Left;
        map[0x7C] = VirtualKey.Right;
        map[0x7D] = VirtualKey.Down;
        map[0x7E] = VirtualKey.Up;
        map[0x73] = VirtualKey.Home;
        map[0x77] = VirtualKey.End;
        map[0x74] = VirtualKey.Prior;     // Page Up
        map[0x79] = VirtualKey.Next;      // Page Down
        map[0x72] = VirtualKey.Insert;    // Help key

        return map;
    }

    public static VirtualKey ToVirtualKey(ushort keyCode) =>
        keyCode < Map.Length ? Map[keyCode] : VirtualKey.None;
}
