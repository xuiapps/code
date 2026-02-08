using System.Collections;

namespace Xui.Core.UI;

/// <summary>
/// A compact collection of CSS-like class name strings for a view.
/// Stores up to 3 class names inline without allocation. If more are added,
/// spills over into a <see cref="HashSet{T}"/>.
/// </summary>
public struct ClassNameCollection : IEnumerable<string>
{
    private string? c0;
    private string? c1;
    private string? c2;
    private HashSet<string>? overflow;

    /// <summary>
    /// Returns the number of class names in the collection.
    /// </summary>
    public readonly int Count
    {
        get
        {
            if (this.overflow is not null)
                return this.overflow.Count;

            int count = 0;
            if (this.c0 is not null) count++;
            if (this.c1 is not null) count++;
            if (this.c2 is not null) count++;
            return count;
        }
    }

    /// <summary>
    /// Returns true if the collection contains the specified class name.
    /// </summary>
    public readonly bool Contains(string className)
    {
        if (this.overflow is not null)
            return this.overflow.Contains(className);

        return this.c0 == className || this.c1 == className || this.c2 == className;
    }

    /// <summary>
    /// Adds a class name to the collection. Returns true if it was added (not already present).
    /// </summary>
    public bool Add(string className)
    {
        if (this.overflow is not null)
            return this.overflow.Add(className);

        if (this.c0 == className || this.c1 == className || this.c2 == className)
            return false;

        if (this.c0 is null) { this.c0 = className; return true; }
        if (this.c1 is null) { this.c1 = className; return true; }
        if (this.c2 is null) { this.c2 = className; return true; }

        // Spill into HashSet
        this.overflow = new HashSet<string> { this.c0, this.c1, this.c2, className };
        this.c0 = null;
        this.c1 = null;
        this.c2 = null;
        return true;
    }

    /// <summary>
    /// Removes a class name from the collection. Returns true if it was found and removed.
    /// </summary>
    public bool Remove(string className)
    {
        if (this.overflow is not null)
        {
            bool removed = this.overflow.Remove(className);

            // Collapse back to inline if small enough
            if (removed && this.overflow.Count <= 3)
            {
                var en = this.overflow.GetEnumerator();
                this.c0 = en.MoveNext() ? en.Current : null;
                this.c1 = en.MoveNext() ? en.Current : null;
                this.c2 = en.MoveNext() ? en.Current : null;
                this.overflow = null;
            }

            return removed;
        }

        if (this.c0 == className) { this.c0 = this.c1; this.c1 = this.c2; this.c2 = null; return true; }
        if (this.c1 == className) { this.c1 = this.c2; this.c2 = null; return true; }
        if (this.c2 == className) { this.c2 = null; return true; }

        return false;
    }

    public static ClassNameCollection operator +(ClassNameCollection collection, string className)
    {
        collection.Add(className);
        return collection;
    }

    public static ClassNameCollection operator -(ClassNameCollection collection, string className)
    {
        collection.Remove(className);
        return collection;
    }

    public readonly IEnumerator<string> GetEnumerator()
    {
        if (this.overflow is not null)
            return this.overflow.GetEnumerator();

        return GetInlineEnumerator();
    }

    readonly IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    private readonly IEnumerator<string> GetInlineEnumerator()
    {
        var s0 = c0;
        var s1 = c1;
        var s2 = c2;
        return Enumerate(s0, s1, s2);

        static IEnumerator<string> Enumerate(string? s0, string? s1, string? s2)
        {
            if (s0 is not null) yield return s0;
            if (s1 is not null) yield return s1;
            if (s2 is not null) yield return s2;
        }
    }
}
