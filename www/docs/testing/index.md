---
title: Testing
description: Overview of the four testing levels available for Xui applications.
---

# Testing

Xui provides testing infrastructure at four levels. Each level trades speed for realism — pick the one that matches what you need to verify.

| Level | What it tests | Runtime needed | GPU/Window | Speed |
|---|---|---|---|---|
| [Unit](unit.md) | A single view's layout and render output | `Xui.Runtime.Software` | No | ~ms |
| [Component](component.md) | View snapshots with diff comparison | `Xui.Runtime.Software` | No | ~ms |
| [Integration](integration.md) | Full app navigation, input, and snapshot sequences | `Xui.Runtime.Test` | No | ~ms |
| [End-to-End](e2e.md) | Real OS process with platform rendering and DevTools | `Xui.Runtime.E2E` | Yes | ~seconds |

All four levels use [xUnit](https://xunit.net/) and run with `dotnet test`. Unit, component, and integration tests need no display server — they render to SVG in-process using `SvgDrawingContext`. E2E tests launch a real desktop window and connect over a named pipe.

## Which level to use

**Unit tests** are best for verifying that a custom view produces the correct visual output for given inputs. They test one view in isolation with no app scaffolding.

**Component tests** add snapshot comparison on top of unit rendering. Commit the `.svg` baselines and any visual regression shows up as a text diff in pull requests.

**Integration tests** boot your full `Application` and `Window` via a synchronous test harness. They can send mouse, keyboard, and touch events, navigate between pages, and snapshot each step — all without an OS event loop or display.

**End-to-end tests** launch your app as a real process, wait for its window to appear, then drive it through the DevTools protocol. They verify platform-specific behaviour that the software renderer cannot replicate: actual font rendering, OS input dispatch, window lifecycle. Use them sparingly for critical user flows.
