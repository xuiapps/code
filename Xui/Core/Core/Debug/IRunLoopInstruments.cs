using System.Runtime.CompilerServices;

namespace Xui.Core.Debug;

/// <summary>
/// Represents a run-loop–aligned instrumentation session used to record
/// structured diagnostic and performance events for a single runtime run loop.
///
/// Implementations of this interface are created by an <see cref="IInstruments"/>
/// factory and are owned exclusively by the run loop for the duration of its
/// lifetime. A run-loop instruments instance is expected to be thread-aligned
/// and must not be shared across multiple run loops or threads.
///
/// This interface is designed for zero-allocation, high-performance tracing
/// of frame boundaries, input events, layout passes, rendering operations,
/// and other performance-critical runtime events for flame chart visualization.
/// </summary>
public interface IRunLoopInstruments : IDisposable, IMessageBuilder
{
    /// <summary>
    /// Logs a performance event with zero allocations using an interpolated string handler.
    /// </summary>
    /// <param name="levelOfDetail">The verbosity level for this event.</param>
    /// <param name="aspect">The aspect category for this event.</param>
    /// <param name="message">The interpolated string message (zero-allocation).</param>
    void Event(
        LevelOfDetail levelOfDetail,
        Aspect aspect,
        [InterpolatedStringHandlerArgument("", "levelOfDetail", "aspect")]
        ref InstrumentsInterpolatedStringHandler message);

    /// <summary>
    /// Begins a trace scope for a method or operation.
    /// Returns a disposable ref struct that calls <see cref="EndTrace"/> on disposal.
    /// </summary>
    /// <param name="levelOfDetail">The verbosity level for this trace.</param>
    /// <param name="aspect">The aspect category for this trace.</param>
    /// <param name="message">The interpolated string message describing the trace scope.</param>
    /// <returns>A TraceScope ref struct that automatically ends the trace when disposed.</returns>
    TraceScope Trace(
        LevelOfDetail levelOfDetail,
        Aspect aspect,
        [InterpolatedStringHandlerArgument("", "levelOfDetail", "aspect")]
        ref InstrumentsInterpolatedStringHandler message);

    /// <summary>
    /// Ends a trace scope. Called automatically by <see cref="TraceScope.Dispose"/>.
    /// </summary>
    void EndTrace();
}
