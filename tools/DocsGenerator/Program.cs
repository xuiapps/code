namespace DocsGenerator
{
    internal class Program
    {
        static void Main()
        {
            // Clean docs folder
            var docsRoot = Path.Combine("www", "docs", "kb");
            if (Directory.Exists(docsRoot))
                Directory.Delete(docsRoot, recursive: true);

            // Recreate root
            Directory.CreateDirectory(docsRoot);

            // Build articles
            new DocsGenerator.Tessellate.TesselateArticle().Build();
            new DocsGenerator.FontReader.FontReaderArticle().Build();
            new DocsGenerator.SvgDrawingContext.SvgDrawingContextArticle().Build();
        }
    }
}
