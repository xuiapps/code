namespace Xui.Runtime.Windows;

public static partial class D2D1
{
    public unsafe struct LayerParameters
    {
        public RectF ContentRect;
        public void* GeometricMask;
        public AntialiasMode MaskAntialiasMode;
        public Matrix3X2F MaskTransform;
        public float Opacity;
        public void* OpacityBrush;
        public LayerOptions LayerOptions;

        public LayerParameters()
        {
            this.ContentRect = RectF.Inifinity;
            this.GeometricMask = null;
            this.MaskTransform = Matrix3X2F.Identity;
            this.MaskAntialiasMode = AntialiasMode.PerPrimitive;
            this.Opacity = 1;
            this.OpacityBrush = null;
            this.LayerOptions = LayerOptions.None;
        }
    }
}