namespace Xui.Core.Abstract;

public interface IRandom
{
    int Next(int minValue, int maxValue);
    double NextDouble();
}

public sealed class SystemRandom : IRandom
{
    public static SystemRandom Default { get; } = new();

    private readonly Random random;

    private SystemRandom()
    {
        random = Random.Shared;
    }

    public int Next(int minValue, int maxValue) => random.Next(minValue, maxValue);
    public double NextDouble() => random.NextDouble();
}

public sealed class SeededRandom : IRandom
{
    private readonly Random random;

    public SeededRandom(int seed)
    {
        random = new Random(seed);
    }

    public int Next(int minValue, int maxValue) => random.Next(minValue, maxValue);
    public double NextDouble() => random.NextDouble();
}
