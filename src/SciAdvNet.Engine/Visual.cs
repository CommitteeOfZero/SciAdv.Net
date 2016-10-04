namespace SciAdvNet.Engine
{
    public abstract class Visual
    {
        protected Visual(int x, int y, int width, int height)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
        }

        public int X { get; }
        public int Y { get; }
        public int Width { get; }
        public int Height { get; }

        internal abstract void Render(IRenderer renderer);
    }
}
