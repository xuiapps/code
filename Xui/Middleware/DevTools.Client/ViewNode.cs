using System.Globalization;
using System.Text;
using System.Text.Json.Serialization;

namespace Xui.Middleware.DevTools.Client;

/// <summary>
/// Represents a node in the Xui view hierarchy as returned by <c>ui.inspect</c>.
/// </summary>
public record ViewNode(
    string Type,
    float X, float Y, float W, float H,
    float CenterX, float CenterY,
    bool Visible,
    string? Id,
    string? ClassName,
    ViewNode[] Children)
{
    /// <summary>
    /// Finds the first node (self or descendant) whose <see cref="Id"/> matches.
    /// Returns <c>null</c> when not found.
    /// </summary>
    public ViewNode? FindById(string id)
    {
        if (Id == id) return this;
        foreach (var child in Children)
        {
            var found = child.FindById(id);
            if (found != null) return found;
        }
        return null;
    }

    /// <summary>Finds all nodes (self and descendants) whose <see cref="Type"/> matches.</summary>
    public IEnumerable<ViewNode> FindAllByType(string typeName)
    {
        if (Type == typeName) yield return this;
        foreach (var child in Children)
            foreach (var node in child.FindAllByType(typeName))
                yield return node;
    }

    /// <summary>Returns an XML representation of this view subtree for diagnostic logging.</summary>
    public string ToXml(int indent = 0)
    {
        var sb = new StringBuilder();
        WriteXml(sb, indent);
        return sb.ToString();
    }

    private void WriteXml(StringBuilder sb, int indent)
    {
        var pad = new string(' ', indent * 2);
        sb.Append(pad).Append('<').Append(Type);
        if (Id != null) sb.Append($" id=\"{Id}\"");
        if (ClassName != null) sb.Append($" class=\"{ClassName}\"");
        sb.Append($" x=\"{X.ToString("F2", CultureInfo.InvariantCulture)}\"");
        sb.Append($" y=\"{Y.ToString("F2", CultureInfo.InvariantCulture)}\"");
        sb.Append($" w=\"{W.ToString("F2", CultureInfo.InvariantCulture)}\"");
        sb.Append($" h=\"{H.ToString("F2", CultureInfo.InvariantCulture)}\"");
        sb.Append($" centerX=\"{CenterX.ToString("F2", CultureInfo.InvariantCulture)}\"");
        sb.Append($" centerY=\"{CenterY.ToString("F2", CultureInfo.InvariantCulture)}\"");
        sb.Append($" visible=\"{Visible.ToString().ToLowerInvariant()}\"");
        if (Children.Length == 0)
        {
            sb.AppendLine(" />");
        }
        else
        {
            sb.AppendLine(">");
            foreach (var child in Children)
                child.WriteXml(sb, indent + 1);
            sb.Append(pad).Append("</").Append(Type).AppendLine(">");
        }
    }
}

/// <summary>Deserialization wrapper for the <c>ui.inspect</c> response.</summary>
public record InspectResult(ViewNode Root);
