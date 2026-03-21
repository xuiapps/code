namespace Xui.GPU.Hardware;

/// <summary>
/// Represents a compiled GPU shader ready for use in a hardware pipeline.
/// </summary>
public interface IGpuShader : IDisposable
{
    /// <summary>Gets the name of this shader.</summary>
    string Name { get; }
}
