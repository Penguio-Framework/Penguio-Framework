using System;
using System.Collections.Generic;

namespace Engine.Interfaces
{
    public interface IScreenManager
    {
        BaseClient Client { get; set; }
        IScreen CurrentScreen { get; set; }
        IScreen CreateScreen();
        IEnumerable<IScreen> Screens { get; }
        void Draw(TimeSpan elapsedGameTime);
        void TouchEvent(TouchType touchType, int x, int y);
        void Tick(TimeSpan elapsedGameTime);
        Size GetScreenSize();
        void SetDefaultScreenSize(int width, int height);
        IScreen CreateDefaultScreenLayout(BaseLayoutView layoutView);
        void ChangeScreen(IScreen screen);
        void Timeout(Action callback, int ms);
        void Interval(Action callback, int ms);
        void Init();
    }
}