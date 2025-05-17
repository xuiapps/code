namespace Xui.Core.Canvas;

public ref partial struct Font
{
    /// <summary>
    /// Implicitly creates a <see cref="Font"/> from a font size.
    /// Uses normal weight, normal style, normal stretch, and line height = 1.2× size.
    /// </summary>
    /// <param name="size">The font size in user-space units (e.g., pixels).</param>
    public static implicit operator Font(float size) =>
        new()
        {
            FontSize = size,
            FontWeight = FontWeight.Normal,
            FontStyle = FontStyle.Normal,
            FontStretch = FontStretch.Normal,
            LineHeight = size * 1.2f
        };

    /// <summary>
    /// Implicitly creates a <see cref="Font"/> from a font size and weight.
    /// Uses normal style, normal stretch, and line height = 1.2× size.
    /// </summary>
    /// <param name="tuple">The font size and font weight.</param>
    public static implicit operator Font((float size, FontWeight weight) tuple) =>
        new()
        {
            FontSize = tuple.size,
            FontWeight = tuple.weight,
            FontStyle = FontStyle.Normal,
            FontStretch = FontStretch.Normal,
            LineHeight = tuple.size * 1.2f
        };

    /// <summary>
    /// Implicitly creates a <see cref="Font"/> from a font size and style.
    /// Uses normal weight, normal stretch, and line height = 1.2× size.
    /// </summary>
    /// <param name="tuple">The font size and font style.</param>
    public static implicit operator Font((float size, FontStyle style) tuple) =>
        new()
        {
            FontSize = tuple.size,
            FontWeight = FontWeight.Normal,
            FontStyle = tuple.style,
            FontStretch = FontStretch.Normal,
            LineHeight = tuple.size * 1.2f
        };

    /// <summary>
    /// Implicitly creates a <see cref="Font"/> from size, weight, and style.
    /// Uses normal stretch and line height = 1.2× size.
    /// </summary>
    /// <param name="tuple">The font size, weight, and style.</param>
    public static implicit operator Font((float size, FontWeight weight, FontStyle style) tuple) =>
        new()
        {
            FontSize = tuple.size,
            FontWeight = tuple.weight,
            FontStyle = tuple.style,
            FontStretch = FontStretch.Normal,
            LineHeight = tuple.size * 1.2f
        };

    /// <summary>
    /// Implicitly creates a <see cref="Font"/> from size, weight, and stretch.
    /// Uses normal style and line height = 1.2× size.
    /// </summary>
    /// <param name="tuple">The font size, weight, and stretch percentage.</param>
    public static implicit operator Font((float size, FontWeight weight, FontStretch stretch) tuple) =>
        new()
        {
            FontSize = tuple.size,
            FontWeight = tuple.weight,
            FontStyle = FontStyle.Normal,
            FontStretch = tuple.stretch,
            LineHeight = tuple.size * 1.2f
        };

    /// <summary>
    /// Implicitly creates a <see cref="Font"/> from size, style, and stretch.
    /// Uses normal weight and line height = 1.2× size.
    /// </summary>
    /// <param name="tuple">The font size, style, and stretch percentage.</param>
    public static implicit operator Font((float size, FontStyle style, FontStretch stretch) tuple) =>
        new()
        {
            FontSize = tuple.size,
            FontWeight = FontWeight.Normal,
            FontStyle = tuple.style,
            FontStretch = tuple.stretch,
            LineHeight = tuple.size * 1.2f
        };

    /// <summary>
    /// Implicitly creates a <see cref="Font"/> from size, weight, style, and stretch.
    /// Uses line height = 1.2× size.
    /// </summary>
    /// <param name="tuple">The font size, weight, style, and stretch percentage.</param>
    public static implicit operator Font((float size, FontWeight weight, FontStyle style, FontStretch stretch) tuple) =>
        new()
        {
            FontSize = tuple.size,
            FontWeight = tuple.weight,
            FontStyle = tuple.style,
            FontStretch = tuple.stretch,
            LineHeight = tuple.size * 1.2f
        };

    /// <summary>
    /// Creates a <see cref="Font"/> from size and line height.
    /// </summary>
    public static implicit operator Font((float size, float lineHeight) tuple) =>
        new()
        {
            FontSize = tuple.size,
            FontWeight = FontWeight.Normal,
            FontStyle = FontStyle.Normal,
            FontStretch = FontStretch.Normal,
            LineHeight = tuple.lineHeight
        };

    /// <summary>
    /// Creates a <see cref="Font"/> from size, weight, and line height.
    /// </summary>
    public static implicit operator Font((float size, FontWeight weight, float lineHeight) tuple) =>
        new()
        {
            FontSize = tuple.size,
            FontWeight = tuple.weight,
            FontStyle = FontStyle.Normal,
            FontStretch = FontStretch.Normal,
            LineHeight = tuple.lineHeight
        };

    /// <summary>
    /// Creates a <see cref="Font"/> from size, style, and line height.
    /// </summary>
    public static implicit operator Font((float size, FontStyle style, float lineHeight) tuple) =>
        new()
        {
            FontSize = tuple.size,
            FontWeight = FontWeight.Normal,
            FontStyle = tuple.style,
            FontStretch = FontStretch.Normal,
            LineHeight = tuple.lineHeight
        };

    /// <summary>
    /// Creates a <see cref="Font"/> from size, weight, style, and line height.
    /// </summary>
    public static implicit operator Font((float size, FontWeight weight, FontStyle style, float lineHeight) tuple) =>
        new()
        {
            FontSize = tuple.size,
            FontWeight = tuple.weight,
            FontStyle = tuple.style,
            FontStretch = FontStretch.Normal,
            LineHeight = tuple.lineHeight
        };

    /// <summary>
    /// Creates a <see cref="Font"/> from size, weight, stretch, and line height.
    /// </summary>
    public static implicit operator Font((float size, FontWeight weight, FontStretch stretch, float lineHeight) tuple) =>
        new()
        {
            FontSize = tuple.size,
            FontWeight = tuple.weight,
            FontStyle = FontStyle.Normal,
            FontStretch = tuple.stretch,
            LineHeight = tuple.lineHeight
        };

    /// <summary>
    /// Creates a <see cref="Font"/> from size, style, stretch, and line height.
    /// </summary>
    public static implicit operator Font((float size, FontStyle style, FontStretch stretch, float lineHeight) tuple) =>
        new()
        {
            FontSize = tuple.size,
            FontWeight = FontWeight.Normal,
            FontStyle = tuple.style,
            FontStretch = tuple.stretch,
            LineHeight = tuple.lineHeight
        };

    /// <summary>
    /// Creates a <see cref="Font"/> from size, weight, style, stretch, and line height.
    /// </summary>
    public static implicit operator Font((float size, FontWeight weight, FontStyle style, FontStretch stretch, float lineHeight) tuple) =>
        new()
        {
            FontSize = tuple.size,
            FontWeight = tuple.weight,
            FontStyle = tuple.style,
            FontStretch = tuple.stretch,
            LineHeight = tuple.lineHeight
        };
}
