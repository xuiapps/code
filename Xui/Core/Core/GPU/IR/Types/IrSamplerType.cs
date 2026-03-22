namespace Xui.GPU.IR;

/// <summary>
/// Represents a sampler type.
/// </summary>
public class IrSamplerType : IrType
{
    /// <summary>Gets the IR node kind for this type.</summary>
    public override IrNodeKind Kind => IrNodeKind.SamplerType;
    /// <summary>Gets the display name of this sampler type.</summary>
    public override string Name => "Sampler";

    /// <summary>Shared singleton instance of the sampler type.</summary>
    public static readonly IrSamplerType Instance = new();
}
