namespace Engine.Interfaces
{
    public interface IRenderer
    {
        ILayer CreateLayer(int width, int height, BaseLayout layout);
        ILayer CreateLayer(BaseLayout layout);
        void AddLayer(ILayer layer);
        IImage CreateImage(string imagePath, PointF center = null);
        IFont CreateFont( string fontPath);
    }
}