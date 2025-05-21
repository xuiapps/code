using Xui.Core.Canvas;
using Xui.Core.Math2D;

namespace Xui.Core.UI;

/// <summary>
/// A base class for container views that hold and manage a list of child views.
/// Provides methods for adding, removing, rendering, and hit testing children.
/// </summary>
public class ViewCollection : View
{
    /// <summary>
    /// The internal list of child views contained within this view.
    /// </summary>
    protected readonly List<View> children = new();

    /// <summary>
    /// Gets the number of child views in this collection.
    /// </summary>
    public override int Count => children.Count;

    /// <summary>
    /// Gets the child view at the specified index.
    /// </summary>
    /// <param name="index">The index of the child to retrieve.</param>
    /// <returns>The child view at the given index.</returns>
    public override View this[int index] => children[index];

    /// <summary>
    /// Adds the provided views to this container during object initialization.
    /// This property is intended for use with C# object initializers and will not clear existing children.
    /// </summary>
    public ReadOnlySpan<View> Content
    {
        init
        {
            foreach (var view in value)
            {
                Add(view);
            }
        }
    }

    /// <summary>
    /// Adds a view to this container.
    /// </summary>
    /// <param name="child">The view to add.</param>
    /// <exception cref="InvalidOperationException">Thrown if the view already has a parent.</exception>
    public virtual void Add(View child)
    {
        if (child.Parent != null)
            throw new InvalidOperationException("View already has a parent.");
        child.Parent = this;
        children.Add(child);
    }

    /// <summary>
    /// Removes a view from this container.
    /// </summary>
    /// <param name="child">The view to remove.</param>
    public virtual void Remove(View child)
    {
        if (children.Remove(child))
            child.Parent = null;
    }
}
