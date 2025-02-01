using System;

namespace Xui.Runtime.Windows;

public static partial class D2D1
{
    public unsafe class GeometrySink : SimplifiedGeometrySink
    {
        public static new readonly Guid IID = new Guid("2cd9069f-12e2-11dc-9fed-001143a055f9");

        public GeometrySink(void* ptr) : base(ptr)
        {
        }

        public void AddLine(Point2F point) =>
            ((delegate* unmanaged[MemberFunction]<void*, Point2F, void>)this[10])(this, point);

        public void AddBezier(BezierSegment bezierSegment) =>
            ((delegate* unmanaged[MemberFunction]<void*, BezierSegment, void>)this[11])(this, bezierSegment);
        
        public void AddQuadraticBezier(QuadraticBezierSegment quadraticBezierSegment) =>
            ((delegate* unmanaged[MemberFunction]<void*, QuadraticBezierSegment, void>)this[12])(this, quadraticBezierSegment);
        
        public void AddQuadraticBeziers(ReadOnlySpan<QuadraticBezierSegment> quadraticBezierSegments)
        {
            uint count = (uint)quadraticBezierSegments.Length;
            fixed(QuadraticBezierSegment* quadraticBezierSegmentsPtr = quadraticBezierSegments)
            {
                ((delegate* unmanaged[MemberFunction]<void*, QuadraticBezierSegment*, uint, void>)this[13])(this, quadraticBezierSegmentsPtr, count);
            }
        }

        public void AddArc(in ArcSegment arcSegment)
        {
            fixed(ArcSegment* arcSegmentPtr = &arcSegment)
            {
                ((delegate* unmanaged[MemberFunction]<void*, ArcSegment*, void>)this[14])(this, arcSegmentPtr);
            }
        }
    }
}