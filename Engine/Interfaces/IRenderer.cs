namespace Engine.Interfaces
{
    public interface IRenderer
    {
        ILayer CreateLayer(int width, int height, ILayout layout);
        ILayer CreateLayer(ILayout layout);
        void AddLayer(ILayer layer);
        IImage CreateImage(string imagePath, PointF center = null);
        IFont CreateFont( string fontPath);
    }
}