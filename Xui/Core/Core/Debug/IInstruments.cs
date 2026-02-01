using System.Diagnostics;

namespace Xui.Core.Debug;

/// <summary>
/// Factory interface for creating run-loop–aligned instrumentation instances.
///
/// Implementations of this interface are used by the application at startup
/// to produce a per-run-loop instrumentation instance that records structured
/// runtime events such as frame boundaries, scheduling decisions, timing scopes,
/// and state transitions.
///
/// The factory itself is application-scoped, while the objects it creates are
/// expected to be thread-aligned and owned exclusively by a single run loop
/// for the duration of its lifetime.
/// </summary>
public interface IInstruments
{
    /// <summary>
    /// Creates a new instrumentation instance associated with a single run loop.
    ///
    /// This method is called exactly once per run loop during application startup.
    /// The returned instance will be used exclusively by the run loop on its
    /// owning thread and must not be shared across multiple run loops or threads.
    ///
    /// Implementations should perform any necessary per-run-loop initialization
    /// here (e.g. allocating buffers or stacks) and avoid relying on thread-local
    /// or global mutable state.
    /// </summary>
    /// <returns>
    /// A run-loop–aligned instrumentation instance.
    /// </returns>
    IRunLoopInstruments CreateRunLoop();
}
