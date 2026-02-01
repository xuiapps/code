using System.Runtime.CompilerServices;

namespace Xui.Core.Debug;

/// <summary>
/// A zero-allocation interpolated string handler for instrumentation events.
///
/// This handler is a shallow pass-through that delegates all formatting operations
/// to the underlying <see cref="IRunLoopInstruments"/> implementation. This allows
/// each platform (console output, mobile compressed streaming, etc.) to handle
/// message formatting in its own optimal way.
/// </summary>
[InterpolatedStringHandler]
public ref struct InstrumentsInterpolatedStringHandler
{
    private IMessageBuilder? builder;

    /// <summary>
    /// Initializes the interpolated string handler.
    /// </summary>
    /// <param name="literalLength">The total length of all literal parts (provided by compiler).</param>
    /// <param name="formattedCount">The number of formatted arguments (provided by compiler).</param>
    /// <param name="builder">The instrumentation instance to delegate formatting operations to.</param>
    /// <param name="levelOfDetail">The level of detail for this event.</param>
    /// <param name="aspect">The aspect category for this event.</param>
    public InstrumentsInterpolatedStringHandler(
        int literalLength,
        int formattedCount,
        IMessageBuilder? builder,
        LevelOfDetail levelOfDetail,
        Aspect aspect)
    {
        this.builder = builder;
    }

    /// <summary>
    /// Appends a literal string by delegating to the instruments implementation.
    /// </summary>
    /// <param name="value">The literal string to append.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void AppendLiteral(string value) =>
        builder?.AppendLiteral(value);

    /// <summary>
    /// Appends a formatted value by delegating to the instruments implementation.
    /// </summary>
    /// <typeparam name="T">The type of the value to format.</typeparam>
    /// <param name="value">The value to format and append.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void AppendFormatted<T>(T value) =>
        builder?.AppendFormatted(value);

    /// <summary>
    /// Appends a formatted value with a format string by delegating to the instruments implementation.
    /// </summary>
    /// <typeparam name="T">The type of the value to format.</typeparam>
    /// <param name="value">The value to format and append.</param>
    /// <param name="format">The format string to apply.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void AppendFormatted<T>(T value, string? format) =>
        builder?.AppendFormatted(value, format);

    /// <summary>
    /// Disposes the handler and signals message completion to the instruments implementation.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Dispose() =>
        builder?.EndMessage();
}
