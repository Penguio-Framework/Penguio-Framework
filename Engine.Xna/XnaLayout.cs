using Engine.Interfaces;

namespace Engine.Xna
{
    public class XnaLayout : ILayout
    {
        public ILayoutView LayoutView { get; set; }
        public IScreen Screen { get; set; }
        public LayoutPosition LayoutPosition { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public ScreenOrientation ScreenOrientation { get; set; }
        public bool AlwaysTick { get; set; }

        public XnaLayout(XnaScreen screen, int width, int height)
        {
            Screen = screen;
            Width = width;
            Height = height;

            ScreenOrientation = ScreenOrientation.Vertical;
            LayoutPosition = new LayoutPosition(new Size(width, height));

            UIManager = new XnaUIManager(this);
        }

        private bool active;

        public bool Active
        {
            get { return active; }
            set
            {
                if (value)
                {
                    if (Screen.OneLayoutAtATime)
                    {
                        foreach (var layout in Screen.Layouts)
                        {
                            layout.Active = false;
                        }
                        active = true;
                    }
                    else
                    {
                        active = true;
                    }
                }
                else
                {
                    active = false;
                }
            }
        }

        public ILayout Offset(int x, int y)
        {
            LayoutPosition.Offset.X = x;
            LayoutPosition.Offset.Y = y;
            return this;
        }

        public void ForceScreenOrientation(ScreenOrientation orientation)
        {
            ScreenOrientation = orientation;
        }


        public ILayout LeftOf(ILayout layout)
        {
            LayoutPosition.Right = layout;
            layout.LayoutPosition.Left = this;
            return this;
        }

        public ILayout RightOf(ILayout layout)
        {
            LayoutPosition.Left = layout;
            layout.LayoutPosition.Right = this;
            return this;
        }

        public ILayout Above(ILayout layout)
        {
            LayoutPosition.Bottom = layout;
            layout.LayoutPosition.Top = this;
            return this;
        }

        public ILayout Below(ILayout layout)
        {
            LayoutPosition.Top = layout;
            layout.LayoutPosition.Bottom = this;
            return this;
        }

        public ILayout MakeActive()
        {
            Active = true;
            Screen.ChangeLayout(this);
            return this;
        }

        public ILayout ForceTick()
        {
            AlwaysTick = true;
            return this;
        }

        public ILayout SetScreenOrientation(ScreenOrientation orientation)
        {
            ScreenOrientation = orientation;
            return this;
        }


        public void ProcessTouchEvent(TouchType touchType, int x, int y)
        {
            if (UIManager.ProcessTouchEvent(touchType, x, y))
            {
                return;
            }
            LayoutView.TouchManager.ProcessTouchEvent(touchType, x, y);
        }


        public IUIManager UIManager { get; set; }
    }
}