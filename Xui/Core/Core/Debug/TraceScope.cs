using System.Runtime.CompilerServices;

namespace Xui.Core.Debug;

/// <summary>
/// A stack-only disposable scope used for hierarchical method tracing.
///
/// This ref struct is returned by <see cref="IRunLoopInstruments.Trace"/> and
/// automatically calls <see cref="IRunLoopInstruments.EndTrace"/> when disposed.
/// It must be used with a 'using' statement to ensure proper disposal.
///
/// The ref struct restriction ensures this is stack-allocated only, providing
/// zero-allocation tracing for performance-critical paths.
///
/// Usage:
/// <code>
/// using var trace = instruments.BeginTrace("MyMethod");
/// // ... method body ...
/// // Automatically calls EndTrace when 'trace' goes out of scope
/// </code>
/// </summary>
public ref struct TraceScope : IDisposable
{
    private IRunLoopInstruments? instruments;

    /// <summary>
    /// Initializes a new trace scope.
    /// This is called internally by <see cref="IRunLoopInstruments.Trace"/>.
    /// </summary>
    /// <param name="instruments">The instrumentation instance to notify on disposal, or null for a no-op scope.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public TraceScope(IRunLoopInstruments? instruments)
    {
        this.instruments = instruments;
    }

    /// <summary>
    /// Disposes the trace scope and calls <see cref="IRunLoopInstruments.EndTrace"/>.
    /// This is automatically called when the 'using' block exits.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Dispose() => instruments?.EndTrace();
}
