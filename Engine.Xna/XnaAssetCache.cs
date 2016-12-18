using System.Collections.Generic;
using Engine.Interfaces;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Engine.Xna
{
    public class XnaAssetCache
    {
        private readonly ContentManager content;
        private readonly IClient client;
        private readonly Dictionary<string, XnaImage> textures;
        private readonly Dictionary<string, XnaSpriteFont> fonts;

        public XnaAssetCache(ContentManager content, IClient client)
        {
            this.content = content;
            this.client = client;
            textures = new Dictionary<string, XnaImage>();
            fonts = new Dictionary<string, XnaSpriteFont>();
        }

        public XnaImage GetImage(string imageName)
        {
            return textures[imageName];
        }

        public IFont GetFont(string fontName)
        {
            return fonts[fontName];
        }

        public IImage CreateImage(string imageName, string imagePath, PointF center = null)
        {
            var assetName = imagePath;
            Texture2D texture2D = null;
            
            texture2D = content.Load<Texture2D>(assetName);
            
            var xnaImage = new XnaImage(texture2D, center);
            textures.Add(imageName, xnaImage);
            return xnaImage;
        }

        public IFont CreateFont(string fontName, string fontPath)
        {
            var fileName =  fontPath + ".xml";
            var j = client.ClientSettings.LoadXmlFile<fontMetrics>(fileName);

            var assetName =  fontPath;
            var font = content.Load<Texture2D>(assetName);

            var loadedFont = new XnaSpriteFont(font, j);
            fonts.Add(fontName , loadedFont);
            return loadedFont;
        }
    }
}