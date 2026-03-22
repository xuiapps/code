namespace Xui.GPU.IR;

/// <summary>
/// Binary operator kind.
/// </summary>
public enum BinaryOperator
{
    /// <summary>Addition operator (+).</summary>
    Add,
    /// <summary>Subtraction operator (-).</summary>
    Subtract,
    /// <summary>Multiplication operator (*).</summary>
    Multiply,
    /// <summary>Division operator (/).</summary>
    Divide,
    /// <summary>Modulo operator (%).</summary>
    Modulo,
    /// <summary>Equality comparison (==).</summary>
    Equal,
    /// <summary>Inequality comparison (!=).</summary>
    NotEqual,
    /// <summary>Less-than comparison (&lt;).</summary>
    LessThan,
    /// <summary>Less-than-or-equal comparison (&lt;=).</summary>
    LessThanOrEqual,
    /// <summary>Greater-than comparison (&gt;).</summary>
    GreaterThan,
    /// <summary>Greater-than-or-equal comparison (&gt;=).</summary>
    GreaterThanOrEqual,
    /// <summary>Logical AND (&&).</summary>
    LogicalAnd,
    /// <summary>Logical OR (||).</summary>
    LogicalOr,
    /// <summary>Bitwise AND (&amp;).</summary>
    BitwiseAnd,
    /// <summary>Bitwise OR (|).</summary>
    BitwiseOr,
    /// <summary>Bitwise XOR (^).</summary>
    BitwiseXor,
    /// <summary>Left bit shift (&lt;&lt;).</summary>
    ShiftLeft,
    /// <summary>Right bit shift (&gt;&gt;).</summary>
    ShiftRight
}
