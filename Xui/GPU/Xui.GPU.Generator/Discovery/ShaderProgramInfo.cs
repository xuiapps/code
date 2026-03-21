using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Xui.GPU.Generator.Discovery;

internal class ShaderProgramInfo
{
    public string Namespace { get; set; } = "";
    public string TypeName { get; set; } = "";
    public string FullTypeName => string.IsNullOrEmpty(Namespace) ? TypeName : $"{Namespace}.{TypeName}";
    public ShaderStageInfo? VertexStage { get; set; }
    public ShaderStageInfo? FragmentStage { get; set; }
}

internal class ShaderStageInfo
{
    public string StageName { get; set; } = "";
    public bool IsVertex { get; set; }
    public INamedTypeSymbol InputType { get; set; } = null!;
    public INamedTypeSymbol OutputType { get; set; } = null!;
    public INamedTypeSymbol? BindingsType { get; set; }
    public MethodDeclarationSyntax ExecuteMethodSyntax { get; set; } = null!;
    public SemanticModel SemanticModel { get; set; } = null!;
}
