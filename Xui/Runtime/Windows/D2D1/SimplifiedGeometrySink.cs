using System;
using System.Runtime.InteropServices;
using static Xui.Runtime.Windows.COM;

namespace Xui.Runtime.Windows;

public static partial class D2D1
{
    public unsafe class SimplifiedGeometrySink : Unknown
    {
        public static new readonly Guid IID = new Guid("2cd9069e-12e2-11dc-9fed-001143a055f9");

        public SimplifiedGeometrySink(void* ptr) : base(ptr)
        {
        }

        public void SetFillMode(FillMode fillMode) =>
            ((delegate* unmanaged[MemberFunction]<void*, FillMode, void>)this[3])(this, fillMode);

        public void SetSegmentFlags(PathSegment pathSegment) =>
            ((delegate* unmanaged[MemberFunction]<void*, PathSegment, void>)this[4])(this, pathSegment);

        public void BeginFigure(Point2F startPoint, FigureBegin figureBegin = FigureBegin.Filled) =>
            ((delegate* unmanaged[MemberFunction]<void*, Point2F, FigureBegin, void>)this[5])(this, startPoint, figureBegin);

        public void AddLines(ReadOnlySpan<Point2F> points)
        {
            uint pointsCount = (uint)points.Length;
            fixed(Point2F* pointsPtr = points)
            {
                ((delegate* unmanaged[MemberFunction]<void*, Point2F*, uint, void>)this[6])(this, pointsPtr, pointsCount);
            }
        }

        public void AddBeziers(ReadOnlySpan<BezierSegment> bezierSegments)
        {
            uint bezierSegmentsCount = (uint)bezierSegments.Length;
            fixed(BezierSegment* bezierSegmentsPtr = bezierSegments)
            {
                ((delegate* unmanaged[MemberFunction]<void*, BezierSegment*, uint, void>)this[7])(this, bezierSegmentsPtr, bezierSegmentsCount);
            }
        }

        public void EndFigure(FigureEnd figureEnd = FigureEnd.Closed) =>
            ((delegate* unmanaged[MemberFunction]<void*, FigureEnd, void>)this[8])(this, figureEnd);
        
        public void Close() =>
            Marshal.ThrowExceptionForHR(((delegate* unmanaged[MemberFunction]<void*, int>)this[9])(this));
    }
}