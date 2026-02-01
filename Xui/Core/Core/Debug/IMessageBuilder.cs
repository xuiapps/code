namespace Xui.Core.Debug;

/// <summary>
/// Internal interface for building instrumentation messages.
///
/// This interface is implemented explicitly by <see cref="IRunLoopInstruments"/>
/// implementations to prevent these methods from being called manually.
/// Only <see cref="InstrumentsInterpolatedStringHandler"/> should invoke these methods.
/// </summary>
public interface IMessageBuilder
{
    /// <summary>
    /// Appends a literal string to the current message being built.
    /// Called by <see cref="InstrumentsInterpolatedStringHandler"/> during message construction.
    /// </summary>
    /// <param name="value">The literal string to append.</param>
    void AppendLiteral(string value);

    /// <summary>
    /// Appends a formatted value to the current message being built.
    /// Called by <see cref="InstrumentsInterpolatedStringHandler"/> during message construction.
    /// </summary>
    /// <typeparam name="T">The type of the value to format.</typeparam>
    /// <param name="value">The value to format and append.</param>
    void AppendFormatted<T>(T value);

    /// <summary>
    /// Appends a formatted value with a format string to the current message being built.
    /// Called by <see cref="InstrumentsInterpolatedStringHandler"/> during message construction.
    /// </summary>
    /// <typeparam name="T">The type of the value to format.</typeparam>
    /// <param name="value">The value to format and append.</param>
    /// <param name="format">The format string to apply.</param>
    void AppendFormatted<T>(T value, string? format);

    /// <summary>
    /// Signals that the current message construction is complete.
    /// Called by <see cref="InstrumentsInterpolatedStringHandler.Dispose"/> after all append operations.
    /// For console implementations, this is typically when a newline is written.
    /// </summary>
    void EndMessage();
}
