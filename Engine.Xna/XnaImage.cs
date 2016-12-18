using Engine.Interfaces;
using Microsoft.Xna.Framework.Graphics;

namespace Engine.Xna
{
    public class XnaImage : IImage
    {
        public XnaImage(Texture2D texture, PointF center)
        {
            Texture = texture;
            Center = center ?? new PointF(texture.Width/2d, texture.Height/2d);
            Width = texture.Width;
            Height = texture.Height;
            SourceRectangle = new Microsoft.Xna.Framework.Rectangle(0, 0, Width, Height);
        }

        public Microsoft.Xna.Framework.Rectangle SourceRectangle { get; private set; }

        public Texture2D Texture { get; set; }

        public PointF Center { get; private set; }
        public int Width { get; private set; }
        public int Height { get; private set; }
    }
}