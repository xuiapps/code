namespace Xui.GPU.IR;

/// <summary>
/// Represents a variable declaration.
/// </summary>
public class IrVarDecl : IrStatement
{
    /// <summary>Gets the IR node kind for this statement.</summary>
    public override IrNodeKind Kind => IrNodeKind.VarDecl;
    /// <summary>Gets the variable name.</summary>
    public string Name { get; }
    /// <summary>Gets the variable type.</summary>
    public IrType Type { get; }
    /// <summary>Gets the optional initializer expression.</summary>
    public IrExpression? Initializer { get; }

    /// <summary>
    /// Initializes a new instance of <see cref="IrVarDecl"/> with the specified name, type, and optional initializer.
    /// </summary>
    public IrVarDecl(string name, IrType type, IrExpression? initializer = null)
    {
        Name = name;
        Type = type;
        Initializer = initializer;
    }
}
