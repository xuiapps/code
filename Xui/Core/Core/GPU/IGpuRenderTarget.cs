namespace Xui.GPU.Hardware;

/// <summary>
/// Represents a GPU render target backed by a texture.
/// Rendered output can be read back to CPU for display.
/// </summary>
public interface IGpuRenderTarget : IDisposable
{
    /// <summary>Gets the underlying texture.</summary>
    IGpuTexture Texture { get; }

    /// <summary>Gets the width of the render target in pixels.</summary>
    int Width { get; }

    /// <summary>Gets the height of the render target in pixels.</summary>
    int Height { get; }

    /// <summary>
    /// Reads back the rendered pixels to a CPU byte array.
    /// The pixel format is BGRA (Blue, Green, Red, Alpha), 4 bytes per pixel.
    /// </summary>
    /// <param name="destination">
    /// A byte array of size Width * Height * 4 to receive the pixels.
    /// </param>
    void ReadbackPixelsBgra(byte[] destination);
}
