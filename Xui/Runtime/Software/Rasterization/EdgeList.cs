using System.Collections.Generic;

namespace Xui.Runtime.Software.Rasterization;

public sealed class EdgeList
{
    private readonly List<Edge> _edges = new();

    public void Add(Edge edge)
    {
        // Avoid horizontal edges initially
        if (edge.Y0 != edge.Y1) 
        {
            _edges.Add(edge);
        }
    }

    public void Clear() => _edges.Clear();

    public void SortByY()
    {
        _edges.Sort((a, b) => a.Y0.CompareTo(b.Y0));
    }

    public IReadOnlyList<Edge> Edges => _edges;
}
