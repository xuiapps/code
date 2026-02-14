using Xui.Core.Canvas;

namespace Xui.Core.UI.Input
{
    /// <summary>
    /// Represents a reference to a pointer event routed to a view, containing identification and pointer state information.
    /// </summary>
    public ref struct PointerEventRef
    {
        /// <summary>
        /// Gets the type of pointer event (e.g., Down, Move, Up, Enter, Leave).
        /// </summary>
        public readonly PointerEventType Type;

        /// <summary>
        /// Gets the unique ID of the pointer. Used to distinguish different active pointers (e.g., different fingers or pen tips).
        /// </summary>
        public readonly int PointerId;

        /// <summary>
        /// Gets the persistent device ID, if known. Identifies the underlying hardware across multiple pointer sessions.
        /// </summary>
        public readonly long PersistentDeviceId;

        /// <summary>
        /// Gets a value indicating whether this pointer is the primary pointer in a multi-pointer interaction (e.g., first touch).
        /// </summary>
        public readonly bool IsPrimary;

        /// <summary>
        /// Gets the current physical state of the pointer (position, pressure, tilt, etc.).
        /// </summary>
        public readonly PointerState State;

        /// <summary>
        /// Gets high-frequency coalesced samples recorded between the last and current events.
        /// </summary>
        public readonly ReadOnlySpan<PointerState> CoalescedStates;

        /// <summary>
        /// Gets future-predicted samples estimated from the current pointer movement.
        /// </summary>
        public readonly ReadOnlySpan<PointerState> PredictedStates;

        /// <summary>
        /// Optional text measure context for hit-testing text positions.
        /// May be null on platforms that do not provide one.
        /// </summary>
        public readonly ITextMeasureContext? TextMeasure;

        /// <summary>
        /// Initializes a new instance of the <see cref="PointerEventRef"/> struct.
        /// </summary>
        public PointerEventRef(
            PointerEventType type,
            int pointerId,
            long persistentDeviceId,
            bool isPrimary,
            PointerState state,
            ReadOnlySpan<PointerState> coalescedStates,
            ReadOnlySpan<PointerState> predictedStates,
            ITextMeasureContext? textMeasure = null)
        {
            this.Type = type;
            PointerId = pointerId;
            PersistentDeviceId = persistentDeviceId;
            IsPrimary = isPrimary;
            State = state;
            CoalescedStates = coalescedStates;
            PredictedStates = predictedStates;
            TextMeasure = textMeasure;
        }
    }
}
