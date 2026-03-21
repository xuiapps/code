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

/// <summary>
/// Represents a scalar type (F32, I32, U32, Bool).
/// </summary>
public class IrScalarType : IrType
{
    public override IrNodeKind Kind => IrNodeKind.ScalarType;
    public override string Name { get; }
    public ScalarKind ScalarKind { get; }

    public IrScalarType(ScalarKind kind)
    {
        ScalarKind = kind;
        Name = kind switch
        {
            ScalarKind.F32 => "F32",
            ScalarKind.I32 => "I32",
            ScalarKind.U32 => "U32",
            ScalarKind.Bool => "Bool",
            _ => throw new ArgumentException($"Unknown scalar kind: {kind}")
        };
    }
}

/// <summary>
/// Kind of scalar type.
/// </summary>
public enum ScalarKind
{
    F32,
    I32,
    U32,
    Bool
}

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

/// <summary>
/// Represents a texture type.
/// </summary>
public class IrTextureType : IrType
{
    public override IrNodeKind Kind => IrNodeKind.TextureType;
    public override string Name => $"Texture2D<{PixelType.Name}>";
    
    public IrType PixelType { get; }

    public IrTextureType(IrType pixelType)
    {
        PixelType = pixelType;
    }
}

/// <summary>
/// Represents a sampler type.
/// </summary>
public class IrSamplerType : IrType
{
    public override IrNodeKind Kind => IrNodeKind.SamplerType;
    public override string Name => "Sampler";

    public static readonly IrSamplerType Instance = new();
}
