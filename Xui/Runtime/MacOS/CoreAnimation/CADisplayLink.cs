using System;
using static Xui.Runtime.MacOS.Foundation;
using static Xui.Runtime.MacOS.ObjC;

namespace Xui.Runtime.MacOS;

public static partial class CoreAnimation
{
    public class CADisplayLink : NSObject
    {
        public static new readonly Class Class = new Class(Lib, "CADisplayLink");

        public static readonly Sel TimestampSel = new Sel("timestamp");

        public static readonly Sel TargetTimestampSel = new Sel("targetTimestamp");

        public static readonly Sel DisplayLinkWithTargetSelector = new Sel("displayLinkWithTarget:selector:");

        public static readonly Sel AddToRunLoopForModeSel = new Sel("addToRunLoop:forMode:");

        public static readonly Sel RemoveFromRunLoopForModeSel = new Sel("removeFromRunLoop:forMode:");

        public CADisplayLink(nint id) : base(id)
        {
        }

        public TimeSpan Timestamp => TimeSpan.FromSeconds(objc_msgSend_retDouble(this, TimestampSel));

        public TimeSpan TargetTimestamp => TimeSpan.FromSeconds(objc_msgSend_retDouble(this, TargetTimestampSel));

        public void AddToRunLoopForMode(NSRunLoop runloop, NSRunLoop.Mode mode) => ObjC.objc_msgSend(this, AddToRunLoopForModeSel, runloop, mode);

        public void RemoveToRunLoopForMode(NSRunLoop runloop, NSRunLoop.Mode mode) => objc_msgSend(this, RemoveFromRunLoopForModeSel, runloop, mode);

        public static CADisplayLink DisplayLink(nint target, nint selector) =>
            new CADisplayLink(ObjC.objc_msgSend_retIntPtr(Class, DisplayLinkWithTargetSelector, target, selector));
    }
}
