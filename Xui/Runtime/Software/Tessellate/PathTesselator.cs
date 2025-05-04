using System.Collections.Generic;
using Xui.Core.Canvas;

namespace Xui.Runtime.Software.Tessellate;

/// <summary>
/// Represents a full tessellation pipeline result, including intermediate geometric representations.
/// Used for visual debugging, documentation, and rendering filled polygons from vector paths.
/// </summary>
public partial class PathTesselator
{
    public PathTesselator(IReadOnlyList<ClosedContour> contours, List<Polygon> polygons)
    {
        Contours = contours;
        Polygons = polygons;
    }

    /// <summary>
    /// Gets the list of closed contours reconstructed from the segments.
    /// Each contour represents a loop used in fill evaluation.
    /// </summary>
    public IReadOnlyList<ClosedContour> Contours { get; }

    /// <summary>
    /// Gets the final list of filled triangle polygons that represent the tessellated shape.
    /// </summary>
    public List<Polygon> Polygons { get; } = new();

    /// <summary>
    /// Performs full tessellation of a vector path into filled triangle polygons using a specified fill rule.
    /// Intermediate data including segments, contours, and trapezoids is retained for debugging or visualization.
    /// </summary>
    /// <param name="path">The vector path to tessellate.</param>
    /// <param name="fillRule">The fill rule (EvenOdd or NonZero) to apply during sweep.</param>
    /// <param name="flatness">The curve flattening tolerance. Smaller values yield more segments.</param>
    /// <returns>A <see cref="PathTesselator"/> instance containing all stages of the tessellation pipeline.</returns>
    public static PathTesselator Fill(Path2D path, FillRule fillRule, nfloat flatness)
    {
        var contours = Contouring.ContourPath(path, flatness);
        var polygons = ScanlineSweep.Fill(contours, fillRule);
        return new PathTesselator(contours, polygons);
    }

    public static PathTesselator Fill(Path2D path, FillRule fillRule) => Fill(path, fillRule, (nfloat)0.25);
}
