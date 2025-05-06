namespace Xui.Core.Math2D
{
    /// <summary>
    /// Common mathematical constants used throughout 2D geometry and curve computations.
    /// </summary>
    public static class Constants
    {
        /// <summary>π (pi): Ratio of a circle’s circumference to its diameter.</summary>
        public static readonly nfloat π = nfloat.Pi;

        /// <summary>τ (tau): One full circle in radians (2π).</summary>
        public static readonly nfloat τ = nfloat.Pi * 2;

        /// <summary>ε (epsilon): A small value used for numerical stability checks.</summary>
        public static readonly nfloat ε = 1e-5f;

        /// <summary>√2 (square root of 2): Diagonal of a unit square.</summary>
        public static readonly nfloat sqrt2 = (nfloat)Math.Sqrt(2);

        /// <summary>ϕ (phi): The golden ratio (≈ 1.618).</summary>
        public static readonly nfloat ϕ = (1 + nfloat.Sqrt(5)) / 2;
    }
}
