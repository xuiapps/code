# Integration Test Run

## Snapshot summary

- ✅ 01. Tab.NameBox
- ✅ 02. Tab.PasswordBox
- ✅ 03. Tab.NumberBox
- ✅ 04. Tab.ColorBox
- ✅ 05. Tab.WrapToNameBox
- ✅ 06. ShiftTab.ColorBox

## Timeline

## Scenario

Traverse editable controls with Tab and Shift+Tab.

- Tab to NameBox, then PasswordBox, NumberBox, ColorBox.
- Tab wraps to NameBox.
- Shift+Tab moves focus backward.

### 01. Tab.NameBox

Status: ✅ Match

#### Expected

<div style="overflow:auto; border:1px solid #ddd; padding:8px; background:#fff;">
<svg xmlns="http://www.w3.org/2000/svg" width="600" height="400" viewBox="0 0 600 400">
  <text x="2" y="28" fill="#000000" text-anchor="start" dominant-baseline="text-before-edge" font-size="15" font-family="Inter" xml:space="preserve">Name:</text>
  <rect x="3" y="55.15" width="594" height="24.15" fill="#FFFFFF" />
  <text x="6" y="58.15" fill="#000000" text-anchor="start" dominant-baseline="text-before-edge" font-size="15" font-family="Inter" xml:space="preserve">Tab1</text>
  <rect x="37.79" y="58.15" width="1" height="18.15" fill="#000000" />
  <rect x="2.5" y="54.65" width="595" height="25.15" fill="none" stroke="#0000FF" />
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
  <text x="6" y="58.15" fill="#000000" text-anchor="start" dominant-baseline="text-before-edge" font-size="15" font-family="Inter" xml:space="preserve">Tab1</text>
  <rect x="37.79" y="58.15" width="1" height="18.15" fill="#000000" />
  <rect x="2.5" y="54.65" width="595" height="25.15" fill="none" stroke="#0000FF" />
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

### 02. Tab.PasswordBox

Status: ✅ Match

#### Expected

<div style="overflow:auto; border:1px solid #ddd; padding:8px; background:#fff;">
<svg xmlns="http://www.w3.org/2000/svg" width="600" height="400" viewBox="0 0 600 400">
  <text x="2" y="28" fill="#000000" text-anchor="start" dominant-baseline="text-before-edge" font-size="15" font-family="Inter" xml:space="preserve">Name:</text>
  <rect x="3" y="55.15" width="594" height="24.15" fill="#FFFFFF" />
  <text x="6" y="58.15" fill="#000000" text-anchor="start" dominant-baseline="text-before-edge" font-size="15" font-family="Inter" xml:space="preserve">Tab1</text>
  <rect x="2.5" y="54.65" width="595" height="25.15" fill="none" stroke="#808080" />
  <text x="2" y="88.3" fill="#000000" text-anchor="start" dominant-baseline="text-before-edge" font-size="15" font-family="Inter" xml:space="preserve">Password:</text>
  <rect x="3" y="115.45" width="594" height="24.15" fill="#FFFFFF" />
  <text x="6" y="118.45" fill="#000000" text-anchor="start" dominant-baseline="text-before-edge" font-size="15" font-family="Inter" xml:space="preserve">••••</text>
  <rect x="39.75" y="118.45" width="1" height="18.15" fill="#000000" />
  <rect x="2.5" y="114.95" width="595" height="25.15" fill="none" stroke="#0000FF" />
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
  <text x="6" y="58.15" fill="#000000" text-anchor="start" dominant-baseline="text-before-edge" font-size="15" font-family="Inter" xml:space="preserve">Tab1</text>
  <rect x="2.5" y="54.65" width="595" height="25.15" fill="none" stroke="#808080" />
  <text x="2" y="88.3" fill="#000000" text-anchor="start" dominant-baseline="text-before-edge" font-size="15" font-family="Inter" xml:space="preserve">Password:</text>
  <rect x="3" y="115.45" width="594" height="24.15" fill="#FFFFFF" />
  <text x="6" y="118.45" fill="#000000" text-anchor="start" dominant-baseline="text-before-edge" font-size="15" font-family="Inter" xml:space="preserve">••••</text>
  <rect x="39.75" y="118.45" width="1" height="18.15" fill="#000000" />
  <rect x="2.5" y="114.95" width="595" height="25.15" fill="none" stroke="#0000FF" />
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

### 03. Tab.NumberBox

Status: ✅ Match

#### Expected

<div style="overflow:auto; border:1px solid #ddd; padding:8px; background:#fff;">
<svg xmlns="http://www.w3.org/2000/svg" width="600" height="400" viewBox="0 0 600 400">
  <text x="2" y="28" fill="#000000" text-anchor="start" dominant-baseline="text-before-edge" font-size="15" font-family="Inter" xml:space="preserve">Name:</text>
  <rect x="3" y="55.15" width="594" height="24.15" fill="#FFFFFF" />
  <text x="6" y="58.15" fill="#000000" text-anchor="start" dominant-baseline="text-before-edge" font-size="15" font-family="Inter" xml:space="preserve">Tab1</text>
  <rect x="2.5" y="54.65" width="595" height="25.15" fill="none" stroke="#808080" />
  <text x="2" y="88.3" fill="#000000" text-anchor="start" dominant-baseline="text-before-edge" font-size="15" font-family="Inter" xml:space="preserve">Password:</text>
  <rect x="3" y="115.45" width="594" height="24.15" fill="#FFFFFF" />
  <text x="6" y="118.45" fill="#000000" text-anchor="start" dominant-baseline="text-before-edge" font-size="15" font-family="Inter" xml:space="preserve">••••</text>
  <rect x="2.5" y="114.95" width="595" height="25.15" fill="none" stroke="#808080" />
  <text x="2" y="148.6" fill="#000000" text-anchor="start" dominant-baseline="text-before-edge" font-size="15" font-family="Inter" xml:space="preserve">Number:</text>
  <rect x="3" y="175.75" width="594" height="24.15" fill="#FFFFFF" />
  <path d="M 6 178.75 H 15.46 A 0 0 0 0 1 15.46 178.75 V 196.9 A 0 0 0 0 1 15.46 196.9 H 6 A 0 0 0 0 1 6 196.9 V 178.75 A 0 0 0 0 1 6 178.75 Z " fill="#0000FF" fill-rule="nonzero" />
  <text x="6" y="178.75" fill="#FFFFFF" text-anchor="start" dominant-baseline="text-before-edge" font-size="15" font-family="Inter" xml:space="preserve">0</text>
  <rect x="2.5" y="175.25" width="595" height="25.15" fill="none" stroke="#0000FF" />
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
  <text x="6" y="58.15" fill="#000000" text-anchor="start" dominant-baseline="text-before-edge" font-size="15" font-family="Inter" xml:space="preserve">Tab1</text>
  <rect x="2.5" y="54.65" width="595" height="25.15" fill="none" stroke="#808080" />
  <text x="2" y="88.3" fill="#000000" text-anchor="start" dominant-baseline="text-before-edge" font-size="15" font-family="Inter" xml:space="preserve">Password:</text>
  <rect x="3" y="115.45" width="594" height="24.15" fill="#FFFFFF" />
  <text x="6" y="118.45" fill="#000000" text-anchor="start" dominant-baseline="text-before-edge" font-size="15" font-family="Inter" xml:space="preserve">••••</text>
  <rect x="2.5" y="114.95" width="595" height="25.15" fill="none" stroke="#808080" />
  <text x="2" y="148.6" fill="#000000" text-anchor="start" dominant-baseline="text-before-edge" font-size="15" font-family="Inter" xml:space="preserve">Number:</text>
  <rect x="3" y="175.75" width="594" height="24.15" fill="#FFFFFF" />
  <path d="M 6 178.75 H 15.46 A 0 0 0 0 1 15.46 178.75 V 196.9 A 0 0 0 0 1 15.46 196.9 H 6 A 0 0 0 0 1 6 196.9 V 178.75 A 0 0 0 0 1 6 178.75 Z " fill="#0000FF" fill-rule="nonzero" />
  <text x="6" y="178.75" fill="#FFFFFF" text-anchor="start" dominant-baseline="text-before-edge" font-size="15" font-family="Inter" xml:space="preserve">0</text>
  <rect x="2.5" y="175.25" width="595" height="25.15" fill="none" stroke="#0000FF" />
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

### 04. Tab.ColorBox

Status: ✅ Match

#### Expected

<div style="overflow:auto; border:1px solid #ddd; padding:8px; background:#fff;">
<svg xmlns="http://www.w3.org/2000/svg" width="600" height="400" viewBox="0 0 600 400">
  <text x="2" y="28" fill="#000000" text-anchor="start" dominant-baseline="text-before-edge" font-size="15" font-family="Inter" xml:space="preserve">Name:</text>
  <rect x="3" y="55.15" width="594" height="24.15" fill="#FFFFFF" />
  <text x="6" y="58.15" fill="#000000" text-anchor="start" dominant-baseline="text-before-edge" font-size="15" font-family="Inter" xml:space="preserve">Tab1</text>
  <rect x="2.5" y="54.65" width="595" height="25.15" fill="none" stroke="#808080" />
  <text x="2" y="88.3" fill="#000000" text-anchor="start" dominant-baseline="text-before-edge" font-size="15" font-family="Inter" xml:space="preserve">Password:</text>
  <rect x="3" y="115.45" width="594" height="24.15" fill="#FFFFFF" />
  <text x="6" y="118.45" fill="#000000" text-anchor="start" dominant-baseline="text-before-edge" font-size="15" font-family="Inter" xml:space="preserve">••••</text>
  <rect x="2.5" y="114.95" width="595" height="25.15" fill="none" stroke="#808080" />
  <text x="2" y="148.6" fill="#000000" text-anchor="start" dominant-baseline="text-before-edge" font-size="15" font-family="Inter" xml:space="preserve">Number:</text>
  <rect x="3" y="175.75" width="594" height="24.15" fill="#FFFFFF" />
  <text x="6" y="178.75" fill="#000000" text-anchor="start" dominant-baseline="text-before-edge" font-size="15" font-family="Inter" xml:space="preserve">0</text>
  <rect x="2.5" y="175.25" width="595" height="25.15" fill="none" stroke="#808080" />
  <text x="2" y="208.9" fill="#000000" text-anchor="start" dominant-baseline="text-before-edge" font-size="15" font-family="Inter" xml:space="preserve">Color (hex):</text>
  <rect x="3" y="236.05" width="594" height="24.15" fill="#FFFFFF" />
  <path d="M 6 239.05 H 71.06 A 0 0 0 0 1 71.06 239.05 V 257.2 A 0 0 0 0 1 71.06 257.2 H 6 A 0 0 0 0 1 6 257.2 V 239.05 A 0 0 0 0 1 6 239.05 Z " fill="#0000FF" fill-rule="nonzero" />
  <text x="6" y="239.05" fill="#FFFFFF" text-anchor="start" dominant-baseline="text-before-edge" font-size="15" font-family="Inter" xml:space="preserve">#FF0000</text>
  <rect x="2.5" y="235.55" width="595" height="25.15" fill="none" stroke="#0000FF" />
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
  <text x="6" y="58.15" fill="#000000" text-anchor="start" dominant-baseline="text-before-edge" font-size="15" font-family="Inter" xml:space="preserve">Tab1</text>
  <rect x="2.5" y="54.65" width="595" height="25.15" fill="none" stroke="#808080" />
  <text x="2" y="88.3" fill="#000000" text-anchor="start" dominant-baseline="text-before-edge" font-size="15" font-family="Inter" xml:space="preserve">Password:</text>
  <rect x="3" y="115.45" width="594" height="24.15" fill="#FFFFFF" />
  <text x="6" y="118.45" fill="#000000" text-anchor="start" dominant-baseline="text-before-edge" font-size="15" font-family="Inter" xml:space="preserve">••••</text>
  <rect x="2.5" y="114.95" width="595" height="25.15" fill="none" stroke="#808080" />
  <text x="2" y="148.6" fill="#000000" text-anchor="start" dominant-baseline="text-before-edge" font-size="15" font-family="Inter" xml:space="preserve">Number:</text>
  <rect x="3" y="175.75" width="594" height="24.15" fill="#FFFFFF" />
  <text x="6" y="178.75" fill="#000000" text-anchor="start" dominant-baseline="text-before-edge" font-size="15" font-family="Inter" xml:space="preserve">0</text>
  <rect x="2.5" y="175.25" width="595" height="25.15" fill="none" stroke="#808080" />
  <text x="2" y="208.9" fill="#000000" text-anchor="start" dominant-baseline="text-before-edge" font-size="15" font-family="Inter" xml:space="preserve">Color (hex):</text>
  <rect x="3" y="236.05" width="594" height="24.15" fill="#FFFFFF" />
  <path d="M 6 239.05 H 71.06 A 0 0 0 0 1 71.06 239.05 V 257.2 A 0 0 0 0 1 71.06 257.2 H 6 A 0 0 0 0 1 6 257.2 V 239.05 A 0 0 0 0 1 6 239.05 Z " fill="#0000FF" fill-rule="nonzero" />
  <text x="6" y="239.05" fill="#FFFFFF" text-anchor="start" dominant-baseline="text-before-edge" font-size="15" font-family="Inter" xml:space="preserve">#FF0000</text>
  <rect x="2.5" y="235.55" width="595" height="25.15" fill="none" stroke="#0000FF" />
  <rect x="0" y="0" width="600" height="24" fill="#FFFFFF" />
  <text x="0" y="0" fill="#000000" text-anchor="start" dominant-baseline="text-before-edge" font-size="15" font-family="Inter" xml:space="preserve">&lt; Back</text>
  <text x="73.18" y="0" fill="#000000" text-anchor="start" dominant-baseline="text-before-edge" font-size="15" font-family="Inter" xml:space="preserve">TextBox MVP</text>
  <g transform="translate(300 186.01025390625)" opacity="0.9">
    <polygon points="0,0 0,12 3,9 5,12 7,11 5,8 9,8" fill="white" stroke="black" stroke-width="0.7"/>
  </g>
</svg>
</div>

### 05. Tab.WrapToNameBox

Status: ✅ Match

#### Expected

<div style="overflow:auto; border:1px solid #ddd; padding:8px; background:#fff;">
<svg xmlns="http://www.w3.org/2000/svg" width="600" height="400" viewBox="0 0 600 400">
  <text x="2" y="28" fill="#000000" text-anchor="start" dominant-baseline="text-before-edge" font-size="15" font-family="Inter" xml:space="preserve">Name:</text>
  <rect x="3" y="55.15" width="594" height="24.15" fill="#FFFFFF" />
  <path d="M 6 58.15 H 37.79 A 0 0 0 0 1 37.79 58.15 V 76.3 A 0 0 0 0 1 37.79 76.3 H 6 A 0 0 0 0 1 6 76.3 V 58.15 A 0 0 0 0 1 6 58.15 Z " fill="#0000FF" fill-rule="nonzero" />
  <text x="6" y="58.15" fill="#FFFFFF" text-anchor="start" dominant-baseline="text-before-edge" font-size="15" font-family="Inter" xml:space="preserve">Tab1</text>
  <rect x="2.5" y="54.65" width="595" height="25.15" fill="none" stroke="#0000FF" />
  <text x="2" y="88.3" fill="#000000" text-anchor="start" dominant-baseline="text-before-edge" font-size="15" font-family="Inter" xml:space="preserve">Password:</text>
  <rect x="3" y="115.45" width="594" height="24.15" fill="#FFFFFF" />
  <text x="6" y="118.45" fill="#000000" text-anchor="start" dominant-baseline="text-before-edge" font-size="15" font-family="Inter" xml:space="preserve">••••</text>
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
  <path d="M 6 58.15 H 37.79 A 0 0 0 0 1 37.79 58.15 V 76.3 A 0 0 0 0 1 37.79 76.3 H 6 A 0 0 0 0 1 6 76.3 V 58.15 A 0 0 0 0 1 6 58.15 Z " fill="#0000FF" fill-rule="nonzero" />
  <text x="6" y="58.15" fill="#FFFFFF" text-anchor="start" dominant-baseline="text-before-edge" font-size="15" font-family="Inter" xml:space="preserve">Tab1</text>
  <rect x="2.5" y="54.65" width="595" height="25.15" fill="none" stroke="#0000FF" />
  <text x="2" y="88.3" fill="#000000" text-anchor="start" dominant-baseline="text-before-edge" font-size="15" font-family="Inter" xml:space="preserve">Password:</text>
  <rect x="3" y="115.45" width="594" height="24.15" fill="#FFFFFF" />
  <text x="6" y="118.45" fill="#000000" text-anchor="start" dominant-baseline="text-before-edge" font-size="15" font-family="Inter" xml:space="preserve">••••</text>
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

### 06. ShiftTab.ColorBox

Status: ✅ Match

#### Expected

<div style="overflow:auto; border:1px solid #ddd; padding:8px; background:#fff;">
<svg xmlns="http://www.w3.org/2000/svg" width="600" height="400" viewBox="0 0 600 400">
  <text x="2" y="28" fill="#000000" text-anchor="start" dominant-baseline="text-before-edge" font-size="15" font-family="Inter" xml:space="preserve">Name:</text>
  <rect x="3" y="55.15" width="594" height="24.15" fill="#FFFFFF" />
  <text x="6" y="58.15" fill="#000000" text-anchor="start" dominant-baseline="text-before-edge" font-size="15" font-family="Inter" xml:space="preserve">Tab1</text>
  <rect x="2.5" y="54.65" width="595" height="25.15" fill="none" stroke="#808080" />
  <text x="2" y="88.3" fill="#000000" text-anchor="start" dominant-baseline="text-before-edge" font-size="15" font-family="Inter" xml:space="preserve">Password:</text>
  <rect x="3" y="115.45" width="594" height="24.15" fill="#FFFFFF" />
  <text x="6" y="118.45" fill="#000000" text-anchor="start" dominant-baseline="text-before-edge" font-size="15" font-family="Inter" xml:space="preserve">••••</text>
  <rect x="2.5" y="114.95" width="595" height="25.15" fill="none" stroke="#808080" />
  <text x="2" y="148.6" fill="#000000" text-anchor="start" dominant-baseline="text-before-edge" font-size="15" font-family="Inter" xml:space="preserve">Number:</text>
  <rect x="3" y="175.75" width="594" height="24.15" fill="#FFFFFF" />
  <text x="6" y="178.75" fill="#000000" text-anchor="start" dominant-baseline="text-before-edge" font-size="15" font-family="Inter" xml:space="preserve">0</text>
  <rect x="2.5" y="175.25" width="595" height="25.15" fill="none" stroke="#808080" />
  <text x="2" y="208.9" fill="#000000" text-anchor="start" dominant-baseline="text-before-edge" font-size="15" font-family="Inter" xml:space="preserve">Color (hex):</text>
  <rect x="3" y="236.05" width="594" height="24.15" fill="#FFFFFF" />
  <path d="M 6 239.05 H 71.06 A 0 0 0 0 1 71.06 239.05 V 257.2 A 0 0 0 0 1 71.06 257.2 H 6 A 0 0 0 0 1 6 257.2 V 239.05 A 0 0 0 0 1 6 239.05 Z " fill="#0000FF" fill-rule="nonzero" />
  <text x="6" y="239.05" fill="#FFFFFF" text-anchor="start" dominant-baseline="text-before-edge" font-size="15" font-family="Inter" xml:space="preserve">#FF0000</text>
  <rect x="2.5" y="235.55" width="595" height="25.15" fill="none" stroke="#0000FF" />
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
  <text x="6" y="58.15" fill="#000000" text-anchor="start" dominant-baseline="text-before-edge" font-size="15" font-family="Inter" xml:space="preserve">Tab1</text>
  <rect x="2.5" y="54.65" width="595" height="25.15" fill="none" stroke="#808080" />
  <text x="2" y="88.3" fill="#000000" text-anchor="start" dominant-baseline="text-before-edge" font-size="15" font-family="Inter" xml:space="preserve">Password:</text>
  <rect x="3" y="115.45" width="594" height="24.15" fill="#FFFFFF" />
  <text x="6" y="118.45" fill="#000000" text-anchor="start" dominant-baseline="text-before-edge" font-size="15" font-family="Inter" xml:space="preserve">••••</text>
  <rect x="2.5" y="114.95" width="595" height="25.15" fill="none" stroke="#808080" />
  <text x="2" y="148.6" fill="#000000" text-anchor="start" dominant-baseline="text-before-edge" font-size="15" font-family="Inter" xml:space="preserve">Number:</text>
  <rect x="3" y="175.75" width="594" height="24.15" fill="#FFFFFF" />
  <text x="6" y="178.75" fill="#000000" text-anchor="start" dominant-baseline="text-before-edge" font-size="15" font-family="Inter" xml:space="preserve">0</text>
  <rect x="2.5" y="175.25" width="595" height="25.15" fill="none" stroke="#808080" />
  <text x="2" y="208.9" fill="#000000" text-anchor="start" dominant-baseline="text-before-edge" font-size="15" font-family="Inter" xml:space="preserve">Color (hex):</text>
  <rect x="3" y="236.05" width="594" height="24.15" fill="#FFFFFF" />
  <path d="M 6 239.05 H 71.06 A 0 0 0 0 1 71.06 239.05 V 257.2 A 0 0 0 0 1 71.06 257.2 H 6 A 0 0 0 0 1 6 257.2 V 239.05 A 0 0 0 0 1 6 239.05 Z " fill="#0000FF" fill-rule="nonzero" />
  <text x="6" y="239.05" fill="#FFFFFF" text-anchor="start" dominant-baseline="text-before-edge" font-size="15" font-family="Inter" xml:space="preserve">#FF0000</text>
  <rect x="2.5" y="235.55" width="595" height="25.15" fill="none" stroke="#0000FF" />
  <rect x="0" y="0" width="600" height="24" fill="#FFFFFF" />
  <text x="0" y="0" fill="#000000" text-anchor="start" dominant-baseline="text-before-edge" font-size="15" font-family="Inter" xml:space="preserve">&lt; Back</text>
  <text x="73.18" y="0" fill="#000000" text-anchor="start" dominant-baseline="text-before-edge" font-size="15" font-family="Inter" xml:space="preserve">TextBox MVP</text>
  <g transform="translate(300 186.01025390625)" opacity="0.9">
    <polygon points="0,0 0,12 3,9 5,12 7,11 5,8 9,8" fill="white" stroke="black" stroke-width="0.7"/>
  </g>
</svg>
</div>
