namespace Xui.Core.Canvas;

/// <summary>
/// Defines methods for drawing a constructed path,
/// following the HTML5 Canvas path API model.
///
/// Reference: https://developer.mozilla.org/en-US/docs/Web/API/CanvasRenderingContext2D#paths
/// </summary>
public interface IPathDrawing
{
    /// <summary>
    /// Fills the current path using the specified fill rule.
    /// </summary>
    /// <param name="rule">The fill rule to use (NonZero or EvenOdd).</param>
    void Fill(FillRule rule = FillRule.NonZero);

    /// <summary>
    /// Strokes the current path using the current stroke style.
    /// </summary>
    void Stroke();
}
