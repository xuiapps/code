namespace Xui.GPU.IR;

/// <summary>
/// Represents a vector type (Float2, Float3, Float4, Int2, etc.).
/// </summary>
public class IrVectorType : IrType
{
    public override IrNodeKind Kind => IrNodeKind.VectorType;
    public override string Name => $"{ElementType.Name}{Dimension}";

    public IrScalarType ElementType { get; }
    public int Dimension { get; }

    public IrVectorType(IrScalarType elementType, int dimension)
    {
        if (dimension < 2 || dimension > 4)
            throw new ArgumentException($"Vector dimension must be 2-4, got {dimension}");

        ElementType = elementType;
        Dimension = dimension;
    }
}
