using Xui.Core.Math2D;
using Xui.Core.Canvas;

namespace Xui.Core.UI.Layout;

/// <summary>
/// CSS Flexible Box Layout implementation for the FlexBox container.
/// Provides MeasureCore and ArrangeCore implementations following the CSS Flexible Box Layout Module Level 1 specification.
/// </summary>
/// <remarks>
/// <para>Implementation notes:</para>
/// <list type="bullet">
/// <item>Flex direction: row, row-reverse, column, column-reverse</item>
/// <item>Flex wrap: nowrap, wrap, wrap-reverse</item>
/// <item>Flex item sizing: flex-grow, flex-shrink, flex-basis</item>
/// <item>Main axis alignment: justify-content (flex-start, flex-end, center, space-between, space-around, space-evenly)</item>
/// <item>Cross axis alignment: align-items (stretch, flex-start, flex-end, center, baseline)</item>
/// <item>Multi-line cross axis: align-content (stretch, flex-start, flex-end, center, space-between, space-around, space-evenly)</item>
/// <item>Gaps: row-gap and column-gap spacing between items/lines</item>
/// </list>
/// <para>Algorithm approach:</para>
/// <list type="number">
/// <item>Determine flex items and establish main/cross axes based on flex-direction</item>
/// <item>Determine flex basis (hypothetical main size) for each item</item>
/// <item>Collect flex items into flex lines based on flex-wrap</item>
/// <item>Resolve flexible lengths: distribute remaining space using flex-grow/shrink</item>
/// <item>Calculate cross-axis sizes (handle stretch alignment)</item>
/// <item>Align flex lines along cross axis (align-content)</item>
/// <item>Align items within each line (justify-content, align-items)</item>
/// </list>
/// </remarks>
public partial class FlexBox
{
    /// <inheritdoc/>
    protected override Size MeasureCore(Size availableBorderEdgeSize, IMeasureContext context)
    {
        if (Count == 0)
            return (0, 0);

        // Build flex structure
        var flexStructure = BuildFlexStructure(context, availableBorderEdgeSize);

        // Calculate total size
        var totalSize = CalculateTotalSize(flexStructure);

        return totalSize;
    }

    /// <inheritdoc/>
    protected override void ArrangeCore(Rect rect, IMeasureContext context)
    {
        if (Count == 0)
            return;

        // Build flex structure
        var flexStructure = BuildFlexStructure(context, rect.Size);

        // Position flex lines along cross axis
        var linePositions = PositionFlexLines(flexStructure, rect);

        // Arrange each item
        foreach (var line in flexStructure.Lines)
        {
            var lineRect = linePositions[line.LineIndex];
            ArrangeFlexLine(line, lineRect, context);
        }
    }

    private FlexStructure BuildFlexStructure(IMeasureContext context, Size availableSize)
    {
        var structure = new FlexStructure();
        structure.IsHorizontal = FlexDirection == Direction.Row || FlexDirection == Direction.RowReverse;
        structure.IsReverse = FlexDirection == Direction.RowReverse || FlexDirection == Direction.ColumnReverse;
        structure.IsWrap = FlexWrap != Wrap.NoWrap;
        structure.IsWrapReverse = FlexWrap == Wrap.WrapReverse;

        // Determine main and cross axis available sizes
        var mainAvailable = structure.IsHorizontal ? availableSize.Width : availableSize.Height;
        var crossAvailable = structure.IsHorizontal ? availableSize.Height : availableSize.Width;

        // Build flex items with hypothetical main sizes
        var items = BuildFlexItems(context, mainAvailable, crossAvailable, structure.IsHorizontal);

        // Collect items into flex lines
        structure.Lines = CollectFlexLines(items, mainAvailable, structure);

        return structure;
    }

    private List<FlexItem> BuildFlexItems(IMeasureContext context, nfloat mainAvailable, nfloat crossAvailable, bool isHorizontal)
    {
        var items = new List<FlexItem>();

        for (int i = 0; i < Count; i++)
        {
            var child = this[i];
            var item = new FlexItem
            {
                ChildIndex = i,
                Order = (int)child[Order],
                Grow = child[Grow],
                Shrink = child[Shrink],
                Basis = child[Basis],
            };

            // Measure child to determine hypothetical main size
            var availSize = new Size(
                nfloat.IsFinite(mainAvailable) && isHorizontal ? mainAvailable : nfloat.PositiveInfinity,
                nfloat.IsFinite(crossAvailable) && !isHorizontal ? crossAvailable : nfloat.PositiveInfinity
            );
            var childSize = child.Measure(availSize, context);

            // Determine flex basis (hypothetical main size)
            if (!nfloat.IsNaN(item.Basis) && item.Basis >= 0)
            {
                item.HypotheticalMainSize = item.Basis;
            }
            else
            {
                item.HypotheticalMainSize = isHorizontal ? childSize.Width : childSize.Height;
            }

            // Store cross size
            item.HypotheticalCrossSize = isHorizontal ? childSize.Height : childSize.Width;

            items.Add(item);
        }

        // Sort by order property (stable sort)
        items.Sort((a, b) => a.Order.CompareTo(b.Order));

        return items;
    }

    private List<FlexLine> CollectFlexLines(List<FlexItem> items, nfloat mainAvailable, FlexStructure structure)
    {
        var lines = new List<FlexLine>();

        if (!structure.IsWrap || !nfloat.IsFinite(mainAvailable))
        {
            // Single line: all items in one line
            var line = new FlexLine
            {
                LineIndex = 0,
                Items = items
            };
            ResolveFlexibleLengths(line, mainAvailable, structure.IsHorizontal);
            lines.Add(line);
        }
        else
        {
            // Multi-line: wrap items based on available space
            var currentLine = new FlexLine { LineIndex = 0, Items = new List<FlexItem>() };
            nfloat currentLineMainSize = 0;
            var gap = structure.IsHorizontal ? ColumnGap : RowGap;

            foreach (var item in items)
            {
                var itemMainSize = item.HypotheticalMainSize;
                var gapSize = currentLine.Items.Count > 0 ? gap : 0;

                // Check if item fits in current line
                if (currentLine.Items.Count > 0 && currentLineMainSize + gapSize + itemMainSize > mainAvailable)
                {
                    // Start new line
                    ResolveFlexibleLengths(currentLine, mainAvailable, structure.IsHorizontal);
                    lines.Add(currentLine);
                    currentLine = new FlexLine { LineIndex = lines.Count, Items = new List<FlexItem>() };
                    currentLineMainSize = 0;
                }

                currentLine.Items.Add(item);
                currentLineMainSize += itemMainSize + gapSize;
            }

            // Add final line
            if (currentLine.Items.Count > 0)
            {
                ResolveFlexibleLengths(currentLine, mainAvailable, structure.IsHorizontal);
                lines.Add(currentLine);
            }
        }

        // Calculate cross size for each line
        foreach (var line in lines)
        {
            line.CrossSize = CalculateLineCrossSize(line);
        }

        return lines;
    }

    private void ResolveFlexibleLengths(FlexLine line, nfloat mainAvailable, bool isHorizontal)
    {
        var gap = isHorizontal ? ColumnGap : RowGap;
        var gapTotal = (line.Items.Count - 1) * gap;

        // Calculate total hypothetical main size
        nfloat totalHypothetical = 0;
        foreach (var item in line.Items)
        {
            totalHypothetical += item.HypotheticalMainSize;
        }

        // Calculate remaining space
        var remainingSpace = nfloat.IsFinite(mainAvailable) ? mainAvailable - totalHypothetical - gapTotal : 0;
        line.RemainingSpace = remainingSpace;

        if (remainingSpace > 0)
        {
            // Distribute space to growing items
            nfloat totalGrow = 0;
            foreach (var item in line.Items)
            {
                if (item.Grow > 0)
                    totalGrow += item.Grow;
            }

            if (totalGrow > 0)
            {
                foreach (var item in line.Items)
                {
                    if (item.Grow > 0)
                    {
                        var extraSpace = (item.Grow / totalGrow) * remainingSpace;
                        item.MainSize = item.HypotheticalMainSize + extraSpace;
                    }
                    else
                    {
                        item.MainSize = item.HypotheticalMainSize;
                    }
                }
                line.RemainingSpace = 0;
            }
            else
            {
                // No flex-grow: items keep hypothetical size
                foreach (var item in line.Items)
                {
                    item.MainSize = item.HypotheticalMainSize;
                }
            }
        }
        else if (remainingSpace < 0)
        {
            // Shrink items to fit
            nfloat totalShrink = 0;
            nfloat totalScaledShrink = 0;
            foreach (var item in line.Items)
            {
                if (item.Shrink > 0)
                {
                    totalShrink += item.Shrink;
                    totalScaledShrink += item.Shrink * item.HypotheticalMainSize;
                }
            }

            if (totalScaledShrink > 0)
            {
                foreach (var item in line.Items)
                {
                    if (item.Shrink > 0)
                    {
                        var scaledShrink = item.Shrink * item.HypotheticalMainSize;
                        var shrinkAmount = (scaledShrink / totalScaledShrink) * (-remainingSpace);
                        item.MainSize = nfloat.Max(0, item.HypotheticalMainSize - shrinkAmount);
                    }
                    else
                    {
                        item.MainSize = item.HypotheticalMainSize;
                    }
                }
                line.RemainingSpace = 0;
            }
            else
            {
                // No flex-shrink: items keep hypothetical size (overflow)
                foreach (var item in line.Items)
                {
                    item.MainSize = item.HypotheticalMainSize;
                }
            }
        }
        else
        {
            // Exact fit
            foreach (var item in line.Items)
            {
                item.MainSize = item.HypotheticalMainSize;
            }
        }
    }

    private nfloat CalculateLineCrossSize(FlexLine line)
    {
        nfloat maxCrossSize = 0;
        foreach (var item in line.Items)
        {
            maxCrossSize = nfloat.Max(maxCrossSize, item.HypotheticalCrossSize);
        }
        return maxCrossSize;
    }

    private Size CalculateTotalSize(FlexStructure structure)
    {
        nfloat totalMainSize = 0;
        nfloat totalCrossSize = 0;

        var mainGap = structure.IsHorizontal ? ColumnGap : RowGap;
        var crossGap = structure.IsHorizontal ? RowGap : ColumnGap;

        foreach (var line in structure.Lines)
        {
            nfloat lineMainSize = 0;
            var itemCount = line.Items.Count;
            for (int i = 0; i < itemCount; i++)
            {
                lineMainSize += line.Items[i].MainSize;
                if (i < itemCount - 1)
                    lineMainSize += mainGap;
            }

            totalMainSize = nfloat.Max(totalMainSize, lineMainSize);
            totalCrossSize += line.CrossSize;
        }

        // Add gaps between lines
        if (structure.Lines.Count > 1)
            totalCrossSize += (structure.Lines.Count - 1) * crossGap;

        return structure.IsHorizontal
            ? new Size(totalMainSize, totalCrossSize)
            : new Size(totalCrossSize, totalMainSize);
    }

    private Dictionary<int, Rect> PositionFlexLines(FlexStructure structure, Rect containerRect)
    {
        var lineRects = new Dictionary<int, Rect>();
        var crossGap = structure.IsHorizontal ? RowGap : ColumnGap;

        // Calculate total cross size
        nfloat totalCrossSize = 0;
        foreach (var line in structure.Lines)
        {
            totalCrossSize += line.CrossSize;
        }
        if (structure.Lines.Count > 1)
            totalCrossSize += (structure.Lines.Count - 1) * crossGap;

        var containerCrossSize = structure.IsHorizontal ? containerRect.Height : containerRect.Width;
        var remainingCrossSpace = containerCrossSize - totalCrossSize;

        // Determine cross-axis positions based on align-content
        var crossPositions = CalculateAlignContentPositions(structure.Lines, remainingCrossSpace, crossGap);

        // Create rects for each line
        var crossStart = structure.IsHorizontal ? containerRect.Y : containerRect.X;
        for (int i = 0; i < structure.Lines.Count; i++)
        {
            var line = structure.Lines[i];
            var crossPos = crossStart + crossPositions[i];

            if (structure.IsHorizontal)
            {
                lineRects[i] = new Rect(containerRect.X, crossPos, containerRect.Width, line.CrossSize);
            }
            else
            {
                lineRects[i] = new Rect(crossPos, containerRect.Y, line.CrossSize, containerRect.Height);
            }
        }

        return lineRects;
    }

    private nfloat[] CalculateAlignContentPositions(List<FlexLine> lines, nfloat remainingSpace, nfloat gap)
    {
        var lineCount = lines.Count;
        var positions = new nfloat[lineCount];

        if (lineCount == 1 || remainingSpace <= 0)
        {
            // Single line or no space: simple positioning
            nfloat offset = 0;
            for (int i = 0; i < lineCount; i++)
            {
                positions[i] = offset;
                offset += lines[i].CrossSize;
                if (i < lineCount - 1)
                    offset += gap;
            }
            return positions;
        }

        // Multi-line with remaining space
        switch (FlexAlignContent)
        {
            case AlignContent.FlexStart:
            case AlignContent.Normal:
                nfloat offset = 0;
                for (int i = 0; i < lineCount; i++)
                {
                    positions[i] = offset;
                    offset += lines[i].CrossSize;
                    if (i < lineCount - 1)
                        offset += gap;
                }
                break;

            case AlignContent.FlexEnd:
                offset = remainingSpace;
                for (int i = 0; i < lineCount; i++)
                {
                    positions[i] = offset;
                    offset += lines[i].CrossSize;
                    if (i < lineCount - 1)
                        offset += gap;
                }
                break;

            case AlignContent.Center:
                offset = remainingSpace / 2;
                for (int i = 0; i < lineCount; i++)
                {
                    positions[i] = offset;
                    offset += lines[i].CrossSize;
                    if (i < lineCount - 1)
                        offset += gap;
                }
                break;

            case AlignContent.SpaceBetween:
                if (lineCount > 1)
                {
                    var spaceBetween = remainingSpace / (lineCount - 1);
                    offset = 0;
                    for (int i = 0; i < lineCount; i++)
                    {
                        positions[i] = offset;
                        offset += lines[i].CrossSize + spaceBetween;
                    }
                }
                else
                {
                    positions[0] = 0;
                }
                break;

            case AlignContent.SpaceAround:
                var spaceAround = remainingSpace / lineCount;
                offset = spaceAround / 2;
                for (int i = 0; i < lineCount; i++)
                {
                    positions[i] = offset;
                    offset += lines[i].CrossSize + spaceAround;
                }
                break;

            case AlignContent.SpaceEvenly:
                var spaceEvenly = remainingSpace / (lineCount + 1);
                offset = spaceEvenly;
                for (int i = 0; i < lineCount; i++)
                {
                    positions[i] = offset;
                    offset += lines[i].CrossSize + spaceEvenly;
                }
                break;

            case AlignContent.Stretch:
                // Distribute extra space to each line
                var extraPerLine = remainingSpace / lineCount;
                offset = 0;
                for (int i = 0; i < lineCount; i++)
                {
                    positions[i] = offset;
                    // For stretch, the line height/width includes the extra space
                    offset += lines[i].CrossSize + extraPerLine;
                    if (i < lineCount - 1)
                        offset += gap;
                }
                break;
        }

        return positions;
    }

    private void ArrangeFlexLine(FlexLine line, Rect lineRect, IMeasureContext context)
    {
        var isHorizontal = FlexDirection == Direction.Row || FlexDirection == Direction.RowReverse;
        var isReverse = FlexDirection == Direction.RowReverse || FlexDirection == Direction.ColumnReverse;
        var mainGap = isHorizontal ? ColumnGap : RowGap;

        // Calculate total main size of items
        nfloat totalItemsMainSize = 0;
        foreach (var item in line.Items)
        {
            totalItemsMainSize += item.MainSize;
        }
        var totalGaps = (line.Items.Count - 1) * mainGap;
        var lineMainSize = isHorizontal ? lineRect.Width : lineRect.Height;
        var remainingMainSpace = lineMainSize - totalItemsMainSize - totalGaps;

        // Calculate main-axis start positions based on justify-content
        var mainPositions = CalculateJustifyContentPositions(line.Items, remainingMainSpace, mainGap);

        // Arrange each item
        for (int i = 0; i < line.Items.Count; i++)
        {
            var item = line.Items[i];
            var child = this[item.ChildIndex];

            // Determine main-axis position and size
            var mainPos = mainPositions[i];
            var mainSize = item.MainSize;

            // Determine cross-axis size (handle stretch)
            var lineCrossSize = isHorizontal ? lineRect.Height : lineRect.Width;
            var crossSize = item.HypotheticalCrossSize;
            
            // For stretch, use the line cross size as available
            var crossAvailable = FlexAlignItems == AlignItems.Stretch ? lineCrossSize : item.HypotheticalCrossSize;

            // Re-measure child with flex-determined sizes to get actual content size
            var childAvailSize = isHorizontal
                ? new Size(mainSize, crossAvailable)
                : new Size(crossAvailable, mainSize);
            var childMeasured = child.Measure(childAvailSize, context);

            // Use flex-determined main size, measured cross size (unless stretching)
            var finalMainSize = mainSize;
            var finalCrossSize = FlexAlignItems == AlignItems.Stretch ? lineCrossSize : 
                                 (isHorizontal ? childMeasured.Height : childMeasured.Width);

            // Apply align-items for cross-axis positioning
            var crossPos = CalculateItemCrossPosition(lineCrossSize, finalCrossSize);

            // Create final rect using flex-determined sizes
            Rect itemRect;
            if (isHorizontal)
            {
                var x = lineRect.X + (isReverse ? lineRect.Width - mainPos - finalMainSize : mainPos);
                var y = lineRect.Y + crossPos;
                itemRect = new Rect(x, y, finalMainSize, finalCrossSize);
            }
            else
            {
                var x = lineRect.X + crossPos;
                var y = lineRect.Y + (isReverse ? lineRect.Height - mainPos - finalMainSize : mainPos);
                itemRect = new Rect(x, y, finalCrossSize, finalMainSize);
            }

            child.Arrange(itemRect, context);
        }
    }

    private nfloat[] CalculateJustifyContentPositions(List<FlexItem> items, nfloat remainingSpace, nfloat gap)
    {
        var itemCount = items.Count;
        var positions = new nfloat[itemCount];

        switch (FlexJustifyContent)
        {
            case JustifyContent.FlexStart:
                nfloat offset = 0;
                for (int i = 0; i < itemCount; i++)
                {
                    positions[i] = offset;
                    offset += items[i].MainSize + gap;
                }
                break;

            case JustifyContent.FlexEnd:
                offset = remainingSpace;
                for (int i = 0; i < itemCount; i++)
                {
                    positions[i] = offset;
                    offset += items[i].MainSize + gap;
                }
                break;

            case JustifyContent.Center:
                offset = remainingSpace / 2;
                for (int i = 0; i < itemCount; i++)
                {
                    positions[i] = offset;
                    offset += items[i].MainSize + gap;
                }
                break;

            case JustifyContent.SpaceBetween:
                if (itemCount > 1)
                {
                    var spaceBetween = remainingSpace / (itemCount - 1);
                    offset = 0;
                    for (int i = 0; i < itemCount; i++)
                    {
                        positions[i] = offset;
                        offset += items[i].MainSize + spaceBetween;
                    }
                }
                else
                {
                    positions[0] = 0;
                }
                break;

            case JustifyContent.SpaceAround:
                var spaceAround = remainingSpace / itemCount;
                offset = spaceAround / 2;
                for (int i = 0; i < itemCount; i++)
                {
                    positions[i] = offset;
                    offset += items[i].MainSize + spaceAround;
                }
                break;

            case JustifyContent.SpaceEvenly:
                var spaceEvenly = remainingSpace / (itemCount + 1);
                offset = spaceEvenly;
                for (int i = 0; i < itemCount; i++)
                {
                    positions[i] = offset;
                    offset += items[i].MainSize + spaceEvenly;
                }
                break;
        }

        return positions;
    }

    private nfloat CalculateItemCrossPosition(nfloat lineCrossSize, nfloat itemCrossSize)
    {
        return FlexAlignItems switch
        {
            AlignItems.FlexStart => 0,
            AlignItems.FlexEnd => lineCrossSize - itemCrossSize,
            AlignItems.Center => (lineCrossSize - itemCrossSize) / 2,
            AlignItems.Stretch => 0,
            AlignItems.Baseline => 0, // Simplified: treat as flex-start
            AlignItems.FirstBaseline => 0,
            AlignItems.LastBaseline => 0,
            _ => 0
        };
    }

    private struct FlexStructure
    {
        public bool IsHorizontal;
        public bool IsReverse;
        public bool IsWrap;
        public bool IsWrapReverse;
        public List<FlexLine> Lines;
    }

    private class FlexLine
    {
        public int LineIndex;
        public List<FlexItem> Items = new();
        public nfloat CrossSize;
        public nfloat RemainingSpace;
    }

    private class FlexItem
    {
        public int ChildIndex;
        public int Order;
        public nfloat Grow;
        public nfloat Shrink;
        public nfloat Basis;
        public nfloat HypotheticalMainSize;
        public nfloat HypotheticalCrossSize;
        public nfloat MainSize;
    }
}
