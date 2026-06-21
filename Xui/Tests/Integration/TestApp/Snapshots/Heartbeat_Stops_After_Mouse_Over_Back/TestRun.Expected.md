# Integration Test Run

## Snapshot summary

- ❌ 01. Home
- ✅ 02. Heart.Rest
- ✅ 03. Heart.PrimaryPeak
- ✅ 04. MouseOverBack
- ✅ 05. Heart.Rest.AfterMouseOver
- ✅ 06. Heart.PrimaryPeak.AfterMouseOver

## Timeline

## Scenario

Verify that heart animation keeps ticking after pointer hover on Back.

### 01. Home

Status: ❌ Diff

#### Expected

<div style="overflow:auto; border:1px solid #ddd; padding:8px; background:#fff;">
<svg xmlns="http://www.w3.org/2000/svg" width="600" height="400" viewBox="0 0 600 400">
  <text x="0" y="0" fill="#000000" text-anchor="start" dominant-baseline="text-before-edge" font-size="24" font-family="Inter" font-weight="Xui.Core.Canvas.FontWeight" xml:space="preserve">Xui SDK Examples </text>
  <text x="3" y="32.04" fill="#000000" text-anchor="start" dominant-baseline="text-before-edge" font-size="15" font-family="Inter" xml:space="preserve">TextMetrics</text>
  <text x="3" y="56.19" fill="#000000" text-anchor="start" dominant-baseline="text-before-edge" font-size="15" font-family="Inter" xml:space="preserve">Text Layout</text>
  <text x="3" y="80.34" fill="#000000" text-anchor="start" dominant-baseline="text-before-edge" font-size="15" font-family="Inter" xml:space="preserve">Nested Stacks</text>
  <text x="3" y="104.49" fill="#000000" text-anchor="start" dominant-baseline="text-before-edge" font-size="15" font-family="Inter" xml:space="preserve">Grid Layout</text>
  <text x="3" y="128.64" fill="#000000" text-anchor="start" dominant-baseline="text-before-edge" font-size="15" font-family="Inter" xml:space="preserve">ViewCollection Alignment</text>
  <text x="3" y="152.79" fill="#000000" text-anchor="start" dominant-baseline="text-before-edge" font-size="15" font-family="Inter" xml:space="preserve">Animated Heart</text>
  <text x="3" y="176.94" fill="#000000" text-anchor="start" dominant-baseline="text-before-edge" font-size="15" font-family="Inter" xml:space="preserve">TextBox MVP</text>
  <text x="3" y="201.08" fill="#000000" text-anchor="start" dominant-baseline="text-before-edge" font-size="15" font-family="Inter" xml:space="preserve">Canvas Tests</text>
  <text x="3" y="225.23" fill="#000000" text-anchor="start" dominant-baseline="text-before-edge" font-size="15" font-family="Inter" xml:space="preserve">Layers</text>
  <text x="3" y="249.38" fill="#000000" text-anchor="start" dominant-baseline="text-before-edge" font-size="15" font-family="Inter" xml:space="preserve">3D GPU Rendering</text>
</svg>
</div>

#### Actual

<div style="overflow:auto; border:1px solid #ddd; padding:8px; background:#fff;">
<svg xmlns="http://www.w3.org/2000/svg" width="600" height="400" viewBox="0 0 600 400">
  <text x="0" y="0" fill="#000000" text-anchor="start" dominant-baseline="text-before-edge" font-size="24" font-family="Inter" font-weight="Xui.Core.Canvas.FontWeight" xml:space="preserve">Xui SDK Examples </text>
  <text x="3" y="32.04" fill="#000000" text-anchor="start" dominant-baseline="text-before-edge" font-size="15" font-family="Inter" xml:space="preserve">TextMetrics</text>
  <text x="3" y="56.19" fill="#000000" text-anchor="start" dominant-baseline="text-before-edge" font-size="15" font-family="Inter" xml:space="preserve">Text Layout</text>
  <text x="3" y="80.34" fill="#000000" text-anchor="start" dominant-baseline="text-before-edge" font-size="15" font-family="Inter" xml:space="preserve">Nested Stacks</text>
  <text x="3" y="104.49" fill="#000000" text-anchor="start" dominant-baseline="text-before-edge" font-size="15" font-family="Inter" xml:space="preserve">Grid Layout</text>
  <text x="3" y="128.64" fill="#000000" text-anchor="start" dominant-baseline="text-before-edge" font-size="15" font-family="Inter" xml:space="preserve">ViewCollection Alignment</text>
  <text x="3" y="152.79" fill="#000000" text-anchor="start" dominant-baseline="text-before-edge" font-size="15" font-family="Inter" xml:space="preserve">Animated Heart</text>
  <text x="3" y="176.94" fill="#000000" text-anchor="start" dominant-baseline="text-before-edge" font-size="15" font-family="Inter" xml:space="preserve">TextBox MVP</text>
  <text x="3" y="201.08" fill="#000000" text-anchor="start" dominant-baseline="text-before-edge" font-size="15" font-family="Inter" xml:space="preserve">Canvas Tests</text>
  <text x="3" y="225.23" fill="#000000" text-anchor="start" dominant-baseline="text-before-edge" font-size="15" font-family="Inter" xml:space="preserve">Layers</text>
  <text x="3" y="249.38" fill="#000000" text-anchor="start" dominant-baseline="text-before-edge" font-size="15" font-family="Inter" xml:space="preserve">3D GPU Rendering</text>
  <text x="3" y="273.53" fill="#000000" text-anchor="start" dominant-baseline="text-before-edge" font-size="15" font-family="Inter" xml:space="preserve">Design System</text>
</svg>
</div>

### 02. Heart.Rest

Status: ✅ Match

#### Expected

<div style="overflow:auto; border:1px solid #ddd; padding:8px; background:#fff;">
<svg xmlns="http://www.w3.org/2000/svg" width="600" height="400" viewBox="0 0 600 400">
  <defs>
    <linearGradient id="grad1" x1="228" y1="10" x2="372" y2="190" gradientUnits="userSpaceOnUse">
      <stop offset="0%" stop-color="#D642CD" />
      <stop offset="100%" stop-color="#8A05FF" />
    </linearGradient>
  </defs>
  <g transform="translate(300,100)">
    <g>
      <path d="M 0 -20.25 C 40.5 -76.95, 97.2 -4.05, 0 76.95 C -97.2 -4.05, -40.5 -76.95, 0 -20.25 Z " fill="url(#grad1)" fill-rule="nonzero" />
      <path d="M 0 -20.25 C 40.5 -76.95, 97.2 -4.05, 0 76.95 C -97.2 -4.05, -40.5 -76.95, 0 -20.25 Z " fill="none" stroke="#FFFFFF" stroke-width="2.5" />
    </g>
  </g>
  <rect x="0" y="0" width="600" height="24" fill="#FFFFFF" />
  <text x="0" y="0" fill="#000000" text-anchor="start" dominant-baseline="text-before-edge" font-size="15" font-family="Inter" xml:space="preserve">&lt; Back</text>
  <text x="73.18" y="0" fill="#000000" text-anchor="start" dominant-baseline="text-before-edge" font-size="15" font-family="Inter" xml:space="preserve">Animated Heart</text>
  <g transform="translate(300 161.86083984375)" opacity="0.9">
    <polygon points="0,0 0,12 3,9 5,12 7,11 5,8 9,8" fill="white" stroke="black" stroke-width="0.7"/>
  </g>
</svg>
</div>

#### Actual

<div style="overflow:auto; border:1px solid #ddd; padding:8px; background:#fff;">
<svg xmlns="http://www.w3.org/2000/svg" width="600" height="400" viewBox="0 0 600 400">
  <defs>
    <linearGradient id="grad1" x1="228" y1="10" x2="372" y2="190" gradientUnits="userSpaceOnUse">
      <stop offset="0%" stop-color="#D642CD" />
      <stop offset="100%" stop-color="#8A05FF" />
    </linearGradient>
  </defs>
  <g transform="translate(300,100)">
    <g>
      <path d="M 0 -20.25 C 40.5 -76.95, 97.2 -4.05, 0 76.95 C -97.2 -4.05, -40.5 -76.95, 0 -20.25 Z " fill="url(#grad1)" fill-rule="nonzero" />
      <path d="M 0 -20.25 C 40.5 -76.95, 97.2 -4.05, 0 76.95 C -97.2 -4.05, -40.5 -76.95, 0 -20.25 Z " fill="none" stroke="#FFFFFF" stroke-width="2.5" />
    </g>
  </g>
  <rect x="0" y="0" width="600" height="24" fill="#FFFFFF" />
  <text x="0" y="0" fill="#000000" text-anchor="start" dominant-baseline="text-before-edge" font-size="15" font-family="Inter" xml:space="preserve">&lt; Back</text>
  <text x="73.18" y="0" fill="#000000" text-anchor="start" dominant-baseline="text-before-edge" font-size="15" font-family="Inter" xml:space="preserve">Animated Heart</text>
  <g transform="translate(300 161.86083984375)" opacity="0.9">
    <polygon points="0,0 0,12 3,9 5,12 7,11 5,8 9,8" fill="white" stroke="black" stroke-width="0.7"/>
  </g>
</svg>
</div>

### 03. Heart.PrimaryPeak

Status: ✅ Match

#### Expected

<div style="overflow:auto; border:1px solid #ddd; padding:8px; background:#fff;">
<svg xmlns="http://www.w3.org/2000/svg" width="600" height="400" viewBox="0 0 600 400">
  <defs>
    <linearGradient id="grad1" x1="228" y1="10" x2="372" y2="190" gradientUnits="userSpaceOnUse">
      <stop offset="0%" stop-color="#D642CD" />
      <stop offset="100%" stop-color="#8A05FF" />
    </linearGradient>
  </defs>
  <g transform="translate(300,100)">
    <g transform="scale(1.13,1.13)">
      <path d="M 0 -20.25 C 40.5 -76.95, 97.2 -4.05, 0 76.95 C -97.2 -4.05, -40.5 -76.95, 0 -20.25 Z " fill="url(#grad1)" fill-rule="nonzero" />
      <path d="M 0 -20.25 C 40.5 -76.95, 97.2 -4.05, 0 76.95 C -97.2 -4.05, -40.5 -76.95, 0 -20.25 Z " fill="none" stroke="#FFFFFF" stroke-width="2.5" />
    </g>
  </g>
  <rect x="0" y="0" width="600" height="24" fill="#FFFFFF" />
  <text x="0" y="0" fill="#000000" text-anchor="start" dominant-baseline="text-before-edge" font-size="15" font-family="Inter" xml:space="preserve">&lt; Back</text>
  <text x="73.18" y="0" fill="#000000" text-anchor="start" dominant-baseline="text-before-edge" font-size="15" font-family="Inter" xml:space="preserve">Animated Heart</text>
  <g transform="translate(300 161.86083984375)" opacity="0.9">
    <polygon points="0,0 0,12 3,9 5,12 7,11 5,8 9,8" fill="white" stroke="black" stroke-width="0.7"/>
  </g>
</svg>
</div>

#### Actual

<div style="overflow:auto; border:1px solid #ddd; padding:8px; background:#fff;">
<svg xmlns="http://www.w3.org/2000/svg" width="600" height="400" viewBox="0 0 600 400">
  <defs>
    <linearGradient id="grad1" x1="228" y1="10" x2="372" y2="190" gradientUnits="userSpaceOnUse">
      <stop offset="0%" stop-color="#D642CD" />
      <stop offset="100%" stop-color="#8A05FF" />
    </linearGradient>
  </defs>
  <g transform="translate(300,100)">
    <g transform="scale(1.13,1.13)">
      <path d="M 0 -20.25 C 40.5 -76.95, 97.2 -4.05, 0 76.95 C -97.2 -4.05, -40.5 -76.95, 0 -20.25 Z " fill="url(#grad1)" fill-rule="nonzero" />
      <path d="M 0 -20.25 C 40.5 -76.95, 97.2 -4.05, 0 76.95 C -97.2 -4.05, -40.5 -76.95, 0 -20.25 Z " fill="none" stroke="#FFFFFF" stroke-width="2.5" />
    </g>
  </g>
  <rect x="0" y="0" width="600" height="24" fill="#FFFFFF" />
  <text x="0" y="0" fill="#000000" text-anchor="start" dominant-baseline="text-before-edge" font-size="15" font-family="Inter" xml:space="preserve">&lt; Back</text>
  <text x="73.18" y="0" fill="#000000" text-anchor="start" dominant-baseline="text-before-edge" font-size="15" font-family="Inter" xml:space="preserve">Animated Heart</text>
  <g transform="translate(300 161.86083984375)" opacity="0.9">
    <polygon points="0,0 0,12 3,9 5,12 7,11 5,8 9,8" fill="white" stroke="black" stroke-width="0.7"/>
  </g>
</svg>
</div>

### 04. MouseOverBack

Status: ✅ Match

#### Expected

<div style="overflow:auto; border:1px solid #ddd; padding:8px; background:#fff;">
<svg xmlns="http://www.w3.org/2000/svg" width="600" height="400" viewBox="0 0 600 400">
  <defs>
    <linearGradient id="grad1" x1="228" y1="10" x2="372" y2="190" gradientUnits="userSpaceOnUse">
      <stop offset="0%" stop-color="#D642CD" />
      <stop offset="100%" stop-color="#8A05FF" />
    </linearGradient>
  </defs>
  <g transform="translate(300,100)">
    <g transform="scale(1.13,1.13)">
      <path d="M 0 -20.25 C 40.5 -76.95, 97.2 -4.05, 0 76.95 C -97.2 -4.05, -40.5 -76.95, 0 -20.25 Z " fill="url(#grad1)" fill-rule="nonzero" />
      <path d="M 0 -20.25 C 40.5 -76.95, 97.2 -4.05, 0 76.95 C -97.2 -4.05, -40.5 -76.95, 0 -20.25 Z " fill="none" stroke="#FFFFFF" stroke-width="2.5" />
    </g>
  </g>
  <rect x="0" y="0" width="600" height="24" fill="#FFFFFF" />
  <rect x="0" y="0" width="49.18" height="24" fill="#D3D3D3" />
  <text x="0" y="0" fill="#000000" text-anchor="start" dominant-baseline="text-before-edge" font-size="15" font-family="Inter" xml:space="preserve">&lt; Back</text>
  <text x="73.18" y="0" fill="#000000" text-anchor="start" dominant-baseline="text-before-edge" font-size="15" font-family="Inter" xml:space="preserve">Animated Heart</text>
  <g transform="translate(24.591064453125 12)" opacity="0.9">
    <polygon points="0,0 0,12 3,9 5,12 7,11 5,8 9,8" fill="white" stroke="black" stroke-width="0.7"/>
  </g>
</svg>
</div>

#### Actual

<div style="overflow:auto; border:1px solid #ddd; padding:8px; background:#fff;">
<svg xmlns="http://www.w3.org/2000/svg" width="600" height="400" viewBox="0 0 600 400">
  <defs>
    <linearGradient id="grad1" x1="228" y1="10" x2="372" y2="190" gradientUnits="userSpaceOnUse">
      <stop offset="0%" stop-color="#D642CD" />
      <stop offset="100%" stop-color="#8A05FF" />
    </linearGradient>
  </defs>
  <g transform="translate(300,100)">
    <g transform="scale(1.13,1.13)">
      <path d="M 0 -20.25 C 40.5 -76.95, 97.2 -4.05, 0 76.95 C -97.2 -4.05, -40.5 -76.95, 0 -20.25 Z " fill="url(#grad1)" fill-rule="nonzero" />
      <path d="M 0 -20.25 C 40.5 -76.95, 97.2 -4.05, 0 76.95 C -97.2 -4.05, -40.5 -76.95, 0 -20.25 Z " fill="none" stroke="#FFFFFF" stroke-width="2.5" />
    </g>
  </g>
  <rect x="0" y="0" width="600" height="24" fill="#FFFFFF" />
  <rect x="0" y="0" width="49.18" height="24" fill="#D3D3D3" />
  <text x="0" y="0" fill="#000000" text-anchor="start" dominant-baseline="text-before-edge" font-size="15" font-family="Inter" xml:space="preserve">&lt; Back</text>
  <text x="73.18" y="0" fill="#000000" text-anchor="start" dominant-baseline="text-before-edge" font-size="15" font-family="Inter" xml:space="preserve">Animated Heart</text>
  <g transform="translate(24.591064453125 12)" opacity="0.9">
    <polygon points="0,0 0,12 3,9 5,12 7,11 5,8 9,8" fill="white" stroke="black" stroke-width="0.7"/>
  </g>
</svg>
</div>

### 05. Heart.Rest.AfterMouseOver

Status: ✅ Match

#### Expected

<div style="overflow:auto; border:1px solid #ddd; padding:8px; background:#fff;">
<svg xmlns="http://www.w3.org/2000/svg" width="600" height="400" viewBox="0 0 600 400">
  <defs>
    <linearGradient id="grad1" x1="228" y1="10" x2="372" y2="190" gradientUnits="userSpaceOnUse">
      <stop offset="0%" stop-color="#D642CD" />
      <stop offset="100%" stop-color="#8A05FF" />
    </linearGradient>
  </defs>
  <g transform="translate(300,100)">
    <g>
      <path d="M 0 -20.25 C 40.5 -76.95, 97.2 -4.05, 0 76.95 C -97.2 -4.05, -40.5 -76.95, 0 -20.25 Z " fill="url(#grad1)" fill-rule="nonzero" />
      <path d="M 0 -20.25 C 40.5 -76.95, 97.2 -4.05, 0 76.95 C -97.2 -4.05, -40.5 -76.95, 0 -20.25 Z " fill="none" stroke="#FFFFFF" stroke-width="2.5" />
    </g>
  </g>
  <rect x="0" y="0" width="600" height="24" fill="#FFFFFF" />
  <rect x="0" y="0" width="49.18" height="24" fill="#D3D3D3" />
  <text x="0" y="0" fill="#000000" text-anchor="start" dominant-baseline="text-before-edge" font-size="15" font-family="Inter" xml:space="preserve">&lt; Back</text>
  <text x="73.18" y="0" fill="#000000" text-anchor="start" dominant-baseline="text-before-edge" font-size="15" font-family="Inter" xml:space="preserve">Animated Heart</text>
  <g transform="translate(24.591064453125 12)" opacity="0.9">
    <polygon points="0,0 0,12 3,9 5,12 7,11 5,8 9,8" fill="white" stroke="black" stroke-width="0.7"/>
  </g>
</svg>
</div>

#### Actual

<div style="overflow:auto; border:1px solid #ddd; padding:8px; background:#fff;">
<svg xmlns="http://www.w3.org/2000/svg" width="600" height="400" viewBox="0 0 600 400">
  <defs>
    <linearGradient id="grad1" x1="228" y1="10" x2="372" y2="190" gradientUnits="userSpaceOnUse">
      <stop offset="0%" stop-color="#D642CD" />
      <stop offset="100%" stop-color="#8A05FF" />
    </linearGradient>
  </defs>
  <g transform="translate(300,100)">
    <g>
      <path d="M 0 -20.25 C 40.5 -76.95, 97.2 -4.05, 0 76.95 C -97.2 -4.05, -40.5 -76.95, 0 -20.25 Z " fill="url(#grad1)" fill-rule="nonzero" />
      <path d="M 0 -20.25 C 40.5 -76.95, 97.2 -4.05, 0 76.95 C -97.2 -4.05, -40.5 -76.95, 0 -20.25 Z " fill="none" stroke="#FFFFFF" stroke-width="2.5" />
    </g>
  </g>
  <rect x="0" y="0" width="600" height="24" fill="#FFFFFF" />
  <rect x="0" y="0" width="49.18" height="24" fill="#D3D3D3" />
  <text x="0" y="0" fill="#000000" text-anchor="start" dominant-baseline="text-before-edge" font-size="15" font-family="Inter" xml:space="preserve">&lt; Back</text>
  <text x="73.18" y="0" fill="#000000" text-anchor="start" dominant-baseline="text-before-edge" font-size="15" font-family="Inter" xml:space="preserve">Animated Heart</text>
  <g transform="translate(24.591064453125 12)" opacity="0.9">
    <polygon points="0,0 0,12 3,9 5,12 7,11 5,8 9,8" fill="white" stroke="black" stroke-width="0.7"/>
  </g>
</svg>
</div>

- Heart.Rest vs Heart.PrimaryPeak should differ before hover.
- Heart.Rest.AfterMouseOver vs Heart.PrimaryPeak.AfterMouseOver should also differ.

### 06. Heart.PrimaryPeak.AfterMouseOver

Status: ✅ Match

#### Expected

<div style="overflow:auto; border:1px solid #ddd; padding:8px; background:#fff;">
<svg xmlns="http://www.w3.org/2000/svg" width="600" height="400" viewBox="0 0 600 400">
  <defs>
    <linearGradient id="grad1" x1="228" y1="10" x2="372" y2="190" gradientUnits="userSpaceOnUse">
      <stop offset="0%" stop-color="#D642CD" />
      <stop offset="100%" stop-color="#8A05FF" />
    </linearGradient>
  </defs>
  <g transform="translate(300,100)">
    <g transform="scale(1.13,1.13)">
      <path d="M 0 -20.25 C 40.5 -76.95, 97.2 -4.05, 0 76.95 C -97.2 -4.05, -40.5 -76.95, 0 -20.25 Z " fill="url(#grad1)" fill-rule="nonzero" />
      <path d="M 0 -20.25 C 40.5 -76.95, 97.2 -4.05, 0 76.95 C -97.2 -4.05, -40.5 -76.95, 0 -20.25 Z " fill="none" stroke="#FFFFFF" stroke-width="2.5" />
    </g>
  </g>
  <rect x="0" y="0" width="600" height="24" fill="#FFFFFF" />
  <rect x="0" y="0" width="49.18" height="24" fill="#D3D3D3" />
  <text x="0" y="0" fill="#000000" text-anchor="start" dominant-baseline="text-before-edge" font-size="15" font-family="Inter" xml:space="preserve">&lt; Back</text>
  <text x="73.18" y="0" fill="#000000" text-anchor="start" dominant-baseline="text-before-edge" font-size="15" font-family="Inter" xml:space="preserve">Animated Heart</text>
  <g transform="translate(24.591064453125 12)" opacity="0.9">
    <polygon points="0,0 0,12 3,9 5,12 7,11 5,8 9,8" fill="white" stroke="black" stroke-width="0.7"/>
  </g>
</svg>
</div>

#### Actual

<div style="overflow:auto; border:1px solid #ddd; padding:8px; background:#fff;">
<svg xmlns="http://www.w3.org/2000/svg" width="600" height="400" viewBox="0 0 600 400">
  <defs>
    <linearGradient id="grad1" x1="228" y1="10" x2="372" y2="190" gradientUnits="userSpaceOnUse">
      <stop offset="0%" stop-color="#D642CD" />
      <stop offset="100%" stop-color="#8A05FF" />
    </linearGradient>
  </defs>
  <g transform="translate(300,100)">
    <g transform="scale(1.13,1.13)">
      <path d="M 0 -20.25 C 40.5 -76.95, 97.2 -4.05, 0 76.95 C -97.2 -4.05, -40.5 -76.95, 0 -20.25 Z " fill="url(#grad1)" fill-rule="nonzero" />
      <path d="M 0 -20.25 C 40.5 -76.95, 97.2 -4.05, 0 76.95 C -97.2 -4.05, -40.5 -76.95, 0 -20.25 Z " fill="none" stroke="#FFFFFF" stroke-width="2.5" />
    </g>
  </g>
  <rect x="0" y="0" width="600" height="24" fill="#FFFFFF" />
  <rect x="0" y="0" width="49.18" height="24" fill="#D3D3D3" />
  <text x="0" y="0" fill="#000000" text-anchor="start" dominant-baseline="text-before-edge" font-size="15" font-family="Inter" xml:space="preserve">&lt; Back</text>
  <text x="73.18" y="0" fill="#000000" text-anchor="start" dominant-baseline="text-before-edge" font-size="15" font-family="Inter" xml:space="preserve">Animated Heart</text>
  <g transform="translate(24.591064453125 12)" opacity="0.9">
    <polygon points="0,0 0,12 3,9 5,12 7,11 5,8 9,8" fill="white" stroke="black" stroke-width="0.7"/>
  </g>
</svg>
</div>
