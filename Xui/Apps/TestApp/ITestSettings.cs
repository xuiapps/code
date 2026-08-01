namespace Xui.Apps.TestApp;

/// <summary>Optional deterministic settings used when TestApp is hosted by its integration tests.</summary>
public interface ITestSettings
{
    /// <summary>Gets the seed for deterministic random behavior, or <see langword="null"/> to use system randomness.</summary>
    int? RandomSeed { get; }

    /// <summary>Gets whether interactive diagnostic overlays should be rendered.</summary>
    bool IsDebugOverlayEnabled { get; }
}

/// <summary>Default deterministic settings for TestApp integration tests.</summary>
public sealed class TestSettings : ITestSettings
{
    /// <summary>Initializes deterministic TestApp settings.</summary>
    public TestSettings(int? randomSeed = 41, bool isDebugOverlayEnabled = false)
    {
        this.RandomSeed = randomSeed;
        this.IsDebugOverlayEnabled = isDebugOverlayEnabled;
    }

    /// <inheritdoc />
    public int? RandomSeed { get; }

    /// <inheritdoc />
    public bool IsDebugOverlayEnabled { get; }
}
