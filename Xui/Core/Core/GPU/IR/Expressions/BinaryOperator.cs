namespace Xui.GPU.IR;

/// <summary>
/// Binary operator kind.
/// </summary>
public enum BinaryOperator
{
    Add, Subtract, Multiply, Divide, Modulo,
    Equal, NotEqual, LessThan, LessThanOrEqual, GreaterThan, GreaterThanOrEqual,
    LogicalAnd, LogicalOr,
    BitwiseAnd, BitwiseOr, BitwiseXor,
    ShiftLeft, ShiftRight
}
