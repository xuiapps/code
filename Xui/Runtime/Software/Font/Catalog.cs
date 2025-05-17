using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Xui.Core.Canvas;

namespace Xui.Runtime.Software.Font;

public class Catalog
{
    private readonly IReadOnlyList<Uri> fontUris;

    private readonly Dictionary<FontFace, TrueTypeFont?> faceToFont = new (32, FontFace.Comparer);

    private readonly Dictionary<Uri, FontFace?> uriToFace = new();

    public Catalog(params IEnumerable<Uri>[] sources)
    {
        var list = new List<Uri>();
        foreach (var source in sources)
        {
            list.AddRange(source);
        }

        fontUris = list.AsReadOnly();
    }

    public virtual IEnumerable<Uri> Fonts() => this.fontUris;

    public virtual ReadOnlyMemory<byte> LoadFromUri(Uri uri)
    {
        return uri.Scheme switch
        {
            "embedded" => LoadEmbedded(uri),
            "file"     => File.ReadAllBytes(uri.LocalPath),
            _          => throw new NotSupportedException($"Unsupported font URI scheme: {uri.Scheme}")
        };
    }

    /// <summary>
    /// Loads embedded font data from a URI in the format: embedded://Assembly.Name/Folder/File.ttf
    /// </summary>
    /// <param name="uri">The embedded URI, e.g. embedded://Xui.Core.Fonts/Inter/Inter-Bold.ttf</param>
    /// <returns>A byte array containing the font file contents.</returns>
    /// <exception cref="InvalidOperationException">Thrown when the resource or assembly cannot be found.</exception>
    public static ReadOnlyMemory<byte> LoadEmbedded(Uri uri)
    {
        var assemblyName = uri.Authority;
        var resourcePath = uri.AbsolutePath.TrimStart('/').Replace('/', '.');

        var assemblies = AppDomain.CurrentDomain.GetAssemblies();
        var assembly = assemblies.FirstOrDefault(a => string.Compare(a.GetName().Name, assemblyName, StringComparison.InvariantCultureIgnoreCase) == 0);

        if (assembly == null)
            throw new InvalidOperationException($"Assembly '{assemblyName}' not found in AppDomain.");

        var fullResourceName = $"{assembly.GetName().Name}.{resourcePath}";
        using var stream = assembly.GetManifestResourceStream(fullResourceName)
            ?? throw new InvalidOperationException($"Embedded resource '{fullResourceName}' not found in assembly '{assemblyName}'.");

        using var ms = new MemoryStream();
        stream.CopyTo(ms);
        // safe as span; can optimize later with array pool if needed
        return ms.ToArray();
    }

    public IEnumerable<Uri> AStar(FontFace font)
    {
        var queue = new List<(Uri uri, int score)>();

        foreach (var uri in Fonts())
        {
            var score = Score(uri, font);
            queue.Add((uri, score));
        }

        queue.Sort((a, b) => a.score.CompareTo(b.score));

        foreach (var (uri, _) in queue)
            yield return uri;
    }

    private static int Score(Uri uri, FontFace font)
    {
        string file = System.IO.Path.GetFileNameWithoutExtension(uri.AbsoluteUri).ToLowerInvariant();
        string family = font.Family.Length > 0 ? font.Family.ToLowerInvariant() : "";

        int score = 0;

        // Family match is most important
        if (file.Contains(family))
            score -= 100;

        // Prefer regular for normal weights
        if (font.Weight == FontWeight.Normal)
        {
            if (file.Contains("regular"))
                score -= 20;
            else if (file.Contains("bold"))
                score += 10;
        }
        else if (font.Weight >= 600)
        {
            if (file.Contains("bold"))
                score -= 10;
            else if (file.Contains("regular"))
                score += 5;
        }

        // Style
        if (font.Style.IsItalic && file.Contains("italic"))
            score -= 10;
        else if (font.Style.IsOblique && file.Contains("oblique"))
            score -= 8;

        // Stretch
        if (font.Stretch < FontStretch.Normal && file.Contains("condensed"))
            score -= 5;
        else if (font.Stretch > FontStretch.Normal && file.Contains("expanded"))
            score -= 5;

        return score;
    }

    public TextMetrics MeasureText(in Core.Canvas.Font font, string text)
    {
        var ttf = FontForFace(ToFace(in font));
        if (ttf is not null)
        {
            return ttf.MeasureText(text, font);
        }

        return default;
    }

    public TrueTypeFont? FontForFace(in FontFace face)
    {
        if (faceToFont.TryGetValue(face, out var cachedTtf))
        {
            return cachedTtf;
        }
        
        // Go through font URIs sorted by how likely they are to represent this font
        foreach(var uri in AStar(face))
        {
            FontFace? uriFace;
            if (!uriToFace.TryGetValue(uri, out uriFace))
            {
                var data = this.LoadFromUri(uri);
                uriFace = TrueTypeFont.FontFace(data.Span);
                uriToFace[uri] = uriFace;
            }

            if (!uriFace.HasValue)
            {
                continue;
            }

            if (FontFace.Comparer.Equals(uriFace.Value, face))
            {
                var data = this.LoadFromUri(uri);
                var ttf = new TrueTypeFont(data, uri);
                faceToFont[uriFace.Value] = ttf;
                return ttf;
            }
        }

        return null;
    }

    private static FontFace ToFace(in Xui.Core.Canvas.Font font)
    {
        // TODO: Add mechanism to specify the default font for the system.

        var family = font.FontFamily.Length > 0
            ? font.FontFamily[0]
            : "Default";

        return new FontFace(
            family,
            font.FontWeight,
            font.FontStyle,
            font.FontStretch
        );
    }
}
