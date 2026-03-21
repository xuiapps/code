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
