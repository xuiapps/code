namespace Xui.GPU.IR;

/// <summary>
/// Represents a texture type.
/// </summary>
public class IrTextureType : IrType
{
    /// <summary>Gets the IR node kind for this type.</summary>
    public override IrNodeKind Kind => IrNodeKind.TextureType;
    /// <summary>Gets the display name of this texture type.</summary>
    public override string Name => $"Texture2D<{PixelType.Name}>";

    /// <summary>Gets the pixel element type of the texture.</summary>
    public IrType PixelType { get; }

    /// <summary>
    /// Initializes a new instance of <see cref="IrTextureType"/> with the specified pixel type.
    /// </summary>
    public IrTextureType(IrType pixelType)
    {
        PixelType = pixelType;
    }
}
