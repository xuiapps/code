namespace Xui.Core.Abstract.Events;

/// <summary>
/// Represents a platform-level event dispatched once per frame,
/// providing timing information used for driving animations and visual updates.
/// </summary>
/// <remarks>
/// This event is emitted from the <c>Actual</c> window and forwarded
/// to the <c>Abstract</c> window, typically during the animation phase
/// of the UI lifecycle. It provides both the previous and next frame times,
/// along with the time delta between them. Consumers can use this data
/// to advance animations or perform time-based layout updates.
/// </remarks>
public ref struct FrameEventRef
{
    /// <summary>
    /// The timestamp of the previous frame.
    /// </summary>
    public TimeSpan Previous;

    /// <summary>
    /// The timestamp of the upcoming frame.
    /// </summary>
    public TimeSpan Next;

    /// <summary>
    /// The time elapsed between the previous and next frames.
    /// </summary>
    public TimeSpan Delta;

    /// <summary>
    /// Initializes a new instance of the <see cref="FrameEventRef"/> struct
    /// with the given previous and next frame timestamps.
    /// </summary>
    /// <param name="previous">The timestamp of the previous frame.</param>
    /// <param name="next">The timestamp of the upcoming frame.</param>
    public FrameEventRef(TimeSpan previous, TimeSpan next)
    {
        Previous = previous;
        Next = next;
        Delta = next - previous;
    }
}
