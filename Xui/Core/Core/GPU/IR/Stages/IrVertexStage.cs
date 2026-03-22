namespace Xui.GPU.IR;

/// <summary>
/// Represents a vertex shader stage.
/// </summary>
public class IrVertexStage : IrNode
{
    /// <summary>Gets the IR node kind for this stage.</summary>
    public override IrNodeKind Kind => IrNodeKind.VertexStage;

    /// <summary>Gets or sets the stage name.</summary>
    public string Name { get; set; } = string.Empty;
    /// <summary>Gets or sets the input struct type.</summary>
    public IrStructType InputType { get; set; } = null!;
    /// <summary>Gets or sets the output struct type.</summary>
    public IrStructType OutputType { get; set; } = null!;
    /// <summary>Gets or sets the optional bindings struct type.</summary>
    public IrStructType? BindingsType { get; set; }
    /// <summary>Gets or sets the body block of this stage.</summary>
    public IrBlock Body { get; set; } = new();
}
