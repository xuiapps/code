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
        public sealed class Format1 : CoverageTable
        {
            private readonly Dictionary<ushort, int> _map = new();

            public Format1(ReadOnlySpan<byte> span)
            {
                ushort glyphCount = BinaryPrimitives.ReadUInt16BigEndian(span.Slice(2, 2));
                for (int i = 0; i < glyphCount; i++)
                {
                    ushort glyph = BinaryPrimitives.ReadUInt16BigEndian(span.Slice(4 + i * 2, 2));
                    _map[glyph] = i;
                }
            }

            public override int Count => _map.Count;
            public override IEnumerable<ushort> Keys => _map.Keys;
            public override IEnumerable<int> Values => _map.Values;
            public override bool ContainsKey(ushort key) => _map.ContainsKey(key);
            public override bool TryGetValue(ushort key, out int value) => _map.TryGetValue(key, out value);
            public override int this[ushort key] => _map[key];
            public override IEnumerator<KeyValuePair<ushort, int>> GetEnumerator() => _map.GetEnumerator();
        }
    }
}
