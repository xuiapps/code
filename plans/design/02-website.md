# Design Request — Xui Website

## Overview

Xui needs a marketing and documentation website.  The V1 site is **statically generated**
(docfx + custom templates, already partially built in `/www`).  This document is an RFQ for
the **complete visual design** of that site — from the landing page CTA through the docs
and C# API reference pages.

---

## 1. Site Structure

```
/                       ← Landing page (CTA, features, use cases, comparisons)
/getting-started        ← Step-by-step onboarding (first app in < 5 min)
/docs/                  ← Technical documentation hub
  architecture          ← Abstract/Actual split, service chain, DI
  canvas                ← 2D Canvas API reference
  views/                ← View tree, layout, input, state, lifecycle
  gpu/                  ← Shader and mesh rendering
  testing/              ← Unit, component, integration, E2E
  services              ← Image, text-measure, sensor services
  ...
/api/                   ← Auto-generated C# API reference (docfx)
/blog/                  ← Engineering blog (optional V1)
```

---

## 2. Landing Page

### 2.1 Hero Section

**Goal:** communicate Xui's core proposition in 3 seconds and move developers to
"Get started" or "View source".

Required elements:

- **Headline** — short, punchy.  Example: *"Write once. Render everywhere. At 120 fps."*
- **Subheadline** — one sentence that names the differentiator: canvas-based, AoT, zero-reflection,
  cross-platform from a single C# codebase.
- **Primary CTA** — `Get started →`  (links to `/getting-started`)
- **Secondary CTA** — `View on GitHub`
- **Hero visual** — animated loop showing the Xui Emulator frame (§01-emulator) running a demo
  app, with the rainbow gradient ring glowing.  The emulator switches between iPhone, Android,
  and tablet form factors to show cross-platform reach.

Visual direction for the hero:

- Dark background (`#080810` or similar deep indigo-black).
- The emulator visual is the star — it should feel like a live device in the page.
- Gradient accent language consistent with the emulator ring design (see `01-emulator.md`).
- No stock photography. No clip-art icons. The code and the running app are the visuals.

### 2.2 Feature Pillars

Three to five feature pillars displayed as cards or a horizontal section below the hero.
Each pillar has a title, a one-sentence description, and a small inline visual / code snippet.

| Pillar | Title | One-liner |
|---|---|---|
| Performance | **120 fps, every frame** | Direct2D and CoreGraphics — no runtime reflection, no boxing on the hot path. |
| Testability | **Unit-test your UI** | The entire abstract layer renders to SVG. Test pixel-perfect layouts without a GPU. |
| True cross-platform | **One codebase, every form factor** | Phone, tablet, desktop, browser — compiled AoT with .NET 10. |
| Canvas API | **The web API you already know** | `ctx.BeginPath()`, `ctx.Arc()`, `ctx.Fill()` — if you know HTML5 Canvas, you know Xui. |
| AI-ready | **Ship 80% with AI** | A `plans/` folder with SVGs and markdown is enough for AI to generate a working app. |

### 2.3 Use Cases Section

A scrollable or tabbed section showcasing target verticals.  Each use case has:

- A full-bleed or inset visual (rendered screenshot or short looping video from the demo app)
- A vertical name and one-paragraph description

Verticals to cover:

| Vertical | Short description |
|---|---|
| **Desktop CAD** | Precision drawing tools, hardware-level input, 120 fps pan/zoom |
| **Hardware control** | Operator panels, CNC dashboards, embedded touch screens |
| **Monitor & dashboards** | Real-time telemetry, custom gauges, data visualisations |
| **IoT interfaces** | Coffee machine, vacuum robot, smart scale — bespoke UI for embedded touch |
| **Mobile games & promo apps** | 60/120 fps canvas games, AR-style promo experiences, loyalty and QR code redemption |
| **Health & fitness** | Smart scale readouts, heart-rate monitors, wearable companion apps |
| **3D visualisation** | Car dashboard with live 3D gauge clusters, architectural floor-plan walkthroughs |
| **Travel & mapping** | Custom map overlays, route visualisations, geo-fenced promotions |

### 2.4 Competitor Comparison

A **non-aggressive**, facts-only comparison table.  Frame it as "pick the right tool":

| | Xui | MAUI | Avalonia | Flutter |
|---|---|---|---|---|
| Language | C# | C# | C# | Dart |
| Rendering | Native 2D API | Native controls | Skia | Skia / Impeller |
| AoT support | ✅ Full | Partial | Partial | ✅ Full |
| Zero-reflection hot path | ✅ | ❌ | ❌ | ✅ |
| HTML5 Canvas API | ✅ | ❌ | ❌ | ❌ |
| Unit-testable without GPU | ✅ (SVG) | ❌ | ❌ | Limited |
| Browser / WASM | ✅ | Limited | ✅ | ✅ |
| Best for | Custom rendering, games, data vis, embedded | Line-of-business apps | Desktop apps | Mobile/web apps |

Tone: collegial, not combative.  Recommend MAUI for LOB apps, recommend Xui for
canvas-heavy, performance-sensitive, or fully custom-rendered UIs.

### 2.5 Social Proof / Stats Strip

A narrow strip with 3–4 metrics or badges:
- Open source (MIT)
- .NET 10 AoT
- 120 fps on M1 (benchmark citation)
- Platform badges: Windows · macOS · iOS · Android · Browser

### 2.6 "Start in 5 minutes" Teaser

A condensed version of the getting-started steps with syntax-highlighted C# code blocks,
ending with a CTA to the full guide.

---

## 3. Getting Started Page

A single-page, step-by-step guide.  Target: a working "Hello, Xui" app running locally
within 5 minutes.

Steps:

1. **Prerequisites** — .NET 10 SDK, OS requirements
2. **Install the template** — `dotnet new install Xui.Templates`
3. **Create the project** — `dotnet new xui-app -n HelloXui`
4. **Project structure** — annotated file tree
5. **Run it** — `dotnet run` with expected output screenshot
6. **Add your first View** — minimal custom `View` subclass with `OnRender`
7. **Next steps** — links to Canvas API, Views docs, example apps

Design requirements:

- Progress indicator (step 1 of 7) visible in the page header or a sticky sidebar
- Code blocks: full syntax highlighting for C#, shell commands clearly differentiated
- Inline annotated screenshots of the emulator showing the expected result at each step
- Mobile-responsive: the guide must be usable on a phone browser

---

## 4. Documentation Pages

### 4.1 Docs Hub (`/docs/`)

- A card grid or two-column list of documentation sections with title, description, and an
  icon or small illustration
- A search bar (full-text search across all docs)
- "What's new" strip for recent additions

### 4.2 Docs Article Page Layout

```
┌────────────────────────────────────────────────────────┐
│  Site header (nav + search)                             │
├──────────┬─────────────────────────────────┬────────────┤
│  Left    │  Article body                   │  Right     │
│  nav     │                                 │  ToC       │
│  tree    │  # Section heading              │  (sticky)  │
│          │                                 │            │
│          │  Prose + code blocks            │  § Heading │
│          │                                 │  § Sub     │
│          │  ┌──────────────────────────┐   │            │
│          │  │  C# code block           │   │            │
│          │  └──────────────────────────┘   │            │
│          │                                 │            │
│          │  ← Prev page  |  Next page →   │            │
└──────────┴─────────────────────────────────┴────────────┘
```

Requirements:

- Left nav: collapsible tree, active section highlighted with gradient accent
- Code blocks: line numbers, copy button, language badge, diff highlighting support
- Inline SVG diagrams (architecture diagrams already exist as SVG in `/www/docs`)
- Callout boxes: `note`, `tip`, `warning`, `danger` with distinct left-border colours
- Mobile: left nav collapses to a hamburger / bottom-sheet

### 4.3 C# API Reference Pages

Auto-generated by docfx from XML documentation comments.  Design requirements:

- Namespace / type / member three-level breadcrumb
- Member signature in a styled monospace block at the top
- Tabs: Summary · Parameters · Returns · Remarks · Examples
- "Since" version badge
- Source-link icon (links to GitHub line)
- Search powered by the same index as the docs

### 4.4 Code Block Design

This is the highest-frequency element in the docs.  Requirements:

- Dark theme (matches the site dark background) and optional light theme toggle
- Syntax highlighting for C# (primary), shell/bash, JSON
- Multi-file tabs (e.g. `Application.cs` | `MainWindow.cs` | `HelloView.cs`)
- Horizontal scroll for wide lines (never wrap code)
- Copy-to-clipboard button (top-right corner of block)
- Optional diff mode (green/red line markers for "before/after" examples)

---

## 5. Navigation & Global Chrome

### 5.1 Top Nav

```
[Xui logo]   Docs   Getting Started   API   Blog   GitHub ↗   [Search]
```

On scroll: compact sticky header with gradient underline.

### 5.2 Footer

- Links: Docs · Getting Started · API · GitHub · License (MIT) · Contributing
- Social: GitHub stars badge, NuGet badge
- Copyright line

---

## 6. Visual Design System (site-specific tokens)

The site should extend or reference `plans/design-system.md` but may use its own site-level
token layer:

| Token | Purpose | Suggested value |
|---|---|---|
| `--bg-primary` | Page background | `#080810` |
| `--bg-surface` | Card / code block surface | `#12121e` |
| `--bg-elevated` | Popover / nav background | `#1a1a2a` |
| `--accent-gradient` | Rainbow gradient accent | `linear-gradient(90deg, #7c3aed, #2563eb, #0ea5e9, #10b981, #f59e0b)` |
| `--text-primary` | Body text | `#f0f0fa` |
| `--text-secondary` | Muted labels | `#8888aa` |
| `--text-code` | Inline code | `#a5f3fc` |
| `--border` | Card / section borders | `1px solid rgba(255,255,255,0.08)` |
| `--radius-card` | Card corner radius | `12px` |
| `--radius-code` | Code block corner radius | `8px` |

---

## 7. Deliverables Requested from Design Studio

1. **Figma / Sketch file** with:
   - All page templates (landing, getting-started, docs article, API reference)
   - Component library: nav, footer, feature card, comparison table, code block, callout box,
     step indicator, hero emulator visual
   - Dark and light colour themes
   - Responsive breakpoints: 320 px, 768 px, 1 024 px, 1 440 px, 1 920 px
2. **SVG icon set** — minimum 24 icons for navigation, pillars, and callouts
3. **Motion specification** — scroll-triggered entrance animations, hover states for nav and cards
4. **Token JSON** — all design tokens in a format compatible with CSS custom properties and
   the Xui design system token model
5. **Accessibility checklist** — WCAG AA compliance for all text, interactive elements, and
   focus indicators

---

## 8. Out of Scope

- Static-site generator configuration (handled by Xui engineering with docfx)
- Content writing (handled by Xui team with AI assistance)
- Backend search indexing
