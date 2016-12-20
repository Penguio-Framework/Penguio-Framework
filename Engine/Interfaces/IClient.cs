using System;

namespace Engine.Interfaces
{
    public interface IClient
    {
        IGame Game { get; set; }
        IScreenManager ScreenManager { get; set; }
        IClientSettings ClientSettings { get; set; }
        DragGestureManager DragDragGestureManager { get; set; }
        bool SoundEnabled { get; set; }
        IUserPreferences UserPreferences { get; }
        void LoadAssets(IRenderer renderer);
        void Init(IRenderer renderer);
        void Draw(TimeSpan elapsedGameTime);
        void TouchEvent(TouchType touchType, int x, int y);
        void Tick(TimeSpan elapsedGameTime);
        void Timeout(Action callback, int ms);
        void Interval(Action callback, int ms);
        void PlaySong(ISong isong);
        ISoundEffect PlaySoundEffect(ISoundEffect isfx, bool repeat = false);
        ISoundEffect CreateSoundEffect(string soundName, string soundPath);
        ISong CreateSong(string SongName, string SongPath);
        void DrawLetterbox();
    }
}