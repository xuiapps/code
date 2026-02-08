using System.Runtime.CompilerServices;
using Xui.Apps.TestApp.Examples;
using Xui.Core.Math2D;
using Xui.Core.UI;
using Xui.Runtime.Software.Actual;

namespace Xui.Tests.TestApp.Example;

/// <summary>
/// Component tests for the AnimatedHeartExample.
/// Renders the heart at key time points within its beat cycle (BPM=72, cycle≈0.833s)
/// and compares SVG snapshots to verify the animation produces correct output.
///
/// Key phases within a single heartbeat cycle:
///   t=0.00s  (x=0.00) — Rest: no pulse, heart at natural scale.
///   t=0.10s  (x=0.12) — Primary peak: heart maximally expanded.
///   t=0.25s  (x=0.30) — Gap: between primary and secondary pulses, heart at rest.
///   t=0.35s  (x=0.42) — Secondary peak: heart expanded (smaller echo pulse).
/// </summary>
public class AnimatedHeartExampleTest
{
    [Theory]
    [InlineData("AnimatedHeart.Rest", 0.0)]
    [InlineData("AnimatedHeart.PrimaryPeak", 0.10)]
    [InlineData("AnimatedHeart.BetweenPulses", 0.25)]
    [InlineData("AnimatedHeart.SecondaryPeak", 0.35)]
    public void SvgSnapshotAtTimePoint(string name, double timeSeconds)
    {
        var view = new AnimatedHeartExample();
        var actual = RenderAtTime(view, TimeSpan.FromSeconds(timeSeconds));

        var expectedPath = GetSnapshotPath($"Snapshots/{name}.svg");
        var actualPath = GetSnapshotPath($"Snapshots/{name}.Actual.svg");

        string expected;
        try
        {
            expected = File.ReadAllText(expectedPath);
        }
        catch
        {
            expected = "";
        }

        try
        {
            Assert.Equal(expected, actual);
        }
        catch
        {
            File.WriteAllText(actualPath, actual);
            throw;
        }
    }

    private static string GetSnapshotPath(string fileName, [CallerFilePath] string callerPath = "")
    {
        var sourceDir = Path.GetDirectoryName(callerPath)!;
        return Path.Combine(sourceDir, fileName);
    }

    private static string RenderAtTime(View view, TimeSpan animationTime)
    {
        var size = new Size(600, 400);
        using var stream = new MemoryStream();
        using (var context = new SvgDrawingContext(size, stream, Xui.Core.Fonts.Inter.URIs, keepOpen: true))
        {
            view.Update(new LayoutGuide
            {
                Anchor = (0, 0),
                AvailableSize = size,
                Pass = LayoutGuide.LayoutPass.Animate
                     | LayoutGuide.LayoutPass.Measure
                     | LayoutGuide.LayoutPass.Arrange
                     | LayoutGuide.LayoutPass.Render,
                PreviousTime = TimeSpan.Zero,
                CurrentTime = animationTime,
                MeasureContext = context,
                RenderContext = context,
                XAlign = LayoutGuide.Align.Start,
                YAlign = LayoutGuide.Align.Start,
                XSize = LayoutGuide.SizeTo.Exact,
                YSize = LayoutGuide.SizeTo.Exact
            });
        }
        stream.Position = 0;
        return new StreamReader(stream).ReadToEnd();
    }
}
