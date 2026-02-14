using System;
using System.Diagnostics;
using Xui.Core.Set;

namespace Xui.Core.Math1D;

public partial class Interval<T> where T : IComparable<T>, IEquatable<T>
{
    /// <summary>
    /// Represents a half-open interval [<see cref="Start"/>, <see cref="End"/>) over a comparable type,
    /// modeled as a set with membership and containment operations.
    /// </summary>
    /// <remarks>
    /// The half-open convention [Start, End) means Start is inclusive and End is exclusive.
    /// The interval is empty when <c>Start &gt;= End</c>, making [n, n) a natural
    /// representation for a cursor position with no selection.
    /// </remarks>
    public struct ClosedOpen : INonEnumerableSet<T>, IEquatable<ClosedOpen>
    {
        /// <summary>The inclusive lower bound of the interval.</summary>
        public T Start;

        /// <summary>The exclusive upper bound of the interval.</summary>
        public T End;

        /// <summary>
        /// Creates a new half-open interval [<paramref name="start"/>, <paramref name="end"/>).
        /// </summary>
        [DebuggerStepThrough]
        public ClosedOpen(T start, T end)
        {
            this.Start = start;
            this.End = end;
        }

        /// <summary>
        /// Whether this interval represents the empty set (Start &gt;= End).
        /// </summary>
        public bool IsEmpty => this.Start.CompareTo(this.End) >= 0;

        /// <summary>
        /// Membership test: <paramref name="value"/> ∈ [Start, End).
        /// </summary>
        [DebuggerStepThrough]
        public bool Contains(T value) =>
            this.Start.CompareTo(value) <= 0 && value.CompareTo(this.End) < 0;

        /// <summary>
        /// Subset test: <paramref name="other"/> ⊆ this.
        /// </summary>
        [DebuggerStepThrough]
        public bool Contains(ClosedOpen other) =>
            other.IsEmpty || (this.Start.CompareTo(other.Start) <= 0 && other.End.CompareTo(this.End) <= 0);

        /// <summary>
        /// Whether two intervals share any elements (this ∩ other ≠ ∅).
        /// </summary>
        [DebuggerStepThrough]
        public bool Overlaps(ClosedOpen other) =>
            !this.IsEmpty && !other.IsEmpty &&
            this.Start.CompareTo(other.End) < 0 && other.Start.CompareTo(this.End) < 0;

        /// <summary>
        /// Set intersection: this ∩ <paramref name="other"/>.
        /// Returns ∅ if the intervals do not overlap.
        /// </summary>
        public ClosedOpen Intersect(ClosedOpen other)
        {
            var s = this.Start.CompareTo(other.Start) >= 0 ? this.Start : other.Start;
            var e = this.End.CompareTo(other.End) <= 0 ? this.End : other.End;
            return s.CompareTo(e) < 0 ? new ClosedOpen(s, e) : default;
        }

        /// <summary>
        /// Implicit conversion from a 2-tuple to a half-open interval.
        /// </summary>
        [DebuggerStepThrough]
        public static implicit operator ClosedOpen((T Start, T End) tuple) => new(tuple.Start, tuple.End);

        /// <inheritdoc/>
        [DebuggerStepThrough]
        public static bool operator ==(ClosedOpen left, ClosedOpen right) => left.Equals(right);

        /// <inheritdoc/>
        [DebuggerStepThrough]
        public static bool operator !=(ClosedOpen left, ClosedOpen right) => !left.Equals(right);

        /// <inheritdoc/>
        [DebuggerStepThrough]
        public bool Equals(ClosedOpen other) =>
            this.Start.Equals(other.Start) && this.End.Equals(other.End);

        /// <inheritdoc/>
        public override bool Equals(object? obj) =>
            obj is ClosedOpen other && this.Equals(other);

        /// <inheritdoc/>
        public override int GetHashCode() => HashCode.Combine(this.Start, this.End);

        /// <inheritdoc/>
        [DebuggerStepThrough]
        public override string ToString() => this.IsEmpty ? "∅" : $"[{this.Start}, {this.End})";
    }
}
