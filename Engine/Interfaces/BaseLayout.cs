namespace Engine.Interfaces
{
    public abstract class BaseLayout
    {
        public IUIManager UIManager { get; set; }
        public BaseLayoutView LayoutView { get; set; }
        public IScreen Screen { get; set; }
        public LayoutPosition LayoutPosition { get; set; }
        public int Width { get; set; }
        public int Height { get; set; } 
        public bool AlwaysTick { get; set; }
        public ScreenOrientation ScreenOrientation { get; set; }


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
                        this.active = true;
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


        public BaseLayout SetLayout(BaseLayoutView layoutView)
        {
            LayoutView = layoutView;
            layoutView.TouchManager = new TouchManager(Screen.ScreenManager.Client);
            layoutView.Layout = this;
            return this;
        }
        public BaseLayout Offset(int x, int y)
        {
            LayoutPosition.Offset.X = x;
            LayoutPosition.Offset.Y = y;
            return this;
        }

        public BaseLayout LeftOf(BaseLayout layout)
        {
            LayoutPosition.Right = layout;
            layout.LayoutPosition.Left = this;
            return this;
        }

        public BaseLayout RightOf(BaseLayout layout)
        {
            LayoutPosition.Left = layout;
            layout.LayoutPosition.Right = this;
            return this;
        }

        public BaseLayout Above(BaseLayout layout)
        {
            LayoutPosition.Bottom = layout;
            layout.LayoutPosition.Top = this;
            return this;
        }

        public BaseLayout Below(BaseLayout layout)
        {
            LayoutPosition.Top = layout;
            layout.LayoutPosition.Bottom = this;
            return this;
        }

        public BaseLayout MakeActive()
        {
            Active = true;
            Screen.ChangeLayout(this);
            return this;
        }

        public BaseLayout ForceTick()
        {
            AlwaysTick = true;
            return this;
        }

        public BaseLayout SetScreenOrientation(ScreenOrientation orientation)
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
    }
}