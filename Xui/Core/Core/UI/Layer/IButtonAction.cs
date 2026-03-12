namespace Xui.Core.UI.Layer;

/// <summary>
/// Zero-allocation click handler for <see cref="ButtonLayer{THost,TAction}"/>.
/// Implement as a private nested struct inside the owning view so <see cref="Execute"/>
/// receives the fully-typed host and can call any method on it without closures.
/// </summary>
public interface IButtonAction<in THost>
    where THost : ILayerHost
{
    /// <summary>Executes the button action on the given host view.</summary>
    /// <param name="host">The host view that owns the button.</param>
    void Execute(THost host);
}
