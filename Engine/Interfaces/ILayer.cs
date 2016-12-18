using System.ComponentModel;

namespace Engine.Interfaces
{
    public interface ILayer
    {
        ILayout Layout { get; set; }
        void Save();
        void Restore();
        void Translate(PointF point);
        void Translate(double x, double y);
        void DrawImage(IImage image);
        void DrawImage(IImage image, PointF point);
        void DrawImage(IImage image, double x, double y);
        void DrawImage(IImage image, PointF point, bool center);
        void DrawImage(IImage image, double x, double y, bool center);
        void DrawImage(IImage image, double x, double y, double width, double height);
        void DrawImage(IImage image, double x, double y, double width, double height, bool center);
        void DrawImage(IImage image, double x, double y, double angle, double centerX, double centerY);
        void DrawImage(IImage image, double x, double y, double angle, bool center);
        void DrawImage(IImage image, double x, double y, double angle, double width, double height, bool center);
        void DrawImage(IImage image, double x, double y, double angle, double width, double height, PointF center);
        void DrawImage(IImage image, double x, double y, double angle, double width, double height, double centerX, double centerY);
        void DrawImage(IImage image, PointF point, double angle, PointF center);

        void DrawString(IFont fontName , string text, Point point, bool center = true);

        void DrawString(IFont fontName, string text, Point point, Color color, bool center = true);

        void DrawString(IFont fontName ,string text, int x, int y, bool center = true);

        void DrawString(IFont fontName ,string text, int x, int y, Color color, bool center = true);

        void Clear();
        PointF MeasureString(IFont fontName ,string text);
        void DrawRectangle(Color color, int x, int y, int width, int height, bool center = false);
        void DrawRectangle(Color color, Rectangle rectangle, bool center = false);

        void StrokeRectangle(Color color, int x, int y, int width, int height, int strokeSize, bool center = false);
        void StrokeRectangle(Color color, Rectangle rectangle, int strokeSize, bool center = false);

        void BoxShadow(Color color, int x, int y, int width, int height, int shadowLength, bool center = false);
        void BoxShadow(Color color, Rectangle rectangle, int shadowLength, bool center = false);

        void SetDrawingEffects(DrawingEffects drawingEffects);
        void SetDrawingTransparency(double alpha);
        void Begin();
        void End();
    }
}