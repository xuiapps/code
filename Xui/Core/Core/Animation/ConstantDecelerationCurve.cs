namespace Xui.Core.Animation;

/// <summary>
/// Represents a 1D motion curve with constant deceleration (e.g., for scroll or fling).
/// Starts at a given position and velocity, then decelerates linearly to a stop.
/// </summary>
/// <remarks>
/// This model is commonly used in UI frameworks to simulate natural-feeling scroll behavior.
/// The acceleration is constant and opposite to the direction of motion, resulting in
/// a predictable, smooth slowdown.
/// </remarks>
public readonly struct ConstantDecelerationCurve
{
    /// <summary>
    /// The time when the motion begins.
    /// </summary>
    public readonly TimeSpan StartTime;

    /// <summary>
    /// The time when the motion stops (velocity reaches zero).
    /// </summary>
    public readonly TimeSpan EndTime;

    /// <summary>
    /// The constant acceleration applied during motion, typically negative.
    /// </summary>
    public readonly nfloat A;

    /// <summary>
    /// The initial velocity at <see cref="StartTime"/>, in pixels per second.
    /// </summary>
    public readonly nfloat V0;

    /// <summary>
    /// The initial position at <see cref="StartTime"/>, in pixels.
    /// </summary>
    public readonly nfloat P0;

    /// <summary>
    /// Constructs a motion curve with constant deceleration.
    /// </summary>
    /// <param name="startTime">The time when motion begins.</param>
    /// <param name="position">The initial position at <paramref name="startTime"/>.</param>
    /// <param name="velocity">The initial velocity in pixels per second.</param>
    /// <param name="accelerationMagnitude">The magnitude of deceleration (must be positive).</param>
    /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="accelerationMagnitude"/> is not positive.</exception>
    public ConstantDecelerationCurve(TimeSpan startTime, nfloat position, nfloat velocity, nfloat accelerationMagnitude)
    {
        if (accelerationMagnitude <= 0)
            throw new ArgumentOutOfRangeException(nameof(accelerationMagnitude), "Acceleration must be positive.");

        StartTime = startTime;
        P0 = position;
        V0 = velocity;
        A = -nfloat.Sign(velocity) * accelerationMagnitude;

        var duration = -velocity / A;
        EndTime = startTime + TimeSpan.FromSeconds(duration);
    }

    /// <summary>
    /// The total duration of the motion in seconds.
    /// </summary>
    public nfloat DurationSeconds => (nfloat)(EndTime - StartTime).TotalSeconds;

    /// <summary>
    /// Evaluates the position at the specified time.
    /// </summary>
    /// <param name="time">The absolute time at which to evaluate the position.</param>
    /// <returns>The position in pixels.</returns>
    public nfloat this[TimeSpan time]
    {
        [DebuggerStepThrough]
        get
        {
            var t = ClampTime(time);
            return P0 + V0 * t + 0.5f * A * t * t;
        }
    }

    /// <summary>
    /// Evaluates the velocity at the specified time.
    /// </summary>
    /// <param name="time">The absolute time at which to evaluate the velocity.</param>
    /// <returns>The velocity in pixels per second.</returns>
    public nfloat VelocityAt(TimeSpan time)
    {
        var t = ClampTime(time);
        return V0 + A * t;
    }

    private nfloat ClampTime(TimeSpan time)
    {
        var t = (nfloat)(time - StartTime).TotalSeconds;
        return nfloat.Clamp(t, 0, DurationSeconds);
    }
}
