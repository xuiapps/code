using System;

namespace Xui.Runtime.Windows;

public static partial class DWrite
{
    public unsafe partial class TextFormat : COM.Unknown
    {
        public struct Ptr : IDisposable
        {
            private void* ptr;

            public Ptr(void* ptr)
            {
                this.ptr = ptr;
            }

            public static implicit operator void*(Ptr p) => p.ptr;

            public bool IsNull => ptr == null;

            public void Dispose()
            {
                if (ptr != null)
                {
                    COM.Unknown.Release(ptr);
                    ptr = null;
                }
            }
        }
    }
}
