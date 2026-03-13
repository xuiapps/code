namespace Xui.Core.Debug;

/// <summary>Controls the verbosity threshold for instrumentation output.</summary>
public enum LevelOfDetail : uint
{
    /// <summary>No output. Silences all instrumentation.</summary>
    Quiet = 0,
    /// <summary>Only the most critical events (start, stop, fatal errors).</summary>
    Essential = 100,
    /// <summary>Standard operational events.</summary>
    Normal = 200,
    /// <summary>Additional informational context useful for understanding behavior.</summary>
    Info = 300,
    /// <summary>Fine-grained diagnostic detail for debugging.</summary>
    Diagnostic = 400,
}
