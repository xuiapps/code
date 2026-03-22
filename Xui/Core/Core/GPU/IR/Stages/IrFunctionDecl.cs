namespace Xui.GPU.IR;

/// <summary>
/// Represents a function declaration.
/// </summary>
public class IrFunctionDecl : IrNode
{
    /// <summary>Gets the IR node kind for this declaration.</summary>
    public override IrNodeKind Kind => IrNodeKind.FunctionDecl;
    /// <summary>Gets or sets the function name.</summary>
    public string Name { get; set; } = string.Empty;
    /// <summary>Gets or sets the return type.</summary>
    public IrType ReturnType { get; set; } = null!;
    /// <summary>Gets the list of function parameters.</summary>
    public List<IrParameter> Parameters { get; } = new();
    /// <summary>Gets or sets the function body block.</summary>
    public IrBlock Body { get; set; } = new();
}
