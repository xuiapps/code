using System;

namespace Xui.Runtime.Windows;

public static partial class D2D1
{
    public unsafe partial class LinearGradientBrush : Brush
    {
        public static new readonly Guid IID = new Guid("2cd906ab-12e2-11dc-9fed-001143a055f9");

        public readonly void* GradientStopCollectionPtr;

        public LinearGradientBrush(void* ptr, void* gradientStopCollectionPtr) : base(ptr)
        {
            this.GradientStopCollectionPtr = gradientStopCollectionPtr;
        }
    }
}