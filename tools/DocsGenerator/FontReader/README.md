# Font Reader

**Xui.Runtime.Software.Font** includes a lightweight TrueType font parser and glyph layout engine designed for use in software-rendered text pipelines.

This example demonstrates how to:

* Load a `.ttf` font from an embedded resource.
* Parse and layout text using `TextLayout`.
* Extract glyph outlines and render them as SVG paths.

---

### 🧪 Example Output

The following SVG was generated from a `TextLayout` using the `Inter-Regular.ttf` font:

![Hello World Glyph Outlines](./hello-world-outlines.svg)

---

### 🧱 Load Font and Generate Glyph Paths

The following utility method shows how a font can be loaded and used to compute layout and extract glyph paths programmatically:

```csharp
/// <summary>
/// Loads a TrueTypeFont from memory, lays out the given text at the specified size,
/// and feeds glyph outlines into the given IPathBuilder.
/// </summary>
public static void LoadFontAndGenerateTextLayoutGlyphsPath(ReadOnlyMemory<byte> fontData, string text, float fontSize, IPathBuilder pathBuilder)
{
    var font = new TrueTypeFont(fontData);
    var layout = new TextLayout(font, text, fontSize);
    layout.Visit(pathBuilder);
}
```

---

### 🧠 Notes

* The coordinate system is **top-left origin**, matching typical 2D canvas systems.
* `TextLayout` automatically positions glyphs with proper advances and kerning.
* You can extract individual glyph paths, bounding boxes, and metrics for more advanced rendering needs.

---

### 🛠️ Next Steps

You can extend this by:

* Drawing glyph bounding boxes and baselines.
* Visualizing font metrics (`ascender`, `descender`, etc.).
* Exporting multiple weights or font families for comparison.
