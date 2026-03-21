namespace Xui.GPU.IR;

/// <summary>
/// Represents a matrix type (Float4x4, etc.).
/// </summary>
public class IrMatrixType : IrType
{
    public override IrNodeKind Kind => IrNodeKind.MatrixType;
    public override string Name => $"{ElementType.Name}{Rows}x{Columns}";

    public IrScalarType ElementType { get; }
    public int Rows { get; }
    public int Columns { get; }

    public IrMatrixType(IrScalarType elementType, int rows, int columns)
    {
        if (rows < 2 || rows > 4 || columns < 2 || columns > 4)
            throw new ArgumentException($"Matrix dimensions must be 2-4, got {rows}x{columns}");

        ElementType = elementType;
        Rows = rows;
        Columns = columns;
    }
}
