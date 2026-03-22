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

#### 3.1.2 `Color.Oklch` — Perceptual Color Space and Ramps

Interpolating between two colors straight in sRGB produces muddy, de-saturated midpoints. The
solution is to work in a **perceptual color space** first. Xui extends `Color` with a nested
`Color.Oklch` value type representing the standard **OKLCH** color space (Oklab-based Lightness,
Chroma, Hue — the same space used by CSS Color Level 4 and Material Design 3's HCT):

- **Lightness** (0.0–1.0) — perceptual lightness (0 = black, 1 = white)
- **Chroma** (0.0–~0.4) — colorfulness; 0 = neutral/gray, higher = fully saturated
- **Hue** (0–360 °) — perceptual hue angle on the color wheel

Conversion to/from `Xui.Core.Canvas.Color` is implicit, so existing APIs are unaffected. The key
addition is `Color.Oklch.Ramp` — a struct that **binds two `Color.Oklch` endpoints** and evaluates to
any intermediate color via `[t]` indexing, exactly like how `Xui.Core.Curves2D` interpolates points
along a Bezier curve.

```csharp
// In Xui.Core.Canvas (extends the existing Color struct):
public partial struct Color
{
    /// <summary>
    /// OKLCH perceptual color space (Oklab-based Lightness, Chroma, Hue).
    /// Interpolation in OKLCH produces vivid, perceptually uniform color transitions.
    /// See: https://bottosson.github.io/posts/oklab/
    /// </summary>
    public readonly struct Oklch
    {
        /// <summary>Perceptual lightness (0.0 = black, 1.0 = white).</summary>
        public nfloat Lightness { get; init; }

        /// <summary>Colorfulness (0.0 = neutral gray; typical max ~0.37 for sRGB gamut).</summary>
        public nfloat Chroma    { get; init; }

        /// <summary>Hue angle in degrees (0–360).</summary>
        public nfloat Hue       { get; init; }

        /// <summary>Converts an sRGB <see cref="Color"/> to OKLCH.</summary>
        public Oklch(Color color) { /* sRGB → linear sRGB → Oklab → OKLCH */ }

        /// <summary>Converts this OKLCH value back to sRGB <see cref="Color"/>.</summary>
        public Color ToColor() { /* OKLCH → Oklab → linear sRGB → sRGB */ }

        public static implicit operator Color(Oklch oklch)   => oklch.ToColor();
        public static implicit operator Oklch(Color color) => new Oklch(color);

        /// <summary>
        /// Creates a <see cref="Ramp"/> between two Oklch colors.
        /// Hue interpolation follows the shortest arc on the color wheel.
        /// </summary>
        public static Ramp Between(Oklch from, Oklch to) => new Ramp(from, to);

        /// <summary>
        /// A pair of Oklch endpoints that can be evaluated at any position t ∈ [0, 1].
        /// Analogous to how <c>Xui.Core.Curves2D</c> evaluates a point on a Bezier curve.
        /// t = 0 returns From; t = 1 returns To.
        /// </summary>
        public readonly struct Ramp
        {
            public Oklch From { get; init; }
            public Oklch To   { get; init; }

            public Ramp(Oklch from, Oklch to) { From = from; To = to; }

            /// <summary>Evaluates the ramp at position t, returning a sRGB Color.</summary>
            public Color this[nfloat t] => Lerp(From, To, t).ToColor();

            /// <summary>Lerps two Oklch values along the shortest hue arc.</summary>
            public static Oklch Lerp(Oklch from, Oklch to, nfloat t)
            {
                nfloat dHue = to.Hue - from.Hue;
                if (dHue >  180) dHue -= 360;
                if (dHue < -180) dHue += 360;
                return new Oklch
                {
                    Lightness = from.Lightness + (to.Lightness - from.Lightness) * t,
                    Chroma    = from.Chroma    + (to.Chroma    - from.Chroma)    * t,
                    Hue       = from.Hue       + dHue                            * t,
                };
            }
        }
    }
}
```

**Generating a tonal ramp** for any hue is then one expression:

```csharp
// Full tonal ramp for hue 240 ° (blue), chroma 0.3:
var blueRamp = Color.Oklch.Between(
    new Color.Oklch { Lightness = 0.0f, Chroma = 0.3f, Hue = 240 },  // L=0 → near black
    new Color.Oklch { Lightness = 1.0f, Chroma = 0.0f, Hue = 240 }   // L=1 → near white
);

Color primary40 = blueRamp[0.40f];  // Filled button fill    (light mode)
Color primary80 = blueRamp[0.80f];  // Filled button fill    (dark mode)
Color primary90 = blueRamp[0.90f];  // Tonal button fill     (light mode)
Color primary30 = blueRamp[0.30f];  // Tonal button fill     (dark mode)
```

`IColorSystem.GetTonalRamp(nfloat hueDegrees, nfloat chroma)` returns a pre-built `Color.Oklch.Ramp`
for that hue. `ColorGroup` (see below) exposes the ramp for each semantic role so widgets can build
hover/press overlays without hard-coding any color values.

---

#### 3.1.3 `ColorGroup` — Semantic Four-Color Bundle

Traditional design tokens expose `Primary`, `OnPrimary`, `PrimaryContainer`, and
`OnPrimaryContainer` as four independent properties. This is verbose and makes the relationship
between them opaque. Xui wraps them into a **`ColorGroup`** — a single struct with four named roles
and the underlying `Ramp`:

```
Background   ←→  Foreground    (strong pair — use for filled elements)
Container    ←→  OnContainer   (light pair  — use for tinted/highlighted elements)
```

| Role | Light-mode tonal stop | Dark-mode tonal stop | Typical use |
|---|---|---|---|
| `Background` | Hue ramp @ 0.40 | Hue ramp @ 0.80 | Filled button fill, active tab indicator |
| `Foreground` | Hue ramp @ 1.00 | Hue ramp @ 0.20 | Label inside a filled button or active icon |
| `Container` | Hue ramp @ 0.90 | Hue ramp @ 0.30 | Tonal button fill, chip, selected segment |
| `OnContainer` | Hue ramp @ 0.10 | Hue ramp @ 0.90 | Label inside a chip or tonal button |

**Why two pairs?**

- **`Background + Foreground`** → high-contrast, saturated pair. Use when the element *is* the call
  to action: a filled primary button, the active indicator dot in a nav rail.
- **`Container + OnContainer`** → lower-contrast, tinted pair. Use when the element *indicates* a
  selected or important state without screaming: a tonal button in a button group, an active chip,
  a highlighted list row.

**`Application` and `Surface` groups** follow the same pattern but draw from the **Neutral** ramp:

| Group | Background | Foreground | Container | OnContainer | Purpose |
|---|---|---|---|---|---|
| `Application` | Neutral 0.99 / 0.06 | Neutral 0.10 / 0.90 | Neutral 0.98 / 0.12 | Neutral 0.10 / 0.90 | Window canvas & body text |
| `Surface` | Neutral 0.98 / 0.12 | Neutral 0.10 / 0.90 | Neutral 0.90 / 0.30 | Neutral 0.30 / 0.80 | Card / panel fill & text |

`Application.Background` is the window/screen canvas. `Surface.Background` is a card resting on
that canvas.  `Surface.Container` is a slightly differentiated alternate fill (e.g. alternating table
rows, a hover highlight on a list item).

<svg viewBox="0 0 520 200" xmlns="http://www.w3.org/2000/svg" font-family="Inter,system-ui,sans-serif" font-size="10">
  <rect width="520" height="200" fill="#f0f0f0" rx="8"/>
  <text x="12" y="16" fill="#333" font-size="10" font-weight="700">ColorGroup — four-color anatomy (light mode, Primary group)</text>

  <!-- Background swatch -->
  <rect x="12" y="26" width="110" height="54" rx="6" fill="#4040ff"/>
  <text x="67" y="48" fill="#fff" text-anchor="middle" font-size="9" font-weight="600">Background</text>
  <text x="67" y="62" fill="#d0d0ff" text-anchor="middle" font-size="8">Primary ramp @ 0.40</text>
  <text x="67" y="74" fill="#d0d0ff" text-anchor="middle" font-size="8">→ filled button fill</text>

  <!-- Foreground swatch -->
  <rect x="132" y="26" width="110" height="54" rx="6" fill="#ffffff" stroke="#e0e0e0" stroke-width="1"/>
  <text x="187" y="44" fill="#4040ff" text-anchor="middle" font-size="9" font-weight="600">Foreground</text>
  <text x="187" y="58" fill="#888" text-anchor="middle" font-size="8">Primary ramp @ 1.00</text>
  <text x="187" y="70" fill="#888" text-anchor="middle" font-size="8">→ label on filled button</text>

  <!-- Connector -->
  <line x1="122" y1="53" x2="132" y2="53" stroke="#4040ff" stroke-width="1.5" stroke-dasharray="3,2"/>
  <text x="127" y="49" fill="#4040ff" text-anchor="middle" font-size="8">on</text>

  <!-- Container swatch -->
  <rect x="12" y="96" width="110" height="54" rx="6" fill="#e0e0ff"/>
  <text x="67" y="118" fill="#1a1aff" text-anchor="middle" font-size="9" font-weight="600">Container</text>
  <text x="67" y="132" fill="#4040aa" text-anchor="middle" font-size="8">Primary ramp @ 0.90</text>
  <text x="67" y="144" fill="#4040aa" text-anchor="middle" font-size="8">→ tonal button / chip</text>

  <!-- OnContainer swatch -->
  <rect x="132" y="96" width="110" height="54" rx="6" fill="#0a0a60"/>
  <text x="187" y="118" fill="#e0e0ff" text-anchor="middle" font-size="9" font-weight="600">OnContainer</text>
  <text x="187" y="132" fill="#9090cc" text-anchor="middle" font-size="8">Primary ramp @ 0.10</text>
  <text x="187" y="144" fill="#9090cc" text-anchor="middle" font-size="8">→ label on chip</text>

  <!-- Connector -->
  <line x1="122" y1="123" x2="132" y2="123" stroke="#4040ff" stroke-width="1.5" stroke-dasharray="3,2"/>
  <text x="127" y="119" fill="#4040ff" text-anchor="middle" font-size="8">on</text>

  <!-- Ramp strip -->
  <text x="260" y="38" fill="#555" font-size="9" font-weight="600">Underlying OKLCH Ramp (hue 240°)</text>
  <rect x="260" y="44" width="22" height="100" rx="2" fill="#000080"/>
  <rect x="284" y="44" width="22" height="100" rx="2" fill="#0000cd"/>
  <rect x="308" y="44" width="22" height="100" rx="2" fill="#1a1aff"/>
  <rect x="332" y="44" width="22" height="100" rx="2" fill="#4040ff"/>
  <rect x="356" y="44" width="22" height="100" rx="2" fill="#8080ff"/>
  <rect x="380" y="44" width="22" height="100" rx="2" fill="#b3b3ff"/>
  <rect x="404" y="44" width="22" height="100" rx="2" fill="#e0e0ff"/>
  <rect x="428" y="44" width="22" height="100" rx="2" fill="#f8f8ff"/>
  <!-- tone markers -->
  <line x1="332" y1="44"  x2="332" y2="32"  stroke="#4040ff" stroke-width=".8"/>
  <text x="332" y="30"  fill="#4040ff" text-anchor="middle" font-size="8">0.40 → Background</text>
  <line x1="404" y1="44"  x2="460" y2="158" stroke="#aaa" stroke-width=".8"/>
  <text x="462" y="162" fill="#aaa" font-size="8">0.90 → Container</text>
  <line x1="260" y1="44"  x2="248" y2="158" stroke="#333" stroke-width=".8"/>
  <text x="150" y="162" fill="#333" font-size="8">0.10 → OnContainer  |  1.00 → Foreground</text>

  <!-- Ramp index arrows -->
  <text x="260" y="158" fill="#777" font-size="8">t=0</text>
  <text x="442" y="158" fill="#777" font-size="8">t=1</text>
  <text x="350" y="166" fill="#555" text-anchor="middle" font-size="8">Color.Oklch.Between(L=0, L=1)[t]</text>

  <!-- Usage summary box -->
  <rect x="260" y="172" width="248" height="24" rx="4" fill="#fffbe6" stroke="#f0c000" stroke-width=".8"/>
  <text x="268" y="186" fill="#7a6000" font-size="8">Primary.Background → filled btn  ·  Primary.Container → chip / tonal btn</text>
</svg>

> **Implementation note**: `IColorSystem.GetTonalRamp(hue, chroma)` returns a `Color.Oklch.Ramp`.
> Each `ColorGroup` also exposes this ramp directly as a `Ramp` property, so widgets can build
> hover/pressed overlays via `Primary.Ramp[pressedLightness]` without any hardcoded color values.

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
/// A group of four semantically related colors derived from a single tonal palette,
/// together with the underlying Oklch ramp that generated them.
/// </summary>
public readonly struct ColorGroup
{
    /// <summary>
    /// Strong, saturated action color (ramp @ tone 0.40 light / 0.80 dark).
    /// Use as fill for filled buttons, active indicators, primary UI elements.
    /// </summary>
    public Color Background  { get; init; }

    /// <summary>
    /// High-contrast text/icon color on top of Background (ramp @ tone 1.00 light / 0.20 dark).
    /// </summary>
    public Color Foreground  { get; init; }

    /// <summary>
    /// Light tinted fill from the same palette (ramp @ tone 0.90 light / 0.30 dark).
    /// Use for tonal buttons, chips, selected segment items, highlighted list rows.
    /// </summary>
    public Color Container   { get; init; }

    /// <summary>
    /// Text/icon color on top of Container (ramp @ tone 0.10 light / 0.90 dark).
    /// </summary>
    public Color OnContainer { get; init; }

    /// <summary>
    /// The full tonal ramp for this palette entry (Lightness 0 → 1 at the group's hue).
    /// Background is at Ramp[0.40f] in light mode / Ramp[0.80f] in dark mode.
    /// Use IColorSystem.IsDark to pick the correct base Lightness, then offset:
    ///   hover   = Ramp[baseLightness + 0.08f]
    ///   pressed = Ramp[baseLightness - 0.06f]
    /// </summary>
    public Color.Oklch.Ramp Ramp { get; init; }
}

/// <summary>
/// Provides color roles derived from a seed palette, grouped into semantic <see cref="ColorGroup"/>
/// bundles and a set of neutral/structural colors.
/// </summary>
public interface IColorSystem
{
    // -- Whole-application canvas (from the Neutral ramp) --

    /// <summary>
    /// The application canvas color group.
    /// Background = window/screen fill · Foreground = body text
    /// Container  = card/panel fill    · OnContainer = card text
    /// </summary>
    ColorGroup Application { get; }

    /// <summary>
    /// The surface (card/panel) color group, slightly elevated from Application.
    /// Background = card fill  · Foreground = card body text
    /// Container  = alternate surface (e.g. alternating table row, hover bg)
    /// OnContainer = secondary text on alternate surface
    /// </summary>
    ColorGroup Surface { get; }

    // -- Borders and dividers (from Neutral ramp at mid-tones) --
    Color Outline        { get; }   // Neutral tone 0.50 light / 0.60 dark
    Color OutlineVariant { get; }   // Neutral tone 0.80 light / 0.30 dark

    // -- Semantic action groups --

    /// <summary>Brand / primary action group (from the Primary tonal ramp).</summary>
    ColorGroup Primary   { get; }

    /// <summary>Supporting / secondary action group (from the Secondary tonal ramp).</summary>
    ColorGroup Secondary { get; }

    /// <summary>Tertiary highlight / accent group (from the Accent tonal ramp).</summary>
    ColorGroup Accent    { get; }

    /// <summary>Error / destructive state group (from the Error tonal ramp).</summary>
    ColorGroup Error     { get; }

    // -- Focus ring (typically Accent.Background at full opacity) --
    Color FocusRing { get; }

    // -- Data-visualization series colors (at least 8, perceptually distinct) --
    ReadOnlySpan<Color> DataVizPalette { get; }

    /// <summary>
    /// Returns a full tonal ramp for any hue/chroma combination.
    /// Use to build custom ColorGroups or hover/press state colors.
    /// </summary>
    Color.Oklch.Ramp GetTonalRamp(nfloat hueDegrees, nfloat chroma);

    /// <summary>True if the current effective color scheme is dark.</summary>
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

Three **importance levels** map to different `ColorGroup` properties:

| Level | Fill | Text | Border | Use |
|---|---|---|---|---|
| `FilledButton` | `Primary.Background` | `Primary.Foreground` | none | Primary CTA |
| `TonalButton` | `Primary.Container` | `Primary.OnContainer` | none | Secondary CTA |
| `OutlinedButton` | transparent | `Primary.Background` | `Outline` | Tertiary CTA |
| `TextButton` | transparent | `Primary.Background` | none | Low-prominence action |

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
  <text x="76"  y="268" fill="#003a00" font-size="8">■ Primary.Container</text>
  <text x="176" y="268" fill="#aaa"    font-size="8">■ Disabled state</text>
</svg>

#### 5.1.1 Button Group (Segment Control)

Three or more buttons sharing a border, with one marked `IsActive`. The active button uses
`Primary.Container` fill + `Primary.OnContainer` text; inactive buttons use `Surface.Container`:

<svg viewBox="0 0 400 120" xmlns="http://www.w3.org/2000/svg" font-family="Inter,system-ui,sans-serif" font-size="12">
  <rect width="400" height="120" fill="#fafafa" rx="8"/>
  <text x="16" y="20" fill="#444" font-size="10" font-weight="600">Button Group (Segment Control)</text>

  <!-- Group border -->
  <rect x="16" y="32" width="234" height="40" rx="20" fill="none" stroke="#c0c0c0" stroke-width="1.5"/>
  <!-- Inactive left -->
  <rect x="16" y="32" width="78" height="40" rx="20" fill="#f0f0f0"/>
  <text x="55" y="57" fill="#444" text-anchor="middle" font-size="12">All</text>
  <!-- Active middle (Primary.Container) -->
  <rect x="94" y="32" width="78" height="40" rx="0" fill="#e0e0ff"/>
  <text x="133" y="57" fill="#1a1aff" text-anchor="middle" font-size="12" font-weight="600">Music</text>
  <!-- Inactive right -->
  <rect x="172" y="32" width="78" height="40" rx="20" fill="#f0f0f0"/>
  <text x="211" y="57" fill="#444" text-anchor="middle" font-size="12">Podcasts</text>

  <!-- Annotation -->
  <line x1="133" y1="80" x2="133" y2="94" stroke="#666" stroke-width=".8"/>
  <text x="133" y="105" fill="#666" text-anchor="middle" font-size="9">IsActive → Primary.Container + Primary.OnContainer</text>

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
_borderLayer.CornerRadius    = ds.Shape.Small;
_borderLayer.BorderColor     = ds.Colors.Outline;
_borderLayer.BackgroundColor = ds.Colors.Surface.Background;
_borderLayer.Padding         = new Frame(ds.Spacing.M, ds.Spacing.S);
_labelStyle                  = ds.Typography.BodyMedium;
_focusColor                  = ds.Colors.FocusRing;
_errorColor                  = ds.Colors.Error.Background;
_errorBgColor                = ds.Colors.Error.Container;
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
  <text x="16" y="182" fill="#aaa" font-size="8">BorderColor → Colors.Error.Background · Helper → Colors.Error.OnContainer</text>

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

### 5.5 Cards — `Surface` ColorGroup in Action

Cards are the primary container that elevates content above the `Application.Background`. They use
`Surface.Background` as fill. Buttons inside the card use `Primary.Background`.

<svg viewBox="0 0 540 320" xmlns="http://www.w3.org/2000/svg" font-family="Inter,system-ui,sans-serif" font-size="11">
  <rect width="540" height="320" fill="#f0f0f0" rx="8"/>
  <text x="14" y="16" fill="#333" font-size="10" font-weight="700">Card variants — Surface.Background / Application.Background relationship</text>

  <!-- App background -->
  <rect x="14" y="24" width="512" height="288" rx="6" fill="#f8f8f8" stroke="#e0e0e0" stroke-width="1"/>
  <text x="26" y="40" fill="#999" font-size="8">Application.Background (Neutral 0.99)</text>

  <!-- Plain card -->
  <rect x="26" y="48" width="150" height="180" rx="12" fill="#ffffff" stroke="#ececec" stroke-width="1"/>
  <text x="40" y="68" fill="#888" font-size="8">Surface.Background</text>
  <!-- image placeholder -->
  <rect x="36" y="76" width="130" height="70" rx="6" fill="#e8e8ff"/>
  <text x="101" y="116" fill="#9090cc" text-anchor="middle" font-size="18">♪</text>
  <!-- text -->
  <text x="36" y="162" fill="#222" font-size="11" font-weight="600">Song Title</text>
  <text x="36" y="177" fill="#888" font-size="9">Artist Name</text>
  <!-- button -->
  <rect x="36" y="188" width="80" height="28" rx="14" fill="#4040ff"/>
  <text x="76" y="206" fill="#fff" text-anchor="middle" font-size="9" font-weight="600">▶  Play</text>
  <text x="26" y="240" fill="#4040ff" font-size="7">Primary.Background → btn fill</text>

  <!-- Tonal/elevated card -->
  <rect x="192" y="48" width="150" height="180" rx="12" fill="#eeeeff" stroke="#d8d8ff" stroke-width="1"/>
  <text x="206" y="68" fill="#6666cc" font-size="8">Surface.Container (selected/active)</text>
  <rect x="202" y="76" width="130" height="70" rx="6" fill="#d8d8ff"/>
  <text x="267" y="116" fill="#4040aa" text-anchor="middle" font-size="18">♪</text>
  <text x="202" y="162" fill="#1a1aff" font-size="11" font-weight="600">Song Title</text>
  <text x="202" y="177" fill="#6666aa" font-size="9">Artist Name</text>
  <rect x="202" y="188" width="80" height="28" rx="14" fill="#0a0a60"/>
  <text x="242" y="206" fill="#e0e0ff" text-anchor="middle" font-size="9" font-weight="600">▶  Play</text>
  <text x="192" y="240" fill="#4040ff" font-size="7">Primary.OnContainer → btn fill (on tinted surface)</text>

  <!-- Error/warning card -->
  <rect x="358" y="48" width="150" height="140" rx="12" fill="#fff8f8" stroke="#ffcccc" stroke-width="1.5"/>
  <text x="372" y="68" fill="#cc0000" font-size="8">Error.Container</text>
  <text x="372" y="86" fill="#880000" font-size="9" font-weight="600">⚠  Upload failed</text>
  <text x="372" y="101" fill="#aa4444" font-size="8">File too large (max 10 MB).</text>
  <text x="372" y="114" fill="#aa4444" font-size="8">Try compressing it first.</text>
  <rect x="372" y="124" width="70" height="24" rx="12" fill="#cc0000"/>
  <text x="407" y="140" fill="#fff" text-anchor="middle" font-size="8" font-weight="600">Retry</text>
  <rect x="448" y="124" width="50" height="24" rx="12" fill="none" stroke="#cc0000" stroke-width="1.2"/>
  <text x="473" y="140" fill="#cc0000" text-anchor="middle" font-size="8">Dismiss</text>
  <text x="358" y="205" fill="#cc0000" font-size="7">Error.Container bg · Error.Background btn · Error.Foreground label</text>

  <!-- Token guide strip -->
  <rect x="14" y="272" width="512" height="36" rx="4" fill="#fffbe6" stroke="#f0c000" stroke-width=".8"/>
  <text x="22" y="284" fill="#7a6000" font-size="8" font-weight="600">ColorGroup usage guide:</text>
  <text x="22" y="296" fill="#7a6000" font-size="8">Application.Background = screen canvas  ·  Surface.Background = card  ·  Surface.Container = highlighted card  ·  Primary.Background = action button</text>
  <text x="22" y="308" fill="#7a6000" font-size="8">Primary.Container = tonal chip/badge  ·  Error.Container = error card bg  ·  Error.Background = destructive button fill</text>
</svg>

---

### 5.6 Form Layout — Token Flow Across Grouped Inputs

A form groups labels, inputs, and helper/error text. Every element resolves its color from the same
`IDesignSystem` instance queried once in `OnAttach`.

<svg viewBox="0 0 520 432" xmlns="http://www.w3.org/2000/svg" font-family="Inter,system-ui,sans-serif" font-size="11">
  <rect width="520" height="432" fill="#fafafa" rx="8"/>
  <text x="14" y="16" fill="#333" font-size="10" font-weight="700">Form layout — design system token flow per element</text>

  <!-- Labels -->
  <text x="14" y="38" fill="#333" font-size="9" font-weight="600">Full name</text>
  <text x="280" y="38" fill="#333" font-size="9" font-weight="600">Email address</text>

  <!-- Input row 1 -->
  <rect x="14" y="44" width="244" height="36" rx="4" fill="#fff" stroke="#b0b0b0" stroke-width="1.2"/>
  <text x="26" y="67" fill="#222" font-size="11">Jane Appleseed</text>
  <rect x="268" y="44" width="238" height="36" rx="4" fill="#fff" stroke="#4040ff" stroke-width="2"/>
  <text x="280" y="67" fill="#222" font-size="11">jane@example.com</text>
  <!-- focus cursor -->
  <line x1="374" y1="52" x2="374" y2="72" stroke="#4040ff" stroke-width="1.5"/>

  <!-- Token annotations row 1 -->
  <text x="14"  y="92" fill="#999" font-size="8">Outline border  ·  Surface.Background fill  ·  Application.Foreground text</text>
  <text x="268" y="92" fill="#4040ff" font-size="8">FocusRing border  ·  Accent.Background cursor</text>

  <!-- Password row -->
  <text x="14" y="112" fill="#333" font-size="9" font-weight="600">Password</text>
  <rect x="14" y="118" width="492" height="36" rx="4" fill="#fff" stroke="#b0b0b0" stroke-width="1.2"/>
  <text x="26" y="141" fill="#222" font-size="11">●●●●●●●●</text>
  <text x="480" y="141" fill="#888" font-size="11">👁</text>
  <text x="14" y="164" fill="#999" font-size="8">Outline border  ·  Surface.Background fill  ·  Application.Foreground placeholder  ·  icon from IIconSystem</text>

  <!-- Error field -->
  <text x="14" y="184" fill="#333" font-size="9" font-weight="600">Phone number</text>
  <rect x="14" y="190" width="492" height="36" rx="4" fill="#fff8f8" stroke="#cc0000" stroke-width="1.5"/>
  <text x="26" y="213" fill="#222" font-size="11">+1 (555) 000-BAD</text>
  <text x="14" y="238" fill="#cc0000" font-size="8">⚠  Please enter a valid phone number</text>
  <text x="14" y="250" fill="#999" font-size="8">Error.Background border  ·  Error.Container background  ·  Error.Foreground helper text</text>

  <!-- Divider -->
  <line x1="14" y1="262" x2="506" y2="262" stroke="#e0e0e0" stroke-width=".8"/>
  <text x="14" y="276" fill="#555" font-size="9" font-weight="600">Notifications</text>

  <!-- Toggle row 1 (on) -->
  <text x="14" y="298" fill="#222" font-size="10">Email notifications</text>
  <text x="14" y="311" fill="#888" font-size="8">Receive updates about activity</text>
  <!-- Toggle ON -->
  <rect x="454" y="288" width="44" height="24" rx="12" fill="#4040ff"/>
  <circle cx="466" cy="300" r="9" fill="#fff"/>
  <text x="480" y="316" fill="#4040ff" font-size="8" text-anchor="middle">ON</text>

  <!-- Toggle row 2 (off) -->
  <text x="14" y="335" fill="#222" font-size="10">Push notifications</text>
  <text x="14" y="348" fill="#888" font-size="8">Require device permission</text>
  <!-- Toggle OFF -->
  <rect x="454" y="325" width="44" height="24" rx="12" fill="#c0c0c0"/>
  <circle cx="490" cy="337" r="9" fill="#fff"/>
  <text x="476" y="353" fill="#888" font-size="8" text-anchor="middle">OFF</text>

  <!-- Token annotations toggle -->
  <text x="14" y="368" fill="#4040ff" font-size="8">ON: Primary.Background track  ·  Surface.Background thumb</text>
  <text x="14" y="380" fill="#888"    font-size="8">OFF: Surface.Container track  ·  Surface.Background thumb</text>

  <!-- Submit button -->
  <rect x="14" y="386" width="120" height="36" rx="18" fill="#4040ff"/>
  <text x="74" y="409" fill="#fff" text-anchor="middle" font-size="11" font-weight="600">Save changes</text>
</svg>

---

### 5.7 Toggle / Switch — State via ColorGroup

The toggle (on/off switch) demonstrates how a single `ColorGroup` drives all visual states:

| State | Track fill | Thumb fill |
|---|---|---|
| Off | `Surface.Container` | `Surface.Background` |
| Off + hover | `Surface.Container` + lightened via `Surface.Ramp` | `Surface.Background` |
| On | `Primary.Background` | `Primary.Foreground` |
| On + hover | `Primary.Ramp[0.48f]` | `Primary.Foreground` |
| Disabled | `OutlineVariant` | `Surface.Background` |

<svg viewBox="0 0 480 160" xmlns="http://www.w3.org/2000/svg" font-family="Inter,system-ui,sans-serif" font-size="11">
  <rect width="480" height="160" fill="#fafafa" rx="8"/>
  <text x="14" y="16" fill="#333" font-size="10" font-weight="700">Toggle states</text>

  <!-- OFF -->
  <text x="14" y="42" fill="#888" font-size="9">Off</text>
  <rect x="14" y="50" width="44" height="24" rx="12" fill="#c8c8c8"/>
  <circle cx="50" cy="62" r="9" fill="#fff"/>

  <!-- OFF hover -->
  <text x="72" y="42" fill="#888" font-size="9">Off hover</text>
  <rect x="72" y="50" width="44" height="24" rx="12" fill="#b8b8b8"/>
  <circle cx="108" cy="62" r="9" fill="#fff"/>

  <!-- ON -->
  <text x="130" y="42" fill="#888" font-size="9">On</text>
  <rect x="130" y="50" width="44" height="24" rx="12" fill="#4040ff"/>
  <circle cx="162" cy="62" r="9" fill="#fff"/>

  <!-- ON hover -->
  <text x="188" y="42" fill="#888" font-size="9">On hover</text>
  <rect x="188" y="50" width="44" height="24" rx="12" fill="#5555ff"/>
  <circle cx="220" cy="62" r="9" fill="#fff"/>

  <!-- Disabled OFF -->
  <text x="246" y="42" fill="#888" font-size="9">Disabled off</text>
  <rect x="246" y="50" width="44" height="24" rx="12" fill="#e8e8e8"/>
  <circle cx="282" cy="62" r="9" fill="#f8f8f8"/>

  <!-- Disabled ON -->
  <text x="304" y="42" fill="#888" font-size="9">Disabled on</text>
  <rect x="304" y="50" width="44" height="24" rx="12" fill="#b0b0f0"/>
  <circle cx="336" cy="62" r="9" fill="#f0f0ff"/>

  <!-- Dark mode row -->
  <rect x="14" y="90" width="450" height="58" rx="6" fill="#111"/>

  <text x="22" y="107" fill="#888" font-size="8">dark mode</text>
  <!-- OFF dark -->
  <rect x="22" y="112" width="44" height="24" rx="12" fill="#444"/>
  <circle cx="58" cy="124" r="9" fill="#999"/>
  <!-- ON dark -->
  <rect x="80" y="112" width="44" height="24" rx="12" fill="#9999ff"/>
  <circle cx="112" cy="124" r="9" fill="#1a1aff"/>

  <text x="140" y="120" fill="#555" font-size="8">OFF: Surface.Container(dark)  ·  thumb: Outline(dark)</text>
  <text x="140" y="134" fill="#9999ff" font-size="8">ON:  Primary.Background(dark)  ·  thumb: Primary.OnContainer(dark)</text>
</svg>

---

### 5.8 Chips / Tags — `Container` + `OnContainer`

Chips are the canonical use of `Container + OnContainer` within a `ColorGroup`. They show
classification, filter state, or attribute labels.

<svg viewBox="0 0 520 200" xmlns="http://www.w3.org/2000/svg" font-family="Inter,system-ui,sans-serif" font-size="11">
  <rect width="520" height="200" fill="#fafafa" rx="8"/>
  <text x="14" y="16" fill="#333" font-size="10" font-weight="700">Chip / Tag variants</text>

  <!-- Row 1: filter chips -->
  <text x="14" y="38" fill="#888" font-size="9">Filter chips</text>

  <!-- Primary container chip (selected) -->
  <rect x="14" y="46" width="72" height="28" rx="14" fill="#e0e0ff"/>
  <text x="50" y="64" fill="#1a1aff" text-anchor="middle" font-size="10" font-weight="600">✓  Music</text>

  <!-- Surface chip (unselected) -->
  <rect x="94" y="46" width="80" height="28" rx="14" fill="#f0f0f0" stroke="#c8c8c8" stroke-width="1"/>
  <text x="134" y="64" fill="#444" text-anchor="middle" font-size="10">Podcasts</text>

  <!-- Surface chip -->
  <rect x="182" y="46" width="72" height="28" rx="14" fill="#f0f0f0" stroke="#c8c8c8" stroke-width="1"/>
  <text x="218" y="64" fill="#444" text-anchor="middle" font-size="10">Video</text>

  <!-- Secondary container chip -->
  <rect x="262" y="46" width="76" height="28" rx="14" fill="#d9f9d9"/>
  <text x="300" y="64" fill="#003a00" text-anchor="middle" font-size="10" font-weight="600">✓  Live</text>

  <!-- Token note -->
  <text x="14" y="90" fill="#4040ff" font-size="8">Selected: Primary.Container bg · Primary.OnContainer text</text>
  <text x="14" y="102" fill="#888"   font-size="8">Unselected: Surface.Container bg · Application.Foreground text · Outline border</text>

  <!-- Row 2: label tags -->
  <text x="14" y="124" fill="#888" font-size="9">Status tags (read-only)</text>
  <rect x="14"  y="132" width="56" height="22" rx="4" fill="#d9f9d9"/>
  <text x="42"  y="147" fill="#003a00" text-anchor="middle" font-size="9" font-weight="600">Active</text>
  <rect x="78"  y="132" width="60" height="22" rx="4" fill="#fff0dd"/>
  <text x="108" y="147" fill="#7a4000" text-anchor="middle" font-size="9" font-weight="600">Pending</text>
  <rect x="146" y="132" width="56" height="22" rx="4" fill="#ffe0e0"/>
  <text x="174" y="147" fill="#880000" text-anchor="middle" font-size="9" font-weight="600">Error</text>
  <rect x="210" y="132" width="64" height="22" rx="4" fill="#e8e8ff"/>
  <text x="242" y="147" fill="#1a1aff" text-anchor="middle" font-size="9" font-weight="600">Featured</text>
  <rect x="282" y="132" width="56" height="22" rx="4" fill="#f0f0f0"/>
  <text x="310" y="147" fill="#666" text-anchor="middle" font-size="9">Inactive</text>

  <!-- Token note row 2 -->
  <text x="14" y="172" fill="#1DB954" font-size="8">Secondary.Container · Error.Container · Primary.Container (each with On-variant text)</text>
  <text x="14" y="184" fill="#888"   font-size="8">Small radius → Shape.Small  ·  Larger radius (pill) → Shape.Full</text>

  <!-- Chip with close icon -->
  <rect x="14"  y="160" width="96" height="28" rx="14" fill="#e0e0ff"/>
  <text x="50"  y="178" fill="#1a1aff" text-anchor="middle" font-size="10" font-weight="600">Music</text>
  <circle cx="99" cy="174" r="8" fill="#c0c0f0"/>
  <text x="99"  y="178" fill="#4040aa" text-anchor="middle" font-size="10">×</text>

  <rect x="118" y="160" width="96" height="28" rx="14" fill="#d9f9d9"/>
  <text x="154" y="178" fill="#003a00" text-anchor="middle" font-size="10" font-weight="600">Podcast</text>
  <circle cx="203" cy="174" r="8" fill="#b0ebb0"/>
  <text x="203" y="178" fill="#005000" text-anchor="middle" font-size="10">×</text>
</svg>

---

### 5.9 Dialog / Modal — Elevation and Surface Hierarchy

A dialog sits above the application layer. Its scrim uses `Application.Background` at reduced opacity;
the dialog surface uses `Surface.Background` elevated via a subtle shadow.

<svg viewBox="0 0 520 300" xmlns="http://www.w3.org/2000/svg" font-family="Inter,system-ui,sans-serif" font-size="11">
  <rect width="520" height="300" fill="#e8e8e8" rx="8"/>

  <!-- App content (dimmed) -->
  <rect x="0" y="0" width="520" height="300" fill="#fafafa" rx="8" opacity=".3"/>

  <!-- Scrim -->
  <rect x="0" y="0" width="520" height="300" fill="#000" opacity=".35" rx="8"/>
  <text x="260" y="290" fill="#ffffff" text-anchor="middle" font-size="8" opacity=".5">Scrim: Application.Background @ 35% opacity</text>

  <!-- Dialog surface -->
  <filter id="shadow"><feDropShadow dx="0" dy="4" stdDeviation="8" flood-color="#00000044"/></filter>
  <rect x="110" y="40" width="300" height="216" rx="16" fill="#ffffff" filter="url(#shadow)"/>

  <!-- Dialog header -->
  <text x="130" y="72" fill="#111" font-size="13" font-weight="600">Discard changes?</text>

  <!-- Dialog body -->
  <text x="130" y="94"  fill="#555" font-size="10">You have unsaved changes. Leaving this</text>
  <text x="130" y="108" fill="#555" font-size="10">page will discard them permanently.</text>

  <!-- Divider -->
  <line x1="110" y1="124" x2="410" y2="124" stroke="#e8e8e8" stroke-width=".8"/>

  <!-- Checkbox row -->
  <rect x="130" y="132" width="14" height="14" rx="3" fill="none" stroke="#b0b0b0" stroke-width="1.5"/>
  <text x="152" y="144" fill="#555" font-size="10">Don't ask me again</text>

  <!-- Action row -->
  <rect x="130" y="174" width="100" height="34" rx="17" fill="none" stroke="#4040ff" stroke-width="1.5"/>
  <text x="180" y="196" fill="#4040ff" text-anchor="middle" font-size="11" font-weight="600">Keep editing</text>

  <rect x="244" y="174" width="100" height="34" rx="17" fill="#4040ff"/>
  <text x="294" y="196" fill="#fff" text-anchor="middle" font-size="11" font-weight="600">Discard</text>

  <!-- Token labels -->
  <text x="130" y="230" fill="#999" font-size="8">Surface.Background fill  ·  Shape.ExtraLarge radius  ·  elevation via shadow</text>
  <text x="130" y="242" fill="#4040ff" font-size="8">Primary.Background filled btn  ·  Primary.Background outlined btn</text>
</svg>

---

### 5.10 List Rows — Density and ColorGroup in Context

List rows use `Application.Background` for the container and `Surface.Container` for hover/selection
highlights. Density adapts via `Spacing` tokens driven by `DeviceIdiom`.

<svg viewBox="0 0 520 300" xmlns="http://www.w3.org/2000/svg" font-family="Inter,system-ui,sans-serif" font-size="11">
  <rect width="520" height="300" fill="#fafafa" rx="8"/>
  <text x="14" y="16" fill="#333" font-size="10" font-weight="700">List row density — Mobile (comfortable) vs Desktop (compact)</text>

  <!-- === Mobile comfortable rows === -->
  <text x="14" y="34" fill="#888" font-size="9">Mobile — comfortable density (Spacing.L padding, 56 pt row height)</text>

  <!-- Row 1: default -->
  <rect x="14" y="40" width="220" height="52" rx="0" fill="#fff" stroke="#f0f0f0" stroke-width=".8"/>
  <rect x="24" y="52" width="28" height="28" rx="14" fill="#e0e0ff"/>
  <text x="38" y="70" fill="#4040ff" text-anchor="middle" font-size="14">♪</text>
  <text x="62" y="62" fill="#222" font-size="11" font-weight="600">Song Title</text>
  <text x="62" y="76" fill="#888" font-size="9">Artist · 3:24</text>
  <text x="218" y="66" fill="#bbb" text-anchor="end" font-size="14">⋮</text>

  <!-- Row 2: hover / selected -->
  <rect x="14" y="92" width="220" height="52" rx="0" fill="#eeeeff" stroke="#d0d0ff" stroke-width=".8"/>
  <rect x="24" y="104" width="28" height="28" rx="14" fill="#c0c0ff"/>
  <text x="38" y="122" fill="#1a1aff" text-anchor="middle" font-size="14">♪</text>
  <text x="62" y="114" fill="#1a1aff" font-size="11" font-weight="600">Playing Now</text>
  <text x="62" y="128" fill="#6666aa" font-size="9">Artist · 1:12 / 3:24</text>
  <text x="218" y="118" fill="#4040ff" text-anchor="end" font-size="14">⏸</text>
  <text x="14" y="156" fill="#4040ff" font-size="8">Selected: Surface.Container bg · Primary.Background icon</text>

  <!-- === Desktop compact rows === -->
  <text x="266" y="34" fill="#888" font-size="9">Desktop — compact density (Spacing.S padding, 28 pt row height)</text>

  <rect x="266" y="40" width="240" height="28" rx="0" fill="#fff" stroke="#f0f0f0" stroke-width=".8"/>
  <text x="278" y="59" fill="#444" font-size="11">♪</text>
  <text x="298" y="59" fill="#222" font-size="11">Song Title</text>
  <text x="440" y="59" fill="#888" text-anchor="end" font-size="9">Artist</text>
  <text x="494" y="59" fill="#888" text-anchor="end" font-size="9">3:24</text>

  <rect x="266" y="68" width="240" height="28" rx="0" fill="#eeeeff" stroke="#d0d0ff" stroke-width=".8"/>
  <text x="278" y="87" fill="#4040ff" font-size="11">▶</text>
  <text x="298" y="87" fill="#1a1aff" font-size="11" font-weight="600">Playing Now</text>
  <text x="440" y="87" fill="#6666aa" text-anchor="end" font-size="9">Artist</text>
  <text x="494" y="87" fill="#6666aa" text-anchor="end" font-size="9">1:12</text>

  <rect x="266" y="96" width="240" height="28" rx="0" fill="#fff" stroke="#f0f0f0" stroke-width=".8"/>
  <text x="278" y="115" fill="#444" font-size="11">♪</text>
  <text x="298" y="115" fill="#222" font-size="11">Another Song</text>
  <text x="440" y="115" fill="#888" text-anchor="end" font-size="9">Artist B</text>
  <text x="494" y="115" fill="#888" text-anchor="end" font-size="9">4:05</text>

  <text x="266" y="140" fill="#aaa" font-size="8">Row height: 52 pt (mobile) vs 28 pt (desktop)</text>
  <text x="266" y="152" fill="#aaa" font-size="8">Spacing.L H-padding (mobile) vs Spacing.S (desktop)</text>

  <!-- Divider -->
  <line x1="14" y1="168" x2="506" y2="168" stroke="#e8e8e8" stroke-width=".8"/>

  <!-- Token summary -->
  <text x="14"  y="186" fill="#333" font-size="9" font-weight="600">Token flow for a list row:</text>
  <text x="14"  y="200" fill="#888" font-size="8">background       = Application.Background (default) or Surface.Container (selected)</text>
  <text x="14"  y="212" fill="#888" font-size="8">primary label    = Application.Foreground  ·  secondary label = Surface.Foreground</text>
  <text x="14"  y="224" fill="#888" font-size="8">icon background  = Primary.Container       ·  icon tint      = Primary.OnContainer</text>
  <text x="14"  y="236" fill="#888" font-size="8">selected icon    = Primary.Background      ·  playing tint   = Primary.Background</text>
  <text x="14"  y="248" fill="#888" font-size="8">row height       = Spacing.XXXL (mobile) or Spacing.XXL (desktop) — driven by DeviceIdiom</text>
  <text x="14"  y="260" fill="#888" font-size="8">h-padding        = Spacing.L (mobile) or Spacing.S (desktop)</text>
</svg>

---

### 5.11 Data Visualization Color Series

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

### 5.12 Full Component Anatomy

<svg viewBox="0 0 480 340" xmlns="http://www.w3.org/2000/svg" font-family="Inter,system-ui,sans-serif" font-size="10">
  <rect width="480" height="340" fill="#f8f8f8" rx="8"/>
  <text x="16" y="18" fill="#333" font-size="11" font-weight="700">Widget anatomy — design system token flow</text>

  <!-- Card -->
  <rect x="16" y="28" width="200" height="140" rx="12" fill="#fff" stroke="#e0e0e0" stroke-width="1.5"/>
  <!-- Card content -->
  <text x="28" y="52" fill="#1a1aff" font-size="9" font-weight="600">Colors.Surface.Background → fill</text>
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
  <text x="28" y="158" fill="#999" font-size="7">IColorSystem.Primary.Background → fill</text>

  <!-- Arrows / token labels on right -->
  <line x1="220" y1="38" x2="250" y2="38" stroke="#4040ff" stroke-width=".8" marker-end="url(#arrow)"/>
  <text x="256" y="42" fill="#4040ff" font-size="9">Shape.Large (12 × RoundnessFactor)</text>

  <line x1="220" y1="70" x2="250" y2="70" stroke="#1DB954" stroke-width=".8"/>
  <text x="256" y="74" fill="#1DB954" font-size="9">Colors.Surface.Background</text>

  <line x1="220" y1="100" x2="250" y2="100" stroke="#888" stroke-width=".8"/>
  <text x="256" y="104" fill="#888" font-size="9">Typography.BodyMedium</text>

  <line x1="220" y1="134" x2="250" y2="134" stroke="#ff7a1a" stroke-width=".8"/>
  <text x="256" y="138" fill="#ff7a1a" font-size="9">Colors.Primary.Background (button fill)</text>

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
    _background      = ds.Colors.Surface.Background;
    _foreground      = ds.Colors.Surface.Foreground;
    _cornerRadius    = ds.Shape.Large;
    _padding         = new Frame(ds.Spacing.L, ds.Spacing.S);
    _titleStyle      = ds.Typography.TitleMedium;
    _hitRadius       = ds.Device.MinimumHitTestRadius;
    _enterAnimation  = ds.Motion.EmphasizedDecelerate;
    _btnFill         = ds.Colors.Primary.Background;
    _btnText         = ds.Colors.Primary.Foreground;
    _btnHover        = ds.Colors.Primary.Ramp[0.48f]; // Background lightness (0.40) + hover offset (0.08)

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

### 7.2 From Hue to Full Palette using `Color.Oklch.Ramp`

```
For each seed hue H:
  1. Define a chroma level C (e.g. 0.2–0.35 in OKLCH — typical sRGB-safe range)
  2. Build a Color.Oklch.Ramp from (L=0, C=C, H=H) to (L=1, C=0, H=H)
     This single ramp covers all tonal stops via ramp[lightness]:
       L=0.00 → ramp[0.00f]   (near black)
       L=0.40 → ramp[0.40f]   → ColorGroup.Background (light mode)
       L=0.80 → ramp[0.80f]   → ColorGroup.Background (dark mode)
       L=0.90 → ramp[0.90f]   → ColorGroup.Container  (light mode)
       L=1.00 → ramp[1.00f]   → ColorGroup.Foreground (light mode, near white)
  3. Map named color roles to ramp positions (see ColorGroup above)
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
Color.Oklch (nested struct inside Xui.Core.Canvas.Color)
├── Lightness : nfloat  (0.0 black … 1.0 white)
├── Chroma    : nfloat  (0.0 gray  … ~0.37 max in sRGB)
├── Hue       : nfloat  (0–360 °)
├── implicit Color ↔ Oklch conversions
└── static Between(Oklch from, Oklch to) → Oklch.Ramp
         Oklch.Ramp[nfloat t] → Color     (t ∈ [0,1])

ColorGroup (struct in Xui.Core.Design)
├── Background  : Color  (strong action color — filled button fill)
├── Foreground  : Color  (contrast text on Background — button label)
├── Container   : Color  (light tinted fill — chip / tonal button)
├── OnContainer : Color  (text on Container — chip label)
└── Ramp        : Color.Oklch.Ramp  (full tonal range for hover/press states)

IDesignSystem
├── Colors: IColorSystem
│   ├── Application: ColorGroup   (Background=window bg, Foreground=body text,
│   │                              Container=surface fill, OnContainer=card text)
│   ├── Surface: ColorGroup       (Background=card fill, Foreground=card text,
│   │                              Container=alt surface, OnContainer=secondary text)
│   ├── Outline, OutlineVariant   (neutral borders)
│   ├── Primary:   ColorGroup     (Background=filled btn, Foreground=btn label,
│   │                              Container=tonal btn / chip, OnContainer=chip label)
│   ├── Secondary: ColorGroup     (supporting action, same four-color structure)
│   ├── Accent:    ColorGroup     (tertiary highlight)
│   ├── Error:     ColorGroup     (destructive / error state)
│   ├── FocusRing
│   ├── DataVizPalette (span of 8+)
│   ├── IsDark
│   └── GetTonalRamp(hue, chroma) → Color.Oklch.Ramp
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
