namespace Xui.Core.Debug;

/// <summary>
/// Factory interface for creating per-run-loop instrumentation sinks.
/// Set on <see cref="Actual.Runtime.Instruments"/> at startup.
/// Implementations are swappable (console, file, network, E2E test).
/// </summary>
public interface IInstruments
{
    IInstrumentsSink CreateSink();
}
