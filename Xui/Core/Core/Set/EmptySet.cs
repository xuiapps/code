namespace Xui.Core.Set;

/// <summary>
/// Represents an empty set — a set that contains no elements.
/// </summary>
/// <typeparam name="T">The type of elements the set could (but does not) contain.</typeparam>
/// <remarks>
/// This type always returns <c>false</c> from <see cref="Contains"/>. It is a useful identity for 
/// set composition and for scenarios like default hit test areas or disabled regions.
/// </remarks>
public sealed class EmptySet<T> : INonEnumerableSet<T>
{
    /// <summary>
    /// A shared singleton instance of the empty set.
    /// </summary>
    public static readonly EmptySet<T> Instance = new();

    // Private constructor to enforce singleton usage.
    private EmptySet() { }

    /// <summary>
    /// Always returns <c>false</c>, since no element belongs to the empty set.
    /// </summary>
    /// <param name="obj">The object to test.</param>
    /// <returns><c>false</c> for all inputs.</returns>
    public bool Contains(T obj) => false;
}
