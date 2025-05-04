using System.Collections.Generic;
using Xui.Core.Math2D;

namespace Xui.Runtime.Software.Tessellate;

public partial class PathTesselator
{
    /// <summary>
    /// Represents a closed contour — a sequence of connected points forming a polygon loop.
    /// </summary>
    public readonly struct ClosedContour
    {
        /// <summary>
        /// Ordered list of points forming the closed contour path.
        /// </summary>
        public readonly IReadOnlyList<Point> Points;

        /// <summary>
        /// Initializes a new closed contour from a sequence of points.
        /// </summary>
        public ClosedContour(IReadOnlyList<Point> points)
        {
            Points = points;
        }

        /// <summary>
        /// Returns the number of points in this contour.
        /// </summary>
        public int Count => Points.Count;

        /// <summary>
        /// Indexer access to individual points.
        /// </summary>
        public Point this[int index] => Points[index];

        public enum WindingDirection { Clockwise, CounterClockwise }

        public WindingDirection Winding
        {
            get
            {
                nfloat sum = 0;
                for (int i = 0; i < Count; i++)
                {
                    var a = Points[i];
                    var b = Points[(i + 1) % Count];
                    sum += (b.X - a.X) * (b.Y + a.Y);
                }
                return sum > 0 ? WindingDirection.Clockwise : WindingDirection.CounterClockwise;
            }
        }

        public Rect Bounds
        {
            get
            {
                nfloat minX = Points[0].X, maxX = Points[0].X;
                nfloat minY = Points[0].Y, maxY = Points[0].Y;
                for (int i = 1; i < Count; i++)
                {
                    var p = Points[i];
                    if (p.X < minX) minX = p.X;
                    if (p.X > maxX) maxX = p.X;
                    if (p.Y < minY) minY = p.Y;
                    if (p.Y > maxY) maxY = p.Y;
                }
                return new Rect(minX, minY, maxX - minX, maxY - minY);
            }
        }

        public nfloat SignedArea
        {
            get
            {
                nfloat area = 0;
                for (int i = 0; i < Count; i++)
                {
                    var a = Points[i];
                    var b = Points[(i + 1) % Count];
                    area += (a.X * b.Y - b.X * a.Y);
                }
                return area / 2;
            }
        }
    }
}
