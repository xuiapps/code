using System;
using System.Diagnostics;
using Xui.Core.Set;

namespace Xui.Core.Math1D;

public partial class Interval<T> where T : IComparable<T>, IEquatable<T>
{
    /// <summary>
    /// Represents a closed interval [<see cref="Start"/>, <see cref="End"/>] over a comparable type,
    /// modeled as a set with membership and containment operations.
    /// </summary>
    /// <remarks>
    /// The closed convention [Start, End] means both bounds are inclusive.
    /// The interval is empty when <c>Start &gt; End</c>.
    /// </remarks>
    public struct Closed : INonEnumerableSet<T>, IEquatable<Closed>
    {
        /// <summary>The inclusive lower bound of the interval.</summary>
        public T Start;

        /// <summary>The inclusive upper bound of the interval.</summary>
        public T End;

        /// <summary>
        /// Creates a new closed interval [<paramref name="start"/>, <paramref name="end"/>].
        /// </summary>
        [DebuggerStepThrough]
        public Closed(T start, T end)
        {
            this.Start = start;
            this.End = end;
        }

        /// <summary>
        /// Whether this interval represents the empty set (Start &gt; End).
        /// </summary>
        public bool IsEmpty => this.Start.CompareTo(this.End) > 0;

        /// <summary>
        /// Membership test: <paramref name="value"/> ∈ [Start, End].
        /// </summary>
        [DebuggerStepThrough]
        public bool Contains(T value) =>
            this.Start.CompareTo(value) <= 0 && value.CompareTo(this.End) <= 0;

        /// <summary>
        /// Subset test: <paramref name="other"/> ⊆ this.
        /// </summary>
        [DebuggerStepThrough]
        public bool Contains(Closed other) =>
            other.IsEmpty || (this.Start.CompareTo(other.Start) <= 0 && other.End.CompareTo(this.End) <= 0);

        /// <summary>
        /// Whether two intervals share any elements (this ∩ other ≠ ∅).
        /// </summary>
        [DebuggerStepThrough]
        public bool Overlaps(Closed other) =>
            !this.IsEmpty && !other.IsEmpty &&
            this.Start.CompareTo(other.End) <= 0 && other.Start.CompareTo(this.End) <= 0;

        /// <summary>
        /// Set intersection: this ∩ <paramref name="other"/>.
        /// Returns ∅ if the intervals do not overlap.
        /// </summary>
        public Closed Intersect(Closed other)
        {
            var s = this.Start.CompareTo(other.Start) >= 0 ? this.Start : other.Start;
            var e = this.End.CompareTo(other.End) <= 0 ? this.End : other.End;
            return s.CompareTo(e) <= 0 ? new Closed(s, e) : default;
        }

        /// <summary>
        /// Implicit conversion from a 2-tuple to a closed interval.
        /// </summary>
        [DebuggerStepThrough]
        public static implicit operator Closed((T Start, T End) tuple) => new(tuple.Start, tuple.End);

        /// <inheritdoc/>
        [DebuggerStepThrough]
        public static bool operator ==(Closed left, Closed right) => left.Equals(right);

        /// <inheritdoc/>
        [DebuggerStepThrough]
        public static bool operator !=(Closed left, Closed right) => !left.Equals(right);

        /// <inheritdoc/>
        [DebuggerStepThrough]
        public bool Equals(Closed other) =>
            this.Start.Equals(other.Start) && this.End.Equals(other.End);

        /// <inheritdoc/>
        public override bool Equals(object? obj) =>
            obj is Closed other && this.Equals(other);

        /// <inheritdoc/>
        public override int GetHashCode() => HashCode.Combine(this.Start, this.End);

        /// <inheritdoc/>
        [DebuggerStepThrough]
        public override string ToString() => this.IsEmpty ? "∅" : $"[{this.Start}, {this.End}]";
    }
}
