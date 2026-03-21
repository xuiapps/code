namespace Xui.GPU.IR;

/// <summary>
/// Represents a function declaration.
/// </summary>
public class IrFunctionDecl : IrNode
{
    public override IrNodeKind Kind => IrNodeKind.FunctionDecl;
    public string Name { get; set; } = string.Empty;
    public IrType ReturnType { get; set; } = null!;
    public List<IrParameter> Parameters { get; } = new();
    public IrBlock Body { get; set; } = new();
}
