using Xui.Runtime.Software.Font;

namespace GlyphLoader
{
    /// <summary>
    /// Test drive for the Software platform's PngEncoder...
    /// </summary>
    internal class Program
    {
        static void Main()
        {
            // var lister = new TtfGlyphLister();
            // lister.ListGlyphsAndPaths("FreebrushScriptPLng.ttf");

            // using var stream = File.OpenRead("/Users/cankov/git/xuiapps/code/tools/GlyphLoader/FreebrushScriptPLng.ttf");
            // var font = new TrueTypeFont(stream);
            // FontLister.ListFont(font);

            var blob = File.ReadAllBytes("/Users/cankov/git/xuiapps/code/tools/GlyphLoader/Inter-Regular.ttf");
            var font = new TrueTypeFont(blob);

            // FontLister.ListFont(font);
            // FontPreview.WriteSVG(font, "H", "font-preview-H.svg");
            // FontPreview.WriteSVG(font, "J", "font-preview-J.svg");
            // FontPreview.WriteSVG(font, "K", "font-preview-K.svg");
            // FontPreview.WriteSVG(font, "L", "font-preview-L.svg");
            // FontPreview.WriteSVG(font, "M", "font-preview-M.svg");
            // FontPreview.WriteSVG(font, "N", "font-preview-N.svg");

            // FontPreview.WriteSVG(font, "Hello World!", "font-preview-Hello-World.svg");
            FontPreview.WriteSVG(font, "AVToVATaYoTeWaLYFAAT", "kern.svg");

            // FontDebugLogger.DumpGPosDebug(font);
        }
    }
}
