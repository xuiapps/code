namespace Xui.GPU.IR;

/// <summary>
/// Represents a built-in semantic decoration.
/// </summary>
public class IrBuiltInDecoration : IrDecoration
{
    /// <summary>Gets the IR node kind for this decoration.</summary>
    public override IrNodeKind Kind => IrNodeKind.BuiltIn;
    /// <summary>Gets the built-in semantic.</summary>
    public BuiltInSemantic Semantic { get; }

    /// <summary>
    /// Initializes a new instance of <see cref="IrBuiltInDecoration"/> with the specified semantic.
    /// </summary>
    public IrBuiltInDecoration(BuiltInSemantic semantic)
    {
        Semantic = semantic;
    }
}
