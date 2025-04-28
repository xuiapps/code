using System;
using System.Buffers.Binary;
using System.Collections.Generic;

namespace Xui.Runtime.Software.Font;

public sealed partial class GPosTable
{
    public abstract partial class CoverageTable : IReadOnlyDictionary<ushort, int>
    {
        public sealed class Format2 : CoverageTable
        {
            private readonly List<RangeRecord> _ranges = new();

            public Format2(ReadOnlySpan<byte> span)
            {
                ushort rangeCount = BinaryPrimitives.ReadUInt16BigEndian(span.Slice(2, 2));
                for (int i = 0; i < rangeCount; i++)
                {
                    int offset = 4 + i * 6;
                    ushort start = BinaryPrimitives.ReadUInt16BigEndian(span.Slice(offset, 2));
                    ushort end = BinaryPrimitives.ReadUInt16BigEndian(span.Slice(offset + 2, 2));
                    ushort startIndex = BinaryPrimitives.ReadUInt16BigEndian(span.Slice(offset + 4, 2));
                    _ranges.Add(new RangeRecord(start, end, startIndex));
                }
            }

            public override int Count
            {
                get
                {
                    int count = 0;
                    foreach (var r in _ranges)
                        count += r.End - r.Start + 1;
                    return count;
                }
            }

            public override IEnumerable<ushort> Keys
            {
                get
                {
                    foreach (var r in _ranges)
                    {
                        for (ushort g = r.Start; g <= r.End; g++)
                            yield return g;
                    }
                }
            }

            public override IEnumerable<int> Values
            {
                get
                {
                    foreach (var r in _ranges)
                    {
                        for (int i = 0; i <= r.End - r.Start; i++)
                            yield return r.StartIndex + i;
                    }
                }
            }

            public override bool ContainsKey(ushort key)
            {
                return TryGetValue(key, out _);
            }

            public override bool TryGetValue(ushort key, out int value)
            {
                foreach (var r in _ranges)
                {
                    if (key >= r.Start && key <= r.End)
                    {
                        value = r.StartIndex + (key - r.Start);
                        return true;
                    }
                }

                value = -1;
                return false;
            }

            public override int this[ushort key]
            {
                get
                {
                    if (!TryGetValue(key, out int value))
                        throw new KeyNotFoundException();
                    return value;
                }
            }

            public override IEnumerator<KeyValuePair<ushort, int>> GetEnumerator()
            {
                foreach (var r in _ranges)
                {
                    for (ushort g = r.Start; g <= r.End; g++)
                        yield return new KeyValuePair<ushort, int>(g, r.StartIndex + (g - r.Start));
                }
            }

            private readonly record struct RangeRecord(ushort Start, ushort End, ushort StartIndex);
        }
    }
}
