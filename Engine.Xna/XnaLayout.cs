using Engine.Interfaces;

namespace Engine.Xna
{
    public class XnaLayout : BaseLayout
    {

        public XnaLayout(XnaScreen screen, int width, int height)
        {
            Screen = screen;
            Width = width;
            Height = height;

            ScreenOrientation = ScreenOrientation.Vertical;
            LayoutPosition = new LayoutPosition(new Size(width, height));

            UIManager = new XnaUIManager(this);
        }
    }
}