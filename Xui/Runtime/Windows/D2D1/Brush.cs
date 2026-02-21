using System;

namespace Xui.Runtime.Windows;

public static partial class D2D1
{
    public unsafe partial class Brush : Resource
    {
        public static new readonly Guid IID = new Guid("2cd906a8-12e2-11dc-9fed-001143a055f9");

        public Brush(void* ptr) : base(ptr)
        {
        }

        public static void SetOpacity(void* brush, float opacity)
            => ((delegate* unmanaged<void*, float, void>)(*(*(void***)brush + 4)))(brush, opacity);

        public static float GetOpacity(void* brush)
            => ((delegate* unmanaged<void*, float>)(*(*(void***)brush + 5)))(brush);
    }
}