namespace Xui.GPU.IR;

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
