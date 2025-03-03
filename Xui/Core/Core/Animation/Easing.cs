namespace Xui.Core.Animation;

public static class Easing
{
    public static nfloat Normalize(nfloat value, nfloat min, nfloat max) =>
        max <= min ? 0 : nfloat.Clamp((value - min) / (max - min), 0, 1);

    public static nfloat EaseInOutSine(nfloat t) =>
        -(nfloat.Cos(nfloat.Pi * t) - 1f) / 2f;

    public static nfloat EaseInOutQuad(nfloat t) =>
        t < 0.5 ? 2 * t * t : 1 - nfloat.Pow(-2 * t + 2, 2) / 2;
    
    public static nfloat SmoothDamp(nfloat from, nfloat to, ref nfloat velocity, nfloat smoothTime, nfloat maxSpeed, nfloat deltaTime)
    {
        nfloat omega = 2f / smoothTime;
        nfloat x = omega * deltaTime;
        nfloat exp = 1f / (1f + x + 0.48f * x * x + 0.235f * x * x * x);
        nfloat change = to - from;

        // max speed
        nfloat maxChange = maxSpeed * smoothTime;
        change = nfloat.Clamp(change, -maxChange, maxChange);
        nfloat temp = (velocity + omega * change) * deltaTime;
        velocity = (velocity - omega * temp) * exp;
        nfloat output = from + change - (change + temp) * exp;

        // reached target
        if ((from - to) * (to - output) >= 0)
        {
            output = to;
            velocity = 0f;
        }

        return output;
    }
}
