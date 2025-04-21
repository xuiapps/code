using System;
using System.Diagnostics;

namespace Xui.Core.Animation;

/// <summary>
/// Represents a motion curve where velocity decays exponentially over time.
/// Commonly used to simulate fling or momentum-based motion with smooth slowdown.
/// </summary>
public readonly struct ExponentialDecayCurve
{
    /// <summary>
    /// The time at which the motion begins.
    /// </summary>
    public readonly TimeSpan StartTime;

    /// <summary>
    /// The time at which the motion ends, determined by when velocity falls below a defined threshold.
    /// </summary>
    public readonly TimeSpan EndTime;

    /// <summary>
    /// The decay factor per second. Lower values result in faster deceleration.
    /// </summary>
    public readonly nfloat DecayPerSecond;

    /// <summary>
    /// The initial velocity at the start of the curve.
    /// </summary>
    public readonly nfloat InitialVelocity;

    /// <summary>
    /// The starting position of the motion.
    /// </summary>
    public readonly nfloat StartPosition;

    /// <summary>
    /// A typical decay factor (~0.998 per millisecond). Use for normal fling decay.
    /// </summary>
    public static readonly nfloat Normal = (nfloat)0.135;

    /// <summary>
    /// A faster decay factor (~0.99 per millisecond). Use for snappier motion.
    /// </summary>
    public static readonly nfloat Fast = (nfloat)0.00004317;

    /// <summary>
    /// Initializes an exponential decay motion curve.
    /// </summary>
    /// <param name="startTime">The time when motion begins.</param>
    /// <param name="startPosition">The initial position.</param>
    /// <param name="initialVelocity">The initial velocity (can be negative).</param>
    /// <param name="decayPerSecond">
    /// The decay multiplier per second. A value less than 1, closer to 0 = faster decay.
    /// Recommended value is <see cref="Normal"/>.
    /// </param>
    /// <param name="velocityThreshold">
    /// The velocity below which motion is considered stopped (defaults to 0.5).
    /// </param>
    [DebuggerStepThrough]
    public ExponentialDecayCurve(
        TimeSpan startTime,
        nfloat startPosition,
        nfloat initialVelocity,
        nfloat decayPerSecond,
        double velocityThreshold = 0.5)
    {
        StartTime = startTime;
        StartPosition = startPosition;
        InitialVelocity = initialVelocity;
        DecayPerSecond = decayPerSecond;

        // Compute EndTime when velocity drops below threshold
        var t = Math.Log(velocityThreshold / Math.Abs(initialVelocity)) / Math.Log(decayPerSecond);
        var seconds = Math.Max(t, 0);
        EndTime = startTime + TimeSpan.FromSeconds(seconds);
    }

    /// <summary>
    /// Gets the final position of the motion at <see cref="EndTime"/>.
    /// </summary>
    public nfloat FinalPosition => this[EndTime];

    /// <summary>
    /// Evaluates the position of the motion at a given time.
    /// </summary>
    /// <param name="time">The time to evaluate.</param>
    /// <returns>The interpolated position at the specified time.</returns>
    public nfloat this[TimeSpan time]
    {
        [DebuggerStepThrough]
        get
        {
            var t = (nfloat)(time - StartTime).TotalSeconds;
            if (t <= 0) return StartPosition;
            if (time >= EndTime) return FinalPosition;

            var decay = (nfloat)Math.Pow(DecayPerSecond, t);
            return StartPosition + InitialVelocity * (1 - decay) / -nfloat.Log(DecayPerSecond);
        }
    }

    /// <summary>
    /// Evaluates the velocity of the motion at a given time.
    /// </summary>
    /// <param name="time">The time to evaluate.</param>
    /// <returns>The current velocity at the specified time.</returns>
    public nfloat VelocityAt(TimeSpan time)
    {
        var t = (nfloat)(time - StartTime).TotalSeconds;
        if (t <= 0) return InitialVelocity;
        if (time >= EndTime) return 0;

        return InitialVelocity * (nfloat)Math.Pow(DecayPerSecond, t);
    }
}
