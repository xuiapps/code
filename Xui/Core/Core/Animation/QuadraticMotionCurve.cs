namespace Xui.Core.Animation;

/// <summary>
/// Represents a 1D motion curve under constant acceleration, defined as a quadratic function of time.
/// 
/// This curve is useful for modeling motion that decelerates smoothly to a stop,
/// such as fling or scroll-stop animations.
/// </summary>
public readonly struct QuadraticMotionCurve
{
    /// <summary>
    /// The time at which the motion starts.
    /// </summary>
    public readonly TimeSpan StartTime;

    /// <summary>
    /// The time at which the motion ends (i.e., when velocity reaches zero).
    /// </summary>
    public readonly TimeSpan EndTime;

    /// <summary>
    /// The quadratic coefficient (0.5 * acceleration).
    /// </summary>
    public readonly nfloat A;

    /// <summary>
    /// The linear coefficient (initial velocity minus adjustment).
    /// </summary>
    public readonly nfloat B;

    /// <summary>
    /// The constant coefficient (initial position minus offset).
    /// </summary>
    public readonly nfloat C;

    /// <summary>
    /// Initializes the curve with explicit polynomial coefficients and time range.
    /// </summary>
    /// <param name="startTime">The time when motion starts.</param>
    /// <param name="endTime">The time when motion ends.</param>
    /// <param name="a">The quadratic coefficient.</param>
    /// <param name="b">The linear coefficient.</param>
    /// <param name="c">The constant offset.</param>
    [DebuggerStepThrough]
    public QuadraticMotionCurve(TimeSpan startTime, TimeSpan endTime, nfloat a, nfloat b, nfloat c)
    {
        StartTime = startTime;
        EndTime = endTime;
        A = a;
        B = b;
        C = c;
    }

    /// <summary>
    /// Constructs a curve from initial position, velocity, and acceleration magnitude.
    /// The acceleration is applied in the opposite direction of velocity to decelerate to zero.
    /// </summary>
    /// <param name="startTime">The time when motion starts.</param>
    /// <param name="startPosition">Initial position.</param>
    /// <param name="startVelocity">Initial velocity (positive or negative).</param>
    /// <param name="accelerationMagnitude">Positive scalar magnitude of deceleration.</param>
    public QuadraticMotionCurve(
        TimeSpan startTime,
        nfloat startPosition,
        nfloat startVelocity,
        nfloat accelerationMagnitude)
    {
        var direction = Math.Sign(startVelocity); // +1 or -1
        var acceleration = -direction * accelerationMagnitude;

        StartTime = startTime;

        // Compute coefficients based on shifted time
        var t0 = (nfloat)startTime.TotalSeconds;

        A = 0.5f * acceleration;
        B = startVelocity - 2 * A * t0;
        C = startPosition - (A * t0 + B) * t0;

        // End time is when velocity reaches 0
        var duration = Math.Abs((double)(-startVelocity / acceleration));
        EndTime = startTime + TimeSpan.FromSeconds(duration);
    }

    /// <summary>
    /// Gets the final position of the motion at <see cref="EndTime"/>.
    /// </summary>
    public nfloat FinalPosition => this[EndTime];

    /// <summary>
    /// Evaluates the position of the motion at the specified <paramref name="time"/>.
    /// </summary>
    /// <param name="time">The time at which to evaluate the position.</param>
    public nfloat this[TimeSpan time]
    {
        [DebuggerStepThrough]
        get
        {
            var t = (nfloat)time.TotalSeconds;
            return (A * t + B) * t + C;
        }
    }

    /// <summary>
    /// Evaluates the instantaneous velocity at the specified <paramref name="time"/>.
    /// </summary>
    /// <param name="time">The time at which to evaluate the velocity.</param>
    /// <returns>The velocity in units per second.</returns>
    public nfloat VelocityAt(TimeSpan time)
    {
        var t = (nfloat)time.TotalSeconds;
        return 2 * A * t + B;
    }
}
