namespace Xui.GPU.Backends;

/// <summary>
/// Base interface for shader backend code generators.
/// </summary>
public interface IShaderBackend
{
    /// <summary>
    /// Gets the name of this backend (e.g., "HLSL", "MSL", "GLSL").
    /// </summary>
    string Name { get; }
    
    /// <summary>
    /// Generates shader code from the IR module.
    /// </summary>
    /// <param name="module">The IR module to generate code from.</param>
    /// <returns>The generated shader code as a string.</returns>
    string GenerateCode(IR.IrShaderModule module);
}
