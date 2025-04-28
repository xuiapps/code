using System;
using System.Buffers.Binary;
using System.Collections;
using System.Collections.Generic;

namespace Xui.Runtime.Software.Font;

public sealed partial class GPosTable
{
    public sealed class FeatureListTable : IReadOnlyDictionary<string, FeatureTable>
    {
        private readonly Dictionary<string, FeatureTable> _features;

        public FeatureListTable(ReadOnlySpan<byte> span)
        {
            // FeatureList:
            //   0: uint16 featureCount
            //   2: FeatureRecord[featureCount]
            //       - Tag (4 bytes)
            //       - Offset16 featureOffset (from start of FeatureList)
            //
            // Each FeatureTable:
            //   0: uint16 featureParamsOffset (usually 0)
            //   2: uint16 lookupIndexCount
            //   4: uint16[] lookupListIndices

            ushort featureCount = BinaryPrimitives.ReadUInt16BigEndian(span.Slice(0, 2));
            _features = new(featureCount);

            for (int i = 0; i < featureCount; i++)
            {
                int offset = 2 + i * 6;
                string tag = System.Text.Encoding.ASCII.GetString(span.Slice(offset, 4));
                ushort featureOffset = BinaryPrimitives.ReadUInt16BigEndian(span.Slice(offset + 4, 2));
                var table = new FeatureTable(span.Slice(featureOffset));
                _features[tag] = table;
            }
        }

        public FeatureTable this[string key] => _features[key];
        public IEnumerable<string> Keys => _features.Keys;
        public IEnumerable<FeatureTable> Values => _features.Values;
        public int Count => _features.Count;

        public bool ContainsKey(string key) => _features.ContainsKey(key);
        public bool TryGetValue(string key, out FeatureTable value) => _features.TryGetValue(key, out value);
        public IEnumerator<KeyValuePair<string, FeatureTable>> GetEnumerator() => _features.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => _features.GetEnumerator();
    }

    public sealed class FeatureTable
    {
        public List<ushort> LookupIndices { get; } = new();

        public FeatureTable(ReadOnlySpan<byte> span)
        {
            // FeatureTable layout:
            //   0: uint16 featureParamsOffset (usually 0)
            //   2: uint16 lookupIndexCount
            //   4: uint16[] lookupIndices

            // Skip featureParamsOffset
            ushort count = BinaryPrimitives.ReadUInt16BigEndian(span.Slice(2, 2));
            for (int i = 0; i < count; i++)
            {
                ushort index = BinaryPrimitives.ReadUInt16BigEndian(span.Slice(4 + i * 2, 2));
                LookupIndices.Add(index);
            }
        }
    }
}
