# Design Request — Xui Icon Library

## Overview

Xui needs a first-party **animated icon set** that spans the full range of Xui's target
verticals — from consumer mobile apps to industrial control panels.  The key design axis is
**corner radius**: a single semantic scale from "Bubbly" (large radii, soft, consumer-friendly)
through "Rounded" (balanced, general-purpose) to "Sharp" (minimal radius, industrial,
data-dense).  All icons share the same grid, stroke weight system, and animation structure so
they can be mixed and composed within a single app by adjusting one design token.

This document is an RFQ for **50 animated icons** in that system.

---

## 1. Design Principles

### 1.1 Corner-Radius Scale

| Level | Token name | Corner radius | Typical context |
|---|---|---|---|
| 0 — Sharp | `icon-radius-sharp` | 0–1 px | Industrial dashboards, CNC panels, data monitors |
| 1 — Crisp | `icon-radius-crisp` | 2–3 px | Developer tools, B2B SaaS, productivity |
| 2 — Rounded | `icon-radius-rounded` | 4–6 px | General mobile and desktop apps |
| 3 — Soft | `icon-radius-soft` | 8–10 px | Consumer apps, lifestyle, retail |
| 4 — Bubbly | `icon-radius-bubbly` | 14–18 px | Games, children's apps, promo/loyalty |

Each icon is delivered in **all five** radius variants so the app can select the variant
matching its design-system token without commissioning a new set.

### 1.2 Stroke & Fill Language

- Default state: **outline / stroke** only (1.5 px optical stroke weight on 24 × 24 grid)
- Active / selected state: **fill + stroke** or **filled** variant; fill uses the app accent colour
- Inactive / disabled state: stroke at 40 % opacity

Animated transitions between states alter lines and fill area (not just opacity) to feel
intentional — e.g. a tab-bar icon that "fills in" as a selection ripple rather than a simple
crossfade.

### 1.3 Grid & Sizing

| Token | Value |
|---|---|
| Base grid | 24 × 24 px (logical) |
| Live area | 20 × 20 px (2 px padding each side) |
| Small variant | 16 × 16 px (same proportions, re-drawn — not scaled) |
| Large variant | 32 × 32 px |
| Stroke weight base | 1.5 px @ 24 px |
| Stroke weight small | 1.5 px @ 16 px (visually heavier — intentional) |

### 1.4 Animation Model

Each icon has two animation slots:

1. **Idle loop** — a subtle, very low-amplitude motion when the icon is visible but not
   interacted with.  Optional; may be `none` for Sharp/Crisp variants.  Examples:
   - A clock icon whose second-hand ticks
   - A WiFi icon that pulses its arcs in sequence
   - A heart icon with a slow beat

2. **State transition** — triggered when the icon changes state (unselected → selected, or
   active → inactive).  Duration: 200–350 ms.  The transition morphs **stroke paths and fill
   regions** — not just opacity.  Examples:
   - A play icon whose triangle fills from left-to-right and whose corners round/sharpen
   - A bookmark icon whose outline strokes to a fill
   - A star icon whose points extend and fill sequentially

Animation parameters must be exported as spring/tween specs (duration ms, easing or
spring k/damping) for implementation with `Xui.Core.Animation` easing curves.

---

## 2. Icon Catalogue (50 icons)

### 2.1 Navigation & Wayfinding (8)

| # | Name | Description |
|---|---|---|
| 01 | Home | House outline → filled house |
| 02 | Back / Arrow Left | Chevron or arrow; morphs weight on press |
| 03 | Forward / Arrow Right | Mirror of Back |
| 04 | Menu / Hamburger | Three lines → X (close) morph |
| 05 | Search | Magnifier; handle extends on focus |
| 06 | Settings / Gear | Gear that rotates on idle, fills on active |
| 07 | Close / X | Two lines that animate in on mount |
| 08 | More / Ellipsis | Three dots that pulse or space out on idle |

### 2.2 Media & Playback (7)

| # | Name | Description |
|---|---|---|
| 09 | Play | Triangle fills from left on activation |
| 10 | Pause | Two bars that briefly squeeze together |
| 11 | Stop | Square; corners sharpen on press |
| 12 | Next track | Double chevron; second bar delays slightly |
| 13 | Previous track | Mirror of Next track |
| 14 | Volume / Speaker | Bell with animated arc waves |
| 15 | Microphone | Capsule + stand; glow pulse on active |

### 2.3 Connectivity & System (7)

| # | Name | Description |
|---|---|---|
| 16 | WiFi | Arcs sequence in/out on idle |
| 17 | Bluetooth | Standard B path; pulses on pairing |
| 18 | Location / Pin | Pin drops on activation |
| 19 | Notification / Bell | Bell swings on new notification |
| 20 | Battery | Fill level animates; bolt overlays on charge |
| 21 | Sync / Refresh | Circle arrow rotates on active |
| 22 | Cloud / Upload | Cloud with arrow rises on active |

### 2.4 Actions & Editing (8)

| # | Name | Description |
|---|---|---|
| 23 | Add / Plus | Lines grow from centre outward |
| 24 | Delete / Trash | Lid lifts, then body shakes on confirm |
| 25 | Edit / Pencil | Pencil tip traces a small stroke |
| 26 | Copy | Top sheet lifts off on activation |
| 27 | Share | Arrow curves out; branches fan on idle |
| 28 | Save / Floppy or Tick | Check mark draws on completion |
| 29 | Filter | Funnel fills then drains on idle |
| 30 | Sort | Bars shuffle into order |

### 2.5 Status & Feedback (6)

| # | Name | Description |
|---|---|---|
| 31 | Success / Checkmark | Circle draws then check strokes in |
| 32 | Warning / Triangle | Triangle assembles; exclamation bounces |
| 33 | Error / X Circle | Circle draws then X strokes in |
| 34 | Info / i Circle | Same circle; dot drops then bar rises |
| 35 | Loading / Spinner | Stroke dash orbits; eases in/out |
| 36 | Star / Favourite | Points extend one by one; fills gold on active |

### 2.6 Commerce & Loyalty (4)

| # | Name | Description |
|---|---|---|
| 37 | QR Code | Corners assemble from outside in |
| 38 | Tag / Promo | Tag swings in on appearance |
| 39 | Cart / Basket | Item drops into basket on add |
| 40 | Gift / Reward | Ribbon ties on active |

### 2.7 IoT & Control (6)

| # | Name | Description |
|---|---|---|
| 41 | Robot / Vacuum | Small rounded robot; swivel head on idle |
| 42 | Schedule / Clock | Clock hands sweep to next scheduled time |
| 43 | Temperature | Mercury rises/falls on value change |
| 44 | Power / On-Off | Arc completes a circle on activation |
| 45 | Map / Floor Plan | Grid lines draw in on appearance |
| 46 | Coffee / Cup | Steam wisps animate upward on idle |

### 2.8 Culture & Lifestyle (4)

| # | Name | Description |
|---|---|---|
| 47 | Audio / Headphones | Cups rotate slightly; cord sways |
| 48 | Language / Globe | Meridian lines spin slowly on idle |
| 49 | Camera | Shutter blades open/close |
| 50 | Health / Heart | Heartbeat pulse on idle; fills on active |

---

## 3. Radius Variant Relationship

The five radius variants are not separate icon sets — they are the **same SVG paths**
parameterised by a single corner-radius token.  The design studio should deliver:

- Master paths designed at the `Rounded` (level 2) canonical radius
- Explicit corner-point annotations on each path segment that should receive radius treatment
- A `corner-radius-scale` mapping function (linear or eased) for the five levels

The Xui rendering engine applies the radius at draw time via `ctx.RoundRect` or
`ctx.BezierCurveTo` corner rounding — no extra SVG files per variant are needed for
**simple shapes**.  For icons with complex organic paths (robot, heart, coffee cup) separate
path data per major radius level (Sharp, Rounded, Bubbly) may be required.

---

## 4. Animation Deliverables

For each of the 50 icons:

| Deliverable | Format |
|---|---|
| Idle loop (if non-trivial) | Lottie JSON or SMIL SVG animation spec |
| State transition | Lottie JSON or spring/tween parameter table |
| Spring / tween table row | `duration_ms`, `easing` or `spring_k` / `spring_damping`, `delay_ms` |

The Xui implementation will consume the spring parameters directly via
`Xui.Core.Animation.Easing` — Lottie is not used at runtime, but the Lottie JSON serves as
a motion reference spec for the engineering team.

---

## 5. Deliverables Requested from Design Studio

1. **SVG source files** — one file per icon, containing:
   - All five radius variants as named layers / artboards
   - Idle and selected state as separate frames within the same file
   - Properly named path groups matching this catalogue's `Name` column
2. **Figma component library** — all 50 icons as components with:
   - `radius` property (Sharp / Crisp / Rounded / Soft / Bubbly)
   - `state` property (Default / Hover / Active / Disabled)
   - Interactive prototype connections showing state transitions
3. **Animation spec sheet** — one row per icon with timing and spring parameters (see §4)
4. **Token JSON** — corner-radius values and stroke-weight values keyed to the five levels,
   compatible with the Xui design system token model (`plans/design-system.md`)
5. **Usage guidelines** — one-page PDF: when to use Sharp vs Bubbly, do/don't examples,
   minimum touch target sizes, padding rules

---

## 6. Out of Scope

- Icon font or glyph embedding (SVG paths used directly in Xui canvas rendering)
- Platform-specific adaptive icons (Android launcher / iOS home screen)
- Full illustration set (this is a UI icon set only)
