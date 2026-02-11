using System.Runtime.CompilerServices;

namespace Xui.Core.Debug;

public ref struct TraceScope : IDisposable
{
    private IInstrumentsSink? sink;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public TraceScope(IInstrumentsSink? sink)
    {
        this.sink = sink;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Dispose() => sink?.EndTrace();
}
