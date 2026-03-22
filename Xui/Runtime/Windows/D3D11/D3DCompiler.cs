using System;
using System.Runtime.InteropServices;

namespace Xui.GPU.Hardware.D3D11;

/// <summary>
/// Thin P/Invoke wrappers for the D3DCompiler shader compilation library.
/// Used to compile HLSL source code into binary shader bytecode at runtime.
/// </summary>
internal static partial class D3DCompiler
{
    private const string D3DCompilerLib = "d3dcompiler_47.dll";

    /// <summary>
    /// D3DCompile flags. See D3DCOMPILE_* constants in d3dcompiler.h.
    /// </summary>
    [Flags]
    internal enum CompileFlags : uint
    {
        None = 0,
        Debug = 0x1,
        SkipValidation = 0x2,
        SkipOptimization = 0x4,
        PackMatrixRowMajor = 0x8,
        PackMatrixColumnMajor = 0x10,
        PartialPrecision = 0x20,
        OptimizationLevel0 = 0x4000,
        OptimizationLevel1 = 0,
        OptimizationLevel2 = 0xC000,
        OptimizationLevel3 = 0x8000,
        WarningsAreErrors = 0x40000,
    }

    // D3DCompile from d3dcompiler_47.dll
    [LibraryImport(D3DCompilerLib, EntryPoint = "D3DCompile",
        StringMarshalling = StringMarshalling.Utf8)]
    private static unsafe partial int D3DCompileNative(
        byte* pSrcData,
        nuint srcDataSize,
        byte* pSourceName,
        void* pDefines,
        void* pInclude,
        byte* pEntrypoint,
        byte* pTarget,
        uint Flags1,
        uint Flags2,
        void** ppCode,
        void** ppErrorMsgs);

    /// <summary>
    /// Compiles HLSL source code to shader bytecode.
    /// </summary>
    /// <param name="hlslSource">The HLSL source code string.</param>
    /// <param name="entryPoint">The name of the entry point function.</param>
    /// <param name="target">The shader target profile (e.g. "vs_5_0", "ps_5_0").</param>
    /// <returns>The compiled shader bytecode.</returns>
    /// <exception cref="InvalidOperationException">Thrown if compilation fails.</exception>
    internal static unsafe byte[] Compile(string hlslSource, string entryPoint, string target)
    {
        var sourceBytes = System.Text.Encoding.UTF8.GetBytes(hlslSource + "\0");
        var entryBytes = System.Text.Encoding.UTF8.GetBytes(entryPoint + "\0");
        var targetBytes = System.Text.Encoding.UTF8.GetBytes(target + "\0");

        void* pCode = null;
        void* pErrors = null;

        int hr;
        fixed (byte* pSrc = sourceBytes)
        fixed (byte* pEntry = entryBytes)
        fixed (byte* pTarget = targetBytes)
        {
            hr = D3DCompileNative(
                pSrcData: pSrc,
                srcDataSize: (nuint)(sourceBytes.Length - 1),  // exclude null terminator
                pSourceName: null,
                pDefines: null,
                pInclude: null,
                pEntrypoint: pEntry,
                pTarget: pTarget,
                Flags1: (uint)(CompileFlags.PackMatrixRowMajor),
                Flags2: 0,
                ppCode: &pCode,
                ppErrorMsgs: &pErrors);
        }

        // If there are errors, extract them before throwing
        if (hr < 0)
        {
            string errorMessage = "HLSL compilation failed";
            if (pErrors != null)
            {
                // ID3DBlob: first 8 bytes are vtable ptr, next is GetBufferPointer and GetBufferSize
                // We use the COM interface layout: [vtable][refcount]
                // GetBufferPointer is vtable[3], GetBufferSize is vtable[4] on x64
                var errorBlob = new D3DBlob(pErrors);
                errorMessage = errorBlob.GetString();
                errorBlob.Release();
            }
            throw new InvalidOperationException($"HLSL shader compilation failed for entry point '{entryPoint}' (target: {target}):\n{errorMessage}");
        }

        if (pCode == null)
        {
            throw new InvalidOperationException($"HLSL shader compilation produced no output for entry point '{entryPoint}'.");
        }

        var codeBlob = new D3DBlob(pCode);
        try
        {
            return codeBlob.GetBytes();
        }
        finally
        {
            codeBlob.Release();
        }
    }

    /// <summary>
    /// Minimal wrapper around ID3DBlob COM object to extract bytecode.
    /// </summary>
    private unsafe struct D3DBlob
    {
        private readonly void* _ptr;

        internal D3DBlob(void* ptr) => _ptr = ptr;

        // ID3DBlob vtable layout (IUnknown + ID3DBlob):
        // [0] QueryInterface
        // [1] AddRef
        // [2] Release
        // [3] GetBufferPointer
        // [4] GetBufferSize
        internal void* GetBufferPointer()
        {
            var vtable = *(void***)_ptr;
            var fn = (delegate* unmanaged[Stdcall]<void*, void*>)vtable[3];
            return fn(_ptr);
        }

        internal nuint GetBufferSize()
        {
            var vtable = *(void***)_ptr;
            var fn = (delegate* unmanaged[Stdcall]<void*, nuint>)vtable[4];
            return fn(_ptr);
        }

        internal uint Release()
        {
            var vtable = *(void***)_ptr;
            var fn = (delegate* unmanaged[Stdcall]<void*, uint>)vtable[2];
            return fn(_ptr);
        }

        internal string GetString()
        {
            var ptr = (byte*)GetBufferPointer();
            var size = (int)GetBufferSize();
            return System.Text.Encoding.UTF8.GetString(ptr, size);
        }

        internal byte[] GetBytes()
        {
            var ptr = (byte*)GetBufferPointer();
            var size = (int)GetBufferSize();
            var result = new byte[size];
            Marshal.Copy((nint)ptr, result, 0, size);
            return result;
        }
    }
}
