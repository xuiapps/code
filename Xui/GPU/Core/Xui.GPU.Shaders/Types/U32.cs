namespace Xui.GPU.Shaders.Types;

/// <summary>
/// Shader 32-bit unsigned integer scalar type.
/// </summary>
public readonly struct U32
{
    private readonly uint _value;

    /// <summary>
    /// Initializes a new instance of the <see cref="U32"/> struct.
    /// </summary>
    /// <param name="value">The uint value.</param>
    public U32(uint value)
    {
        _value = value;
    }

    /// <summary>
    /// Gets the zero constant.
    /// </summary>
    public static U32 Zero => new(0);

    /// <summary>
    /// Gets the one constant.
    /// </summary>
    public static U32 One => new(1);

    /// <summary>
    /// Converts from uint to U32.
    /// </summary>
    public static implicit operator U32(uint value) => new(value);

    /// <summary>
    /// Converts from U32 to uint.
    /// </summary>
    public static implicit operator uint(U32 value) => value._value;

    /// <inheritdoc/>
    public override string ToString() => _value.ToString();
}
