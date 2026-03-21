namespace Xui.GPU.Shaders.Attributes;

/// <summary>
/// Marks a type as a fragment shader stage.
/// </summary>
[AttributeUsage(AttributeTargets.Struct, AllowMultiple = false, Inherited = false)]
public sealed class FragmentShaderAttribute : Attribute
{
}
