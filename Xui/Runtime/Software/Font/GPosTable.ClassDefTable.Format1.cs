using System;
using System.Buffers.Binary;

namespace Xui.Runtime.Software.Font;

public sealed partial class GPosTable
{
    public abstract partial class ClassDefTable
    {
        public sealed class Format1 : ClassDefTable
        {
            private readonly ushort _startGlyph;
            private readonly ushort[] _classValues;

            public Format1(ReadOnlySpan<byte> span)
            {
                // ClassDefTable Format 1:
                //   0: uint16 format (=1)
                //   2: uint16 startGlyph
                //   4: uint16 glyphCount
                //   6: uint16[glyphCount] classValues

                ushort format = BinaryPrimitives.ReadUInt16BigEndian(span.Slice(0, 2));
                if (format != 1)
                    throw new NotSupportedException("Only ClassDef Format 1 is supported.");

                _startGlyph = BinaryPrimitives.ReadUInt16BigEndian(span.Slice(2, 2));
                ushort glyphCount = BinaryPrimitives.ReadUInt16BigEndian(span.Slice(4, 2));

                _classValues = new ushort[glyphCount];
                for (int i = 0; i < glyphCount; i++)
                {
                    _classValues[i] = BinaryPrimitives.ReadUInt16BigEndian(span.Slice(6 + i * 2, 2));
                }
            }

            /// <summary>
            /// Returns the class ID for the given glyph ID.
            /// If the glyph is not in the defined range, returns 0 (default class).
            /// </summary>
            public override ushort this[ushort glyphId]
            {
                get
                {
                    int index = glyphId - _startGlyph;
                    if (index >= 0 && index < _classValues.Length)
                        return _classValues[index];

                    return 0;
                }
            }
        }
    }
}
