using System;

namespace Xui.Runtime.Windows;

public static partial class D2D1
{
    public unsafe partial class GeometrySink : SimplifiedGeometrySink
    {
        public struct Ptr : IDisposable
        {
            private void* ptr;

            public Ptr(void* ptr)
            {
                this.ptr = ptr;
            }

            private void* this[uint slot] => *(*(void***)this.ptr + slot);

            public static implicit operator void*(Ptr p) => p.ptr;

            public bool IsNull => ptr == null;

            // SimplifiedGeometrySink methods (slots 3-9)
            public void SetFillMode(FillMode fillMode) =>
                ((delegate* unmanaged[MemberFunction]<void*, FillMode, void>)this[3])(this.ptr, fillMode);

            public void BeginFigure(Point2F startPoint, FigureBegin figureBegin = FigureBegin.Filled) =>
                ((delegate* unmanaged[MemberFunction]<void*, Point2F, FigureBegin, void>)this[5])(this.ptr, startPoint, figureBegin);

            public void EndFigure(FigureEnd figureEnd = FigureEnd.Closed) =>
                ((delegate* unmanaged[MemberFunction]<void*, FigureEnd, void>)this[8])(this.ptr, figureEnd);

            public void Close() =>
                System.Runtime.InteropServices.Marshal.ThrowExceptionForHR(
                    ((delegate* unmanaged[MemberFunction]<void*, int>)this[9])(this.ptr));

            // GeometrySink methods (slots 10-14)
            public void AddLine(Point2F point) =>
                ((delegate* unmanaged[MemberFunction]<void*, Point2F, void>)this[10])(this.ptr, point);

            public void AddBezier(BezierSegment bezierSegment) =>
                ((delegate* unmanaged[MemberFunction]<void*, BezierSegment, void>)this[11])(this.ptr, bezierSegment);

            public void AddQuadraticBezier(QuadraticBezierSegment quadraticBezierSegment) =>
                ((delegate* unmanaged[MemberFunction]<void*, QuadraticBezierSegment, void>)this[12])(this.ptr, quadraticBezierSegment);

            public void AddArc(in ArcSegment arcSegment)
            {
                fixed (ArcSegment* arcSegmentPtr = &arcSegment)
                {
                    ((delegate* unmanaged[MemberFunction]<void*, ArcSegment*, void>)this[14])(this.ptr, arcSegmentPtr);
                }
            }

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
