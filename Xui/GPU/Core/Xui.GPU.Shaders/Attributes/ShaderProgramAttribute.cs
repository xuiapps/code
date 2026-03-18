namespace Xui.GPU.Shaders.Attributes;

/// <summary>
/// Marks a type as a shader program definition.
/// Used by source generators to discover and process shader programs.
/// </summary>
[AttributeUsage(AttributeTargets.Struct, AllowMultiple = false, Inherited = false)]
public sealed class ShaderProgramAttribute : Attribute
{
}
