namespace Engine.Interfaces
{
    public interface IScreenView
    {
        BaseLayoutView LayoutView { get; set; }
        int Width { get; set; }
        int Height { get; set; }
    }
}