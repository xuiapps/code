using System.Runtime.InteropServices;

namespace Xui.Middleware.Emulator.Actual;

public readonly record struct EmulatorStatusBarStyle(
    string? TimeText = null,
    string? NetworkText = null,
    NFloat? BatteryLevel = null,
    NFloat? SignalStrength = null)
{
    public static EmulatorStatusBarStyle Deterministic => new(
        TimeText: "9:41",
        NetworkText: "5G",
        BatteryLevel: 0.75f,
        SignalStrength: 0.85f);
}
