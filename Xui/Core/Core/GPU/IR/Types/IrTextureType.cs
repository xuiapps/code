namespace Xui.GPU.IR;

/// <summary>
/// Represents a texture type.
/// </summary>
public class IrTextureType : IrType
{
    public override IrNodeKind Kind => IrNodeKind.TextureType;
    public override string Name => $"Texture2D<{PixelType.Name}>";

    public IrType PixelType { get; }

    public IrTextureType(IrType pixelType)
    {
        PixelType = pixelType;
    }
}
