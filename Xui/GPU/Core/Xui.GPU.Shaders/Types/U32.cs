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

    /// <summary>
    /// Adds two U32 values.
    /// </summary>
    public static U32 operator +(U32 left, U32 right) => new(left._value + right._value);

    /// <summary>
    /// Subtracts two U32 values.
    /// </summary>
    public static U32 operator -(U32 left, U32 right) => new(left._value - right._value);

    /// <summary>
    /// Multiplies two U32 values.
    /// </summary>
    public static U32 operator *(U32 left, U32 right) => new(left._value * right._value);

    /// <summary>
    /// Divides two U32 values.
    /// </summary>
    public static U32 operator /(U32 left, U32 right) => new(left._value / right._value);

    /// <summary>
    /// Tests equality of two U32 values.
    /// </summary>
    public static Bool operator ==(U32 left, U32 right) => new(left._value == right._value);

    /// <summary>
    /// Tests inequality of two U32 values.
    /// </summary>
    public static Bool operator !=(U32 left, U32 right) => new(left._value != right._value);

    /// <summary>
    /// Tests if left is less than right.
    /// </summary>
    public static Bool operator <(U32 left, U32 right) => new(left._value < right._value);

    /// <summary>
    /// Tests if left is greater than right.
    /// </summary>
    public static Bool operator >(U32 left, U32 right) => new(left._value > right._value);

    /// <summary>
    /// Tests if left is less than or equal to right.
    /// </summary>
    public static Bool operator <=(U32 left, U32 right) => new(left._value <= right._value);

    /// <summary>
    /// Tests if left is greater than or equal to right.
    /// </summary>
    public static Bool operator >=(U32 left, U32 right) => new(left._value >= right._value);

    /// <summary>
    /// Performs bitwise AND of two U32 values.
    /// </summary>
    public static U32 operator &(U32 left, U32 right) => new(left._value & right._value);

    /// <summary>
    /// Performs bitwise OR of two U32 values.
    /// </summary>
    public static U32 operator |(U32 left, U32 right) => new(left._value | right._value);

    /// <summary>
    /// Performs bitwise XOR of two U32 values.
    /// </summary>
    public static U32 operator ^(U32 left, U32 right) => new(left._value ^ right._value);

    /// <summary>
    /// Performs bitwise NOT of a U32 value.
    /// </summary>
    public static U32 operator ~(U32 value) => new(~value._value);

    /// <summary>
    /// Left shifts a U32 value.
    /// </summary>
    public static U32 operator <<(U32 left, int right) => new(left._value << right);

    /// <summary>
    /// Right shifts a U32 value.
    /// </summary>
    public static U32 operator >>(U32 left, int right) => new(left._value >> right);

    /// <inheritdoc/>
    public override bool Equals(object? obj) => obj is U32 other && _value == other._value;

    /// <inheritdoc/>
    public override int GetHashCode() => _value.GetHashCode();

    /// <inheritdoc/>
    public override string ToString() => _value.ToString();
}
