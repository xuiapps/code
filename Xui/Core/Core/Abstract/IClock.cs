namespace Xui.Core.Abstract;

public interface IClock
{
    DateTime Now { get; }
}

public sealed class SystemClock : IClock
{
    public static SystemClock Default { get; } = new();

    private SystemClock()
    {
    }

    public DateTime Now => DateTime.Now;
}

public sealed class FixedClock : IClock
{
    public FixedClock(DateTime now)
    {
        Now = now;
    }

    public DateTime Now { get; private set; }

    public void Set(DateTime now)
    {
        Now = now;
    }

    public void Advance(TimeSpan delta)
    {
        Now += delta;
    }
}
