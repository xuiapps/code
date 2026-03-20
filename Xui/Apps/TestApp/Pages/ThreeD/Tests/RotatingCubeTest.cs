using System.Runtime.InteropServices;
using Xui.Core.Canvas;
using Xui.Core.DI;
using Xui.Core.Math2D;
using Xui.Core.UI;
using Xui.Core.UI.Input;
using Xui.GPU.Shaders.Types;
using Xui.GPU.Software;
using static Xui.Core.Canvas.Colors;

namespace Xui.Apps.TestApp.Pages.ThreeD.Tests;

/// <summary>
/// A 300x300 3D surface showing a rotating cube.
/// Features click-and-drag rotation interaction.
/// </summary>
public class RotatingCubeTest : View
{
    private NFloat rotationX = 0.3f;  // Initial X rotation in radians
    private NFloat rotationY = 0.3f;  // Initial Y rotation in radians
    private Point? dragStart = null;
    private NFloat dragStartRotX = 0;
    private NFloat dragStartRotY = 0;
    
    private unsafe Framebuffer? framebuffer = null;
    private IImage? platformImage = null;
    private byte[]? bgraBuffer = null;

    private bool isInitialized = false;

    public RotatingCubeTest()
    {
        // Animation would be added here if Window animation API exists
    }

    private void OnAnimationFrame(double timestamp)
    {
        // Auto-rotate slowly if not being dragged
        if (dragStart == null)
        {
            rotationY += 0.01f;
        }
        
        this.InvalidateRender();
        // Window animation would be requested here
    }

    public override void OnPointerEvent(ref PointerEventRef e, EventPhase phase)
    {
        if (e.State.PointerType == PointerType.Mouse)
        {
            if (phase == EventPhase.Tunnel && e.Type == PointerEventType.Down)
            {
                this.CapturePointer(e.PointerId);
                dragStart = e.State.Position;
                dragStartRotX = rotationX;
                dragStartRotY = rotationY;
            }
            else if (e.Type == PointerEventType.Move && dragStart != null)
            {
                var delta = e.State.Position - dragStart.Value;
                rotationY = dragStartRotY + delta.X * 0.01f;
                rotationX = dragStartRotX + delta.Y * 0.01f;
                this.InvalidateRender();
            }
            else if (phase == EventPhase.Tunnel && e.Type == PointerEventType.Up)
            {
                this.ReleasePointer(e.PointerId);
                dragStart = null;
            }
        }

        base.OnPointerEvent(ref e, phase);
    }

    protected override Size MeasureCore(Size availableBorderEdgeSize, IMeasureContext context)
    {
        return new Size(300, 300);
    }

    protected override unsafe void RenderCore(IContext context)
    {
        // Initialize resources if needed
        if (!isInitialized || framebuffer == null)
        {
            framebuffer?.Dispose();
            framebuffer = new Framebuffer(300, 300, withDepthBuffer: true);
            bgraBuffer = new byte[300 * 300 * 4];
            platformImage = this.GetService<IImage>();
            isInitialized = true;
        }

        // Render the 3D cube to the software framebuffer
        RenderCube(framebuffer.Value);

        // Convert framebuffer pixels (stored as uint 0xRRGGBBAA, LE bytes [A,B,G,R])
        // to BGRA bytes ([B,G,R,A]) expected by DXGI_FORMAT_B8G8R8A8_UNORM.
        ConvertToBgra(framebuffer.Value, bgraBuffer!);

        // Upload pixels to the platform image and draw
        platformImage?.LoadPixels(300, 300, bgraBuffer);
        var destRect = new Rect(this.Frame.X, this.Frame.Y, 300, 300);
        if (platformImage != null)
            context.DrawImage(platformImage, destRect);

        // Draw border
        context.SetStroke(Black);
        context.StrokeRect(destRect);
    }

    private static unsafe void ConvertToBgra(Framebuffer fb, byte[] bgra)
    {
        uint* src = fb.ColorData;
        int count = fb.Width * fb.Height;
        for (int i = 0; i < count; i++)
        {
            uint px = src[i]; // 0xRRGGBBAA in value
            bgra[i * 4 + 0] = (byte)(px >> 8);   // B
            bgra[i * 4 + 1] = (byte)(px >> 16);  // G
            bgra[i * 4 + 2] = (byte)(px >> 24);  // R
            bgra[i * 4 + 3] = (byte)(px);        // A
        }
    }

    private unsafe void RenderCube(Framebuffer fb)
    {
        // Clear framebuffer
        fb.ClearColor(0xFF808080);  // Gray background
        fb.ClearDepth(1.0f);

        // Simple cube rendering using software rasterization
        // This is a placeholder - ideally would use the GPU shader pipeline
        
        // Define cube vertices in 3D space (centered at origin)
        var vertices = new[]
        {
            new Float3(-1, -1, -1), new Float3(1, -1, -1), new Float3(1, 1, -1), new Float3(-1, 1, -1),  // Front
            new Float3(-1, -1, 1),  new Float3(1, -1, 1),  new Float3(1, 1, 1),  new Float3(-1, 1, 1),   // Back
        };

        // Cube face colors
        var colors = new[]
        {
            new Color4(1, 0, 0, 1),  // Red
            new Color4(0, 1, 0, 1),  // Green
            new Color4(0, 0, 1, 1),  // Blue
            new Color4(1, 1, 0, 1),  // Yellow
            new Color4(1, 0, 1, 1),  // Magenta
            new Color4(0, 1, 1, 1),  // Cyan
        };

        // Transform vertices
        var transformed = new Float2[8];
        for (int i = 0; i < vertices.Length; i++)
        {
            transformed[i] = Project3DTo2D(vertices[i], rotationX, rotationY);
        }

        // Draw cube faces (simple painter's algorithm - back to front)
        DrawQuad(fb, transformed[4], transformed[5], transformed[6], transformed[7], colors[5]);  // Back
        DrawQuad(fb, transformed[1], transformed[5], transformed[4], transformed[0], colors[3]);  // Left
        DrawQuad(fb, transformed[2], transformed[6], transformed[5], transformed[1], colors[4]);  // Right
        DrawQuad(fb, transformed[3], transformed[7], transformed[6], transformed[2], colors[2]);  // Top
        DrawQuad(fb, transformed[0], transformed[4], transformed[7], transformed[3], colors[1]);  // Bottom
        DrawQuad(fb, transformed[0], transformed[1], transformed[2], transformed[3], colors[0]);  // Front
    }

    private Float2 Project3DTo2D(Float3 point, NFloat rotX, NFloat rotY)
    {
        // Simple rotation matrices
        float cx = MathF.Cos((float)rotX);
        float sx = MathF.Sin((float)rotX);
        float cy = MathF.Cos((float)rotY);
        float sy = MathF.Sin((float)rotY);

        // Rotate around X axis
        float y1 = (float)point.Y * cx - (float)point.Z * sx;
        float z1 = (float)point.Y * sx + (float)point.Z * cx;

        // Rotate around Y axis
        float x2 = (float)point.X * cy + z1 * sy;
        float z2 = -(float)point.X * sy + z1 * cy;

        // Perspective projection
        float distance = 5.0f;
        float scale = 60.0f / (distance + z2);

        return new Float2(
            new F32(x2 * scale + 150),
            new F32(y1 * scale + 150)
        );
    }

    private unsafe void DrawQuad(Framebuffer fb, Float2 p1, Float2 p2, Float2 p3, Float2 p4, Color4 color)
    {
        // Simple filled quad using scanline rasterization
        uint packedColor = ColorTarget.ToRgba32(color);

        // Draw two triangles
        DrawTriangle(fb, p1, p2, p3, packedColor);
        DrawTriangle(fb, p1, p3, p4, packedColor);
    }

    private unsafe void DrawTriangle(Framebuffer fb, Float2 v1, Float2 v2, Float2 v3, uint color)
    {
        // Simple triangle rasterization
        int x1 = (int)(float)v1.X, y1 = (int)(float)v1.Y;
        int x2 = (int)(float)v2.X, y2 = (int)(float)v2.Y;
        int x3 = (int)(float)v3.X, y3 = (int)(float)v3.Y;

        // Bounding box
        int minX = Math.Max(0, Math.Min(x1, Math.Min(x2, x3)));
        int maxX = Math.Min(299, Math.Max(x1, Math.Max(x2, x3)));
        int minY = Math.Max(0, Math.Min(y1, Math.Min(y2, y3)));
        int maxY = Math.Min(299, Math.Max(y1, Math.Max(y2, y3)));

        // Scanline fill
        for (int y = minY; y <= maxY; y++)
        {
            for (int x = minX; x <= maxX; x++)
            {
                if (PointInTriangle(x, y, x1, y1, x2, y2, x3, y3))
                {
                    fb.SetColor(x, y, color);
                }
            }
        }
    }

    private bool PointInTriangle(int px, int py, int x1, int y1, int x2, int y2, int x3, int y3)
    {
        // Barycentric coordinate test
        int d1 = Sign(px, py, x1, y1, x2, y2);
        int d2 = Sign(px, py, x2, y2, x3, y3);
        int d3 = Sign(px, py, x3, y3, x1, y1);

        bool hasNeg = (d1 < 0) || (d2 < 0) || (d3 < 0);
        bool hasPos = (d1 > 0) || (d2 > 0) || (d3 > 0);

        return !(hasNeg && hasPos);
    }

    private int Sign(int px, int py, int x1, int y1, int x2, int y2)
    {
        return (px - x2) * (y1 - y2) - (x1 - x2) * (py - y2);
    }
}

