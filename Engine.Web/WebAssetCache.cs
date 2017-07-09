using System;
using System.Collections.Generic;
using Engine.Interfaces;

namespace Engine.Web
{
    public class WebAssetCache
    {
        private readonly IClient client;

        public WebAssetCache(IClient client)
        {
            this.client = client;
        }
        public IImage CreateImage( string imagePath, PointF center, Action ready)
        {
            return new WebImage("assets/"+imagePath+".png", center, ready);
        }
        

        public IFont CreateFont(string fontPath)
        {
            var fileName =  "assets/"+fontPath + ".xml";
            fontMetrics j = client.ClientSettings.LoadXmlFile<fontMetrics>(fileName);

            var assetName =  fontPath;
            var font = CreateImage( "../"+assetName, new PointF(0, 0), () => { });

            return  new WebSpriteFont(font, j);
        }
    }
}