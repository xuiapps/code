using Xui.Core.Actual;
using Xui.Core.Canvas;
using Xui.Core.Debug;

namespace Xui.Core.Actual;

/// <summary>
/// Provides global access to the platform-specific runtime environment for the current Xui application.
/// 
/// The platform must assign <see cref="Current"/> at startup with a concrete <see cref="IRuntime"/> implementation,
/// which wires up platform-specific services like rendering, window creation, and dispatching.
/// </summary>
public static class Runtime
{
    private static IRuntime? current;

    /// <summary>
    /// Gets or sets the current platform runtime instance.
    /// This must be initialized at application startup by platform bootstrap code.
    /// </summary>
    /// <exception cref="RuntimeNotAvailable">
    /// Thrown if accessed before the runtime has been initialized.
    /// </exception>
    public static IRuntime Current
    {
        get
        {
            if (current == null)
            {
                throw new RuntimeNotAvailable();
            }
            else
            {
                return current;
            }
        }

        set
        {
            current = value;
        }
    }

    /// <summary>
    /// Gets the global drawing context provided by the current platform runtime, if available.
    /// </summary>
    public static IContext? DrawingContext => Current?.DrawingContext;

    /// <summary>
    /// Gets the main dispatcher for scheduling UI work on the platform's main thread, if available.
    /// </summary>
    public static IDispatcher? MainDispatcher => Current?.MainDispatcher;

    /// <summary>
    /// Gets or sets the instrumentation factory for runtime diagnostics.
    /// Set at startup before calling <see cref="Abstract.Application.Run"/>.
    /// </summary>
    public static IInstruments? Instruments { get; set; }

    [ThreadStatic]
    private static IInstrumentsSink? currentSink;

    [ThreadStatic]
    private static bool sinkInitialized;

    /// <summary>
    /// Gets the thread-local instrumentation accessor.
    /// Lazily creates a sink from <see cref="Instruments"/> on first access per thread.
    /// </summary>
    public static InstrumentsAccessor CurrentInstruments
    {
        get
        {
            if (!sinkInitialized)
            {
                sinkInitialized = true;
                currentSink = Instruments?.CreateSink();
            }
            return new InstrumentsAccessor(currentSink);
        }
    }

    /// <summary>
    /// Exception thrown when <see cref="Current"/> is accessed before it has been initialized.
    /// </summary>
    public class RuntimeNotAvailable : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RuntimeNotAvailable"/> exception
        /// with a helpful diagnostic message.
        /// </summary>
        public RuntimeNotAvailable()
            : base("First thing to do in your app is set Runtime.Current!\nSomehow the execution reached a Runtime.Current call before a runtime was set.")
        {
        }
    }
}
