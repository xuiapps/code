namespace Xui.GPU.Hardware;

/// <summary>
/// Represents a GPU texture that can be used as a render target.
/// </summary>
public interface IGpuTexture : IDisposable
{
    /// <summary>Gets the width of the texture in pixels.</summary>
    int Width { get; }

    /// <summary>Gets the height of the texture in pixels.</summary>
    int Height { get; }
}
