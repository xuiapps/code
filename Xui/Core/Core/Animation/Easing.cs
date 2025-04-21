namespace Xui.Core.Animation;

/// <summary>
/// Provides a collection of easing functions and smoothing utilities used in animations.
/// All easing methods are normalized to take a parameter <c>t</c> in the range [0, 1].
/// </summary>
public static partial class Easing
{
    private static readonly nfloat c1 = 1.70158f;
    private static readonly nfloat c2 = 1.70158f * 1.525f;
    private static readonly nfloat c4 = (nfloat)(2 * Math.PI) / 3;
    private static readonly nfloat c5 = (nfloat)(2 * Math.PI) / (nfloat)4.5;
    private static readonly nfloat n1 = 7.5625f;
    private static readonly nfloat d1 = 2.75f;

    /// <summary>
    /// Normalizes a value between <paramref name="min"/> and <paramref name="max"/> into a [0,1] range.
    /// Returns 0 if <paramref name="max"/> ≤ <paramref name="min"/>.
    /// </summary>
    public static nfloat Normalize(nfloat value, nfloat min, nfloat max) =>
        max <= min ? 0 : nfloat.Clamp((value - min) / (max - min), 0, 1);

    /// <summary>
    /// Eases in and out with a sine curve. Smoothest at beginning and end.
    /// </summary>
    public static nfloat EaseInOutSine(nfloat t) =>
        -(nfloat.Cos(nfloat.Pi * t) - 1f) / 2f;

    /// <summary>
    /// Accelerates slowly from 0 to 1 with a quadratic curve.
    /// </summary>
    public static nfloat EaseInQuad(nfloat t) => t * t;

    /// <summary>
    /// Decelerates smoothly from 1 to 0 with a quadratic curve.
    /// </summary>
    public static nfloat EaseOutQuad(nfloat t) => 1 - (1 - t) * (1 - t);

    /// <summary>
    /// Accelerates and decelerates using a quadratic curve.
    /// </summary>
    public static nfloat EaseInOutQuad(nfloat t) =>
        t < 0.5 ? 2 * t * t : 1 - nfloat.Pow(-2 * t + 2, 2) / 2;

    /// <summary>
    /// Accelerates slowly with a cubic curve.
    /// </summary>
    public static nfloat EaseInCubic(nfloat t) => t * t * t;

    /// <summary>
    /// Decelerates slowly with a cubic curve.
    /// </summary>
    public static nfloat EaseOutCubic(nfloat t) => 1 - nfloat.Pow(1 - t, 3);

    /// <summary>
    /// Smoothly accelerates and decelerates with a cubic curve.
    /// </summary>
    public static nfloat EaseInOutCubic(nfloat t) =>
        t < 0.5 ? 4 * t * t * t : 1 - nfloat.Pow(-2 * t + 2, 3) / 2;

    /// <summary>
    /// Stronger acceleration from 0 using a quartic curve.
    /// </summary>
    public static nfloat EaseInQuart(nfloat t) => t * t * t * t;

    /// <summary>
    /// Stronger deceleration to 1 using a quartic curve.
    /// </summary>
    public static nfloat EaseOutQuart(nfloat t) => 1 - nfloat.Pow(1 - t, 4);

    /// <summary>
    /// Strong acceleration and deceleration using a quartic curve.
    /// </summary>
    public static nfloat EaseInOutQuart(nfloat t) =>
        t < 0.5 ? 8 * t * t * t * t : 1 - nfloat.Pow(-2 * t + 2, 4) / 2;

    /// <summary>
    /// Creates an elastic "bounce-in" effect at the beginning of the transition.
    /// </summary>
    public static nfloat EaseInElastic(nfloat t) =>
        t == 0 ? 0 : t == 1 ? 1 : -nfloat.Pow(2, 10 * t - 10) * nfloat.Sin((t * 10 - 10.75f) * c4);

    /// <summary>
    /// Creates an elastic "bounce-out" effect at the end of the transition.
    /// </summary>
    public static nfloat EaseOutElastic(nfloat t) =>
        t == 0 ? 0 : t == 1 ? 1 : nfloat.Pow(2, -10 * t) * nfloat.Sin((t * 10 - 0.75f) * c4) + 1;

    /// <summary>
    /// Combines EaseInElastic and EaseOutElastic for a spring-like bounce effect at both ends.
    /// </summary>
    public static nfloat EaseInOutElastic(nfloat t)
    {
        if (t == 0) return 0;
        if (t == 1) return 1;
        return t < 0.5
            ? -(nfloat.Pow(2, 20 * t - 10) * nfloat.Sin((20 * t - 11.125f) * c5)) / 2
            : (nfloat.Pow(2, -20 * t + 10) * nfloat.Sin((20 * t - 11.125f) * c5)) / 2 + 1;
    }

    /// <summary>
    /// Eases in with a "backwards" overshoot effect before settling forward.
    /// </summary>
    public static nfloat EaseInBack(nfloat t) =>
        (c1 + 1) * t * t * t - c1 * t * t;

    /// <summary>
    /// Eases out with a slight overshoot after reaching the target.
    /// </summary>
    public static nfloat EaseOutBack(nfloat t) =>
        1 + (c1 + 1) * nfloat.Pow(t - 1, 3) + c1 * nfloat.Pow(t - 1, 2);

    /// <summary>
    /// Eases in and out with a "backwards" overshoot at both ends.
    /// </summary>
    public static nfloat EaseInOutBack(nfloat t) =>
        t < 0.5
            ? nfloat.Pow(2 * t, 2) * ((c2 + 1) * 2 * t - c2) / 2
            : (nfloat.Pow(2 * t - 2, 2) * ((c2 + 1) * (2 * t - 2) + c2) + 2) / 2;

    /// <summary>
    /// Creates a bounce effect as if hitting the ground and bouncing back.
    /// </summary>
    public static nfloat EaseOutBounce(nfloat t)
    {
        if (t < 1 / d1)
            return n1 * t * t;
        else if (t < 2 / d1)
        {
            t -= 1.5f / d1;
            return n1 * t * t + 0.75f;
        }
        else if (t < 2.5f / d1)
        {
            t -= 2.25f / d1;
            return n1 * t * t + 0.9375f;
        }
        else
        {
            t -= 2.625f / d1;
            return n1 * t * t + 0.984375f;
        }
    }

    /// <summary>
    /// Reverses EaseOutBounce to ease in with bounce.
    /// </summary>
    public static nfloat EaseInBounce(nfloat t) =>
        1 - EaseOutBounce(1 - t);

    /// <summary>
    /// Eases in and out with a bounce at both ends.
    /// </summary>
    public static nfloat EaseInOutBounce(nfloat t) =>
        t < 0.5
            ? (1 - EaseOutBounce(1 - 2 * t)) / 2
            : (1 + EaseOutBounce(2 * t - 1)) / 2;

    /// <summary>
    /// Applies a smoothstep-like easing function that produces smooth transitions with zero first and second derivatives at boundaries.
    /// </summary>
    public static nfloat SmootherStep(nfloat t) =>
        t * t * t * (t * (t * 6 - 15) + 10);

    /// <summary>
    /// Smoothly interpolates between two values using a critically damped spring-like function.
    /// </summary>
    /// <param name="from">The starting value.</param>
    /// <param name="to">The target value.</param>
    /// <param name="velocity">Reference to current velocity (will be modified).</param>
    /// <param name="smoothTime">Time it takes to reach the target value.</param>
    /// <param name="maxSpeed">Maximum speed during interpolation.</param>
    /// <param name="deltaTime">Elapsed time since the last update.</param>
    public static nfloat SmoothDamp(nfloat from, nfloat to, ref nfloat velocity, nfloat smoothTime, nfloat maxSpeed, nfloat deltaTime)
    {
        nfloat omega = 2f / smoothTime;
        nfloat x = omega * deltaTime;
        nfloat exp = 1f / (1f + x + 0.48f * x * x + 0.235f * x * x * x);
        nfloat change = to - from;

        nfloat maxChange = maxSpeed * smoothTime;
        change = nfloat.Clamp(change, -maxChange, maxChange);

        nfloat temp = (velocity + omega * change) * deltaTime;
        velocity = (velocity - omega * temp) * exp;
        nfloat output = from + change - (change + temp) * exp;

        if ((from - to) * (to - output) >= 0)
        {
            output = to;
            velocity = 0f;
        }

        return output;
    }
}
