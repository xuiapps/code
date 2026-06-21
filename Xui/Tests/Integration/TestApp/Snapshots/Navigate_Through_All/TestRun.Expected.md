# Integration Test Run

## Snapshot summary

- ❌ 01. HomePage
- ✅ 02. TextMetrics
- ✅ 03. TextLayout
- ✅ 04. NestedStacks
- ✅ 05. ViewCollectionAlignment
- ✅ 06. AnimatedHeart
- ✅ 07. TextBox

## Timeline

## Scenario

Visit each example page from home, snapshot it, then return to home.

### 01. HomePage

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

### Navigate: TextMetrics

### 02. TextMetrics

Status: ✅ Match

#### Expected

<div style="overflow:auto; border:1px solid #ddd; padding:8px; background:#fff;">
<svg xmlns="http://www.w3.org/2000/svg" width="600" height="400" viewBox="0 0 600 400">
  <text x="300" y="212" fill="#000000" text-anchor="middle" dominant-baseline="alphabetic" font-size="64" font-family="Inter" xml:space="preserve">Hello World!</text>
  <path d="M 100 150 L 500 150 " fill="none" stroke="#0000FF" stroke-width="2" />
  <path d="M 100 212 L 500 212 " fill="none" stroke="#000000" stroke-width="2" />
  <path d="M 100 227.44 L 500 227.44 " fill="none" stroke="#008000" stroke-width="2" />
  <rect x="115.69" y="150" width="363.63" height="77.44" fill="none" stroke="#008000" stroke-width="5" />
  <rect x="115.69" y="150" width="363.63" height="77.44" fill="none" stroke="#FFA500" stroke-width="3" />
  <rect x="115.69" y="165.44" width="363.63" height="46.56" fill="none" stroke="#FF0000" />
  <path d="M 295 212 L 305 212 M 300 207 L 300 217 " fill="none" stroke="#FF0000" stroke-width="3" />
  <rect x="0" y="0" width="600" height="24" fill="#FFFFFF" />
  <text x="0" y="0" fill="#000000" text-anchor="start" dominant-baseline="text-before-edge" font-size="15" font-family="Inter" xml:space="preserve">&lt; Back</text>
  <text x="73.18" y="0" fill="#000000" text-anchor="start" dominant-baseline="text-before-edge" font-size="15" font-family="Inter" xml:space="preserve">TextMetrics</text>
  <g transform="translate(300 41.11376953125)" opacity="0.9">
    <polygon points="0,0 0,12 3,9 5,12 7,11 5,8 9,8" fill="white" stroke="black" stroke-width="0.7"/>
  </g>
</svg>
</div>

#### Actual

<div style="overflow:auto; border:1px solid #ddd; padding:8px; background:#fff;">
<svg xmlns="http://www.w3.org/2000/svg" width="600" height="400" viewBox="0 0 600 400">
  <text x="300" y="212" fill="#000000" text-anchor="middle" dominant-baseline="alphabetic" font-size="64" font-family="Inter" xml:space="preserve">Hello World!</text>
  <path d="M 100 150 L 500 150 " fill="none" stroke="#0000FF" stroke-width="2" />
  <path d="M 100 212 L 500 212 " fill="none" stroke="#000000" stroke-width="2" />
  <path d="M 100 227.44 L 500 227.44 " fill="none" stroke="#008000" stroke-width="2" />
  <rect x="115.69" y="150" width="363.63" height="77.44" fill="none" stroke="#008000" stroke-width="5" />
  <rect x="115.69" y="150" width="363.63" height="77.44" fill="none" stroke="#FFA500" stroke-width="3" />
  <rect x="115.69" y="165.44" width="363.63" height="46.56" fill="none" stroke="#FF0000" />
  <path d="M 295 212 L 305 212 M 300 207 L 300 217 " fill="none" stroke="#FF0000" stroke-width="3" />
  <rect x="0" y="0" width="600" height="24" fill="#FFFFFF" />
  <text x="0" y="0" fill="#000000" text-anchor="start" dominant-baseline="text-before-edge" font-size="15" font-family="Inter" xml:space="preserve">&lt; Back</text>
  <text x="73.18" y="0" fill="#000000" text-anchor="start" dominant-baseline="text-before-edge" font-size="15" font-family="Inter" xml:space="preserve">TextMetrics</text>
  <g transform="translate(300 41.11376953125)" opacity="0.9">
    <polygon points="0,0 0,12 3,9 5,12 7,11 5,8 9,8" fill="white" stroke="black" stroke-width="0.7"/>
  </g>
</svg>
</div>

### Navigate: TextLayout

### 03. TextLayout

Status: ✅ Match

#### Expected

<div style="overflow:auto; border:1px solid #ddd; padding:8px; background:#fff;">
<svg xmlns="http://www.w3.org/2000/svg" width="600" height="400" viewBox="0 0 600 400">
  <rect x="7" y="31" width="124.53" height="28.94" fill="#D3D3D3" />
  <text x="13" y="37" fill="#000000" text-anchor="start" dominant-baseline="text-before-edge" font-size="14" font-family="Inter" xml:space="preserve">Normal text, 14pt</text>
  <rect x="6.5" y="30.5" width="125.53" height="29.94" fill="none" stroke="#808080" />
  <rect x="7" y="73.94" width="123.26" height="31.36" fill="#D3D3D3" />
  <text x="13" y="79.94" fill="#000000" text-anchor="start" dominant-baseline="text-before-edge" font-size="16" font-family="Inter" font-weight="Xui.Core.Canvas.FontWeight" xml:space="preserve">Bold text, 16pt</text>
  <rect x="6.5" y="73.44" width="124.26" height="32.36" fill="none" stroke="#808080" />
  <rect x="7" y="119.3" width="135.35" height="33.78" fill="#D3D3D3" />
  <text x="13" y="125.3" fill="#000000" text-anchor="start" dominant-baseline="text-before-edge" font-size="18" font-family="Inter" font-style="italic" xml:space="preserve">Italic text, 18pt</text>
  <rect x="6.5" y="118.8" width="136.35" height="34.78" fill="none" stroke="#808080" />
  <rect x="7" y="167.08" width="184.31" height="36.2" fill="#D3D3D3" />
  <text x="13" y="173.08" fill="#000000" text-anchor="start" dominant-baseline="text-before-edge" font-size="20" font-family="Inter" font-weight="Xui.Core.Canvas.FontWeight" font-style="italic" xml:space="preserve">Bold + Italic, 20pt</text>
  <rect x="6.5" y="166.58" width="185.31" height="37.2" fill="none" stroke="#808080" />
  <rect x="7" y="217.28" width="200.65" height="41.04" fill="#D3D3D3" />
  <text x="13" y="223.28" fill="#000000" text-anchor="start" dominant-baseline="text-before-edge" font-size="24" font-family="Inter" xml:space="preserve">Larger text, 24pt</text>
  <rect x="6.5" y="216.78" width="201.65" height="42.04" fill="none" stroke="#808080" />
  <rect x="0" y="0" width="600" height="24" fill="#FFFFFF" />
  <text x="0" y="0" fill="#000000" text-anchor="start" dominant-baseline="text-before-edge" font-size="15" font-family="Inter" xml:space="preserve">&lt; Back</text>
  <text x="73.18" y="0" fill="#000000" text-anchor="start" dominant-baseline="text-before-edge" font-size="15" font-family="Inter" xml:space="preserve">Text Layout</text>
  <g transform="translate(300 65.26318359375)" opacity="0.9">
    <polygon points="0,0 0,12 3,9 5,12 7,11 5,8 9,8" fill="white" stroke="black" stroke-width="0.7"/>
  </g>
</svg>
</div>

#### Actual

<div style="overflow:auto; border:1px solid #ddd; padding:8px; background:#fff;">
<svg xmlns="http://www.w3.org/2000/svg" width="600" height="400" viewBox="0 0 600 400">
  <rect x="7" y="31" width="124.53" height="28.94" fill="#D3D3D3" />
  <text x="13" y="37" fill="#000000" text-anchor="start" dominant-baseline="text-before-edge" font-size="14" font-family="Inter" xml:space="preserve">Normal text, 14pt</text>
  <rect x="6.5" y="30.5" width="125.53" height="29.94" fill="none" stroke="#808080" />
  <rect x="7" y="73.94" width="123.26" height="31.36" fill="#D3D3D3" />
  <text x="13" y="79.94" fill="#000000" text-anchor="start" dominant-baseline="text-before-edge" font-size="16" font-family="Inter" font-weight="Xui.Core.Canvas.FontWeight" xml:space="preserve">Bold text, 16pt</text>
  <rect x="6.5" y="73.44" width="124.26" height="32.36" fill="none" stroke="#808080" />
  <rect x="7" y="119.3" width="135.35" height="33.78" fill="#D3D3D3" />
  <text x="13" y="125.3" fill="#000000" text-anchor="start" dominant-baseline="text-before-edge" font-size="18" font-family="Inter" font-style="italic" xml:space="preserve">Italic text, 18pt</text>
  <rect x="6.5" y="118.8" width="136.35" height="34.78" fill="none" stroke="#808080" />
  <rect x="7" y="167.08" width="184.31" height="36.2" fill="#D3D3D3" />
  <text x="13" y="173.08" fill="#000000" text-anchor="start" dominant-baseline="text-before-edge" font-size="20" font-family="Inter" font-weight="Xui.Core.Canvas.FontWeight" font-style="italic" xml:space="preserve">Bold + Italic, 20pt</text>
  <rect x="6.5" y="166.58" width="185.31" height="37.2" fill="none" stroke="#808080" />
  <rect x="7" y="217.28" width="200.65" height="41.04" fill="#D3D3D3" />
  <text x="13" y="223.28" fill="#000000" text-anchor="start" dominant-baseline="text-before-edge" font-size="24" font-family="Inter" xml:space="preserve">Larger text, 24pt</text>
  <rect x="6.5" y="216.78" width="201.65" height="42.04" fill="none" stroke="#808080" />
  <rect x="0" y="0" width="600" height="24" fill="#FFFFFF" />
  <text x="0" y="0" fill="#000000" text-anchor="start" dominant-baseline="text-before-edge" font-size="15" font-family="Inter" xml:space="preserve">&lt; Back</text>
  <text x="73.18" y="0" fill="#000000" text-anchor="start" dominant-baseline="text-before-edge" font-size="15" font-family="Inter" xml:space="preserve">Text Layout</text>
  <g transform="translate(300 65.26318359375)" opacity="0.9">
    <polygon points="0,0 0,12 3,9 5,12 7,11 5,8 9,8" fill="white" stroke="black" stroke-width="0.7"/>
  </g>
</svg>
</div>

### Navigate: NestedStacks

### 04. NestedStacks

Status: ✅ Match

#### Expected

<div style="overflow:auto; border:1px solid #ddd; padding:8px; background:#fff;">
<svg xmlns="http://www.w3.org/2000/svg" width="600" height="400" viewBox="0 0 600 400">
  <path d="M 16 33 H 584 A 7 7 0 0 1 591 40 V 82.15 A 7 7 0 0 1 584 89.15 H 16 A 7 7 0 0 1 9 82.15 V 40 A 7 7 0 0 1 16 33 Z " fill="#FFFFFF" fill-rule="nonzero" />
  <text x="25" y="52" fill="#000000" text-anchor="start" dominant-baseline="text-before-edge" font-size="15" font-family="Inter" font-weight="Xui.Core.Canvas.FontWeight" xml:space="preserve">Row 1:</text>
  <path d="M 83.44 46 H 99.79 A 3 3 0 0 1 102.79 49 V 73.15 A 3 3 0 0 1 99.79 76.15 H 83.44 A 3 3 0 0 1 80.44 73.15 V 49 A 3 3 0 0 1 83.44 46 Z " fill="#D3D3D3" fill-rule="nonzero" />
  <text x="86.44" y="52" fill="#000000" text-anchor="start" dominant-baseline="text-before-edge" font-size="15" font-family="Inter" xml:space="preserve">A</text>
  <path d="M 83.44 45.5 H 99.79 A 3.5 3.5 0 0 1 103.29 49 V 73.15 A 3.5 3.5 0 0 1 99.79 76.65 H 83.44 A 3.5 3.5 0 0 1 79.94 73.15 V 49 A 3.5 3.5 0 0 1 83.44 45.5 Z " fill="none" stroke="#000000" />
  <path d="M 115.79 46 H 131.61 A 3 3 0 0 1 134.61 49 V 73.15 A 3 3 0 0 1 131.61 76.15 H 115.79 A 3 3 0 0 1 112.79 73.15 V 49 A 3 3 0 0 1 115.79 46 Z " fill="#D3D3D3" fill-rule="nonzero" />
  <text x="118.79" y="52" fill="#000000" text-anchor="start" dominant-baseline="text-before-edge" font-size="15" font-family="Inter" xml:space="preserve">B</text>
  <path d="M 115.79 45.5 H 131.61 A 3.5 3.5 0 0 1 135.11 49 V 73.15 A 3.5 3.5 0 0 1 131.61 76.65 H 115.79 A 3.5 3.5 0 0 1 112.29 73.15 V 49 A 3.5 3.5 0 0 1 115.79 45.5 Z " fill="none" stroke="#000000" />
  <path d="M 147.61 46 H 164.56 A 3 3 0 0 1 167.56 49 V 73.15 A 3 3 0 0 1 164.56 76.15 H 147.61 A 3 3 0 0 1 144.61 73.15 V 49 A 3 3 0 0 1 147.61 46 Z " fill="#D3D3D3" fill-rule="nonzero" />
  <text x="150.61" y="52" fill="#000000" text-anchor="start" dominant-baseline="text-before-edge" font-size="15" font-family="Inter" xml:space="preserve">C</text>
  <path d="M 147.61 45.5 H 164.56 A 3.5 3.5 0 0 1 168.06 49 V 73.15 A 3.5 3.5 0 0 1 164.56 76.65 H 147.61 A 3.5 3.5 0 0 1 144.11 73.15 V 49 A 3.5 3.5 0 0 1 147.61 45.5 Z " fill="none" stroke="#000000" />
  <path d="M 16 32.5 H 584 A 7.5 7.5 0 0 1 591.5 40 V 82.15 A 7.5 7.5 0 0 1 584 89.65 H 16 A 7.5 7.5 0 0 1 8.5 82.15 V 40 A 7.5 7.5 0 0 1 16 32.5 Z " fill="none" stroke="#808080" />
  <path d="M 16 107.15 H 584 A 7 7 0 0 1 591 114.15 V 156.3 A 7 7 0 0 1 584 163.3 H 16 A 7 7 0 0 1 9 156.3 V 114.15 A 7 7 0 0 1 16 107.15 Z " fill="#FFFFFF" fill-rule="nonzero" />
  <text x="25" y="126.15" fill="#000000" text-anchor="start" dominant-baseline="text-before-edge" font-size="15" font-family="Inter" font-weight="Xui.Core.Canvas.FontWeight" xml:space="preserve">Row 2:</text>
  <path d="M 86.42 120.15 H 121.5 A 3 3 0 0 1 124.5 123.15 V 147.3 A 3 3 0 0 1 121.5 150.3 H 86.42 A 3 3 0 0 1 83.42 147.3 V 123.15 A 3 3 0 0 1 86.42 120.15 Z " fill="#D3D3D3" fill-rule="nonzero" />
  <text x="89.42" y="126.15" fill="#000000" text-anchor="start" dominant-baseline="text-before-edge" font-size="15" font-family="Inter" xml:space="preserve">One</text>
  <path d="M 86.42 119.65 H 121.5 A 3.5 3.5 0 0 1 125 123.15 V 147.3 A 3.5 3.5 0 0 1 121.5 150.8 H 86.42 A 3.5 3.5 0 0 1 82.92 147.3 V 123.15 A 3.5 3.5 0 0 1 86.42 119.65 Z " fill="none" stroke="#000000" />
  <path d="M 137.5 120.15 H 173.24 A 3 3 0 0 1 176.24 123.15 V 147.3 A 3 3 0 0 1 173.24 150.3 H 137.5 A 3 3 0 0 1 134.5 147.3 V 123.15 A 3 3 0 0 1 137.5 120.15 Z " fill="#D3D3D3" fill-rule="nonzero" />
  <text x="140.5" y="126.15" fill="#000000" text-anchor="start" dominant-baseline="text-before-edge" font-size="15" font-family="Inter" xml:space="preserve">Two</text>
  <path d="M 137.5 119.65 H 173.24 A 3.5 3.5 0 0 1 176.74 123.15 V 147.3 A 3.5 3.5 0 0 1 173.24 150.8 H 137.5 A 3.5 3.5 0 0 1 134 147.3 V 123.15 A 3.5 3.5 0 0 1 137.5 119.65 Z " fill="none" stroke="#000000" />
  <path d="M 16 106.65 H 584 A 7.5 7.5 0 0 1 591.5 114.15 V 156.3 A 7.5 7.5 0 0 1 584 163.8 H 16 A 7.5 7.5 0 0 1 8.5 156.3 V 114.15 A 7.5 7.5 0 0 1 16 106.65 Z " fill="none" stroke="#808080" />
  <path d="M 16 181.3 H 584 A 7 7 0 0 1 591 188.3 V 230.45 A 7 7 0 0 1 584 237.45 H 16 A 7 7 0 0 1 9 230.45 V 188.3 A 7 7 0 0 1 16 181.3 Z " fill="#FFFFFF" fill-rule="nonzero" />
  <text x="25" y="200.3" fill="#000000" text-anchor="start" dominant-baseline="text-before-edge" font-size="15" font-family="Inter" font-weight="Xui.Core.Canvas.FontWeight" xml:space="preserve">Row 3:</text>
  <path d="M 86.66 194.3 H 119.83 A 3 3 0 0 1 122.83 197.3 V 221.45 A 3 3 0 0 1 119.83 224.45 H 86.66 A 3 3 0 0 1 83.66 221.45 V 197.3 A 3 3 0 0 1 86.66 194.3 Z " fill="#D3D3D3" fill-rule="nonzero" />
  <text x="89.66" y="200.3" fill="#000000" text-anchor="start" dominant-baseline="text-before-edge" font-size="15" font-family="Inter" xml:space="preserve">Left</text>
  <path d="M 86.66 193.8 H 119.83 A 3.5 3.5 0 0 1 123.33 197.3 V 221.45 A 3.5 3.5 0 0 1 119.83 224.95 H 86.66 A 3.5 3.5 0 0 1 83.16 221.45 V 197.3 A 3.5 3.5 0 0 1 86.66 193.8 Z " fill="none" stroke="#000000" />
  <path d="M 135.83 194.3 H 189.55 A 3 3 0 0 1 192.55 197.3 V 221.45 A 3 3 0 0 1 189.55 224.45 H 135.83 A 3 3 0 0 1 132.83 221.45 V 197.3 A 3 3 0 0 1 135.83 194.3 Z " fill="#D3D3D3" fill-rule="nonzero" />
  <text x="138.83" y="200.3" fill="#000000" text-anchor="start" dominant-baseline="text-before-edge" font-size="15" font-family="Inter" xml:space="preserve">Center</text>
  <path d="M 135.83 193.8 H 189.55 A 3.5 3.5 0 0 1 193.05 197.3 V 221.45 A 3.5 3.5 0 0 1 189.55 224.95 H 135.83 A 3.5 3.5 0 0 1 132.33 221.45 V 197.3 A 3.5 3.5 0 0 1 135.83 193.8 Z " fill="none" stroke="#000000" />
  <path d="M 205.55 194.3 H 247.81 A 3 3 0 0 1 250.81 197.3 V 221.45 A 3 3 0 0 1 247.81 224.45 H 205.55 A 3 3 0 0 1 202.55 221.45 V 197.3 A 3 3 0 0 1 205.55 194.3 Z " fill="#D3D3D3" fill-rule="nonzero" />
  <text x="208.55" y="200.3" fill="#000000" text-anchor="start" dominant-baseline="text-before-edge" font-size="15" font-family="Inter" xml:space="preserve">Right</text>
  <path d="M 205.55 193.8 H 247.81 A 3.5 3.5 0 0 1 251.31 197.3 V 221.45 A 3.5 3.5 0 0 1 247.81 224.95 H 205.55 A 3.5 3.5 0 0 1 202.05 221.45 V 197.3 A 3.5 3.5 0 0 1 205.55 193.8 Z " fill="none" stroke="#000000" />
  <path d="M 263.81 194.3 H 320.68 A 3 3 0 0 1 323.68 197.3 V 221.45 A 3 3 0 0 1 320.68 224.45 H 263.81 A 3 3 0 0 1 260.81 221.45 V 197.3 A 3 3 0 0 1 263.81 194.3 Z " fill="#D3D3D3" fill-rule="nonzero" />
  <text x="266.81" y="200.3" fill="#000000" text-anchor="start" dominant-baseline="text-before-edge" font-size="15" font-family="Inter" xml:space="preserve">Stretch</text>
  <path d="M 263.81 193.8 H 320.68 A 3.5 3.5 0 0 1 324.18 197.3 V 221.45 A 3.5 3.5 0 0 1 320.68 224.95 H 263.81 A 3.5 3.5 0 0 1 260.31 221.45 V 197.3 A 3.5 3.5 0 0 1 263.81 193.8 Z " fill="none" stroke="#000000" />
  <path d="M 16 180.8 H 584 A 7.5 7.5 0 0 1 591.5 188.3 V 230.45 A 7.5 7.5 0 0 1 584 237.95 H 16 A 7.5 7.5 0 0 1 8.5 230.45 V 188.3 A 7.5 7.5 0 0 1 16 180.8 Z " fill="none" stroke="#808080" />
  <rect x="0" y="0" width="600" height="24" fill="#FFFFFF" />
  <text x="0" y="0" fill="#000000" text-anchor="start" dominant-baseline="text-before-edge" font-size="15" font-family="Inter" xml:space="preserve">&lt; Back</text>
  <text x="73.18" y="0" fill="#000000" text-anchor="start" dominant-baseline="text-before-edge" font-size="15" font-family="Inter" xml:space="preserve">Nested Stacks</text>
  <g transform="translate(300 89.41259765625)" opacity="0.9">
    <polygon points="0,0 0,12 3,9 5,12 7,11 5,8 9,8" fill="white" stroke="black" stroke-width="0.7"/>
  </g>
</svg>
</div>

#### Actual

<div style="overflow:auto; border:1px solid #ddd; padding:8px; background:#fff;">
<svg xmlns="http://www.w3.org/2000/svg" width="600" height="400" viewBox="0 0 600 400">
  <path d="M 16 33 H 584 A 7 7 0 0 1 591 40 V 82.15 A 7 7 0 0 1 584 89.15 H 16 A 7 7 0 0 1 9 82.15 V 40 A 7 7 0 0 1 16 33 Z " fill="#FFFFFF" fill-rule="nonzero" />
  <text x="25" y="52" fill="#000000" text-anchor="start" dominant-baseline="text-before-edge" font-size="15" font-family="Inter" font-weight="Xui.Core.Canvas.FontWeight" xml:space="preserve">Row 1:</text>
  <path d="M 83.44 46 H 99.79 A 3 3 0 0 1 102.79 49 V 73.15 A 3 3 0 0 1 99.79 76.15 H 83.44 A 3 3 0 0 1 80.44 73.15 V 49 A 3 3 0 0 1 83.44 46 Z " fill="#D3D3D3" fill-rule="nonzero" />
  <text x="86.44" y="52" fill="#000000" text-anchor="start" dominant-baseline="text-before-edge" font-size="15" font-family="Inter" xml:space="preserve">A</text>
  <path d="M 83.44 45.5 H 99.79 A 3.5 3.5 0 0 1 103.29 49 V 73.15 A 3.5 3.5 0 0 1 99.79 76.65 H 83.44 A 3.5 3.5 0 0 1 79.94 73.15 V 49 A 3.5 3.5 0 0 1 83.44 45.5 Z " fill="none" stroke="#000000" />
  <path d="M 115.79 46 H 131.61 A 3 3 0 0 1 134.61 49 V 73.15 A 3 3 0 0 1 131.61 76.15 H 115.79 A 3 3 0 0 1 112.79 73.15 V 49 A 3 3 0 0 1 115.79 46 Z " fill="#D3D3D3" fill-rule="nonzero" />
  <text x="118.79" y="52" fill="#000000" text-anchor="start" dominant-baseline="text-before-edge" font-size="15" font-family="Inter" xml:space="preserve">B</text>
  <path d="M 115.79 45.5 H 131.61 A 3.5 3.5 0 0 1 135.11 49 V 73.15 A 3.5 3.5 0 0 1 131.61 76.65 H 115.79 A 3.5 3.5 0 0 1 112.29 73.15 V 49 A 3.5 3.5 0 0 1 115.79 45.5 Z " fill="none" stroke="#000000" />
  <path d="M 147.61 46 H 164.56 A 3 3 0 0 1 167.56 49 V 73.15 A 3 3 0 0 1 164.56 76.15 H 147.61 A 3 3 0 0 1 144.61 73.15 V 49 A 3 3 0 0 1 147.61 46 Z " fill="#D3D3D3" fill-rule="nonzero" />
  <text x="150.61" y="52" fill="#000000" text-anchor="start" dominant-baseline="text-before-edge" font-size="15" font-family="Inter" xml:space="preserve">C</text>
  <path d="M 147.61 45.5 H 164.56 A 3.5 3.5 0 0 1 168.06 49 V 73.15 A 3.5 3.5 0 0 1 164.56 76.65 H 147.61 A 3.5 3.5 0 0 1 144.11 73.15 V 49 A 3.5 3.5 0 0 1 147.61 45.5 Z " fill="none" stroke="#000000" />
  <path d="M 16 32.5 H 584 A 7.5 7.5 0 0 1 591.5 40 V 82.15 A 7.5 7.5 0 0 1 584 89.65 H 16 A 7.5 7.5 0 0 1 8.5 82.15 V 40 A 7.5 7.5 0 0 1 16 32.5 Z " fill="none" stroke="#808080" />
  <path d="M 16 107.15 H 584 A 7 7 0 0 1 591 114.15 V 156.3 A 7 7 0 0 1 584 163.3 H 16 A 7 7 0 0 1 9 156.3 V 114.15 A 7 7 0 0 1 16 107.15 Z " fill="#FFFFFF" fill-rule="nonzero" />
  <text x="25" y="126.15" fill="#000000" text-anchor="start" dominant-baseline="text-before-edge" font-size="15" font-family="Inter" font-weight="Xui.Core.Canvas.FontWeight" xml:space="preserve">Row 2:</text>
  <path d="M 86.42 120.15 H 121.5 A 3 3 0 0 1 124.5 123.15 V 147.3 A 3 3 0 0 1 121.5 150.3 H 86.42 A 3 3 0 0 1 83.42 147.3 V 123.15 A 3 3 0 0 1 86.42 120.15 Z " fill="#D3D3D3" fill-rule="nonzero" />
  <text x="89.42" y="126.15" fill="#000000" text-anchor="start" dominant-baseline="text-before-edge" font-size="15" font-family="Inter" xml:space="preserve">One</text>
  <path d="M 86.42 119.65 H 121.5 A 3.5 3.5 0 0 1 125 123.15 V 147.3 A 3.5 3.5 0 0 1 121.5 150.8 H 86.42 A 3.5 3.5 0 0 1 82.92 147.3 V 123.15 A 3.5 3.5 0 0 1 86.42 119.65 Z " fill="none" stroke="#000000" />
  <path d="M 137.5 120.15 H 173.24 A 3 3 0 0 1 176.24 123.15 V 147.3 A 3 3 0 0 1 173.24 150.3 H 137.5 A 3 3 0 0 1 134.5 147.3 V 123.15 A 3 3 0 0 1 137.5 120.15 Z " fill="#D3D3D3" fill-rule="nonzero" />
  <text x="140.5" y="126.15" fill="#000000" text-anchor="start" dominant-baseline="text-before-edge" font-size="15" font-family="Inter" xml:space="preserve">Two</text>
  <path d="M 137.5 119.65 H 173.24 A 3.5 3.5 0 0 1 176.74 123.15 V 147.3 A 3.5 3.5 0 0 1 173.24 150.8 H 137.5 A 3.5 3.5 0 0 1 134 147.3 V 123.15 A 3.5 3.5 0 0 1 137.5 119.65 Z " fill="none" stroke="#000000" />
  <path d="M 16 106.65 H 584 A 7.5 7.5 0 0 1 591.5 114.15 V 156.3 A 7.5 7.5 0 0 1 584 163.8 H 16 A 7.5 7.5 0 0 1 8.5 156.3 V 114.15 A 7.5 7.5 0 0 1 16 106.65 Z " fill="none" stroke="#808080" />
  <path d="M 16 181.3 H 584 A 7 7 0 0 1 591 188.3 V 230.45 A 7 7 0 0 1 584 237.45 H 16 A 7 7 0 0 1 9 230.45 V 188.3 A 7 7 0 0 1 16 181.3 Z " fill="#FFFFFF" fill-rule="nonzero" />
  <text x="25" y="200.3" fill="#000000" text-anchor="start" dominant-baseline="text-before-edge" font-size="15" font-family="Inter" font-weight="Xui.Core.Canvas.FontWeight" xml:space="preserve">Row 3:</text>
  <path d="M 86.66 194.3 H 119.83 A 3 3 0 0 1 122.83 197.3 V 221.45 A 3 3 0 0 1 119.83 224.45 H 86.66 A 3 3 0 0 1 83.66 221.45 V 197.3 A 3 3 0 0 1 86.66 194.3 Z " fill="#D3D3D3" fill-rule="nonzero" />
  <text x="89.66" y="200.3" fill="#000000" text-anchor="start" dominant-baseline="text-before-edge" font-size="15" font-family="Inter" xml:space="preserve">Left</text>
  <path d="M 86.66 193.8 H 119.83 A 3.5 3.5 0 0 1 123.33 197.3 V 221.45 A 3.5 3.5 0 0 1 119.83 224.95 H 86.66 A 3.5 3.5 0 0 1 83.16 221.45 V 197.3 A 3.5 3.5 0 0 1 86.66 193.8 Z " fill="none" stroke="#000000" />
  <path d="M 135.83 194.3 H 189.55 A 3 3 0 0 1 192.55 197.3 V 221.45 A 3 3 0 0 1 189.55 224.45 H 135.83 A 3 3 0 0 1 132.83 221.45 V 197.3 A 3 3 0 0 1 135.83 194.3 Z " fill="#D3D3D3" fill-rule="nonzero" />
  <text x="138.83" y="200.3" fill="#000000" text-anchor="start" dominant-baseline="text-before-edge" font-size="15" font-family="Inter" xml:space="preserve">Center</text>
  <path d="M 135.83 193.8 H 189.55 A 3.5 3.5 0 0 1 193.05 197.3 V 221.45 A 3.5 3.5 0 0 1 189.55 224.95 H 135.83 A 3.5 3.5 0 0 1 132.33 221.45 V 197.3 A 3.5 3.5 0 0 1 135.83 193.8 Z " fill="none" stroke="#000000" />
  <path d="M 205.55 194.3 H 247.81 A 3 3 0 0 1 250.81 197.3 V 221.45 A 3 3 0 0 1 247.81 224.45 H 205.55 A 3 3 0 0 1 202.55 221.45 V 197.3 A 3 3 0 0 1 205.55 194.3 Z " fill="#D3D3D3" fill-rule="nonzero" />
  <text x="208.55" y="200.3" fill="#000000" text-anchor="start" dominant-baseline="text-before-edge" font-size="15" font-family="Inter" xml:space="preserve">Right</text>
  <path d="M 205.55 193.8 H 247.81 A 3.5 3.5 0 0 1 251.31 197.3 V 221.45 A 3.5 3.5 0 0 1 247.81 224.95 H 205.55 A 3.5 3.5 0 0 1 202.05 221.45 V 197.3 A 3.5 3.5 0 0 1 205.55 193.8 Z " fill="none" stroke="#000000" />
  <path d="M 263.81 194.3 H 320.68 A 3 3 0 0 1 323.68 197.3 V 221.45 A 3 3 0 0 1 320.68 224.45 H 263.81 A 3 3 0 0 1 260.81 221.45 V 197.3 A 3 3 0 0 1 263.81 194.3 Z " fill="#D3D3D3" fill-rule="nonzero" />
  <text x="266.81" y="200.3" fill="#000000" text-anchor="start" dominant-baseline="text-before-edge" font-size="15" font-family="Inter" xml:space="preserve">Stretch</text>
  <path d="M 263.81 193.8 H 320.68 A 3.5 3.5 0 0 1 324.18 197.3 V 221.45 A 3.5 3.5 0 0 1 320.68 224.95 H 263.81 A 3.5 3.5 0 0 1 260.31 221.45 V 197.3 A 3.5 3.5 0 0 1 263.81 193.8 Z " fill="none" stroke="#000000" />
  <path d="M 16 180.8 H 584 A 7.5 7.5 0 0 1 591.5 188.3 V 230.45 A 7.5 7.5 0 0 1 584 237.95 H 16 A 7.5 7.5 0 0 1 8.5 230.45 V 188.3 A 7.5 7.5 0 0 1 16 180.8 Z " fill="none" stroke="#808080" />
  <rect x="0" y="0" width="600" height="24" fill="#FFFFFF" />
  <text x="0" y="0" fill="#000000" text-anchor="start" dominant-baseline="text-before-edge" font-size="15" font-family="Inter" xml:space="preserve">&lt; Back</text>
  <text x="73.18" y="0" fill="#000000" text-anchor="start" dominant-baseline="text-before-edge" font-size="15" font-family="Inter" xml:space="preserve">Nested Stacks</text>
  <g transform="translate(300 89.41259765625)" opacity="0.9">
    <polygon points="0,0 0,12 3,9 5,12 7,11 5,8 9,8" fill="white" stroke="black" stroke-width="0.7"/>
  </g>
</svg>
</div>

### Navigate: ViewCollectionAlignment

### 05. ViewCollectionAlignment

Status: ✅ Match

#### Expected

<div style="overflow:auto; border:1px solid #ddd; padding:8px; background:#fff;">
<svg xmlns="http://www.w3.org/2000/svg" width="600" height="400" viewBox="0 0 600 400">
  <path d="M 5 25 H 18.74 A 4 4 0 0 1 22.74 29 V 395 A 4 4 0 0 1 18.74 399 H 5 A 4 4 0 0 1 1 395 V 29 A 4 4 0 0 1 5 25 Z " fill="#FFFFFF" fill-rule="nonzero" />
  <text x="1" y="204.74" fill="#000000" text-anchor="start" dominant-baseline="text-before-edge" font-size="12" font-family="Inter" xml:space="preserve">Left</text>
  <path d="M 5 24.5 H 18.74 A 4.5 4.5 0 0 1 23.24 29 V 395 A 4.5 4.5 0 0 1 18.74 399.5 H 5 A 4.5 4.5 0 0 1 0.5 395 V 29 A 4.5 4.5 0 0 1 5 24.5 Z " fill="none" stroke="#000000" />
  <path d="M 284.91 25 H 315.09 A 4 4 0 0 1 319.09 29 V 395 A 4 4 0 0 1 315.09 399 H 284.91 A 4 4 0 0 1 280.91 395 V 29 A 4 4 0 0 1 284.91 25 Z " fill="#FFFFFF" fill-rule="nonzero" />
  <text x="280.91" y="204.74" fill="#000000" text-anchor="start" dominant-baseline="text-before-edge" font-size="12" font-family="Inter" xml:space="preserve">Center</text>
  <path d="M 284.91 24.5 H 315.09 A 4.5 4.5 0 0 1 319.59 29 V 395 A 4.5 4.5 0 0 1 315.09 399.5 H 284.91 A 4.5 4.5 0 0 1 280.41 395 V 29 A 4.5 4.5 0 0 1 284.91 24.5 Z " fill="none" stroke="#000000" />
  <path d="M 573.99 25 H 595 A 4 4 0 0 1 599 29 V 395 A 4 4 0 0 1 595 399 H 573.99 A 4 4 0 0 1 569.99 395 V 29 A 4 4 0 0 1 573.99 25 Z " fill="#FFFFFF" fill-rule="nonzero" />
  <text x="569.99" y="204.74" fill="#000000" text-anchor="start" dominant-baseline="text-before-edge" font-size="12" font-family="Inter" xml:space="preserve">Right</text>
  <path d="M 573.99 24.5 H 595 A 4.5 4.5 0 0 1 599.5 29 V 395 A 4.5 4.5 0 0 1 595 399.5 H 573.99 A 4.5 4.5 0 0 1 569.49 395 V 29 A 4.5 4.5 0 0 1 573.99 24.5 Z " fill="none" stroke="#000000" />
  <path d="M 5 25 H 595 A 4 4 0 0 1 599 29 V 35.52 A 4 4 0 0 1 595 39.52 H 5 A 4 4 0 0 1 1 35.52 V 29 A 4 4 0 0 1 5 25 Z " fill="#FFFFFF" fill-rule="nonzero" />
  <text x="289.32" y="25" fill="#000000" text-anchor="start" dominant-baseline="text-before-edge" font-size="12" font-family="Inter" xml:space="preserve">Top</text>
  <path d="M 5 24.5 H 595 A 4.5 4.5 0 0 1 599.5 29 V 35.52 A 4.5 4.5 0 0 1 595 40.02 H 5 A 4.5 4.5 0 0 1 0.5 35.52 V 29 A 4.5 4.5 0 0 1 5 24.5 Z " fill="none" stroke="#000000" />
  <path d="M 5 204.74 H 595 A 4 4 0 0 1 599 208.74 V 215.26 A 4 4 0 0 1 595 219.26 H 5 A 4 4 0 0 1 1 215.26 V 208.74 A 4 4 0 0 1 5 204.74 Z " fill="#FFFFFF" fill-rule="nonzero" />
  <text x="280.83" y="204.74" fill="#000000" text-anchor="start" dominant-baseline="text-before-edge" font-size="12" font-family="Inter" xml:space="preserve">Middle</text>
  <path d="M 5 204.24 H 595 A 4.5 4.5 0 0 1 599.5 208.74 V 215.26 A 4.5 4.5 0 0 1 595 219.76 H 5 A 4.5 4.5 0 0 1 0.5 215.26 V 208.74 A 4.5 4.5 0 0 1 5 204.24 Z " fill="none" stroke="#000000" />
  <path d="M 5 384.48 H 595 A 4 4 0 0 1 599 388.48 V 395 A 4 4 0 0 1 595 399 H 5 A 4 4 0 0 1 1 395 V 388.48 A 4 4 0 0 1 5 384.48 Z " fill="#FFFFFF" fill-rule="nonzero" />
  <text x="279.76" y="384.48" fill="#000000" text-anchor="start" dominant-baseline="text-before-edge" font-size="12" font-family="Inter" xml:space="preserve">Bottom</text>
  <path d="M 5 383.98 H 595 A 4.5 4.5 0 0 1 599.5 388.48 V 395 A 4.5 4.5 0 0 1 595 399.5 H 5 A 4.5 4.5 0 0 1 0.5 395 V 388.48 A 4.5 4.5 0 0 1 5 383.98 Z " fill="none" stroke="#000000" />
  <path d="M 29 49 H 67.46 A 4 4 0 0 1 71.46 53 V 59.52 A 4 4 0 0 1 67.46 63.52 H 29 A 4 4 0 0 1 25 59.52 V 53 A 4 4 0 0 1 29 49 Z " fill="#FFFFFF" fill-rule="nonzero" />
  <text x="25" y="49" fill="#000000" text-anchor="start" dominant-baseline="text-before-edge" font-size="12" font-family="Inter" xml:space="preserve">Top Left</text>
  <path d="M 29 48.5 H 67.46 A 4.5 4.5 0 0 1 71.96 53 V 59.52 A 4.5 4.5 0 0 1 67.46 64.02 H 29 A 4.5 4.5 0 0 1 24.5 59.52 V 53 A 4.5 4.5 0 0 1 29 48.5 Z " fill="none" stroke="#000000" />
  <path d="M 272.55 49 H 327.45 A 4 4 0 0 1 331.45 53 V 59.52 A 4 4 0 0 1 327.45 63.52 H 272.55 A 4 4 0 0 1 268.55 59.52 V 53 A 4 4 0 0 1 272.55 49 Z " fill="#FFFFFF" fill-rule="nonzero" />
  <text x="268.55" y="49" fill="#000000" text-anchor="start" dominant-baseline="text-before-edge" font-size="12" font-family="Inter" xml:space="preserve">Top Center</text>
  <path d="M 272.55 48.5 H 327.45 A 4.5 4.5 0 0 1 331.95 53 V 59.52 A 4.5 4.5 0 0 1 327.45 64.02 H 272.55 A 4.5 4.5 0 0 1 268.05 59.52 V 53 A 4.5 4.5 0 0 1 272.55 48.5 Z " fill="none" stroke="#000000" />
  <path d="M 525.26 49 H 571 A 4 4 0 0 1 575 53 V 59.52 A 4 4 0 0 1 571 63.52 H 525.26 A 4 4 0 0 1 521.26 59.52 V 53 A 4 4 0 0 1 525.26 49 Z " fill="#FFFFFF" fill-rule="nonzero" />
  <text x="521.26" y="49" fill="#000000" text-anchor="start" dominant-baseline="text-before-edge" font-size="12" font-family="Inter" xml:space="preserve">Top Right</text>
  <path d="M 525.26 48.5 H 571 A 4.5 4.5 0 0 1 575.5 53 V 59.52 A 4.5 4.5 0 0 1 571 64.02 H 525.26 A 4.5 4.5 0 0 1 520.76 59.52 V 53 A 4.5 4.5 0 0 1 525.26 48.5 Z " fill="none" stroke="#000000" />
  <path d="M 29 204.74 H 84.46 A 4 4 0 0 1 88.46 208.74 V 215.26 A 4 4 0 0 1 84.46 219.26 H 29 A 4 4 0 0 1 25 215.26 V 208.74 A 4 4 0 0 1 29 204.74 Z " fill="#FFFFFF" fill-rule="nonzero" />
  <text x="25" y="204.74" fill="#000000" text-anchor="start" dominant-baseline="text-before-edge" font-size="12" font-family="Inter" xml:space="preserve">Middle Left</text>
  <path d="M 29 204.24 H 84.46 A 4.5 4.5 0 0 1 88.96 208.74 V 215.26 A 4.5 4.5 0 0 1 84.46 219.76 H 29 A 4.5 4.5 0 0 1 24.5 215.26 V 208.74 A 4.5 4.5 0 0 1 29 204.24 Z " fill="none" stroke="#000000" />
  <path d="M 264.05 204.74 H 335.95 A 4 4 0 0 1 339.95 208.74 V 215.26 A 4 4 0 0 1 335.95 219.26 H 264.05 A 4 4 0 0 1 260.05 215.26 V 208.74 A 4 4 0 0 1 264.05 204.74 Z " fill="#FFFFFF" fill-rule="nonzero" />
  <text x="260.05" y="204.74" fill="#000000" text-anchor="start" dominant-baseline="text-before-edge" font-size="12" font-family="Inter" xml:space="preserve">Middle Center</text>
  <path d="M 264.05 204.24 H 335.95 A 4.5 4.5 0 0 1 340.45 208.74 V 215.26 A 4.5 4.5 0 0 1 335.95 219.76 H 264.05 A 4.5 4.5 0 0 1 259.55 215.26 V 208.74 A 4.5 4.5 0 0 1 264.05 204.24 Z " fill="none" stroke="#000000" />
  <path d="M 508.27 204.74 H 571 A 4 4 0 0 1 575 208.74 V 215.26 A 4 4 0 0 1 571 219.26 H 508.27 A 4 4 0 0 1 504.27 215.26 V 208.74 A 4 4 0 0 1 508.27 204.74 Z " fill="#FFFFFF" fill-rule="nonzero" />
  <text x="504.27" y="204.74" fill="#000000" text-anchor="start" dominant-baseline="text-before-edge" font-size="12" font-family="Inter" xml:space="preserve">Middle Right</text>
  <path d="M 508.27 204.24 H 571 A 4.5 4.5 0 0 1 575.5 208.74 V 215.26 A 4.5 4.5 0 0 1 571 219.76 H 508.27 A 4.5 4.5 0 0 1 503.77 215.26 V 208.74 A 4.5 4.5 0 0 1 508.27 204.24 Z " fill="none" stroke="#000000" />
  <path d="M 29 360.48 H 86.6 A 4 4 0 0 1 90.6 364.48 V 371 A 4 4 0 0 1 86.6 375 H 29 A 4 4 0 0 1 25 371 V 364.48 A 4 4 0 0 1 29 360.48 Z " fill="#FFFFFF" fill-rule="nonzero" />
  <text x="25" y="360.48" fill="#000000" text-anchor="start" dominant-baseline="text-before-edge" font-size="12" font-family="Inter" xml:space="preserve">Bottom Left</text>
  <path d="M 29 359.98 H 86.6 A 4.5 4.5 0 0 1 91.1 364.48 V 371 A 4.5 4.5 0 0 1 86.6 375.5 H 29 A 4.5 4.5 0 0 1 24.5 371 V 364.48 A 4.5 4.5 0 0 1 29 359.98 Z " fill="none" stroke="#000000" />
  <path d="M 262.98 360.48 H 337.02 A 4 4 0 0 1 341.02 364.48 V 371 A 4 4 0 0 1 337.02 375 H 262.98 A 4 4 0 0 1 258.98 371 V 364.48 A 4 4 0 0 1 262.98 360.48 Z " fill="#FFFFFF" fill-rule="nonzero" />
  <text x="258.98" y="360.48" fill="#000000" text-anchor="start" dominant-baseline="text-before-edge" font-size="12" font-family="Inter" xml:space="preserve">Bottom Center</text>
  <path d="M 262.98 359.98 H 337.02 A 4.5 4.5 0 0 1 341.52 364.48 V 371 A 4.5 4.5 0 0 1 337.02 375.5 H 262.98 A 4.5 4.5 0 0 1 258.48 371 V 364.48 A 4.5 4.5 0 0 1 262.98 359.98 Z " fill="none" stroke="#000000" />
  <path d="M 506.13 360.48 H 571 A 4 4 0 0 1 575 364.48 V 371 A 4 4 0 0 1 571 375 H 506.13 A 4 4 0 0 1 502.13 371 V 364.48 A 4 4 0 0 1 506.13 360.48 Z " fill="#FFFFFF" fill-rule="nonzero" />
  <text x="502.13" y="360.48" fill="#000000" text-anchor="start" dominant-baseline="text-before-edge" font-size="12" font-family="Inter" xml:space="preserve">Bottom Right</text>
  <path d="M 506.13 359.98 H 571 A 4.5 4.5 0 0 1 575.5 364.48 V 371 A 4.5 4.5 0 0 1 571 375.5 H 506.13 A 4.5 4.5 0 0 1 501.63 371 V 364.48 A 4.5 4.5 0 0 1 506.13 359.98 Z " fill="none" stroke="#000000" />
  <rect x="0" y="0" width="600" height="24" fill="#FFFFFF" />
  <text x="0" y="0" fill="#000000" text-anchor="start" dominant-baseline="text-before-edge" font-size="15" font-family="Inter" xml:space="preserve">&lt; Back</text>
  <text x="73.18" y="0" fill="#000000" text-anchor="start" dominant-baseline="text-before-edge" font-size="15" font-family="Inter" xml:space="preserve">ViewCollection Alignment</text>
  <g transform="translate(300 137.71142578125)" opacity="0.9">
    <polygon points="0,0 0,12 3,9 5,12 7,11 5,8 9,8" fill="white" stroke="black" stroke-width="0.7"/>
  </g>
</svg>
</div>

#### Actual

<div style="overflow:auto; border:1px solid #ddd; padding:8px; background:#fff;">
<svg xmlns="http://www.w3.org/2000/svg" width="600" height="400" viewBox="0 0 600 400">
  <path d="M 5 25 H 18.74 A 4 4 0 0 1 22.74 29 V 395 A 4 4 0 0 1 18.74 399 H 5 A 4 4 0 0 1 1 395 V 29 A 4 4 0 0 1 5 25 Z " fill="#FFFFFF" fill-rule="nonzero" />
  <text x="1" y="204.74" fill="#000000" text-anchor="start" dominant-baseline="text-before-edge" font-size="12" font-family="Inter" xml:space="preserve">Left</text>
  <path d="M 5 24.5 H 18.74 A 4.5 4.5 0 0 1 23.24 29 V 395 A 4.5 4.5 0 0 1 18.74 399.5 H 5 A 4.5 4.5 0 0 1 0.5 395 V 29 A 4.5 4.5 0 0 1 5 24.5 Z " fill="none" stroke="#000000" />
  <path d="M 284.91 25 H 315.09 A 4 4 0 0 1 319.09 29 V 395 A 4 4 0 0 1 315.09 399 H 284.91 A 4 4 0 0 1 280.91 395 V 29 A 4 4 0 0 1 284.91 25 Z " fill="#FFFFFF" fill-rule="nonzero" />
  <text x="280.91" y="204.74" fill="#000000" text-anchor="start" dominant-baseline="text-before-edge" font-size="12" font-family="Inter" xml:space="preserve">Center</text>
  <path d="M 284.91 24.5 H 315.09 A 4.5 4.5 0 0 1 319.59 29 V 395 A 4.5 4.5 0 0 1 315.09 399.5 H 284.91 A 4.5 4.5 0 0 1 280.41 395 V 29 A 4.5 4.5 0 0 1 284.91 24.5 Z " fill="none" stroke="#000000" />
  <path d="M 573.99 25 H 595 A 4 4 0 0 1 599 29 V 395 A 4 4 0 0 1 595 399 H 573.99 A 4 4 0 0 1 569.99 395 V 29 A 4 4 0 0 1 573.99 25 Z " fill="#FFFFFF" fill-rule="nonzero" />
  <text x="569.99" y="204.74" fill="#000000" text-anchor="start" dominant-baseline="text-before-edge" font-size="12" font-family="Inter" xml:space="preserve">Right</text>
  <path d="M 573.99 24.5 H 595 A 4.5 4.5 0 0 1 599.5 29 V 395 A 4.5 4.5 0 0 1 595 399.5 H 573.99 A 4.5 4.5 0 0 1 569.49 395 V 29 A 4.5 4.5 0 0 1 573.99 24.5 Z " fill="none" stroke="#000000" />
  <path d="M 5 25 H 595 A 4 4 0 0 1 599 29 V 35.52 A 4 4 0 0 1 595 39.52 H 5 A 4 4 0 0 1 1 35.52 V 29 A 4 4 0 0 1 5 25 Z " fill="#FFFFFF" fill-rule="nonzero" />
  <text x="289.32" y="25" fill="#000000" text-anchor="start" dominant-baseline="text-before-edge" font-size="12" font-family="Inter" xml:space="preserve">Top</text>
  <path d="M 5 24.5 H 595 A 4.5 4.5 0 0 1 599.5 29 V 35.52 A 4.5 4.5 0 0 1 595 40.02 H 5 A 4.5 4.5 0 0 1 0.5 35.52 V 29 A 4.5 4.5 0 0 1 5 24.5 Z " fill="none" stroke="#000000" />
  <path d="M 5 204.74 H 595 A 4 4 0 0 1 599 208.74 V 215.26 A 4 4 0 0 1 595 219.26 H 5 A 4 4 0 0 1 1 215.26 V 208.74 A 4 4 0 0 1 5 204.74 Z " fill="#FFFFFF" fill-rule="nonzero" />
  <text x="280.83" y="204.74" fill="#000000" text-anchor="start" dominant-baseline="text-before-edge" font-size="12" font-family="Inter" xml:space="preserve">Middle</text>
  <path d="M 5 204.24 H 595 A 4.5 4.5 0 0 1 599.5 208.74 V 215.26 A 4.5 4.5 0 0 1 595 219.76 H 5 A 4.5 4.5 0 0 1 0.5 215.26 V 208.74 A 4.5 4.5 0 0 1 5 204.24 Z " fill="none" stroke="#000000" />
  <path d="M 5 384.48 H 595 A 4 4 0 0 1 599 388.48 V 395 A 4 4 0 0 1 595 399 H 5 A 4 4 0 0 1 1 395 V 388.48 A 4 4 0 0 1 5 384.48 Z " fill="#FFFFFF" fill-rule="nonzero" />
  <text x="279.76" y="384.48" fill="#000000" text-anchor="start" dominant-baseline="text-before-edge" font-size="12" font-family="Inter" xml:space="preserve">Bottom</text>
  <path d="M 5 383.98 H 595 A 4.5 4.5 0 0 1 599.5 388.48 V 395 A 4.5 4.5 0 0 1 595 399.5 H 5 A 4.5 4.5 0 0 1 0.5 395 V 388.48 A 4.5 4.5 0 0 1 5 383.98 Z " fill="none" stroke="#000000" />
  <path d="M 29 49 H 67.46 A 4 4 0 0 1 71.46 53 V 59.52 A 4 4 0 0 1 67.46 63.52 H 29 A 4 4 0 0 1 25 59.52 V 53 A 4 4 0 0 1 29 49 Z " fill="#FFFFFF" fill-rule="nonzero" />
  <text x="25" y="49" fill="#000000" text-anchor="start" dominant-baseline="text-before-edge" font-size="12" font-family="Inter" xml:space="preserve">Top Left</text>
  <path d="M 29 48.5 H 67.46 A 4.5 4.5 0 0 1 71.96 53 V 59.52 A 4.5 4.5 0 0 1 67.46 64.02 H 29 A 4.5 4.5 0 0 1 24.5 59.52 V 53 A 4.5 4.5 0 0 1 29 48.5 Z " fill="none" stroke="#000000" />
  <path d="M 272.55 49 H 327.45 A 4 4 0 0 1 331.45 53 V 59.52 A 4 4 0 0 1 327.45 63.52 H 272.55 A 4 4 0 0 1 268.55 59.52 V 53 A 4 4 0 0 1 272.55 49 Z " fill="#FFFFFF" fill-rule="nonzero" />
  <text x="268.55" y="49" fill="#000000" text-anchor="start" dominant-baseline="text-before-edge" font-size="12" font-family="Inter" xml:space="preserve">Top Center</text>
  <path d="M 272.55 48.5 H 327.45 A 4.5 4.5 0 0 1 331.95 53 V 59.52 A 4.5 4.5 0 0 1 327.45 64.02 H 272.55 A 4.5 4.5 0 0 1 268.05 59.52 V 53 A 4.5 4.5 0 0 1 272.55 48.5 Z " fill="none" stroke="#000000" />
  <path d="M 525.26 49 H 571 A 4 4 0 0 1 575 53 V 59.52 A 4 4 0 0 1 571 63.52 H 525.26 A 4 4 0 0 1 521.26 59.52 V 53 A 4 4 0 0 1 525.26 49 Z " fill="#FFFFFF" fill-rule="nonzero" />
  <text x="521.26" y="49" fill="#000000" text-anchor="start" dominant-baseline="text-before-edge" font-size="12" font-family="Inter" xml:space="preserve">Top Right</text>
  <path d="M 525.26 48.5 H 571 A 4.5 4.5 0 0 1 575.5 53 V 59.52 A 4.5 4.5 0 0 1 571 64.02 H 525.26 A 4.5 4.5 0 0 1 520.76 59.52 V 53 A 4.5 4.5 0 0 1 525.26 48.5 Z " fill="none" stroke="#000000" />
  <path d="M 29 204.74 H 84.46 A 4 4 0 0 1 88.46 208.74 V 215.26 A 4 4 0 0 1 84.46 219.26 H 29 A 4 4 0 0 1 25 215.26 V 208.74 A 4 4 0 0 1 29 204.74 Z " fill="#FFFFFF" fill-rule="nonzero" />
  <text x="25" y="204.74" fill="#000000" text-anchor="start" dominant-baseline="text-before-edge" font-size="12" font-family="Inter" xml:space="preserve">Middle Left</text>
  <path d="M 29 204.24 H 84.46 A 4.5 4.5 0 0 1 88.96 208.74 V 215.26 A 4.5 4.5 0 0 1 84.46 219.76 H 29 A 4.5 4.5 0 0 1 24.5 215.26 V 208.74 A 4.5 4.5 0 0 1 29 204.24 Z " fill="none" stroke="#000000" />
  <path d="M 264.05 204.74 H 335.95 A 4 4 0 0 1 339.95 208.74 V 215.26 A 4 4 0 0 1 335.95 219.26 H 264.05 A 4 4 0 0 1 260.05 215.26 V 208.74 A 4 4 0 0 1 264.05 204.74 Z " fill="#FFFFFF" fill-rule="nonzero" />
  <text x="260.05" y="204.74" fill="#000000" text-anchor="start" dominant-baseline="text-before-edge" font-size="12" font-family="Inter" xml:space="preserve">Middle Center</text>
  <path d="M 264.05 204.24 H 335.95 A 4.5 4.5 0 0 1 340.45 208.74 V 215.26 A 4.5 4.5 0 0 1 335.95 219.76 H 264.05 A 4.5 4.5 0 0 1 259.55 215.26 V 208.74 A 4.5 4.5 0 0 1 264.05 204.24 Z " fill="none" stroke="#000000" />
  <path d="M 508.27 204.74 H 571 A 4 4 0 0 1 575 208.74 V 215.26 A 4 4 0 0 1 571 219.26 H 508.27 A 4 4 0 0 1 504.27 215.26 V 208.74 A 4 4 0 0 1 508.27 204.74 Z " fill="#FFFFFF" fill-rule="nonzero" />
  <text x="504.27" y="204.74" fill="#000000" text-anchor="start" dominant-baseline="text-before-edge" font-size="12" font-family="Inter" xml:space="preserve">Middle Right</text>
  <path d="M 508.27 204.24 H 571 A 4.5 4.5 0 0 1 575.5 208.74 V 215.26 A 4.5 4.5 0 0 1 571 219.76 H 508.27 A 4.5 4.5 0 0 1 503.77 215.26 V 208.74 A 4.5 4.5 0 0 1 508.27 204.24 Z " fill="none" stroke="#000000" />
  <path d="M 29 360.48 H 86.6 A 4 4 0 0 1 90.6 364.48 V 371 A 4 4 0 0 1 86.6 375 H 29 A 4 4 0 0 1 25 371 V 364.48 A 4 4 0 0 1 29 360.48 Z " fill="#FFFFFF" fill-rule="nonzero" />
  <text x="25" y="360.48" fill="#000000" text-anchor="start" dominant-baseline="text-before-edge" font-size="12" font-family="Inter" xml:space="preserve">Bottom Left</text>
  <path d="M 29 359.98 H 86.6 A 4.5 4.5 0 0 1 91.1 364.48 V 371 A 4.5 4.5 0 0 1 86.6 375.5 H 29 A 4.5 4.5 0 0 1 24.5 371 V 364.48 A 4.5 4.5 0 0 1 29 359.98 Z " fill="none" stroke="#000000" />
  <path d="M 262.98 360.48 H 337.02 A 4 4 0 0 1 341.02 364.48 V 371 A 4 4 0 0 1 337.02 375 H 262.98 A 4 4 0 0 1 258.98 371 V 364.48 A 4 4 0 0 1 262.98 360.48 Z " fill="#FFFFFF" fill-rule="nonzero" />
  <text x="258.98" y="360.48" fill="#000000" text-anchor="start" dominant-baseline="text-before-edge" font-size="12" font-family="Inter" xml:space="preserve">Bottom Center</text>
  <path d="M 262.98 359.98 H 337.02 A 4.5 4.5 0 0 1 341.52 364.48 V 371 A 4.5 4.5 0 0 1 337.02 375.5 H 262.98 A 4.5 4.5 0 0 1 258.48 371 V 364.48 A 4.5 4.5 0 0 1 262.98 359.98 Z " fill="none" stroke="#000000" />
  <path d="M 506.13 360.48 H 571 A 4 4 0 0 1 575 364.48 V 371 A 4 4 0 0 1 571 375 H 506.13 A 4 4 0 0 1 502.13 371 V 364.48 A 4 4 0 0 1 506.13 360.48 Z " fill="#FFFFFF" fill-rule="nonzero" />
  <text x="502.13" y="360.48" fill="#000000" text-anchor="start" dominant-baseline="text-before-edge" font-size="12" font-family="Inter" xml:space="preserve">Bottom Right</text>
  <path d="M 506.13 359.98 H 571 A 4.5 4.5 0 0 1 575.5 364.48 V 371 A 4.5 4.5 0 0 1 571 375.5 H 506.13 A 4.5 4.5 0 0 1 501.63 371 V 364.48 A 4.5 4.5 0 0 1 506.13 359.98 Z " fill="none" stroke="#000000" />
  <rect x="0" y="0" width="600" height="24" fill="#FFFFFF" />
  <text x="0" y="0" fill="#000000" text-anchor="start" dominant-baseline="text-before-edge" font-size="15" font-family="Inter" xml:space="preserve">&lt; Back</text>
  <text x="73.18" y="0" fill="#000000" text-anchor="start" dominant-baseline="text-before-edge" font-size="15" font-family="Inter" xml:space="preserve">ViewCollection Alignment</text>
  <g transform="translate(300 137.71142578125)" opacity="0.9">
    <polygon points="0,0 0,12 3,9 5,12 7,11 5,8 9,8" fill="white" stroke="black" stroke-width="0.7"/>
  </g>
</svg>
</div>

### Navigate: AnimatedHeart

### 06. AnimatedHeart

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

### Navigate: TextBox

### 07. TextBox

Status: ✅ Match

#### Expected

<div style="overflow:auto; border:1px solid #ddd; padding:8px; background:#fff;">
<svg xmlns="http://www.w3.org/2000/svg" width="600" height="400" viewBox="0 0 600 400">
  <text x="2" y="28" fill="#000000" text-anchor="start" dominant-baseline="text-before-edge" font-size="15" font-family="Inter" xml:space="preserve">Name:</text>
  <rect x="3" y="55.15" width="594" height="24.15" fill="#FFFFFF" />
  <rect x="2.5" y="54.65" width="595" height="25.15" fill="none" stroke="#808080" />
  <text x="2" y="88.3" fill="#000000" text-anchor="start" dominant-baseline="text-before-edge" font-size="15" font-family="Inter" xml:space="preserve">Password:</text>
  <rect x="3" y="115.45" width="594" height="24.15" fill="#FFFFFF" />
  <rect x="2.5" y="114.95" width="595" height="25.15" fill="none" stroke="#808080" />
  <text x="2" y="148.6" fill="#000000" text-anchor="start" dominant-baseline="text-before-edge" font-size="15" font-family="Inter" xml:space="preserve">Number:</text>
  <rect x="3" y="175.75" width="594" height="24.15" fill="#FFFFFF" />
  <text x="6" y="178.75" fill="#000000" text-anchor="start" dominant-baseline="text-before-edge" font-size="15" font-family="Inter" xml:space="preserve">0</text>
  <rect x="2.5" y="175.25" width="595" height="25.15" fill="none" stroke="#808080" />
  <text x="2" y="208.9" fill="#000000" text-anchor="start" dominant-baseline="text-before-edge" font-size="15" font-family="Inter" xml:space="preserve">Color (hex):</text>
  <rect x="3" y="236.05" width="594" height="24.15" fill="#FFFFFF" />
  <text x="6" y="239.05" fill="#000000" text-anchor="start" dominant-baseline="text-before-edge" font-size="15" font-family="Inter" xml:space="preserve">#FF0000</text>
  <rect x="2.5" y="235.55" width="595" height="25.15" fill="none" stroke="#808080" />
  <rect x="0" y="0" width="600" height="24" fill="#FFFFFF" />
  <text x="0" y="0" fill="#000000" text-anchor="start" dominant-baseline="text-before-edge" font-size="15" font-family="Inter" xml:space="preserve">&lt; Back</text>
  <text x="73.18" y="0" fill="#000000" text-anchor="start" dominant-baseline="text-before-edge" font-size="15" font-family="Inter" xml:space="preserve">TextBox MVP</text>
  <g transform="translate(300 186.01025390625)" opacity="0.9">
    <polygon points="0,0 0,12 3,9 5,12 7,11 5,8 9,8" fill="white" stroke="black" stroke-width="0.7"/>
  </g>
</svg>
</div>

#### Actual

<div style="overflow:auto; border:1px solid #ddd; padding:8px; background:#fff;">
<svg xmlns="http://www.w3.org/2000/svg" width="600" height="400" viewBox="0 0 600 400">
  <text x="2" y="28" fill="#000000" text-anchor="start" dominant-baseline="text-before-edge" font-size="15" font-family="Inter" xml:space="preserve">Name:</text>
  <rect x="3" y="55.15" width="594" height="24.15" fill="#FFFFFF" />
  <rect x="2.5" y="54.65" width="595" height="25.15" fill="none" stroke="#808080" />
  <text x="2" y="88.3" fill="#000000" text-anchor="start" dominant-baseline="text-before-edge" font-size="15" font-family="Inter" xml:space="preserve">Password:</text>
  <rect x="3" y="115.45" width="594" height="24.15" fill="#FFFFFF" />
  <rect x="2.5" y="114.95" width="595" height="25.15" fill="none" stroke="#808080" />
  <text x="2" y="148.6" fill="#000000" text-anchor="start" dominant-baseline="text-before-edge" font-size="15" font-family="Inter" xml:space="preserve">Number:</text>
  <rect x="3" y="175.75" width="594" height="24.15" fill="#FFFFFF" />
  <text x="6" y="178.75" fill="#000000" text-anchor="start" dominant-baseline="text-before-edge" font-size="15" font-family="Inter" xml:space="preserve">0</text>
  <rect x="2.5" y="175.25" width="595" height="25.15" fill="none" stroke="#808080" />
  <text x="2" y="208.9" fill="#000000" text-anchor="start" dominant-baseline="text-before-edge" font-size="15" font-family="Inter" xml:space="preserve">Color (hex):</text>
  <rect x="3" y="236.05" width="594" height="24.15" fill="#FFFFFF" />
  <text x="6" y="239.05" fill="#000000" text-anchor="start" dominant-baseline="text-before-edge" font-size="15" font-family="Inter" xml:space="preserve">#FF0000</text>
  <rect x="2.5" y="235.55" width="595" height="25.15" fill="none" stroke="#808080" />
  <rect x="0" y="0" width="600" height="24" fill="#FFFFFF" />
  <text x="0" y="0" fill="#000000" text-anchor="start" dominant-baseline="text-before-edge" font-size="15" font-family="Inter" xml:space="preserve">&lt; Back</text>
  <text x="73.18" y="0" fill="#000000" text-anchor="start" dominant-baseline="text-before-edge" font-size="15" font-family="Inter" xml:space="preserve">TextBox MVP</text>
  <g transform="translate(300 186.01025390625)" opacity="0.9">
    <polygon points="0,0 0,12 3,9 5,12 7,11 5,8 9,8" fill="white" stroke="black" stroke-width="0.7"/>
  </g>
</svg>
</div>
