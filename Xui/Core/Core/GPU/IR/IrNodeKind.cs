namespace Xui.GPU.IR;

/// <summary>
/// Enumeration of all IR node kinds.
/// </summary>
public enum IrNodeKind
{
    /// <summary>Scalar type node.</summary>
    ScalarType,
    /// <summary>Vector type node.</summary>
    VectorType,
    /// <summary>Matrix type node.</summary>
    MatrixType,
    /// <summary>Struct type node.</summary>
    StructType,
    /// <summary>Texture type node.</summary>
    TextureType,
    /// <summary>Sampler type node.</summary>
    SamplerType,

    /// <summary>Constant expression.</summary>
    Constant,
    /// <summary>Parameter reference expression.</summary>
    Parameter,
    /// <summary>Field access expression.</summary>
    Field,
    /// <summary>Binary operation expression.</summary>
    BinaryOp,
    /// <summary>Unary operation expression.</summary>
    UnaryOp,
    /// <summary>Method call expression.</summary>
    MethodCall,
    /// <summary>Constructor call expression.</summary>
    Constructor,

    /// <summary>Variable declaration statement.</summary>
    VarDecl,
    /// <summary>Assignment statement.</summary>
    Assignment,
    /// <summary>Return statement.</summary>
    Return,
    /// <summary>Conditional statement.</summary>
    If,
    /// <summary>Block of statements.</summary>
    Block,

    /// <summary>Location decoration.</summary>
    Location,
    /// <summary>Built-in semantic decoration.</summary>
    BuiltIn,
    /// <summary>Resource binding decoration.</summary>
    Binding,
    /// <summary>Interpolation mode decoration.</summary>
    Interpolation,

    /// <summary>Shader module node.</summary>
    ShaderModule,
    /// <summary>Vertex shader stage.</summary>
    VertexStage,
    /// <summary>Fragment shader stage.</summary>
    FragmentStage,
    /// <summary>Struct declaration.</summary>
    StructDecl,
    /// <summary>Function declaration.</summary>
    FunctionDecl,
}
