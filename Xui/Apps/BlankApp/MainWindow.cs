using System.Diagnostics;
using System.Runtime.InteropServices;
using Xui.Core.Abstract;
using Xui.Core.Abstract.Events;
using Xui.Core.Canvas;
using static Xui.Core.Canvas.Colors;
using Xui.Core.Math2D;
using Xui.Core.Actual;

using Window = Xui.Core.Abstract.Window;
using Point = Xui.Core.Math2D.Point;
using Rect = Xui.Core.Math2D.Rect;
using Color = Xui.Core.Canvas.Color;
using Colors = Xui.Core.Canvas.Colors;
using Font = Xui.Core.Canvas.Font;
using CornerRadius = Xui.Core.Canvas.CornerRadius;
using Xui.Core.UI;
using Xui.Core.Canvas.SVG;

namespace Xui.Apps.BlankApp;

public class MainWindow : Window
{
    private Point mousePoint;
    private Point scrollPoint;

    public override void OnMouseMove(ref MouseMoveEventRef e)
    {
        this.mousePoint = e.Position;
        this.Invalidate();
        base.OnMouseMove(ref e);
    }

    public override void OnScrollWheel(ref ScrollWheelEventRef e)
    {
        this.scrollPoint += e.Delta;
        Console.WriteLine($"ScrollPoint = {this.scrollPoint} (added scroll wheel delta {e.Delta})");

        this.Invalidate();
        base.OnScrollWheel(ref e);
    }

    private Xui.Core.Math2D.Point touchPoint;
    private Vector scrollInertia = Vector.Zero;
    private bool runScrollInertia = false;

    private nint fps = 0;

    private nint maxFps = 0;

    private nint didIStutter = 0;

    public override void OnTouch(ref TouchEventRef e)
    {
        foreach(var touch in e.Touches)
        {
            // TODO: Browser indices are not 0 based...
            // if (touch.Index == 0)
            // {
                if (touch.Phase == TouchPhase.Start)
                {
                    Console.WriteLine($"Touch Start: {touch.Position.X} {touch.Position.Y}");

                    this.runScrollInertia = false;
                    this.scrollInertia = Vector.Zero;
                    this.touchPoint = touch.Position;
                    this.Invalidate();

                    this.mousePoint = this.touchPoint;
                }
                else if (touch.Phase == TouchPhase.Move)
                {
                    Console.WriteLine($"Touch Move: {touch.Position.X} {touch.Position.Y}");

                    this.runScrollInertia = false;

                    var delta = touch.Position - this.touchPoint;
                    this.touchPoint = touch.Position;

                    this.scrollInertia = delta;

                    this.scrollPoint += delta;
                    Console.WriteLine($"ScrollPoint = {this.scrollPoint} (added delta {delta})");
                    this.Invalidate();

                    this.mousePoint = this.touchPoint;
                }
                else if (touch.Phase == TouchPhase.End)
                {
                    this.scrollInertia *= 0.6f - 0.4f * NFloat.Cos(NFloat.Clamp(this.scrollInertia.Magnitude * 0.2f, 0, NFloat.Pi));
                    Console.WriteLine($"Touch End: {touch.Position.X} {touch.Position.Y} (inertia {this.scrollInertia.X} {this.scrollInertia.Y})");

                    this.runScrollInertia = true;
                    this.Invalidate();

                    this.mousePoint = Point.Zero;
                }
            // }
        }

        base.OnTouch(ref e);
    }

    public override void OnAnimationFrame(ref FrameEventRef e)
    {
        if (this.runScrollInertia)
        {
            this.scrollPoint += this.scrollInertia;
            Console.WriteLine($"ScrollPoint = {this.scrollPoint} (added inertial {this.scrollInertia})");

            // Drag at high speed
            this.scrollInertia *= 0.985f;

            if (this.scrollInertia.Magnitude > 0.15f)
            {
                // Slow speed-down force
                this.scrollInertia -= this.scrollInertia.Normalized * 0.15f;
                this.runScrollInertia = true;
            }
            else
            {
                this.scrollInertia = Vector.Zero;
                this.runScrollInertia = false;
            }

            this.Invalidate();
        }

        var frameFPS = (nint)Math.Round(1f / e.Delta.TotalSeconds);

        if (frameFPS != this.fps)
        {
            this.fps = frameFPS;
            this.maxFps = this.fps;
            this.Invalidate();
        }

        if (frameFPS != this.maxFps)
        {
            this.didIStutter++;
            this.Invalidate();
        }

        base.OnAnimationFrame(ref e);
    }

    public override void Render(ref RenderEventRef render)
    {
        using var ctx = Xui.Core.Actual.Runtime.DrawingContext!;

        var centerGuide = render.Rect.Width / 2f;

        NFloat Normalize(NFloat value, NFloat min, NFloat max)
        {
            return max <= min ? 0 : NFloat.Clamp((value - min) / (max - min), 0, 1);
        }
        NFloat EaseInOutSine(NFloat t)
        {
            return -(NFloat.Cos(NFloat.Pi * t) - 1f) / 2f;
        }
        NFloat EaseInOutQuad(NFloat t) {
            return t < 0.5 ? 2 * t * t : 1 - NFloat.Pow(-2 * t + 2, 2) / 2;
        }

        // ctx.SetFill(Colors.White);
        ctx.SetFill(new LinearGradient(
            start: (0, 0 + this.scrollPoint.Y * 1.75f),
            end: (0, 1500 + this.scrollPoint.Y * 1.75f),
            gradient: [
                new (0.45f, 0xFFFFFFFF),
                new (0.60f, 0xFFFFDDFF),
                new (0.75f, 0xFFFFEEFF),
                new (0.90f, 0xFFFFFFFF),
            ]
        ));
        ctx.BeginPath();
        ctx.Rect(render.Rect);
        ctx.Fill();
        // ctx.SetFill(0xFFFFDDFF);
        //ctx.FillRect(render.Rect);
        
        // Content
        ctx.SetFill(0x000000FF);
        ctx.TextAlign = TextAlign.Center;
        ctx.SetFont(new Font() {
            FontFamily = ["Inter"],
            FontSize = 18,
            FontStyle = FontStyle.Normal,
            FontWeight = 700,
            LineHeight = 18
        });
        ctx.FillText("Delightful App Development", (centerGuide, 320 + this.scrollPoint.Y));
        ctx.SetFont(new Font() {
            FontFamily = ["Inter"],
            FontSize = 14,
            FontStyle = FontStyle.Normal,
            FontWeight = 400,
            LineHeight = 14
        });
        ctx.FillText("A dotnet UI application framework.", (centerGuide, 348 + this.scrollPoint.Y));

        // Content
        var left = render.Rect.Left;
        var right = render.Rect.Right;

        ctx.SetFont(new Font() {
            FontFamily = ["Inter"],
            FontSize = 18,
            FontStyle = FontStyle.Normal,
            FontWeight = 700,
            LineHeight = 18
        });
        ctx.TextAlign = TextAlign.Left;
        ctx.SetFill(Colors.Black);
        ctx.FillText("Technology", (left + 32, 495 + 12 + this.scrollPoint.Y));

        var runtimesFrame = new Rect(left + 16, 540 + this.scrollPoint.Y, right - left - 16 - 16, 100);
        ctx.BeginPath();
        ctx.RoundRect(runtimesFrame, 15f);
        ctx.SetFill(0xE8BEEDFF);
        ctx.Fill();
        ctx.SetFont(new Font() {
            FontFamily = ["Inter"],
            FontSize = 14,
            FontStyle = FontStyle.Normal,
            FontWeight = 400,
            LineHeight = 14
        });
        ctx.TextAlign = TextAlign.Left;
        ctx.SetFill(Colors.Black);
        ctx.FillText("Runtimes", runtimesFrame.TopLeft + (16, 12));

        var canvasFrame = new Rect(left + 16, 660 + this.scrollPoint.Y, right - left - 16 - 16, 200);
        ctx.BeginPath();
        ctx.RoundRect(canvasFrame, 15f);
        ctx.SetFill(0xE8BEEDFF);
        ctx.Fill();
        ctx.SetFont(new Font() {
            FontFamily = ["Inter"],
            FontSize = 14,
            FontStyle = FontStyle.Normal,
            FontWeight = 400,
            LineHeight = 14
        });
        ctx.TextAlign = TextAlign.Left;
        ctx.SetFill(Colors.Black);
        ctx.FillText("Canvas", canvasFrame.TopLeft + (16, 12));

        var widgetsFrame = new Rect(left + 16, 880 + this.scrollPoint.Y, right - left - 16 - 16, 100);
        ctx.BeginPath();
        ctx.RoundRect(widgetsFrame, 15f);
        ctx.SetFill(0xE8BEEDFF);
        ctx.Fill();
        ctx.SetFont(new Font() {
            FontFamily = ["Inter"],
            FontSize = 14,
            FontStyle = FontStyle.Normal,
            FontWeight = 400,
            LineHeight = 14
        });
        ctx.TextAlign = TextAlign.Left;
        ctx.SetFill(Colors.Black);
        ctx.FillText("Widgets", widgetsFrame.TopLeft + (16, 12));

        ctx.SetFont(new Font() {
            FontFamily = ["Inter"],
            FontSize = 18,
            FontStyle = FontStyle.Normal,
            FontWeight = 700,
            LineHeight = 18
        });
        ctx.TextAlign = TextAlign.Left;
        ctx.SetFill(Colors.Black);
        ctx.FillText("Applications", (left + 32, 1015 + 12 + this.scrollPoint.Y));

        // Header Overlay
        var headerHeight = 30 + 60; // Safe area + header height...
        var headerAppearanceProgress = Normalize(-this.scrollPoint.Y, 100, 150);
        var headerAppearanceEasedProgress = EaseInOutQuad(headerAppearanceProgress);
        ctx.Rect((0, 0, render.Rect.Width, headerHeight));
        ctx.SetFill(new Color(1, 0xEE/255f, 0xEE/255f, headerAppearanceEasedProgress));
        ctx.Fill();
        ctx.SetStroke(new Color(0, 0, 0, headerAppearanceEasedProgress));
        ctx.BeginPath();
        ctx.MoveTo((0, headerHeight));
        ctx.LineTo((render.Rect.Width, headerHeight));
        ctx.LineWidth = 0.5f;
        ctx.Stroke();

        // Hamburger
        ctx.Save();
        var hamburgerInContent = new Point(render.Rect.Width * 0.5f + 64f, 420 + this.scrollPoint.Y);
        var hamburgerInTitle = new Point(render.Rect.Width - 24f, 68f);
        ctx.Translate(Point.Lerp(hamburgerInContent, hamburgerInTitle, EaseInOutQuad(Normalize(-this.scrollPoint.Y, 200, 270))));
        ctx.Render(Hamburger.Instance);
        ctx.Restore();

        ctx.Save();
        var spyGlassInContent = new Point(render.Rect.Width * 0.5f, 420 + this.scrollPoint.Y);
        var spyGlassInTitle = new Point(render.Rect.Width - 58f, 68f);
        ctx.Translate(Point.Lerp(spyGlassInContent, spyGlassInTitle, EaseInOutQuad(Normalize(-this.scrollPoint.Y, 220, 290))));
        ctx.Render(SpyGlass.Instance);
        ctx.Restore();

        ctx.Save();
        var starInContent = new Point(render.Rect.Width * 0.5f - 64f, 420 + this.scrollPoint.Y);
        var starInTitle = new Point(render.Rect.Width - 92f, 68f);
        ctx.Translate(Point.Lerp(starInContent, starInTitle, EaseInOutQuad(Normalize(-this.scrollPoint.Y, 240, 310))));
        ctx.Render(Star.Instance);
        ctx.Restore();

        // Logo
        var logoDisappearProgress = Normalize(-this.scrollPoint.Y, 0, 150);
        var logoScale = NFloat.Lerp(2f, 0.5f, EaseInOutSine(logoDisappearProgress));
        ctx.Save();
        var logoPositionInContent = new Point(centerGuide, 180 + this.scrollPoint.Y);
        var logoPositionInHeader = new Point(centerGuide - 124, 64);
        var logoHeaderShiftProgress = Normalize(-this.scrollPoint.Y, 70, 140);
        var logoPosition = Point.Lerp(logoPositionInContent, logoPositionInHeader, EaseInOutSine(logoHeaderShiftProgress));
        ctx.Translate(logoPosition);
        ctx.Scale((logoScale, logoScale));
        ctx.Translate((-32, -32));
        ctx.Render(XuiLogo.Instance);
        ctx.Restore();

        // Title
        var titleContentToHeaderProgress = Normalize(-this.scrollPoint.Y, 60, 190);
        var titlePositionInContent = new Point(centerGuide, 260 + this.scrollPoint.Y);
        var titlePositionInHeader = new Point(centerGuide - 60, 56);
        var titlePosition = Point.Lerp(
            titlePositionInContent,
            titlePositionInHeader,
            EaseInOutQuad(titleContentToHeaderProgress));
        ctx.SetFill(0x000000FF);
        ctx.TextAlign = TextAlign.Center;
        ctx.SetFont(new Font() {
            FontFamily = ["Inter"],
            FontSize = NFloat.Lerp(32f, 22f, EaseInOutSine(titleContentToHeaderProgress)),
            FontStyle = FontStyle.Normal,
            FontWeight = 700,
            LineHeight = 32
        });
        ctx.TextBaseline = TextBaseline.Alphabetic;
        ctx.FillText("Xuiapps", titlePosition);

        // Settings
        

        // ctx.BeginPath();
        // ctx.PathData()
        //     .M((10, 30))
        //     .A((20, 20), 0, ArcFlag.Small, Winding.ClockWise, (50, 30))
        //     .A((20, 20), 0, ArcFlag.Small, Winding.ClockWise, (90, 30))
        //     .Q((90, 60), (50, 90))
        //     .Q((10, 60), (10, 30))
        //     .Z();
        // ctx.SetFill(0xFF0000FF);
        // ctx.Fill();

        return;

        // var rect = render.Rect;
        // try
        // {
        //     // Not all graphics contexts are fully implemented, let's see how far we can go...
        //     ctx.SetFill(new Color(0x222222FF));
        //     ctx.FillRect(rect.Inset(20));

        //     ctx.SetStroke(new Color(0x888888FF));
        //     ctx.StrokeRect(rect.Inset(20));

        //     // Excel sim
        //     int left = (int)((-scrollPoint.X - 100f + rect.X) / 100f);
        //     int right = (int)((-scrollPoint.X + rect.X + 100f + rect.Width) / 100f);
        //     int top = (int)((-scrollPoint.Y - 24f + rect.Y)/ 24f);
        //     int bottom = (int)((-scrollPoint.Y + rect.Y + 24f + rect.Height) / 24f);
        //     ctx.SetFill(Colors.Gray);
        //     ctx.SetFont(new Font()
        //     {
        //         FontFamily = ["Verdana"],
        //         FontWeight = 400,
        //         FontSize = 12,
        //         FontStyle = FontStyle.CustomOblique(14f), // FontStyle.Italic,
        //         LineHeight = 32
        //     });

        //     ctx.TextBaseline = TextBaseline.Top;
        //     for (int x = left; x <= right; x++)
        //     {
        //         for (int y = top; y <= bottom; y++)
        //         {
        //             var cellRect = new Rect(x * 100f + scrollPoint.X, y * 24f + scrollPoint.Y, 100f, 24f);

        //             var color = (uint)(x * 98873 ^ y * 98869);

        //             ctx.SetFill(new Color((cellRect.Contains(mousePoint) ? (uint)0x999999FF : (uint)0x222222FF) | color));
        //             ctx.FillRect(cellRect);

        //             ctx.SetStroke(new Color(0x444444FF));
        //             ctx.StrokeRect(cellRect);

        //             ctx.SetFill(Colors.White);
        //             ctx.FillText($"C {x}x{y}", cellRect.TopLeft);
        //         }
        //     }

        //     ctx.MoveTo(new (30, 30));
        //     ctx.LineTo(new (30, 40));
        //     ctx.CurveTo(new (30, 50), new (50, 50), new (50, 40));
        //     ctx.LineTo(new (50, 30));
        //     ctx.ClosePath();

        //     ctx.SetFill(new Color(0x444444FF));
        //     ctx.Fill();

        //     ctx.SetStroke(new Color(0xAAAAAAFF));
        //     ctx.LineWidth = 1f;

        //     ctx.MoveTo(new (30, 30));
        //     ctx.LineTo(new (30, 40));
        //     ctx.CurveTo(new (30, 50), new (50, 50), new (50, 40));
        //     ctx.LineTo(new (50, 30));
        //     ctx.ClosePath();

        //     ctx.Stroke();

        //     // Button 1
        //     var btn1Rect = new Rect(100.5f, 50.5f, 200, 32);
        //     if (btn1Rect.Contains(this.mousePoint))
        //     {
        //         ctx.SetFill(LightGray);
        //         ctx.SetStroke(White);
        //     }
        //     else
        //     {
        //         ctx.SetFill(Gray);
        //         ctx.SetStroke(LightGray);
        //     }

        //     ctx.BeginPath();
        //     ctx.RoundRect(btn1Rect, 5);
        //     ctx.Fill();

        //     ctx.BeginPath();
        //     ctx.RoundRect(btn1Rect, 5);
        //     ctx.Stroke();

        //     var btn2Rect = new Rect(100.5f, 100.5f, 200, 32);
        //     if (btn2Rect.Contains(this.mousePoint))
        //     {
        //         ctx.SetFill(new LinearGradient(
        //             start: new (100, 100),
        //             end: new (100, 132),
        //             [
        //                 new (0, 0x9999FFFF),
        //                 new (1, 0x4444BBFF),
        //             ]));
        //     }
        //     else
        //     {
        //         ctx.SetFill(new LinearGradient(
        //             start: new (100, 100),
        //             end: new (100, 132),
        //             [
        //                 new (0, 0x7777FFFF),
        //                 new (1, 0x2222AAFF),
        //             ]));
        //     }
        //     ctx.BeginPath();
        //     ctx.RoundRect(new (100.5f, 100.5f, 200, 32), 5);
        //     ctx.Fill();

        //     var btn3Rect = new Rect(100.5f, 150.5f, 200, 32);
        //     if (btn3Rect.Contains(this.mousePoint))
        //     {
        //         ctx.SetFill(
        //             new RadialGradient(
        //                 center: new (200, 166),
        //                 radius: 150f,
        //                 [
        //                     new (0, 0xAAAAFFFF),
        //                     new (1, 0xFF55EEFF),
        //                 ]
        //             ));
        //     }
        //     else
        //     {
        //         ctx.SetFill(
        //             new RadialGradient(
        //                 center: new (200, 166),
        //                 radius: 150f,
        //                 [
        //                     new (0, 0x7777FFFF),
        //                     new (1, 0xFF22AAFF),
        //                 ]
        //             ));
        //     }
        //     ctx.BeginPath();
        //     ctx.RoundRect(btn3Rect, 5);
        //     ctx.Fill();

        //     ctx.SetStroke(
        //         new LinearGradient(
        //             start: new (150, 150),
        //             end: new (150, 182),
        //             [
        //                 new (0, 0x7777FFFF),
        //                 new (1, 0xFF22AAFF),
        //             ]
        //         ));
        //     ctx.BeginPath();
        //     ctx.RoundRect(new (100.5f, 150.5f, 200, 32), 5);
        //     ctx.LineWidth = 2;
        //     ctx.Stroke();

        //     ctx.BeginPath();
        //     ctx.RoundRect(
        //         new Rect(100.5f, 200.5f, 200, 32),
        //         new CornerRadius(5, 20, 5, 20));
        //     ctx.SetFill(new Color(0x00FFFFFF));
        //     ctx.Fill();

        //     ctx.BeginPath();
        //     ctx.MoveTo((200f, 50f));
        //     ctx.ArcTo((250f, 50f), (250f, 75f), 15f);
        //     ctx.ArcTo((250f, 100f), (300f, 100f), 15f);
        //     ctx.ClosePath();
        //     ctx.Stroke();

        //     ctx.SetStroke(0xFFFFFFFF);
        //     ctx.BeginPath();
        //     // Draw the ellipse
        //     ctx.Ellipse(new (100, 100), 50, 75, NFloat.Pi / 4f, 0, 2f * NFloat.Pi);
        //     ctx.Stroke();

        //     // Draw the ellipse's line of reflection
        //     ctx.BeginPath();
        //     try
        //     {
        //         ctx.SetLineDash([5, 5]);
        //     }
        //     catch(Exception e)
        //     {
        //         Debug.WriteLine("SetLineDash exception: " + e);
        //     }
        //     ctx.MoveTo(new (0, 200));
        //     ctx.LineTo(new (200, 0));
        //     ctx.Stroke();
        //     try
        //     {
        //         ctx.SetLineDash([]);
        //     }
        //     catch(Exception e)
        //     {
        //         Debug.WriteLine("SetLineDash exception: " + e);
        //     }

        //     ctx.SetFill(Red);
        //     ctx.BeginPath();
        //     ctx.Ellipse(new (60, 75), 50, 30, NFloat.Pi * 0.25f, 0, NFloat.Pi * 1.5f);
        //     ctx.Fill();

        //     ctx.SetFill(Blue);
        //     ctx.BeginPath();
        //     ctx.Ellipse(new (150, 75), 50, 30, NFloat.Pi * 0.25f, 0, NFloat.Pi);
        //     ctx.Fill();

        //     ctx.SetFill(Green);
        //     ctx.BeginPath();
        //     ctx.Ellipse(new (240, 75), 50, 30, NFloat.Pi * 0.25f, 0, NFloat.Pi, Winding.CounterClockWise);
        //     ctx.Fill();

        //     ctx.SetFill(Blue);
        //     ctx.BeginPath();
        //     ctx.MoveTo(new (60, 75));
        //     ctx.Ellipse(new (60, 75), 50, 30, NFloat.Pi * 0.25f, 0, NFloat.Pi * 0.25f);
        //     ctx.Fill();

        //     ctx.SetFill(Green);
        //     ctx.BeginPath();
        //     ctx.MoveTo(new (60, 75));
        //     ctx.Ellipse(new (60, 75), 50, 30, NFloat.Pi * 0.25f, NFloat.Pi * 0.25f, NFloat.Pi * 0.5f);
        //     ctx.Fill();

        //     ctx.Save();
        //     ctx.SetStroke(Orange);
        //     ctx.Translate(new (400, 100));
        //     ctx.Rotate(NFloat.Pi * 0.125f);
        //     ctx.StrokeRect(new (-50, -50, 100, 100));
        //     ctx.Restore();

        //     ctx.Save();
        //     ctx.SetStroke(Cyan);
        //     ctx.Transform(new AffineTransform(
        //         NFloat.CosPi(0.125f),
        //         NFloat.SinPi(0.125f),
        //         -NFloat.SinPi(0.125f),
        //         NFloat.CosPi(0.125f),
        //         410,
        //         100)); 
        //     ctx.StrokeRect(new (-50, -50, 100, 100));
        //     ctx.Restore();

        //     ctx.Save();
        //     ctx.SetStroke(Purple);

        //     ctx.Translate(new (400, 100));
        //     ctx.Rotate(NFloat.Pi * 0.125f);
        //     // Set will inverse the transform and rotate
        //     ctx.SetTransform(new AffineTransform(
        //         NFloat.CosPi(0.125f),
        //         NFloat.SinPi(0.125f),
        //         -NFloat.SinPi(0.125f),
        //         NFloat.CosPi(0.125f),
        //         420,
        //         100));
        //     ctx.StrokeRect(new (-50, -50, 100, 100));
        //     ctx.Restore();

        //     ctx.SetFill(Red);
        //     ctx.SetFont(new Font()
        //     {
        //         FontFamily = ["Times New Roman"], //, "Verdana"], // ["Times New Roman", "Verdana"],
        //         FontWeight = 400,
        //         FontSize = 24,
        //         FontStyle = FontStyle.CustomOblique(14f), // FontStyle.Italic,
        //         LineHeight = 32
        //     });
        //     ctx.FillText("Hello World! 中文", new (200, 100));

        //     // Draw cursor...
        //     ctx.LineWidth = 3;
        //     ctx.SetStroke(Colors.White);
        //     ctx.MoveTo(new (this.mousePoint.X - 20, this.mousePoint.Y));
        //     ctx.LineTo(new (this.mousePoint.X + 20, this.mousePoint.Y));
        //     ctx.Stroke();
        //     ctx.MoveTo(new (this.mousePoint.X, this.mousePoint.Y - 20));
        //     ctx.LineTo(new (this.mousePoint.X, this.mousePoint.Y + 20));
        //     ctx.Stroke();
        // }
        // catch
        // {
        //     // Console.WriteLine("We had some progres with IContext but that's how far we could go: " + e);
        // }

        // try
        // {
        //     var fps = $"{this.fps} fps";
        //     var fpsSize = ctx.MeasureText(fps);
        //     Rect fpsRect = new (
        //         topLeft: rect.TopLeft + (40, 60),
        //         size: fpsSize);

        //     ctx.SetFill(DarkRed);
            
        //     ctx.RoundRect(fpsRect.Expand(12, 6), 10);
        //     ctx.Fill();
        //     // ctx.FillRect(fpsRect.Expand(12, 6));

        //     ctx.SetFill(White);
        //     ctx.FillText(fps, fpsRect.TopLeft);

        //     var stutter = $"Did I stutter: {this.didIStutter}";
        //     var didIStutterSize = ctx.MeasureText(stutter);
        //     Rect didIStutterRect = new (
        //         topLeft: rect.BottomLeft - (0, didIStutterSize.Y) + (40, -60),
        //         didIStutterSize);

        //     ctx.SetFill(DarkRed);
        //     ctx.BeginPath();
        //     ctx.RoundRect(didIStutterRect.Expand(12, 6), 10);
        //     ctx.Fill();
        //     // ctx.FillRect(didIStutterRect.Expand(12, 6));

        //     ctx.SetFill(White);
        //     ctx.FillText(stutter, didIStutterRect.TopLeft);
        // }
        // catch
        // {
        //     // Console.WriteLine("We had some progres with IContext but that's how far we could go: " + e);
        // }
    }
}
