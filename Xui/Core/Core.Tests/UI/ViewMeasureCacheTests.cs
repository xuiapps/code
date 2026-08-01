using Xui.Core.Canvas;
using Xui.Core.Math2D;

namespace Xui.Core.UI.Tests;

public class ViewMeasureCacheTests
{
    [Fact]
    public void Measure_ReusesTheLastResultForTheSameConstraints()
    {
        var view = new CountingView();

        Assert.Equal(new Size(20, 10), view.Measure((100, 100), null!));
        Assert.Equal(new Size(20, 10), view.Measure((100, 100), null!));

        Assert.Equal(1, view.MeasureCount);
    }

    [Fact]
    public void Arrange_ReusesTheMeasureMadeByItsParent()
    {
        var view = new CountingView
        {
            HorizontalAlignment = HorizontalAlignment.Left,
            VerticalAlignment = VerticalAlignment.Top,
        };

        var desired = view.Measure((100, 100), null!);
        view.Arrange(new Rect(0, 0, 100, 100), null!);

        Assert.Equal(new Size(20, 10), desired);
        Assert.Equal(1, view.MeasureCount);
    }

    [Fact]
    public void Measure_DoesNotReuseAResultForDifferentConstraints()
    {
        var view = new CountingView();

        view.Measure((100, 100), null!);
        view.Measure((200, 100), null!);

        Assert.Equal(2, view.MeasureCount);
    }

    [Fact]
    public void Measure_DoesNotReuseAResultForDifferentConstraintModes()
    {
        var view = new CountingView
        {
            HorizontalAlignment = HorizontalAlignment.Left,
            VerticalAlignment = VerticalAlignment.Top,
        };
        Measure(view, LayoutSizeMode.AtMost, LayoutSizeMode.AtMost);
        Measure(view, LayoutSizeMode.Exact, LayoutSizeMode.AtMost);

        Assert.Equal(2, view.MeasureCount);
    }

    [Fact]
    public void ChangingAlignment_InvalidatesTheCachedMeasurement()
    {
        var view = new CountingView();
        Assert.Equal(new Size(100, 100), Measure(view, LayoutSizeMode.Exact, LayoutSizeMode.Exact));
        view.HorizontalAlignment = HorizontalAlignment.Left;

        Assert.Equal(new Size(20, 100), Measure(view, LayoutSizeMode.Exact, LayoutSizeMode.Exact));
        Assert.Equal(1, view.MeasureCount);
    }

    [Fact]
    public void ChildMeasureInvalidation_InvalidatesTheAncestorMeasureCache()
    {
        var child = new CountingView();
        var parent = new CountingVerticalStack { Content = [child] };

        Assert.Equal(new Size(20, 10), parent.Measure((100, 100), null!));
        child.ChangeSize((40, 30));

        Assert.Equal(new Size(40, 30), parent.Measure((100, 100), null!));
        Assert.Equal(2, parent.MeasureCount);
        Assert.Equal(2, child.MeasureCount);
    }

    [Fact]
    public void AddingAChild_InvalidatesTheParentMeasureCache()
    {
        var parent = new CountingVerticalStack();

        Assert.Equal(Size.Empty, parent.Measure((100, 100), null!));
        parent.Add(new CountingView());

        Assert.Equal(new Size(20, 10), parent.Measure((100, 100), null!));
        Assert.Equal(2, parent.MeasureCount);
    }

    [Fact]
    public void Arrange_ReusesTheLastResultForTheSameGuide()
    {
        var view = new CountingArrangeView();
        var rect = new Rect(0, 0, 100, 100);

        view.Arrange(rect, null!);
        view.Arrange(rect, null!);

        Assert.Equal(1, view.ArrangeCount);
    }

    [Fact]
    public void ChildArrangeInvalidation_RearrangesOnlyTheChangedBranch()
    {
        var changedChild = new CountingArrangeView();
        var unchangedChild = new CountingArrangeView();
        var parent = new VerticalStack { Content = [changedChild, unchangedChild] };
        var rect = new Rect(0, 0, 100, 100);

        parent.Arrange(rect, null!);
        changedChild.ChangeArrangement();
        parent.Arrange(rect, null!);

        Assert.Equal(2, changedChild.ArrangeCount);
        Assert.Equal(1, unchangedChild.ArrangeCount);
    }

    private static Size Measure(View view, LayoutSizeMode xSize, LayoutSizeMode ySize)
    {
        var frame = default(LayoutFrameContext);
        var update = new LayoutUpdate(
            LayoutPass.Measure,
            new MeasureConstraints((100, 100), xSize, ySize),
            default);
        LayoutMeasurements measurements = default;
        view.Update(in frame, in update, ref measurements);
        return measurements.DesiredSize;
    }

    private sealed class CountingView : View
    {
        private Size size = (20, 10);

        public int MeasureCount { get; private set; }

        protected override Size MeasureCore(Size availableBorderEdgeSize, IMeasureContext context)
        {
            this.MeasureCount++;
            return this.size;
        }

        public void ChangeSize(Size value)
        {
            this.size = value;
            this.InvalidateMeasure();
        }
    }

    private sealed class CountingVerticalStack : VerticalStack
    {
        public int MeasureCount { get; private set; }

        protected override Size MeasureCore(Size availableBorderEdgeSize, IMeasureContext context)
        {
            this.MeasureCount++;
            return base.MeasureCore(availableBorderEdgeSize, context);
        }
    }

    private sealed class CountingArrangeView : View
    {
        public int ArrangeCount { get; private set; }

        protected override void ArrangeCore(Rect rect, IMeasureContext context)
        {
            this.ArrangeCount++;
        }

        public void ChangeArrangement() => this.InvalidateArrange();
    }
}
