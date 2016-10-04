using System;
using System.IO;

namespace SciAdvNet.Engine
{
    public interface IRenderer : IDisposable
    {
        Texture LoadTexture(string fileName, Stream stream);
        void DrawTexture(TextureVisual visual);
        void DrawRectangle(RectangleVisual visual);
    }
}
