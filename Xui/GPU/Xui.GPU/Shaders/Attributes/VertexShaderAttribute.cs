namespace Xui.GPU.Shaders.Attributes;

/// <summary>
/// Marks a type as a vertex shader stage.
/// </summary>
[AttributeUsage(AttributeTargets.Struct, AllowMultiple = false, Inherited = false)]
public sealed class VertexShaderAttribute : Attribute
{
}
