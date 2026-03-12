using System.Runtime.CompilerServices;

namespace Xui.Core.Debug;

/// <summary>
/// Lightweight zero-allocation facade over an <see cref="IInstrumentsSink"/> for logging and tracing.
/// All formatting is skipped when the sink is disabled for the given scope and level.
/// </summary>
public readonly struct InstrumentsAccessor
{
    internal readonly IInstrumentsSink? Sink;

    /// <summary>Initializes a new accessor backed by the given sink.</summary>
    /// <param name="sink">The sink to write to, or <c>null</c> to produce a no-op accessor.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public InstrumentsAccessor(IInstrumentsSink? sink)
    {
        this.Sink = sink;
    }

    /// <summary>Logs a formatted message at the given scope and level of detail.</summary>
    /// <param name="scope">The instrumentation scope.</param>
    /// <param name="lod">The level of detail for filtering.</param>
    /// <param name="message">The interpolated message string. Not evaluated when the sink is disabled.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Log(
        Scope scope,
        LevelOfDetail lod,
        [InterpolatedStringHandlerArgument("", "scope", "lod")]
        ref InstrumentsInterpolatedStringHandler message)
    {
        if (Sink != null)
            Sink.Log(scope, lod, message.AsSpan());
        message.Dispose();
    }

    /// <summary>Begins a named trace scope and returns a <see cref="TraceScope"/> that ends it when disposed.</summary>
    /// <param name="scope">The instrumentation scope.</param>
    /// <param name="lod">The level of detail for filtering.</param>
    /// <param name="message">The interpolated trace label. Not evaluated when the sink is disabled.</param>
    /// <returns>A <see cref="TraceScope"/> that calls <see cref="IInstrumentsSink.EndTrace"/> on dispose.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public TraceScope Trace(
        Scope scope,
        LevelOfDetail lod,
        [InterpolatedStringHandlerArgument("", "scope", "lod")]
        ref InstrumentsInterpolatedStringHandler message)
    {
        if (Sink != null)
        {
            Sink.BeginTrace(scope, lod, message.AsSpan());
            message.Dispose();
            return new TraceScope(Sink);
        }
        message.Dispose();
        return default;
    }
}
