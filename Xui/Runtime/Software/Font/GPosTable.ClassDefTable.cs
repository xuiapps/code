using System;
using System.Buffers.Binary;
using System.Collections.Generic;

namespace Xui.Runtime.Software.Font;

public sealed partial class GPosTable
{
    public abstract partial class ClassDefTable
    {
        /// <summary>
        /// Parses a ClassDefTable (Format 1 or 2).
        /// </summary>
        public static ClassDefTable Parse(ReadOnlySpan<byte> span)
        {
            ushort format = BinaryPrimitives.ReadUInt16BigEndian(span.Slice(0, 2));
            return format switch
            {
                1 => new Format1(span),
                2 => new Format2(span),
                _ => throw new NotSupportedException($"Unsupported ClassDef format: {format}")
            };
        }

        public abstract ushort this[ushort glyphId] { get; }
    }
}
