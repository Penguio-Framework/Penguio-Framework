using System;
using System.Collections.Generic;
using System.Html;
using System.Html.Media.Graphics;
using System.Runtime.CompilerServices;
using Engine.Interfaces;

namespace Engine.Web
{
    public class WebRenderer : IRenderer
    {
        private readonly WebClient _client;
        private readonly Action _loaded;
        private readonly WebAssetCache assetCache;

        private List<WebLayer> layers;

        public WebRenderer(WebClient client, Action loaded)
        {
            _client = client;
            _loaded = loaded;

            layers = new List<WebLayer>();
            assetCache = new WebAssetCache(client);

            ClickManager = (DivElement)Document.CreateElement("div");
            ClickManager.ClassName = "clickManager";
            ClickManager.Style.Width = "1000px";
            ClickManager.Style.Height = "1000px";
            ClickManager.OnMousedown = (e) =>
            {
                client.TouchEvent(TouchType.TouchDown, ((dynamic)e).pageX - ((dynamic)e).target.offsetLeft, ((dynamic)e).pageY - ((dynamic)e).target.offsetTop);
            };
            ClickManager.OnMousemove = (e) =>
            {
                client.TouchEvent(TouchType.TouchMove, ((dynamic)e).pageX - ((dynamic)e).target.offsetLeft, ((dynamic)e).pageY - ((dynamic)e).target.offsetTop);
            };
            ClickManager.OnMouseup = (e) =>
            {
                client.TouchEvent(TouchType.TouchUp, ((dynamic)e).pageX - ((dynamic)e).target.offsetLeft, ((dynamic)e).pageY - ((dynamic)e).target.offsetTop);
            };
            Document.Body.AppendChild(ClickManager);
        }

        public ILayer CreateLayer(int width, int height, ILayout layout)
        {
            return new WebLayer(this, width, height, layout);
        }

        public ILayer CreateLayer(ILayout layout)
        {
            return new WebLayer(this, layout.Width, layout.Height, layout);

        }

        public void AddLayer(ILayer layer)
        {
            var webLayout = (WebLayout)layer.Layout;
            webLayout.Element.AppendChild(((WebLayer)layer).CanvasInformation.Canvas);
            layers.Add((WebLayer)layer);
        }

        public IImage GetImage(string imageName)
        {
            return assetCache.GetImage(imageName);
        }

        private int numberOfImages;
        private int numberOfImagesLoaded;
        public DivElement ClickManager { get; set; }

        public IImage CreateImage(string imageName, string imagePath, PointF center = null)
        {
            numberOfImages++;
            return assetCache.CreateImage(imageName, imagePath, center, imagesReady);
        }

        public IFont CreateFont(string fontName, string fontPath)
        {
            return assetCache.CreateFont(fontName, fontPath);

        }

        public IFont GetFont(string fontName)
        {
            return assetCache.GetFont(fontName);
        }

        private bool hasLoaded = false;

        private void imagesReady()
        {
            if (hasLoaded) return;

            numberOfImagesLoaded++;
            if (numberOfImagesLoaded == numberOfImages)
            {
                hasLoaded = true;
                _loaded();
            }
        }

        public void BeginRender()
        {
            foreach (var xnaLayer in layers)
            {
                xnaLayer.Begin();
            }
        }

        public void EndRender()
        {
            foreach (var xnaLayer in layers)
            {
                xnaLayer.End();
            }

        }

        public void DoneLoadingAssets()
        {
            if (numberOfImagesLoaded == numberOfImages)
            {
                _loaded();
            }
        }
         
    }

    public class WebSpriteFont : IFont
    {
        public IImage Font { get; set; }
        public fontMetrics FontMetrics { get; set; }

        public WebSpriteFont(IImage font, fontMetrics fontMetrics)
        {
            Font = font;
            FontMetrics = fontMetrics;

            FontDict = new Dictionary<string, Rectangle>();
            foreach (var characters in fontMetrics.character)
            {
                FontDict.Add(((char)int.Parse(characters.character)).ToString(), new Rectangle()
                {
                    X = int.Parse(characters.x),
                    Y = int.Parse(characters.y),
                    Width = int.Parse(characters.width),
                    Height = int.Parse(characters.height),
                });
            }
        }

        public Dictionary<string, Rectangle> FontDict { get; set; }

        public PointF MeasureString(string text)
        {
            var width = 0;
            var highestWidth = 0;
            var height = 0;
            var fontDict = FontDict;
            var tallestFontThisLine = 0;
            foreach (var @char in text.ToCharArray())
            {
                if (@char == '\n')
                {
                    width = 0;
                    height += tallestFontThisLine;
                    tallestFontThisLine = 0;
                    continue;
                }

                width += fontDict[@char.ToString()].Width + kerning;
                tallestFontThisLine = tallestFontThisLine > fontDict[@char.ToString()].Height ? tallestFontThisLine : fontDict[@char.ToString()].Height;
                if (width > highestWidth)
                {
                    highestWidth = width;
                }
            }
            height += tallestFontThisLine;
            return new PointF(width, height);
        }


        private JsDictionary<string, CanvasInformation> fontImageCaches = new JsDictionary<string, CanvasInformation>();

        public void DrawString(CanvasRenderingContext2D context, string text, PointF position, Color color, Point offset)
        {

            var fic = fontImageCaches[color.ToString()];

            if (fic == null)
            {
                fic = fontImageCaches[color.ToString()] = colorizeImage(((WebImage)Font).Image, color);
            }
            
            var imageElement = fic.Canvas;

            var fontDict = FontDict;
            var tallestFontThisLine = 0;
            foreach (var @char in text.ToCharArray())
            {
                if (@char == '\n')
                {
                    position.X = 0;
                    position.Y += tallestFontThisLine + kerning;
                    tallestFontThisLine = 0;
                    continue;
                }
                tallestFontThisLine = tallestFontThisLine > fontDict[@char.ToString()].Height ? tallestFontThisLine : fontDict[@char.ToString()].Height;
                var rectangle = fontDict[@char.ToString()];
                context.DrawImage(imageElement, rectangle.X, rectangle.Y, rectangle.Width, rectangle.Height, position.X, position.Y, rectangle.Width, rectangle.Height);
                position.X += rectangle.Width + kerning;
            }

        }
        public void DrawCenteredString(CanvasRenderingContext2D context, string text, PointF position, Color color, PointF offset)
        {
            var fic = fontImageCaches[color.ToString()];

            if (fic == null)
            {
                fic = fontImageCaches[color.ToString()] = colorizeImage(((WebImage)Font).Image, color);
            }

            var imageElement = fic.Canvas;


            var measures = text.Split('\n').Select(MeasureString).ToArray();
            var totalMeasure = MeasureString(text);
            var fontDict = FontDict;
            var positionX = 0;
            var currentMeasure = 0;
            position.Y -= (float)totalMeasure.Y / 2;
            var measure = measures[currentMeasure];
            positionX = (int)-measure.X / 2;
            foreach (var @char in text.ToCharArray())
            {
                if (@char == '\n')
                {
                    currentMeasure++;

                    measure = measures[currentMeasure];
                    position.Y += (float)measure.Y;

                    positionX = (int)-measure.X / 2;
                    continue;
                }

                var rectangle = fontDict[@char.ToString()];
                context.DrawImage(imageElement, rectangle.X, rectangle.Y, rectangle.Width, rectangle.Height, position.X + positionX, (float)(position.Y), rectangle.Width, rectangle.Height);
                positionX += rectangle.Width + kerning;
            }
        }

        private CanvasInformation colorizeImage(ImageElement img, Color color)
        {
            var canvas = CanvasInformation.Create(img.Width, img.Height);

            canvas.Context.DrawImage(img, 0, 0, img.NaturalWidth, img.NaturalHeight, 0, 0, img.Width, img.Height);
            var originalPixels = canvas.Context.GetImageData(0, 0, img.Width, img.Height);
            var currentPixels = canvas.Context.GetImageData(0, 0, img.Width, img.Height);

            var L = originalPixels.Data.Length;
            for (var I = 0; I < L; I += 4)
            {
                    currentPixels.Data[I] = (byte)(originalPixels.Data[I] / 255d * color.R % 256);
                    currentPixels.Data[I + 1] = (byte)(originalPixels.Data[I + 1] / 255d * color.G % 256);
                    currentPixels.Data[I + 2] = (byte)(originalPixels.Data[I + 2] / 255d * color.B % 256);
                    currentPixels.Data[I + 3] = (byte)(originalPixels.Data[I + 3] / 255d * color.A % 256);
            }

            canvas.Context.PutImageData(currentPixels, 0, 0);

            return canvas;
        }

        int kerning = 0;
    }

    [Serializable]
    public class fontMetrics
    {
        public FontMetricsCharacter[] character { get; set; }
        public string file { get; set; }
    }
    [Serializable]
    public class FontMetricsCharacter
    {
        public string x { get; set; }
        public string y { get; set; }
        public string width { get; set; }
        public string height { get; set; }
        public string character { get; set; }
    }

}
