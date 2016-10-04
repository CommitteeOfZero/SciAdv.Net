namespace SciAdvNet.Engine
{
    public abstract class Texture : Asset
    {
        protected Texture(string fileName, int width, int height)
            : base(fileName)
        {
            Width = width;
            Height = height;
        }

        public int Width { get; }
        public int Height { get; }
    }
}
