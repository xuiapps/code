using System;
using System.Runtime.InteropServices;

namespace Xui.Runtime.Windows;

public static partial class DWrite
{
    public unsafe partial class TextLayout : TextFormat
    {
        private static new readonly Guid IID = new Guid("53737037-6d14-410b-9bfe-0b182bb70961");

        public TextLayout(void* ptr) : base(ptr)
        {
        }

        public TextMetrics GetTextMetrics()
        {
            TextMetrics textMetrics;
            Marshal.ThrowExceptionForHR(((delegate* unmanaged[MemberFunction]<void*, TextMetrics*, int>)this[60])(this, &textMetrics));
            return textMetrics;
        }

        public LineMetrics[] GetLineMetrics()
        {
            uint actual = this.GetLineMetricsLinesCount();

            if (actual == 0)
            {
                return Array.Empty<LineMetrics>();
            }

            var metrics = new LineMetrics[actual];

            fixed (LineMetrics* p = metrics)
            {
                uint written = 0;

                // Second call: fill buffer
                GetLineMetrics(p, actual, &written);

                if (written == actual)
                {
                    return metrics;
                }

                if (written == 0)
                {
                    return Array.Empty<LineMetrics>();
                }

                var trimmed = new LineMetrics[written];
                Array.Copy(metrics, trimmed, (int)written);
                return trimmed;
            }
        }

        private uint GetLineMetricsLinesCount()
        {
            uint actualLineCount;
            ((delegate* unmanaged[MemberFunction]<void*, LineMetrics*, uint, uint*, int>)this[59])(
                    this,
                    null,
                    0,
                    &actualLineCount);
            return actualLineCount;
        }

        private void GetLineMetrics(LineMetrics* metrics, uint maxLineCount, uint* actualLineCount) =>
            Marshal.ThrowExceptionForHR(((delegate* unmanaged[MemberFunction]<void*, LineMetrics*, uint, uint*, int>)this[59])(
                    this,
                    metrics,
                    maxLineCount,
                    actualLineCount));
    }
}