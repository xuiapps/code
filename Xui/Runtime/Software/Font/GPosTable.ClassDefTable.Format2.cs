using System;
using System.Buffers.Binary;
using System.Collections.Generic;

namespace Xui.Runtime.Software.Font;

public sealed partial class GPosTable
{
    public abstract partial class ClassDefTable
    {
        public sealed class Format2 : ClassDefTable
        {
            private readonly List<ClassRangeRecord> _ranges = new();

            public Format2(ReadOnlySpan<byte> span)
            {
                // Format 2 layout:
                //   0: uint16 format (=2)
                //   2: uint16 classRangeCount
                //   4: ClassRangeRecord[classRangeCount]
                //
                // ClassRangeRecord:
                //   - uint16 startGlyphID
                //   - uint16 endGlyphID
                //   - uint16 classValue

                ushort rangeCount = BinaryPrimitives.ReadUInt16BigEndian(span.Slice(2, 2));

                for (int i = 0; i < rangeCount; i++)
                {
                    int offset = 4 + i * 6;
                    ushort start = BinaryPrimitives.ReadUInt16BigEndian(span.Slice(offset, 2));
                    ushort end = BinaryPrimitives.ReadUInt16BigEndian(span.Slice(offset + 2, 2));
                    ushort classId = BinaryPrimitives.ReadUInt16BigEndian(span.Slice(offset + 4, 2));
                    _ranges.Add(new ClassRangeRecord(start, end, classId));
                }
            }

            public override ushort this[ushort glyphId]
            {
                get
                {
                    int low = 0;
                    int high = _ranges.Count - 1;

                    while (low <= high)
                    {
                        int mid = (low + high) / 2;
                        var range = _ranges[mid];

                        if (glyphId < range.Start)
                        {
                            high = mid - 1;
                        }
                        else if (glyphId > range.End)
                        {
                            low = mid + 1;
                        }
                        else
                        {
                            return range.Class;
                        }
                    }

                    return 0; // Default class if not found
                }
            }

            private readonly record struct ClassRangeRecord(ushort Start, ushort End, ushort Class);
        }
    }
}
