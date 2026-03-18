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

    /// <summary>
    /// Adds two I32 values.
    /// </summary>
    public static I32 operator +(I32 left, I32 right) => new(left._value + right._value);

    /// <summary>
    /// Subtracts two I32 values.
    /// </summary>
    public static I32 operator -(I32 left, I32 right) => new(left._value - right._value);

    /// <summary>
    /// Multiplies two I32 values.
    /// </summary>
    public static I32 operator *(I32 left, I32 right) => new(left._value * right._value);

    /// <summary>
    /// Divides two I32 values.
    /// </summary>
    public static I32 operator /(I32 left, I32 right) => new(left._value / right._value);

    /// <summary>
    /// Negates an I32 value.
    /// </summary>
    public static I32 operator -(I32 value) => new(-value._value);

    /// <summary>
    /// Tests equality of two I32 values.
    /// </summary>
    public static Bool operator ==(I32 left, I32 right) => new(left._value == right._value);

    /// <summary>
    /// Tests inequality of two I32 values.
    /// </summary>
    public static Bool operator !=(I32 left, I32 right) => new(left._value != right._value);

    /// <summary>
    /// Tests if left is less than right.
    /// </summary>
    public static Bool operator <(I32 left, I32 right) => new(left._value < right._value);

    /// <summary>
    /// Tests if left is greater than right.
    /// </summary>
    public static Bool operator >(I32 left, I32 right) => new(left._value > right._value);

    /// <summary>
    /// Tests if left is less than or equal to right.
    /// </summary>
    public static Bool operator <=(I32 left, I32 right) => new(left._value <= right._value);

    /// <summary>
    /// Tests if left is greater than or equal to right.
    /// </summary>
    public static Bool operator >=(I32 left, I32 right) => new(left._value >= right._value);

    /// <summary>
    /// Performs bitwise AND of two I32 values.
    /// </summary>
    public static I32 operator &(I32 left, I32 right) => new(left._value & right._value);

    /// <summary>
    /// Performs bitwise OR of two I32 values.
    /// </summary>
    public static I32 operator |(I32 left, I32 right) => new(left._value | right._value);

    /// <summary>
    /// Performs bitwise XOR of two I32 values.
    /// </summary>
    public static I32 operator ^(I32 left, I32 right) => new(left._value ^ right._value);

    /// <summary>
    /// Performs bitwise NOT of an I32 value.
    /// </summary>
    public static I32 operator ~(I32 value) => new(~value._value);

    /// <summary>
    /// Left shifts an I32 value.
    /// </summary>
    public static I32 operator <<(I32 left, int right) => new(left._value << right);

    /// <summary>
    /// Right shifts an I32 value.
    /// </summary>
    public static I32 operator >>(I32 left, int right) => new(left._value >> right);

    /// <inheritdoc/>
    public override bool Equals(object? obj) => obj is I32 other && _value == other._value;

    /// <inheritdoc/>
    public override int GetHashCode() => _value.GetHashCode();

    /// <inheritdoc/>
    public override string ToString() => _value.ToString();
}
