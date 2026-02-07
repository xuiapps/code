using System;
using System.Runtime.InteropServices;
using static Xui.Runtime.Windows.COM;
using static Xui.Runtime.Windows.Win32.Types;

namespace Xui.Runtime.Windows;

/// <summary>
/// Code from &lt;dwrite.h&gt; in the dwrite.dll lib.
/// </summary>
public static partial class DWrite
{
    public unsafe partial class Factory : Unknown
    {
        private static new readonly Guid IID = new Guid("B859EE5A-D838-4B5B-A2E8-1ADC7D93DB48");

        [LibraryImport("DWrite.dll")]
        public static partial HRESULT DWriteCreateFactory(FactoryType factoryType, in Guid iid, out void* ppIFactory);

        private static void* CreateFactory()
        {
            Marshal.ThrowExceptionForHR(DWriteCreateFactory(FactoryType.Isolated, in IID, out var ppIFactory));
            return ppIFactory;
        }

        public Factory() : base(CreateFactory())
        {
        }

        public Factory(void* ptr) : base(ptr)
        {
        }

        public FontCollection GetSystemFontCollection(bool checkForUpdates = false)
        {
            void* fontCollection;

            Marshal.ThrowExceptionForHR(
                ((delegate* unmanaged[MemberFunction]<void*, void**, int, int>)this[3])(
                    this,
                    &fontCollection,
                    checkForUpdates ? 1 : 0));

            return new FontCollection(fontCollection);
        }

        public FontCollection.Ref GetSystemFontCollectionRef(bool checkForUpdates = false)
        {
            void* fontCollection;

            Marshal.ThrowExceptionForHR(
                ((delegate* unmanaged[MemberFunction]<void*, void**, int, int>)this[3])(
                    this,
                    &fontCollection,
                    checkForUpdates ? 1 : 0));

            return new FontCollection.Ref(fontCollection);
        }

        public TextFormat CreateTextFormat(string fontFamilyName, FontCollection? fontCollection, FontWeight fontWeight, FontStyle fontStyle, FontStretch fontStretch, float fontSize, string localeName)
        {
            void* textFormat;

            fixed (void* fontFamilyNamePtr = &global::System.Runtime.InteropServices.Marshalling.Utf16StringMarshaller.GetPinnableReference(fontFamilyName))
            fixed (void* localeNamePtr = &global::System.Runtime.InteropServices.Marshalling.Utf16StringMarshaller.GetPinnableReference(localeName))
            {
                Marshal.ThrowExceptionForHR(((delegate* unmanaged[MemberFunction]<void*, void*, void*, FontWeight, FontStyle, FontStretch, float, void*, void**, int>)this[15])(this, fontFamilyNamePtr, fontCollection, fontWeight, fontStyle, fontStretch, fontSize, localeNamePtr, &textFormat));
            }

            return new TextFormat(textFormat);
        }

        public TextLayout CreateTextLayout(string text, TextFormat textFormat, float maxWidth, float maxHeight)
        {
            void* textLayout;
            fixed (void* textPtr = &global::System.Runtime.InteropServices.Marshalling.Utf16StringMarshaller.GetPinnableReference(text))
            {
                Marshal.ThrowExceptionForHR(((delegate* unmanaged[MemberFunction]<void*, void*, uint, void*, float, float, void**, int>)this[18])(this, textPtr, (uint)text.Length, textFormat, maxWidth, maxHeight, &textLayout));
            }
            return new TextLayout(textLayout);
        }

        public TextLayout.Ref CreateTextLayoutRef(string text, TextFormat textFormat, float maxWidth, float maxHeight)
        {
            void* textLayout;
            fixed (void* textPtr = &global::System.Runtime.InteropServices.Marshalling.Utf16StringMarshaller.GetPinnableReference(text))
            {
                Marshal.ThrowExceptionForHR(((delegate* unmanaged[MemberFunction]<void*, void*, uint, void*, float, float, void**, int>)this[18])(this, textPtr, (uint)text.Length, textFormat, maxWidth, maxHeight, &textLayout));
            }
            return new TextLayout.Ref(textLayout);
        }
    }
}