using System.Runtime.CompilerServices;

namespace Xui.Core.Debug;

/// <summary>
/// A ref struct returned by <see cref="InstrumentsAccessor.Trace"/> that closes the active trace
/// scope when disposed. Intended for use with <c>using</c>.
/// </summary>
public ref struct TraceScope : IDisposable
{
    private IInstrumentsSink? sink;

    /// <summary>Initializes a new <see cref="TraceScope"/> that will call <see cref="IInstrumentsSink.EndTrace"/> on the given sink when disposed.</summary>
    /// <param name="sink">The sink to notify on dispose, or <c>null</c> for a no-op scope.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public TraceScope(IInstrumentsSink? sink)
    {
        this.sink = sink;
    }

    /// <summary>Ends the active trace scope on the sink.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Dispose() => sink?.EndTrace();
}
