using System;

public static class Program
{
    public static int Main(string[] args)
    {
        #if EMULATOR && MACOS
        Console.WriteLine("Emulator running on macOS...");
        #elif EMULATOR && WINDOWS
        Console.WriteLine("Emulator running on Windows...");
        #elif WINDOWS
        Console.WriteLine("Windows app");
        #elif MACOS
        Console.WriteLine("macOS app");
        #elif IOS
        Console.WriteLine("iOS app");
        #elif ANDROID
        Console.WriteLine("Android app");
        #endif

        return 0;
    }
}
