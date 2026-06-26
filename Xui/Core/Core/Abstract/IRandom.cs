namespace Xui.Core.Abstract;

/// <summary>
/// Provides random value generation.
/// </summary>
public interface IRandom
{
    /// <summary>
    /// Returns a random integer within the specified range.
    /// </summary>
    /// <param name="minValue">Inclusive lower bound of the random number.</param>
    /// <param name="maxValue">Exclusive upper bound of the random number.</param>
    /// <returns>A random integer in the specified range.</returns>
    int Next(int minValue, int maxValue);

    /// <summary>
    /// Returns a random floating-point number in the range [0.0, 1.0).
    /// </summary>
    /// <returns>A random floating-point number.</returns>
    double NextDouble();
}

/// <summary>
/// Default random implementation backed by <see cref="Random.Shared"/>.
/// </summary>
public sealed class SystemRandom : IRandom
{
    /// <summary>
    /// Gets the shared default system random instance.
    /// </summary>
    public static SystemRandom Default { get; } = new();

    private readonly Random random;

    private SystemRandom()
    {
        random = Random.Shared;
    }

    /// <inheritdoc/>
    public int Next(int minValue, int maxValue) => random.Next(minValue, maxValue);

    /// <inheritdoc/>
    public double NextDouble() => random.NextDouble();
}

/// <summary>
/// Random implementation that uses a fixed seed for deterministic output.
/// </summary>
public sealed class SeededRandom : IRandom
{
    private readonly Random random;

    /// <summary>
    /// Initializes a new seeded random instance.
    /// </summary>
    /// <param name="seed">The deterministic seed value.</param>
    public SeededRandom(int seed)
    {
        random = new Random(seed);
    }

    /// <inheritdoc/>
    public int Next(int minValue, int maxValue) => random.Next(minValue, maxValue);

    /// <inheritdoc/>
    public double NextDouble() => random.NextDouble();
}
