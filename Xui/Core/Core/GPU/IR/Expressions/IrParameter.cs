namespace Xui.GPU.IR;

/// <summary>
/// Represents a parameter reference.
/// </summary>
public class IrParameter : IrExpression
{
    /// <summary>Gets the node kind for this parameter reference.</summary>
    public override IrNodeKind Kind => IrNodeKind.Parameter;

    /// <summary>Gets the type of the parameter.</summary>
    public override IrType Type { get; }

    /// <summary>Gets the name of the parameter.</summary>
    public string Name { get; }

    /// <summary>Initializes a new instance of the <see cref="IrParameter"/> class.</summary>
    public IrParameter(string name, IrType type)
    {
        Name = name;
        Type = type;
    }
}
