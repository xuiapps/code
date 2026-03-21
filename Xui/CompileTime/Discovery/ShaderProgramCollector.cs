using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Xui.CompileTime.Discovery;

/// <summary>
/// Discovers shader stages within a [ShaderProgram] struct.
/// </summary>
internal static class ShaderProgramCollector
{
    public static ShaderProgramInfo? Extract(GeneratorAttributeSyntaxContext ctx)
    {
        if (ctx.TargetSymbol is not INamedTypeSymbol programType)
            return null;

        var info = new ShaderProgramInfo
        {
            Namespace = programType.ContainingNamespace?.ToDisplayString() ?? "",
            TypeName = programType.Name
        };

        // Walk nested types looking for IVertexShader<,,> and IFragmentShader<,,>
        foreach (var member in programType.GetTypeMembers())
        {
            foreach (var iface in member.AllInterfaces)
            {
                if (iface.IsGenericType && iface.TypeArguments.Length == 3)
                {
                    var ifaceName = iface.OriginalDefinition.ToDisplayString();

                    bool isVertex = ifaceName == "Xui.GPU.Shaders.IVertexShader<TInput, TOutput, TBindings>";
                    bool isFragment = ifaceName == "Xui.GPU.Shaders.IFragmentShader<TInput, TOutput, TBindings>";

                    if (!isVertex && !isFragment)
                        continue;

                    var inputType = (INamedTypeSymbol)iface.TypeArguments[0];
                    var outputType = (INamedTypeSymbol)iface.TypeArguments[1];
                    var bindingsType = (INamedTypeSymbol)iface.TypeArguments[2];

                    // Find the Execute method syntax
                    var executeMethod = FindExecuteMethod(member, ctx);
                    if (executeMethod == null)
                        continue;

                    var stageInfo = new ShaderStageInfo
                    {
                        StageName = member.Name,
                        IsVertex = isVertex,
                        InputType = inputType,
                        OutputType = outputType,
                        BindingsType = bindingsType,
                        ExecuteMethodSyntax = executeMethod,
                        SemanticModel = ctx.SemanticModel,
                    };

                    if (isVertex) info.VertexStage = stageInfo;
                    else info.FragmentStage = stageInfo;
                }
            }
        }

        if (info.VertexStage == null && info.FragmentStage == null)
            return null;

        return info;
    }

    private static MethodDeclarationSyntax? FindExecuteMethod(
        INamedTypeSymbol stageType, GeneratorAttributeSyntaxContext ctx)
    {
        foreach (var syntaxRef in stageType.DeclaringSyntaxReferences)
        {
            var syntax = syntaxRef.GetSyntax();
            if (syntax is StructDeclarationSyntax structSyntax)
            {
                foreach (var memberSyntax in structSyntax.Members)
                {
                    if (memberSyntax is MethodDeclarationSyntax method
                        && method.Identifier.Text == "Execute")
                    {
                        return method;
                    }
                }
            }
        }

        return null;
    }
}
