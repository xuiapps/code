namespace Xui.GPU.Shaders.Types;

/// <summary>
/// Shader 32-bit floating-point scalar type.
/// This is the fundamental scalar type for shader math operations.
/// </summary>
/// <remarks>
/// In CPU execution, this may be represented as a single float (in vertex stage)
/// or as a quad of floats (in fragment stage for derivative support).
/// </remarks>
public readonly struct F32
{
    private readonly float _value;

    /// <summary>
    /// Initializes a new instance of the <see cref="F32"/> struct.
    /// </summary>
    /// <param name="value">The float value.</param>
    public F32(float value)
    {
        _value = value;
    }

    /// <summary>
    /// Gets the zero constant.
    /// </summary>
    public static F32 Zero => new(0.0f);

    /// <summary>
    /// Gets the one constant.
    /// </summary>
    public static F32 One => new(1.0f);

    /// <summary>
    /// Converts from float to F32.
    /// </summary>
    public static implicit operator F32(float value) => new(value);

    /// <summary>
    /// Converts from F32 to float.
    /// </summary>
    public static implicit operator float(F32 value) => value._value;

    /// <summary>
    /// Adds two F32 values.
    /// </summary>
    public static F32 operator +(F32 left, F32 right) => new(left._value + right._value);

    /// <summary>
    /// Subtracts two F32 values.
    /// </summary>
    public static F32 operator -(F32 left, F32 right) => new(left._value - right._value);

    /// <summary>
    /// Multiplies two F32 values.
    /// </summary>
    public static F32 operator *(F32 left, F32 right) => new(left._value * right._value);

    /// <summary>
    /// Divides two F32 values.
    /// </summary>
    public static F32 operator /(F32 left, F32 right) => new(left._value / right._value);

    /// <summary>
    /// Negates an F32 value.
    /// </summary>
    public static F32 operator -(F32 value) => new(-value._value);

    /// <summary>
    /// Tests equality of two F32 values.
    /// </summary>
    public static Bool operator ==(F32 left, F32 right) => new(left._value == right._value);

    /// <summary>
    /// Tests inequality of two F32 values.
    /// </summary>
    public static Bool operator !=(F32 left, F32 right) => new(left._value != right._value);

    /// <summary>
    /// Tests if left is less than right.
    /// </summary>
    public static Bool operator <(F32 left, F32 right) => new(left._value < right._value);

    /// <summary>
    /// Tests if left is greater than right.
    /// </summary>
    public static Bool operator >(F32 left, F32 right) => new(left._value > right._value);

    /// <summary>
    /// Tests if left is less than or equal to right.
    /// </summary>
    public static Bool operator <=(F32 left, F32 right) => new(left._value <= right._value);

    /// <summary>
    /// Tests if left is greater than or equal to right.
    /// </summary>
    public static Bool operator >=(F32 left, F32 right) => new(left._value >= right._value);

    /// <inheritdoc/>
    public override bool Equals(object? obj) => obj is F32 other && _value == other._value;

    /// <inheritdoc/>
    public override int GetHashCode() => _value.GetHashCode();

    /// <inheritdoc/>
    public override string ToString() => _value.ToString();
}
