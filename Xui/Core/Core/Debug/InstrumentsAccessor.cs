using System.Runtime.CompilerServices;

namespace Xui.Core.Debug;

public readonly struct InstrumentsAccessor
{
    internal readonly IInstrumentsSink? Sink;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public InstrumentsAccessor(IInstrumentsSink? sink)
    {
        this.Sink = sink;
    }

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
