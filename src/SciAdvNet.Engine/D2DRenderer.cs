using SharpDX;
using SharpDX.Direct2D1;
using SharpDX.Direct3D;
using SharpDX.Direct3D11;
using SharpDX.DirectWrite;
using SharpDX.DXGI;
using SharpDX.WIC;
using System.Collections.Generic;
using System.IO;

namespace SciAdvNet.Engine
{
    public class D2DRenderer : IRenderer
    {
        private bool _isDisposed;
        private SharpDX.Direct3D11.Device1 _d3dDevice;

        private SharpDX.Direct2D1.Device _d2dDevice;
        private SharpDX.Direct2D1.Factory1 _d2dFactory;
        private SharpDX.Direct2D1.DeviceContext _deviceContext;

        private SharpDX.DXGI.SwapChain1 _swapChain;
        private SharpDX.Direct2D1.Bitmap1 _d2dTargetBitmap;

        private SharpDX.DirectWrite.Factory1 _dwriteFactory;
        private ImagingFactory _wicFactory;
        private FormatConverter _formatConverter;

        private TextFormat _textFormat;
        private readonly SolidColorBrush _blackBrush;
        private readonly SolidColorBrush _whiteBrush;

        private List<Visual> _visuals;

        public D2DRenderer(GameWindow gameWindow)
        {
            InitializeDirectX(gameWindow);

            _visuals = new List<Visual>();

            _textFormat = new TextFormat(_dwriteFactory, "Calibri", 70);
            _blackBrush = new SolidColorBrush(_deviceContext, SharpDX.Color.Black);
            _whiteBrush = new SolidColorBrush(_deviceContext, SharpDX.Color.White);
        }

        private void InitializeDirectX(GameWindow gameWindow)
        {
            using (var defaultDevice = new SharpDX.Direct3D11.Device(DriverType.Hardware, DeviceCreationFlags.BgraSupport))
            {
                _d3dDevice = defaultDevice.QueryInterface<SharpDX.Direct3D11.Device1>();
            }

            var swapChainDesc = new SwapChainDescription1()
            {
                Width = 0,
                Height = 0,
                Format = Format.B8G8R8A8_UNorm,
                BufferCount = 2,
                Usage = Usage.BackBuffer | Usage.RenderTargetOutput,
                SwapEffect = SwapEffect.FlipSequential,
                SampleDescription = new SampleDescription(1, 0),
                Scaling = Scaling.AspectRatioStretch
            };

            using (var dxgiDevice = _d3dDevice.QueryInterface<SharpDX.DXGI.Device2>())
            using (var dxgiFactory = dxgiDevice.Adapter.GetParent<SharpDX.DXGI.Factory2>())
            {
                var window = new ComObject(gameWindow.WindowObject);
                _swapChain = new SwapChain1(dxgiFactory, _d3dDevice, window, ref swapChainDesc);
                _d2dFactory = new SharpDX.Direct2D1.Factory1();
                _d2dDevice = new SharpDX.Direct2D1.Device(_d2dFactory, dxgiDevice);
            }

            _deviceContext = new SharpDX.Direct2D1.DeviceContext(_d2dDevice, new DeviceContextOptions());
            using (var surface = Surface.FromSwapChain(_swapChain, 0))
            {
                var pixelFormat = new SharpDX.Direct2D1.PixelFormat(Format.Unknown, SharpDX.Direct2D1.AlphaMode.Premultiplied);
                var bitmapProperties = new BitmapProperties1(pixelFormat, 0, 0, BitmapOptions.Target | BitmapOptions.CannotDraw);

                _d2dTargetBitmap = new SharpDX.Direct2D1.Bitmap1(_deviceContext, surface, bitmapProperties);
                _deviceContext.Target = _d2dTargetBitmap;
            }

            _dwriteFactory = new SharpDX.DirectWrite.Factory1();
            _wicFactory = new ImagingFactory();
            _formatConverter = new FormatConverter(_wicFactory);
            _deviceContext.TextAntialiasMode = SharpDX.Direct2D1.TextAntialiasMode.Cleartype;
        }

        public Texture LoadTexture(string fileName, Stream stream)
        {
            using (var bitmapDecoder = new BitmapDecoder(_wicFactory, stream, DecodeOptions.CacheOnDemand))
            using (stream)
            {
                var frame = bitmapDecoder.GetFrame(0);
                var converter = new FormatConverter(_wicFactory);
                converter.Initialize(frame, SharpDX.WIC.PixelFormat.Format32bppPRGBA);

                var bitmap = SharpDX.Direct2D1.Bitmap1.FromWicBitmap(_deviceContext, converter);
                var size = bitmap.PixelSize;
                return new D2DTexture(fileName, bitmap);
            }
        }

        public void DrawTexture(TextureVisual visual)
        {
            var d2dTexture = visual.Texture as D2DTexture;
            _deviceContext.DrawBitmap(d2dTexture.D2DBitmap, 1.0f, SharpDX.Direct2D1.InterpolationMode.Linear);
        }

        public void DrawRectangle(RectangleVisual visual)
        {
            var brush = new SolidColorBrush(_deviceContext, new SharpDX.Color(visual.Color.R, visual.Color.G, visual.Color.B));
            _deviceContext.FillRectangle(new RectangleF(visual.X, visual.Y, visual.Width, visual.Height), brush);
        }

        public void AddVisual(Visual visual)
        {
            _visuals.Add(visual);
        }

        public void RenderFrame()
        {
            if (!_isDisposed)
            {
                _deviceContext.BeginDraw();
                _deviceContext.Clear(SharpDX.Color.White);

                foreach (var visual in _visuals)
                {
                    visual.Render(this);
                }

                _deviceContext.EndDraw();
                _swapChain.Present(1, PresentFlags.None);
            }
        }

        public void Dispose()
        {
            _isDisposed = true;

            _formatConverter.Dispose();
            _wicFactory.Dispose();
            _dwriteFactory?.Dispose();
            _d2dFactory?.Dispose();
            _deviceContext?.Dispose();
            _d2dTargetBitmap?.Dispose();
            _d2dDevice?.Dispose();
            _swapChain?.Dispose();
            _d3dDevice?.Dispose();
        }
    }
}
