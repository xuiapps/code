---
title: Documents
description: Rendering Xui views as offline media and static SVG documents.
---

# Documents

Xui can render the same view tree to an interactive application, an offline media asset, or static web media. `SVGDocument` is the document host for the latter two cases: it has no window or input, runs the normal measure, arrange, and render passes, and supplies a deterministic time of zero by default. This makes it suitable for documentation authoring, generated design assets, and component illustrations.

```csharp
var svg = new SVGDocument
{
    Content = new MyView(),
    Size = (800, 600),
    FontUris = Xui.Core.Fonts.Inter.URIs,
}.ToSVG();
```

Pass the same bundled font URIs used by the application whenever text metrics affect layout. Otherwise the software renderer cannot measure that font deterministically.

## Fonts in standalone SVG

An SVG served through `<img src="...">` is a separate document. `font-family="Inter"` names a font, but does not download it, and the SVG does not inherit the host page's `@font-face` rules. A browser will substitute an installed fallback when Inter is unavailable.

For a standalone SVG, provide its own `@font-face` rule. `SvgDrawingContext.SvgFontResolver` selects the strategy:

- `System` (the default) emits only `font-family`; use it only for fonts guaranteed to be installed.
- `WebLink` emits an SVG-local `@font-face` pointing at a web font. Keep the font same-origin with the SVG, or configure CORS correctly. Do not rely on it when the SVG is loaded through `<img>`: Chromium-based browsers can block nested font loads in that image-document mode.
- `Embedded` writes the font as a data URI in the SVG. It is self-contained and is the reliable choice for SVGs displayed through `<img>`, but makes the SVG substantially larger.

The generated metrics figure embeds Inter so it renders reliably when displayed as an image. The published [Inter Regular](img/fonts/Inter-Regular.ttf) face and its [license](img/fonts/LICENSE.txt) remain available for documents that deliberately use `WebLink`.

For artifacts that must look identical in every browser, previewer, and image converter, convert text to paths as a final publishing step. That loses selectable text and should not be the default for Xui layout examples.
