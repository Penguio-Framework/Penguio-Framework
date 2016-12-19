using System;
using System.Collections.Generic;
using Engine.Interfaces;

namespace Engine.Web
{
    public class WebImageCache
    {
        private readonly IClient client;
        private readonly Dictionary<string, WebImage> textures;
        private readonly Dictionary<string, WebSpriteFont> fonts;

        public WebImageCache(IClient client)
        {
            this.client = client;
            textures = new Dictionary<string, WebImage>();
            fonts = new Dictionary<string, WebSpriteFont>();
        }
        public WebImage GetImage(string imageName)
        {
            return textures[imageName];
        }
        public IImage CreateImage(string imageName, string imagePath, PointF center, Action ready)
        {
            return textures[imageName] = new WebImage(imagePath, center, ready);
        }

        public WebSpriteFont GetFont(string fontName, int fontSize)
        {
            return fonts[fontName + fontSize];
        }

        public void CreateFont(string fontName, int fontSize, string fontPath)
        {
            var fileName = fontPath + ".xml";
            fontMetrics j = client.ClientSettings.LoadXmlFile<fontMetrics>(fileName);

            var assetName = fontPath + ".png";
            var font = CreateImage(fontName + fontSize, assetName, new PointF(0, 0), () => { });

            fonts[fontName + fontSize] = new WebSpriteFont(font, j);
        }
    }
}