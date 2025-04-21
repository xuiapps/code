using Xui.Core.Actual;

[assembly: System.Reflection.Metadata.MetadataUpdateHandler(typeof(Xui.Core.Abstract.HotReload))]

namespace Xui.Core.Abstract;

/// <summary>
/// Internal integration point for .NET's MetadataUpdateHandler, enabling partial support
/// for Hot Reload during development. This is not a stable API and should not be used
/// by application developers.
/// </summary>
/// <remarks>
/// This type is invoked automatically by the runtime when types are updated via
/// Edit and Continue or Hot Reload. However, Hot Reload support in Xui is limited,
/// and application developers should rely on full rebuilds for consistent behavior.
///
/// The current implementation simply invalidates open windows and clears transient caches,
/// without attempting to rehydrate or diff application state.
/// </remarks>
public static class HotReload
{
    /// <summary>
    /// Called by the runtime to clear any cached data after a hot reload.
    /// Not intended for use by application developers.
    /// </summary>
    /// <param name="updatedTypes">The list of updated types, if available.</param>
    public static void ClearCache(Type[]? updatedTypes)
    {
        // Placeholder for future cache invalidation logic.
    }

    /// <summary>
    /// Posts a request to the main UI dispatcher to refresh application state.
    /// </summary>
    /// <param name="updatedTypes">The list of updated types, if available.</param>
    public static void UpdateApplication(Type[]? updatedTypes) =>
        Runtime.Current?.MainDispatcher?.Post(() =>
            MainThreadUpdateApplication(updatedTypes));

    /// <summary>
    /// Performs a synchronous application update on the main thread.
    /// This currently forces all open windows to re-render.
    /// </summary>
    /// <param name="updatedTypes">The list of updated types, if available.</param>
    public static void MainThreadUpdateApplication(Type[]? updatedTypes)
    {
        foreach (var window in Window.OpenWindows)
        {
            window.Invalidate();
        }
    }
}
