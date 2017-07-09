using System;
using System.Collections.Generic;
using Engine.Interfaces;

namespace Engine.Xna
{
    public class XnaScreenManager : IScreenManager
    {
        public XnaScreenManager(XnaRenderer renderer, BaseClient client)
        {
            Renderer = renderer;
            Client = client;
            XnaScreens = new List<XnaScreen>();
        }

        public List<XnaScreen> XnaScreens { get; set; }

        public XnaRenderer Renderer { get; set; }
        public BaseClient Client { get; set; }

        public IScreen CreateScreen()
        {
            var xnaScreen = new XnaScreen(this);
            XnaScreens.Add(xnaScreen);
            return xnaScreen;
        }

        public IScreen CurrentScreen { get; set; }

        public IEnumerable<IScreen> Screens
        {
            get { return XnaScreens; }
        }

        public void Draw(TimeSpan elapsedGameTime)
        {
            var size = CurrentScreen.GetLayoutSize();
            if (Renderer.graphics.PreferredBackBufferWidth != size.Width ||
                Renderer.graphics.PreferredBackBufferHeight != size.Height)
            {
                //                Renderer.graphics.PreferredBackBufferWidth = size.Width;
                //                Renderer.graphics.PreferredBackBufferHeight = size.Height;
                //                Renderer.graphicsDevice.Viewport = new Viewport(0, 0, size.Width, size.Height);
                //                Renderer.graphics.ApplyChanges();
            }

            Renderer.graphicsDevice.Clear(Microsoft.Xna.Framework.Color.Black);
            Renderer.BeginRender();
            CurrentScreen.Draw(elapsedGameTime);
            Renderer.EndRender();
        }

        public void TouchEvent(TouchType touchType, int x, int y)
        {
            CurrentScreen.TouchEvent(touchType, x, y);
        }

        private TimeSpan lastElapsedTime;
        private readonly List<Tuple<Action, TimeSpan>> timeouts = new List<Tuple<Action, TimeSpan>>();
        private readonly List<Tuple<Action, TimeSpan, TimeSpan>> intervals = new List<Tuple<Action, TimeSpan, TimeSpan>>();

        public void Timeout(Action callback, int ms)
        {
            var cur = lastElapsedTime.Add(new TimeSpan(0, 0, 0, 0, ms));
            timeouts.Add(new Tuple<Action, TimeSpan>(callback, cur));
        }

        public void Interval(Action callback, int ms)
        {
            var timeSpan = new TimeSpan(0, 0, 0, 0, ms);
            var cur = lastElapsedTime.Add(timeSpan);
            intervals.Add(new Tuple<Action, TimeSpan, TimeSpan>(callback, cur, timeSpan));
        }

        public void Init()
        {
            CurrentScreen.Init();
        }

        public void Tick(TimeSpan elapsedGameTime)
        {
            lastElapsedTime = elapsedGameTime;
            for (var i = timeouts.Count - 1; i >= 0; i--)
            {
                if (timeouts[i].Item2 < elapsedGameTime)
                {
                    timeouts[i].Item1();
                    timeouts.RemoveAt(i);
                }
            }

            for (var i = intervals.Count - 1; i >= 0; i--)
            {
                if (intervals[i].Item2 < elapsedGameTime)
                {
                    intervals[i].Item1();
                    intervals[i].Item2.Add(intervals[i].Item3);
                }
            }

            Renderer.ClearScaleMatrix();
            CurrentScreen.Tick(elapsedGameTime);
        }

        public Size GetScreenSize()
        {
            return CurrentScreen.GetLayoutSize();
        }

        public void ChangeScreen(IScreen screen)
        {
            if (CurrentScreen != null)
            {
                CurrentScreen.Destroy();
            }
            CurrentScreen = screen;
            Init();
        }
    }
}