# Xui
A cross platform .NET UI framework.

## Architecture
### Core
 - `Xui/Core/Core/Xui.Core.csproj`  
   Contains ground code with abstractions and interfaces for all platforms.
   - `Xui.Core.Abstract` - abstract Application, Window, HotReload
   - `Xui.Core.Abstract.Events` - Web-lize input events, zero-heap allocation, all events are allocated as ref structs and supposed to travel up and down the window and view chain, without allocating event objects in heap
   - `Xui.Core.Actual` - interfaces for actual implementations (macOS, Windows, iOS) handling native interop for the abstracts
   - `Xui.Core.Animation` - primitive math for animations - easing, curves, etc.
   - `Xui.Core.Canvas` - Web-like, zero-heap-alloc interface for 2D canvas, the root of 2D display rendering
   - `Xui.Core.Canvas.SVG` - Primitives for Canvas to SVG and vice versa convertion
   - `Xui.Core.Curves2D` - Math related to 2D math for curves and arcs, verious presentations and convertions
   - `Xui.Core.Math2D` - Matrix, Point, Vector, Size, operators and convertions
   - `Xui.Core.Memory` - Auxiliaries for zero-heap-allocation operations
   - `Xui.Core.Middleware` - Application developers provide "Abstract" window, Xui platforms provide a native "Actual" counterpart. Sometimes (like when working with emulators, or in testing) - "Actual" native window is connected to "Middleware" mapping pointer to touch events and drawing a phone frame, then connecting that to "Abstract" mobile app.
   - `Xui.Core.Set` - Math primitives from set theory
   - `Xui.Core.UI` - Base UI primitives, RootView dispatching window events into View hierarchy, View, basic layout views, ViewCollection, input propagation. These are supposed to compose the UI hierarchies within an Abstract window. Techincally the window can handle events and provide the rendering without any Views, but most 2D heavy apps will segment the UI into View hierarchies.
 - `Xui/Core/Core.Tests/Xui.Core.Tests.csproj` - Unit tests for Xui.Core
 - `Xui/Core/Fonts/Xui.Core.Fonts.csproj` - Achieving similar rendering on all platforms we re-ship "Inter"

### Middleware
 - `Xui/Middleware/Emulator/Emulator.csproj` - Classes often bridging desktop and mobile - software emulator
 - `Xui/Middleware/Test/Test.csproj` - Infrastructure used to simulate a platform within a Unit testing framework, may be replaced by utilities in Xui/Runtime/Software.

### Runtime
 - `Xui/Runtime/Android/Android.csproj` - Android implementation of the "Actual" core interfaces. The Android platform is the only one not exposing directly C APIs for 2D, although it does ship a version of Skia built-in and bound theough Java JNI. The plan is to implement C# software text measure interfaces, and binary render list where C# would produce and Java would consume to render in attempt to keep the footprint of Xui libraries small, yet achieving high performance by minimizing marshalling and interop.
 - `Xui/Runtime/Browser/Browser.csproj` - WASM C# implementing the "Actual" core interfaces through a Browser canvas. Suitable for preview, demo and doc purposes.
 - `Xui/Runtime/IOS/IOS.csproj` - A UIKit and Objective-C interop implementing the "Actual" core interfaces. CoreAnimation, CoreFoundation, CoreGraphics, CoreText C# wrappers.
 - `Xui/Runtime/MacOS/MacOS.csproj` - An AppKit and Objective-C interop implementing the "Actual" core interfaces. CoreAnimation, CoreFoundation, CoreGraphics, CoreText C# wrappers.
 - `Xui/Runtime/Software/Sotfware.csproj` - A software implementation of a platform. Used for simulation, extended text measure capabilities in a Browser, testing.  
   - `Xui.Runtime.Software.Actual` - Software versions of window, drawing context, etc. Used for simulation and assertion. The SvgDrawingContext implement the Canvas interface but produces an SVG that can be serialized and deserialized for display in design surface or asserted as snapshots in testing.
   - `Xui.Runtime.Software.Font` - Font capabilites, glyph paths, text metrics and measure with kerning, currently supporting TrueTypeFont (like Inter)
   - `Xui.Runtime.Software.Rasterization` - Vector to raster APIs, currently incomplete
   - `Xui.Runtime.Software.Render` - Bitmap structures
   - `Xui.Runtime.Software.Tesselate` - Vector tesselation
 - `Xui/Runtime/Windows/Windows.csproj` - A Windows implementation of a platform. Using Win32 for input and runloop, COM objects as ref counted native objects. Direct2D, Direct3D, DirectComposition, DXGI, DirectWrite wrappers.

### Testing
#### Unit Tests
Unit tests are located close to the projects they relate to.
 - `Xui/Core/Core.Tests/Core.Tests.csproj` - Tests for Xui.Core

#### Component and Integration Tests
Component and integration test are separated and refer multiple projects.
 - `Xui/Tests/Component/Xui.Tests.Component.csproj` - Component testing, mocks using the software platform testing capabilities of Views and other components by rendering in SVG and comparing with snapshots.

### Test and Demo Apps
 - `Xui/Apps/ClockApp` - A low level Window-only app used to implement a simple chrome-less window and test rendering and animation timing events
 - `Xui/Apps/LoadTestApp` - A low level Window-only app used to simulate infinite scrolling grid, and display overlay with FPS, times and memory pressure
 - `Xui/Apps/BlankApp` - A playground
 - `Xui/Apps/TestApp` - A test app utilizing simple navigation with home-page and example pages, used to develop view hierarchies, events, view flags, view rendering and animations

## Important Constructs
### Types
Use NFloat or aliased nfloat everywhere a float or double would be used.

### Memory
#### Short Lived, Single Call
Any objects that live for a single call - use `ref struct`.
This involves providing native interop that would use ref-counted (Objective-C Retain/Relase, Windows COM Add/Release) that can be called by the `ref struct`'s IDisposable Dispose method. Usually suffixed with `Ref`, or nested structure called ref, e.g. `NSStringRef` or `D2D11.SolidColorBrush.Ref`.

#### Owned Resource
Any objects that live for a few calls. E.g. SetFillColor for gradient color brush, that would be used by multiple Fill calls. Use a `struct` implementing IDisposable and capture in a class whose Dispose method would dispose the structure. Usually named with `Ptr` suffix. E.g. `D2D11.SolidColorBrushPtr`.

#### Long-lived Resource
A class wrapping a `void*` pointer to ref-coutned object. Often used to wrap Objective-C or COM object that would live for multiple frames. Like a native Window or View, Direct2D factories and render targets.

