namespace SciAdvNet.Engine
{
    public sealed class RectangleVisual : Visual
    {
        public RectangleVisual(int x, int y, int width, int height, Color color)
            : base(x, y, width, height)
        {
            Color = color;
        }

        public Color Color { get; }

        internal override void Render(IRenderer renderer)
        {
            renderer.DrawRectangle(this);
        }
    }
}
