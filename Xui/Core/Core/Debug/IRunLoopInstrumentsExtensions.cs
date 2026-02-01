using System.Runtime.CompilerServices;

namespace Xui.Core.Debug;

/// <summary>
/// Extension methods for <see cref="IRunLoopInstruments"/> to simplify usage with nullable instances.
/// </summary>
public static class IRunLoopInstrumentsExtensions
{
    /// <summary>
    /// Logs a performance event. If instruments is null, this is a no-op.
    /// </summary>
    /// <param name="instruments">The instrumentation instance, or null.</param>
    /// <param name="levelOfDetail">The verbosity level for this event.</param>
    /// <param name="aspect">The aspect category for this event.</param>
    /// <param name="message">The interpolated string message.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void NullableEvent(
        this IRunLoopInstruments? instruments,
        LevelOfDetail levelOfDetail,
        Aspect aspect,
        [InterpolatedStringHandlerArgument("instruments", "levelOfDetail", "aspect")]
        ref InstrumentsInterpolatedStringHandler message)
    {
        instruments?.Event(levelOfDetail, aspect, ref message);
    }

    /// <summary>
    /// Begins a trace scope. If instruments is null, returns a no-op scope.
    /// </summary>
    /// <param name="instruments">The instrumentation instance, or null.</param>
    /// <param name="levelOfDetail">The verbosity level for this trace.</param>
    /// <param name="aspect">The aspect category for this trace.</param>
    /// <param name="message">The interpolated string message describing the trace scope.</param>
    /// <returns>A TraceScope that ends the trace when disposed.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static TraceScope NullableTrace(
        this IRunLoopInstruments? instruments,
        LevelOfDetail levelOfDetail,
        Aspect aspect,
        [InterpolatedStringHandlerArgument("instruments", "levelOfDetail", "aspect")]
        ref InstrumentsInterpolatedStringHandler message)
    {
        if (instruments == null)
            return default;

        return instruments.Trace(levelOfDetail, aspect, ref message);
    }
}
