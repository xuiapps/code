namespace Xui.GPU.IR;

/// <summary>
/// Represents a matrix type (Float4x4, etc.).
/// </summary>
public class IrMatrixType : IrType
{
    /// <summary>Gets the IR node kind for this type.</summary>
    public override IrNodeKind Kind => IrNodeKind.MatrixType;
    /// <summary>Gets the display name of this matrix type.</summary>
    public override string Name => $"{ElementType.Name}{Rows}x{Columns}";

    /// <summary>Gets the scalar element type of the matrix.</summary>
    public IrScalarType ElementType { get; }
    /// <summary>Gets the number of rows.</summary>
    public int Rows { get; }
    /// <summary>Gets the number of columns.</summary>
    public int Columns { get; }

    /// <summary>
    /// Initializes a new instance of <see cref="IrMatrixType"/> with the specified element type and dimensions.
    /// </summary>
    public IrMatrixType(IrScalarType elementType, int rows, int columns)
    {
        if (rows < 2 || rows > 4 || columns < 2 || columns > 4)
            throw new ArgumentException($"Matrix dimensions must be 2-4, got {rows}x{columns}");

        ElementType = elementType;
        Rows = rows;
        Columns = columns;
    }
}
