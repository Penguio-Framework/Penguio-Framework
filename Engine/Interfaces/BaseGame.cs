namespace Engine.Interfaces
{
    public abstract class BaseGame
    {
        public BaseClient Client { get; set; }
        public AssetManager AssetManager { get; set; }
        public IRenderer Renderer { get; set; }
        public IScreenManager ScreenManager { get; set; }
        public abstract void InitScreens();
        public abstract void LoadAssets();
        public abstract void BeforeTick();
        public abstract void AfterTick();
        public abstract void BeforeDraw();
        public abstract void AfterDraw();
    }
}