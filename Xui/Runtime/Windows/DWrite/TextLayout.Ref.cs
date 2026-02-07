using System;
using System.Runtime.InteropServices;

namespace Xui.Runtime.Windows;

public static partial class DWrite
{
    public unsafe partial class TextLayout : TextFormat
    {
        public ref struct Ref : IDisposable
        {
            private void* ptr;

            public Ref(void* ptr)
            {
                this.ptr = ptr;
            }

            private void* this[uint slot] => *(*(void***)this.ptr + slot);

            public static implicit operator void*(Ref r) => r.ptr;

            public TextMetrics GetTextMetrics()
            {
                TextMetrics textMetrics;
                Marshal.ThrowExceptionForHR(((delegate* unmanaged[MemberFunction]<void*, TextMetrics*, int>)this[60])(this.ptr, &textMetrics));
                return textMetrics;
            }

            public OverhangMetrics GetOverhangMetrics()
            {
                OverhangMetrics m;
                Marshal.ThrowExceptionForHR(((delegate* unmanaged[MemberFunction]<void*, OverhangMetrics*, int>)this[61])(this.ptr, &m));
                return m;
            }

            public LineMetrics[] GetLineMetrics()
            {
                uint actualLineCount;
                ((delegate* unmanaged[MemberFunction]<void*, LineMetrics*, uint, uint*, int>)this[59])(
                    this.ptr, null, 0, &actualLineCount);

                if (actualLineCount == 0)
                {
                    return Array.Empty<LineMetrics>();
                }

                var metrics = new LineMetrics[actualLineCount];

                fixed (LineMetrics* p = metrics)
                {
                    uint written = 0;
                    Marshal.ThrowExceptionForHR(((delegate* unmanaged[MemberFunction]<void*, LineMetrics*, uint, uint*, int>)this[59])(
                        this.ptr, p, actualLineCount, &written));

                    if (written == actualLineCount)
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

            public void Dispose()
            {
                if (this.ptr != null)
                {
                    COM.Unknown.Release(this.ptr);
                    this.ptr = null;
                }
            }
        }
    }
}