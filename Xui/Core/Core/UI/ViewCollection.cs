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
    public override int Count => this.children.Count;

    /// <summary>
    /// Gets the child view at the specified index.
    /// </summary>
    public override View this[int index] => this.children[index];

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
                this.Add(view);
            }
        }
    }

    /// <summary>
    /// Adds a view to this container.
    /// </summary>
    /// <exception cref="InvalidOperationException">Thrown if the view already has a parent.</exception>
    public virtual void Add(View child)
    {
        if (child is null)
            throw new ArgumentNullException(nameof(child));

        // Centralized parent wiring + invalidation
        this.AddProtectedChild(child);

        // Maintain the list after we know attach succeeded
        this.children.Add(child);
    }

    /// <summary>
    /// Removes a view from this container.
    /// </summary>
    public virtual void Remove(View child)
    {
        if (child is null)
            throw new ArgumentNullException(nameof(child));

        // Only detach if it was actually present
        if (this.children.Remove(child))
        {
            // Centralized parent unwiring + invalidation
            this.RemoveProtectedChild(child);
        }
    }

    /// <summary>
    /// Removes all child views from this container.
    /// </summary>
    public virtual void Clear()
    {
        if (this.children.Count == 0)
            return;

        // Detach all first (so if something throws you don't end half-detached)
        for (int i = 0; i < this.children.Count; i++)
        {
            this.RemoveProtectedChild(this.children[i]);
        }

        this.children.Clear();
    }
}
