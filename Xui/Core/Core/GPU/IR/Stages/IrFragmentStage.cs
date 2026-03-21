namespace Xui.GPU.IR;

/// <summary>
/// Represents a fragment shader stage.
/// </summary>
public class IrFragmentStage : IrNode
{
    public override IrNodeKind Kind => IrNodeKind.FragmentStage;

    public string Name { get; set; } = string.Empty;
    public IrStructType InputType { get; set; } = null!;
    public IrStructType OutputType { get; set; } = null!;
    public IrStructType? BindingsType { get; set; }
    public IrBlock Body { get; set; } = new();
}
