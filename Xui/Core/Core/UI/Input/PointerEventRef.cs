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
        /// Initializes a new instance of the <see cref="PointerEventRef"/> struct.
        /// </summary>
        /// <param name="type">The type of pointer event (Down, Move, Up, etc.).</param>
        /// <param name="pointerId">The unique ID of the pointer.</param>
        /// <param name="persistentDeviceId">The persistent ID of the physical device, if available.</param>
        /// <param name="isPrimary">Indicates whether this pointer is the primary pointer.</param>
        /// <param name="state">The current physical state of the pointer.</param>
        /// <param name="coalescedStates">High-frequency samples coalesced since the last event.</param>
        /// <param name="predictedStates">Future-predicted samples for smoothing or latency compensation.</param>
        public PointerEventRef(
            PointerEventType type,
            int pointerId,
            long persistentDeviceId,
            bool isPrimary,
            PointerState state,
            ReadOnlySpan<PointerState> coalescedStates,
            ReadOnlySpan<PointerState> predictedStates)
        {
            this.Type = type;
            PointerId = pointerId;
            PersistentDeviceId = persistentDeviceId;
            IsPrimary = isPrimary;
            State = state;
            CoalescedStates = coalescedStates;
            PredictedStates = predictedStates;
        }
    }
}
