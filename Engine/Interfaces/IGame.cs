namespace Engine.Interfaces
{
    public interface IGame
    {
        IClient Client { get; set; }
        AssetManager AssetManager { get; set; }
        void InitScreens(IRenderer renderer, IScreenManager screenManager);
        void LoadAssets(IRenderer renderer);

        void BeforeTick();
        void AfterTick();

        void BeforeDraw();
        void AfterDraw();
    }
}