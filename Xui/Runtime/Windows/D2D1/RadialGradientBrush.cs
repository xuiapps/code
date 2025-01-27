using System;

namespace Xui.Runtime.Windows;

public static partial class D2D1
{
    public unsafe partial class RadialGradientBrush : Brush
    {
        public static new readonly Guid IID = new Guid("2cd906ac-12e2-11dc-9fed-001143a055f9");

        public readonly void* GradientStopCollectionPtr;

        public RadialGradientBrush(void* ptr, void* gradientStopCollectionPtr) : base(ptr)
        {
            this.GradientStopCollectionPtr = gradientStopCollectionPtr;
        }
    }
}