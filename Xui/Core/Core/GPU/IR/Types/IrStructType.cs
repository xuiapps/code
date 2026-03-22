namespace Xui.GPU.IR;

/// <summary>
/// Represents a struct type.
/// </summary>
public class IrStructType : IrType
{
    /// <summary>Gets the IR node kind for this type.</summary>
    public override IrNodeKind Kind => IrNodeKind.StructType;
    /// <summary>Gets the name of this struct type.</summary>
    public override string Name { get; }

    /// <summary>Gets the list of fields in this struct.</summary>
    public List<IrStructField> Fields { get; } = new();

    /// <summary>
    /// Initializes a new instance of <see cref="IrStructType"/> with the specified name.
    /// </summary>
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
    /// <summary>Gets or sets the field name.</summary>
    public string Name { get; set; } = string.Empty;
    /// <summary>Gets or sets the field type.</summary>
    public IrType Type { get; set; } = null!;
    /// <summary>Gets the list of decorations applied to this field.</summary>
    public List<IrDecoration> Decorations { get; } = new();
}
