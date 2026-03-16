using Xui.Core.Math2D;
using Xui.Core.Canvas;

namespace Xui.Core.UI.Layout;

public partial class Grid
{
    /// <inheritdoc/>
    protected override Size MeasureCore(Size availableBorderEdgeSize, IMeasureContext context)
    {
        if (Count == 0)
            return (0, 0);

        // Build the grid structure
        var gridStructure = BuildGridStructure(context);

        // Measure all tracks
        var columnSizes = MeasureTracks(gridStructure.ColumnTracks, gridStructure, true, availableBorderEdgeSize.Width, context);
        var rowSizes = MeasureTracks(gridStructure.RowTracks, gridStructure, false, availableBorderEdgeSize.Height, context);

        // Calculate total size including gaps
        var totalWidth = SumTrackSizes(columnSizes) + (columnSizes.Length - 1) * ColumnGap;
        var totalHeight = SumTrackSizes(rowSizes) + (rowSizes.Length - 1) * RowGap;

        return new Size(totalWidth, totalHeight);
    }

    /// <inheritdoc/>
    protected override void ArrangeCore(Rect rect, IMeasureContext context)
    {
        if (Count == 0)
            return;

        // Build the grid structure
        var gridStructure = BuildGridStructure(context);

        // Measure all tracks
        var columnSizes = MeasureTracks(gridStructure.ColumnTracks, gridStructure, true, rect.Width, context);
        var rowSizes = MeasureTracks(gridStructure.RowTracks, gridStructure, false, rect.Height, context);

        // Calculate track positions
        var columnPositions = CalculateTrackPositions(columnSizes, ColumnGap);
        var rowPositions = CalculateTrackPositions(rowSizes, RowGap);

        // Apply grid alignment (justify-content and align-content)
        var totalWidth = SumTrackSizes(columnSizes) + (columnSizes.Length - 1) * ColumnGap;
        var totalHeight = SumTrackSizes(rowSizes) + (rowSizes.Length - 1) * RowGap;
        var (offsetX, offsetY) = CalculateContentAlignment(rect, totalWidth, totalHeight);

        // Arrange each child in its grid area
        foreach (var placement in gridStructure.Placements)
        {
            var child = this[placement.ChildIndex];

            // Calculate the grid area bounds
            var x = rect.X + offsetX + columnPositions[placement.ColumnStart];
            var y = rect.Y + offsetY + rowPositions[placement.RowStart];
            var width = columnPositions[placement.ColumnEnd] - columnPositions[placement.ColumnStart] - ColumnGap;
            var height = rowPositions[placement.RowEnd] - rowPositions[placement.RowStart] - RowGap;

            // Measure the child
            var childDesiredSize = child.Measure(new Size(width, height), context);

            // Apply item alignment (justify-items and align-items)
            var childRect = CalculateItemAlignment(
                new Rect(x, y, width, height),
                childDesiredSize,
                GridJustifyItems,
                GridAlignItems
            );

            child.Arrange(childRect, context);
        }
    }

    private GridStructure BuildGridStructure(IMeasureContext context)
    {
        var structure = new GridStructure();

        // Determine explicit grid size
        var explicitColumns = TemplateColumns.Length > 0 ? TemplateColumns.Length : 0;
        var explicitRows = TemplateRows.Length > 0 ? TemplateRows.Length : 0;

        // Parse template areas if provided
        var areaMap = ParseTemplateAreas();

        // Process each child to determine placement
        var placements = new List<GridPlacement>();
        int implicitColumnEnd = explicitColumns;
        int implicitRowEnd = explicitRows;

        for (int i = 0; i < Count; i++)
        {
            var child = this[i];
            var placement = DeterminePlacement(child, i, areaMap, ref implicitColumnEnd, ref implicitRowEnd);
            placements.Add(placement);
        }

        // Build track lists
        structure.ColumnTracks = BuildTrackList(TemplateColumns, AutoColumns, implicitColumnEnd);
        structure.RowTracks = BuildTrackList(TemplateRows, AutoRows, implicitRowEnd);
        structure.Placements = placements;

        return structure;
    }

    private TrackSize[] BuildTrackList(TrackSize[] template, TrackSize autoSize, int totalCount)
    {
        var tracks = new TrackSize[totalCount];
        for (int i = 0; i < totalCount; i++)
        {
            tracks[i] = i < template.Length ? template[i] : autoSize;
        }
        return tracks;
    }

    private Dictionary<string, GridArea> ParseTemplateAreas()
    {
        var areaMap = new Dictionary<string, GridArea>();
        if (TemplateAreas.Length == 0)
            return areaMap;

        for (int row = 0; row < TemplateAreas.Length; row++)
        {
            var cells = TemplateAreas[row].Split(' ', StringSplitOptions.RemoveEmptyEntries);
            for (int col = 0; col < cells.Length; col++)
            {
                var areaName = cells[col];
                if (areaName == ".")
                    continue;

                if (!areaMap.TryGetValue(areaName, out var area))
                {
                    area = new GridArea { RowStart = row, ColumnStart = col, RowEnd = row + 1, ColumnEnd = col + 1 };
                    areaMap[areaName] = area;
                }
                else
                {
                    // Extend the area
                    area.RowEnd = Math.Max(area.RowEnd, row + 1);
                    area.ColumnEnd = Math.Max(area.ColumnEnd, col + 1);
                    areaMap[areaName] = area;
                }
            }
        }

        return areaMap;
    }

    private GridPlacement DeterminePlacement(View child, int childIndex, Dictionary<string, GridArea> areaMap, ref int implicitColumnEnd, ref int implicitRowEnd)
    {
        // Check for named area first
        var areaName = child[Area];
        if (areaName != null && areaMap.TryGetValue(areaName, out var area))
        {
            implicitColumnEnd = Math.Max(implicitColumnEnd, area.ColumnEnd);
            implicitRowEnd = Math.Max(implicitRowEnd, area.RowEnd);
            return new GridPlacement
            {
                ChildIndex = childIndex,
                RowStart = area.RowStart,
                RowEnd = area.RowEnd,
                ColumnStart = area.ColumnStart,
                ColumnEnd = area.ColumnEnd
            };
        }

        // Check for explicit positioning (1-indexed in CSS, convert to 0-indexed)
        int rowStart = child[RowStart] > 0 ? (int)child[RowStart] - 1 : -1;
        int rowEnd = child[RowEnd] > 0 ? (int)child[RowEnd] - 1 : -1;
        int columnStart = child[ColumnStart] > 0 ? (int)child[ColumnStart] - 1 : -1;
        int columnEnd = child[ColumnEnd] > 0 ? (int)child[ColumnEnd] - 1 : -1;

        // Handle row span
        var rowSpanValue = child[RowSpan];
        if (rowStart >= 0 && rowEnd < 0 && rowSpanValue > 1)
            rowEnd = rowStart + (int)rowSpanValue;
        
        // Handle column span
        var columnSpanValue = child[ColumnSpan];
        if (columnStart >= 0 && columnEnd < 0 && columnSpanValue > 1)
            columnEnd = columnStart + (int)columnSpanValue;

        // Default to span 1 if only start is specified
        if (rowStart >= 0 && rowEnd < 0)
            rowEnd = rowStart + 1;
        if (columnStart >= 0 && columnEnd < 0)
            columnEnd = columnStart + 1;

        // Auto-placement if not fully specified
        if (rowStart < 0 || columnStart < 0)
        {
            var (autoRow, autoCol) = AutoPlaceItem(childIndex, rowStart, columnStart, rowEnd - rowStart, columnEnd - columnStart);
            if (rowStart < 0)
            {
                rowStart = autoRow;
                rowEnd = rowStart + (rowEnd < 0 ? 1 : rowEnd - rowStart);
            }
            if (columnStart < 0)
            {
                columnStart = autoCol;
                columnEnd = columnStart + (columnEnd < 0 ? 1 : columnEnd - columnStart);
            }
        }

        implicitColumnEnd = Math.Max(implicitColumnEnd, columnEnd);
        implicitRowEnd = Math.Max(implicitRowEnd, rowEnd);

        return new GridPlacement
        {
            ChildIndex = childIndex,
            RowStart = rowStart,
            RowEnd = rowEnd,
            ColumnStart = columnStart,
            ColumnEnd = columnEnd
        };
    }

    private (int row, int column) AutoPlaceItem(int itemIndex, int fixedRow, int fixedColumn, int rowSpan, int columnSpan)
    {
        // Simple auto-placement based on grid-auto-flow
        if (GridAutoFlow == AutoFlow.Column)
        {
            // Place in columns first
            int col = itemIndex;
            int row = 0;
            return (row, col);
        }
        else
        {
            // Place in rows first (default)
            int explicitColumns = TemplateColumns.Length > 0 ? TemplateColumns.Length : 1;
            int row = itemIndex / explicitColumns;
            int col = itemIndex % explicitColumns;
            return (row, col);
        }
    }

    private nfloat[] MeasureTracks(TrackSize[] tracks, GridStructure structure, bool isColumn, nfloat availableSize, IMeasureContext context)
    {
        var sizes = new nfloat[tracks.Length];
        var frTracks = new List<int>();
        nfloat usedSpace = 0;
        nfloat totalFr = 0;

        // First pass: measure fixed and content-based tracks
        for (int i = 0; i < tracks.Length; i++)
        {
            var track = tracks[i];
            switch (track.Kind)
            {
                case TrackSizeKind.Length:
                    sizes[i] = track.Value;
                    usedSpace += track.Value;
                    break;

                case TrackSizeKind.Auto:
                case TrackSizeKind.MinContent:
                case TrackSizeKind.MaxContent:
                    sizes[i] = MeasureContentTrack(structure, i, isColumn, track.Kind, context);
                    usedSpace += sizes[i];
                    break;

                case TrackSizeKind.Fr:
                    frTracks.Add(i);
                    totalFr += track.Value;
                    break;

                case TrackSizeKind.MinMax:
                    sizes[i] = MeasureMinMaxTrack(structure, i, isColumn, track.MinValue, track.MaxValue, context);
                    usedSpace += sizes[i];
                    break;

                case TrackSizeKind.FitContent:
                    var contentSize = MeasureContentTrack(structure, i, isColumn, TrackSizeKind.MaxContent, context);
                    sizes[i] = nfloat.Min(contentSize, track.MaxValue);
                    usedSpace += sizes[i];
                    break;
            }
        }

        // Second pass: distribute remaining space to fr tracks
        if (frTracks.Count > 0 && totalFr > 0)
        {
            var gapSpace = (tracks.Length - 1) * (isColumn ? ColumnGap : RowGap);
            var remainingSpace = nfloat.Max(0, availableSize - usedSpace - gapSpace);
            var frUnit = remainingSpace / totalFr;

            foreach (var trackIndex in frTracks)
            {
                sizes[trackIndex] = tracks[trackIndex].Value * frUnit;
            }
        }

        return sizes;
    }

    private nfloat MeasureContentTrack(GridStructure structure, int trackIndex, bool isColumn, TrackSizeKind kind, IMeasureContext context)
    {
        nfloat maxSize = 0;

        foreach (var placement in structure.Placements)
        {
            // Check if this item is in this track
            bool inTrack = isColumn
                ? (placement.ColumnStart <= trackIndex && trackIndex < placement.ColumnEnd)
                : (placement.RowStart <= trackIndex && trackIndex < placement.RowEnd);

            if (!inTrack)
                continue;

            // Only measure items that occupy exactly one track (no spanning)
            int span = isColumn ? (placement.ColumnEnd - placement.ColumnStart) : (placement.RowEnd - placement.RowStart);
            if (span != 1)
                continue;

            var child = this[placement.ChildIndex];
            var availableSize = new Size(nfloat.PositiveInfinity, nfloat.PositiveInfinity);
            var childSize = child.Measure(availableSize, context);

            var size = isColumn ? childSize.Width : childSize.Height;
            maxSize = nfloat.Max(maxSize, size);
        }

        return maxSize;
    }

    private nfloat MeasureMinMaxTrack(GridStructure structure, int trackIndex, bool isColumn, nfloat min, nfloat max, IMeasureContext context)
    {
        var contentSize = MeasureContentTrack(structure, trackIndex, isColumn, TrackSizeKind.MaxContent, context);
        return nfloat.Max(min, nfloat.Min(contentSize, max));
    }

    private nfloat SumTrackSizes(nfloat[] sizes)
    {
        nfloat total = 0;
        foreach (var size in sizes)
            total += size;
        return total;
    }

    private nfloat[] CalculateTrackPositions(nfloat[] sizes, nfloat gap)
    {
        var positions = new nfloat[sizes.Length + 1];
        positions[0] = 0;

        for (int i = 0; i < sizes.Length; i++)
        {
            positions[i + 1] = positions[i] + sizes[i] + gap;
        }

        return positions;
    }

    private (nfloat offsetX, nfloat offsetY) CalculateContentAlignment(Rect rect, nfloat totalWidth, nfloat totalHeight)
    {
        nfloat offsetX = GridJustifyContent switch
        {
            JustifyContent.Start => 0,
            JustifyContent.End => rect.Width - totalWidth,
            JustifyContent.Center => (rect.Width - totalWidth) / 2,
            JustifyContent.SpaceBetween => 0,
            JustifyContent.SpaceAround => 0,
            JustifyContent.SpaceEvenly => 0,
            _ => 0
        };

        nfloat offsetY = GridAlignContent switch
        {
            AlignContent.Start => 0,
            AlignContent.End => rect.Height - totalHeight,
            AlignContent.Center => (rect.Height - totalHeight) / 2,
            AlignContent.SpaceBetween => 0,
            AlignContent.SpaceAround => 0,
            AlignContent.SpaceEvenly => 0,
            _ => 0
        };

        return (offsetX, offsetY);
    }

    private Rect CalculateItemAlignment(Rect cellRect, Size childDesiredSize, JustifyItems justifyItems, AlignItems alignItems)
    {
        var x = cellRect.X;
        var y = cellRect.Y;
        var width = childDesiredSize.Width;
        var height = childDesiredSize.Height;

        // Horizontal alignment (justify-items)
        switch (justifyItems)
        {
            case JustifyItems.Start:
                // Already at start
                break;
            case JustifyItems.End:
                x = cellRect.X + cellRect.Width - width;
                break;
            case JustifyItems.Center:
                x = cellRect.X + (cellRect.Width - width) / 2;
                break;
            case JustifyItems.Stretch:
                width = cellRect.Width;
                break;
        }

        // Vertical alignment (align-items)
        switch (alignItems)
        {
            case AlignItems.Start:
                // Already at start
                break;
            case AlignItems.End:
                y = cellRect.Y + cellRect.Height - height;
                break;
            case AlignItems.Center:
                y = cellRect.Y + (cellRect.Height - height) / 2;
                break;
            case AlignItems.Stretch:
                height = cellRect.Height;
                break;
        }

        return new Rect(x, y, width, height);
    }

    private struct GridStructure
    {
        public TrackSize[] ColumnTracks;
        public TrackSize[] RowTracks;
        public List<GridPlacement> Placements;
    }

    private struct GridPlacement
    {
        public int ChildIndex;
        public int RowStart;
        public int RowEnd;
        public int ColumnStart;
        public int ColumnEnd;
    }

    private struct GridArea
    {
        public int RowStart;
        public int RowEnd;
        public int ColumnStart;
        public int ColumnEnd;
    }
}
