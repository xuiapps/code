
namespace Xui.Core.Set;

/// <summary>
/// Represents a mathematical set as defined by its membership test: 
/// whether an element belongs to the set.
/// </summary>
/// <typeparam name="T">The type of elements being tested for membership.</typeparam>
/// <remarks>
/// This interface does not imply enumeration, mutation, or collection semantics. 
/// It aligns with the pure set-theoretic notion where a set is characterized solely 
/// by the ability to determine if a value is a member of the set (i.e., <c>x ∈ S</c>).
/// </remarks>
public interface INonEnumerableSet<T>
{
    /// <summary>
    /// Determines whether the specified element is a member of this set.
    /// </summary>
    /// <param name="obj">The element to test for membership.</param>
    /// <returns><c>true</c> if the element belongs to the set; otherwise, <c>false</c>.</returns>
    public bool Contains(T obj);
}
