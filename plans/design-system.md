# Xui Design System

## Overview

This document defines the architecture and token categories for the Xui Design System — a timeless,
cross-platform, cross-form-factor visual language for Xui applications. The goal is to let application
authors write UI once and have it feel natural on every device: a phone, a tablet, a desktop, a car
screen, or a TV — the way a Spotify app feels consistent yet adapts to context.

The design system is pure **data + math**. There are no CSS variables, no DOM elements per cell, no
pseudo-selectors. Instead, widgets query a typed `IDesignSystem` interface at attach time (or when the
system changes) and use the returned values directly in their rendering code.

---

## 1. Research: Modern Cross-Platform Design Systems

### 1.1 Material Design 3 (Google)

Material Design 3 (Material You, 2021) is the most data-driven public design system to date.

**Key ideas**
- **Dynamic Color** — generates a full tonal palette from a single seed color using the HCT (Hue,
  Chroma, Tone) color space, which is perceptually uniform relative to human vision.
- **Color roles** — named slots (Primary, Secondary, Tertiary, Error, Surface, Outline, …) each having
  a *container* variant and an *on-container* text/icon color, giving 28+ semantic color tokens.
- **Typography scale** — five levels (Display, Headline, Title, Body, Label) × three sizes, all with
  explicit size, line-height, letter-spacing, and weight defaults.
- **Shape** — five shape families (None, Extra-Small, Small, Medium, Large, Full) mapped to corner
  radius values.  Each component is assigned a shape category.
- **Motion** — "Expressive" tokens: `Emphasized`, `EmphasizedDecelerate`, `EmphasizedAccelerate`, and
  `Standard`, each a cubic Bezier with explicit duration ranges.
- **Elevation** — surface tinting at different levels replaces drop-shadows for expressing depth.

**Strengths**: Mathematically derivable from one seed color; excellent accessibility math (contrast
ratio checking baked in).  
**Limitation**: Web/Android-first; relies on platform-native theming hooks that don't exist in Xui.

---

### 1.2 Apple Human Interface Guidelines (Cupertino)

Apple's HIG is narrative rather than token-based but implies a coherent set of choices:

**Key ideas**
- **Semantic colors** — `systemBackground`, `secondarySystemBackground`, `label`, `secondaryLabel`, etc.
  Automatically switch between light and dark, and between different contrast modes.
- **Dynamic Type** — text size levels (`largeTitle`, `title1` … `caption2`) scale with the user's
  Accessibility font size preference.  Each level has a minimum size floor.
- **Vibrancy / materials** — blur-based translucency (`.ultraThinMaterial`, `.regularMaterial`, etc.)
  adapt foreground colors to whatever is behind the view.
- **SF Symbols** — vector icons whose weight and scale match the surrounding text weight and size.
- **Corner radius** — contextually scaled: `12 pt` for cards, `10 pt` for buttons, `8 pt` for text
  fields, fully round for pills and toggles.
- **Animation** — spring-based (`damping`, `initialVelocity`) rather than duration/easing.

**Strengths**: Deep accessibility integration; system-level dark mode; rich haptics model.  
**Limitation**: Heavily platform-tied; not trivially portable to non-Apple platforms.

---

### 1.3 Flutter (Material + Cupertino + Custom)

Flutter abstracts both Material and Cupertino behind a `ThemeData` tree that resolves through the
widget `BuildContext`, analogous to Xui's parent-chain DI:

```dart
// Provider at the root
MaterialApp(theme: ThemeData(colorScheme: ColorScheme.fromSeed(seedColor: Colors.deepPurple)));

// Consumer deep in the tree
final color = Theme.of(context).colorScheme.primary;
```

**Key ideas**
- `ThemeData` is a single immutable snapshot injected at the `MaterialApp` root.
- `ColorScheme` encodes all Material 3 color roles (derived via `ColorScheme.fromSeed`).
- `TextTheme` encodes the typography scale.
- `ShapeBorderTheme` maps component types to shape families.
- **Component-level overrides** — e.g. `ButtonThemeData`, `InputDecorationTheme` allow fine-grained
  per-component token overrides without touching the global theme.
- `ThemeExtension<T>` allows apps to inject custom typed sub-themes.

**Strengths**: Context-driven resolution is exactly the DI model Xui already uses; rich component
override story; well-documented.

---

### 1.4 Microsoft Fluent Design System

Fluent 2 (WinUI 3, Teams, Microsoft 365) defines:

**Key ideas**
- **Color ramp** — each brand color generates a ramp of 10 tints (10 % lighter) and 10 shades (10 %
  darker); semantic aliases (`neutralForeground1`, `brandBackground1`, etc.) map ramp stops to roles.
- **Typography** — `Caption1`, `Body1`, `Body1Strong`, `Body2`, `Subtitle1`, `Subtitle2`, `Title1`,
  `Title2`, `Title3`, `LargeTitle`, `Display` — all with explicit `font-weight`, `font-size`, and
  `line-height` values.
- **Geometry / Shape** — `borderRadiusNone (0)`, `borderRadiusSmall (2)`, `borderRadiusMedium (4)`,
  `borderRadiusLarge (6)`, `borderRadiusXLarge (8)`, `borderRadiusCircular (9999)`.
- **Spacing** — baseline 4 pt grid: `spacingHorizontalNone (0)`, `…XXS (2)`, `…XS (4)`, `…S (8)`,
  `…M (12)`, `…L (16)`, `…XL (20)`, `…XXL (24)`, `…XXXL (32)`.
- **Elevation** — shadow levels (2, 4, 8, 16, 28, 64) with explicit shadow color, blur, and spread.
- **Motion** — `durationUltraFast (50 ms)` … `durationSlow (400 ms)`; standard easing curves.

---

### 1.5 IBM Carbon Design System

IBM Carbon targets enterprise / data-heavy applications:

**Key ideas**
- Strict 8 pt spacing grid with 2 pt sub-grid for dense UI.
- **Size variants** for every component: `sm`, `md`, `lg` — maps to both height and padding.
- Two-layer neutral palette: Gray 10 (light) and Gray 100 (dark) with 10-step tonal ramps for each
  functional color.
- Explicit **interactive states**: enabled, hover, active, focus, disabled, skeleton/loading.
- **Type scale** is geometric: each level × 1.25 the previous.

---

### 1.6 Radix Primitives / Radix Themes

Radix targets cross-browser accessible primitives first, then layers tokens on top:

**Key ideas**
- **Gray scale** — nine functional grays (1–12) derived for both light and dark.
- **Accent scale** — same nine slots applied to any of 30 color families; same mathematical derivation.
- **Type scale** — `1` (xs) … `9` (2xl), font-size + letter-spacing co-derived.
- **Radius** — `1 (3 px)` … `6 (full)` with a global `--radius-factor` multiplier that scales all radii.
- **Space** — `1 (4 px)` … `9 (40 px)`; component padding maps to named space tokens.

---

### 1.7 Comparative Summary

| Aspect | Material 3 | Cupertino | Fluent 2 | Carbon | Radix |
|---|---|---|---|---|---|
| Color derivation | HCT tonal palette from seed | Semantic system colors | Brand ramp + semantic aliases | Functional tonal ramps | Scale 1–12 per hue |
| Dark mode | Auto via tonal roles | Auto via semantic colors | Auto via semantic aliases | Two separate palettes | Auto via scale |
| Typography scale | 15 named slots | 12 Dynamic Type levels | 12 named slots | Geometric × 1.25 | 9 size tokens |
| Shape / radius | 6 families per component | Context-scaled points | 6 named radii | Strict sizes per component | 6 levels + factor |
| Spacing | 4 pt grid | 8/12/16 multiples | 4 pt grid | 8 pt grid / 2 pt sub-grid | 4 pt grid |
| Motion | 4 easing tokens + durations | Spring-based | 5 duration tokens + easings | Duration-based | N/A |
| Form-factor | Adaptive layouts | Size classes | Adaptive panels | Responsive columns | N/A |
| DI / context | Context-tree (`Theme.of`) | Environment (SwiftUI) | N/A (CSS variables) | N/A (CSS vars) | N/A (CSS vars) |

**Takeaways for Xui**:
1. Color must derive from a small seed set using **color-space math** (not hardcoded tables).
2. Typography must participate in **accessibility scaling** (Dynamic Type equivalent).
3. All tokens must be **named and typed** so widgets read them programmatically.
4. Context-tree resolution (like Flutter's `Theme.of`) is already native to Xui's parent-chain DI.
5. Components should map to shape **families**, not hardcoded point values, so the entire app can shift
   roundness with one knob.
6. Motion tokens must distinguish **physics-based** (springs) from **curve-based** (Bezier) so the
   "bouncy art app vs stiff business app" axis is explicit.
7. Icons and drawables must be an **abstraction** that lets platform providers supply native vectors or
   custom renderers.

---

## 2. Device and Form-Factor Model

### 2.1 Device Idiom

```
IDeviceInfo
├── Idiom: Mobile | Tablet | Desktop | Car | TV | Watch
├── PointerModel: Touch | Stylus | Mouse | Controller | Eye
└── Scale: nfloat  (physical px per logical pt, e.g. 2.0 for Retina)
```

| Idiom | Pointer | Typical primary hit-test radius | Layout density |
|---|---|---|---|
| Mobile | Touch | 44 pt (Apple) / 48 dp (Material) | Compact |
| Tablet | Touch + Stylus | 44 pt touch, 16 pt stylus | Regular |
| Desktop | Mouse | 8–16 pt | Dense |
| Car | Touch | 60 pt (driver distraction) | Very coarse |
| TV | D-pad / Eye | N/A | Very large text |
| Watch | Touch | 44 pt but very small canvas | Ultra-compact |

### 2.2 Hit-Test Area vs Visual Size

Large hit-test area does **not** mean large visual element. A search field magnifying-glass icon may
render at 16 pt but have a 44 pt tappable region:

```
┌──────────────────────────────────────────────────────┐
│  Hit area (44 × 44 pt transparent, touch-only)       │
│       ╔══════════════╗                                │
│       ║  Search icon ║  ← visual: 16 × 16 pt         │
│       ║   🔍  16pt   ║                                │
│       ╚══════════════╝                                │
└──────────────────────────────────────────────────────┘
```

The design system exposes `MinimumHitTestRadius` and widgets use it when computing their hit-test
extension but keep their visual bounds separate.

### 2.3 Spotify Paradigm: Same App, Different Idiom

<svg viewBox="0 0 700 300" xmlns="http://www.w3.org/2000/svg" font-family="Inter,system-ui,sans-serif" font-size="11">
  <!-- Mobile -->
  <rect x="10" y="10" width="140" height="280" rx="18" fill="#111" stroke="#444" stroke-width="1.5"/>
  <rect x="20" y="20" width="120" height="180" rx="4" fill="#1DB954" opacity=".18"/>
  <text x="80" y="115" fill="#1DB954" text-anchor="middle" font-size="28">♪</text>
  <rect x="20" y="208" width="120" height="32" rx="16" fill="#1DB954" opacity=".9"/>
  <text x="80" y="228" fill="#fff" text-anchor="middle" font-weight="600" font-size="10">▶  PLAY</text>
  <text x="80" y="265" fill="#888" text-anchor="middle" font-size="9">⊕ Mobile — 44pt buttons</text>

  <!-- Tablet -->
  <rect x="180" y="30" width="200" height="240" rx="12" fill="#111" stroke="#444" stroke-width="1.5"/>
  <rect x="180" y="30" width="72" height="240" rx="12" fill="#0a0a0a" stroke="#333" stroke-width="1"/>
  <text x="216" y="90" fill="#1DB954" text-anchor="middle" font-size="11">Home</text>
  <text x="216" y="115" fill="#888" text-anchor="middle" font-size="11">Search</text>
  <text x="216" y="140" fill="#888" text-anchor="middle" font-size="11">Library</text>
  <rect x="260" y="50" width="108" height="70" rx="4" fill="#1DB954" opacity=".18"/>
  <text x="314" y="91" fill="#1DB954" text-anchor="middle" font-size="22">♪</text>
  <rect x="260" y="130" width="108" height="28" rx="14" fill="#1DB954" opacity=".9"/>
  <text x="314" y="148" fill="#fff" text-anchor="middle" font-weight="600" font-size="10">▶  PLAY</text>
  <text x="280" y="282" fill="#888" text-anchor="middle" font-size="9">⊕ Tablet — side nav</text>

  <!-- Desktop -->
  <rect x="410" y="20" width="270" height="260" rx="6" fill="#111" stroke="#444" stroke-width="1.5"/>
  <rect x="410" y="20" width="80" height="260" rx="6" fill="#0a0a0a" stroke="#333" stroke-width="1"/>
  <text x="450" y="60" fill="#1DB954" text-anchor="middle" font-size="10" font-weight="600">Home</text>
  <text x="450" y="80" fill="#888" text-anchor="middle" font-size="10">Search</text>
  <text x="450" y="100" fill="#888" text-anchor="middle" font-size="10">Library</text>
  <text x="450" y="120" fill="#888" text-anchor="middle" font-size="10">Liked</text>
  <!-- content area -->
  <rect x="498" y="30" width="174" height="100" rx="3" fill="#1a1a1a"/>
  <text x="585" y="85" fill="#1DB954" text-anchor="middle" font-size="18">♪</text>
  <!-- dense track list -->
  <rect x="498" y="138" width="174" height="16" rx="2" fill="#1a1a1a"/>
  <rect x="498" y="158" width="174" height="16" rx="2" fill="#222"/>
  <rect x="498" y="178" width="174" height="16" rx="2" fill="#1a1a1a"/>
  <!-- playback bar -->
  <rect x="410" y="240" width="270" height="40" rx="0" fill="#181818" stroke="#333" stroke-width=".5"/>
  <text x="545" y="264" fill="#fff" text-anchor="middle" font-size="11">▶  ⏭  ♡  🔊</text>
  <text x="545" y="290" fill="#888" text-anchor="middle" font-size="9">⊕ Desktop — dense layout, 8pt hit areas</text>
</svg>

The same `IDesignSystem` feeds all three — only `IDeviceInfo.Idiom` and `MinimumHitTestRadius` change.

---

## 3. Design Token Categories

### 3.1 Color System

#### 3.1.1 Seed → Palette Math

The Xui color system starts from **one to four seed hues** and derives a complete set of roles using
**HSL / OKLCH interpolation**:

1. **Primary hue** (brand identity color)
2. **Secondary hue** (optional; if omitted, derived as the split-complementary at ±150°)
3. **Tertiary hue** (optional; at ±90° or user-specified)
4. **Neutral hue** (optional; typically the primary hue desaturated 90 %)

Each hue generates a **tonal ramp** of 13 stops: `0, 5, 10, 20, 30, 40, 50, 60, 70, 80, 90, 95, 100`.

Color-scheme relationships:

| Scheme | Formula (H = primary hue in degrees) |
|---|---|
| Complementary | Secondary = H + 180° |
| Split-complementary | Secondary = H + 150°, Tertiary = H + 210° |
| Triadic | Secondary = H + 120°, Tertiary = H + 240° |
| Tetradic | Secondary = H + 90°, Tertiary = H + 180°, Quaternary = H + 270° |
| Analogous | Secondary = H + 30°, Tertiary = H + 60° |

<svg viewBox="0 0 480 160" xmlns="http://www.w3.org/2000/svg" font-family="Inter,system-ui,sans-serif" font-size="9">
  <!-- Title -->
  <text x="8" y="14" fill="#ccc" font-size="10" font-weight="600">Tonal palette — Primary (blue), Secondary (green), Neutral</text>
  <!-- Tone labels -->
  <text x="8"   y="35" fill="#999" text-anchor="middle">0</text>
  <text x="46"  y="35" fill="#999" text-anchor="middle">10</text>
  <text x="84"  y="35" fill="#999" text-anchor="middle">20</text>
  <text x="122" y="35" fill="#999" text-anchor="middle">30</text>
  <text x="160" y="35" fill="#999" text-anchor="middle">40</text>
  <text x="198" y="35" fill="#999" text-anchor="middle">50</text>
  <text x="236" y="35" fill="#999" text-anchor="middle">60</text>
  <text x="274" y="35" fill="#999" text-anchor="middle">70</text>
  <text x="312" y="35" fill="#999" text-anchor="middle">80</text>
  <text x="350" y="35" fill="#999" text-anchor="middle">90</text>
  <text x="388" y="35" fill="#999" text-anchor="middle">95</text>
  <text x="426" y="35" fill="#999" text-anchor="middle">99</text>
  <text x="460" y="35" fill="#999" text-anchor="middle">100</text>
  <!-- Primary ramp (blue) -->
  <text x="8" y="62" fill="#ccc" font-size="8">P</text>
  <rect x="20"  y="42" width="30" height="20" rx="2" fill="#000080"/>
  <rect x="58"  y="42" width="30" height="20" rx="2" fill="#00008b"/>
  <rect x="96"  y="42" width="30" height="20" rx="2" fill="#0000cd"/>
  <rect x="134" y="42" width="30" height="20" rx="2" fill="#1a1aff"/>
  <rect x="172" y="42" width="30" height="20" rx="2" fill="#4040ff"/>
  <rect x="210" y="42" width="30" height="20" rx="2" fill="#6060ff"/>
  <rect x="248" y="42" width="30" height="20" rx="2" fill="#8080ff"/>
  <rect x="286" y="42" width="30" height="20" rx="2" fill="#9999ff"/>
  <rect x="324" y="42" width="30" height="20" rx="2" fill="#b3b3ff"/>
  <rect x="362" y="42" width="30" height="20" rx="2" fill="#ccccff"/>
  <rect x="400" y="42" width="30" height="20" rx="2" fill="#e0e0ff"/>
  <rect x="438" y="42" width="30" height="20" rx="2" fill="#f0f0ff"/>
  <!-- Secondary ramp (green) -->
  <text x="8" y="90" fill="#ccc" font-size="8">S</text>
  <rect x="20"  y="70" width="30" height="20" rx="2" fill="#004000"/>
  <rect x="58"  y="70" width="30" height="20" rx="2" fill="#006000"/>
  <rect x="96"  y="70" width="30" height="20" rx="2" fill="#008000"/>
  <rect x="134" y="70" width="30" height="20" rx="2" fill="#1aaa1a"/>
  <rect x="172" y="70" width="30" height="20" rx="2" fill="#28c028"/>
  <rect x="210" y="70" width="30" height="20" rx="2" fill="#3dd63d"/>
  <rect x="248" y="70" width="30" height="20" rx="2" fill="#66e066"/>
  <rect x="286" y="70" width="30" height="20" rx="2" fill="#80e680"/>
  <rect x="324" y="70" width="30" height="20" rx="2" fill="#a3f0a3"/>
  <rect x="362" y="70" width="30" height="20" rx="2" fill="#c0f5c0"/>
  <rect x="400" y="70" width="30" height="20" rx="2" fill="#d9f9d9"/>
  <rect x="438" y="70" width="30" height="20" rx="2" fill="#edfaed"/>
  <!-- Neutral ramp (gray) -->
  <text x="8" y="118" fill="#ccc" font-size="8">N</text>
  <rect x="20"  y="98" width="30" height="20" rx="2" fill="#000000"/>
  <rect x="58"  y="98" width="30" height="20" rx="2" fill="#1a1a1a"/>
  <rect x="96"  y="98" width="30" height="20" rx="2" fill="#333333"/>
  <rect x="134" y="98" width="30" height="20" rx="2" fill="#4d4d4d"/>
  <rect x="172" y="98" width="30" height="20" rx="2" fill="#666666"/>
  <rect x="210" y="98" width="30" height="20" rx="2" fill="#808080"/>
  <rect x="248" y="98" width="30" height="20" rx="2" fill="#999999"/>
  <rect x="286" y="98" width="30" height="20" rx="2" fill="#b3b3b3"/>
  <rect x="324" y="98" width="30" height="20" rx="2" fill="#cccccc"/>
  <rect x="362" y="98" width="30" height="20" rx="2" fill="#e0e0e0"/>
  <rect x="400" y="98" width="30" height="20" rx="2" fill="#f0f0f0"/>
  <rect x="438" y="98" width="30" height="20" rx="2" fill="#f8f8f8"/>
  <!-- Accent / data-viz ramp (orange) -->
  <text x="8" y="146" fill="#ccc" font-size="8">A</text>
  <rect x="20"  y="126" width="30" height="20" rx="2" fill="#401500"/>
  <rect x="58"  y="126" width="30" height="20" rx="2" fill="#7a2800"/>
  <rect x="96"  y="126" width="30" height="20" rx="2" fill="#c04000"/>
  <rect x="134" y="126" width="30" height="20" rx="2" fill="#e05800"/>
  <rect x="172" y="126" width="30" height="20" rx="2" fill="#ff7a1a"/>
  <rect x="210" y="126" width="30" height="20" rx="2" fill="#ff993d"/>
  <rect x="248" y="126" width="30" height="20" rx="2" fill="#ffb366"/>
  <rect x="286" y="126" width="30" height="20" rx="2" fill="#ffc080"/>
  <rect x="324" y="126" width="30" height="20" rx="2" fill="#ffd4a3"/>
  <rect x="362" y="126" width="30" height="20" rx="2" fill="#ffe4c4"/>
  <rect x="400" y="126" width="30" height="20" rx="2" fill="#fff0dd"/>
  <rect x="438" y="126" width="30" height="20" rx="2" fill="#fff8f0"/>
</svg>

#### 3.1.2 Color Roles

Each tonal palette maps to **semantic color roles**. The table below shows light-mode defaults:

| Role | Token | Tonal stop (light) | Tonal stop (dark) | Purpose |
|---|---|---|---|---|
| Background | `Colors.Background` | Neutral 99 | Neutral 6 | Window / screen background |
| OnBackground | `Colors.OnBackground` | Neutral 10 | Neutral 90 | Text on background |
| Surface | `Colors.Surface` | Neutral 98 | Neutral 12 | Card / panel fill |
| OnSurface | `Colors.OnSurface` | Neutral 10 | Neutral 90 | Text on surface |
| SurfaceVariant | `Colors.SurfaceVariant` | Neutral 90 | Neutral 30 | Alternate panel fill |
| Outline | `Colors.Outline` | Neutral 50 | Neutral 60 | Border, divider |
| OutlineVariant | `Colors.OutlineVariant` | Neutral 80 | Neutral 30 | Subtle divider |
| Primary | `Colors.Primary` | Primary 40 | Primary 80 | Brand / action color |
| OnPrimary | `Colors.OnPrimary` | Primary 100 | Primary 20 | Text on primary |
| PrimaryContainer | `Colors.PrimaryContainer` | Primary 90 | Primary 30 | Tinted container |
| OnPrimaryContainer | `Colors.OnPrimaryContainer` | Primary 10 | Primary 90 | Text on container |
| Secondary | `Colors.Secondary` | Secondary 40 | Secondary 80 | Supporting action |
| OnSecondary | `Colors.OnSecondary` | Secondary 100 | Secondary 20 | Text on secondary |
| SecondaryContainer | `Colors.SecondaryContainer` | Secondary 90 | Secondary 30 | Supporting container |
| OnSecondaryContainer | `Colors.OnSecondaryContainer` | Secondary 10 | Secondary 90 | Text on sec. container |
| Tertiary / Accent | `Colors.Accent` | Tertiary 40 | Tertiary 80 | Highlight / accent |
| OnAccent | `Colors.OnAccent` | Tertiary 100 | Tertiary 20 | Text on accent |
| Error | `Colors.Error` | Error 40 | Error 80 | Destructive / error |
| OnError | `Colors.OnError` | Error 100 | Error 20 | Text on error |
| DataViz[0..N] | `Colors.DataViz` | — | — | Chart series colors |

> **Implementation note**: `IColorSystem.GetTone(hue, saturation, tone)` converts HSL or OKLCH values
> to `Xui.Core.Canvas.Color`. The tonal ramp functions are pure math with no lookup tables.

---

### 3.2 Typography

The typography system defines a **scale** of named text styles.  Each style carries:

| Property | Type | Description |
|---|---|---|
| `FontFamily` | `string` | Family name (defaults to app-level `DefaultFontFamily`) |
| `FontSize` | `nfloat` | Size in points; scaled by `AccessibilityFontScale` |
| `LineHeight` | `nfloat` | In points (not a multiplier) |
| `LetterSpacing` | `nfloat` | Additional tracking in points |
| `FontWeight` | `FontWeight` | 100–900 |
| `FontStyle` | `FontStyle` | Normal / Italic |

**Named scale levels**:

| Level | Default Size | Weight | Use |
|---|---|---|---|
| `Display` | 57 | 400 | Hero, marketing, splash |
| `HeadlineLarge` | 32 | 400 | Page title |
| `HeadlineMedium` | 28 | 400 | Section title |
| `HeadlineSmall` | 24 | 400 | Sub-section |
| `TitleLarge` | 22 | 400 | List group header |
| `TitleMedium` | 16 | 500 | Card header, toolbar |
| `TitleSmall` | 14 | 500 | Tab label, chip |
| `BodyLarge` | 16 | 400 | Reading text |
| `BodyMedium` | 14 | 400 | Default UI text |
| `BodySmall` | 12 | 400 | Secondary text |
| `LabelLarge` | 14 | 500 | Button, link |
| `LabelMedium` | 12 | 500 | Badge, tag |
| `LabelSmall` | 11 | 500 | Caption, metadata |

`AccessibilityFontScale` is a `nfloat` multiplier (default `1.0`) provided by `IDeviceInfo` and
reflecting the user's platform accessibility font size preference. Widgets multiply every `FontSize` by
this value.

<svg viewBox="0 0 480 320" xmlns="http://www.w3.org/2000/svg" font-family="Inter,system-ui,sans-serif">
  <rect width="480" height="320" fill="#0f0f0f" rx="8"/>
  <text x="16" y="28" fill="#888" font-size="10">Type Scale</text>
  <text x="16" y="64"  fill="#f0f0f0" font-size="28" font-weight="400">Display — 57 pt</text>
  <text x="16" y="96"  fill="#f0f0f0" font-size="22" font-weight="400">Headline Large — 32 pt</text>
  <text x="16" y="122" fill="#f0f0f0" font-size="18" font-weight="400">Headline Medium — 28 pt</text>
  <text x="16" y="144" fill="#f0f0f0" font-size="15" font-weight="400">Headline Small — 24 pt</text>
  <text x="16" y="164" fill="#f0f0f0" font-size="14" font-weight="400">Title Large — 22 pt</text>
  <text x="16" y="182" fill="#f0f0f0" font-size="12" font-weight="500">Title Medium — 16 pt · weight 500</text>
  <text x="16" y="198" fill="#ccc"    font-size="11" font-weight="500">Title Small — 14 pt · weight 500</text>
  <text x="16" y="214" fill="#f0f0f0" font-size="11" font-weight="400">Body Large — 16 pt</text>
  <text x="16" y="229" fill="#ccc"    font-size="10" font-weight="400">Body Medium — 14 pt (default)</text>
  <text x="16" y="242" fill="#999"    font-size="9"  font-weight="400">Body Small — 12 pt</text>
  <text x="16" y="257" fill="#e0e0e0" font-size="10" font-weight="500">Label Large — 14 pt · weight 500</text>
  <text x="16" y="270" fill="#aaa"    font-size="9"  font-weight="500">Label Medium — 12 pt · weight 500</text>
  <text x="16" y="282" fill="#777"    font-size="8"  font-weight="500">Label Small — 11 pt · weight 500</text>
  <line x1="16" y1="288" x2="464" y2="288" stroke="#333" stroke-width=".5"/>
  <text x="16" y="300" fill="#555" font-size="8">All sizes × AccessibilityFontScale (default 1.0)</text>
</svg>

---

### 3.3 Spacing

A **4 pt base grid**. Named tokens:

| Token | Value |
|---|---|
| `Spacing.None` | 0 |
| `Spacing.XXS` | 2 |
| `Spacing.XS` | 4 |
| `Spacing.S` | 8 |
| `Spacing.M` | 12 |
| `Spacing.L` | 16 |
| `Spacing.XL` | 24 |
| `Spacing.XXL` | 32 |
| `Spacing.XXXL` | 48 |

Component defaults:

| Component | Internal padding | Recommended margin |
|---|---|---|
| Button (default) | H: `L` (16) · V: `S` (8) | `XS` (4) |
| Button (compact, density-reduced) | H: `M` (12) · V: `XS` (4) | `XXS` (2) |
| TextBox / Input | H: `M` (12) · V: `S` (8) | `XS` (4) |
| Card / Surface | All sides: `L` (16) | `S` (8) |
| Section header | H: `L` (16) · V: `S` (8) | — |
| List row | H: `L` (16) · V: `S` (8) | — |
| Icon | Touch target extended to `MinimumHitTestRadius × 2` | — |

`MinimumHitTestRadius` is derived from `IDeviceInfo.PointerModel`:

```
PointerModel.Touch   → 22 pt  (44 pt diameter)
PointerModel.Stylus  → 10 pt  (20 pt diameter)
PointerModel.Mouse   →  4 pt  ( 8 pt diameter)
PointerModel.Eye     →  0 pt  (focus-based)
```

---

### 3.4 Roundness (Shape)

A **single `CornerStyle` enum** maps to a `CornerRadius` multiplied by a global `RoundnessFactor`:

| CornerStyle | Base radius | × `RoundnessFactor` (default 1.0) |
|---|---|---|
| `None` | 0 | 0 |
| `ExtraSmall` | 2 | 2 |
| `Small` | 4 | 4 |
| `Medium` | 8 | 8 |
| `Large` | 12 | 12 |
| `ExtraLarge` | 16 | 16 |
| `Full` | 9999 (pill) | 9999 |

`RoundnessFactor` is a `nfloat` (0.0 = all square, 1.0 = default, 2.0 = very round). Setting it above
`1.0` multiplies every base radius proportionally (capped at `Full`).

Component shape defaults:

| Component | CornerStyle |
|---|---|
| Button (filled) | `Full` |
| Button (outlined) | `Full` |
| Chip | `Small` |
| Card | `Large` |
| TextBox | `Small` |
| Dialog | `ExtraLarge` |
| BottomSheet | `Large` (top corners only) |
| Navigation rail | `None` |

---

### 3.5 Animation

Two categories of motion tokens:

#### Curve-based (Bezier)

| Token | Cubic Bezier | Duration range | Use |
|---|---|---|---|
| `Motion.EmphasizedDecelerate` | (0.05, 0.7, 0.1, 1.0) | 400–500 ms | Elements entering the screen |
| `Motion.EmphasizedAccelerate` | (0.3, 0.0, 0.8, 0.15) | 200–300 ms | Elements leaving the screen |
| `Motion.Standard` | (0.2, 0.0, 0.0, 1.0) | 300–500 ms | General transitions |
| `Motion.StandardDecelerate` | (0.0, 0.0, 0.0, 1.0) | 250–400 ms | Settling transitions |
| `Motion.StandardAccelerate` | (0.3, 0.0, 1.0, 1.0) | 200–300 ms | Quick dismissals |
| `Motion.Linear` | (0.0, 0.0, 1.0, 1.0) | any | Progress bars, continuous |

#### Spring-based (Physics)

| Token | Stiffness | Damping | Use |
|---|---|---|---|
| `Motion.SpringBouncy` | 600 | 0.5 | Art / lifestyle apps, button press |
| `Motion.SpringResponsive` | 300 | 0.8 | Default interactive feedback |
| `Motion.SpringSmooth` | 200 | 1.0 (critically damped) | Business / utility apps, modals |

`IMotionSystem.Preference` is `Curve` or `Spring`; apps or platform adapters set this to match the
desired personality. Individual widget authors query it and choose their animation strategy.

The global `ReducedMotion` bool (from `IDeviceInfo.PrefersReducedMotion`) disables all non-essential
transitions.

---

### 3.6 Icons and Drawables

Icons are expressed as `IDrawable` — a zero-allocation interface invoked during the render pass:

```csharp
public interface IDrawable
{
    /// Render this drawable into `context` within `frame`.
    void Draw(IContext context, Rect frame);

    /// Intrinsic size hint; (0,0) means unconstrained.
    Size IntrinsicSize { get; }
}
```

The icon system registers **named drawables** that can be overridden per-platform:

```
IIconSystem
├── GetIcon(IconName name) : IDrawable
└── RegisterIcon(IconName name, IDrawable drawable)
```

**Well-known icon names** (non-exhaustive):

| Category | Names |
|---|---|
| Navigation | `ChevronDown`, `ChevronRight`, `ChevronLeft`, `ChevronUp` |
| Actions | `Close`, `Add`, `Remove`, `Edit`, `Confirm`, `Search` |
| Data | `SortAscending`, `SortDescending`, `Filter`, `Download`, `Upload` |
| State | `CheckboxEmpty`, `CheckboxChecked`, `CheckboxIndeterminate`, `RadioOff`, `RadioOn` |
| Feedback | `ErrorCircle`, `WarningTriangle`, `InfoCircle`, `SuccessCircle` |

Platform adapters may supply vector paths from SF Symbols, Fluent Icons, or custom SVG path data.
Custom effects (ripple animations, loading spinners, lottie-style animations) are also `IDrawable`
implementations.

---

## 4. C# Interface Design

The design system is a **service** resolved via Xui's parent-chain DI — the same mechanism used for
`IFocus`, `ITextMeasureContext`, etc. Widgets call `GetService<IDesignSystem>()` in `OnAttach`.

### 4.1 Primary Interface

```csharp
namespace Xui.Core.Design;

/// <summary>
/// Root interface for the Xui Design System.
/// Resolved from the parent-chain service provider (GetService&lt;IDesignSystem&gt;()).
/// </summary>
public interface IDesignSystem
{
    /// <summary>Color tokens and palette math.</summary>
    IColorSystem Colors { get; }

    /// <summary>Typography scale.</summary>
    ITypographySystem Typography { get; }

    /// <summary>Spacing tokens derived from the 4-pt grid.</summary>
    ISpacingSystem Spacing { get; }

    /// <summary>Shape / corner-radius tokens.</summary>
    IShapeSystem Shape { get; }

    /// <summary>Motion tokens (curves and springs).</summary>
    IMotionSystem Motion { get; }

    /// <summary>Named icon and drawable registry.</summary>
    IIconSystem Icons { get; }

    /// <summary>Information about the current device and pointer model.</summary>
    IDeviceInfo Device { get; }
}
```

### 4.2 Color System Interface

```csharp
namespace Xui.Core.Design;

using Xui.Core.Canvas;

/// <summary>
/// Provides color roles derived from a seed palette.
/// </summary>
public interface IColorSystem
{
    // -- Application background / foreground --
    Color Background { get; }
    Color OnBackground { get; }

    // -- Surface (cards, panels) --
    Color Surface { get; }
    Color OnSurface { get; }
    Color SurfaceVariant { get; }
    Color OnSurfaceVariant { get; }

    // -- Borders and dividers --
    Color Outline { get; }
    Color OutlineVariant { get; }

    // -- Primary action color --
    Color Primary { get; }
    Color OnPrimary { get; }
    Color PrimaryContainer { get; }
    Color OnPrimaryContainer { get; }

    // -- Secondary / supporting action --
    Color Secondary { get; }
    Color OnSecondary { get; }
    Color SecondaryContainer { get; }
    Color OnSecondaryContainer { get; }

    // -- Accent / tertiary highlight --
    Color Accent { get; }
    Color OnAccent { get; }

    // -- Semantic error state --
    Color Error { get; }
    Color OnError { get; }
    Color ErrorContainer { get; }
    Color OnErrorContainer { get; }

    // -- Focus ring color (typically Accent at full opacity) --
    Color FocusRing { get; }

    // -- Data-visualization series colors (at least 6) --
    ReadOnlySpan<Color> DataVizPalette { get; }

    /// <summary>
    /// Interpolates a color at a given tone (0–100) for a specific hue/saturation.
    /// Tone 0 = black, 100 = white; mid-tones vary by hue.
    /// </summary>
    Color GetTone(nfloat hueDegrees, nfloat saturation, nfloat tone);

    /// <summary>
    /// Indicates if the current effective color scheme is dark.
    /// </summary>
    bool IsDark { get; }
}
```

### 4.3 Typography System Interface

```csharp
namespace Xui.Core.Design;

using Xui.Core.Canvas;

/// <summary>
/// Provides the typography scale.
/// All FontSize values are pre-multiplied by <see cref="IDeviceInfo.AccessibilityFontScale"/>.
/// </summary>
public interface ITypographySystem
{
    TextStyle Display         { get; }
    TextStyle HeadlineLarge   { get; }
    TextStyle HeadlineMedium  { get; }
    TextStyle HeadlineSmall   { get; }
    TextStyle TitleLarge      { get; }
    TextStyle TitleMedium     { get; }
    TextStyle TitleSmall      { get; }
    TextStyle BodyLarge       { get; }
    TextStyle BodyMedium      { get; }
    TextStyle BodySmall       { get; }
    TextStyle LabelLarge      { get; }
    TextStyle LabelMedium     { get; }
    TextStyle LabelSmall      { get; }

    /// <summary>The default font family used across the application.</summary>
    string DefaultFontFamily { get; }
}

/// <summary>
/// An immutable snapshot of a single text style from the typography scale.
/// </summary>
public readonly struct TextStyle
{
    public string     FontFamily     { get; init; }
    public nfloat     FontSize       { get; init; }
    public nfloat     LineHeight     { get; init; }
    public nfloat     LetterSpacing  { get; init; }
    public FontWeight FontWeight     { get; init; }
    public FontStyle  FontStyle      { get; init; }
}
```

### 4.4 Spacing System Interface

```csharp
namespace Xui.Core.Design;

/// <summary>
/// Provides spacing tokens based on a 4-pt grid.
/// </summary>
public interface ISpacingSystem
{
    nfloat None   { get; }  //  0
    nfloat XXS    { get; }  //  2
    nfloat XS     { get; }  //  4
    nfloat S      { get; }  //  8
    nfloat M      { get; }  // 12
    nfloat L      { get; }  // 16
    nfloat XL     { get; }  // 24
    nfloat XXL    { get; }  // 32
    nfloat XXXL   { get; }  // 48
}
```

### 4.5 Shape System Interface

```csharp
namespace Xui.Core.Design;

using Xui.Core.Canvas;

/// <summary>
/// Provides corner-radius tokens scaled by <see cref="RoundnessFactor"/>.
/// </summary>
public interface IShapeSystem
{
    /// <summary>Global multiplier for all corner radii (default 1.0).</summary>
    nfloat RoundnessFactor { get; }

    CornerRadius None        { get; }  //  0
    CornerRadius ExtraSmall  { get; }  //  2 × RoundnessFactor
    CornerRadius Small       { get; }  //  4 × RoundnessFactor
    CornerRadius Medium      { get; }  //  8 × RoundnessFactor
    CornerRadius Large       { get; }  // 12 × RoundnessFactor
    CornerRadius ExtraLarge  { get; }  // 16 × RoundnessFactor
    CornerRadius Full        { get; }  // 9999 (pill)
}
```

### 4.6 Motion System Interface

```csharp
namespace Xui.Core.Design;

using Xui.Core.Animation;

/// <summary>
/// Provides motion tokens for animations.
/// </summary>
public interface IMotionSystem
{
    /// <summary>Whether springs or curves are preferred for interactive feedback.</summary>
    MotionPreference Preference { get; }

    /// <summary>True if the user has requested reduced motion (accessibility).</summary>
    bool ReducedMotion { get; }

    // -- Curve-based tokens --
    CurveToken EmphasizedDecelerate  { get; }
    CurveToken EmphasizedAccelerate  { get; }
    CurveToken Standard              { get; }
    CurveToken StandardDecelerate    { get; }
    CurveToken StandardAccelerate    { get; }
    CurveToken Linear                { get; }

    // -- Spring-based tokens --
    SpringToken SpringBouncy         { get; }
    SpringToken SpringResponsive     { get; }
    SpringToken SpringSmooth         { get; }
}

public enum MotionPreference { Curve, Spring }

public readonly struct CurveToken
{
    public float P1x { get; init; }
    public float P1y { get; init; }
    public float P2x { get; init; }
    public float P2y { get; init; }
    public float DefaultDurationMs { get; init; }
}

public readonly struct SpringToken
{
    public float Stiffness { get; init; }
    public float Damping   { get; init; }
}
```

### 4.7 Icon System Interface

```csharp
namespace Xui.Core.Design;

using Xui.Core.Canvas;
using Xui.Core.Math2D;

/// <summary>
/// Named icon and drawable registry. Platform adapters register icons;
/// widgets look them up by name.
/// </summary>
public interface IIconSystem
{
    /// <summary>Returns a drawable for the given icon name, or null if not registered.</summary>
    IDrawable? GetIcon(string name);

    /// <summary>Returns an icon scaled to a specific visual size.</summary>
    IDrawable? GetIcon(string name, Size visualSize);
}

/// <summary>
/// A zero-allocation rendering primitive that draws into a canvas frame.
/// </summary>
public interface IDrawable
{
    Size IntrinsicSize { get; }
    void Draw(IContext context, Rect frame);
}
```

### 4.8 Device Info Interface

```csharp
namespace Xui.Core.Design;

/// <summary>
/// Provides device and pointer model information for layout adaptation.
/// </summary>
public interface IDeviceInfo
{
    DeviceIdiom   Idiom              { get; }
    PointerModel  PointerModel       { get; }
    nfloat        Scale              { get; }   // physical px / logical pt
    nfloat        MinimumHitTestRadius { get; } // pt
    nfloat        AccessibilityFontScale { get; } // 1.0 = default
    bool          PrefersReducedMotion  { get; }
    bool          PrefersHighContrast   { get; }
    ColorScheme   ColorScheme           { get; } // Light | Dark
}

public enum DeviceIdiom  { Mobile, Tablet, Desktop, Car, TV, Watch }
public enum PointerModel { Touch, Stylus, Mouse, Controller, Eye }
public enum ColorScheme  { Light, Dark }
```

---

## 5. Widget Design with SVG Mockups

### 5.1 Buttons

Three **importance levels** map to different color role usage:

| Level | Fill | Text | Border | Use |
|---|---|---|---|---|
| `FilledButton` | `Primary` | `OnPrimary` | none | Primary CTA |
| `TonalButton` | `SecondaryContainer` | `OnSecondaryContainer` | none | Secondary CTA |
| `OutlinedButton` | transparent | `Primary` | `Outline` | Tertiary CTA |
| `TextButton` | transparent | `Primary` | none | Low-prominence action |

**Corner radius**: `Shape.Full` (pill) by default.

<svg viewBox="0 0 480 280" xmlns="http://www.w3.org/2000/svg" font-family="Inter,system-ui,sans-serif" font-size="12">
  <rect width="480" height="280" fill="#fafafa" rx="8"/>
  <!-- Section label -->
  <text x="16" y="20" fill="#444" font-size="10" font-weight="600">Button variants — light theme</text>

  <!-- === Filled (Primary) === -->
  <text x="16" y="44" fill="#888" font-size="9">Filled (Primary)</text>
  <!-- Default -->
  <rect x="16"  y="50" width="80" height="36" rx="18" fill="#4040ff"/>
  <text x="56"  y="73" fill="#fff" text-anchor="middle" font-weight="600">Action</text>
  <!-- Hover -->
  <rect x="108" y="50" width="80" height="36" rx="18" fill="#5555ff"/>
  <text x="148" y="73" fill="#fff" text-anchor="middle" font-weight="600">Action</text>
  <text x="148" y="98" fill="#aaa" text-anchor="middle" font-size="8">hover</text>
  <!-- Pressed -->
  <rect x="200" y="50" width="80" height="36" rx="18" fill="#3030e0"/>
  <text x="240" y="73" fill="#fff" text-anchor="middle" font-weight="600">Action</text>
  <text x="240" y="98" fill="#aaa" text-anchor="middle" font-size="8">pressed</text>
  <!-- Disabled -->
  <rect x="292" y="50" width="80" height="36" rx="18" fill="#e0e0e0"/>
  <text x="332" y="73" fill="#aaa" text-anchor="middle" font-weight="600">Action</text>
  <text x="332" y="98" fill="#aaa" text-anchor="middle" font-size="8">disabled</text>

  <!-- === Tonal (SecondaryContainer) === -->
  <text x="16" y="118" fill="#888" font-size="9">Tonal (SecondaryContainer)</text>
  <rect x="16"  y="124" width="80" height="36" rx="18" fill="#c0f5c0"/>
  <text x="56"  y="147" fill="#003a00" text-anchor="middle" font-weight="600">Action</text>
  <rect x="108" y="124" width="80" height="36" rx="18" fill="#d0f8d0"/>
  <text x="148" y="147" fill="#003a00" text-anchor="middle" font-weight="600">Action</text>
  <text x="148" y="171" fill="#aaa" text-anchor="middle" font-size="8">hover</text>
  <rect x="200" y="124" width="80" height="36" rx="18" fill="#aaebaa"/>
  <text x="240" y="147" fill="#003a00" text-anchor="middle" font-weight="600">Action</text>
  <text x="240" y="171" fill="#aaa" text-anchor="middle" font-size="8">pressed</text>
  <rect x="292" y="124" width="80" height="36" rx="18" fill="#f0f0f0"/>
  <text x="332" y="147" fill="#aaa" text-anchor="middle" font-weight="600">Action</text>
  <text x="332" y="171" fill="#aaa" text-anchor="middle" font-size="8">disabled</text>

  <!-- === Outlined === -->
  <text x="16" y="191" fill="#888" font-size="9">Outlined</text>
  <rect x="16"  y="197" width="80" height="36" rx="18" fill="none" stroke="#4040ff" stroke-width="1.5"/>
  <text x="56"  y="220" fill="#4040ff" text-anchor="middle" font-weight="600">Action</text>
  <rect x="108" y="197" width="80" height="36" rx="18" fill="#f0f0ff" stroke="#4040ff" stroke-width="1.5"/>
  <text x="148" y="220" fill="#4040ff" text-anchor="middle" font-weight="600">Action</text>
  <text x="148" y="244" fill="#aaa" text-anchor="middle" font-size="8">hover</text>
  <rect x="200" y="197" width="80" height="36" rx="18" fill="#e0e0ff" stroke="#4040ff" stroke-width="1.5"/>
  <text x="240" y="220" fill="#4040ff" text-anchor="middle" font-weight="600">Action</text>
  <text x="240" y="244" fill="#aaa" text-anchor="middle" font-size="8">pressed</text>
  <rect x="292" y="197" width="80" height="36" rx="18" fill="none" stroke="#ccc" stroke-width="1.5"/>
  <text x="332" y="220" fill="#aaa" text-anchor="middle" font-weight="600">Action</text>
  <text x="332" y="244" fill="#aaa" text-anchor="middle" font-size="8">disabled</text>

  <!-- Token legend -->
  <text x="16"  y="268" fill="#4040ff" font-size="8">■ Primary</text>
  <text x="76"  y="268" fill="#003a00" font-size="8">■ OnSecContainer</text>
  <text x="176" y="268" fill="#aaa"    font-size="8">■ Disabled state</text>
</svg>

#### 5.1.1 Button Group (Segment Control)

Three or more buttons sharing a border, with one marked `IsActive`. The active button uses
`PrimaryContainer` fill; inactive buttons use `SurfaceVariant`:

<svg viewBox="0 0 400 120" xmlns="http://www.w3.org/2000/svg" font-family="Inter,system-ui,sans-serif" font-size="12">
  <rect width="400" height="120" fill="#fafafa" rx="8"/>
  <text x="16" y="20" fill="#444" font-size="10" font-weight="600">Button Group (Segment Control)</text>

  <!-- Group border -->
  <rect x="16" y="32" width="234" height="40" rx="20" fill="none" stroke="#c0c0c0" stroke-width="1.5"/>
  <!-- Inactive left -->
  <rect x="16" y="32" width="78" height="40" rx="20" fill="#f0f0f0"/>
  <text x="55" y="57" fill="#444" text-anchor="middle" font-size="12">All</text>
  <!-- Active middle (PrimaryContainer) -->
  <rect x="94" y="32" width="78" height="40" rx="0" fill="#e0e0ff"/>
  <text x="133" y="57" fill="#1a1aff" text-anchor="middle" font-size="12" font-weight="600">Music</text>
  <!-- Inactive right -->
  <rect x="172" y="32" width="78" height="40" rx="20" fill="#f0f0f0"/>
  <text x="211" y="57" fill="#444" text-anchor="middle" font-size="12">Podcasts</text>

  <!-- Annotation -->
  <line x1="133" y1="80" x2="133" y2="94" stroke="#666" stroke-width=".8"/>
  <text x="133" y="105" fill="#666" text-anchor="middle" font-size="9">IsActive → PrimaryContainer + OnPrimaryContainer</text>

  <!-- Dark mode variant -->
  <rect x="268" y="32" width="114" height="40" rx="20" fill="#1a1a2e" stroke="#333" stroke-width="1"/>
  <rect x="268" y="32" width="38" height="40" rx="20" fill="#1a1a2e"/>
  <text x="287" y="57" fill="#888" text-anchor="middle" font-size="12">All</text>
  <rect x="306" y="32" width="38" height="40" rx="0" fill="#3030a0"/>
  <text x="325" y="57" fill="#9999ff" text-anchor="middle" font-size="12" font-weight="600">◉</text>
  <rect x="344" y="32" width="38" height="40" rx="20" fill="#1a1a2e"/>
  <text x="363" y="57" fill="#888" text-anchor="middle" font-size="12">Pod</text>
  <text x="325" y="100" fill="#555" text-anchor="middle" font-size="9">dark mode</text>
</svg>

---

### 5.2 TextBox and Input Fields

A `TextBox` queries design tokens at `OnAttach` and applies them to its `BorderLayer`:

```csharp
// Inside a custom TextBox-style widget's OnAttach:
var ds = GetService<IDesignSystem>()!;
_borderLayer.CornerRadius = ds.Shape.Small;
_borderLayer.BorderColor  = ds.Colors.Outline;
_borderLayer.Padding      = new Frame(ds.Spacing.M, ds.Spacing.S);
_labelStyle               = ds.Typography.BodyMedium;
```

<svg viewBox="0 0 480 240" xmlns="http://www.w3.org/2000/svg" font-family="Inter,system-ui,sans-serif" font-size="12">
  <rect width="480" height="240" fill="#fafafa" rx="8"/>
  <text x="16" y="18" fill="#444" font-size="10" font-weight="600">TextBox states</text>

  <!-- Default -->
  <text x="16" y="40" fill="#888" font-size="9">Default</text>
  <rect x="16" y="46" width="200" height="40" rx="4" fill="#fff" stroke="#b0b0b0" stroke-width="1.5"/>
  <text x="28" y="71" fill="#999" font-size="12">Enter text…</text>

  <!-- Focused -->
  <text x="240" y="40" fill="#888" font-size="9">Focused</text>
  <rect x="240" y="46" width="220" height="40" rx="4" fill="#fff" stroke="#4040ff" stroke-width="2"/>
  <text x="252" y="71" fill="#222" font-size="12">Hello, world</text>
  <line x1="326" y1="56" x2="326" y2="76" stroke="#4040ff" stroke-width="1.5"/>
  <text x="350" y="96" fill="#4040ff" font-size="8">Focus ring → Colors.FocusRing</text>

  <!-- Error -->
  <text x="16" y="114" fill="#888" font-size="9">Error</text>
  <rect x="16" y="120" width="200" height="40" rx="4" fill="#fff8f8" stroke="#cc0000" stroke-width="1.5"/>
  <text x="28" y="145" fill="#222" font-size="12">invalid@</text>
  <text x="16" y="170" fill="#cc0000" font-size="9">⚠  Please enter a valid email address</text>
  <text x="16" y="182" fill="#aaa" font-size="8">BorderColor → Colors.Error · Helper → Colors.OnErrorContainer</text>

  <!-- Disabled -->
  <text x="240" y="114" fill="#888" font-size="9">Disabled</text>
  <rect x="240" y="120" width="220" height="40" rx="4" fill="#f5f5f5" stroke="#e0e0e0" stroke-width="1.5"/>
  <text x="252" y="145" fill="#bbb" font-size="12">Disabled input</text>

  <!-- Label float animation note -->
  <rect x="16" y="196" width="440" height="30" rx="4" fill="#f0f0ff"/>
  <text x="26" y="215" fill="#4040ff" font-size="9">Token path: CornerRadius ← Shape.Small · Padding ← Spacing.M/S · Label ← Typography.BodyMedium · FocusColor ← Colors.FocusRing</text>
</svg>

---

### 5.3 SearchBox — Visual Size vs Hit-Test Area

<svg viewBox="0 0 480 200" xmlns="http://www.w3.org/2000/svg" font-family="Inter,system-ui,sans-serif" font-size="12">
  <rect width="480" height="200" fill="#fafafa" rx="8"/>
  <text x="16" y="18" fill="#444" font-size="10" font-weight="600">SearchBox — hit area expansion</text>

  <!-- Full search bar (desktop) -->
  <text x="16" y="40" fill="#888" font-size="9">Desktop: full-width search bar</text>
  <rect x="16" y="46" width="320" height="36" rx="18" fill="#f0f0f0" stroke="#e0e0e0" stroke-width="1"/>
  <text x="42" y="69" fill="#333" font-size="13">🔍</text>
  <text x="60" y="69" fill="#888" font-size="12">Search…</text>

  <!-- Icon-only mode (mobile dense header) -->
  <text x="16" y="110" fill="#888" font-size="9">Mobile: icon-only — small visual (16pt) · large hit area (44pt)</text>
  <!-- Hit area indicator -->
  <rect x="28" y="118" width="44" height="44" rx="22" fill="#4040ff" opacity=".08" stroke="#4040ff" stroke-width=".8" stroke-dasharray="3,2"/>
  <!-- Visual icon -->
  <rect x="44" y="134" width="16" height="16" rx="4" fill="none" stroke="#444" stroke-width="1.5"/>
  <line x1="57" y1="147" x2="63" y2="153" stroke="#444" stroke-width="1.5" stroke-linecap="round"/>
  <circle cx="51" cy="141" r="5" fill="none" stroke="#444" stroke-width="1.5"/>
  <!-- Label hit area -->
  <text x="90" y="138" fill="#aaa" font-size="9">Hit area = 44 × 44 pt (MinimumHitTestRadius × 2)</text>
  <text x="90" y="152" fill="#aaa" font-size="9">Visual icon = 16 × 16 pt</text>
  <text x="90" y="166" fill="#aaa" font-size="9">Hit area is transparent, extends symmetrically</text>

  <!-- Annotation lines -->
  <line x1="28"  y1="140" x2="28"  y2="170" stroke="#4040ff" stroke-width=".5" stroke-dasharray="2,2"/>
  <line x1="72"  y1="140" x2="72"  y2="170" stroke="#4040ff" stroke-width=".5" stroke-dasharray="2,2"/>
  <line x1="44"  y1="185" x2="44"  y2="170" stroke="#aaa" stroke-width=".5"/>
  <line x1="60"  y1="185" x2="60"  y2="170" stroke="#aaa" stroke-width=".5"/>
  <line x1="28"  y1="175" x2="72"  y2="175" stroke="#4040ff" stroke-width=".8"/>
  <text x="50"   y="188"  fill="#4040ff" text-anchor="middle" font-size="8">44 pt</text>
  <line x1="44"  y1="180" x2="60"  y2="180" stroke="#aaa" stroke-width=".8"/>
  <text x="52"   y="192"  fill="#aaa" text-anchor="middle" font-size="7">16 pt</text>
</svg>

---

### 5.4 Navigation Patterns

#### Bottom navigation (mobile) vs Navigation rail (desktop/tablet)

<svg viewBox="0 0 560 300" xmlns="http://www.w3.org/2000/svg" font-family="Inter,system-ui,sans-serif" font-size="11">
  <!-- Mobile -->
  <rect x="10" y="10" width="160" height="280" rx="16" fill="#fafafa" stroke="#ddd" stroke-width="1.5"/>
  <!-- Content area placeholder -->
  <rect x="18" y="18" width="144" height="210" rx="4" fill="#f5f5f5"/>
  <text x="90" y="120" fill="#ccc" text-anchor="middle" font-size="9">Content</text>
  <!-- Bottom nav -->
  <rect x="10" y="228" width="160" height="62" rx="0" fill="#fff" stroke="#eee" stroke-width="1"/>
  <rect x="10" y="252" width="160" height="38" rx="0" fill="#fff"/>
  <!-- Icons -->
  <text x="40"  y="248" fill="#4040ff" text-anchor="middle" font-size="16">⌂</text>
  <text x="90"  y="248" fill="#aaa"    text-anchor="middle" font-size="16">♪</text>
  <text x="140" y="248" fill="#aaa"    text-anchor="middle" font-size="16">☰</text>
  <!-- Indicator pill -->
  <rect x="22" y="252" width="36" height="4" rx="2" fill="#4040ff"/>
  <!-- Labels -->
  <text x="40"  y="268" fill="#4040ff" text-anchor="middle" font-size="8" font-weight="600">Home</text>
  <text x="90"  y="268" fill="#aaa"    text-anchor="middle" font-size="8">Explore</text>
  <text x="140" y="268" fill="#aaa"    text-anchor="middle" font-size="8">Library</text>
  <text x="90"  y="294" fill="#888"    text-anchor="middle" font-size="8">Bottom nav — DeviceIdiom.Mobile</text>

  <!-- Tablet -->
  <rect x="196" y="10" width="220" height="280" rx="10" fill="#fafafa" stroke="#ddd" stroke-width="1.5"/>
  <!-- Left rail -->
  <rect x="196" y="10" width="72" height="280" rx="10" fill="#fff" stroke="#eee" stroke-width="1"/>
  <!-- Active pill -->
  <rect x="202" y="46" width="60" height="36" rx="18" fill="#e0e0ff"/>
  <text x="232" y="68" fill="#1a1aff" text-anchor="middle" font-size="14">⌂</text>
  <!-- Other items -->
  <text x="232" y="108" fill="#aaa"    text-anchor="middle" font-size="14">♪</text>
  <text x="232" y="148" fill="#aaa"    text-anchor="middle" font-size="14">☰</text>
  <text x="232" y="188" fill="#aaa"    text-anchor="middle" font-size="14">⚙</text>
  <!-- Content area -->
  <rect x="276" y="18" width="130" height="264" rx="4" fill="#f5f5f5"/>
  <text x="341" y="150" fill="#ccc" text-anchor="middle" font-size="9">Content</text>
  <text x="306" y="294" fill="#888" text-anchor="middle" font-size="8">Nav rail — DeviceIdiom.Tablet</text>

  <!-- Desktop -->
  <rect x="440" y="10" width="110" height="280" rx="6" fill="#fafafa" stroke="#ddd" stroke-width="1.5"/>
  <rect x="440" y="10" width="34" height="280" rx="6" fill="#fff" stroke="#eee" stroke-width="1"/>
  <!-- Expanded rail with labels -->
  <rect x="443" y="46" width="28" height="28" rx="14" fill="#e0e0ff"/>
  <text x="457" y="64" fill="#1a1aff" text-anchor="middle" font-size="11">⌂</text>
  <text x="457" y="88"  fill="#aaa" text-anchor="middle" font-size="11">♪</text>
  <text x="457" y="116" fill="#aaa" text-anchor="middle" font-size="11">☰</text>
  <text x="457" y="144" fill="#aaa" text-anchor="middle" font-size="11">⚙</text>
  <!-- Content -->
  <rect x="482" y="18" width="60" height="264" rx="3" fill="#f5f5f5"/>
  <text x="512" y="150" fill="#ccc" text-anchor="middle" font-size="8">Content</text>
  <text x="495" y="294" fill="#888" text-anchor="middle" font-size="8">Desktop</text>
</svg>

---

### 5.5 Data Visualization Color Series

<svg viewBox="0 0 480 60" xmlns="http://www.w3.org/2000/svg" font-family="Inter,system-ui,sans-serif" font-size="9">
  <rect width="480" height="60" fill="#fafafa" rx="6"/>
  <text x="12" y="14" fill="#444" font-size="10" font-weight="600">DataViz palette — 8 series colors</text>
  <rect x="12"  y="22" width="44" height="28" rx="4" fill="#4040ff"/>
  <rect x="62"  y="22" width="44" height="28" rx="4" fill="#1DB954"/>
  <rect x="112" y="22" width="44" height="28" rx="4" fill="#ff7a1a"/>
  <rect x="162" y="22" width="44" height="28" rx="4" fill="#e03060"/>
  <rect x="212" y="22" width="44" height="28" rx="4" fill="#9b59b6"/>
  <rect x="262" y="22" width="44" height="28" rx="4" fill="#00bcd4"/>
  <rect x="312" y="22" width="44" height="28" rx="4" fill="#f0c040"/>
  <rect x="362" y="22" width="44" height="28" rx="4" fill="#795548"/>
  <text x="34"  y="58" fill="#666" text-anchor="middle">D[0]</text>
  <text x="84"  y="58" fill="#666" text-anchor="middle">D[1]</text>
  <text x="134" y="58" fill="#666" text-anchor="middle">D[2]</text>
  <text x="184" y="58" fill="#666" text-anchor="middle">D[3]</text>
  <text x="234" y="58" fill="#666" text-anchor="middle">D[4]</text>
  <text x="284" y="58" fill="#666" text-anchor="middle">D[5]</text>
  <text x="334" y="58" fill="#666" text-anchor="middle">D[6]</text>
  <text x="384" y="58" fill="#666" text-anchor="middle">D[7]</text>
</svg>

---

### 5.6 Full Component Anatomy

<svg viewBox="0 0 480 340" xmlns="http://www.w3.org/2000/svg" font-family="Inter,system-ui,sans-serif" font-size="10">
  <rect width="480" height="340" fill="#f8f8f8" rx="8"/>
  <text x="16" y="18" fill="#333" font-size="11" font-weight="700">Widget anatomy — design system token flow</text>

  <!-- Card -->
  <rect x="16" y="28" width="200" height="140" rx="12" fill="#fff" stroke="#e0e0e0" stroke-width="1.5"/>
  <!-- Card content -->
  <text x="28" y="52" fill="#1a1aff" font-size="9" font-weight="600">IColorSystem.Surface → fill</text>
  <text x="28" y="66" fill="#888"    font-size="8">IShapeSystem.Large → rx="12"</text>
  <!-- Title -->
  <rect x="28" y="74" width="100" height="12" rx="2" fill="#e8e8ff"/>
  <text x="28" y="85"  fill="#333" font-size="9">Typography.TitleMedium</text>
  <!-- Body -->
  <rect x="28" y="92"  width="170" height="8" rx="2" fill="#f0f0f0"/>
  <rect x="28" y="104" width="140" height="8" rx="2" fill="#f0f0f0"/>
  <!-- Button -->
  <rect x="28" y="120" width="60" height="28" rx="14" fill="#4040ff"/>
  <text x="58" y="139" fill="#fff" text-anchor="middle" font-weight="600" font-size="9">Action</text>
  <text x="28" y="158" fill="#999" font-size="7">IColorSystem.Primary → fill</text>

  <!-- Arrows / token labels on right -->
  <line x1="220" y1="38" x2="250" y2="38" stroke="#4040ff" stroke-width=".8" marker-end="url(#arrow)"/>
  <text x="256" y="42" fill="#4040ff" font-size="9">Shape.Large (12 × RoundnessFactor)</text>

  <line x1="220" y1="70" x2="250" y2="70" stroke="#1DB954" stroke-width=".8"/>
  <text x="256" y="74" fill="#1DB954" font-size="9">Colors.Surface</text>

  <line x1="220" y1="100" x2="250" y2="100" stroke="#888" stroke-width=".8"/>
  <text x="256" y="104" fill="#888" font-size="9">Typography.BodyMedium</text>

  <line x1="220" y1="134" x2="250" y2="134" stroke="#ff7a1a" stroke-width=".8"/>
  <text x="256" y="138" fill="#ff7a1a" font-size="9">Colors.Primary (button fill)</text>

  <!-- Spacing diagram -->
  <text x="16" y="188" fill="#333" font-size="10" font-weight="600">Spacing token usage in a list row</text>
  <rect x="16" y="196" width="440" height="56" rx="4" fill="#fff" stroke="#e0e0e0" stroke-width="1"/>
  <!-- H padding indicators -->
  <rect x="16" y="196" width="16" height="56" rx="0" fill="#4040ff" opacity=".12"/>
  <rect x="440" y="196" width="16" height="56" rx="0" fill="#4040ff" opacity=".12"/>
  <text x="24"  y="226" fill="#4040ff" font-size="7" text-anchor="middle">L</text>
  <text x="448" y="226" fill="#4040ff" font-size="7" text-anchor="middle">L</text>
  <!-- V padding indicators -->
  <rect x="16" y="196" width="440" height="8" rx="0" fill="#1DB954" opacity=".12"/>
  <rect x="16" y="244" width="440" height="8" rx="0" fill="#1DB954" opacity=".12"/>
  <text x="230" y="203" fill="#1DB954" font-size="7" text-anchor="middle">S</text>
  <text x="230" y="253" fill="#1DB954" font-size="7" text-anchor="middle">S</text>
  <!-- Icon -->
  <rect x="40" y="212" width="24" height="24" rx="12" fill="#e0e0ff"/>
  <text x="52" y="229" fill="#4040ff" text-anchor="middle" font-size="12">♪</text>
  <!-- Text content -->
  <text x="74" y="222" fill="#222" font-size="11" font-weight="600">Song Title</text>
  <text x="74" y="236" fill="#888" font-size="9">Artist Name</text>
  <!-- Gap indicator -->
  <rect x="64" y="218" width="10" height="12" rx="0" fill="#ff7a1a" opacity=".2"/>
  <text x="69" y="226" fill="#ff7a1a" font-size="7" text-anchor="middle">S</text>

  <!-- Legend -->
  <rect x="16"  y="264" width="10" height="10" rx="1" fill="#4040ff" opacity=".3"/>
  <text x="30"  y="273" fill="#333" font-size="8">Spacing.L (16 pt) — horizontal padding</text>
  <rect x="16"  y="278" width="10" height="10" rx="1" fill="#1DB954" opacity=".3"/>
  <text x="30"  y="287" fill="#333" font-size="8">Spacing.S  (8 pt) — vertical padding</text>
  <rect x="16"  y="292" width="10" height="10" rx="1" fill="#ff7a1a" opacity=".3"/>
  <text x="30"  y="301" fill="#333" font-size="8">Spacing.S  (8 pt) — gap between icon and text</text>
  <text x="16"  y="316" fill="#555" font-size="8">MinimumHitTestRadius expands the transparent touch area of the icon to 44 pt (mobile) or 8 pt (desktop)</text>
  <text x="16"  y="330" fill="#555" font-size="8">without changing its visual size or the row's layout.</text>
</svg>

---

## 6. Integration with the DI System

Xui's DI resolves services by walking the view parent chain. `IDesignSystem` follows the same path:

```
View
  └─ GetService<IDesignSystem>() → Parent.GetService<IDesignSystem>()
       └─ RootView → Window.GetService<IDesignSystem>()
            └─ Abstract.Window → Context (app-level DI)
                 └─ IServiceProvider → registered IDesignSystem implementation
```

### 6.1 Registration

```csharp
// In the application host builder:
builder.Services.AddSingleton<IDesignSystem>(new XuiDesignSystem(
    primaryHue: 240f,          // blue
    secondaryHue: 120f,        // green
    roundnessFactor: 1.0f,
    motionPreference: MotionPreference.Curve,
    colorScheme: ColorScheme.Light
));
```

### 6.2 Widget Consumption

```csharp
// In a widget's OnAttach:
protected override void OnAttach()
{
    var ds = GetService<IDesignSystem>()
        ?? throw new InvalidOperationException("IDesignSystem is required.");

    // Read tokens into cached fields for use during Render and Measure:
    _background      = ds.Colors.Surface;
    _foreground      = ds.Colors.OnSurface;
    _cornerRadius    = ds.Shape.Large;
    _padding         = new Frame(ds.Spacing.L, ds.Spacing.S);
    _titleStyle      = ds.Typography.TitleMedium;
    _hitRadius       = ds.Device.MinimumHitTestRadius;
    _enterAnimation  = ds.Motion.EmphasizedDecelerate;

    base.OnAttach();
}

// In Render:
public override void Render(IContext context)
{
    context.SetFill(_background);
    context.BeginPath();
    context.RoundRect(Frame, _cornerRadius);
    context.Fill();
    // ... render content
}
```

### 6.3 Theme Change Propagation

When the system-level color scheme changes (e.g. dark mode toggle), the platform adapter fires a
`IDesignSystem.Changed` event. The root window invalidates the entire view tree, causing every widget
to re-query during the next draw pass — or widgets subscribe to `Changed` and re-read only their
relevant tokens.

```csharp
public interface IDesignSystem
{
    // ... (existing members)

    /// <summary>Raised when any design token has changed (e.g. dark mode toggle, font scale change).</summary>
    event Action? Changed;
}
```

---

## 7. Color Theory Guide

### 7.1 Choosing Seed Colors

Given one primary brand hue `H` (0–360°), the following schemes are derivable:

```
Complementary       → Secondary = H + 180°
Analogous           → Secondary = H + 30°,  Tertiary  = H + 60°
Split-complementary → Secondary = H + 150°, Tertiary  = H + 210°
Triadic             → Secondary = H + 120°, Tertiary  = H + 240°
Tetradic / Square   → Secondary = H + 90°,  Tertiary  = H + 180°, Quaternary = H + 270°
```

### 7.2 From Hue to Full Palette

```
For each seed hue H:
  1. Define a chroma level C (e.g. 0.3–0.5 in OKLCH, or 60–90 % in HSL)
  2. Generate 13 tonal stops: tone ∈ { 0, 5, 10, 20, 30, 40, 50, 60, 70, 80, 90, 95, 99, 100 }
     In OKLCH:  L = tone / 100 (perceptual lightness), C = chroma × sin(π × tone/100), H = seedHue
     In HSL:    L = tone/100,   S = chroma × (1 - |2L - 1|) (no chroma at extremes)
  3. Map named color roles to specific tonal stops (light scheme / dark scheme)
```

### 7.3 Accessibility Contrast Check

Before using a foreground/background pair, assert:

```
contrastRatio(fg, bg) ≥ 4.5  for normal text  (WCAG AA)
contrastRatio(fg, bg) ≥ 7.0  for normal text  (WCAG AAA)
contrastRatio(fg, bg) ≥ 3.0  for large text or UI components
```

Where:
```
relativeLuminance(c) = (c.R ≤ 0.04045 ? c.R/12.92 : ((c.R+0.055)/1.055)^2.4 * 0.2126)
                     + (… G … * 0.7152)
                     + (… B … * 0.0722)
contrastRatio(fg, bg) = (L_lighter + 0.05) / (L_darker + 0.05)
```

`IColorSystem` should expose a helper `nfloat ContrastRatio(Color foreground, Color background)`.

---

## 8. Roadmap and Next Steps

| Phase | Deliverable |
|---|---|
| **Phase 1** (this doc) | Design system specification, interfaces, token categories |
| **Phase 2** | Implement `Xui.Core.Design` project with all interfaces and a default `XuiDesignSystem` |
| **Phase 3** | Port existing `Border`, `TextBox`, `Label` to consume `IDesignSystem` tokens |
| **Phase 4** | Build `Button` (Filled / Tonal / Outlined / Text) and `ButtonGroup` widgets |
| **Phase 5** | Navigation primitives: `BottomNavBar`, `NavigationRail`, `NavigationDrawer` |
| **Phase 6** | Data visualization palette integration (chart series colors) |
| **Phase 7** | Platform adapters expose `IDeviceInfo` (scale, pointer model, accessibility scale) |
| **Phase 8** | SVG snapshot tests for each widget in all states using the Software renderer |
| **Phase 9** | Demo app (TestApp) adopts design system; BlankApp shows system customization |

---

## Appendix A: Token Reference Quick-Sheet

```
IDesignSystem
├── Colors: IColorSystem
│   ├── Background, OnBackground
│   ├── Surface, OnSurface, SurfaceVariant, OnSurfaceVariant
│   ├── Outline, OutlineVariant
│   ├── Primary, OnPrimary, PrimaryContainer, OnPrimaryContainer
│   ├── Secondary, OnSecondary, SecondaryContainer, OnSecondaryContainer
│   ├── Accent, OnAccent
│   ├── Error, OnError, ErrorContainer, OnErrorContainer
│   ├── FocusRing
│   ├── DataVizPalette (span of 8+)
│   ├── IsDark
│   └── GetTone(hue, saturation, tone) → Color
│
├── Typography: ITypographySystem
│   ├── Display, HeadlineLarge/Medium/Small
│   ├── TitleLarge/Medium/Small
│   ├── BodyLarge/Medium/Small
│   ├── LabelLarge/Medium/Small
│   └── DefaultFontFamily
│
├── Spacing: ISpacingSystem
│   └── None(0) XXS(2) XS(4) S(8) M(12) L(16) XL(24) XXL(32) XXXL(48)
│
├── Shape: IShapeSystem
│   ├── RoundnessFactor
│   └── None ExtraSmall Small Medium Large ExtraLarge Full
│
├── Motion: IMotionSystem
│   ├── Preference (Curve | Spring), ReducedMotion
│   ├── EmphasizedDecelerate, EmphasizedAccelerate, Standard, …  (CurveToken)
│   └── SpringBouncy, SpringResponsive, SpringSmooth              (SpringToken)
│
├── Icons: IIconSystem
│   ├── GetIcon(name) → IDrawable?
│   └── GetIcon(name, size) → IDrawable?
│
└── Device: IDeviceInfo
    ├── Idiom (Mobile|Tablet|Desktop|Car|TV|Watch)
    ├── PointerModel (Touch|Stylus|Mouse|Controller|Eye)
    ├── Scale (physical px / logical pt)
    ├── MinimumHitTestRadius
    ├── AccessibilityFontScale
    ├── PrefersReducedMotion
    ├── PrefersHighContrast
    └── ColorScheme (Light | Dark)
```
