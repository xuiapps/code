namespace Xui.Core.Canvas;

/// <summary>
/// Provides methods to save and restore the current drawing state,
/// including styles, transforms, and clipping paths.
/// Mirrors the behavior of the HTML5 Canvas <c>save()</c> and <c>restore()</c> methods.
/// </summary>
public interface IStateContext
{
    /// <summary>
    /// Pushes the current drawing state onto the state stack.
    /// This includes styles, transformations, clipping paths, etc.
    /// </summary>
    void Save();

    /// <summary>
    /// Pops the top state off the state stack and restores it.
    /// Any modifications made since the last <see cref="Save"/> are discarded.
    /// </summary>
    void Restore();
}
