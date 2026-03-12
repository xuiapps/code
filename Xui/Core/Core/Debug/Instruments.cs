namespace Xui.Core.Debug;

/// <summary>Factory for creating instrumentation sinks.</summary>
public static partial class Instruments
{
    /// <summary>Returns an instrumentation sink that writes to the console.</summary>
    public static readonly IInstruments Console = new ConsoleLogInstruments();
}
