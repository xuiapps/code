namespace Xui.GPU.IR;

/// <summary>
/// Represents a binary operation.
/// </summary>
public class IrBinaryOp : IrExpression
{
    public override IrNodeKind Kind => IrNodeKind.BinaryOp;
    public override IrType Type { get; }
    public BinaryOperator Operator { get; }
    public IrExpression Left { get; }
    public IrExpression Right { get; }

    public IrBinaryOp(BinaryOperator op, IrExpression left, IrExpression right, IrType type)
    {
        Operator = op;
        Left = left;
        Right = right;
        Type = type;
    }
}
