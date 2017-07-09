using System;
using System.Collections.Generic;
using Bridge.Html5;
using Engine.Interfaces;

namespace Engine.Web
{
    public class WebScreenManager : IScreenManager
    {
        public WebScreenManager(WebRenderer renderer, BaseClient client)
        {
            Renderer = renderer;
            Client = client;
            WebScreens = new List<WebScreen>();
        }

        public List<WebScreen> WebScreens { get; set; }

        public WebRenderer Renderer { get; set; }
        public BaseClient Client { get; set; }
        public bool OneLayoutAtATime { get; set; }

        public IScreen CreateScreen()
        {
            var webScreen = new WebScreen(this);
            WebScreens.Add(webScreen);
            return webScreen;
        }

        public IScreen CurrentScreen { get; set; }
        public IEnumerable<IScreen> Screens { get { return WebScreens; } }
        public void Draw(TimeSpan elapsedGameTime)
        {
            Renderer.BeginRender();
            CurrentScreen.Draw(elapsedGameTime);

            Renderer.EndRender();

        }

        public void TouchEvent(TouchType touchType, int x, int y)
        {
            CurrentScreen.TouchEvent(touchType, x, y);
        }

        private TimeSpan lastElapsedTime;
        private List<Tuple<Action, TimeSpan>> timeouts = new List<Tuple<Action, TimeSpan>>();
        public void Timeout(Action callback, int ms)
        {

            var cur = lastElapsedTime.Add(new TimeSpan(0, 0, 0, 0, ms));
            timeouts.Add(new Tuple<Action, TimeSpan>(callback, cur));
        }

        public void Interval(Action callback, int ms)
        {
             
        }

        public void Init()
        {
            CurrentScreen.Init();
        }

        public void Tick(TimeSpan elapsedGameTime)
        {
            lastElapsedTime = elapsedGameTime;
            for (int i = timeouts.Count - 1; i >= 0; i--)
            {
                if (timeouts[i].Item2 < elapsedGameTime)
                {
                    timeouts[i].Item1();
                    timeouts.RemoveAt(i);
                }
            }
            CurrentScreen.Tick(elapsedGameTime);

        }

        public Size GetScreenSize()
        {
            return CurrentScreen.GetLayoutSize();
        }

        public Size DefaultScreenSize { get; set; }
        public void SetDefaultScreenSize(int width, int height)
        {
            DefaultScreenSize = new Size(width, height);
        }

        public IScreen CreateDefaultScreenLayout(BaseLayoutView layoutView)
        {
            var screen = CreateScreen();
            screen
                .CreateLayout(DefaultScreenSize.Width, DefaultScreenSize.Height)
                .MakeActive()
                .SetScreenOrientation(ScreenOrientation.Vertical)
                .SetLayout(layoutView);
            return screen;
        }



        public void ChangeScreen(IScreen screen)
        {

            IScreen currentScreen = CurrentScreen;

            if (currentScreen != null)
            {
                currentScreen.Destroy();
            }

            CurrentScreen = screen;
            var size = CurrentScreen.GetLayoutSize();
            Renderer.ClickManager.Style.Width = size.Width + "px";
            Renderer.ClickManager.Style.Height = size.Height + "px";
            foreach (WebLayout layout in CurrentScreen.Layouts)
            {
                layout.Element.Style.Position = Position.Absolute;
                layout.Element.Style.Left = layout.LayoutPosition.Location.X + "px";
                layout.Element.Style.Top = layout.LayoutPosition.Location.Y + "px";
                layout.Element.Style.Width = layout.LayoutPosition.Location.Width + "px";
                layout.Element.Style.Height = layout.LayoutPosition.Location.Height + "px";
            }

            Init();

        }
    }
}