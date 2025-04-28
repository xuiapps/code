using System;
using System.Buffers.Binary;
using System.Collections;
using System.Collections.Generic;

namespace Xui.Runtime.Software.Font;

public sealed partial class GPosTable
{
    // Represents the ScriptList section of the GPOS table
    public sealed class ScriptListTable : IReadOnlyDictionary<string, ScriptTable>
    {
        private readonly Dictionary<string, ScriptTable> _scripts;

        public ScriptListTable(ReadOnlySpan<byte> span)
        {
            // ScriptList:
            //   0: uint16 scriptCount
            //   2: ScriptRecord[scriptCount]
            //       - Tag (4 bytes)
            //       - Offset16 scriptOffset (from start of ScriptList)

            ushort scriptCount = BinaryPrimitives.ReadUInt16BigEndian(span.Slice(0, 2));
            _scripts = new(scriptCount);

            for (int i = 0; i < scriptCount; i++)
            {
                int offset = 2 + i * 6;
                string tag = System.Text.Encoding.ASCII.GetString(span.Slice(offset, 4));
                ushort scriptOffset = BinaryPrimitives.ReadUInt16BigEndian(span.Slice(offset + 4, 2));
                var table = new ScriptTable(span.Slice(scriptOffset));
                _scripts[tag] = table;
            }
        }

        public ScriptTable this[string key]
        {
            get => _scripts.TryGetValue(key, out var table)
                ? table
                : _scripts.TryGetValue("DFLT", out var fallback)
                    ? fallback
                    : throw new KeyNotFoundException($"Script tag '{key}' not found and no 'DFLT' fallback.");
        }

        public IEnumerable<string> Keys => _scripts.Keys;
        public IEnumerable<ScriptTable> Values => _scripts.Values;
        public int Count => _scripts.Count;

        public bool ContainsKey(string key) => _scripts.ContainsKey(key) || _scripts.ContainsKey("DFLT");

        public bool TryGetValue(string key, out ScriptTable value)
        {
            if (_scripts.TryGetValue(key, out value)) return true;
            if (_scripts.TryGetValue("DFLT", out value)) return true;
            value = null!;
            return false;
        }

        public IEnumerator<KeyValuePair<string, ScriptTable>> GetEnumerator() => _scripts.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => _scripts.GetEnumerator();
    }

    // Represents a parsed ScriptTable with a default and named language systems.
    public sealed class ScriptTable : IReadOnlyDictionary<string, LangSysTable>
    {
        private readonly Dictionary<string, LangSysTable> _languages;
        private readonly LangSysTable? _default;

        public ScriptTable(ReadOnlySpan<byte> span)
        {
            // ScriptTable layout:
            //   0: Offset16 defaultLangSys
            //   2: uint16 langSysCount
            //   4: LangSysRecord[langSysCount]
            //       - Tag (4 bytes)
            //       - Offset16 langSysOffset

            ushort defaultLangSysOffset = BinaryPrimitives.ReadUInt16BigEndian(span.Slice(0, 2));
            if (defaultLangSysOffset != 0)
                _default = new LangSysTable(span.Slice(defaultLangSysOffset));

            ushort langSysCount = BinaryPrimitives.ReadUInt16BigEndian(span.Slice(2, 2));
            _languages = new(langSysCount);

            for (int i = 0; i < langSysCount; i++)
            {
                int offset = 4 + i * 6;
                string tag = System.Text.Encoding.ASCII.GetString(span.Slice(offset, 4));
                ushort langSysOffset = BinaryPrimitives.ReadUInt16BigEndian(span.Slice(offset + 4, 2));
                var langSys = new LangSysTable(span.Slice(langSysOffset));
                _languages[tag] = langSys;
            }
        }

        public LangSysTable this[string key]
        {
            get => _languages.TryGetValue(key, out var value)
                ? value
                : _default ?? throw new KeyNotFoundException($"LangSys tag '{key}' not found and no default.");
        }

        public IEnumerable<string> Keys => _languages.Keys;
        public IEnumerable<LangSysTable> Values => _languages.Values;
        public int Count => _languages.Count;

        public bool ContainsKey(string key) => _languages.ContainsKey(key) || _default != null;

        public bool TryGetValue(string key, out LangSysTable value)
        {
            if (_languages.TryGetValue(key, out value)) return true;
            if (_default != null)
            {
                value = _default;
                return true;
            }
            value = null!;
            return false;
        }

        public IEnumerator<KeyValuePair<string, LangSysTable>> GetEnumerator() => _languages.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => _languages.GetEnumerator();
    }

    // Represents the LangSys table with required feature and additional features.
    public sealed class LangSysTable
    {
        public ushort RequiredFeatureIndex { get; }
        public List<ushort> FeatureIndices { get; } = new();

        public LangSysTable(ReadOnlySpan<byte> span)
        {
            // LangSysTable layout:
            //   0: Offset16 lookupOrder (usually 0)
            //   2: uint16 requiredFeatureIndex (0xFFFF = none)
            //   4: uint16 featureIndexCount
            //   6: uint16[] featureIndices

            // Skip lookupOrder (offset 0, usually 0)
            RequiredFeatureIndex = BinaryPrimitives.ReadUInt16BigEndian(span.Slice(2, 2));
            ushort count = BinaryPrimitives.ReadUInt16BigEndian(span.Slice(4, 2));

            for (int i = 0; i < count; i++)
            {
                ushort index = BinaryPrimitives.ReadUInt16BigEndian(span.Slice(6 + i * 2, 2));
                FeatureIndices.Add(index);
            }
        }
    }
}
