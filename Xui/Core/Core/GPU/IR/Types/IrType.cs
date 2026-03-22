namespace Xui.GPU.IR;

/// <summary>
/// Base class for all type nodes in the IR.
/// </summary>
public abstract class IrType : IrNode
{
    /// <summary>
    /// Gets the name of this type.
    /// </summary>
    public abstract string Name { get; }
}
