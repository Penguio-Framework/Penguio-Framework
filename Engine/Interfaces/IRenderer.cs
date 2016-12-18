namespace Engine.Interfaces
{
    public interface IRenderer
    {
        ILayer CreateLayer(int width, int height, ILayout layout);
        ILayer CreateLayer(ILayout layout);
        void AddLayer(ILayer layer);
        IImage GetImage(string imageName);
        IImage CreateImage(string imageName, string imagePath, PointF center = null);
        IFont CreateFont(string fontName, string fontPath);
        IFont GetFont(string fontName);
    }
}