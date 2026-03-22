namespace Xui.GPU.IR;

/// <summary>
/// Represents a complete shader module with all stages and declarations.
/// </summary>
public class IrShaderModule : IrNode
{
    /// <summary>Gets the IR node kind for this module.</summary>
    public override IrNodeKind Kind => IrNodeKind.ShaderModule;

    /// <summary>Gets or sets the module name.</summary>
    public string Name { get; set; } = string.Empty;
    /// <summary>Gets the list of struct declarations.</summary>
    public List<IrStructDecl> Structs { get; } = new();
    /// <summary>Gets the list of function declarations.</summary>
    public List<IrFunctionDecl> Functions { get; } = new();
    /// <summary>Gets or sets the optional vertex shader stage.</summary>
    public IrVertexStage? VertexStage { get; set; }
    /// <summary>Gets or sets the optional fragment shader stage.</summary>
    public IrFragmentStage? FragmentStage { get; set; }
}
