namespace Xui.Core.Abstract;

/// <summary>
/// Provides access to the current clock time.
/// </summary>
public interface IClock
{
    /// <summary>
    /// Gets the current local time.
    /// </summary>
    DateTime Now { get; }
}

/// <summary>
/// Default clock implementation backed by <see cref="DateTime.Now"/>.
/// </summary>
public sealed class SystemClock : IClock
{
    /// <summary>
    /// Gets the shared default system clock instance.
    /// </summary>
    public static SystemClock Default { get; } = new();

    private SystemClock()
    {
    }

    /// <inheritdoc/>
    public DateTime Now => DateTime.Now;
}

/// <summary>
/// Mutable clock implementation used for deterministic scenarios such as tests.
/// </summary>
public sealed class FixedClock : IClock
{
    /// <summary>
    /// Initializes a new fixed clock with an initial time value.
    /// </summary>
    /// <param name="now">The initial local time.</param>
    public FixedClock(DateTime now)
    {
        Now = now;
    }

    /// <summary>
    /// Gets the current fixed local time.
    /// </summary>
    public DateTime Now { get; private set; }

    /// <summary>
    /// Sets the current fixed time.
    /// </summary>
    /// <param name="now">The new local time.</param>
    public void Set(DateTime now)
    {
        Now = now;
    }

    /// <summary>
    /// Advances the current fixed time by a delta.
    /// </summary>
    /// <param name="delta">The time delta to add.</param>
    public void Advance(TimeSpan delta)
    {
        Now += delta;
    }
}
