namespace Xui.Core.Actual;

/// <summary>
/// Represents a platform-specific dispatcher for marshaling callbacks onto the main UI thread.
/// Used by the Xui runtime to ensure thread-safe execution of UI logic.
///
/// Each platform must provide an implementation that posts callbacks to the appropriate runloop or UI thread.
/// </summary>
public interface IDispatcher
{
    /// <summary>
    /// Posts the specified callback to be executed on the dispatcher's thread (typically the UI thread).
    /// Use this method when calling from a background thread and needing to safely transition
    /// to the main thread for UI updates or layout work.
    /// </summary>
    /// <param name="callback">The action to execute on the dispatcher's thread.</param>
    void Post(Action callback);
}
