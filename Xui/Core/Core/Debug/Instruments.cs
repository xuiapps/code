using System.Diagnostics;

namespace Xui.Core.Debug;

/// <summary>
/// Provides built-in instrumentation factories that can be used to enable
/// runtime tracing and diagnostics for Xui applications.
///
/// The values exposed by this class implement <see cref="IInstruments"/> and
/// are intended to be supplied to <see cref="Abstract.Application.Instruments"/>
/// or to the runtime configuration during application startup.
/// </summary>
public static partial class Instruments
{
    /// <summary>
    /// An instrumentation factory that creates run-loop–aligned instruments
    /// which emit human-readable diagnostic output to the console.
    ///
    /// This implementation is intended for development and debugging scenarios
    /// and may incur higher overhead than minimal or no-op instrumentation.
    /// </summary>
    public static readonly IInstruments Console = new ConsoleLogInstruments();
}
