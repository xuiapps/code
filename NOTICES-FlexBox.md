# FlexBox Layout Implementation Notices

## CSS Flexible Box Layout Implementation

### Implementation Details

The FlexBox layout implementation in `Xui.Core.UI.Layout.FlexBox.Layout.cs` is an original implementation following the W3C CSS Flexible Box Layout Module Level 1 specification.

**Implementation Date:** March 2026  
**Author:** Xui Development Team  
**License:** Same as Xui project license

### Specification References

This implementation follows the W3C CSS Flexible Box Layout Module Level 1 specification:
- **Specification:** https://www.w3.org/TR/css-flexbox-1/
- **Status:** W3C Candidate Recommendation
- **Copyright:** W3C (MIT, ERCIM, Keio, Beihang)

The specification is used as a reference for behavior and terminology. No code was copied from any implementation.

### Research and Inspiration

During development, the following open-source implementations were researched for algorithm understanding:

1. **Taffy** (Rust, MIT License)
   - Repository: https://github.com/DioxusLabs/taffy
   - Used for: Understanding CSS FlexBox layout algorithm architecture
   - No code was directly ported; only used for conceptual reference
   - Copyright: The Taffy Contributors (based on Visly's stretch)

2. **Yoga** (C++, MIT License)
   - Repository: https://github.com/facebook/yoga
   - Used for: Understanding mobile-optimized flexbox implementations
   - No code was directly ported; only used for conceptual reference
   - Copyright: Facebook, Inc. and its affiliates

3. **CSS Flexible Box Layout Specification**
   - Used for: Understanding expected behavior and terminology
   - Reference: https://www.w3.org/TR/css-flexbox-1/

### Implementation Approach

The Xui FlexBox implementation is a clean-room implementation designed specifically for the Xui layout system:

1. **Flex Container:** Establishes a flex formatting context for children
2. **Main Axis & Cross Axis:** Supports flex-direction (row, row-reverse, column, column-reverse)
3. **Flex Line:** Single-line (nowrap) or multi-line (wrap, wrap-reverse) layouts
4. **Flex Item Sizing:** Resolves flex-basis, applies flex-grow and flex-shrink with available space distribution
5. **Alignment:** 
   - Main axis: justify-content (flex-start, flex-end, center, space-between, space-around, space-evenly)
   - Cross axis: align-items (stretch, flex-start, flex-end, center, baseline)
   - Multi-line cross axis: align-content (stretch, flex-start, flex-end, center, space-between, space-around, space-evenly)
6. **Gaps:** Applies row-gap and column-gap spacing between items/lines

### Algorithm Overview

The implementation follows these key steps:

1. **Determine flex items:** Collect children and apply order property
2. **Resolve flex directions:** Establish main axis and cross axis based on flex-direction
3. **Determine flex basis:** Calculate hypothetical main size for each item
4. **Collect flex lines:** Group items into flex lines based on flex-wrap
5. **Resolve flexible lengths:** Distribute remaining space using flex-grow/shrink
6. **Cross-axis sizing:** Calculate cross-axis sizes (handle stretch alignment)
7. **Main-axis alignment:** Position items along main axis (justify-content)
8. **Cross-axis alignment:** Position items/lines along cross axis (align-items, align-content)
9. **Position flex items:** Arrange each item with final bounds

---

## Acknowledgments

We acknowledge the W3C CSS Working Group for the CSS Flexible Box Layout specification, which provided the foundation for understanding CSS FlexBox behavior.

We also acknowledge the open-source community, particularly the Taffy and Yoga projects, for providing reference implementations that helped us understand complex layout algorithms. While no code was directly ported, these projects were invaluable for understanding algorithm structure and edge cases.

## MIT License References

Both Taffy and Yoga are released under the MIT License, which permits use for understanding and reference:

### Taffy MIT License
```
MIT License

Copyright (c) The Taffy Contributors

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
```

### Yoga MIT License
```
MIT License

Copyright (c) Facebook, Inc. and its affiliates.

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
```
