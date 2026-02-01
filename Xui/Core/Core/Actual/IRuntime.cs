using Xui.Core.Canvas;
using Xui.Core.Debug;

namespace Xui.Core.Actual;

/// <summary>
/// Provides a platform-specific implementation of the Xui runtime,
/// responsible for creating and connecting abstract application components to their actual counterparts.
///
/// This interface acts as a bridge between the platform-independent core and the underlying OS-specific APIs
/// (e.g., Win32, Cocoa, UIKit), enabling rendering, windowing, and event dispatch.
/// </summary>
public interface IRuntime
{
    /// <summary>
    /// Gets the global drawing context for the current platform.
    /// This typically wraps a native graphics context such as Direct2D (Windows) or CGContext (macOS),
    /// and serves as the entry point for rendering operations.
    /// </summary>
    IContext DrawingContext { get; }

    /// <summary>
    /// Gets the main thread dispatcher for scheduling UI work.
    /// Used to marshal execution onto the main thread for layout, input, and rendering.
    /// </summary>
    IDispatcher MainDispatcher { get; }

    /// <summary>
    /// Creates a platform-specific window that is bound to the given abstract window definition.
    /// </summary>
    /// <param name="windowAbstract">The abstract window definition provided by user code.</param>
    /// <returns>A concrete window implementation for the current platform.</returns>
    Actual.IWindow CreateWindow(Abstract.IWindow windowAbstract);

    /// <summary>
    /// Creates a platform-specific run loop associated with the given abstract application.
    /// The returned run loop is responsible for managing the application's execution lifecycle,
    /// including event dispatch, animation timing, layout, and rendering.
    ///
    /// An optional <see cref="IRunLoopInstruments"/> instance may be provided to enable
    /// low-overhead runtime instrumentation for this run loop. When supplied, the run loop
    /// will use it to record structured events such as frame boundaries, scheduling decisions,
    /// timing scopes, and state transitions (e.g. invalidation and rendering phases).
    ///
    /// The instruments instance is assumed to be thread-aligned and owned by the run loop
    /// for its entire lifetime. Implementations must not share a single instruments instance
    /// across multiple run loops or threads.
    /// </summary>
    /// <param name="applicationAbstract">
    /// The abstract application instance defined by user code.
    /// </param>
    /// <param name="instruments">
    /// An optional run-loop-aligned instrumentation instance used for tracing and performance
    /// analysis. When <c>null</c>, the run loop should operate without instrumentation and
    /// incur no additional overhead.
    /// </param>
    /// <returns>
    /// A platform-specific run loop instance.
    /// </returns>
    IRunLoop CreateRunloop(Abstract.Application applicationAbstract, IRunLoopInstruments? instruments);
}
