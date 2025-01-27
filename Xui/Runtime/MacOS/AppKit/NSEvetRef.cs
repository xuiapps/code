using System.Runtime.InteropServices;
using static Xui.Runtime.MacOS.CoreFoundation;
using static Xui.Runtime.MacOS.Foundation;
using static Xui.Runtime.MacOS.ObjC;

namespace Xui.Runtime.MacOS;

public static partial class AppKit
{
    public ref partial struct NSEventRef
    {
        private static readonly Sel TypeSel = new Sel("type");

        private static readonly Sel SubtypeSel = new Sel("subtype");

        private static readonly Sel LocationInWindowSel = new Sel("locationInWindow");

        private static readonly Sel ScrollingDeltaXSel = new Sel("scrollingDeltaX");

        private static readonly Sel ScrollingDeltaYSel = new Sel("scrollingDeltaY");

        private static readonly Sel CharactersSel = new Sel("characters");

        private static readonly Sel UTF8StringSel = new Sel("UTF8String");

        private static readonly Sel KeyCodeSel = new Sel("keyCode");

        private static readonly Sel PressureSel = new Sel("pressure");

        private static readonly Sel PressedMouseButtonsSel = new Sel("pressedMouseButtons");

        public readonly nint Self;

        public NSEventRef(nint self)
        {
            if (self == 0)
            {
                throw new ObjCException($"{nameof(NSEventRef)} instantiated with nil self.");
            }

            this.Self = self;
        }

        public NSEventType Type => (NSEventType)objc_msgSend_retIntPtr(this, TypeSel);

        public NSEventSubtype Subtype => (NSEventSubtype)objc_msgSend_retIntPtr(this, SubtypeSel);

        public NSPoint LocationInWindow => objc_msgSend_NSPointRet(this, LocationInWindowSel);

        public NSPoint ScrollingDelta => new NSPoint(
                objc_msgSend_retNFloat(this, ScrollingDeltaXSel),
                objc_msgSend_retNFloat(this, ScrollingDeltaYSel)
            );

        public ushort KeyCode => objc_msgSend_USHortRet(this, KeyCodeSel);

        public string? Characters => CFStringRef.Marshal(objc_msgSend_retIntPtr(this, CharactersSel));

        public float Pressure => objc_msgSend_retFloat(this, PressureSel);

        [LibraryImport(ObjC.LibObjCLib, EntryPoint = "objc_msgSend")]
        private static partial NSPoint objc_msgSend_NSPointRet(nint obj, nint sel);

        [LibraryImport(ObjC.LibObjCLib, EntryPoint = "objc_msgSend")]
        private static partial ushort objc_msgSend_USHortRet(nint obj, nint sel);

        public void Dispose()
        {
            if (this.Self != 0)
            {
                CFRelease(this.Self);
            }
        }

        public static implicit operator nint(NSEventRef cfStringRef) => cfStringRef.Self;

        public enum NSEventType : long // nint
        {
            LeftMouseDown = 1,
            LeftMouseUp = 2,
            RightMouseDown = 3,
            RightMouseUp = 4,
            MouseMoved = 5,
            LeftMouseDragged = 6,
            RightMouseDragged = 7,
            MouseEntered = 8,
            MouseExited = 9,
            KeyDown = 10,
            KeyUp = 11,
            FlagsChanged = 12,
            AppKitDefined = 13,
            SystemDefined = 14,
            ApplicationDefined = 15,
            Periodic = 16,
            CursorUpdate = 17,
            ScrollWheel = 22,
            TabletPoint = 23,
            TabletProximity = 24,
            OtherMouseDown = 25,
            OtherMouseUp = 26,
            OtherMouseDragged = 27,
            Gesture = 29,
            Magnify = 30,
            Swipe = 31,
            Rotate = 18,
            BeginGesture = 19,
            EndGesture = 20,
            SmartMagnify = 32,
            QuickLook = 33,
            Pressure = 34,
            DirectTouch = 37,
            ChangeMode = 38,
        }

        public enum NSEventSubtype : short
        {
            // event subtypes for NSEventTypeAppKitDefined events
            WindowExposed = 0,
            ApplicationActivated = 1,
            ApplicationDeactivated = 2,
            WindowMoved = 4,
            ScreenChanged = 8,
            
            // event subtypes for NSEventTypeSystemDefined events
            PowerOff = 1,
            
            // event subtypes for mouse events
            MouseEvent = 0,
            TabletPoint = 1,
            TabletProximity = 2,
            Touch = 3
        }
    }
}
