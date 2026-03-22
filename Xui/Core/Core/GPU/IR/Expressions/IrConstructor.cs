namespace Xui.GPU.IR;

/// <summary>
/// Represents a constructor call.
/// </summary>
public class IrConstructor : IrExpression
{
    /// <summary>Gets the node kind for this constructor call.</summary>
    public override IrNodeKind Kind => IrNodeKind.Constructor;

    /// <summary>Gets the type being constructed.</summary>
    public override IrType Type { get; }

    /// <summary>Gets the list of arguments passed to the constructor.</summary>
    public List<IrExpression> Arguments { get; }

    /// <summary>Initializes a new instance of the <see cref="IrConstructor"/> class.</summary>
    public IrConstructor(IrType type, List<IrExpression> arguments)
    {
        Type = type;
        Arguments = arguments;
    }
}
