namespace Xui.Core.Math2D;

/// <summary>
/// Represents a 2D vector with X and Y components, commonly used for geometric operations,
/// layout math, and movement in 2D space.
/// </summary>
public struct Vector
{
    /// <summary>The horizontal component of the vector.</summary>
    public nfloat X;

    /// <summary>The vertical component of the vector.</summary>
    public nfloat Y;

    /// <summary>
    /// Initializes a new instance of the <see cref="Vector"/> struct
    /// with the specified X and Y components.
    /// </summary>
    /// <param name="x">The horizontal component of the vector.</param>
    /// <param name="y">The vertical component of the vector.</param>
    [DebuggerStepThrough]
    public Vector(nfloat x, nfloat y)
    {
        this.X = x;
        this.Y = y;
    }

    /// <summary>A zero vector (0, 0).</summary>
    public static readonly Vector Zero = (0, 0);

    /// <summary>A unit vector (1, 1).</summary>
    public static readonly Vector One = (1, 1);

    /// <summary>A unit vector pointing left (-1, 0).</summary>
    public static readonly Vector Left = (-1, 0);

    /// <summary>A unit vector pointing up (0, -1).</summary>
    public static readonly Vector Up = (0, -1);

    /// <summary>A unit vector pointing right (1, 0).</summary>
    public static readonly Vector Right = (1, 0);

    /// <summary>A unit vector pointing down (0, 1).</summary>
    public static readonly Vector Down = (0, 1);

    /// <summary>
    /// Returns a normalized (unit length) version of this vector.
    /// </summary>
    public Vector Normalized
    {
        get
        {
            if (this.X == 0 && this.Y == 0)
                return Zero;

            var len = this.Magnitude;
            return new(this.X / len, this.Y / len);
        }
    }

    /// <summary>
    /// Returns the vector rotated 90° counter-clockwise (CCW).
    /// </summary>
    public Vector PerpendicularCCW => new(-Y, X);

    /// <summary>
    /// Returns the vector rotated 90° clockwise (CW).
    /// </summary>
    public Vector PerpendicularCW => new(Y, -X);

    /// <summary>
    /// Returns the magnitude (length) of the vector.
    /// </summary>
    public nfloat Magnitude => nfloat.Sqrt(this.X * this.X + this.Y * this.Y);

    /// <summary>
    /// Returns the squared magnitude (length squared) of the vector.
    /// This avoids the square root computation used in <see cref="Magnitude"/>.
    /// </summary>
    public nfloat MagnitudeSquared => this.X * this.X + this.Y * this.Y;

    /// <summary>
    /// Gets the angle in radians between the vector and the positive X-axis,
    /// measured counter-clockwise in the range [-π, π].
    /// </summary>
    /// <remarks>
    /// This is equivalent to calling <c>Atan2(Y, X)</c> and is commonly used to compute
    /// polar angles from directional vectors, such as when determining the angular position
    /// of a point on a circle or ellipse.
    /// </remarks>
    /// <returns>
    /// The angle in radians between this vector and the X-axis.
    /// </returns>
    public nfloat ArcAngle => nfloat.Atan2(Y, X);

    /// <summary>
    /// Rotates the vector counterclockwise by the given angle (in degrees).
    /// </summary>
    public Vector Rotate(nfloat degrees)
    {
        var radians = degrees * (nfloat)Math.PI / 180f;
        var cos = nfloat.Cos(radians);
        var sin = nfloat.Sin(radians);
        return new Vector(X * cos - Y * sin, X * sin + Y * cos);
    }

    /// <summary>
    /// Returns the dot product of two vectors.
    /// </summary>
    [DebuggerStepThrough]
    public static nfloat Dot(Vector lhs, Vector rhs) => lhs.X * rhs.X + lhs.Y * rhs.Y;

    /// <summary>
    /// Projects <paramref name="lhs"/> onto <paramref name="rhs"/>.
    /// </summary>
    [DebuggerStepThrough]
    public static Vector Project(Vector lhs, Vector rhs)
    {
        var denominator = rhs.X * rhs.X + rhs.Y * rhs.Y;
        if (denominator == 0) return Vector.Zero;
        var scalar = Dot(lhs, rhs) / denominator;
        return rhs * scalar;
    }

    /// <summary>
    /// Returns the 2D cross product (scalar) of two vectors.
    /// </summary>
    [DebuggerStepThrough]
    public static nfloat Cross(Vector lhs, Vector rhs) => lhs.X * rhs.Y - lhs.Y * rhs.X;

    /// <summary>
    /// Returns a new vector with its magnitude limited to the specified maximum.
    /// </summary>
    [DebuggerStepThrough]
    public Vector Clamp(nfloat max)
    {
        var mag = this.Magnitude;
        return mag > max ? this * (max / mag) : this;
    }

    /// <summary>
    /// Returns the clockwise angle (in radians) from the upward direction (0, 1) to the given vector.
    /// </summary>
    [DebuggerStepThrough]
    public static nfloat Angle(Vector v) =>
        nfloat.Atan2(v.X, v.Y);

    /// <summary>
    /// Returns a unit vector rotated clockwise from the upward direction (0, 1) by the given angle (in radians).
    /// </summary>
    [DebuggerStepThrough]
    public static Vector Angle(nfloat radians) =>
        new Vector(nfloat.Sin(radians), nfloat.Cos(radians));

    /// <summary>
    /// Returns the signed angle (in radians) from <paramref name="lhs"/> to <paramref name="rhs"/>, clockwise from upward.
    /// </summary>
    [DebuggerStepThrough]
    public static nfloat Angle(Vector lhs, Vector rhs) =>
        nfloat.Atan2(Cross(lhs, rhs), Dot(lhs, rhs));

    /// <summary>
    /// Linearly interpolates between two vectors by <paramref name="step"/>.
    /// </summary>
    [DebuggerStepThrough]
    public static Vector Lerp(Vector start, Vector end, nfloat step)
    {
        nfloat stepMinusOne = 1f - step;
        return new (start.X * stepMinusOne + end.X * step, start.Y * stepMinusOne + end.Y * step);
    }

    /// <summary>
    /// Implicitly converts a <see cref="Point"/> to a <see cref="Vector"/>.
    /// </summary>
    [DebuggerStepThrough]
    public static implicit operator Vector(Point point) => new (point.X, point.Y);

    /// <summary>
    /// Multiplies a vector by a scalar.
    /// </summary>
    [DebuggerStepThrough]
    public static Vector operator*(Vector v, nfloat f) => new (v.X * f, v.Y * f);

    /// <summary>
    /// Divides a vector by a scalar.
    /// </summary>
    [DebuggerStepThrough]
    public static Vector operator/(Vector v, nfloat f) => new (v.X / f, v.Y / f);

    /// <summary>
    /// Multiplies a scalar by a vector.
    /// </summary>
    [DebuggerStepThrough]
    public static Vector operator*(nfloat f, Vector v) => new (v.X * f, v.Y * f);

    /// <summary>
    /// Adds two vectors.
    /// </summary>
    [DebuggerStepThrough]
    public static Vector operator+(Vector lhs, Vector rhs) => new (lhs.X + rhs.X, lhs.Y + rhs.Y);

    /// <summary>
    /// Subtracts one vector from another.
    /// </summary>
    [DebuggerStepThrough]
    public static Vector operator-(Vector lhs, Vector rhs) => new (lhs.X - rhs.X, lhs.Y - rhs.Y);

    /// <summary>
    /// Implicitly converts a tuple to a <see cref="Vector"/>.
    /// </summary>
    [DebuggerStepThrough]
    public static implicit operator Vector(ValueTuple<nfloat, nfloat> tuple) => new (tuple.Item1, tuple.Item2);

    /// <summary>
    /// Returns true if the two vectors have equal components.
    /// </summary>
    [DebuggerStepThrough]
    public static bool operator ==(Vector lhs, Vector rhs) =>
        lhs.X == rhs.X && lhs.Y == rhs.Y;

    /// <summary>
    /// Returns true if any component of the two vectors is different.
    /// </summary>
    [DebuggerStepThrough]
    public static bool operator !=(Vector lhs, Vector rhs) =>
        lhs.X != rhs.X || lhs.Y != rhs.Y;

    /// <inheritdoc/>
    public override bool Equals(object? obj) =>
        obj is Vector other && this.X == other.X && this.Y == other.Y;

    /// <summary>
    /// Returns true if this vector is equal to another vector.
    /// </summary>
    public bool Equals(Vector other) =>
        this.X == other.X && this.Y == other.Y;

    /// <inheritdoc/>
    public override int GetHashCode() =>
        HashCode.Combine(X, Y);
        
    /// <inheritdoc/>
    public override string ToString() =>
        $"Vector [X={X}, Y={Y}]";
}