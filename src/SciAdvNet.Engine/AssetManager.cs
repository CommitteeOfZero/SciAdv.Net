using System.IO;

namespace SciAdvNet.Engine
{
    public class AssetManager
    {
        private readonly IRenderer _renderer;

        internal AssetManager(IRenderer renderer)
        {
            _renderer = renderer;
        }

        public T LoadAsset<T>(string assetName, Stream stream)
            where T: Asset
        {
            ThrowIfNotSupported(assetName);
            if (typeof(T) == typeof(Texture))
            {
                return LoadTexture(assetName, stream) as T;
            }

            return null;
        }

        private void ThrowIfNotSupported(string assetName)
        {
            string extension = Path.GetExtension(assetName);
            switch (extension)
            {
                case ".jpg":
                case ".png":
                case ".ogg":
                    break;

                default:
                    throw new InvalidDataException($"Unsupported file format: '{extension}'");
            }
        }

        private Texture LoadTexture(string fileName, Stream stream)
        {
            return _renderer.LoadTexture(fileName, stream);
        }
    }
}
