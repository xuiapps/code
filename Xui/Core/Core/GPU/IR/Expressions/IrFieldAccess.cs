namespace Xui.GPU.IR;

/// <summary>
/// Represents a field access.
/// </summary>
public class IrFieldAccess : IrExpression
{
    /// <summary>Gets the node kind for this field access.</summary>
    public override IrNodeKind Kind => IrNodeKind.Field;

    /// <summary>Gets the result type of the field access.</summary>
    public override IrType Type { get; }

    /// <summary>Gets the expression representing the object being accessed.</summary>
    public IrExpression Object { get; }

    /// <summary>Gets the name of the field being accessed.</summary>
    public string FieldName { get; }

    /// <summary>Initializes a new instance of the <see cref="IrFieldAccess"/> class.</summary>
    public IrFieldAccess(IrExpression obj, string fieldName, IrType type)
    {
        Object = obj;
        FieldName = fieldName;
        Type = type;
    }
}
