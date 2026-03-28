---
title: Unit Tests
description: Test a single view's layout and render output using SvgDrawingContext — no GPU or window required.
---

# Unit Tests

Unit tests verify that a single view measures, arranges, and renders correctly. They use `SvgDrawingContext` as a software renderer that emits SVG, so no GPU, window manager, or display is needed.

## Project setup

Add references to `Xui.Core`, `Xui.Runtime.Software`, and optionally `Xui.Core.Fonts` for text measurement:

```xml
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <TargetFramework>net10.0</TargetFramework>
  </PropertyGroup>

  <ItemGroup>
    <PackageReference Include="xunit" Version="2.*" />
    <PackageReference Include="Microsoft.NET.Test.Sdk" Version="17.*" />
    <ProjectReference Include="../../Core/Core/Xui.Core.csproj" />
    <ProjectReference Include="../../Core/Fonts/Xui.Core.Fonts.csproj" />
    <ProjectReference Include="../../Runtime/Software/Xui.Runtime.Software.csproj" />
  </ItemGroup>
</Project>
```

## Rendering a view to SVG

The core helper instantiates a view, runs the full layout pipeline (measure + arrange + render), and returns the SVG string:

```csharp
using Xui.Core.Math2D;
using Xui.Core.UI;
using Xui.Runtime.Software.Actual;

static string RenderToSvg(View view, Size size)
{
    using var stream = new MemoryStream();
    using (var context = new SvgDrawingContext(size, stream,
        Xui.Core.Fonts.Inter.URIs, keepOpen: true))
    {
        view.Update(new LayoutGuide
        {
            Pass          = LayoutGuide.LayoutPass.Measure
                          | LayoutGuide.LayoutPass.Arrange
                          | LayoutGuide.LayoutPass.Render,
            AvailableSize = size,
            Anchor        = (0, 0),
            XSize         = LayoutGuide.SizeTo.Exact,
            YSize         = LayoutGuide.SizeTo.Exact,
            XAlign        = LayoutGuide.Align.Start,
            YAlign        = LayoutGuide.Align.Start,
            MeasureContext = context,
            RenderContext  = context,
        });
    }

    stream.Position = 0;
    return new StreamReader(stream).ReadToEnd();
}
```

`keepOpen: true` keeps the underlying stream open so you can read it after the `SvgDrawingContext` is disposed (dispose writes the closing `</svg>` tag).

## Writing a test

```csharp
[Fact]
public void ProgressBar_Renders_At_60_Percent()
{
    var view = new ProgressBar { Value = 0.6 };
    var svg  = RenderToSvg(view, new Size(320, 24));

    // Assert structural properties of the SVG, or compare to a baseline.
    Assert.Contains("width=\"192\"", svg); // 60% of 320
}
```

Unit tests are ideal for:

- Verifying layout math (a view measures to the correct size)
- Checking that render output contains expected drawing primitives
- Testing edge cases (empty content, zero size, extreme values)

## CallerFilePath for output paths

When writing generated SVGs next to the test source file, use `[CallerFilePath]` to resolve the path without hard-coding:

```csharp
private static string GetSnapshotPath(
    string fileName,
    [CallerFilePath] string callerPath = "")
{
    var dir = Path.GetDirectoryName(callerPath)!;
    return Path.Combine(dir, "Snapshots", fileName);
}
```
