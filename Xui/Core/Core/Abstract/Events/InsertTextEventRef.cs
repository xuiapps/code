namespace Xui.Core.Abstract.Events;

/// <summary>
/// Represents a platform-level input event requesting insertion of text
/// at the current caret or selection position.
/// </summary>
/// <remarks>
/// This event originates from the <c>Actual</c> window and is forwarded
/// to the <c>Abstract</c> layer for dispatch through the view hierarchy.
/// It is typically triggered by user input such as typing characters,
/// pasting from the clipboard, or text input from IMEs.
/// </remarks>
public ref struct InsertTextEventRef
{
    /// <summary>
    /// The text to be inserted into the current input context.
    /// </summary>
    public readonly string Text;

    /// <summary>
    /// Initializes a new instance of the <see cref="InsertTextEventRef"/> struct
    /// with the specified text to insert.
    /// </summary>
    /// <param name="text">The string of text to be inserted.</param>
    public InsertTextEventRef(string text)
    {
        Text = text;
    }
}
