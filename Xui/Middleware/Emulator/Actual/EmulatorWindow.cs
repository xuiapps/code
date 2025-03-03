using System;
using System.Runtime.InteropServices;
using Xui.Core.Abstract;
using Xui.Core.Abstract.Events;
using Xui.Core.Canvas;
using Xui.Core.Math2D;

namespace Xui.Emulator.Actual;

public class EmulatorWindow : Xui.Core.Abstract.IWindow, Xui.Core.Actual.IWindow, Xui.Core.Abstract.IWindow.IDesktopStyle
{
    private Point? leftMouseButtonTouch = null;

    public EmulatorWindow()
    {
    }

    public Xui.Core.Abstract.IWindow? Abstract { get; set; }
    public Xui.Core.Actual.IWindow? Platform { get; set; }

    bool Xui.Core.Abstract.IWindow.IDesktopStyle.Chromeless => true;

    Size? Xui.Core.Abstract.IWindow.IDesktopStyle.StartupSize => new (330, 740);

#region Platform.IWindow
    string Xui.Core.Actual.IWindow.Title { get => Platform!.Title; set => Platform!.Title = value; }

    void Xui.Core.Actual.IWindow.Invalidate() => Platform!.Invalidate();

    void Xui.Core.Actual.IWindow.Show() => Platform!.Show();

    public Rect DisplayArea { get; set; }

    public Rect SafeArea { get; set; }
#endregion

#region Abstract.IWindow
    public bool RequireKeyboard { get; set; }

    void Xui.Core.Abstract.IWindow.Closed() => Abstract!.Closed();

    bool Xui.Core.Abstract.IWindow.Closing() => Abstract!.Closing();

    void Xui.Core.Abstract.IWindow.OnAnimationFrame(ref FrameEventRef animationFrame)
    {
        Abstract!.OnAnimationFrame(ref animationFrame);
    }

    void Xui.Core.Abstract.IWindow.OnMouseDown(ref MouseDownEventRef evRef)
    {
        MouseDownEventRef evMobile = new MouseDownEventRef()
        {
            Position = evRef.Position + (-8, -52 - 8 - 8),
            Button = evRef.Button
        };
        this.Abstract!.OnMouseDown(ref evMobile);

        // Synthetic touch
        this.leftMouseButtonTouch = evMobile.Position;
        var touchEventRef = new TouchEventRef([new()
        {
            Index = 0,
            Phase = TouchPhase.Start,
            Position = evMobile.Position,
            Radius = 0.5f
        }]);
        this.Abstract!.OnTouch(ref touchEventRef);
    }

    void Xui.Core.Abstract.IWindow.OnMouseMove(ref MouseMoveEventRef evRef)
    {
        MouseMoveEventRef evMobile = new MouseMoveEventRef()
        {
            Position = evRef.Position + (-8, -52 - 8 - 8)
        };
        Abstract!.OnMouseMove(ref evMobile);

        // Synthetic touch
        if (this.leftMouseButtonTouch.HasValue)
        {
            this.leftMouseButtonTouch = evMobile.Position;
            var touchEventRef = new TouchEventRef([new()
            {
                Index = 0,
                Phase = TouchPhase.Move,
                Position = evMobile.Position,
                Radius = 0.5f
            }]);
            this.Abstract!.OnTouch(ref touchEventRef);
        }
    }

    void Xui.Core.Abstract.IWindow.OnMouseUp(ref MouseUpEventRef evRef)
    {
        MouseUpEventRef evMobile = new MouseUpEventRef()
        {
            Position = evRef.Position + (-8, -52 - 8 - 8),
            Button = evRef.Button
        };
        Abstract!.OnMouseUp(ref evMobile);

        // Synthetic touch
        if (this.leftMouseButtonTouch.HasValue)
        {
            this.leftMouseButtonTouch = null;
            var touchEventRef = new TouchEventRef([new()
            {
                Index = 0,
                Phase = TouchPhase.End,
                Position = evMobile.Position,
                Radius = 0.5f
            }]);
            this.Abstract!.OnTouch(ref touchEventRef);
        }
    }

    void Xui.Core.Abstract.IWindow.OnScrollWheel(ref ScrollWheelEventRef evRef)
    {
        Abstract!.OnScrollWheel(ref evRef);
    }

    void Xui.Core.Abstract.IWindow.OnTouch(ref TouchEventRef touchEventRef)
    {
        Abstract!.OnTouch(ref touchEventRef);
    }

    void Xui.Core.Abstract.IWindow.Render(ref RenderEventRef render)
    {
        NFloat titleHeight = 52f;
        NFloat gap = 8f;

        NFloat borderWidth = 8f;
        NFloat borderOutline = 2.5f; // Included in borderWidth
        NFloat screenCornerRadius = 45f;

        Rect titleRect = new (0, 0, render.Rect.Width, titleHeight);
        Rect emulatorRect = new (
            x: borderWidth,
            y: titleHeight + gap + borderWidth,
            width: render.Rect.Width - borderWidth - borderWidth,
            height: render.Rect.Height - titleHeight - gap - borderWidth - borderWidth);

        using (var ctx = Xui.Core.Actual.Runtime.DrawingContext!)
        {
            ctx.Save();

            ctx.BeginPath();
            ctx.RoundRect(emulatorRect, screenCornerRadius);
            ctx.Clip();

            ctx.BeginPath();

            ctx.Translate((borderWidth, titleHeight + gap + borderWidth));

            RenderEventRef emulatorRender = new RenderEventRef(
                rect: new Rect(0, 0, emulatorRect.Width, emulatorRect.Height),
                frame: render.Frame
            );

            ctx.SetFill(Colors.White);
            ctx.FillRect(emulatorRender.Rect);

            Abstract!.DisplayArea = emulatorRender.Rect;
            Abstract!.SafeArea = emulatorRender.Rect - new Frame(0, 40, 0, 20);

            // TODO: DrawingContext is disposable,
            // each call is supposed to capture and dispose,
            // but it doesn't understand the callstack,
            // so Render will dispose it with the ctx above... 
            Abstract!.Render(ref emulatorRender);
        }

        using (var ctx = Xui.Core.Actual.Runtime.DrawingContext!)
        {
            ctx.Restore();

            if (this.leftMouseButtonTouch.HasValue)
            {
                ctx.Save();
                ctx.BeginPath();
                ctx.RoundRect(emulatorRect, screenCornerRadius);
                ctx.Clip();

                ctx.Translate((borderWidth, titleHeight + gap + borderWidth));

                ctx.BeginPath();
                ctx.Ellipse(this.leftMouseButtonTouch.Value, 15f, 15f, 0, 0, NFloat.Pi * 2, Winding.ClockWise);
                ctx.SetFill(0x66888888);
                ctx.Fill();

                ctx.Ellipse(this.leftMouseButtonTouch.Value, 15f, 15f, 0, 0, NFloat.Pi * 2, Winding.ClockWise);
                ctx.LineWidth = 3f;
                ctx.SetStroke(0x88AAAAAA);
                ctx.Stroke();

                ctx.Restore();
            }

            // Pinch-hole:
            ctx.BeginPath();
            ctx.RoundRect(
                rect: new Rect(
                    x: render.Rect.Width / 2f - 45f,
                    y: titleHeight + gap + borderWidth + 12f,
                    width: 90f,
                    height: 26f),
                radius: 13f
            );
            ctx.SetFill(0x111111FF);
            ctx.Fill();

            ctx.BeginPath();
            ctx.RoundRect(
                rect: emulatorRect.Expand(borderWidth * 0.5f),
                radius: screenCornerRadius + borderWidth * 0.5f);
            ctx.SetStroke(new Color(0x111111FF));
            ctx.LineWidth = borderWidth;
            ctx.Stroke();

            ctx.BeginPath();
            ctx.RoundRect(
                rect: emulatorRect.Expand(borderWidth - borderOutline * 0.5f),
                radius: screenCornerRadius + borderWidth * 0.5f + 0.25f * borderOutline);
            ctx.SetStroke(new Color(0x444444FF));
            ctx.LineWidth = borderOutline;
            ctx.Stroke();

            // Clock
            ctx.SetFill(Colors.Black);
            ctx.TextAlign = TextAlign.Center;
            ctx.SetFont(new Font()
            {
                FontFamily = ["Verdana"],
                FontSize = 12,
                FontWeight = 600,
                FontStyle = FontStyle.Normal,
                LineHeight = 16
            });
            ctx.FillText(DateTime.Now.ToString("H:mm"), ((render.Rect.Width / 2f - 22f) / 2, titleHeight + gap + borderWidth + 18));

            // Signal Strength
            ctx.Save();
            ctx.Translate((232, titleHeight + gap + borderWidth + 18 + 13.5f));

            ctx.BeginPath();
            ctx.SetStroke(Colors.Black);
            ctx.Arc((0, 0), 2.5f, NFloat.Pi * 1.75f, + NFloat.Pi * 1.25f, Winding.ClockWise);
            ctx.LineWidth = 4f;
            ctx.Stroke();

            ctx.BeginPath();
            ctx.SetStroke(Colors.Black);
            ctx.Arc((0, 0), 7.5f, NFloat.Pi * 1.75f, + NFloat.Pi * 1.25f, Winding.ClockWise);
            ctx.LineWidth = 2f;
            ctx.Stroke();

            ctx.BeginPath();
            ctx.SetStroke(0x00000055);
            ctx.Arc((0, 0), 11.5f, NFloat.Pi * 1.75f, + NFloat.Pi * 1.25f, Winding.ClockWise);
            ctx.LineWidth = 2f;
            ctx.Stroke();
            ctx.Restore();

            // 5G
            ctx.SetFill(Colors.Black);
            ctx.TextAlign = TextAlign.Center;
            ctx.SetFont(new Font()
            {
                FontFamily = ["Verdana"],
                FontSize = 12,
                FontWeight = 600,
                FontStyle = FontStyle.Normal,
                LineHeight = 16
            });
            ctx.FillText("5G", ((render.Rect.Width - 73f), titleHeight + gap + borderWidth + 18));

            // Battery
            ctx.Save();
            ctx.Translate((272, titleHeight + gap + borderWidth + 18 + 2.5f));

            ctx.BeginPath();
            ctx.RoundRect(new Rect(0, 0, 20, 10), 3f);
            ctx.LineWidth = 1f;
            ctx.SetStroke(Colors.Black);
            ctx.Stroke();

            ctx.BeginPath();
            ctx.RoundRect(new Rect(1.5f, 1.5f, 12, 7), 2f);
            ctx.LineWidth = 1f;
            ctx.SetFill(Colors.Black);
            ctx.Fill();
            ctx.Restore();

            // Menu Handle
            ctx.BeginPath();
            ctx.RoundRect(
                rect: new Rect(
                    x: render.Rect.Width / 2f - 55f,
                    y: render.Rect.Height - borderWidth - 3f - 5f,
                    width: 110f,
                    height: 4f),
                radius: 1.5f
            );
            ctx.SetFill(0x111111FF);
            ctx.Fill();

            // Window title
            ctx.BeginPath();
            ctx.RoundRect(titleRect, 10f);
            ctx.SetFill(new Color(0x333333FF));
            ctx.Fill();
        }
    }

    void Xui.Core.Abstract.IWindow.WindowHitTest(ref WindowHitTestEventRef evRef)
    {
        NFloat titleHeight = 52f;
        NFloat gap = 8f;

        NFloat borderWidth = 8f;

        NFloat screenCornerRadius = 45f;

        var point = evRef.Point;

        if (point.Y < titleHeight)
        {
            evRef.Area = WindowHitTestEventRef.WindowArea.Title;
        }

        Rect emulatorRect = new Rect(evRef.Window.Left, evRef.Window.Top + titleHeight + gap, evRef.Window.Width, evRef.Window.Height - titleHeight - gap);
        if (emulatorRect.Contains(point))
        {
            var topLeftCenter = emulatorRect.TopLeft + (screenCornerRadius, screenCornerRadius);
            var topRightCenter = emulatorRect.TopRight + (-screenCornerRadius, screenCornerRadius);
            var bottomLeftCenter = emulatorRect.BottomLeft + (screenCornerRadius, -screenCornerRadius);
            var bottomRightCenter = emulatorRect.BottomRight + (-screenCornerRadius, -screenCornerRadius);

            NFloat signedBorderDistance;
            if (point.X < topLeftCenter.X && point.Y < topLeftCenter.Y)
            {
                signedBorderDistance = (point - topLeftCenter).Magnitude - screenCornerRadius;
            }
            else if (point.X > topRightCenter.X && point.Y < topRightCenter.Y)
            {
                signedBorderDistance = (point - topRightCenter).Magnitude - screenCornerRadius;
            }
            else if (point.X < bottomLeftCenter.X && point.Y > bottomLeftCenter.Y)
            {
                signedBorderDistance = (point - bottomLeftCenter).Magnitude - screenCornerRadius;
            }
            else if (point.X > bottomRightCenter.X && point.Y > bottomRightCenter.Y)
            {
                signedBorderDistance = (point - bottomRightCenter).Magnitude - screenCornerRadius;
            }
            else
            {
                signedBorderDistance = NFloat.Max(
                    NFloat.Max(emulatorRect.Left - point.X, point.X - emulatorRect.Right),
                    NFloat.Max(emulatorRect.Top - point.Y, point.Y - emulatorRect.Bottom)
                );
            }

            if (-borderWidth <= signedBorderDistance && signedBorderDistance <= 0)
            {
                NFloat resizeRect = 48f;

                // On border!
                if (point.X <= emulatorRect.Left + resizeRect && point.Y <= emulatorRect.Top + resizeRect)
                {
                    evRef.Area = WindowHitTestEventRef.WindowArea.BorderTopLeft;
                }
                else if (point.X >= emulatorRect.Right - resizeRect && point.Y <= emulatorRect.Top + resizeRect)
                {
                    evRef.Area = WindowHitTestEventRef.WindowArea.BorderTopRight;
                }
                else if (point.X >= emulatorRect.Right - resizeRect && point.Y >= emulatorRect.Bottom - resizeRect)
                {
                    evRef.Area = WindowHitTestEventRef.WindowArea.BorderBottomRight;
                }
                else if (point.X <= emulatorRect.Left + resizeRect && point.Y >= emulatorRect.Bottom - resizeRect)
                {
                    evRef.Area = WindowHitTestEventRef.WindowArea.BorderBottomLeft;
                }
                else if (point.Y <= emulatorRect.Top + resizeRect)
                {
                    evRef.Area = WindowHitTestEventRef.WindowArea.BorderTop;
                }
                else if (point.Y >= emulatorRect.Bottom - resizeRect)
                {
                    evRef.Area = WindowHitTestEventRef.WindowArea.BorderBottom;
                }
                else if (point.X <= emulatorRect.Left + resizeRect)
                {
                    evRef.Area = WindowHitTestEventRef.WindowArea.BorderLeft;
                }
                else if (point.X >= emulatorRect.Right - resizeRect)
                {
                    evRef.Area = WindowHitTestEventRef.WindowArea.BorderRight;
                }
                else
                {
                    evRef.Area = WindowHitTestEventRef.WindowArea.Client;
                }
            }
            else if (signedBorderDistance <= 0)
            {
                evRef.Area = WindowHitTestEventRef.WindowArea.Client;
            }
            else
            {
                evRef.Area = WindowHitTestEventRef.WindowArea.Transparent;
            }
        }
    }
    #endregion
}