namespace Xui.GPU.IR;

/// <summary>
/// Represents a built-in semantic decoration.
/// </summary>
public class IrBuiltInDecoration : IrDecoration
{
    public override IrNodeKind Kind => IrNodeKind.BuiltIn;
    public BuiltInSemantic Semantic { get; }

    public IrBuiltInDecoration(BuiltInSemantic semantic)
    {
        Semantic = semantic;
    }
}
