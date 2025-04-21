namespace Xui.Core.Animation;

/// <summary>
/// Represents a 1D cubic motion curve over time, interpolating both position and velocity.
/// Time is expressed using <see cref="TimeSpan"/>, and calculations are performed in seconds.
/// </summary>
/// <remarks>
/// This curve is defined by a cubic polynomial:
/// <c>f(t) = A·t³ + B·t² + C·t + D</c>, where <c>t</c> is time in seconds since the global epoch.
/// It supports generation from boundary conditions (position and velocity at both ends),
/// as well as curve continuation with seamless velocity transitions.
/// </remarks>
public readonly struct CubicMotionCurve
{
    /// <summary>
    /// The time at which the motion curve starts.
    /// </summary>
    public readonly TimeSpan StartTime;

    /// <summary>
    /// The time at which the motion curve ends.
    /// </summary>
    public readonly TimeSpan EndTime;

    /// <summary>
    /// The cubic coefficient (multiplied by t³).
    /// </summary>
    public readonly nfloat A;

    /// <summary>
    /// The quadratic coefficient (multiplied by t²).
    /// </summary>
    public readonly nfloat B;

    /// <summary>
    /// The linear coefficient (multiplied by t).
    /// </summary>
    public readonly nfloat C;

    /// <summary>
    /// The constant offset term.
    /// </summary>
    public readonly nfloat D;

    /// <summary>
    /// Initializes a new cubic motion curve with the given polynomial coefficients and time range.
    /// </summary>
    public CubicMotionCurve(TimeSpan startTime, TimeSpan endTime, nfloat a, nfloat b, nfloat c, nfloat d)
    {
        StartTime = startTime;
        EndTime = endTime;
        A = a;
        B = b;
        C = c;
        D = d;
    }

    /// <summary>
    /// The final position at <see cref="EndTime"/>.
    /// </summary>
    public nfloat FinalPosition => this[EndTime];

    /// <summary>
    /// The duration of the motion curve, in seconds.
    /// </summary>
    public nfloat DurationSeconds => (nfloat)(EndTime - StartTime).TotalSeconds;

    /// <summary>
    /// Continues the current curve by constructing a new cubic motion curve that
    /// begins where this one leaves off, matching position and velocity, and
    /// interpolating to a new position and velocity over the given time range.
    /// </summary>
    public CubicMotionCurve ContinueWithBoundaryConditions(
        TimeSpan startTime, TimeSpan endTime,
        nfloat endPosition, nfloat endVelocity)
    {
        var startPosition = this[startTime];
        var startVelocity = VelocityAt(startTime);
        return FromBoundaryConditions(startTime, startPosition, startVelocity, endTime, endPosition, endVelocity);
    }

    /// <summary>
    /// Constructs a cubic motion curve that interpolates position and velocity
    /// between two points in time using Hermite interpolation.
    /// </summary>
    /// <param name="startTime">The curve's start time.</param>
    /// <param name="startPosition">The position at <paramref name="startTime"/>.</param>
    /// <param name="startVelocity">The velocity at <paramref name="startTime"/>.</param>
    /// <param name="endTime">The curve's end time.</param>
    /// <param name="endPosition">The position at <paramref name="endTime"/>.</param>
    /// <param name="endVelocity">The velocity at <paramref name="endTime"/>.</param>
    /// <returns>A cubic motion curve satisfying the specified boundary conditions.</returns>
    public static CubicMotionCurve FromBoundaryConditions(
        TimeSpan startTime, nfloat startPosition, nfloat startVelocity,
        TimeSpan endTime, nfloat endPosition, nfloat endVelocity)
    {
        var t0 = (nfloat)startTime.TotalSeconds;
        var t1 = (nfloat)endTime.TotalSeconds;
        var dt = t1 - t0;

        var dt2 = dt * dt;
        var dt3 = dt2 * dt;

        var A = 2 * (startPosition - endPosition) / dt3 + (startVelocity + endVelocity) / dt2;
        var B = 3 * (endPosition - startPosition) / dt2 - (2 * startVelocity + endVelocity) / dt;
        var C = startVelocity;
        var D = startPosition;

        return new CubicMotionCurve(startTime, endTime, A, B, C, D);
    }

    /// <summary>
    /// Evaluates the position at the specified time.
    /// </summary>
    /// <param name="time">An absolute time value.</param>
    /// <returns>The position at the given time.</returns>
    public nfloat this[TimeSpan time]
    {
        [DebuggerStepThrough]
        get
        {
            var t = (nfloat)time.TotalSeconds;
            return ((A * t + B) * t + C) * t + D;
        }
    }

    /// <summary>
    /// Evaluates the velocity at the specified time.
    /// </summary>
    /// <param name="time">An absolute time value.</param>
    /// <returns>The velocity at the given time.</returns>
    public nfloat VelocityAt(TimeSpan time)
    {
        var t = (nfloat)time.TotalSeconds;
        return (3 * A * t + 2 * B) * t + C;
    }
}
