namespace Xui.GPU.Shaders.Types;

/// <summary>
/// Shader 32-bit signed integer scalar type.
/// </summary>
public readonly struct I32
{
    private readonly int _value;

    /// <summary>
    /// Initializes a new instance of the <see cref="I32"/> struct.
    /// </summary>
    /// <param name="value">The int value.</param>
    public I32(int value)
    {
        _value = value;
    }

    /// <summary>
    /// Gets the zero constant.
    /// </summary>
    public static I32 Zero => new(0);

    /// <summary>
    /// Gets the one constant.
    /// </summary>
    public static I32 One => new(1);

    /// <summary>
    /// Converts from int to I32.
    /// </summary>
    public static implicit operator I32(int value) => new(value);

    /// <summary>
    /// Converts from I32 to int.
    /// </summary>
    public static implicit operator int(I32 value) => value._value;

    /// <inheritdoc/>
    public override string ToString() => _value.ToString();
}
