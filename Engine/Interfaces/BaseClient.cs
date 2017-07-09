using System;

namespace Engine.Interfaces
{
    public abstract class BaseClient
    {
        protected BaseClient(BaseGame game, IClientSettings clientSettings, IUserPreferences userPreferences)
        {
            Game = game;
            ClientSettings = clientSettings;
            UserPreferences = userPreferences;
        }

        public BaseGame Game { get; set; }
        public IScreenManager ScreenManager { get; set; }
        public IClientSettings ClientSettings { get; set; }
        public DragGestureManager DragDragGestureManager { get; set; }
        public abstract bool SoundEnabled { get; set; }
        public IUserPreferences UserPreferences { get; set; }
        public abstract void LoadAssets(IRenderer renderer);
        public abstract void Init(IRenderer renderer);
        public abstract void Draw(TimeSpan elapsedGameTime);
        public abstract void TouchEvent(TouchType touchType, int x, int y);
        public abstract void Tick(TimeSpan elapsedGameTime);
        public abstract void Timeout(Action callback, int ms);
        public abstract void Interval(Action callback, int ms);
        public abstract void PlaySong(ISong isong);
        public abstract ISoundEffect PlaySoundEffect(ISoundEffect isfx, bool repeat = false);
        public abstract ISoundEffect CreateSoundEffect(string soundPath);
        public abstract ISong CreateSong(string songPath);

        public abstract void DrawLetterbox();
        public abstract void SetCustomLetterbox(IImage image);
    }
}