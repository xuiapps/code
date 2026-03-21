namespace Xui.GPU.IR;

/// <summary>
/// Represents a complete shader module with all stages and declarations.
/// </summary>
public class IrShaderModule : IrNode
{
    public override IrNodeKind Kind => IrNodeKind.ShaderModule;
    
    public string Name { get; set; } = string.Empty;
    public List<IrStructDecl> Structs { get; } = new();
    public List<IrFunctionDecl> Functions { get; } = new();
    public IrVertexStage? VertexStage { get; set; }
    public IrFragmentStage? FragmentStage { get; set; }
}

/// <summary>
/// Represents a struct declaration.
/// </summary>
public class IrStructDecl : IrNode
{
    public override IrNodeKind Kind => IrNodeKind.StructDecl;
    public IrStructType Type { get; }

    public IrStructDecl(IrStructType type)
    {
        Type = type;
    }
}

/// <summary>
/// Represents a function declaration.
/// </summary>
public class IrFunctionDecl : IrNode
{
    public override IrNodeKind Kind => IrNodeKind.FunctionDecl;
    public string Name { get; set; } = string.Empty;
    public IrType ReturnType { get; set; } = null!;
    public List<IrParameter> Parameters { get; } = new();
    public IrBlock Body { get; set; } = new();
}

/// <summary>
/// Represents a vertex shader stage.
/// </summary>
public class IrVertexStage : IrNode
{
    public override IrNodeKind Kind => IrNodeKind.VertexStage;
    
    public string Name { get; set; } = string.Empty;
    public IrStructType InputType { get; set; } = null!;
    public IrStructType OutputType { get; set; } = null!;
    public IrStructType? BindingsType { get; set; }
    public IrBlock Body { get; set; } = new();
}

/// <summary>
/// Represents a fragment shader stage.
/// </summary>
public class IrFragmentStage : IrNode
{
    public override IrNodeKind Kind => IrNodeKind.FragmentStage;
    
    public string Name { get; set; } = string.Empty;
    public IrStructType InputType { get; set; } = null!;
    public IrStructType OutputType { get; set; } = null!;
    public IrStructType? BindingsType { get; set; }
    public IrBlock Body { get; set; } = new();
}
