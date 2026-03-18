using System;
using System.Runtime.InteropServices;

namespace Xui.GPU.Software;

/// <summary>
/// Represents a source of vertex data for rendering.
/// </summary>
/// <typeparam name="T">The vertex structure type.</typeparam>
/// <remarks>
/// VertexSource provides a view over a contiguous array of vertex data.
/// The vertex type T must be a value type with sequential layout.
/// </remarks>
public unsafe struct VertexSource<T> where T : unmanaged
{
    private readonly T* _data;
    private readonly int _count;

    /// <summary>
    /// Gets the number of vertices in this source.
    /// </summary>
    public readonly int Count => _count;

    /// <summary>
    /// Gets a pointer to the vertex data.
    /// </summary>
    public readonly T* Data => _data;

    /// <summary>
    /// Creates a vertex source from a managed array.
    /// </summary>
    /// <param name="vertices">Array of vertex data.</param>
    /// <returns>A pinned vertex source.</returns>
    /// <remarks>
    /// The returned VertexSource does not own the memory.
    /// The caller must ensure the array remains pinned during use.
    /// </remarks>
    public static VertexSource<T> FromArray(T[] vertices)
    {
        if (vertices == null || vertices.Length == 0)
            throw new ArgumentException("Vertex array cannot be null or empty", nameof(vertices));

        fixed (T* ptr = vertices)
        {
            return new VertexSource<T>(ptr, vertices.Length);
        }
    }

    /// <summary>
    /// Creates a vertex source from a memory pointer.
    /// </summary>
    /// <param name="data">Pointer to vertex data.</param>
    /// <param name="count">Number of vertices.</param>
    public VertexSource(T* data, int count)
    {
        if (data == null)
            throw new ArgumentNullException(nameof(data));
        if (count <= 0)
            throw new ArgumentOutOfRangeException(nameof(count), "Count must be greater than 0");

        _data = data;
        _count = count;
    }

    /// <summary>
    /// Gets the vertex at the specified index.
    /// </summary>
    /// <param name="index">Zero-based vertex index.</param>
    /// <returns>The vertex at the specified index.</returns>
    /// <exception cref="IndexOutOfRangeException">
    /// Thrown when index is out of bounds.
    /// </exception>
    public readonly T this[int index]
    {
        get
        {
            if (index < 0 || index >= _count)
                throw new IndexOutOfRangeException($"Index {index} is out of range [0, {_count})");
            return _data[index];
        }
    }

    /// <summary>
    /// Gets a read-only span over the vertex data.
    /// </summary>
    /// <returns>A read-only span of vertices.</returns>
    public readonly ReadOnlySpan<T> AsSpan()
    {
        return new ReadOnlySpan<T>(_data, _count);
    }
}
