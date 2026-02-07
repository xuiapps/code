using System;
using System.Runtime.InteropServices;
using static Xui.Runtime.Windows.COM;
using static Xui.Runtime.Windows.Win32.Types;

namespace Xui.Runtime.Windows;

public static partial class D2D1
{
    public unsafe partial class Factory : Unknown
    {
        public static new readonly Guid IID = new Guid("06152247-6F50-465A-9245-118BFD3B6007");

        [LibraryImport(D2D1Lib)]
        private static partial HRESULT D2D1CreateFactory(FactoryType factoryType, in Guid factoryRIID, in FactoryOptions fo, out void* ppIFactory);

        protected static void* CreateFactory(in Guid iid)
        {
            FactoryOptions fo = new FactoryOptions(DebugLevel.None);
            Marshal.ThrowExceptionForHR(D2D1CreateFactory(FactoryType.SingleThreaded, in iid, in fo, out var ppIFactory));
            return ppIFactory;
        }

        public Factory() : base(CreateFactory(IID))
        {
        }

        public Factory(void* ptr) : base(ptr)
        {
        }

        public void GetDesktopDpi(out float dpiX, out float dpiY)
        {
            float x;
            float y;
            ((delegate* unmanaged[MemberFunction]<void*, float*, float*, void>)this[4])(this, &x, &y);
            dpiX = x;
            dpiY = y;
        }

        public PathGeometry CreatePathGeometry()
        {
            void* pathGeometry;
            Marshal.ThrowExceptionForHR(((delegate* unmanaged[MemberFunction]<void*, void**, int>)this[10])(this, &pathGeometry));
            return new PathGeometry(pathGeometry);
        }

        public PathGeometry.Ptr CreatePathGeometryPtr()
        {
            void* pathGeometry;
            Marshal.ThrowExceptionForHR(((delegate* unmanaged[MemberFunction]<void*, void**, int>)this[10])(this, &pathGeometry));
            return new PathGeometry.Ptr(pathGeometry);
        }

        public StrokeStyle CreateStrokeStyle(in StrokeStyleProperties strokeStyleProperties, ReadOnlySpan<float> dashes)
        {
            void* strokeStyle;
            fixed(StrokeStyleProperties* strokeStylePropertiesPtr = &strokeStyleProperties)
            fixed(float* dashesPtr = dashes)
            {
                Marshal.ThrowExceptionForHR(((delegate* unmanaged[MemberFunction]<void*, StrokeStyleProperties*, float*, uint, void**, int>)this[11])(this, strokeStylePropertiesPtr, dashesPtr, (uint)dashes.Length, &strokeStyle));
            }
            return new StrokeStyle(strokeStyle);
        }

        public StrokeStyle.Ptr CreateStrokeStylePtr(in StrokeStyleProperties strokeStyleProperties, ReadOnlySpan<float> dashes)
        {
            void* strokeStyle;
            fixed(StrokeStyleProperties* strokeStylePropertiesPtr = &strokeStyleProperties)
            fixed(float* dashesPtr = dashes)
            {
                Marshal.ThrowExceptionForHR(((delegate* unmanaged[MemberFunction]<void*, StrokeStyleProperties*, float*, uint, void**, int>)this[11])(this, strokeStylePropertiesPtr, dashesPtr, (uint)dashes.Length, &strokeStyle));
            }
            return new StrokeStyle.Ptr(strokeStyle);
        }

        public DrawingStateBlock CreateDrawingStateBlock()
        {
            void* drawingStateBlock;
            Marshal.ThrowExceptionForHR(((delegate* unmanaged[MemberFunction]<void*, void*, void*, void**, int>)this[12])(this, null, null, &drawingStateBlock));
            return new DrawingStateBlock(drawingStateBlock);
        }

        public DrawingStateBlock.Ptr CreateDrawingStateBlockPtr()
        {
            void* drawingStateBlock;
            Marshal.ThrowExceptionForHR(((delegate* unmanaged[MemberFunction]<void*, void*, void*, void**, int>)this[12])(this, null, null, &drawingStateBlock));
            return new DrawingStateBlock.Ptr(drawingStateBlock);
        }
    }
}