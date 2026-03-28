---
title: Component Tests
description: Snapshot-based visual regression testing for individual views.
---

# Component Tests

Component tests extend unit tests with snapshot comparison. You render a view to SVG, save it as a baseline, and on subsequent runs the test asserts that the output hasn't changed. Any visual regression appears as a text diff in pull requests.

## Project setup

Same as [unit tests](unit.md) — reference `Xui.Core`, `Xui.Runtime.Software`, and your app project if you're testing app views.

## Snapshot pattern

```csharp
[Theory]
[InlineData("MyButton", typeof(MyButton))]
[InlineData("MyCard",   typeof(MyCard))]
public void SnapshotTest(string name, Type viewType)
{
    var view = (View)Activator.CreateInstance(viewType)!;
    var actual = RenderToSvg(view, new Size(600, 400));

    var expectedPath = GetSnapshotPath($"Snapshots/{name}.svg");
    var actualPath   = GetSnapshotPath($"Snapshots/{name}.Actual.svg");

    string expected;
    try { expected = File.ReadAllText(expectedPath); }
    catch { expected = ""; }

    try
    {
        Assert.Equal(expected, actual);
    }
    catch
    {
        // Write actual output for review when the assertion fails
        File.WriteAllText(actualPath, actual);
        throw;
    }
}
```

## Workflow

1. **First run** — no baseline exists. The test fails and writes `{Name}.Actual.svg`.
2. **Review** the `.Actual.svg` file. If it looks correct, rename it to `{Name}.svg` (the baseline).
3. **Commit** the `.svg` baseline alongside your tests.
4. **Subsequent runs** — the test compares output against the committed baseline. Any difference fails the test and writes a new `.Actual.svg` for review.

## What to test

Component tests work best for views that have deterministic visual output:

- Custom controls with specific styling
- Layout containers (stacks, grids) with fixed content
- Views that respond to property changes (e.g. `Value`, `IsSelected`)

Parameterize with `[Theory]` and `[InlineData]` to cover multiple views or states in a single test method.

## Tips

- **Commit baselines to source control.** The `.svg` files are small text files that diff well.
- **`.Actual.svg` files should be gitignored** — they're temporary artifacts for failed test review.
- **Font consistency** — pass `Xui.Core.Fonts.Inter.URIs` to `SvgDrawingContext` so text metrics are identical across machines.
