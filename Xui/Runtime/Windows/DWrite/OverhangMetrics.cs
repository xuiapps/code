using System.Runtime.InteropServices;

namespace Xui.Runtime.Windows
{
    public static partial class DWrite
    {
        // Matches: typedef struct DWRITE_OVERHANG_METRICS { FLOAT left, top, right, bottom; } ...
        // Size = 16 bytes (4 floats).
        [StructLayout(LayoutKind.Explicit, Size = 16)]
        public struct OverhangMetrics
        {
            [FieldOffset(0)]
            public float Left;

            [FieldOffset(4)]
            public float Top;

            [FieldOffset(8)]
            public float Right;

            [FieldOffset(12)]
            public float Bottom;
        }
    }
}
