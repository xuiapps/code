namespace Xui.GPU.IR;

/// <summary>
/// Represents a location decoration for vertex inputs/outputs.
/// </summary>
public class IrLocationDecoration : IrDecoration
{
    /// <summary>Gets the IR node kind for this decoration.</summary>
    public override IrNodeKind Kind => IrNodeKind.Location;
    /// <summary>Gets the location index.</summary>
    public int Location { get; }

    /// <summary>
    /// Initializes a new instance of <see cref="IrLocationDecoration"/> with the specified location.
    /// </summary>
    public IrLocationDecoration(int location)
    {
        Location = location;
    }
}
