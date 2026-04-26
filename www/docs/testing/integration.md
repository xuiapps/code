---
title: Integration Tests
description: Test full application navigation, input, and multi-step snapshot sequences using TestSinglePageApp.
---

# Integration Tests

Integration tests boot your full `Application` and `Window` through a synchronous test harness. You can send mouse, keyboard, and touch events, navigate between pages, and snapshot at each step — all without an OS event loop, GPU, or display.

## Project setup

Reference `Xui.Runtime.Test` in addition to the standard test packages and your app project:

```xml
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <TargetFramework>net10.0</TargetFramework>
  </PropertyGroup>

  <ItemGroup>
    <PackageReference Include="xunit" Version="2.*" />
    <PackageReference Include="Microsoft.NET.Test.Sdk" Version="17.*" />
    <ProjectReference Include="../YourApp/YourApp.Desktop.csproj" />
    <ProjectReference Include="../../Core/Core/Xui.Core.csproj" />
    <ProjectReference Include="../../Core/Fonts/Xui.Core.Fonts.csproj" />
    <ProjectReference Include="../../Runtime/Software/Xui.Runtime.Software.csproj" />
    <ProjectReference Include="../../Runtime/Test/Xui.Runtime.Test.csproj" />
  </ItemGroup>
</Project>
```

## TestSinglePageApp

`TestSinglePageApp<TApplication, TWindow>` is the main harness. It creates a host with `TestPlatform` as the runtime, starts your application, and exposes synchronous methods to drive it:

```csharp
using var app = new TestSinglePageApp<Application, MainWindow>(
    windowSize: new Size(800, 600));
```

The harness:

- Boots your `Application` and `Window` via the DI host
- Sets the window's `DisplayArea` to the given size
- Provides a software text measure context for accurate hit testing
- Writes snapshot artifacts to a `Snapshots/{testName}/` folder next to the test file

### Input methods

| Method | Description |
|---|---|
| `MouseMove(Point)` | Moves the virtual cursor to a position |
| `MouseMove(View)` | Moves the virtual cursor to a view's center |
| `MouseDown(View)` | Sends a mouse-down at a view's center |
| `MouseUp(View)` | Sends a mouse-up at a view's center |
| `KeyDown(VirtualKey)` | Sends a key-down event |
| `Char(char)` | Sends a character input event |
| `Type(string)` | Sends a `Char` event for each character in the string |

### Rendering and snapshots

| Method | Description |
|---|---|
| `Render()` | Runs a full layout + render pass and returns the SVG |
| `Snapshot(name)` | Renders, saves actual/expected SVGs, and records the comparison |
| `AnimationFrame(prev, next)` | Advances the animation clock to test animated views |

## Example: navigating and snapshotting

```csharp
[Fact]
public void Navigate_To_Settings_And_Back()
{
    using var app = new TestSinglePageApp<MyApp, MyWindow>(new Size(800, 600));

    // Render home page
    app.Render();
    app.Snapshot("HomePage");

    // Click the Settings button
    var settings = app.Window.RootView.FindViewById("Settings");
    Assert.NotNull(settings);
    app.MouseMove(settings);
    app.MouseDown(settings);
    app.MouseUp(settings);
    app.Snapshot("SettingsPage");

    // Navigate back
    var back = app.Window.RootView.FindViewById("Back");
    Assert.NotNull(back);
    app.MouseMove(back);
    app.MouseDown(back);
    app.MouseUp(back);
    app.Snapshot("BackToHome");
}
```

## Snapshot comparison and HTML report

On `Dispose()`, the harness:

1. Generates a `TestRun.html` with a side-by-side wipe-comparison viewer for all snapshots.
2. Asserts that every snapshot matches its `.Expected.svg` baseline.
3. Reports all failures with step numbers and names.

The workflow:

1. **First run** — `.Actual.svg` files are written and copied as `.Expected.svg` baselines. The test fails (no prior baseline).
2. **Review** the `TestRun.html` in a browser. If the renders are correct, commit the `.Expected.svg` files.
3. **Subsequent runs** — actual output is compared to expected. Mismatches fail the test and update the `.Actual.svg` for review.

## Input-driven testing

The mouse cursor position is tracked and injected into snapshot SVGs as a virtual cursor overlay, making it easy to see where the mouse was at each step:

```csharp
app.MouseMove(button);           // cursor moves to button center
app.Snapshot("Hover");           // SVG includes cursor at hover position
app.MouseDown(button);
app.Snapshot("Pressed");         // SVG includes pressed cursor indicator
```

## Animation testing

Drive animation frames manually to test time-dependent views:

```csharp
app.AnimationFrame(TimeSpan.Zero, TimeSpan.Zero);
app.Snapshot("Heart.Rest");

app.AnimationFrame(TimeSpan.Zero, TimeSpan.FromSeconds(0.10));
app.Snapshot("Heart.Peak");
```

## DI configuration

Pass a configuration callback to register additional services:

```csharp
using var app = new TestSinglePageApp<MyApp, MyWindow>(
    new Size(800, 600),
    configure: services =>
    {
        services.AddSingleton<IDataStore>(new MockDataStore());
    });
```
