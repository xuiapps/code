namespace Xui.GPU.IR;

/// <summary>
/// Represents a location in source code.
/// Used to map IR nodes back to original C# source for error reporting.
/// </summary>
public class SourceLocation
{
    /// <summary>
    /// Gets the source file path.
    /// </summary>
    public string FilePath { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets the line number (1-based).
    /// </summary>
    public int Line { get; set; }
    
    /// <summary>
    /// Gets the column number (1-based).
    /// </summary>
    public int Column { get; set; }
    
    /// <summary>
    /// Gets a formatted string representation of this location.
    /// </summary>
    public string ToDisplayString() => $"{FilePath}({Line},{Column})";
}

/// <summary>
/// Base class for all IR nodes in the intermediate representation.
/// </summary>
public abstract class IrNode
{
    /// <summary>
    /// Gets the node kind for pattern matching and visitors.
    /// </summary>
    public abstract IrNodeKind Kind { get; }
    
    /// <summary>
    /// Gets or sets the source location where this node originates from.
    /// Used to generate accurate error messages mapping back to C# source.
    /// </summary>
    public SourceLocation? SourceLocation { get; set; }
}

/// <summary>
/// Enumeration of all IR node kinds.
/// </summary>
public enum IrNodeKind
{
    // Type nodes
    ScalarType,
    VectorType,
    MatrixType,
    StructType,
    TextureType,
    SamplerType,
    
    // Expression nodes
    Constant,
    Parameter,
    Field,
    BinaryOp,
    UnaryOp,
    MethodCall,
    Constructor,
    
    // Statement nodes
    VarDecl,
    Assignment,
    Return,
    If,
    Block,
    
    // Decoration nodes
    Location,
    BuiltIn,
    Binding,
    Interpolation,
    
    // Module and stage nodes
    ShaderModule,
    VertexStage,
    FragmentStage,
    StructDecl,
    FunctionDecl,
}
