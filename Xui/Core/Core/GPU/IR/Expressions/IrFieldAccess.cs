namespace Xui.GPU.IR;

/// <summary>
/// Represents a field access.
/// </summary>
public class IrFieldAccess : IrExpression
{
    public override IrNodeKind Kind => IrNodeKind.Field;
    public override IrType Type { get; }
    public IrExpression Object { get; }
    public string FieldName { get; }

    public IrFieldAccess(IrExpression obj, string fieldName, IrType type)
    {
        Object = obj;
        FieldName = fieldName;
        Type = type;
    }
}
