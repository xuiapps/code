namespace Xui.GPU.IR;

/// <summary>
/// Represents a vector type (Float2, Float3, Float4, Int2, etc.).
/// </summary>
public class IrVectorType : IrType
{
    /// <summary>Gets the IR node kind for this type.</summary>
    public override IrNodeKind Kind => IrNodeKind.VectorType;
    /// <summary>Gets the display name of this vector type.</summary>
    public override string Name => $"{ElementType.Name}{Dimension}";

    /// <summary>Gets the scalar element type of the vector.</summary>
    public IrScalarType ElementType { get; }
    /// <summary>Gets the number of components (2-4).</summary>
    public int Dimension { get; }

    /// <summary>
    /// Initializes a new instance of <see cref="IrVectorType"/> with the specified element type and dimension.
    /// </summary>
    public IrVectorType(IrScalarType elementType, int dimension)
    {
        if (dimension < 2 || dimension > 4)
            throw new ArgumentException($"Vector dimension must be 2-4, got {dimension}");

        ElementType = elementType;
        Dimension = dimension;
    }
}
