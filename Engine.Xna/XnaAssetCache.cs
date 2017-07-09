using System;
using System.Collections.Generic;
using Engine.Interfaces;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Engine.Xna
{
    public class XnaAssetCache
    {
        private readonly ContentManager content;
        private readonly BaseClient client;

        public XnaAssetCache(ContentManager content, BaseClient client)
        {
            this.content = content;
            this.client = client;
        }


        public IImage CreateImage(string imagePath, PointF center = null)
        {
            var assetName = imagePath;
            Texture2D texture2D = null;
            
            texture2D = content.Load<Texture2D>(assetName);

            var xnaImage = new XnaImage(texture2D, center);
            return xnaImage;
        }

        public IFont CreateFont(string fontPath)
        {
            var fileName =  fontPath + ".xml";
            var j = client.ClientSettings.LoadXmlFile<fontMetrics>(fileName);

            var assetName =  fontPath;
            var font = content.Load<Texture2D>(assetName);

            var loadedFont = new XnaSpriteFont(font, j);
            return loadedFont;
        }
    }
}