namespace Xui.GPU.Shaders.Types;

/// <summary>
/// Shader boolean type.
/// </summary>
public readonly struct Bool
{
    private readonly bool _value;

    /// <summary>
    /// Initializes a new instance of the <see cref="Bool"/> struct.
    /// </summary>
    /// <param name="value">The bool value.</param>
    public Bool(bool value)
    {
        _value = value;
    }

    /// <summary>
    /// Gets the false constant.
    /// </summary>
    public static Bool False => new(false);

    /// <summary>
    /// Gets the true constant.
    /// </summary>
    public static Bool True => new(true);

    /// <summary>
    /// Converts from bool to Bool.
    /// </summary>
    public static implicit operator Bool(bool value) => new(value);

    /// <summary>
    /// Converts from Bool to bool.
    /// </summary>
    public static implicit operator bool(Bool value) => value._value;

    /// <summary>
    /// Tests equality of two Bool values.
    /// </summary>
    public static Bool operator ==(Bool left, Bool right) => new(left._value == right._value);

    /// <summary>
    /// Tests inequality of two Bool values.
    /// </summary>
    public static Bool operator !=(Bool left, Bool right) => new(left._value != right._value);

    /// <summary>
    /// Performs logical AND of two Bool values.
    /// </summary>
    public static Bool operator &(Bool left, Bool right) => new(left._value && right._value);

    /// <summary>
    /// Performs logical OR of two Bool values.
    /// </summary>
    public static Bool operator |(Bool left, Bool right) => new(left._value || right._value);

    /// <summary>
    /// Performs logical NOT of a Bool value.
    /// </summary>
    public static Bool operator !(Bool value) => new(!value._value);

    /// <inheritdoc/>
    public override bool Equals(object? obj) => obj is Bool other && _value == other._value;

    /// <inheritdoc/>
    public override int GetHashCode() => _value.GetHashCode();

    /// <inheritdoc/>
    public override string ToString() => _value.ToString();
}
