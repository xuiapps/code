namespace Xui.GPU.IR;

/// <summary>
/// Represents a struct type.
/// </summary>
public class IrStructType : IrType
{
    public override IrNodeKind Kind => IrNodeKind.StructType;
    public override string Name { get; }

    public List<IrStructField> Fields { get; } = new();

    public IrStructType(string name)
    {
        Name = name;
    }
}

/// <summary>
/// Represents a field in a struct.
/// </summary>
public class IrStructField
{
    public string Name { get; set; } = string.Empty;
    public IrType Type { get; set; } = null!;
    public List<IrDecoration> Decorations { get; } = new();
}
