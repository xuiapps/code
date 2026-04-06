# Design Request — Xui Emulator Frame

## Overview

The Xui Emulator is a desktop middleware window that wraps any Xui mobile/tablet app in a
simulated device frame while the developer runs the app on macOS or Windows.  It already
renders a black rounded-rectangle frame with a status-bar row (clock, signal, battery, 5G)
and a bottom home-handle pill. This document is an RFQ for a **complete visual and interaction
redesign** of that frame.

---

## 1. Current Implementation Summary

| Property | Current value |
|---|---|
| Frame fill | Solid `#111111` rounded rect |
| Frame border | 8 px solid `#111111` + 2.5 px outer `#444444` |
| Status bar | Black label area at top, clock + pinhole/notch/island cutout + signal + battery + 5G |
| Bottom handle | 110 × 4 px pill, `#111111` |
| Title bar | 52 px solid `#333333` rounded rect (draggable, chromeless window) |
| Device models | iPhone 15 Pro (Dynamic Island), iPhone 14 (Notch), iPhone SE (no notch), iPad Pro, iPad Air, Pixel 8 Pro (Pinhole), Galaxy S24 Ultra (Pinhole, small radius), Galaxy Tab S9+, Pixel Tablet, OnePlus 12, iPhone 13 mini |
| Notch types | `None`, `Notch`, `DynamicIsland`, `PinHole` |

---

## 2. Aesthetic Direction

**Not skeuomorphic.**
The goal is a **sleek, modern, hardware-inspired frame** with a colourful gradient accent
language — similar to the way modern developer tools (Raycast, Arc, Linear) use gradient
highlights on dark surfaces.

Key visual principles:

- **Rainbow / spectrum gradient** as the primary accent.  The frame border should be a thin
  luminous gradient ring that transitions smoothly through the visible spectrum
  (or a curated subset — violet → cyan → green → gold).  The gradient should feel like a glowing
  edge, not a flat rainbow.
- **Dark translucent body.**  The physical frame body should sit on a very dark surface
  (`#0a0a0f` or similar deep navy-black), with subtle depth cues (inner shadow, slight sheen).
- **"Expand to reveal" controls.**  Hardware input buttons (volume up/down, power,
  mute/ringer) should normally be **invisible or very subtle** protrusions in the dark frame.
  On hover — either of the whole frame or of the button zone — the border should animate
  **outward** (5–8 px expansion, spring physics) to reveal labelled interactive controls.
  This keeps the neutral state clean and the interactive state discoverable.
- **Micro-contrast.**  Screen corners, the display bezels, and the pinhole/notch/island
  cutout should all use finely tuned radius and depth to feel real without being photographic.
- **Responsive sizing.**  The frame should scale gracefully between the phone sizes
  (~330 × 680 dp) and tablet sizes (~768 × 1024 dp) without the gradient ring or hardware
  buttons looking stretched.

---

## 3. Frame Structure & Layers

```
┌──────────────────────────────────────────────────────┐
│  Title / toolbar row  (draggable, contains dev tools) │
├──────────────────────────────────────────────────────┤
│  ╔═══════════════════════════════════════════════╗   │  ← gradient ring (always visible)
│  ║  ┌─────────────────────────────────────────┐ ║   │  ← device frame body (dark)
│  ║  │  Status bar (notch / island / clean)    │ ║   │  ← screen display area (app content)
│  ║  │                                         │ ║   │
│  ║  │            APP CONTENT                  │ ║   │
│  ║  │                                         │ ║   │
│  ║  │  ▼ home indicator bar                   │ ║   │
│  ║  └─────────────────────────────────────────┘ ║   │
│  ╚═══════════════════════════════════════════════╝   │
│  [side-button zone — revealed on hover]              │
└──────────────────────────────────────────────────────┘
```

Layers (back to front):

1. **Background** — very dark window background
2. **Device body** — rounded rect, dark metal-like fill with a very subtle specular gradient
3. **Screen inset** — slightly recessed rectangle with corner radii matching the device profile
4. **Gradient ring** — thin (2–4 px) luminous ring wrapping the screen inset edge
5. **Screen content** — clipped app render
6. **Overlay glass** — optional slight gloss pass for the upper-half of the screen
7. **Notch / Dynamic Island / Pinhole** — device-accurate cutout rendered on top
8. **Status bar items** — clock, signal, battery composited over the app
9. **Hardware buttons** — revealed side affordances (see §4)
10. **Title toolbar** — developer tool row above the frame

---

## 4. Hardware Input Controls

### 4.1 Physical Side Buttons (always present on real devices)

These should be rendered as subtle raised areas on the device body frame.
On hover they expand and show click targets and tooltips.

| Button | Side | Devices | Simulated action |
|---|---|---|---|
| Power / Side button | Right | All | Lock screen / wake |
| Volume Up | Left | All phones | Raise software volume |
| Volume Down | Left | All phones | Lower software volume |
| Mute / Ringer switch | Left | iPhone (non-SE) | Toggle ringer mode |
| Action button | Left | iPhone 15 Pro | Custom configurable action |

Tablet-specific:
| Button | Side | Devices |
|---|---|---|
| Home button | Bezel | iPad SE, older |
| Power button | Top | iPad Air, iPad Pro (USB-C) |

### 4.2 Software Input Panel

A **collapsible panel** (attached below or to the side of the device frame, toggle via toolbar)
provides input simulation outside the device chrome:

#### Push Notifications
- **Sender name** text field
- **Notification title** text field  
- **Notification body** text field
- **App icon** selector (choose from installed test apps)
- **[Send notification]** button → fires a synthetic `UNNotification`-style event into the abstract window

#### Location / Geo Services
- **Latitude / Longitude** numeric inputs (with map preview thumbnail)
- **Accuracy** slider (GPS accuracy in meters: 3 m → 1 000 m)
- **Speed** input (km/h) for motion simulation
- **Heading** input (0–360°)
- **[Set location]** button → updates the simulated `CoreLocation`/`Android Location` service
- **Route playback** — load a GPX file and replay a route at configurable speed

#### Sensors & System State
- **Battery level** slider (0–100%) + **Charging** toggle
- **Network type** selector: `WiFi`, `5G`, `4G LTE`, `3G`, `Offline`
- **Signal strength** slider (0–4 bars)
- **Dark / Light mode** toggle
- **Accessibility font size** slider (xs → accessibility-xl)
- **Locale / language** selector
- **Low power mode** toggle

#### Camera & Media (for camera-dependent apps)
- **Front camera** feed selector: real webcam / still image / video file
- **Rear camera** feed selector: same options
- **Microphone** mute toggle

---

## 5. Device Switcher

The toolbar row should include a **device switcher** that lets the developer:

- Select from the full `DeviceCatalog` (phone or tablet)
- Filter by: `Brand` (Apple / Android), `DeviceType` (Phone / Tablet)
- See a live-preview thumbnail of the selected form factor (small icon)
- Rotate between portrait and landscape
- Common quick-picks accessible via a toolbar button group (e.g. "SE → 15 Pro → iPad Air")

Visual groupings in the picker:

| Group | Variants |
|---|---|
| Small phones | iPhone SE; phones ≤ 375 pt wide |
| Standard phones | iPhone 14/15; Pixel 8 Pro; OnePlus 12 |
| Large phones | Galaxy S24 Ultra; phones ≥ 440 pt wide |
| Tablets | iPad Pro 12.9″; iPad Air; Galaxy Tab S9+; Pixel Tablet |

Notch type should auto-update the rendered status-bar chrome (no notch / pill notch /
Dynamic Island / pinhole).  Corner radius is taken directly from `DeviceProfile.ScreenCornerRadius`.

---

## 6. Title / Developer Toolbar

The 52 px drag-area at the top should evolve into a proper toolbar with:

| Control | Description |
|---|---|
| App icon + name | Identifies the running app (from `Application.Title`) |
| Device name label | Current device model, e.g. "iPhone 15 Pro" |
| Orientation toggle | Portrait ↔ Landscape icon button |
| Device picker | Dropdown / popover (§5) |
| Simulation panel toggle | Show / hide the software input panel (§4.2) |
| FPS counter | Live frames-per-second badge |
| Hot-reload indicator | Amber dot when hot-reload is active |
| Minimize / Fullscreen | Window management icons (top-right, macOS-style) |

The toolbar should use the same gradient accent as the frame ring but at reduced opacity
so it reads as "UI chrome" rather than "device metal".

---

## 7. Animation Specifications

| Trigger | Animation |
|---|---|
| Hover enters frame edge | Border expands 6 px outward, spring k=300 damping=22 |
| Hover leaves frame | Border springs back, same spring |
| Device switch | Cross-fade + slight scale (0.95 → 1.0), 280 ms ease-out |
| Notification arrives | Frame border flashes white → gradient, 400 ms ease-out |
| Location set | Brief map-pin icon pulse over the screen corner, 300 ms |
| Hot-reload | Screen fades to black → content, 220 ms |
| Orientation change | Frame rotates with 3D perspective tilt, 350 ms |

---

## 8. Deliverables Requested from Design Studio

1. **SVG master artboard** with all device variants (phone tall, phone wide, tablet) at 2× density
2. **Figma / Sketch component library** with named layers matching the code structure in
   `EmulatorWindow.cs` (gradient ring, body, screen inset, status bar, hardware buttons, toolbar)
3. **Interaction specification** document (Figma prototype or annotated PDF) covering
   expand-on-hover, device-switcher popover, and software-input panel slide-in
4. **Token export** — all colours, radii, spacing, shadow definitions as a JSON token file
   compatible with the `Xui.Core.Canvas` token model (see `plans/design-system.md`)
5. **Motion spec** — easing curves and spring parameters for every animation in §7
6. **Accessibility pass** — minimum contrast ratios for all text and icon elements in both
   light-toolbar and dark-toolbar themes

---

## 9. Out of Scope

- Actual C# implementation (handled by Xui engineering)
- Keyboard shortcuts UI (out of scope for V1)
- Multi-window / side-by-side device comparison (planned for V2)
