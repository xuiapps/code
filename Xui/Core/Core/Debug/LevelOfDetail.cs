using System.Runtime.CompilerServices;

namespace Xui.Core.Debug;

/// <summary>
/// Verbosity levels for instrumentation events.
/// </summary>
public enum LevelOfDetail : uint
{
    /// <summary>No logging.</summary>
    Quiet = 0,

    /// <summary>Essential events only (frame boundaries, critical events).</summary>
    Essential = 100,

    /// <summary>Normal events (common events like input, major state changes).</summary>
    Normal = 200,

    /// <summary>Informational events (layout passes, rendering).</summary>
    Info = 300,

    /// <summary>Diagnostic events (detailed tracing of method calls).</summary>
    Diagnostic = 400
}