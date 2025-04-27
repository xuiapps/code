namespace Xui.Core.Canvas;

/// <summary>
/// Defines a measurement context that provides access to platform-specific text metrics,
/// supports subpixel snapping for layout precision, and enables accurate text size calculations
/// using the underlying rendering engine's font rasterization and shaping systems.
/// </summary>
public interface IMeasureContext : ITextMeasureContext
{
}