namespace Xui.GPU.IR;

/// <summary>
/// Represents a vertex shader stage.
/// </summary>
public class IrVertexStage : IrNode
{
    public override IrNodeKind Kind => IrNodeKind.VertexStage;

    public string Name { get; set; } = string.Empty;
    public IrStructType InputType { get; set; } = null!;
    public IrStructType OutputType { get; set; } = null!;
    public IrStructType? BindingsType { get; set; }
    public IrBlock Body { get; set; } = new();
}
