using System;

namespace Xui.Core.Canvas;

/// <summary>
/// Fixed-capacity, allocation-free-after-construction LRU cache keyed by <see cref="Font"/>.
/// </summary>
/// <typeparam name="T">The cached value associated with a resolved font.</typeparam>
public class FontCache<T>
{
    /// <summary>The standard number of font entries retained by a context cache.</summary>
    public const int DefaultCapacity = 64;

    private readonly Entry[] entries;
    private ulong useSequence;

    /// <summary>Initializes a cache with the requested number of entries.</summary>
    public FontCache(int capacity = DefaultCapacity)
    {
        if (capacity <= 0)
            throw new ArgumentOutOfRangeException(nameof(capacity));

        entries = new Entry[capacity];
    }

    /// <summary>Gets the number of retained font entries.</summary>
    public int EntryCapacity => entries.Length;

    /// <summary>Gets the cached value for <paramref name="font"/> when available.</summary>
    public bool TryGet(in Font font, out T value)
    {
        for (int i = 0; i < entries.Length; i++)
        {
            ref var entry = ref entries[i];
            if (!entry.HasValue || entry.Font != font)
                continue;

            entry.LastUse = NextUseSequence();
            value = entry.Value;
            return true;
        }

        value = default!;
        return false;
    }

    /// <summary>Adds or refreshes the value for <paramref name="font"/>.</summary>
    public void Set(in Font font, T value)
    {
        int candidate = 0;
        ulong oldestUse = ulong.MaxValue;
        for (int i = 0; i < entries.Length; i++)
        {
            ref var entry = ref entries[i];
            if (entry.HasValue && entry.Font == font)
            {
                OnEvicted(entry.Value);
                entry.Value = value;
                entry.LastUse = NextUseSequence();
                return;
            }

            if (!entry.HasValue)
            {
                candidate = i;
                oldestUse = 0;
                break;
            }

            if (entry.LastUse < oldestUse)
            {
                candidate = i;
                oldestUse = entry.LastUse;
            }
        }

        ref var replaced = ref entries[candidate];
        if (replaced.HasValue)
            OnEvicted(replaced.Value);

        replaced = new Entry
        {
            HasValue = true,
            Font = font,
            Value = value,
            LastUse = NextUseSequence(),
        };
    }

    /// <summary>Clears every retained entry.</summary>
    public void Clear()
    {
        for (int i = 0; i < entries.Length; i++)
        {
            ref var entry = ref entries[i];
            if (entry.HasValue)
                OnEvicted(entry.Value);
        }

        Array.Clear(entries);
        useSequence = 0;
    }

    /// <summary>Releases a value when its entry is replaced, evicted, or cleared.</summary>
    protected virtual void OnEvicted(T value) { }

    private ulong NextUseSequence() => ++useSequence;

    private struct Entry
    {
        public bool HasValue;
        public Font Font;
        public T Value;
        public ulong LastUse;
    }
}
