using System.Runtime.InteropServices;
using Xui.Core.Canvas;
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
    private GpuImage? gpuImage = null;
    
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
        // Initialize GPU resources if needed
        if (!isInitialized || framebuffer == null)
        {
            framebuffer?.Dispose();
            framebuffer = new Framebuffer(300, 300, withDepthBuffer: true);
            isInitialized = true;
        }

        // Render the 3D cube to the framebuffer
        RenderCube(framebuffer.Value);

        // Create or update the GPU image wrapper
        if (gpuImage == null)
        {
            gpuImage = new GpuImage(framebuffer.Value);
        }
        else
        {
            gpuImage.UpdateFramebuffer(framebuffer.Value);
        }

        // Draw the GPU-rendered image to the canvas
        var destRect = new Rect(this.Frame.X, this.Frame.Y, 300, 300);
        context.DrawImage(gpuImage, destRect);

        // Draw border
        context.SetStroke(Black);
        context.StrokeRect(destRect);
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

/// <summary>
/// Wrapper class that bridges GPU Framebuffer to IImage for canvas rendering.
/// This allows GPU-rendered content to be drawn using standard canvas DrawImage APIs.
/// </summary>
public class GpuImage : IImage
{
    private unsafe Framebuffer framebuffer;
    private byte[]? cachedBytes;
    private bool isDirty = true;

    public unsafe Size Size => new Size(framebuffer.Width, framebuffer.Height);

    public unsafe GpuImage(Framebuffer fb)
    {
        framebuffer = fb;
    }

    public unsafe void UpdateFramebuffer(Framebuffer fb)
    {
        framebuffer = fb;
        isDirty = true;
    }

    public void Load(string uri)
    {
        // GpuImage is already loaded - it wraps a live framebuffer
        // This method is here to satisfy IImage interface but is not used
        // The framebuffer is updated via UpdateFramebuffer()
    }

    public Task LoadAsync(string uri)
    {
        // GpuImage is already loaded - async version
        return Task.CompletedTask;
    }

    public void Dispose()
    {
        // GpuImage doesn't own the framebuffer, so don't dispose it
        cachedBytes = null;
    }

    // Platform-specific rendering would access the framebuffer data
    // For now, this provides the byte array for software rendering
    internal unsafe byte[] GetBytes()
    {
        if (isDirty || cachedBytes == null)
        {
            cachedBytes = framebuffer.ToByteArray();
            isDirty = false;
        }
        return cachedBytes;
    }
}
