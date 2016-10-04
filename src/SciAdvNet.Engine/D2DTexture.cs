using SharpDX.Direct2D1;

namespace SciAdvNet.Engine
{
    public sealed class D2DTexture : Texture
    {
        public D2DTexture(string fileName, Bitmap1 d2dBitmap)
            : base(fileName, d2dBitmap.PixelSize.Width, d2dBitmap.PixelSize.Height)
        {
            D2DBitmap = d2dBitmap;
        }

        public Bitmap1 D2DBitmap { get; }
    }
}
