using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using static Xui.Runtime.MacOS.Block;

namespace Xui.Runtime.MacOS;

public static partial class Dispatch
{
    public const string DispatchLib = "/usr/lib/system/libdispatch.dylib";

    public static readonly nint Lib;

    static Dispatch()
    {
        Lib = NativeLibrary.Load(DispatchLib);
        _dispatch_main_q = NativeLibrary.GetExport(Lib, "_dispatch_main_q");
    }

    // // TODO: Working with Objecitve-C block is completely separate story:
    // // https://clang.llvm.org/docs/Block-ABI-Apple.html
    // [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    // public delegate void DispatchBlock();

    private static readonly nint _dispatch_main_q;

    /// <summary>
    /// An alternative for the dispatch_get_main_queue macro,
    /// under the hood it reads _dispatch_main_q from libdispatch.dylib.
    /// </summary>
    public static nint MainQueue => _dispatch_main_q;

    [LibraryImport(DispatchLib, EntryPoint="dispatch_async")]
    public static partial void DispatchAsync(nint queue, nint block);

    [LibraryImport(DispatchLib, EntryPoint="dispatch_async")]
    public static partial void DispatchAsync(nint queue, ref BlockLiteral block);

    [LibraryImport(DispatchLib, EntryPoint="dispatch_async")]
    public static partial void DispatchAsync(nint queue, BlockRef block);
}