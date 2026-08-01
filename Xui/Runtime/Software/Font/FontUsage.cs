using System;
using System.Collections.Generic;
using System.Text;

namespace Xui.Runtime.Software.Font;

/// <summary>
/// The characters and adjacent character pairs rendered with one font face.
/// </summary>
/// <remarks>
/// This deliberately records source characters rather than only glyph IDs. A compact font needs
/// a cmap for the SVG text, while pairs allow the subsetter to retain the kerning actually used.
/// Complex OpenType shaping is outside the first subset format and will add richer usage records.
/// </remarks>
public sealed class FontUsage
{
    private readonly HashSet<Rune> characters = [];
    private readonly HashSet<RunePair> pairs = [];

    public IReadOnlySet<Rune> Characters => characters;
    public IReadOnlySet<RunePair> Pairs => pairs;

    public void Add(string text)
    {
        ArgumentNullException.ThrowIfNull(text);

        Rune? previous = null;
        foreach (var character in text.EnumerateRunes())
        {
            characters.Add(character);
            if (previous is Rune left)
                pairs.Add(new RunePair(left, character));
            previous = character;
        }
    }

    public readonly record struct RunePair(Rune Left, Rune Right);
}
