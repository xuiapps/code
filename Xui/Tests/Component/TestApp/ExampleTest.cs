using System.Runtime.CompilerServices;
using Xui.Apps.TestApp.Examples;
using Xui.Core.Math2D;
using Xui.Core.UI;
using Xui.Runtime.Software.Actual;

namespace Xui.Tests.TestApp.Example;

public class NestedStacksExampleTest
{
    [Theory]
    [InlineData("NestedStacksExample", typeof(NestedStacksExample))]
    [InlineData("TextLayoutExample", typeof(TextLayoutExample))]
    [InlineData("ViewCollectionAlignmentExample", typeof(ViewCollectionAlignmentExample))]
    [InlineData("TextMetricsExample", typeof(TextMetricsExample))]
    public void SvgExampleSnapshotTest(string name, Type type)
    {
        var view = (View)Activator.CreateInstance(type)!;
        var actual = Render(view);

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

    private static string Render(View view)
    {
        var size = new Size(600, 400);
        using var stream = new MemoryStream();
        using (var context = new SvgDrawingContext(size, stream, Xui.Core.Fonts.Inter.URIs, keepOpen: true))
        {
            var frame = new LayoutFrameContext(TimeSpan.Zero, TimeSpan.Zero, context, context, default);
            var update = new LayoutUpdate(
                LayoutPass.Measure | LayoutPass.Arrange | LayoutPass.Render,
                new MeasureConstraints(size, LayoutSizeMode.Exact, LayoutSizeMode.Exact),
                new ArrangeConstraints(size, default, (0, 0)));
            LayoutMeasurements measurements = default;
            view.Update(in frame, in update, ref measurements);
        }
        stream.Position = 0;
        var svg = new StreamReader(stream).ReadToEnd();
        return svg;
    }
}
