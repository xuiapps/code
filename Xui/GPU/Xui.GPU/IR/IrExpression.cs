namespace Xui.GPU.IR;

/// <summary>
/// Base class for all expression nodes in the IR.
/// </summary>
public abstract class IrExpression : IrNode
{
    /// <summary>
    /// Gets the type of this expression.
    /// </summary>
    public abstract IrType Type { get; }
}

/// <summary>
/// Represents a constant value.
/// </summary>
public class IrConstant : IrExpression
{
    public override IrNodeKind Kind => IrNodeKind.Constant;
    public override IrType Type { get; }
    public object Value { get; }

    public IrConstant(IrType type, object value)
    {
        Type = type;
        Value = value;
    }
}

/// <summary>
/// Represents a parameter reference.
/// </summary>
public class IrParameter : IrExpression
{
    public override IrNodeKind Kind => IrNodeKind.Parameter;
    public override IrType Type { get; }
    public string Name { get; }

    public IrParameter(string name, IrType type)
    {
        Name = name;
        Type = type;
    }
}

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

/// <summary>
/// Unary operator kind.
/// </summary>
public enum UnaryOperator
{
    Negate, LogicalNot, BitwiseNot
}

/// <summary>
/// Represents a unary operation.
/// </summary>
public class IrUnaryOp : IrExpression
{
    public override IrNodeKind Kind => IrNodeKind.UnaryOp;
    public override IrType Type { get; }
    public UnaryOperator Operator { get; }
    public IrExpression Operand { get; }

    public IrUnaryOp(UnaryOperator op, IrExpression operand, IrType type)
    {
        Operator = op;
        Operand = operand;
        Type = type;
    }
}

/// <summary>
/// Represents a method call (shader intrinsic or user function).
/// </summary>
public class IrMethodCall : IrExpression
{
    public override IrNodeKind Kind => IrNodeKind.MethodCall;
    public override IrType Type { get; }
    public string MethodName { get; }
    public List<IrExpression> Arguments { get; }
    public IrExpression? Target { get; }

    public IrMethodCall(string methodName, List<IrExpression> arguments, IrType type, IrExpression? target = null)
    {
        MethodName = methodName;
        Arguments = arguments;
        Type = type;
        Target = target;
    }
}

/// <summary>
/// Represents a constructor call.
/// </summary>
public class IrConstructor : IrExpression
{
    public override IrNodeKind Kind => IrNodeKind.Constructor;
    public override IrType Type { get; }
    public List<IrExpression> Arguments { get; }

    public IrConstructor(IrType type, List<IrExpression> arguments)
    {
        Type = type;
        Arguments = arguments;
    }
}
