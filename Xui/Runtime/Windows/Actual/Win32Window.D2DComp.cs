using System;
using Xui.Core.Abstract.Events;
using Xui.Core.Math2D;
using Xui.Runtime.Windows.Win32;
using static Xui.Runtime.Windows.D2D1;
using static Xui.Runtime.Windows.DXGI;
using static Xui.Runtime.Windows.Win32.User32;

namespace Xui.Runtime.Windows.Actual;

public partial class Win32Window
{
    public partial class D2DComp : RenderTarget
    {

        public D2DComp(Win32Window win32Window) : base(win32Window)
        {
        }

        // Composition SwapChain
        protected D3D11.Device D3D11Device { get; private set; }
        protected D3D11.FeatureLevel D3D11FeatureLevel { get; private set; }
        protected D3D11.DeviceContext D3D11DeviceContext { get; private set; }
        protected DXGI.Device DXGIDevice { get; private set; }
        protected DXGI.Factory2 DXGIFactory2 { get; private set; }
        protected DXGI.SwapChain1 SwapChain1 { get; private set; }

        // Direct2D
        protected D2D1.Factory3 D2D1Factory3 { get; private set; }
        protected D2D1.Device1 D2D1Device1 { get; private set; }
        protected D2D1.DeviceContext D2D1DeviceContext { get; private set; }

        //
        protected DXGI.Surface DXGISurface { get; private set; }
        protected D2D1.Bitmap1 D2D1Bitmap1 { get; private set; }

        // DComposition
        protected DComp.Device DCompDevice { get; private set; }
        protected DComp.Target DCompTarget { get; private set; }
        protected DComp.Visual DCompVisual { get; private set; }

        // 
        protected D2D1.RenderTarget? RenderTarget => this.D2D1DeviceContext;
        protected DWrite.Factory DWriteFactory { get; private set; }
        protected Direct2DContext Direct2DContext { get; private set; }

        // Frame
        private TimeSpan LastFrameTime = TimeSpan.Zero;
        private TimeSpan LastNextEstimatedFrameTime = TimeSpan.Zero;
        private TimeSpan NextEstimatedFrameTime = TimeSpan.Zero;

        public override bool HandleOnMessage(User32.Types.HWND hWnd, User32.WindowMessage uMsg, Win32.Types.WPARAM wParam, Win32.Types.LPARAM lParam, out int result)
        {
            if (uMsg == WindowMessage.WM_PAINT)
            {
                unsafe
                {
                    if (this.SwapChain1 == null)
                    {
                        // Composition SwapChain
                        D3D11.CreateDevice(out var d3d11Device, out var d3d11FeatureLevel, out var d3d11DeviceContext);
                        this.D3D11Device = d3d11Device;
                        this.D3D11FeatureLevel = d3d11FeatureLevel;
                        this.D3D11DeviceContext = d3d11DeviceContext;
                        this.DXGIDevice = new DXGI.Device(this.D3D11Device.QueryInterface(in DXGI.Device.IID));
                        this.DXGIFactory2 = DXGI.Factory2.Create();

                        hWnd.GetClientRect(out var rect);
                        SwapChainDesc1 swapChainDesc1 = new SwapChainDesc1()
                        {
                            Width = (uint)(rect.Right - rect.Left),
                            Height = (uint)(rect.Bottom - rect.Top),
                            Format = Format.B8G8R8A8_UNORM,
                            Stereo = false,
                            SampleDesc = new SampleDesc()
                            {
                                Count = 1,
                                Quality = 0,
                            },
                            BufferUsage = Usage.RenderTargetOutput,
                            BufferCount = 2,
                            Scaling = Scaling.Stretch,
                            SwapEffect = SwapEffect.FlipSequential,
                            AlphaMode = DXGI.AlphaMode.Premultiplied,
                            Flags = 0
                        };
                        this.SwapChain1 = this.DXGIFactory2.CreateSwapChainForComposition(this.DXGIDevice, swapChainDesc1);

                        // Now Direct2D
                        this.D2D1Factory3 = new D2D1.Factory3();
                        this.D2D1Device1 = this.D2D1Factory3.CreateDevice(this.DXGIDevice);
                        this.D2D1DeviceContext = this.D2D1Device1.CreateDeviceContext(D2D1.DeviceContextOptions.None);

                        // 
                        this.DXGISurface = this.SwapChain1.GetBufferAsSurface(0);
                        D2D1.BitmapProperties1 bitmapProperties1 = new D2D1.BitmapProperties1()
                        {
                            PixelFormat = new D2D1.PixelFormat()
                            {
                                AlphaMode = D2D1.AlphaMode.Premultiplied,
                                Format = Format.B8G8R8A8_UNORM
                            },
                            BitmapOptions = D2D1.BitmapOptions.Target | D2D1.BitmapOptions.CannotDraw,
                        };
                        this.D2D1Bitmap1 = this.D2D1DeviceContext.CreateBitmapFromDxgiSurface(this.DXGISurface, bitmapProperties1);
                        this.D2D1DeviceContext.SetTarget(this.D2D1Bitmap1);

                        this.DWriteFactory = new DWrite.Factory();

                        this.Direct2DContext = new Direct2DContext(this.D2D1DeviceContext, D2D1Factory3, DWriteFactory);

                        // DComposition
                        this.DCompDevice = DComp.Device.Create(this.DXGIDevice);
                        this.DCompTarget = this.DCompDevice.CreateTargetForHwnd(hWnd, false);
                        this.DCompVisual = this.DCompDevice.CreateVisual();
                        this.DCompVisual.SetContent(this.SwapChain1);
                        this.DCompTarget.SetRoot(this.DCompVisual);
                        this.DCompDevice.Commit();
                    }

                    if (this.RenderTarget != null)
                    {
                        hWnd.GetClientRect(out var rc);
                        D2D1.SizeU size = new SizeU()
                        {
                            Width = (uint)(rc.Right - rc.Left),
                            Height = (uint)(rc.Bottom - rc.Top)
                        };

                        PAINTSTRUCT ps = new();
                        hWnd.BeginPaint(ref ps);

                        FrameEventRef f = new FrameEventRef(this.LastNextEstimatedFrameTime, this.NextEstimatedFrameTime);
                        RenderEventRef e = new RenderEventRef(new Rect(0, 0, size.Width, size.Height), f);

                        this.RenderTarget.BeginDraw();
                        this.RenderTarget.Clear(new ColorF() { A = 0f, B = 0, G = 0, R = 0 });

                        this.Direct2DContext.BeginDraw();
                        Win32Platform.DisplayContextStack.Push(this.Direct2DContext);
                        try
                        {
                            hWnd.GetClientRect(out var rect);

                            this.Win32Window.Render(e);
                            this.Direct2DContext.EndDraw();
                        }
                        finally
                        {
                            Win32Platform.DisplayContextStack.Pop();
                        }

                        this.RenderTarget.EndDraw();

                        this.SwapChain1.Present(0, 0);

                        hWnd.EndPaint(ref ps);
                    }
                }

                result = 1;
                return true;
            }

            return base.HandleOnMessage(hWnd, uMsg, wParam, lParam, out result);
        }
    
        public override void CheckForCompositionFrame()
        {
            if (this.DCompDevice != null)
            {
                var frameStats = this.DCompDevice.GetFrameStatistics();
                var lastFrameTime = TimeSpan.FromSeconds(frameStats.LastFrameTime / (double)frameStats.TimeFrequency);
                var currentTime = TimeSpan.FromSeconds(frameStats.CurrentTime / (double)frameStats.TimeFrequency);
                var nextEstimatedFrameTime = TimeSpan.FromSeconds(frameStats.NextEstimatedFrameTime / (double)frameStats.TimeFrequency);

                if (lastFrameTime != this.LastFrameTime)
                {
                    FrameEventRef animationFrame = new FrameEventRef(this.LastNextEstimatedFrameTime, nextEstimatedFrameTime);

                    this.LastFrameTime = lastFrameTime;
                    this.LastNextEstimatedFrameTime = this.NextEstimatedFrameTime;
                    this.NextEstimatedFrameTime = nextEstimatedFrameTime;

                    this.Win32Window.OnAnimationFrame(animationFrame);
                }
            }

            base.CheckForCompositionFrame();
        }
    }
}

