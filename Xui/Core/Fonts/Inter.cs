namespace Xui.Core.Fonts;

public static class Inter
{
    /// <summary>
    /// Returns URIs for all embedded Inter font variants.
    /// </summary>
    public static IEnumerable<Uri> URIs
    {
        get
        {
            const string assembly = "Xui.Core.Fonts";
            const string folder = "Inter";

            yield return new Uri($"embedded://{assembly}/{folder}/Inter-Black.ttf");
            yield return new Uri($"embedded://{assembly}/{folder}/Inter-BlackItalic.ttf");
            yield return new Uri($"embedded://{assembly}/{folder}/Inter-Bold.ttf");
            yield return new Uri($"embedded://{assembly}/{folder}/Inter-BoldItalic.ttf");
            yield return new Uri($"embedded://{assembly}/{folder}/Inter-ExtraBold.ttf");
            yield return new Uri($"embedded://{assembly}/{folder}/Inter-ExtraBoldItalic.ttf");
            yield return new Uri($"embedded://{assembly}/{folder}/Inter-ExtraLight.ttf");
            yield return new Uri($"embedded://{assembly}/{folder}/Inter-ExtraLightItalic.ttf");
            yield return new Uri($"embedded://{assembly}/{folder}/Inter-Italic.ttf");
            yield return new Uri($"embedded://{assembly}/{folder}/Inter-Light.ttf");
            yield return new Uri($"embedded://{assembly}/{folder}/Inter-LightItalic.ttf");
            yield return new Uri($"embedded://{assembly}/{folder}/Inter-Medium.ttf");
            yield return new Uri($"embedded://{assembly}/{folder}/Inter-MediumItalic.ttf");
            yield return new Uri($"embedded://{assembly}/{folder}/Inter-Regular.ttf");
            yield return new Uri($"embedded://{assembly}/{folder}/Inter-SemiBold.ttf");
            yield return new Uri($"embedded://{assembly}/{folder}/Inter-SemiBoldItalic.ttf");
            yield return new Uri($"embedded://{assembly}/{folder}/Inter-Thin.ttf");
            yield return new Uri($"embedded://{assembly}/{folder}/Inter-ThinItalic.ttf");
        }
    }
}
