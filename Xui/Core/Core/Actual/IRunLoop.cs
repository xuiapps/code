using Xui.Core.Debug;

namespace Xui.Core.Actual;

/// <summary>
/// Represents a platform-specific run loop responsible for driving the application's lifecycle.
/// Each platform must provide an implementation that enters the appropriate system event loop
/// and continues running until the application exits.
///
/// The Xui runtime uses this interface to abstract over platform differences in event dispatch and app execution.
/// </summary>
public interface IRunLoop
{
    /// <summary>
    /// Gets the instrumentation instance for this run loop, if available.
    /// This allows access to performance tracing and diagnostic events for the run loop.
    /// </summary>
    IRunLoopInstruments? Instruments { get; }

    /// <summary>
    /// Starts the main run loop for the application.
    /// This method may block until the application terminates or exits naturally.
    /// On platforms with built-in UI event loops (e.g., iOS, Android),
    /// this method may return immediately after bootstrapping the application delegate.
    /// </summary>
    /// <returns>The application's exit code.</returns>
    int Run();
}
