namespace Xui.GPU.IR;

/// <summary>
/// Base class for decoration nodes (attributes).
/// </summary>
public abstract class IrDecoration : IrNode
{
}

/// <summary>
/// Represents a location decoration for vertex inputs/outputs.
/// </summary>
public class IrLocationDecoration : IrDecoration
{
    public override IrNodeKind Kind => IrNodeKind.Location;
    public int Location { get; }

    public IrLocationDecoration(int location)
    {
        Location = location;
    }
}

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

/// <summary>
/// Built-in semantic kinds.
/// </summary>
public enum BuiltInSemantic
{
    Position,
    FragDepth,
    InstanceId,
    VertexId
}

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

/// <summary>
/// Represents an interpolation mode decoration.
/// </summary>
public class IrInterpolationDecoration : IrDecoration
{
    public override IrNodeKind Kind => IrNodeKind.Interpolation;
    public InterpolationMode Mode { get; }

    public IrInterpolationDecoration(InterpolationMode mode)
    {
        Mode = mode;
    }
}

/// <summary>
/// Interpolation mode for varyings.
/// </summary>
public enum InterpolationMode
{
    Linear,
    Flat,
    Perspective
}
