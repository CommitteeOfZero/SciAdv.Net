namespace SciAdvNet.Engine
{
    public class TextureVisual : Visual
    {
        public TextureVisual(Texture texture, int x, int y, int width, int height)
            : base(x, y, width, height)
        {
            Texture = texture;
        }

        public Texture Texture { get; }

        internal override void Render(IRenderer renderer)
        {
            renderer.DrawTexture(this);
        }
    }
}
