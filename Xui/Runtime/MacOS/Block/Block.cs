using System;
using System.Runtime.InteropServices;

namespace Xui.Runtime.MacOS;

public partial class Block
{
    public const string SystemLib = "/usr/lib/libSystem.dylib";

    public static readonly nint Lib;

    public static readonly nint NSConcreteStackBlock;
    public static readonly nint NSConcreteGlobalBlock;
    public static readonly nint GlobalBlockDescriptor;
    public static readonly nint StackBlockDescriptor;

    public static readonly nint CopyFPtr;

    public static readonly nint InvokeFPtr;

    public static readonly nint DisposeFPtr;

    static unsafe Block()
    {
        Lib = NativeLibrary.Load(SystemLib);

        NSConcreteStackBlock = NativeLibrary.GetExport(Block.Lib, "_NSConcreteStackBlock");
        NSConcreteGlobalBlock = NativeLibrary.GetExport(Block.Lib, "_NSConcreteGlobalBlock");

        CopyFPtr = Marshal.GetFunctionPointerForDelegate<CopyDelegate>(Copy);
        InvokeFPtr = Marshal.GetFunctionPointerForDelegate<InvokeDelegate>(Invoke);
        DisposeFPtr = Marshal.GetFunctionPointerForDelegate<DisposeDelegate>(Dispose);

        GlobalBlockDescriptor = (nint)NativeMemory.AlignedAlloc((nuint)Marshal.SizeOf<BlockDescriptor>(), 8);
        Marshal.StructureToPtr(new BlockDescriptor() {
            Reserved = 0,
            Size = (uint)Marshal.SizeOf<BlockLiteral>(),
            CopyFPtr = 0,
            DisposeFPtr = 0,
            Signature = 0,
        }, GlobalBlockDescriptor, false);

        StackBlockDescriptor = (nint)NativeMemory.AlignedAlloc((nuint)Marshal.SizeOf<BlockDescriptor>(), 8);
        Marshal.StructureToPtr(new BlockDescriptor() {
            Reserved = 0,
            Size = (uint)Marshal.SizeOf<BlockLiteral>(),
            CopyFPtr = 0, // Marshal.GetFunctionPointerForDelegate<CopyDelegate>(Copy),
            DisposeFPtr = Marshal.GetFunctionPointerForDelegate<DisposeDelegate>(Dispose),
            Signature = 0,
        }, StackBlockDescriptor, false);
    }

    public delegate void CopyDelegate(ref BlockLiteral dst, ref BlockLiteral src);

    private static void Copy(ref BlockLiteral dst, ref BlockLiteral src)
    {
    }

    public delegate void DisposeDelegate(ref BlockLiteral blockRef);

    private static void Dispose(ref BlockLiteral blockRef) =>
        GCHandle.FromIntPtr(blockRef.Handler).Free();

    public delegate void InvokeDelegate(ref BlockLiteral blockRef);

    private static void Invoke(ref BlockLiteral blockRef) =>
        ((Action)GCHandle.FromIntPtr(blockRef.Handler).Target!)();
}