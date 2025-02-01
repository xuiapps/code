using static Xui.Runtime.MacOS.ObjC;

namespace Xui.Runtime.MacOS;

public static partial class AppKit
{
    public class NSCursor : NSObject
    {
        public static new readonly Class Class = new Class(AppKit.Lib, "NSCursor");

        private static readonly Sel SetSel = new Sel("set");

        private static readonly Sel PushSel = new Sel("push");

        private static readonly Sel PopSel = new Sel("pop");

        // Singletons...

        public static void PopStatic() => objc_msgSend(Class, PopSel);

        private static readonly Sel ArrowCursorSel = new Sel("arrowCursor");

        public static readonly NSCursor ArrowCursor = new NSCursor(objc_msgSend_retIntPtr(Class,ArrowCursorSel));

        private static readonly Sel IBeamCursorSel = new Sel("IBeamCursor");

        public static readonly NSCursor IBeamCursor = new NSCursor(objc_msgSend_retIntPtr(Class, IBeamCursorSel));

        private static readonly Sel CrosshairCursorSel = new Sel("crosshairCursor");

        public static readonly NSCursor CrosshairCursor = new NSCursor(objc_msgSend_retIntPtr(Class, CrosshairCursorSel));

        private static readonly Sel ClosedHandCursorSel = new Sel("closedHandCursor");

        public static readonly NSCursor ClosedHandCursor = new NSCursor(objc_msgSend_retIntPtr(Class, ClosedHandCursorSel));

        private static readonly Sel OpenHandCursorSel = new Sel("openHandCursor");

        public static readonly NSCursor OpenHandCursor = new NSCursor(objc_msgSend_retIntPtr(Class, OpenHandCursorSel));

        private static readonly Sel PointingHandCursorSel = new Sel("pointingHandCursor");

        public static readonly NSCursor PointingHandCursor = new NSCursor(objc_msgSend_retIntPtr(Class, PointingHandCursorSel));

        private static readonly Sel ResizeLeftCursorSel = new Sel("resizeLeftCursor");

        public static readonly NSCursor ResizeLeftCursor = new NSCursor(objc_msgSend_retIntPtr(Class, ResizeLeftCursorSel));

        private static readonly Sel ResizeRightCursorSel = new Sel("resizeRightCursor");

        public static readonly NSCursor ResizeRightCursor = new NSCursor(objc_msgSend_retIntPtr(Class, ResizeRightCursorSel));

        private static readonly Sel ResizeLeftRightCursorSel = new Sel("resizeLeftRightCursor");

        public static readonly NSCursor ResizeLeftRightCursor = new NSCursor(objc_msgSend_retIntPtr(Class, ResizeLeftRightCursorSel));

        private static readonly Sel ResizeUpCursorSel = new Sel("resizeUpCursor");

        public static readonly NSCursor ResizeUpCursor = new NSCursor(objc_msgSend_retIntPtr(Class, ResizeUpCursorSel));

        private static readonly Sel ResizeDownCursorSel = new Sel("resizeDownCursor");

        public static readonly NSCursor ResizeDownCursor = new NSCursor(objc_msgSend_retIntPtr(Class, ResizeDownCursorSel));

        private static readonly Sel ResizeUpDownCursorSel = new Sel("resizeUpDownCursor");

        public static readonly NSCursor ResizeUpDownCursor = new NSCursor(objc_msgSend_retIntPtr(Class, ResizeUpDownCursorSel));

        private static readonly Sel DisappearingItemCursorSel = new Sel("disappearingItemCursor");

        public static readonly NSCursor DisappearingItemCursor = new NSCursor(objc_msgSend_retIntPtr(Class, DisappearingItemCursorSel));

        private static readonly Sel IBeamCursorForVerticalLayoutSel = new Sel("IBeamCursorForVerticalLayout");

        public static readonly NSCursor IBeamCursorForVerticalLayout = new NSCursor(objc_msgSend_retIntPtr(Class, IBeamCursorForVerticalLayoutSel));

        private static readonly Sel OperationNotAllowedCursorSel = new Sel("operationNotAllowedCursor");

        public static readonly NSCursor OperationNotAllowedCursor = new NSCursor(objc_msgSend_retIntPtr(Class, OperationNotAllowedCursorSel));

        private static readonly Sel DragLinkCursorSel = new Sel("dragLinkCursor");

        public static readonly NSCursor DragLinkCursor = new NSCursor(objc_msgSend_retIntPtr(Class, DragLinkCursorSel));

        private static readonly Sel DragCopyCursorSel = new Sel("dragCopyCursor");

        public static readonly NSCursor DragCopyCursor = new NSCursor(objc_msgSend_retIntPtr(Class, DragCopyCursorSel));

        private static readonly Sel ContextualMenuCursorSel = new Sel("contextualMenuCursor");

        public static readonly NSCursor ContextualMenuCursor = new NSCursor(objc_msgSend_retIntPtr(Class, ContextualMenuCursorSel));

        // Private cursors...
        // https://stackoverflow.com/questions/10733228/native-osx-lion-resize-cursor-for-custom-nswindow-or-nsview

        private static readonly Sel _WindowResizeEastCursorSel = new Sel("_windowResizeEastCursor");

        public static readonly NSCursor _WindowResizeEastCursor = new NSCursor(objc_msgSend_retIntPtr(Class, _WindowResizeEastCursorSel));

        private static readonly Sel _WindowResizeWestCursorSel = new Sel("_windowResizeWestCursor");

        public static readonly NSCursor _WindowResizeWestCursor = new NSCursor(objc_msgSend_retIntPtr(Class, _WindowResizeWestCursorSel));
    
        private static readonly Sel _WindowResizeEastWestCursorSel = new Sel("_windowResizeEastWestCursor");

        public static readonly NSCursor _WindowResizeEastWestCursor = new NSCursor(objc_msgSend_retIntPtr(Class, _WindowResizeEastWestCursorSel));

        private static readonly Sel _WindowResizeNorthCursorSel = new Sel("_windowResizeNorthCursor");

        public static readonly NSCursor _WindowResizeNorthCursor = new NSCursor(objc_msgSend_retIntPtr(Class, _WindowResizeNorthCursorSel));

        private static readonly Sel _WindowResizeSouthCursorSel = new Sel("_windowResizeSouthCursor");

        public static readonly NSCursor _WindowResizeSouthCursor = new NSCursor(objc_msgSend_retIntPtr(Class, _WindowResizeSouthCursorSel));

        private static readonly Sel _WindowResizeNorthSouthCursorSel = new Sel("_windowResizeNorthSouthCursor");

        public static readonly NSCursor _WindowResizeNorthSouthCursor = new NSCursor(objc_msgSend_retIntPtr(Class, _WindowResizeNorthSouthCursorSel));

        private static readonly Sel _WindowResizeNorthEastCursorSel = new Sel("_windowResizeNorthEastCursor");

        public static readonly NSCursor _WindowResizeNorthEastCursor = new NSCursor(objc_msgSend_retIntPtr(Class, _WindowResizeNorthEastCursorSel));

        private static readonly Sel _WindowResizeNorthWestCursorSel = new Sel("_windowResizeNorthWestCursor");

        public static readonly NSCursor _WindowResizeNorthWestCursor = new NSCursor(objc_msgSend_retIntPtr(Class, _WindowResizeNorthWestCursorSel));

        private static readonly Sel _WindowResizeSouthEastCursorSel = new Sel("_windowResizeSouthEastCursor");

        public static readonly NSCursor _WindowResizeSouthEastCursor = new NSCursor(objc_msgSend_retIntPtr(Class, _WindowResizeSouthEastCursorSel));

        private static readonly Sel _WindowResizeSouthWestCursorSel = new Sel("_windowResizeSouthWestCursor");

        public static readonly NSCursor _WindowResizeSouthWestCursor = new NSCursor(objc_msgSend_retIntPtr(Class, _WindowResizeSouthWestCursorSel));

        private static readonly Sel _WindowResizeNorthEastSouthWestCursorSel = new Sel("_windowResizeNorthEastSouthWestCursor");

        public static readonly NSCursor _WindowResizeNorthEastSouthWestCursor = new NSCursor(objc_msgSend_retIntPtr(Class, _WindowResizeNorthEastSouthWestCursorSel));

        private static readonly Sel _WindowResizeNorthWestSouthEastCursorSel = new Sel("_windowResizeNorthWestSouthEastCursor");

        public static readonly NSCursor _WindowResizeNorthWestSouthEastCursor = new NSCursor(objc_msgSend_retIntPtr(Class, _WindowResizeNorthWestSouthEastCursorSel));

        public NSCursor(nint id) : base(id)
        {
        }

        public void Set() => objc_msgSend(this, SetSel);

        public void Push() => objc_msgSend(this, PushSel);

        public void Pop() => objc_msgSend(this, PopSel);

        public override NSCursor Autorelease() => (NSCursor)base.Autorelease();
    }
}