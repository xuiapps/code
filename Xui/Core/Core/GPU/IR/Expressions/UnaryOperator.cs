namespace Xui.GPU.IR;

/// <summary>
/// Unary operator kind.
/// </summary>
public enum UnaryOperator
{
    /// <summary>Arithmetic negation (-).</summary>
    Negate,
    /// <summary>Logical NOT (!).</summary>
    LogicalNot,
    /// <summary>Bitwise NOT (~).</summary>
    BitwiseNot
}
