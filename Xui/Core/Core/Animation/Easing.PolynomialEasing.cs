namespace Xui.Core.Animation;

public static partial class Easing
{
    /// <summary>
    /// Represents a 3rd- or 4th-degree polynomial easing function for animation timing curves.
    /// </summary>
    /// <remarks>
    /// This type can be used to approximate Bézier-based easing curves (like <see cref="Easing.CubicBezier"/>)
    /// using polynomial coefficients, which are faster to evaluate and easier to store or serialize.
    /// </remarks>
    public readonly struct PolynomialEasing
    {
        /// <summary>The coefficient for x⁴ (A = 0 for cubic curves).</summary>
        public readonly nfloat A;

        /// <summary>The coefficient for x³.</summary>
        public readonly nfloat B;

        /// <summary>The coefficient for x².</summary>
        public readonly nfloat C;

        /// <summary>The coefficient for x¹.</summary>
        public readonly nfloat D;

        /// <summary>The constant term.</summary>
        public readonly nfloat E;

        /// <summary>
        /// Constructs a 3rd-degree polynomial: y = B·x³ + C·x² + D·x + E
        /// </summary>
        public PolynomialEasing(nfloat b, nfloat c, nfloat d, nfloat e)
        {
            A = 0;
            B = b;
            C = c;
            D = d;
            E = e;
        }

        /// <summary>
        /// Constructs a 4th-degree polynomial: y = A·x⁴ + B·x³ + C·x² + D·x + E
        /// </summary>
        public PolynomialEasing(nfloat a, nfloat b, nfloat c, nfloat d, nfloat e)
        {
            A = a;
            B = b;
            C = c;
            D = d;
            E = e;
        }

        /// <summary>
        /// Approximates a <see cref="Easing.CubicBezier"/> easing curve using a cubic polynomial.
        /// The result fits y = B·x³ + C·x² + D·x + E over the domain x ∈ [0, 1].
        /// </summary>
        /// <param name="bezier">The Bézier curve to approximate.</param>
        /// <param name="samples">Number of sample points to use in least-squares fitting. Default is 16.</param>
        public PolynomialEasing(CubicBezier bezier, int samples = 16)
        {
            Span<nfloat> sumX = stackalloc nfloat[7];
            Span<nfloat> sumXY = stackalloc nfloat[4];

            for (int i = 0; i <= samples; i++)
            {
                var t = (nfloat)i / samples;
                var p = bezier.Lerp(t);

                var x = p.X;
                var y = p.Y;

                nfloat xPow = 1;
                for (int pow = 0; pow <= 6; pow++)
                {
                    if (pow <= 3)
                        sumXY[pow] += xPow * y;

                    sumX[pow] += xPow;
                    xPow *= x;
                }
            }

            Span<nfloat> coefficients = stackalloc nfloat[4];
            Span<nfloat> rhs = stackalloc nfloat[4];
            for (int i = 0; i < 4; i++)
                rhs[i] = sumXY[i];

            Span<nfloat> matrix = stackalloc nfloat[16]; // 4x4 matrix
            for (int r = 0; r < 4; r++)
                for (int c = 0; c < 4; c++)
                    matrix[r * 4 + c] = sumX[r + c];

            SolveGaussian(matrix, rhs, coefficients);

            A = 0;
            B = coefficients[0];
            C = coefficients[1];
            D = coefficients[2];
            E = coefficients[3];
        }

        /// <summary>
        /// Evaluates the polynomial at a given value of x ∈ [0, 1].
        /// </summary>
        /// <param name="x">The input value (typically time).</param>
        /// <returns>The eased value at x.</returns>
        public nfloat this[nfloat x]
        {
            [DebuggerStepThrough]
            get => (((A * x + B) * x + C) * x + D) * x + E;
        }

        /// <summary>
        /// Solves a 4x4 linear system using Gaussian elimination.
        /// </summary>
        /// <param name="m">The coefficient matrix (flattened row-major).</param>
        /// <param name="b">The right-hand side vector.</param>
        /// <param name="result">The resulting vector of solved coefficients.</param>
        private static void SolveGaussian(Span<nfloat> m, Span<nfloat> b, Span<nfloat> result)
        {
            const int N = 4;
            for (int i = 0; i < N; i++)
            {
                int pivot = i;
                for (int j = i + 1; j < N; j++)
                {
                    if (nfloat.Abs(m[j * 4 + i]) > nfloat.Abs(m[pivot * 4 + i]))
                        pivot = j;
                }

                if (pivot != i)
                {
                    for (int k = 0; k < N; k++)
                        (m[i * 4 + k], m[pivot * 4 + k]) = (m[pivot * 4 + k], m[i * 4 + k]);

                    (b[i], b[pivot]) = (b[pivot], b[i]);
                }

                var div = m[i * 4 + i];
                for (int k = i + 1; k < N; k++)
                {
                    var f = m[k * 4 + i] / div;
                    for (int j = i; j < N; j++)
                        m[k * 4 + j] -= f * m[i * 4 + j];

                    b[k] -= f * b[i];
                }
            }

            for (int i = N - 1; i >= 0; i--)
            {
                var sum = b[i];
                for (int j = i + 1; j < N; j++)
                    sum -= m[i * 4 + j] * result[j];

                result[i] = sum / m[i * 4 + i];
            }
        }
    }
}
