---
title: End-to-End Tests
description: Launch a real Xui desktop app and drive it through the DevTools protocol.
---

# End-to-End Tests

E2E tests launch your Xui app as a real OS process, wait for its window to appear, then drive it through the DevTools named-pipe protocol. They test the full stack — platform rendering, OS input dispatch, window lifecycle — on real hardware.

## Project setup

Reference `Xui.Runtime.E2E`:

```xml
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <TargetFramework>net10.0</TargetFramework>
  </PropertyGroup>

  <ItemGroup>
    <PackageReference Include="xunit" Version="2.*" />
    <PackageReference Include="Microsoft.NET.Test.Sdk" Version="17.*" />
    <ProjectReference Include="../../Runtime/E2E/Xui.Runtime.E2E.csproj" />
  </ItemGroup>
</Project>
```

The test project does **not** reference the app project directly — it launches the app as a subprocess via `dotnet run`.

## RealApp

`RealApp` is the main harness. It starts your app, connects to DevTools, and provides async methods to inspect and interact with it:

```csharp
await using var app = await RealApp.StartAsync(
    "Path/To/YourApp.Desktop.csproj",
    workingDirectory: solutionRoot);

var root = await app.WaitForWindowAsync();
```

`StartAsync` runs `dotnet run --project <path> -c Debug`, waits for the app to print `DEVTOOLS_READY:<pipe-name>` to stdout, then connects to the named pipe. If the app crashes or times out, the exception includes the full stdout/stderr log.

### Methods

| Method | Description |
|---|---|
| `WaitForWindowAsync(timeout?)` | Polls until the window is ready. Throws if the app crashes. |
| `InspectAsync()` | Returns the current UI tree as a `ViewNode` hierarchy |
| `ScreenshotAsync()` | Returns an SVG screenshot of the app window |
| `ClickAsync(x, y)` | Sends a mouse click (down + up) at the given coordinates |
| `TapAsync(x, y)` | Sends a touch tap at the given coordinates |
| `PointerAsync(phase, x, y)` | Sends a pointer event (`"start"`, `"move"`, `"end"`, `"cancel"`) |
| `InvalidateAsync()` | Forces an immediate redraw |
| `IdentifyAsync(label)` | Sets the AI pointer overlay label |
| `GetLog()` | Returns captured stdout/stderr output from the app |
| `HasCrashed` | Returns `true` if the app process has exited unexpectedly |

### ViewNode

`InspectAsync()` returns a `ViewNode` tree that mirrors the live view hierarchy:

```csharp
var root = await app.InspectAsync();

// Find a view by Id
var button = root!.FindById("Settings");

// Find all views of a type
var labels = root!.FindAllByType("Label");

// Use coordinates for interaction
await app.ClickAsync(button!.CenterX, button!.CenterY);
```

Each `ViewNode` has: `Type`, `X`, `Y`, `W`, `H`, `CenterX`, `CenterY`, `Visible`, `Id`, `ClassName`, and `Children`.

## Error handling

E2E tests are strict — no silent failures:

- Every `RealApp` method checks `ThrowIfFaulted()` before communicating with the app. If the process has crashed or an I/O drain has faulted, the test fails immediately with the full app log.
- `WaitForWindowAsync` fails fast if the process exits instead of polling to timeout.
- Timeouts include the captured stdout/stderr so you can see build errors or crash output.

## TestLog — diagnostic markdown

`TestLog` records every DevTools operation and writes a human-readable (and AI-readable) markdown file at the end of the test:

```csharp
await using var log = new TestLog(nameof(MyTestMethod));
await using var app = await RealApp.StartAsync(
    projectPath,
    workingDirectory: solutionRoot,
    testLog: log);
```

This writes `TestResults/MyTestMethod.md` next to the test file, containing:

- Timestamped sections for every DevTools command
- The full UI tree as XML after each `InspectAsync()` call
- SVG screenshots rendered inline
- Click/tap coordinates
- The app's stdout/stderr log

This makes test failures easy to diagnose — read the `.md` to see exactly what the app looked like at each step.

## Example

```csharp
public class NavigationTest
{
    private const string AppProject = "Path/To/YourApp.Desktop.csproj";

    private static string SolutionRoot([CallerFilePath] string path = "")
        => RealApp.FindSolutionRoot(path);

    [Fact]
    public async Task Can_Navigate_To_Settings()
    {
        await using var log = new TestLog(nameof(Can_Navigate_To_Settings));
        await using var app = await RealApp.StartAsync(
            AppProject,
            workingDirectory: SolutionRoot(),
            testLog: log);

        // Wait for the window and take a screenshot
        var home = await app.WaitForWindowAsync();
        await app.ScreenshotAsync();

        // Find and click the Settings button
        var settings = home.FindById("Settings");
        Assert.NotNull(settings);
        await app.ClickAsync(settings.CenterX, settings.CenterY);

        // Wait for navigation to complete
        await Task.Delay(300);

        // Verify the settings page rendered
        var page = await app.InspectAsync();
        Assert.NotNull(page);
        Assert.NotNull(page.FindById("SettingsPanel"));
        await app.ScreenshotAsync();
    }
}
```

## When to use E2E tests

E2E tests are the slowest (seconds per test) and require a display. Use them for:

- Critical user flows that must work on real hardware
- Platform-specific rendering behaviour
- Verifying DevTools integration itself
- Smoke tests for release validation

For everything else, prefer [integration tests](integration.md) — they cover the same application logic orders of magnitude faster.
