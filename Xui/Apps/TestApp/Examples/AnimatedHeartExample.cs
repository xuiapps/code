using System;
using System.Diagnostics;
using Xui.Core.UI;
using Xui.Core.Canvas;
using Xui.Core.Math2D;
using System.Runtime.InteropServices;

namespace Xui.Apps.TestApp.Examples
{
    public class AnimatedHeartExample : Example
    {
        public AnimatedHeartExample()
        {
            this.Title = "Animated Heart";

            this.Content = new VerticalStack
            {
                Content =
                [
                    new AnimatedHeartView
                    {
                        // Optional: tweak these live while running
                        BeatsPerMinute = 72f,
                        PulseAmplitude = 0.13f,
                        StrokeWidth = 2.5f
                    }
                ]
            };
        }

        public class AnimatedHeartView : View
        {
            private readonly Stopwatch _clock = Stopwatch.StartNew();

            /// <summary>Heart rate in beats per minute.</summary>
            public NFloat BeatsPerMinute { get; set; } = 70f;

            /// <summary>How much the scale expands on a beat (e.g., 0.12 = ±12%).</summary>
            public NFloat PulseAmplitude { get; set; } = 0.12f;

            /// <summary>Outline stroke width in device units.</summary>
            public NFloat StrokeWidth { get; set; } = 2.5f;

            protected override void AnimateCore(TimeSpan previousTime, TimeSpan currentTime)
            {
                base.AnimateCore(previousTime, currentTime);
                this.InvalidateRender();
                this.RequestAnimationFrame();
            }

            protected override void RenderCore(IContext context)
            {
                // --- Animation timing
                NFloat t = (NFloat)_clock.Elapsed.TotalSeconds;
                NFloat beat = Heartbeat02(t, BeatsPerMinute); // 0..1 envelope with a double pulse
                NFloat s = 1.0f + PulseAmplitude * beat;

                // --- Canvas layout (fit heart into the current context size)
                var size = this.Frame.Size;
                NFloat w = size.Width > 0 ? (NFloat)size.Width : 200f;
                NFloat h = size.Height > 0 ? (NFloat)size.Height : 200f;
                NFloat unit = NFloat.Min(w, h) * 0.9f;
                NFloat cx = w * 0.5f;
                NFloat cy = h * 0.5f;

                context.Save();

                // --- Style
                context.SetFill(new LinearGradient(
                    start: (cx - unit * 0.4f, cy - unit * 0.5f),
                    end: (cx + unit * 0.4f, cy + unit * 0.5f),
                    gradient: [
                        (0.00f, 0xD642CDFF),   // magenta
                        (1.00f, 0x8A05FFFF),   // purple
                    ]));
                context.SetStroke(Colors.White);
                context.LineWidth = StrokeWidth;

                // --- Draw
                context.Translate((cx, cy));
                context.Scale((s, s));
                DrawHeartPath(context, unit * 0.9f);
                context.Fill(FillRule.NonZero);
                context.Stroke();
                context.Restore();
            }

            /// <summary>
            /// Normalized heart path centered at (0,0).
            /// Scales with 'size' so the heart height ~ size.
            /// </summary>
            private static void DrawHeartPath(IContext ctx, NFloat size)
            {
                // Symmetric heart centered at (0,0), height ≈ size.
                NFloat s = size * 0.5f;

                var pTop = (0f, -0.25f * s);
                var pBottom = (0f, 0.95f * s);

                // Right half control points
                var c1 = (0.50f * s, -0.95f * s);
                var c2 = (1.20f * s, -0.05f * s);

                // Left half control points (mirror of right, note order swap when going back up)
                var c3 = (-1.20f * s, -0.05f * s);
                var c4 = (-0.50f * s, -0.95f * s);

                ctx.BeginPath();
                ctx.MoveTo(pTop);
                ctx.CurveTo(c1, c2, pBottom); // right side down
                ctx.CurveTo(c3, c4, pTop);    // left side up (mirrored)
                ctx.ClosePath();
            }

            /// <summary>
            /// A 0..1 "double-beat" envelope per cycle: quick hit, quick echo, then rest.
            /// </summary>
            private static NFloat Heartbeat02(NFloat timeSec, NFloat bpm)
            {
                NFloat spb = 60f / NFloat.Max(1f, bpm);       // seconds per beat
                NFloat phase = timeSec % spb;                // [0..spb)
                NFloat x = phase / spb;                      // [0..1)

                // Two pulses in one cycle using smoothstep windows:
                NFloat primary   = SmoothPulse(x, 0.00f, 0.10f, 0.18f, 0.28f);
                NFloat secondary = SmoothPulse(x, 0.32f, 0.42f, 0.50f, 0.60f) * 0.75f;

                // Slight ease-out to make relaxation visible
                NFloat env = NFloat.Min(1f, primary + secondary);
                return EaseOutCubic(env);
            }

            /// <summary>Hermite smoothstep: 0..1..0 between edges (a,b) rising and (c,d) falling.</summary>
            private static NFloat SmoothPulse(NFloat x, NFloat a, NFloat b, NFloat c, NFloat d)
            {
                NFloat rise = Smoothstep(a, b, x);
                NFloat fall = 1f - Smoothstep(c, d, x);
                return NFloat.Max(0f, NFloat.Min(rise, fall));
            }

            private static NFloat Smoothstep(NFloat edge0, NFloat edge1, NFloat x)
            {
                if (edge0 == edge1) return x < edge0 ? 0f : 1f;
                NFloat t = NFloat.Clamp((x - edge0) / (edge1 - edge0), 0f, 1f);
                return t * t * (3f - 2f * t);
            }

            private static NFloat EaseOutCubic(NFloat t)
            {
                NFloat u = 1f - NFloat.Clamp(t, 0f, 1f);
                return 1f - u * u * u;
            }
        }
    }
}
