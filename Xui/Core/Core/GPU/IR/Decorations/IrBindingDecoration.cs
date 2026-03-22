namespace Xui.GPU.IR;

/// <summary>
/// Represents a binding decoration for resources.
/// </summary>
public class IrBindingDecoration : IrDecoration
{
    /// <summary>Gets the IR node kind for this decoration.</summary>
    public override IrNodeKind Kind => IrNodeKind.Binding;
    /// <summary>Gets the binding group index.</summary>
    public int Group { get; }
    /// <summary>Gets the binding slot index within the group.</summary>
    public int Binding { get; }

    /// <summary>
    /// Initializes a new instance of <see cref="IrBindingDecoration"/> with the specified group and binding.
    /// </summary>
    public IrBindingDecoration(int group, int binding)
    {
        Group = group;
        Binding = binding;
    }
}
