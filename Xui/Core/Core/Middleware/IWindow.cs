namespace Xui.Core.Middleware;

/// <summary>
/// Represents a middleware-level window that bridges both the abstract UI model and the platform-specific window implementation.
/// </summary>
/// <remarks>
/// This interface unifies <see cref="Abstract.IWindow"/> and <see cref="Actual.IWindow"/>, enabling a single component to mediate input, rendering,
/// and lifecycle events between the platform and the UI framework. It is commonly used in emulators, testing layers, or composition hosts
/// that need to intercept and control both sides of the window stack.
/// </remarks>
public interface IWindow : Actual.IWindow, Abstract.IWindow
{
}
