namespace Xui.GPU.Shaders.Types;

/// <summary>
/// Represents a 2D texture resource in shader code.
/// </summary>
/// <typeparam name="TPixel">The pixel format type (typically Color4).</typeparam>
/// <remarks>
/// In CPU execution, this wraps actual texture data.
/// In GPU execution, this will be translated to the appropriate backend texture type.
/// </remarks>
public readonly struct Texture2D<TPixel>
    where TPixel : unmanaged
{
    private readonly object? _data;

    /// <summary>
    /// Initializes a new instance of the <see cref="Texture2D{TPixel}"/> struct.
    /// </summary>
    /// <param name="data">The underlying texture data (implementation-specific).</param>
    public Texture2D(object? data)
    {
        _data = data;
    }

    /// <summary>
    /// Gets the underlying texture data.
    /// </summary>
    internal object? Data => _data;
}

/// <summary>
/// Represents texture sampling addressing modes.
/// </summary>
public enum AddressMode
{
    /// <summary>
    /// Texture coordinates wrap at the boundaries (repeat).
    /// </summary>
    Repeat,

    /// <summary>
    /// Texture coordinates are clamped to the edge.
    /// </summary>
    ClampToEdge,

    /// <summary>
    /// Texture coordinates mirror at the boundaries.
    /// </summary>
    MirroredRepeat,

    /// <summary>
    /// Texture coordinates outside [0, 1] return a border color.
    /// </summary>
    ClampToBorder,
}

/// <summary>
/// Represents texture sampling filter modes.
/// </summary>
public enum FilterMode
{
    /// <summary>
    /// Point (nearest neighbor) sampling.
    /// </summary>
    Point,

    /// <summary>
    /// Linear (bilinear) sampling.
    /// </summary>
    Linear,
}

/// <summary>
/// Represents a texture sampler that controls how textures are sampled.
/// </summary>
/// <remarks>
/// Samplers define the filtering and addressing behavior when reading from textures.
/// </remarks>
public readonly struct Sampler
{
    /// <summary>
    /// Gets the minification filter mode.
    /// </summary>
    public FilterMode MinFilter { get; init; }

    /// <summary>
    /// Gets the magnification filter mode.
    /// </summary>
    public FilterMode MagFilter { get; init; }

    /// <summary>
    /// Gets the addressing mode for the U coordinate.
    /// </summary>
    public AddressMode AddressU { get; init; }

    /// <summary>
    /// Gets the addressing mode for the V coordinate.
    /// </summary>
    public AddressMode AddressV { get; init; }

    /// <summary>
    /// Initializes a new instance of the <see cref="Sampler"/> struct.
    /// </summary>
    public Sampler(
        FilterMode minFilter = FilterMode.Linear,
        FilterMode magFilter = FilterMode.Linear,
        AddressMode addressU = AddressMode.Repeat,
        AddressMode addressV = AddressMode.Repeat)
    {
        MinFilter = minFilter;
        MagFilter = magFilter;
        AddressU = addressU;
        AddressV = addressV;
    }

    /// <summary>
    /// Gets a default linear sampler with repeat addressing.
    /// </summary>
    public static Sampler LinearRepeat => new(
        FilterMode.Linear,
        FilterMode.Linear,
        AddressMode.Repeat,
        AddressMode.Repeat
    );

    /// <summary>
    /// Gets a default linear sampler with clamp-to-edge addressing.
    /// </summary>
    public static Sampler LinearClamp => new(
        FilterMode.Linear,
        FilterMode.Linear,
        AddressMode.ClampToEdge,
        AddressMode.ClampToEdge
    );

    /// <summary>
    /// Gets a default point sampler with repeat addressing.
    /// </summary>
    public static Sampler PointRepeat => new(
        FilterMode.Point,
        FilterMode.Point,
        AddressMode.Repeat,
        AddressMode.Repeat
    );

    /// <summary>
    /// Gets a default point sampler with clamp-to-edge addressing.
    /// </summary>
    public static Sampler PointClamp => new(
        FilterMode.Point,
        FilterMode.Point,
        AddressMode.ClampToEdge,
        AddressMode.ClampToEdge
    );
}
