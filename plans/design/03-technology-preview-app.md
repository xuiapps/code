# Design Request — Technology Preview Application

## Overview

The Xui Technology Preview App is a single desktop/mobile application that showcases the
SDK's capabilities across **six real-world verticals**.  It is not a finished product — it is
an interactive SDK preview that explains the technology, demonstrates its potential, and
invites early-access developers to explore each vertical in depth.

This document is an RFQ for the **visual design** of the app shell, the six demo modules,
and the transitions between them.

---

## 1. App Purpose & Audience

| | |
|---|---|
| Primary audience | Cross-platform .NET developers evaluating Xui |
| Goal | "I can see myself building *that* with Xui" — within 2 minutes of opening the app |
| Format | Desktop app (macOS / Windows) via the Xui Emulator, also runnable as a browser WASM demo |
| Tone | Technical confidence, not marketing gloss.  Real code, real performance, real canvas. |

---

## 2. App Shell Design

### 2.1 Home / Selection Screen

The home screen shows **six vertical tiles** arranged in a 2 × 3 or 3 × 2 grid (responsive).

Each tile:
- Full-bleed background — a looping micro-animation specific to the vertical (e.g. the CAD
  tile slowly pans a technical drawing; the dashboard tile pulses live-ish gauge needles)
- Vertical name (bold, white, bottom-left)
- One-line description (light, semi-transparent)
- "Explore →" label
- On hover: tile expands slightly (scale 1.02), gradient border appears, description fully visible

The home screen itself has a dark background with the rainbow gradient accent language
from the emulator and website design (see `01-emulator.md` and `02-website.md`).

### 2.2 App Header / Nav Chrome

```
[Xui logo]   Technology Preview                    [← Back to home]
```

- Full-width translucent header with a gradient underline
- Back button fades in when inside a demo, hidden on home
- Subtle version badge: "Technology Preview · v0.1"

### 2.3 Navigation Transitions

Moving from home → demo:
- Selected tile expands to fill the screen (hero transition, 400 ms ease-out-expo)
- Demo content fades in from the tile's origin

Moving from demo → home:
- Reverse: content fades, tile collapses back, home grid reassembles

---

## 3. Six Demo Modules

### 3.1 Desktop CAD — "Precision Canvas"

**What it shows:** Pan, zoom, high-DPI canvas rendering, custom input handling.

Screen layout:
- Left panel: tool palette (pen, select, zoom, pan, measurement)
- Central canvas: a technical drawing (floor plan or mechanical part) rendered with
  `IContext.BeginPath` / `Stroke` / `Fill`
- Right panel: property inspector (selected object dimensions, coordinates)
- Status bar: cursor coordinates, scale, zoom level

Demo interaction:
- Mouse wheel zoom in/out (smooth, 60 fps)
- Pan with middle mouse or two-finger scroll
- Click on a shape to select + show dimensions
- Animated "typing" of coordinates in the property panel to show real-time updates

Design notes:
- Monochrome technical drawing aesthetic — white lines on `#0d0d17`
- Accent colour used only for selection highlight and tool-palette active state
- Grid overlay with dynamic density that adapts to zoom level

---

### 3.2 Hardware Dashboard — "Real-Time Monitor"

**What it shows:** Animated canvas gauges, data visualisation, 120 fps rendering.

Screen layout:
- Six circular gauge widgets (simulated RPM, temperature, voltage, current, flow rate, pressure)
- Two sparkline charts (time-series)
- One large bar-graph (frequency histogram)
- Alert strip at bottom (simulated threshold alerts)

Demo interaction:
- Values animate continuously (sine-wave-driven simulation)
- Click a gauge to expand it to full-screen detail view
- Threshold sliders to trigger alert state (gauge turns red with pulse animation)

Design notes:
- Dark HUD aesthetic (`#060612`)
- Gauge ring colour ramps from green (safe) → amber (warning) → red (critical)
- Font: monospace or near-monospace for numeric values
- Scanline / glow post-process effect on the gauge arcs

---

### 3.3 Mobile Game — "Canvas Playground"

**What it shows:** Touch input, sprite rendering, animation, 60/120 fps loop.

Screen layout (shown inside the emulator frame from §01-emulator.md):
- A simple arcade-style canvas game: colourful shapes orbiting a central attractor, tap to
  "collect" them
- Score counter (animated increment)
- Level indicator
- Pause / play button

Demo interaction:
- Tap / click to interact with game objects
- The emulator frame (if present) shows the rainbow gradient ring glowing in sync with the
  score multiplier

Design notes:
- Vibrant, saturated colour palette for game objects (contrast with dark background)
- Particle effects on collection (canvas-rendered, not DOM)
- Game runs at 60 fps minimum, 120 fps on ProMotion displays

---

### 3.4 IoT Control — "Smart Device Interface"

**What it shows:** Custom bespoke UI, touch-optimised controls, real-time state.

Screen layout:
- A coffee machine or vacuum robot control panel
- Large, touch-friendly buttons with animated state (brewing progress arc, robot path overlay)
- Status cards: battery level, last clean time, current mode

Demo interaction:
- Tap "Start brew" → animated arc fills, steam icon pulses, "Done" state triggers
- Drag slider for temperature / strength
- Vacuum robot shows animated path-finding on a floor-plan canvas

Design notes:
- Form follows function: controls are large, labelled, accessible
- The floor-plan canvas uses the same rendering primitives as the CAD demo
- Colour: warm amber accents for the coffee machine; teal for the robot

---

### 3.5 Health Monitor — "Smart Scale + Vitals"

**What it shows:** Data visualisation, SVG-based charts, animation, wearable companion UI.

Screen layout:
- Weight trend chart (line chart over 30 days)
- Body composition ring charts (fat %, muscle %, water %)
- Heart rate sparkline
- Sleep quality heatmap grid
- "Sync" button (animated spinner during simulated BLE sync)

Demo interaction:
- Tap a chart to expand + show detailed tooltip
- "New reading" button → animates a new data point arriving on the line chart

Design notes:
- Health-app aesthetic: clean white / light grey surfaces with colour-coded data tracks
- Accessible colour palette: avoid red/green-only differentiation
- Typography: large, readable numbers for the key metrics

---

### 3.6 3D Visualisation — "Spatial Canvas"

**What it shows:** 3D rendering integration, GPU shader pipeline, interactive 3D.

Screen layout:
- A 3D car dashboard (steering wheel, instrument cluster, centre console)
  *or* a 3D building floor-plan with selectable rooms
- Camera orbit controls (drag to rotate, pinch to zoom)
- Panel at right: selected element properties

Demo interaction:
- Drag to orbit the 3D model
- Click a room / component to highlight and show metadata
- Animated "live" data overlaid on 3D surfaces (temperature heatmap on rooms, speed on gauge)

Design notes:
- Dark environment with indirect lighting (ambient occlusion look)
- Data overlays use the canvas 2D API rendered on top of the 3D layer
- The transition from 2D gauges (demo 3.2) to 3D gauges here should feel evolutionary

---

## 4. Shared Component Patterns

These components appear in multiple demo modules and should be designed as a reusable system:

| Component | Used in |
|---|---|
| Circular gauge | Dashboard, Health, 3D |
| Sparkline chart | Dashboard, Health |
| Line chart | Health |
| Floor-plan canvas | IoT (robot), 3D (building) |
| Data card | All |
| Alert strip | Dashboard, IoT |
| Tab bar (bottom, mobile) | All mobile layouts |
| Expanded detail overlay | All |

---

## 5. Xui Capability Callouts

Each demo module should have an optional **"How Xui does this"** overlay that the developer
can toggle.  It shows:

- A simplified code snippet (2–5 lines of C#) that produces the visual being shown
- A label identifying the Xui API used (`IContext.Arc`, `LinearGradient`, `TouchEventRef`, …)
- A link to the relevant docs page

This overlay should be dismissible, non-intrusive, and use a dark card with a gradient
left-border accent.

---

## 6. Performance Indicator

A persistent but subtle FPS + memory badge in the app chrome (similar to the emulator
toolbar) that stays visible across all demos.  This is a deliberate signal that Xui is fast.

---

## 7. Deliverables Requested from Design Studio

1. **Figma / Sketch file** with:
   - Home selection screen (all 6 tiles, hover states, mobile responsive layout)
   - All 6 demo module screens (primary view + one expanded detail state each)
   - App shell chrome (header, nav, FPS badge, capability callout overlay)
   - Navigation transition storyboards (home → demo → detail → back)
2. **Motion specification**:
   - Hero tile expand/collapse (timing, easing, spring parameters)
   - Gauge needle animation curve
   - Chart data-arrival animation
   - Capability callout slide-in
3. **Asset list** — every SVG, icon, and illustration needed; placeholder or real
4. **Token mapping** — how site tokens (§6 of `02-website.md`) map to in-app tokens
5. **Accessibility pass** — focus states, keyboard nav order, contrast ratios

---

## 8. Out of Scope

- Actual data sources / BLE / hardware integration (simulated data only for V1)
- App Store / Play Store submission packaging
- Localisation (English only for V1)
