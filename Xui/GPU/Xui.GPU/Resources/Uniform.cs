namespace Xui.GPU.Resources;

/// <summary>
/// Represents a uniform buffer resource containing constant shader data.
/// </summary>
/// <typeparam name="T">The uniform data structure type.</typeparam>
public readonly struct Uniform<T>
    where T : unmanaged
{
    /// <summary>
    /// Gets the uniform data value.
    /// </summary>
    public T Value { get; init; }

    /// <summary>
    /// Initializes a new instance of the <see cref="Uniform{T}"/> struct.
    /// </summary>
    /// <param name="value">The uniform data.</param>
    public Uniform(T value)
    {
        Value = value;
    }
}
