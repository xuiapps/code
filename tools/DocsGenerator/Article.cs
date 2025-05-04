using System;
using System.IO;
using System.Reflection;

namespace DocsGenerator;

/// <summary>
/// Base class for documentation articles. Handles output folder setup and file writing.
/// </summary>
public abstract class Article
{
    private readonly string folderName;
    private readonly string outputDir;

    /// <summary>
    /// Initializes a new article under the specified folder name.
    /// </summary>
    protected Article(string folderName)
    {
        this.folderName = folderName;
        outputDir = Path.Combine("www", "docs", "kb", folderName);
    }

    /// <summary>
    /// Called to build this article's outputs (e.g., SVGs, markdown, etc.).
    /// Subclasses should override this.
    /// </summary>
    public virtual void Build()
    {
        Directory.CreateDirectory(outputDir);
        CopyReadme();
    }

    /// <summary>
    /// Creates a new file for writing inside this article's output directory.
    /// </summary>
    public Stream WriteFile(string fileName)
    {
        var fullPath = Path.Combine(outputDir, fileName);
        return File.Create(fullPath);
    }

    /// <summary>
    /// Gets the full absolute path to a file inside this article’s output directory.
    /// </summary>
    public string GetPath(string fileName) =>
        Path.Combine(outputDir, fileName);
    
    private void CopyReadme()
    {
        var assembly = Assembly.GetExecutingAssembly();
        var resourceName = assembly.GetManifestResourceNames()
            .FirstOrDefault(name => name.EndsWith($"{folderName}.README.md", StringComparison.OrdinalIgnoreCase));

        if (resourceName is null)
        {
            Console.WriteLine("README.md not found in embedded resources.");
            return;
        }

        using var stream = assembly.GetManifestResourceStream(resourceName);
        using var reader = new StreamReader(stream!);
        File.WriteAllText(Path.Combine(outputDir, "README.md"), reader.ReadToEnd());
    }
}
