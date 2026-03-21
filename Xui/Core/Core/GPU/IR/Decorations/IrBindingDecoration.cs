namespace Xui.GPU.IR;

/// <summary>
/// Represents a binding decoration for resources.
/// </summary>
public class IrBindingDecoration : IrDecoration
{
    public override IrNodeKind Kind => IrNodeKind.Binding;
    public int Group { get; }
    public int Binding { get; }

    public IrBindingDecoration(int group, int binding)
    {
        Group = group;
        Binding = binding;
    }
}
