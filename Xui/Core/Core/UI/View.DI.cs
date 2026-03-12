namespace Xui.Core.UI;

/// <summary>
/// Base class for all UI elements in the Xui layout engine.
/// A view participates in layout, rendering, and input hit testing, and may contain child views.
/// </summary>
public partial class View : IServiceProvider, Layer.ILayerHost
{
    /// <summary>
    /// Resolves a service of the given type by walking up the parent chain.
    /// Returns <c>null</c> if no ancestor provides the service.
    /// </summary>
    public virtual object? GetService(Type serviceType) =>
        this.Parent?.GetService(serviceType);
}
