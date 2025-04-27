using Xui.Core.UI;
using static Xui.Core.Canvas.Colors;
using Xui.Core.Canvas;

namespace Xui.Apps.BlankApp;

public class App : Xui.Core.Abstract.Application
{
    public static int Main(string[] argv)
    {
#if MACOS && EMULATOR
        Xui.Core.Actual.Runtime.Current = new Xui.Middleware.Emulator.Actual.EmulatorPlatform(
            Xui.Runtime.MacOS.Actual.MacOSPlatform.Instance);
#elif WINDOWS && EMULATOR
        Xui.Core.Actual.Runtime.Current = new Xui.Middleware.Emulator.Actual.EmulatorPlatform(
            Xui.Runtime.Windows.Actual.Win32Platform.Instance);
#elif BROWSER && EMULATOR
        Xui.Core.Actual.Runtime.Current = new Xui.Middleware.Emulator.Actual.EmulatorPlatform(
            Xui.Runtime.Browser.Actual.BrowserPlatform.Instance);
#elif IOS
        Xui.Core.Actual.Runtime.Current = Xui.Runtime.IOS.Actual.IOSPlatform.Instance;
#elif ANDROID
        Xui.Core.Actual.Runtime.Current = Xui.Runtime.Android.Actual.AndroidPlatform.Instance;
#elif MACOS
        Xui.Core.Actual.Runtime.Current = Xui.Runtime.MacOS.Actual.MacOSPlatform.Instance;
#elif WINDOWS
        Xui.Core.Actual.Runtime.Current = Xui.Runtime.Windows.Actual.Win32Platform.Instance;
#elif BROWSER
        Xui.Core.Actual.Runtime.Current = Xui.Runtime.Browser.Actual.BrowserPlatform.Instance;
#endif

        return new App().Run();
    }

    public override void Start()
    {
        var window = new MainWindow
        {
            Title = "Xui BlankApp",

            Content = new VerticalStack()
            {
                Margin = (50, 0, 0, 0),
                Content = [
                    new HorizontalUniformStack()
                    {
                        Content = [
                            new Border()
                            {
                                BorderThickness = 2,
                                BackgroundColor = Teal,
                                BorderColor = Red,
                                MinimumWidth = 50,
                                MinimumHeight = 40,
                                Content = new Label()
                                {
                                    Text = "Hello World"
                                }
                            },
                            new Border()
                            {
                                BorderThickness = 10,
                                BackgroundColor = Teal,
                                BorderColor = Red,
                                CornerRadius = 20,
                                MinimumWidth = 50,
                                MinimumHeight = 40
                            },
                            new Border()
                            {
                                BorderThickness = 10,
                                BackgroundColor = Teal,
                                BorderColor = Red,
                                CornerRadius = (20, 0, 20, 0),
                                MinimumWidth = 50,
                                MinimumHeight = 40
                            }
                        ]
                    },
                    new Border() 
                    {
                        BorderThickness = 1,
                        BorderColor = Orange,
                        Margin = 5,
                        Content = new Label()
                        {
                            Text = "Line2",
                            Margin = 5,
                            FontStyle = FontStyle.Normal,
                            FontWeight = 400,
                            FontSize = 23
                        }
                    }
                ]
            }

            // Content = new Border
            // {
            //     Margin = 20,
            //     BorderWidth = 20,
            //     BorderColor = Blue,
            //     BackgroundColor = Green,
            //     Content = new Border
            //     {
            //         BorderWidth = 20,
            //         Margin = 5,
            //         BackgroundColor = 0xFFFFFF66,
            //         BorderColor = 0xFF000066,
            //         Content = new Label("hello")
            //     }
            // }

            // Content = new VerticalStack
            // {
            //     Margin = (15, 50, 15, 20),

            //     Content =
            //     [
            //         new Border
            //         {
            //             Margin = 5,
            //             VerticalAlign = VerticalAlign.Top,
            //             BorderWidth = 1,
            //             Padding = 0,
            //             BorderColor = Black,
            //             BackgroundColor = Colors.White,
            //             BorderRadius = 10,

            //             Content = new VerticalStack
            //             {
            //                 Content =
            //                 [
            //                     new Border
            //                     {
            //                         Margin = 5,
            //                         Padding = 5,
            //                         BorderRadius = 3,
            //                         Content = new Label("Nulla nec orci laoreet, finibus dui et,\ntristique mauris.\nFusce aliquam sed mi quis fermentum.")
            //                     },
            //                     new Border
            //                     {
            //                         Margin = 5,
            //                         Padding = 5,
            //                         Content = new Label("Xuiapps is awesome asd asd")
            //                     },
            //                     new HorizontalUniformStack
            //                     {
            //                         Margin = 5,
            //                         Gap=10,
            //                         Content = [
            //                             new Border
            //                             {
            //                                 BorderWidth = 1,
            //                                 BorderRadius = 7,
            //                                 Padding = 5,
            //                                 BorderColor = 0x999999FF,
            //                                 BackgroundColor = 0xEEEEEEFF,
            //                                 Content = new Label
            //                                 {
            //                                     HorizontalAlign = HorizontalAlign.Center,
            //                                     Text = "❤️"
            //                                 }
            //                             },
            //                             new Border
            //                             {
            //                                 BorderWidth = 1,
            //                                 BorderRadius = 7,
            //                                 Padding = 5,
            //                                 BorderColor = 0x999999FF,
            //                                 BackgroundColor = 0xEEEEEEFF,
            //                                 Content = new Label
            //                                 {
            //                                     HorizontalAlign = HorizontalAlign.Center,
            //                                     Text = "👍"
            //                                 }
            //                             },
            //                             new Border
            //                             {
            //                                 BorderWidth = 1,
            //                                 BorderRadius = 7,
            //                                 Padding = 5,
            //                                 BorderColor = 0x999999FF,
            //                                 BackgroundColor = 0xEEEEEEFF,
            //                                 Content = new Label
            //                                 {
            //                                     HorizontalAlign = HorizontalAlign.Center,
            //                                     Text = "💬"
            //                                 }
            //                             }
            //                         ]
            //                     }
            //                 ]
            //             }
            //         },
            //         new Border
            //         {
            //             Margin = 5,
            //             VerticalAlign = VerticalAlign.Top,
            //             BorderWidth = 3,
            //             Padding = 0,
            //             BorderColor = Black,
            //             BackgroundColor = Colors.White,
            //             BorderRadius = 10,

            //             Content = new VerticalStack
            //             {
            //                 Content =
            //                 [
            //                     new Border
            //                     {
            //                         Margin = 5,
            //                         Padding = 5,
            //                         BorderRadius = 3,
            //                         Content = new Label("Nulla nec orci laoreet, finibus dui et,\ntristique mauris.\nFusce aliquam sed mi quis fermentum.")
            //                     },
            //                     new Border
            //                     {
            //                         Margin = 5,
            //                         Padding = 5,
            //                         Content = new Label("Xuiapps is awesome asd asd")
            //                     },
            //                     new HorizontalUniformStack
            //                     {
            //                         Margin = 5,
            //                         Gap=10,
            //                         Content = [
            //                             new Border
            //                             {
            //                                 BorderWidth = 3,
            //                                 BorderRadius = 7,
            //                                 Padding = 5,
            //                                 BorderColor = 0x999999FF,
            //                                 BackgroundColor = 0xEEEEEEFF,
            //                                 Content = new Label
            //                                 {
            //                                     HorizontalAlign = HorizontalAlign.Center,
            //                                     Text = "❤️"
            //                                 }
            //                             },
            //                             new Border
            //                             {
            //                                 BorderWidth = 1,
            //                                 BorderRadius = 7,
            //                                 Padding = 5,
            //                                 BorderColor = 0x999999FF,
            //                                 BackgroundColor = 0xEEEEEEFF,
            //                                 Content = new Label
            //                                 {
            //                                     HorizontalAlign = HorizontalAlign.Center,
            //                                     Text = "👍"
            //                                 }
            //                             },
            //                             new Border
            //                             {
            //                                 BorderWidth = 1,
            //                                 BorderRadius = 7,
            //                                 Padding = 5,
            //                                 BorderColor = 0x999999FF,
            //                                 BackgroundColor = 0xEEEEEEFF,
            //                                 Content = new Label
            //                                 {
            //                                     HorizontalAlign = HorizontalAlign.Center,
            //                                     Text = "💬"
            //                                 }
            //                             }
            //                         ]
            //                     }
            //                 ]
            //             }
            //         }
            //     ]
            // }
        };

        window.Show();
    }
}
