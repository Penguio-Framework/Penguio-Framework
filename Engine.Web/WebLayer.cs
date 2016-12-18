using System;
using System.Collections.Generic;
using System.Html;
using System.Runtime.CompilerServices;
using Engine.Interfaces;

namespace Engine.Web
{
    public class WebLayer : ILayer
    {
        private readonly WebRenderer renderer;
        private readonly int width;
        private readonly int height;
        public ILayout Layout { get; set; }

        [IntrinsicProperty]
        public CanvasInformation CanvasInformation { get; set; }
        public WebLayer(WebRenderer renderer, int width, int height, ILayout layout)
        {
            this.renderer = renderer;
            this.width = width;
            this.height = height;
            Layout = layout;
            CanvasInformation = CanvasInformation.Create(width, height);
            settingsStack = new List<WebContextSettings>();

            settingsStack.Add(new WebContextSettings());
        }


        public void SetDrawingTransparency(double alpha)
        {
            CanvasInformation.Context.GlobalAlpha = alpha;
        }

        public void Begin()
        {
            //not needed for web
        }
        public void End()
        {
            //not needed for web
        }

        private List<WebContextSettings> settingsStack;

        public void Save()
        {
            settingsStack.Add(settingsStack.Last().Clone());
            CanvasInformation.Context.Save();
        }
        private WebContextSettings CurrentSettings()
        {
            return settingsStack.Last();
        }
        public void Restore()
        {
            settingsStack.RemoveAt(settingsStack.Count - 1);
            CanvasInformation.Context.Restore();

        }

        public void Translate(PointF point)
        {
            CanvasInformation.Context.Translate(point.X,point.Y);
        }

        public void Translate(double x, double y)
        {
            CanvasInformation.Context.Translate(x, y);
        } 

        public void DrawImage(IImage image)
        {
            DrawImage(image, 0, 0);
        }
         
        public void DrawImage(IImage image, PointF point)
        {
            DrawImage(image, point.X, point.Y);
        }

        public void DrawImage(IImage image, double x, double y)
        {
            DrawImage(image, x, y, false);
        }

        public void DrawImage(IImage image, PointF point, bool center)
        {
            DrawImage(image, point.X, point.Y, center);
        }

        public void DrawImage(IImage image, double x, double y, bool center)
        {
            CanvasInformation.Context.Save();
            image = flipCanvas(image);
            if (center)
                CanvasInformation.Context.DrawImage(((WebImage)image).Image, x - image.Center.X, y - image.Center.Y);
            else
                CanvasInformation.Context.DrawImage(((WebImage)image).Image, x, y);
            CanvasInformation.Context.Restore();
        }

        public void DrawImage(IImage image, double x, double y, double width, double height)
        {
            DrawImage(image, x, y, width, height, false);
        }

        public void DrawImage(IImage image, double x, double y, double width, double height, bool center)
        {
            CanvasInformation.Context.Save();
            image = flipCanvas(image);
            if (center)
                CanvasInformation.Context.DrawImage(((WebImage)image).Image, x - width / 2, y - height/ 2, width, height);
            else
                CanvasInformation.Context.DrawImage(((WebImage)image).Image, x, y, width, height);
            CanvasInformation.Context.Restore();

        }

        private IImage flipCanvas(IImage image)
        {
            var wi = (WebImage) image;
            wi = wi.FlipVersion(CurrentSettings().DrawingEffects);

            return wi;
        }

        public void DrawImage(IImage image, double x, double y, double angle, double centerX, double centerY)
        {
            DrawImage(image, x, y, angle, image.Width, image.Height, centerX, centerY);
        }

        public void DrawImage(IImage image, double x, double y, double angle, bool center)
        {
            if (center)
            {
                DrawImage(image, x, y, angle, image.Center.X, image.Center.Y);
            }
            else
            {
                DrawImage(image, x, y, angle, 0, 0);
            }

        }
        public void DrawImage(IImage image, double x, double y, double angle, double width, double height, bool center)
        {
            if (center)
            {
                DrawImage(image, x, y, angle, width, height, width / 2, height / 2);
            }
            else
            {
                DrawImage(image, x, y, angle, width, height, 0, 0);
            }
        }

        public void DrawImage(IImage image, double x, double y, double angle, double width, double height, PointF center)
        {
            DrawImage(image, x, y, angle, width, height, center.X, center.Y);
        }


        public void DrawImage(IImage image, double x, double y, double angle, double width, double height, double centerX, double centerY)
        {
            var xnaContextSettings = CurrentSettings();

            var destination = new Rectangle((int)(x), (int)(y), (int)width, (int)height);
            Save();

            image = flipCanvas(image);
            CanvasInformation.Context.Translate(destination.X, destination.Y);
//            CanvasInformation.Context.Translate(centerX, centerY);
            CanvasInformation.Context.Rotate(angle);
            CanvasInformation.Context.Translate(-centerX, -centerY);
   
            CanvasInformation.Context.DrawImage(((WebImage)image).Image, 0,0,destination.Width,destination.Height);
            Restore();
        }

        public void DrawImage(IImage image, double x, double y, double angle, PointF center)
        {
            DrawImage(image, x, y, angle, center.X, center.Y);
        }
        public void DrawImage(IImage image, PointF point, double angle, PointF center)
        {
            DrawImage(image, point.X, point.Y, angle, center.X, center.Y);
        }


        public void DrawString(IFont font, string text, Point point, bool center = true)
        {
            DrawString(font, text, point.X, point.Y, center);
        }


        public void DrawString(IFont font, string text, Point point, Color color, bool center = true)
        {
            DrawString(font, text, point.X, point.Y, color, center);
        }

        public void DrawString(IFont font, string text, int x, int y, bool center = true)
        {
            DrawString(font, text, x, y, new Color(255,255,255), center);
        }

        public void DrawString(IFont font, string text, int x, int y, Color color, bool center = true)
        {
            if (font == null) return;
            PointF  position = new PointF(x, y);
            if (center)
            {
                ((WebSpriteFont)font).DrawCenteredString(CanvasInformation.Context, text, position, color, PointF.Zero);
            }
            else
            {
                ((WebSpriteFont)font).DrawString(CanvasInformation.Context, text, position, color, PointF.Zero);
            }
        }
        public PointF MeasureString(IFont font, string text)
        {
            if (font == null) return null;

            var m = ((WebSpriteFont)font).MeasureString(text);
            return m;
        }
        public void Clear()
        {
            CanvasInformation.Context.ClearRect(0, 0, width, height);
        }

        public void StrokeRectangle(Color color, int x, int y, int width, int height, int strokeSize, bool center = false)
        {
            CanvasInformation.Context.Save();
            CanvasInformation.Context.StrokeStyle = string.Format("rgba({0},{1},{2},{3})", color.R, color.G, color.B, color.A / 255f);
            if (center)
            {
                CanvasInformation.Context.StrokeRect(x - width / 2, y - height / 2, width, height);
            }
            else
            {
                CanvasInformation.Context.StrokeRect(x, y, width, height);
            }
            CanvasInformation.Context.Restore();
        }

        public void DrawRectangle(Color color, Rectangle rectangle, bool center = false)
        {
            DrawRectangle(color, rectangle.X, rectangle.Y, rectangle.Width, rectangle.Height,  center);
        }

        public void StrokeRectangle(Color color, Rectangle rectangle, int strokeSize, bool center = false)
        {
            StrokeRectangle(color, rectangle.X, rectangle.Y, rectangle.Width, rectangle.Height,strokeSize, center);
        }

        public void BoxShadow(Color color, int x, int y, int width, int height, int shadowLength, bool center = false)
        {
             
        }

        public void BoxShadow(Color color, Rectangle rectangle, int shadowLength, bool center = false)
        { 
        }


        public void DrawRectangle(Color color, int x, int y, int width, int height, bool center = false)
        {
            CanvasInformation.Context.Save();
            CanvasInformation.Context.FillStyle = string.Format("rgba({0},{1},{2},{3})", color.R, color.G, color.B, color.A / 255f);
            if (center)
            {
                CanvasInformation.Context.FillRect(x - width / 2, y - height / 2, width, height);
            }
            else
            {
                CanvasInformation.Context.FillRect(x, y, width, height);
            }
            CanvasInformation.Context.Restore();
        }

        public void SetDrawingEffects(DrawingEffects drawingEffects)
        {
            CurrentSettings().DrawingEffects = drawingEffects;
        }
    }
    public class WebContextSettings
    {
        public DrawingEffects DrawingEffects { get; set; }

        public WebContextSettings Clone()
        {
            return new WebContextSettings()
            {
                DrawingEffects = this.DrawingEffects

            };
        }
    }

}