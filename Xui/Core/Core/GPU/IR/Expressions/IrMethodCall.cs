namespace Xui.GPU.IR;

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
