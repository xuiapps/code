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
            // Layout + render
            view.Update(new LayoutGuide
            {
                Anchor = (0, 0),
                AvailableSize = size,
                Pass = LayoutGuide.LayoutPass.Measure | LayoutGuide.LayoutPass.Arrange | LayoutGuide.LayoutPass.Render,
                MeasureContext = context,
                RenderContext = context,
                XAlign = LayoutGuide.Align.Start,
                YAlign = LayoutGuide.Align.Start,
                XSize = LayoutGuide.SizeTo.Exact,
                YSize = LayoutGuide.SizeTo.Exact
            });
        }
        stream.Position = 0;
        var svg = new StreamReader(stream).ReadToEnd();
        return svg;
    }
}
