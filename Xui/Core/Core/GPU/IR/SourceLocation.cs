namespace Xui.GPU.IR;

/// <summary>
/// Represents a location in source code.
/// Used to map IR nodes back to original C# source for error reporting.
/// </summary>
public class SourceLocation
{
    /// <summary>
    /// Gets the source file path.
    /// </summary>
    public string FilePath { get; set; } = string.Empty;

    /// <summary>
    /// Gets the line number (1-based).
    /// </summary>
    public int Line { get; set; }

    /// <summary>
    /// Gets the column number (1-based).
    /// </summary>
    public int Column { get; set; }

    /// <summary>
    /// Gets a formatted string representation of this location.
    /// </summary>
    public string ToDisplayString() => $"{FilePath}({Line},{Column})";
}
