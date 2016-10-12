using SharpDX;
using SharpDX.Direct2D1;
using SharpDX.DirectWrite;
using SharpDX.WIC;
using System;
using System.IO;
using System.Linq;

namespace FontEditor
{
    public sealed class GlyphRenderer : RendererBase
    {
        private const int ColumnCount = 64;
        private const int CellWidth = 48;
        private const int CellHeight = 48;

        private const int FontSize = 48;

        private FontCollection _systemFonts;
        private Guid _imageFormat;

        public GlyphRenderer()
            : base()
        {
            _systemFonts = DWriteFactory.GetSystemFontCollection(false);
        }

        public Stream InsertGlyphs(Stream fontImageStream, string fontFamily, string characters, int originY)
        {
            using (var fontImage = DecodeImage(fontImageStream))
            {
                DoInsert(fontImage, fontFamily, characters, originY);
                return EncodeBitmap(fontImage, _imageFormat);
            }
        }

        private void DoInsert(Bitmap1 target, string fontFamily, string characters, int originY)
        {
            DeviceContext.Target = target;
            using (var font = GetSystemFont(fontFamily))
            {
                DeviceContext.BeginDraw();
#if DEBUG
                DrawGridLines();
#endif
                DeviceContext.Transform = Matrix3x2.Translation(0, originY);
                for (int i = 0; i < characters.Length; i += ColumnCount)
                {
                    int currentRowLength = Math.Min(ColumnCount, characters.Length - i);
                    string currentRow = characters.Substring(i, currentRowLength);

                    using (var glyphRun = new GlyphRun())
                    {
                        glyphRun.FontFace = new FontFace(font);
                        glyphRun.FontSize = FontSize;
                        glyphRun.Indices = glyphRun.FontFace.GetGlyphIndices(currentRow.Select(c => (int)c).ToArray());
                        glyphRun.Advances = Enumerable.Repeat<float>(CellWidth, currentRowLength).ToArray();

                        var baselineOrigin = new Vector2(1.0f, 41.0f);
                        DeviceContext.PushAxisAlignedClip(new RectangleF(0, 0, CellWidth * ColumnCount, CellHeight), AntialiasMode.PerPrimitive);
                        DeviceContext.DrawGlyphRun(baselineOrigin, glyphRun, WhiteBrush, MeasuringMode.Natural);
                        DeviceContext.PopAxisAlignedClip();
                    }

                    var transform = Matrix3x2.Multiply(DeviceContext.Transform, Matrix3x2.Translation(0, CellHeight));
                    DeviceContext.Transform = transform;
                }

                DeviceContext.EndDraw();
            }

            DeviceContext.Target = null;
        }

        private void DrawGridLines()
        {
            int rowCount = DeviceContext.PixelSize.Height / CellHeight;
            for (int row = 0; row < rowCount; row++)
            {
                for (int col = 0; col < ColumnCount; col++)
                {
                    DeviceContext.DrawRectangle(new RectangleF(col * CellWidth, row * CellHeight, CellWidth, CellHeight), RedBrush);
                }
            }
        }

        private Font GetSystemFont(string fontFamilyName)
        {
            int fontFamilyIndex;
            bool found = _systemFonts.FindFamilyName(fontFamilyName, out fontFamilyIndex);
            if (!found)
            {
                throw new ArgumentException("Couldn't find the specified font.");
            }

            using (var fontFamily = _systemFonts.GetFontFamily(fontFamilyIndex))
            {
                return fontFamily.GetFirstMatchingFont(FontWeight.Normal, FontStretch.Normal, FontStyle.Normal);
            }
        }

        private Bitmap1 DecodeImage(Stream imageStream)
        {
            using (var bitmapDecoder = new BitmapDecoder(WicFactory, imageStream, DecodeOptions.CacheOnDemand))
            using (var converter = new FormatConverter(WicFactory))
            {
                _imageFormat = bitmapDecoder.ContainerFormat;
                var frame = bitmapDecoder.GetFrame(0);
                converter.Initialize(frame, WicPixelFormat);

                var props = new BitmapProperties1()
                {
                    BitmapOptions = BitmapOptions.Target,
                    PixelFormat = Direct2DPixelFormat
                };

                return SharpDX.Direct2D1.Bitmap1.FromWicBitmap(DeviceContext, converter, props);
            }
        }

        private Stream EncodeBitmap(Bitmap1 bitmap, Guid containerFormat)
        {
            using (var bitmapEncoder = new BitmapEncoder(WicFactory, containerFormat))
            {
                var memoryStream = new MemoryStream();
                bitmapEncoder.Initialize(memoryStream);
                using (var frameEncode = new BitmapFrameEncode(bitmapEncoder))
                {
                    frameEncode.Initialize();
                    var wicPixelFormat = WicPixelFormat;
                    frameEncode.SetPixelFormat(ref wicPixelFormat);

                    var imageParams = new ImageParameters()
                    {
                        PixelFormat = Direct2DPixelFormat,
                        DpiX = 96,
                        DpiY = 96,
                        PixelWidth = bitmap.PixelSize.Width,
                        PixelHeight = bitmap.PixelSize.Height
                    };

                    WicImageEncoder.WriteFrame(bitmap, frameEncode, imageParams);

                    frameEncode.Commit();
                    bitmapEncoder.Commit();
                }

                memoryStream.Position = 0;
                return memoryStream;
            }
        }

        public override void Dispose()
        {
            _systemFonts.Dispose();
            base.Dispose();
        }
    }
}
