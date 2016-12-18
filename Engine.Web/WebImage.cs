using System;
using System.Collections.Generic;
using System.Html;
using System.Runtime.CompilerServices;
using Engine.Interfaces;

namespace Engine.Web
{
    public class WebImage : IImage
    {
        [IntrinsicProperty]
        public ImageElement Image { get; set; }

        public WebImage(string imagePath, PointF center, Action ready)
        {
            ImageElement image = new ImageElement();
            image.Src = imagePath;
            Image = image;
            image.OnLoad = (e) =>
            {
                Width = (int)image.Width;
                Height = (int)image.Height;
                Center = center ?? new PointF((image.Width / 2f), image.Height / 2f);

                ready();
            };
        }
        public WebImage(CanvasElement canvas, PointF center)
        {

            ImageElement image = new ImageElement();
            image.Src = canvas.ToDataURL();
            Image = image;

            Width = (int)canvas.Width;
            Height = (int)canvas.Height;
            Center = center ?? new PointF((Width / 2f), Height / 2f);

            Image = image;
        }

        public PointF Center { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }

        private Dictionary<DrawingEffects, WebImage> flipVersions = new Dictionary<DrawingEffects, WebImage>();

        public WebImage FlipVersion(DrawingEffects drawingEffects)
        {
            if (!flipVersions.ContainsKey(drawingEffects))
            {
                CanvasInformation canvas;
                switch (drawingEffects)
                {
                    case DrawingEffects.None:

                        flipVersions.Add(DrawingEffects.None, this);                    
                        break;

                    case DrawingEffects.FlipHorizontally:

                        canvas = CanvasInformation.Create(Width, Height);
                        canvas.Context.Translate(Width, 0);
                        canvas.Context.Scale(-1, 1);
                        canvas.Context.DrawImage(Image, 0, 0);

                        flipVersions.Add(DrawingEffects.FlipHorizontally, new WebImage(canvas.Canvas, Center));

                        break;
                    case DrawingEffects.FlipVertically:
                        canvas = CanvasInformation.Create(Width, Height);
                        canvas.Context.Translate(0, Height);
                        canvas.Context.Scale(1, -1);
                        canvas.Context.DrawImage(Image, 0, 0);

                        flipVersions.Add(DrawingEffects.FlipVertically, new WebImage(canvas.Canvas, Center));
                        break;
                    default:
                        throw new ArgumentOutOfRangeException("drawingEffects");
                }
            }

            return flipVersions[drawingEffects];
        }
    }
}