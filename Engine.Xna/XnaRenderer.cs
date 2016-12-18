using System;
using System.Collections.Generic;
using Engine.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Matrix = Microsoft.Xna.Framework.Matrix;

namespace Engine.Xna
{
    public class XnaRenderer : IRenderer
    {
        public IClient Client { get; set; }
        protected internal readonly GraphicsDevice graphicsDevice;

        /// todo move content to somewhere better, renderer does way too much
        public readonly ContentManager content;

        protected internal readonly GraphicsDeviceManager graphics;
        private readonly XnaAssetCache assetCache;
        private readonly List<XnaLayer> layers;


        public XnaRenderer(GraphicsDevice graphicsDevice, ContentManager content, GraphicsDeviceManager graphics, IClient client)
        { 
            Client = client;
            this.graphicsDevice = graphicsDevice;
            this.content = content;
            this.graphics = graphics;

            assetCache = new XnaAssetCache(content, client);
            layers = new List<XnaLayer>();
        }


        private Matrix? scaleMatrix;
        private Point offsetMatrix;
        private Point sizeMatrix;

        public void ClearScaleMatrix()
        {
            scaleMatrix = null;
            offsetMatrix = null;
            sizeMatrix = null;
        }

        public Matrix GetScaleMatrix()
        {
            if (scaleMatrix.HasValue == false)
            {
                var screenSize = Client.ScreenManager.CurrentScreen.GetLayoutSize();

                var gameWorldSize = new Vector2(screenSize.Width, screenSize.Height);
                var vp = graphicsDevice.Viewport;

                var scaleX = vp.Width/gameWorldSize.X;
                var scaleY = vp.Height/gameWorldSize.Y;
                scaleY = scaleX;

                var translateX = (vp.Width - (gameWorldSize.X*scaleX))/2f;
                var translateY = (vp.Height - (gameWorldSize.Y*scaleY))/2f;


                var camera = Matrix.CreateScale(scaleX, scaleY, 1)
                             *Matrix.CreateTranslation(translateX, translateY, 0);

                scaleMatrix = camera;
            }

            return scaleMatrix.Value;
        }

        public Point GetOffset()
        {
            if (offsetMatrix == null)
            {
                var screenSize = Client.ScreenManager.CurrentScreen.GetLayoutSize();

                var gameWorldSize = new Vector2(screenSize.Width, screenSize.Height);
                var vp = graphicsDevice.Viewport;

                var scaleX = vp.Width/gameWorldSize.X;
                var scaleY = vp.Height/gameWorldSize.Y;
                scaleY = scaleX;

                var translateX = (vp.Width - (gameWorldSize.X*scaleX))/2f;
                var translateY = (vp.Height - (gameWorldSize.Y*scaleY))/2f;


                offsetMatrix = new Point((int)Math.Max(translateX, 0), (int)Math.Max(translateY, 0));
            }

            return offsetMatrix;
        }

        public Point GetScreenSize()
        {
            if (sizeMatrix == null)
            {
                var screenSize = Client.ScreenManager.CurrentScreen.GetLayoutSize();

                var gameWorldSize = new Vector2(screenSize.Width, screenSize.Height);
                var vp = graphicsDevice.Viewport;

                var scaleX = vp.Width/gameWorldSize.X;
                var scaleY = vp.Height/gameWorldSize.Y;
                scaleY = scaleX;

                sizeMatrix = new Point((int) (gameWorldSize.X*scaleX), (int) (gameWorldSize.Y*scaleY));
            }

            return sizeMatrix;
        }

        public ILayer CreateLayer(int width, int height, ILayout layout)
        {
            return new XnaLayer(this, width, height, layout);
        }

        public ILayer CreateLayer(ILayout layout)
        {
            return new XnaLayer(this, layout.Width, layout.Height, layout);
        }


        public void AddLayer(ILayer layer)
        {
            layers.Add((XnaLayer) layer);
        }

        public IImage GetImage(string imageName)
        {
            return assetCache.GetImage(imageName);
        }


        public IFont GetFont(string fontName)
        {
            return assetCache.GetFont(fontName);
        }

        public IImage CreateImage(string imageName, string imagePath, PointF center = null)
        {
            return assetCache.CreateImage(imageName, imagePath, center);
        }

        public IFont CreateFont(string fontName , string fontPath)
        {
            return  assetCache.CreateFont(fontName,  fontPath);
        }

        public void BeginRender()
        {
        }

        public void EndRender()
        {
        }
    }
}