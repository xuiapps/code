namespace Xui.GPU.IR;

/// <summary>
/// Represents a return statement.
/// </summary>
public class IrReturn : IrStatement
{
    /// <summary>Gets the IR node kind for this statement.</summary>
    public override IrNodeKind Kind => IrNodeKind.Return;
    /// <summary>Gets the optional return value expression.</summary>
    public IrExpression? Value { get; }

    /// <summary>
    /// Initializes a new instance of <see cref="IrReturn"/> with an optional return value.
    /// </summary>
    public IrReturn(IrExpression? value = null)
    {
        Value = value;
    }
}
