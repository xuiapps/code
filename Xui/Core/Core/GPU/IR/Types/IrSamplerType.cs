namespace Xui.GPU.IR;

/// <summary>
/// Represents a sampler type.
/// </summary>
public class IrSamplerType : IrType
{
    public override IrNodeKind Kind => IrNodeKind.SamplerType;
    public override string Name => "Sampler";

    public static readonly IrSamplerType Instance = new();
}
