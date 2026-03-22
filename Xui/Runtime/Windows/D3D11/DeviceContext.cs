
using System;
using System.Runtime.InteropServices;
using static Xui.Runtime.Windows.COM;

namespace Xui.Runtime.Windows;

public static unsafe partial class D3D11
{
    public unsafe class DeviceContext : DeviceChild
    {
        public static new readonly Guid IID = new Guid("c0bfa96c-e089-44fb-8eaf-26f8796190da");

        public DeviceContext(void* ptr) : base(ptr)
        {
        }

        /// <summary>
        /// Sets constant buffers for the vertex shader stage.
        /// Wraps <c>ID3D11DeviceContext::VSSetConstantBuffers</c> (vtable [7]).
        /// </summary>
        public void VSSetConstantBuffers(uint startSlot, uint numBuffers, void** ppConstantBuffers) =>
            ((delegate* unmanaged[MemberFunction]<void*, uint, uint, void**, void>)this[7])
            (this, startSlot, numBuffers, ppConstantBuffers);

        /// <summary>
        /// Sets a pixel shader.
        /// Wraps <c>ID3D11DeviceContext::PSSetShader</c> (vtable [9]).
        /// </summary>
        public void PSSetShader(void* pPixelShader, void** ppClassInstances, uint numClassInstances) =>
            ((delegate* unmanaged[MemberFunction]<void*, void*, void**, uint, void>)this[9])
            (this, pPixelShader, ppClassInstances, numClassInstances);

        /// <summary>
        /// Sets a vertex shader.
        /// Wraps <c>ID3D11DeviceContext::VSSetShader</c> (vtable [11]).
        /// </summary>
        public void VSSetShader(void* pVertexShader, void** ppClassInstances, uint numClassInstances) =>
            ((delegate* unmanaged[MemberFunction]<void*, void*, void**, uint, void>)this[11])
            (this, pVertexShader, ppClassInstances, numClassInstances);

        /// <summary>
        /// Maps a subresource for CPU access.
        /// Wraps <c>ID3D11DeviceContext::Map</c> (vtable [14]).
        /// </summary>
        public void Map(void* pResource, uint subresource, Map mapType, uint mapFlags, MappedSubresource* pMappedResource)
        {
            Marshal.ThrowExceptionForHR(
                ((delegate* unmanaged[MemberFunction]<void*, void*, uint, Map, uint, MappedSubresource*, int>)this[14])
                (this, pResource, subresource, mapType, mapFlags, pMappedResource));
        }

        /// <summary>
        /// Unmaps a previously mapped subresource.
        /// Wraps <c>ID3D11DeviceContext::Unmap</c> (vtable [15]).
        /// </summary>
        public void Unmap(void* pResource, uint subresource) =>
            ((delegate* unmanaged[MemberFunction]<void*, void*, uint, void>)this[15])
            (this, pResource, subresource);

        /// <summary>
        /// Sets constant buffers for the pixel shader stage.
        /// Wraps <c>ID3D11DeviceContext::PSSetConstantBuffers</c> (vtable [16]).
        /// </summary>
        public void PSSetConstantBuffers(uint startSlot, uint numBuffers, void** ppConstantBuffers) =>
            ((delegate* unmanaged[MemberFunction]<void*, uint, uint, void**, void>)this[16])
            (this, startSlot, numBuffers, ppConstantBuffers);

        /// <summary>
        /// Sets the input layout.
        /// Wraps <c>ID3D11DeviceContext::IASetInputLayout</c> (vtable [17]).
        /// </summary>
        public void IASetInputLayout(void* pInputLayout) =>
            ((delegate* unmanaged[MemberFunction]<void*, void*, void>)this[17])
            (this, pInputLayout);

        /// <summary>
        /// Binds vertex buffers to the input-assembler stage.
        /// Wraps <c>ID3D11DeviceContext::IASetVertexBuffers</c> (vtable [18]).
        /// </summary>
        public void IASetVertexBuffers(uint startSlot, uint numBuffers, void** ppVertexBuffers, uint* pStrides, uint* pOffsets) =>
            ((delegate* unmanaged[MemberFunction]<void*, uint, uint, void**, uint*, uint*, void>)this[18])
            (this, startSlot, numBuffers, ppVertexBuffers, pStrides, pOffsets);

        /// <summary>
        /// Sets the primitive topology.
        /// Wraps <c>ID3D11DeviceContext::IASetPrimitiveTopology</c> (vtable [24]).
        /// </summary>
        public void IASetPrimitiveTopology(uint topology) =>
            ((delegate* unmanaged[MemberFunction]<void*, uint, void>)this[24])
            (this, topology);

        /// <summary>
        /// Sets render targets and the depth-stencil buffer.
        /// Wraps <c>ID3D11DeviceContext::OMSetRenderTargets</c> (vtable [33]).
        /// </summary>
        public void OMSetRenderTargets(uint numViews, void** ppRenderTargetViews, void* pDepthStencilView) =>
            ((delegate* unmanaged[MemberFunction]<void*, uint, void**, void*, void>)this[33])
            (this, numViews, ppRenderTargetViews, pDepthStencilView);

        /// <summary>
        /// Sets the depth-stencil state.
        /// Wraps <c>ID3D11DeviceContext::OMSetDepthStencilState</c> (vtable [35]).
        /// </summary>
        public void OMSetDepthStencilState(void* pDepthStencilState, uint stencilRef) =>
            ((delegate* unmanaged[MemberFunction]<void*, void*, uint, void>)this[35])
            (this, pDepthStencilState, stencilRef);

        /// <summary>
        /// Sets the rasterizer state.
        /// Wraps <c>ID3D11DeviceContext::RSSetState</c> (vtable [43]).
        /// </summary>
        public void RSSetState(void* pRasterizerState) =>
            ((delegate* unmanaged[MemberFunction]<void*, void*, void>)this[43])
            (this, pRasterizerState);

        /// <summary>
        /// Sets the viewports.
        /// Wraps <c>ID3D11DeviceContext::RSSetViewports</c> (vtable [44]).
        /// </summary>
        public void RSSetViewports(uint numViewports, Viewport* pViewports) =>
            ((delegate* unmanaged[MemberFunction]<void*, uint, Viewport*, void>)this[44])
            (this, numViewports, pViewports);

        /// <summary>
        /// Copies a resource.
        /// Wraps <c>ID3D11DeviceContext::CopyResource</c> (vtable [47]).
        /// </summary>
        public void CopyResource(void* pDstResource, void* pSrcResource) =>
            ((delegate* unmanaged[MemberFunction]<void*, void*, void*, void>)this[47])
            (this, pDstResource, pSrcResource);

        /// <summary>
        /// Clears a render target view to a specified color.
        /// Wraps <c>ID3D11DeviceContext::ClearRenderTargetView</c> (vtable [50]).
        /// </summary>
        public void ClearRenderTargetView(void* pRenderTargetView, float* colorRGBA) =>
            ((delegate* unmanaged[MemberFunction]<void*, void*, float*, void>)this[50])
            (this, pRenderTargetView, colorRGBA);

        /// <summary>
        /// Clears a depth-stencil view.
        /// Wraps <c>ID3D11DeviceContext::ClearDepthStencilView</c> (vtable [53]).
        /// </summary>
        public void ClearDepthStencilView(void* pDepthStencilView, uint clearFlags, float depth, byte stencil) =>
            ((delegate* unmanaged[MemberFunction]<void*, void*, uint, float, byte, void>)this[53])
            (this, pDepthStencilView, clearFlags, depth, stencil);

        /// <summary>
        /// Updates a subresource.
        /// Wraps <c>ID3D11DeviceContext::UpdateSubresource</c> (vtable [68]).
        /// </summary>
        public void UpdateSubresource(void* pDstResource, uint dstSubresource, void* pDstBox, void* pSrcData, uint srcRowPitch, uint srcDepthPitch) =>
            ((delegate* unmanaged[MemberFunction]<void*, void*, uint, void*, void*, uint, uint, void>)this[68])
            (this, pDstResource, dstSubresource, pDstBox, pSrcData, srcRowPitch, srcDepthPitch);

        /// <summary>
        /// Draws non-indexed, non-instanced primitives.
        /// Wraps <c>ID3D11DeviceContext::Draw</c> (vtable [73]).
        /// </summary>
        public void DrawPrimitive(uint vertexCount, uint startVertexLocation) =>
            ((delegate* unmanaged[MemberFunction]<void*, uint, uint, void>)this[73])
            (this, vertexCount, startVertexLocation);

        /// <summary>
        /// Clears all device state to defaults.
        /// Wraps <c>ID3D11DeviceContext::ClearState</c> (vtable [110]).
        /// </summary>
        public void ClearState() =>
            ((delegate* unmanaged[MemberFunction]<void*, void>)this[110])(this);

        /// <summary>
        /// Sends queued commands to the GPU.
        /// Wraps <c>ID3D11DeviceContext::Flush</c> (vtable [114]).
        /// </summary>
        public void Flush() =>
            ((delegate* unmanaged[MemberFunction]<void*, void>)this[114])(this);
    }
}