using System.Diagnostics.CodeAnalysis;

namespace Xui.Core.UI
{
    /// <summary>
    /// Provides extension methods for navigating the Xui view tree.
    /// </summary>
    public static class ViewExtensions
    {
        /// <summary>
        /// Returns an allocation-free enumerable that walks up the parent chain of a view.
        /// </summary>
        /// <param name="view">The view whose parent chain should be traversed.</param>
        /// <returns>An enumerable of the view's parents, from immediate to root.</returns>
        public static ParentsEnumerable Parents(this View view) => new(view);

        /// <summary>
        /// Attempts to find the first parent of a specific type in the view tree.
        /// Zero allocations. Returns true if a matching parent was found.
        /// </summary>
        /// <typeparam name="T">The type of view to search for.</typeparam>
        /// <param name="view">The view whose parent chain will be searched.</param>
        /// <param name="result">The found parent of type <typeparamref name="T"/>, or null if not found.</param>
        /// <returns>True if a matching parent was found; otherwise, false.</returns>
        public static bool TryFindParent<T>(this View view, [NotNullWhen(true)] out T? result) where T : View
        {
            foreach (var parent in view.Parents())
            {
                if (parent is T typed)
                {
                    result = typed;
                    return true;
                }
            }

            result = null;
            return false;
        }

        /// <summary>
        /// Allocation-free enumerable for walking up the parent chain of a view.
        /// </summary>
        public readonly ref struct ParentsEnumerable
        {
            private readonly View? start;

            /// <summary>
            /// Creates a new <see cref="ParentsEnumerable"/> starting from the given view.
            /// </summary>
            /// <param name="start">The starting view.</param>
            public ParentsEnumerable(View? start) => this.start = start;

            /// <summary>
            /// Returns an enumerator that iterates over the view's parents.
            /// </summary>
            public Enumerator GetEnumerator() => new(start);

            /// <summary>
            /// Enumerator for <see cref="ParentsEnumerable"/>, walking up the view hierarchy.
            /// </summary>
            public ref struct Enumerator
            {
                private View? current;

                /// <summary>
                /// Initializes a new instance of the <see cref="Enumerator"/> struct.
                /// </summary>
                /// <param name="start">The starting view for traversal.</param>
                public Enumerator(View? start) => current = start?.Parent;

                /// <summary>
                /// Gets the current parent view in the traversal.
                /// </summary>
                public View Current => current!;

                /// <summary>
                /// Moves to the next parent view.
                /// </summary>
                /// <returns>True if there is a next parent; otherwise, false.</returns>
                public bool MoveNext()
                {
                    if (current is null) return false;
                    current = current.Parent;
                    return current is not null;
                }
            }
        }
    }
}
