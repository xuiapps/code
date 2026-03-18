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

    /// <inheritdoc/>
    public override string ToString() => _value.ToString();
}
