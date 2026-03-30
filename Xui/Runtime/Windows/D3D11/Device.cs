using System;
using System.Runtime.InteropServices;
using static Xui.Runtime.Windows.COM;

namespace Xui.Runtime.Windows;

/// <summary>
/// Code from &lt;d3d11.h&gt; in the d3d11.dll lib.
/// </summary>
public static partial class D3D11
{
    public unsafe class Device : Unknown
    {
        public static new readonly Guid IID = new Guid("db6f6ddb-ac77-4e88-8253-819df9bbf140");

        public Device(void* ptr) : base(ptr)
        {
        }

        /// <summary>
        /// Creates a buffer resource.
        /// Wraps <c>ID3D11Device::CreateBuffer</c> (vtable [3]).
        /// </summary>
        public void* CreateBuffer(in BufferDesc desc, SubresourceData* pInitialData)
        {
            void* buffer;
            fixed (BufferDesc* descPtr = &desc)
            {
                Marshal.ThrowExceptionForHR(
                    ((delegate* unmanaged[MemberFunction]<void*, BufferDesc*, SubresourceData*, void**, int>)this[3])
                    (this, descPtr, pInitialData, &buffer));
            }
            return buffer;
        }

        /// <summary>
        /// Creates a 2D texture resource.
        /// Wraps <c>ID3D11Device::CreateTexture2D</c> (vtable [5]).
        /// </summary>
        public Texture2D CreateTexture2D(in Texture2DDesc desc, in SubresourceData initialData)
        {
            void* texture;
            fixed (Texture2DDesc* descPtr = &desc)
            fixed (SubresourceData* dataPtr = &initialData)
            {
                Marshal.ThrowExceptionForHR(
                    ((delegate* unmanaged[MemberFunction]<void*, Texture2DDesc*, SubresourceData*, void**, int>)this[5])
                    (this, descPtr, dataPtr, &texture));
            }
            return new Texture2D(texture);
        }

        /// <summary>
        /// Creates a 2D texture resource (no initial data).
        /// Wraps <c>ID3D11Device::CreateTexture2D</c> (vtable [5]).
        /// </summary>
        public void* CreateTexture2D(in Texture2DDesc desc)
        {
            void* texture;
            fixed (Texture2DDesc* descPtr = &desc)
            {
                Marshal.ThrowExceptionForHR(
                    ((delegate* unmanaged[MemberFunction]<void*, Texture2DDesc*, SubresourceData*, void**, int>)this[5])
                    (this, descPtr, null, &texture));
            }
            return texture;
        }

        /// <summary>
        /// Creates a render-target view.
        /// Wraps <c>ID3D11Device::CreateRenderTargetView</c> (vtable [9]).
        /// </summary>
        public void* CreateRenderTargetView(void* pResource, void* pDesc)
        {
            void* rtv;
            Marshal.ThrowExceptionForHR(
                ((delegate* unmanaged[MemberFunction]<void*, void*, void*, void**, int>)this[9])
                (this, pResource, pDesc, &rtv));
            return rtv;
        }

        /// <summary>
        /// Creates a depth-stencil view.
        /// Wraps <c>ID3D11Device::CreateDepthStencilView</c> (vtable [10]).
        /// </summary>
        public void* CreateDepthStencilView(void* pResource, void* pDesc)
        {
            void* dsv;
            Marshal.ThrowExceptionForHR(
                ((delegate* unmanaged[MemberFunction]<void*, void*, void*, void**, int>)this[10])
                (this, pResource, pDesc, &dsv));
            return dsv;
        }

        /// <summary>
        /// Creates an input-layout object.
        /// Wraps <c>ID3D11Device::CreateInputLayout</c> (vtable [11]).
        /// </summary>
        public void* CreateInputLayout(InputElementDesc* pInputElementDescs, uint numElements, void* pShaderBytecode, nuint bytecodeLength)
        {
            void* inputLayout;
            Marshal.ThrowExceptionForHR(
                ((delegate* unmanaged[MemberFunction]<void*, InputElementDesc*, uint, void*, nuint, void**, int>)this[11])
                (this, pInputElementDescs, numElements, pShaderBytecode, bytecodeLength, &inputLayout));
            return inputLayout;
        }

        /// <summary>
        /// Creates a vertex-shader object.
        /// Wraps <c>ID3D11Device::CreateVertexShader</c> (vtable [12]).
        /// </summary>
        public void* CreateVertexShader(void* pShaderBytecode, nuint bytecodeLength, void* pClassLinkage)
        {
            void* shader;
            Marshal.ThrowExceptionForHR(
                ((delegate* unmanaged[MemberFunction]<void*, void*, nuint, void*, void**, int>)this[12])
                (this, pShaderBytecode, bytecodeLength, pClassLinkage, &shader));
            return shader;
        }

        /// <summary>
        /// Creates a pixel-shader object.
        /// Wraps <c>ID3D11Device::CreatePixelShader</c> (vtable [15]).
        /// </summary>
        public void* CreatePixelShader(void* pShaderBytecode, nuint bytecodeLength, void* pClassLinkage)
        {
            void* shader;
            Marshal.ThrowExceptionForHR(
                ((delegate* unmanaged[MemberFunction]<void*, void*, nuint, void*, void**, int>)this[15])
                (this, pShaderBytecode, bytecodeLength, pClassLinkage, &shader));
            return shader;
        }

        /// <summary>
        /// Creates a depth-stencil state object.
        /// Wraps <c>ID3D11Device::CreateDepthStencilState</c> (vtable [21]).
        /// </summary>
        public void* CreateDepthStencilState(in DepthStencilDesc desc)
        {
            void* state;
            fixed (DepthStencilDesc* descPtr = &desc)
            {
                Marshal.ThrowExceptionForHR(
                    ((delegate* unmanaged[MemberFunction]<void*, DepthStencilDesc*, void**, int>)this[21])
                    (this, descPtr, &state));
            }
            return state;
        }

        /// <summary>
        /// Creates a rasterizer state object.
        /// Wraps <c>ID3D11Device::CreateRasterizerState</c> (vtable [22]).
        /// </summary>
        public void* CreateRasterizerState(in RasterizerDesc desc)
        {
            void* state;
            fixed (RasterizerDesc* descPtr = &desc)
            {
                Marshal.ThrowExceptionForHR(
                    ((delegate* unmanaged[MemberFunction]<void*, RasterizerDesc*, void**, int>)this[22])
                    (this, descPtr, &state));
            }
            return state;
        }
    }
}
