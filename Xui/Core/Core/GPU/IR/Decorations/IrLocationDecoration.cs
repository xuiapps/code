namespace Xui.GPU.IR;

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
