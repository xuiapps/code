using Xui.Core.Math2D;

namespace Xui.Core.UI.Input
{
    /// <summary>
    /// Describes the physical state of a pointer at a specific moment in time, including position, pressure, tilt, and button information.
    /// </summary>
    public readonly struct PointerState
    {
        /// <summary>
        /// Gets the position of the pointer in global (window) coordinates.
        /// </summary>
        public readonly Point Position;

        /// <summary>
        /// Gets the size of the pointer's contact geometry, such as the area covered by a finger or stylus tip.
        /// </summary>
        public readonly Size ContactSize;

        /// <summary>
        /// Gets the normalized pressure applied by the pointer (range: 0.0 to 1.0).
        /// </summary>
        public readonly nfloat Pressure;

        /// <summary>
        /// Gets the normalized tangential (barrel) pressure applied by the pointer (range: -1.0 to 1.0).
        /// </summary>
        public readonly nfloat TangentialPressure;

        /// <summary>
        /// Gets the tilt of the pointer relative to the X and Y axes (in degrees).
        /// </summary>
        public readonly Vector Tilt;

        /// <summary>
        /// Gets the clockwise twist (rotation) of the pointer around its major axis, in degrees (0.0 to 359.9).
        /// </summary>
        public readonly nfloat Twist;

        /// <summary>
        /// Gets the altitude angle of the pointer relative to the surface (0 = horizontal, π/2 = vertical), in radians.
        /// </summary>
        public readonly nfloat AltitudeAngle;

        /// <summary>
        /// Gets the azimuth angle (compass direction) of the pointer around the vertical axis, in radians.
        /// </summary>
        public readonly nfloat AzimuthAngle;

        /// <summary>
        /// Gets the type of device generating the pointer input (mouse, touch, pen, etc.).
        /// </summary>
        public readonly PointerType PointerType;

        /// <summary>
        /// Gets which button was responsible for triggering this pointer state change (left, right, middle, eraser, etc.).
        /// </summary>
        public readonly PointerButton Button;

        /// <summary>
        /// Gets the set of all currently pressed buttons on the device.
        /// </summary>
        public readonly PointerButtons Buttons;

        /// <summary>
        /// Initializes a new instance of the <see cref="PointerState"/> struct.
        /// </summary>
        /// <param name="position">The global position of the pointer.</param>
        /// <param name="contactSize">The size of the contact area.</param>
        /// <param name="pressure">The normalized pressure applied.</param>
        /// <param name="tangentialPressure">The normalized tangential (barrel) pressure applied.</param>
        /// <param name="tilt">The tilt vector of the pointer.</param>
        /// <param name="twist">The clockwise twist rotation of the pointer.</param>
        /// <param name="altitudeAngle">The altitude angle relative to the surface.</param>
        /// <param name="azimuthAngle">The azimuth angle (compass direction).</param>
        /// <param name="pointerType">The type of input device.</param>
        /// <param name="button">The button that triggered the event.</param>
        /// <param name="buttons">The set of currently pressed buttons.</param>
        public PointerState(
            Point position,
            Size contactSize,
            nfloat pressure,
            nfloat tangentialPressure,
            Vector tilt,
            nfloat twist,
            nfloat altitudeAngle,
            nfloat azimuthAngle,
            PointerType pointerType,
            PointerButton button,
            PointerButtons buttons)
        {
            Position = position;
            ContactSize = contactSize;
            Pressure = pressure;
            TangentialPressure = tangentialPressure;
            Tilt = tilt;
            Twist = twist;
            AltitudeAngle = altitudeAngle;
            AzimuthAngle = azimuthAngle;
            PointerType = pointerType;
            Button = button;
            Buttons = buttons;
        }
    }
}
