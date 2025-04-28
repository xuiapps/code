using System;
using System.Buffers.Binary;
using Xui.Core.Canvas;
using Xui.Core.Math2D;

namespace Xui.Runtime.Software.Font;

/// <summary>
/// A parsed TrueType glyph. This is a lightweight, stack-only view into raw glyph data.
/// </summary>
public ref struct GlyphShape
{
    private readonly ReadOnlySpan<byte> _data;

    public GlyphShape(ReadOnlySpan<byte> data)
    {
        _data = data;
    }

    public void Visit(IGlyphPathBuilder builder)
    {
        /*
        TrueType Simple Glyph Format Layout
        -----------------------------------

        After the 10-byte glyph header, a simple glyph is laid out as follows:

        Offset (from start)         Field
        ----------------------      ----------------------------------------------
        +0                          Glyph Header (10 bytes total):
                                    - int16 numberOfContours
                                    - int16 xMin
                                    - int16 yMin
                                    - int16 xMax
                                    - int16 yMax

        +10                         endPtsOfContours[] (2 bytes each)
                                    - Count: numberOfContours
                                    - Each entry gives the last point index of a contour

        +10 + (2 * numberOfContours) instructionLength (2 bytes)
                                    - Length of instructions that follow

        +...                        instructions[] (instructionLength bytes)
                                    - Hinting instructions (can be ignored)

        +...                        flags[] (variable length)
                                    - One flag per point, compressed using REPEAT_FLAG
                                    - Bits:
                                        Bit 0: ON_CURVE_POINT
                                        Bit 1: X_SHORT_VECTOR
                                        Bit 2: Y_SHORT_VECTOR
                                        Bit 3: REPEAT_FLAG
                                        Bit 4: X_IS_SAME_OR_POSITIVE_X_SHORT_VECTOR
                                        Bit 5: Y_IS_SAME_OR_POSITIVE_Y_SHORT_VECTOR

        +...                        xCoordinates[] (variable length)
                                    - Delta-encoded using flags
                                    - Uses 1 or 2 bytes per point depending on flags

        +...                        yCoordinates[] (variable length)
                                    - Delta-encoded using flags
                                    - Uses 1 or 2 bytes per point depending on flags

        Notes:
        - The total number of points = endPtsOfContours[numberOfContours - 1] + 1
        - Points must be reconstructed by walking flags and applying deltas
        */

        short numberOfContours = BinaryPrimitives.ReadInt16BigEndian(_data.Slice(0, 2));
        short xMin = BinaryPrimitives.ReadInt16BigEndian(_data.Slice(2, 2));
        short yMin = BinaryPrimitives.ReadInt16BigEndian(_data.Slice(4, 2));
        short xMax = BinaryPrimitives.ReadInt16BigEndian(_data.Slice(6, 2));
        short yMax = BinaryPrimitives.ReadInt16BigEndian(_data.Slice(8, 2));

        if (numberOfContours < 0)
        {
            // Composite glyphs not yet supported
            return;
        }

        int instructionOffset = 10 + numberOfContours * 2;
        ushort instructionLength = BinaryPrimitives.ReadUInt16BigEndian(_data.Slice(instructionOffset, 2));
        int flagsOffset = instructionOffset + 2 + instructionLength;

        int lastEndPtOffset = 10 + (numberOfContours - 1) * 2;
        int pointCount = BinaryPrimitives.ReadUInt16BigEndian(_data.Slice(lastEndPtOffset, 2)) + 1;

        CalculateCoordinateByteSizes(_data, flagsOffset, pointCount, out int flagsByteLen, out int xCoordBytes, out int yCoordBytes);
        int xCoordOffset = flagsOffset + flagsByteLen;
        int yCoordOffset = xCoordOffset + xCoordBytes;

        int pointIndex = 0;
        var flagReader = new FlagReader(_data, flagsOffset);
        var pointDecoder = new PointDecoder(_data, xCoordOffset, yCoordOffset);
        for (int contourIndex = 0; contourIndex < numberOfContours; contourIndex++)
        {
            var contourEnd = BinaryPrimitives.ReadUInt16BigEndian(_data.Slice(10 + contourIndex * 2, 2));

            var path = new GlyphPath(builder);
            for (int contourStart = pointIndex; pointIndex <= contourEnd; pointIndex++)
            {
                byte flag = flagReader.Next();
                Point currentPoint = pointDecoder.Next(flag);
                bool isOffCurve = (flag & (byte)Flags.OnCurve) == 0;

                path.Push(currentPoint, isOffCurve);
            }

            path.End();
        }
    }

    private ref struct GlyphPath
    {
        private readonly IGlyphPathBuilder builder;

        private Point? firstOff;
        private Point? startOnPoint;
        private Point? lastPointOff;

        uint index;

        public GlyphPath(IGlyphPathBuilder builder)
        {
            this.builder = builder;
            firstOff = null;
            startOnPoint = null;
            lastPointOff = null;
            index = 0;
        }

        public void Push(Point p, bool isOff)
        {
            // Flip Y to convert from font-space (Y-up) to canvas-space (Y-down)
            p = new Point(p.X, -p.Y);

            if (index == 0)
            {
                if (isOff)
                {
                    firstOff = p;
                }
                else
                {
                    builder.MoveTo(p);
                    startOnPoint = p;
                }
            }
            else if (index == 1 && firstOff is not null)
            {
                var mid = Point.Lerp(firstOff.Value, p, (nfloat).5);
                startOnPoint = mid;
                builder.MoveTo(mid);
                lastPointOff = p;
            }
            else
            {
                if (isOff)
                {
                    if (lastPointOff is not null)
                    {
                        var mid = Point.Lerp(lastPointOff.Value, p, (nfloat).5);
                        builder.CurveTo(lastPointOff.Value, mid);
                    }
                    lastPointOff = p;
                }
                else
                {
                    if (lastPointOff is not null)
                    {
                        builder.CurveTo(lastPointOff.Value, p);
                        lastPointOff = null;
                    }
                    else
                    {
                        builder.LineTo(p);
                    }
                }
            }

            index++;
        }

        public void End()
        {
            if (lastPointOff is null)
            {
                if (firstOff is null)
                {
                    builder.ClosePath();
                }
                else
                {
                    if (startOnPoint is not null)
                    {
                        builder.CurveTo(firstOff.Value, startOnPoint.Value);
                        builder.ClosePath();
                    }
                }
            }
            else
            {
                if (firstOff is null)
                {
                    if (startOnPoint is not null)
                    {
                        builder.CurveTo(lastPointOff.Value, startOnPoint.Value);
                        builder.ClosePath();
                    }
                }
                else
                {
                    var mid = Point.Lerp(lastPointOff.Value, firstOff.Value, (nfloat).5);
                    if (startOnPoint.HasValue)
                    {
                        builder.CurveTo(lastPointOff.Value, mid);
                        builder.CurveTo(firstOff.Value, startOnPoint.Value);
                        builder.ClosePath();
                    }
                }
            }
        }
    }

    private static void CalculateCoordinateByteSizes(
        ReadOnlySpan<byte> data, int flagsOffset, int pointCount,
        out int flagsByteLength, out int xByteLength, out int yByteLength)
    {

        int cursor = flagsOffset;
        int flagsRead = 0;

        flagsByteLength = 0;
        xByteLength = 0;
        yByteLength = 0;

        while (flagsRead < pointCount && cursor < data.Length)
        {
            byte flag = data[cursor++];
            flagsByteLength++;

            int repeatCount = 1;
            if ((flag & (byte)Flags.RepeatFlag) != 0)
            {
                if (cursor >= data.Length)
                    break;

                repeatCount = data[cursor++] + 1;
                flagsByteLength++;
            }

            for (int i = 0; i < repeatCount && flagsRead < pointCount; i++, flagsRead++)
            {
                if ((flag & (byte)Flags.XShortVector) != 0)
                    xByteLength += 1;
                else if ((flag & (byte)Flags.XSameOrPositive) == 0)
                    xByteLength += 2;

                if ((flag & (byte)Flags.YShortVector) != 0)
                    yByteLength += 1;
                else if ((flag & (byte)Flags.YSameOrPositive) == 0)
                    yByteLength += 2;
            }
        }
    }

    private ref struct FlagReader
    {
        private ReadOnlySpan<byte> _data;
        private int _cursor;
        private byte _currentFlag;
        private int _repeatCount;

        public FlagReader(ReadOnlySpan<byte> data, int start)
        {
            _data = data;
            _cursor = start;
            _currentFlag = 0;
            _repeatCount = 0;
        }

        public byte Next()
        {
            if (_repeatCount > 0)
            {
                _repeatCount--;
                return _currentFlag;
            }

            if (_cursor >= _data.Length)
                return 0; // or throw?

            _currentFlag = _data[_cursor++];

            if ((_currentFlag & (byte)Flags.RepeatFlag) != 0)
            {
                if (_cursor < _data.Length)
                    _repeatCount = _data[_cursor++];
            }

            return _currentFlag;
        }

        public int Cursor => _cursor; // to update x/y offset base later
    }

    /// <summary>
    /// Flags used in TrueType glyph data to describe point format and compression.
    /// These are defined per-point and may be repeated using run-length encoding.
    /// </summary>
    [Flags]
    enum Flags : byte
    {
        /// <summary>
        /// If set, the point is on the curve.
        /// If not set, the point is an off-curve control point (used in quadratic Bézier curves).
        /// </summary>
        OnCurve = 1 << 0,

        /// <summary>
        /// If set, the X-coordinate is stored as a 1-byte unsigned delta.
        /// If not set, the X-coordinate is stored as a 2-byte signed delta (unless skipped).
        /// </summary>
        XShortVector = 1 << 1,

        /// <summary>
        /// If set, the Y-coordinate is stored as a 1-byte unsigned delta.
        /// If not set, the Y-coordinate is stored as a 2-byte signed delta (unless skipped).
        /// </summary>
        YShortVector = 1 << 2,

        /// <summary>
        /// If set, the next byte specifies how many times to repeat this flag.
        /// Used for run-length encoding of flags to reduce size.
        /// </summary>
        RepeatFlag = 1 << 3,

        /// <summary>
        /// Interprets the X-coordinate delta:
        /// - If XShortVector is set:
        ///   - If this bit is set, the delta is positive.
        ///   - If not set, the delta is negative.
        /// - If XShortVector is not set:
        ///   - If this bit is set, no delta is stored (X is same as previous).
        ///   - If not set, a 2-byte signed delta follows.
        /// </summary>
        XSameOrPositive = 1 << 4,

        /// <summary>
        /// Interprets the Y-coordinate delta:
        /// - If YShortVector is set:
        ///   - If this bit is set, the delta is positive.
        ///   - If not set, the delta is negative.
        /// - If YShortVector is not set:
        ///   - If this bit is set, no delta is stored (Y is same as previous).
        ///   - If not set, a 2-byte signed delta follows.
        /// </summary>
        YSameOrPositive = 1 << 5
    }

    private ref struct PointDecoder
    {
        private ReadOnlySpan<byte> _data;
        private int _xOffset;
        private int _yOffset;
        private int _absX;
        private int _absY;

        public PointDecoder(ReadOnlySpan<byte> data, int xStart, int yStart)
        {
            _data = data;
            _xOffset = xStart;
            _yOffset = yStart;
            _absX = 0;
            _absY = 0;
        }

        public Point Next(byte flag)
        {
            int dx = 0;
            int dy = 0;

            // Decode X
            if ((flag & (byte)Flags.XShortVector) != 0)
            {
                byte b = _data[_xOffset++];
                dx = (flag & (byte)Flags.XSameOrPositive) != 0 ? b : -b;
            }
            else if ((flag & (byte)Flags.XSameOrPositive) == 0)
            {
                dx = BinaryPrimitives.ReadInt16BigEndian(_data.Slice(_xOffset, 2));
                _xOffset += 2;
            }

            // Decode Y
            if ((flag & (byte)Flags.YShortVector) != 0)
            {
                byte b = _data[_yOffset++];
                dy = (flag & (byte)Flags.YSameOrPositive) != 0 ? b : -b;
            }
            else if ((flag & (byte)Flags.YSameOrPositive) == 0)
            {
                dy = BinaryPrimitives.ReadInt16BigEndian(_data.Slice(_yOffset, 2));
                _yOffset += 2;
            }

            _absX += dx;
            _absY += dy;

            return new Point(_absX, _absY);
        }
    }
}
