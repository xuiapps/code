using Xui.Runtime.Software.Font;
using static Xui.Runtime.Software.Actual.SvgDrawingContext;

namespace DocsGenerator;

public class XuiDemoFontResolver : SvgFontResolver
{
    public static readonly XuiDemoFontResolver Instance = new XuiDemoFontResolver();

    public override Resolved Resolve(FontFace face, Uri? uri)
    {
        if (uri != null &&
            uri.Scheme == "embedded" &&
            string.Equals(uri.Authority, "Xui.Core.Fonts", StringComparison.InvariantCultureIgnoreCase))
        {
            var filename = uri.Segments.LastOrDefault();
            if (!string.IsNullOrEmpty(filename))
            {
                var webUri = new Uri($"https://xuiapps.com/fonts/{filename}", UriKind.Absolute);
                return new Resolved(SvgFontMode.WebLink, webUri);
            }
        }

        // Fallback to default behavior (System-installed)
        return base.Resolve(face, uri);
    }
}
