using Xui.Core.Actual;

namespace Xui.Core.Abstract;

/// <summary>
/// Represents an abstract base class for Xui applications.
/// This class is paired at runtime with a platform-specific counterpart,
/// which delegates to actual system APIs on macOS, Windows, Android, etc.
///
/// Users should subclass <see cref="Application"/>, override the <see cref="Start"/> method,
/// and call <see cref="Run"/> to launch the application.
/// </summary>
public abstract class Application
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Application"/> class.
    /// </summary>
    public Application()
    {
    }

    /// <summary>
    /// Starts the main application loop by delegating to the platform-specific run loop.
    /// This method may block until the application exits,
    /// or may return immediately if the platform bootstraps a runtime loop before instantiating the app delegate.
    /// </summary>
    /// <returns>The application's exit code.</returns>
    public int Run() =>
        Runtime.Current
            .CreateRunloop(this, Runtime.Instruments?.CreateRunLoop())
            .Run();

    /// <summary>
    /// Called by the runtime after initialization.
    /// Override this method to set up application state and display the initial UI.
    /// </summary>
    public abstract void Start();
}
