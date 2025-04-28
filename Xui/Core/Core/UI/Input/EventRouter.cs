using Xui.Core.Math2D;
using Xui.Core.Abstract.Events;

namespace Xui.Core.UI.Input
{
    /// <summary>
    /// Routes platform-level input events through the view hierarchy, translating them into abstract pointer events.
    /// Handles pointer capture, enter/leave events, and event phase delivery (tunneling and bubbling).
    /// </summary>
    public class EventRouter
    {
        private readonly View _rootView;

        // Tracks capture, previous hit target, and last known position per pointer ID
        private readonly Dictionary<int, PointerTracking> _pointerTracking = new();

        /// <summary>
        /// Initializes a new instance of the <see cref="EventRouter"/> class, responsible for translating platform input events
        /// and dispatching them through the view hierarchy starting from the specified root view.
        /// </summary>
        /// <param name="rootView">The root view of the window hierarchy that will receive routed pointer events.</param>
        public EventRouter(View rootView)
        {
            _rootView = rootView;
        }

        /// <summary>
        /// Dispatches a touch event, normalizing it into abstract pointer events.
        /// </summary>
        /// <param name="touchEvent">The touch event to dispatch.</param>
        public void Dispatch(ref TouchEventRef touchEvent)
        {
            foreach (var touch in touchEvent.Touches)
            {
                var eventType = touch.Phase switch
                {
                    TouchPhase.Start => PointerEventType.Down,
                    TouchPhase.Move => PointerEventType.Move,
                    TouchPhase.End => PointerEventType.Up,
                    _ => throw new InvalidOperationException()
                };

                int pointerId = touch.Index;

                var state = new PointerState(
                    position: touch.Position,
                    contactSize: new Size(touch.Radius, touch.Radius),
                    pressure: 1.0f,
                    tangentialPressure: 0,
                    tilt: (0, 0),
                    twist: 0,
                    altitudeAngle: MathF.PI / 2,
                    azimuthAngle: 0,
                    pointerType: PointerType.Touch,
                    button: PointerButton.Left,
                    buttons: PointerButtons.Left
                );

                var coalesced = ReadOnlySpan<PointerState>.Empty;
                var predicted = ReadOnlySpan<PointerState>.Empty;

                var pointerEvent = new PointerEventRef(
                    eventType,
                    pointerId,
                    persistentDeviceId: 0,
                    isPrimary: true,
                    state,
                    coalesced,
                    predicted
                );

                DispatchPointer(ref pointerEvent);
            }
        }

        /// <summary>
        /// Dispatches a normalized pointer event into the view hierarchy.
        /// Handles capture, hit-testing, and event phase routing.
        /// </summary>
        private void DispatchPointer(ref PointerEventRef e)
        {
            View? targetView = null;

            if (_pointerTracking.TryGetValue(e.PointerId, out var tracking) && tracking.Captured != null)
            {
                targetView = tracking.Captured;
            }
            else
            {
                targetView = HitTest(_rootView, e.State.Position);
            }

            if (targetView == null)
                return;

            // Build the route from root to target
            var route = BuildRoute(targetView);

            // Tunneling phase (root ➔ target)
            for (int i = 0; i < route.Count; i++)
            {
                route[i].RaisePointerEvent(ref e, EventPhase.Tunnel);
            }

            // Bubbling phase (target ➔ root)
            for (int i = route.Count - 1; i >= 0; i--)
            {
                route[i].RaisePointerEvent(ref e, EventPhase.Bubble);
            }

            // Update last known position
            if (_pointerTracking.TryGetValue(e.PointerId, out tracking))
            {
                tracking.LastPosition = e.State.Position;
                _pointerTracking[e.PointerId] = tracking;
            }
            else
            {
                _pointerTracking[e.PointerId] = new PointerTracking
                {
                    Captured = null,
                    PreviousTarget = null,
                    LastPosition = e.State.Position
                };
            }
        }

        /// <summary>
        /// Recursively performs hit-testing starting from the given view,
        /// returning the deepest visible view under the specified position.
        /// </summary>
        /// <param name="view">The view to start hit-testing from.</param>
        /// <param name="position">The global position to test.</param>
        /// <returns>The deepest view under the point, or null if none hit.</returns>
        private View? HitTest(View view, Point position)
        {
            // Test children first (deepest views take priority)
            for (int i = view.Count - 1; i >= 0; i--)
            {
                var child = view[i];
                var result = HitTest(child, position);
                if (result != null)
                {
                    return result;
                }
            }

            // Then test this view itself
            if (view.HitTest(position))
            {
                return view;
            }

            return null;
        }

        /// <summary>
        /// Builds the route from the root to the target view.
        /// </summary>
        private List<View> BuildRoute(View target)
        {
            var route = new List<View>();

            View? current = target;
            while (current != null)
            {
                route.Insert(0, current);
                current = current.Parent;
            }

            return route;
        }

        /// <summary>
        /// Tracks information about a specific active pointer (touch, mouse, pen) for routing and event management.
        /// </summary>
        private struct PointerTracking
        {
            /// <summary>
            /// The view that captured this pointer, if any.
            /// </summary>
            public View? Captured;

            /// <summary>
            /// The last view hit by this pointer, used for enter/leave tracking.
            /// </summary>
            public View? PreviousTarget;

            /// <summary>
            /// The last known position of the pointer in global window coordinates.
            /// </summary>
            public Point LastPosition;
        }
    }
}
