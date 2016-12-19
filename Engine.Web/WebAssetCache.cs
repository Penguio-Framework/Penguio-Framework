using System;
using System.Collections.Generic;
using Engine.Interfaces;

namespace Engine.Web
{
    public class WebAssetCache
    {
        private readonly IClient client;
        private readonly Dictionary<string, WebImage> textures;
        private readonly Dictionary<string, WebSpriteFont> fonts;

        public WebAssetCache(IClient client)
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
            return textures[imageName] = new WebImage(imagePath+".png", center, ready);
        }

        public WebSpriteFont GetFont(string fontName)
        {
            return fonts[fontName ];
        }

        public IFont CreateFont(string fontName, string fontPath)
        {
            var fileName =  fontPath + ".xml";
            fontMetrics j = client.ClientSettings.LoadXmlFile<fontMetrics>(fileName);

            var assetName =  fontPath;
            var font = CreateImage(fontName, assetName, new PointF(0, 0), () => { });

            return fonts[fontName] = new WebSpriteFont(font, j);
        }
    }
}