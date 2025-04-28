using System;
using System.Buffers.Binary;
using System.Collections.Generic;

namespace Xui.Runtime.Software.Font;

public sealed partial class GPosTable
{
    /// <summary>
    /// Represents a GPOS Coverage Table used in subtables to identify glyphs affected.
    /// </summary>
    public abstract partial class CoverageTable : IReadOnlyDictionary<ushort, int>
    {
        public static CoverageTable Parse(ReadOnlySpan<byte> span)
        {
            ushort format = BinaryPrimitives.ReadUInt16BigEndian(span.Slice(0, 2));
            return format switch
            {
                1 => new Format1(span),
                2 => new Format2(span),
                _ => throw new NotSupportedException($"Unsupported CoverageTable format: {format}")
            };
        }

        public abstract int Count { get; }
        public abstract IEnumerable<ushort> Keys { get; }
        public abstract IEnumerable<int> Values { get; }

        public abstract bool ContainsKey(ushort key);
        public abstract bool TryGetValue(ushort key, out int value);
        public abstract int this[ushort key] { get; }
        public abstract IEnumerator<KeyValuePair<ushort, int>> GetEnumerator();
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
