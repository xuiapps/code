namespace Xui.Core.UI;

public partial class View
{
    private Dictionary<Extra, object?>? extras;

    /// <summary>Gets or sets an <see cref="nint"/> extra on this view.</summary>
    public nint this[Extra<nint> key]
    {
        get => extras != null && extras.TryGetValue(key, out var v) ? (nint)v! : key.DefaultValue;
        set => (extras ??= [])[key] = value;
    }

    /// <summary>Gets or sets an <see cref="nfloat"/> extra on this view.</summary>
    public nfloat this[Extra<nfloat> key]
    {
        get => extras != null && extras.TryGetValue(key, out var v) ? (nfloat)v! : key.DefaultValue;
        set => (extras ??= [])[key] = value;
    }

    /// <summary>Gets or sets a <see cref="string"/> extra on this view.</summary>
    public string? this[Extra<string?> key]
    {
        get => extras != null && extras.TryGetValue(key, out var v) ? (string?)v : key.DefaultValue;
        set => (extras ??= [])[key] = value;
    }

    /// <summary>Gets or sets a <see cref="bool"/> extra on this view.</summary>
    public bool this[Extra<bool> key]
    {
        get => extras != null && extras.TryGetValue(key, out var v) ? (bool)v! : key.DefaultValue;
        set => (extras ??= [])[key] = value;
    }

    /// <summary>
    /// Fallback indexer for extras whose value type is not covered by a typed overload (e.g. enums).
    /// Values are boxed. Prefer the typed overloads for <see cref="nint"/>, <see cref="nfloat"/>,
    /// <see cref="string"/>, and <see cref="bool"/> where possible.
    /// </summary>
    public object? this[Extra key]
    {
        get => extras != null && extras.TryGetValue(key, out var v) ? v : key.BoxedDefault;
        set => (extras ??= [])[key] = value;
    }

    /// <summary>
    /// Non-generic base for <see cref="Extra{T}"/>. Used as a dictionary key when storage is implemented.
    /// </summary>
    public abstract class Extra
    {
        internal abstract object? BoxedDefault { get; }
    }

    /// <summary>
    /// A typed key for attaching extra layout or styling data to any <see cref="View"/>.
    /// Declare one static instance per logical property (e.g. <c>Grid.Row</c>, <c>FlexBox.Grow</c>)
    /// and use it with the view indexer: <c>view[Grid.Row] = 2</c>.
    /// </summary>
    /// <typeparam name="T">The type of the value stored under this key.</typeparam>
    public sealed class Extra<T> : Extra
    {
        /// <summary>The value returned when no data has been stored for this key on a given view.</summary>
        public T DefaultValue { get; }

        internal override object? BoxedDefault => this.DefaultValue;

        /// <param name="defaultValue">Returned by the indexer getter until a value is explicitly set.</param>
        public Extra(T defaultValue = default!) => this.DefaultValue = defaultValue;

        /// <summary>
        /// Returns the value stored for this key on <paramref name="view"/>,
        /// or <see cref="DefaultValue"/> if nothing has been set.
        /// </summary>
        public T Get(View view) => view[this] is T value ? value : this.DefaultValue;
    }
}
