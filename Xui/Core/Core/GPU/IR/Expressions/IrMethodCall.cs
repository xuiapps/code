namespace Xui.GPU.IR;

/// <summary>
/// Represents a method call (shader intrinsic or user function).
/// </summary>
public class IrMethodCall : IrExpression
{
    /// <summary>Gets the node kind for this method call.</summary>
    public override IrNodeKind Kind => IrNodeKind.MethodCall;

    /// <summary>Gets the return type of the method call.</summary>
    public override IrType Type { get; }

    /// <summary>Gets the name of the method being called.</summary>
    public string MethodName { get; }

    /// <summary>Gets the list of arguments passed to the method.</summary>
    public List<IrExpression> Arguments { get; }

    /// <summary>Gets the optional target expression for instance method calls.</summary>
    public IrExpression? Target { get; }

    /// <summary>Initializes a new instance of the <see cref="IrMethodCall"/> class.</summary>
    public IrMethodCall(string methodName, List<IrExpression> arguments, IrType type, IrExpression? target = null)
    {
        MethodName = methodName;
        Arguments = arguments;
        Type = type;
        Target = target;
    }
}
