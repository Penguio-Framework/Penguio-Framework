using System.Collections.Generic;
using Engine.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Engine.Xna
{
    public class XnaLayer : ILayer
    {
        private readonly XnaRenderer renderer;
        private readonly SpriteBatch currentSpriteBatch;
        private readonly int width;
        private readonly int height;
        public ILayout Layout { get; set; }
        private readonly XnaShapeCache shapeCache;

        public XnaLayer(XnaRenderer renderer, int width, int height, ILayout layout)
        {
            this.renderer = renderer;
            this.width = width;
            this.height = height;
            Layout = layout;


            currentSpriteBatch = new SpriteBatch(renderer.graphicsDevice);

            settingsStack = new List<XnaContextSettings>();

            settingsStack.Add(new XnaContextSettings());
            shapeCache = new XnaShapeCache(renderer.graphicsDevice);
            _rasterizerState = new RasterizerState()
            {
                //                           ScissorTestEnable = true 
                MultiSampleAntiAlias = true,
            };
//            _blendState = BlendState.NonPremultiplied;
            
        }

        private Microsoft.Xna.Framework.Rectangle currentScissorRect;
        private readonly RasterizerState _rasterizerState;

        public void SetDrawingTransparency(double alpha)
        {
            CurrentSettings().Alpha = alpha;
        }

        public void Begin()
        {
            transformMatrix = renderer.GetScaleMatrix();

            var settings = CurrentSettings();
            if (Layout.Screen.OneLayoutAtATime)
            {
                settings.Left = 0;
                settings.Top = 0;
            }
            else
            {
                settings.Left = Layout.LayoutPosition.Location.X;
                settings.Top = Layout.LayoutPosition.Location.Y;
            }

            //            Resolution.getTransformationMatrix()


            //          currentSpriteBatch.GraphicsDevice.ScissorRectangle = new Microsoft.Xna.Framework.Rectangle(settings.Left, settings.Top,width,height);
            currentSpriteBatch.Begin(SpriteSortMode.Deferred, _blendState, null, null, _rasterizerState, null, transformMatrix);
        }

        public void End()
        {
            currentSpriteBatch.End();
        }

/*
         private void DrawRoundedRectangle(Graphics gfx, Rectangle bounds, int cornerRadius, Pen drawPen, Color fillColor)
        {
            int strokeOffset = Convert.ToInt32(Math.Ceiling(drawPen.Width));
            bounds = Rectangle.Inflate(bounds, -strokeOffset, -strokeOffset);
 
            var gfxPath = new GraphicsPath();
            if (cornerRadius > 0)
            {
                gfxPath.AddArc(bounds.X, bounds.Y, cornerRadius, cornerRadius, 180, 90);
                gfxPath.AddArc(bounds.X + bounds.Width - cornerRadius, bounds.Y, cornerRadius, cornerRadius, 270, 90);
                gfxPath.AddArc(bounds.X + bounds.Width - cornerRadius, bounds.Y + bounds.Height - cornerRadius, cornerRadius,
                               cornerRadius, 0, 90);
                gfxPath.AddArc(bounds.X, bounds.Y + bounds.Height - cornerRadius, cornerRadius, cornerRadius, 90, 90);
            }
            else
            {
                gfxPath.AddRectangle(bounds);
            }
            gfxPath.CloseAllFigures();
 
            gfx.FillPath(new SolidBrush(fillColor), gfxPath);
            if (drawPen != Pens.Transparent)
            {
                var pen = new Pen(drawPen.Color);
                pen.EndCap = pen.StartCap = LineCap.Round;
                gfx.DrawPath(pen, gfxPath);
            }
        }
*/
        private readonly List<XnaContextSettings> settingsStack;
        private static Microsoft.Xna.Framework.Color WHITE = new Microsoft.Xna.Framework.Color(255, 255, 255);
        private Color transparentEngineWhite;
        private Microsoft.Xna.Framework.Color transparentWhite;
        private Matrix transformMatrix;
        private readonly BlendState _blendState;

        public void Save()
        {
            settingsStack.Add(settingsStack.Last().Clone());
        }

        private XnaContextSettings CurrentSettings()
        {
            return settingsStack.Last();
        }

        public void Restore()
        {
            settingsStack.RemoveAt(settingsStack.Count - 1);
        }
         
        public void Translate(PointF point)
        {
            Translate(point.X, point.Y);
        }

        public void Translate(double x, double y)
        {
            var xnaContextSettings = CurrentSettings();
            xnaContextSettings.Left += (int) x;
            xnaContextSettings.Top += (int) y;
        }

        public void DrawImage(IImage image, Point point)
        {
            DrawImage(image, point.X, point.Y);
        }

        public void DrawImage(IImage image)
        {
            DrawImage(image, 0, 0);
        }

        public void DrawImage(IImage image, int x, int y)
        {
            DrawImage(image, x, y, image.Width, image.Height);
        }

        public void DrawImage(IImage image, Point point, bool center)
        {
            DrawImage(image, point.X, point.Y, center);
        }

        public void DrawImage(IImage image, int x, int y, bool center)
        {
            var _x = x;
            var _y = y;

            if (center)
            {
                _x -= (int) (image.Center.X);
                _y -= (int) image.Center.Y;
            }

            DrawImage(image, _x, _y);
        }

        public void DrawImage(IImage image, int x, int y, int width, int height)
        {
            DrawImage(image, x, y, width, height, false);
        }

        public void DrawImage(IImage image, int x, int y, int width, int height, bool center)
        {
            var xnaContextSettings = CurrentSettings();
            var xnaImage = (XnaImage) image;

            if (center)
            {
                currentSpriteBatch.Draw(xnaImage.Texture, new Microsoft.Xna.Framework.Rectangle(xnaContextSettings.Left + x - width/2, xnaContextSettings.Top + y - height/2, width, height), xnaImage.SourceRectangle, TransparentWhite, 0, Vector2.Zero,
                    DrawingEffectsToSpriteEffects(xnaContextSettings.DrawingEffects), 1);
            }
            else
            {
                currentSpriteBatch.Draw(xnaImage.Texture, new Microsoft.Xna.Framework.Rectangle(xnaContextSettings.Left + x, xnaContextSettings.Top + y, width, height), xnaImage.SourceRectangle, TransparentWhite, 0, Vector2.Zero, DrawingEffectsToSpriteEffects(xnaContextSettings.DrawingEffects), 1);
            }
        }

        public void DrawImage(IImage image, int x, int y, double angle, int centerX, int centerY)
        {
            var xnaContextSettings = CurrentSettings();
            var xnaImage = (XnaImage) image;


            var location = new Vector2(xnaContextSettings.Left + x - centerX, xnaContextSettings.Top + y - centerY);
            var origin = new Vector2(centerX, centerY);

            currentSpriteBatch.Draw(xnaImage.Texture, location, xnaImage.SourceRectangle, TransparentWhite, (float) angle, origin, 1.0f, DrawingEffectsToSpriteEffects(xnaContextSettings.DrawingEffects), 1);
        }

        public void DrawImage(IImage image, int x, int y, double angle, bool center)
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


        public void DrawImage(IImage image, int x, int y, double angle, Point center)
        {
            DrawImage(image, x, y, angle, center.X, center.Y);
        }

        public void DrawImage(IImage image, Point point, double angle, Point center)
        {
            DrawImage(image, point.X, point.Y, angle, center.X, center.Y);
        }


        public void DrawImage(IImage image, PointF point)
        {
            DrawImage(image, point.X, point.Y);
        }

        public void DrawImage(IImage image, double x, double y)
        {
            DrawImage(image, x, y, image.Width, image.Height);
        }

        public void DrawImage(IImage image, PointF point, bool center)
        {
            DrawImage(image, point.X, point.Y, center);
        }

        public void DrawImage(IImage image, double x, double y, bool center)
        {
            var _x = x;
            var _y = y;

            if (center)
            {
                _x -= image.Center.X;
                _y -= image.Center.Y;
            }

            DrawImage(image, _x, _y, image.Width, image.Height);
        }

        public void DrawImage(IImage image, double x, double y, double width, double height)
        {
            DrawImage(image, x, y, width, height, false);
        }

        public void DrawImage(IImage image, double x, double y, double width, double height, bool center)
        {
            var xnaContextSettings = CurrentSettings();
            var xnaImage = (XnaImage) image;
            var texture = xnaImage.Texture; //CreateBlurredTexture(xnaImage.Texture, SpriteEffects.None);
            double cx;
            double cy;

            if (center)
            {
                cx = width/2d;
                cy = height/2d;
            }
            else
            {
                cx = 0;
                cy = 0;
            }
            var destRect = new Microsoft.Xna.Framework.Rectangle((int) (xnaContextSettings.Left + x - cx), (int) (xnaContextSettings.Top + y - cy), (int) width, (int) height);

            /*
                        var blurImage=(XnaImage)renderer.GetImage("blurBox");
                        var boxRect = new Microsoft.Xna.Framework.Rectangle(destRect.X, destRect.Y, destRect.Width, destRect.Height);
                        boxRect.Inflate(15 ,15);
                        currentSpriteBatch.Draw(blurImage.Texture, boxRect, xnaImage.SourceRectangle, TransparentWhite, 0, Vector2.Zero, DrawingEffectsToSpriteEffects(xnaContextSettings.DrawingEffects), 1);

            */
            currentSpriteBatch.Draw(texture, destRect, xnaImage.SourceRectangle, TransparentWhite, 0, Vector2.Zero, DrawingEffectsToSpriteEffects(xnaContextSettings.DrawingEffects), 1);
        }

        public void DrawImage(IImage image, double x, double y, double angle, double centerX, double centerY)
        {
            DrawImage(image, x, y, angle, image.Width, image.Height, centerX, centerY);
        }


        public void DrawImage(IImage image, double x, double y, double angle, double width, double height, double centerX, double centerY)
        {
            var xnaContextSettings = CurrentSettings();
            var xnaImage = (XnaImage) image;


            var destination = new Microsoft.Xna.Framework.Rectangle((int) (xnaContextSettings.Left + x), (int) (xnaContextSettings.Top + y), (int) width, (int) height);
            //need to offset center for source rectangle, not destination
            var origin = new Vector2((float) (centerX) + (float) (image.Width - width)/2, (float) centerY + (float) (image.Height - height)/2);


            currentSpriteBatch.Draw(xnaImage.Texture, destination, xnaImage.SourceRectangle, TransparentWhite, (float) angle, origin, DrawingEffectsToSpriteEffects(xnaContextSettings.DrawingEffects), 1);
        }

        public void DrawImage(IImage image, double x, double y, double angle, bool center)
        {
            if (center)
            {
                DrawImage(image, x, y, angle, image.Width, image.Height, image.Center.X, image.Center.Y);
            }
            else
            {
                DrawImage(image, x, y, angle, image.Width, image.Height, 0, 0);
            }
        }

        public void DrawImage(IImage image, double x, double y, double angle, double width, double height, bool center)
        {
            if (center)
            {
                DrawImage(image, x, y, angle, width, height, width/2, height/2);
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
            DrawString(font,text, point.X, point.Y, center);
        }


        private static SpriteEffects DrawingEffectsToSpriteEffects(DrawingEffects drawingEffects)
        {
            return (SpriteEffects) drawingEffects;
        }

        public void StrokeRectangle(Color color, int x, int y, int width, int height,int strokeSize, bool center = false)
        {
            var xnaContextSettings = CurrentSettings();

            currentSpriteBatch.Draw(shapeCache.GetStrokeRect(color, width, height, strokeSize), new Vector2(xnaContextSettings.Left + x + (center ? -width / 2 : 0) - strokeSize/2, xnaContextSettings.Top + y + (center ? -height / 2 : 0) - strokeSize/2), TransparentWhite);
        }

        public void DrawRectangle(Color color, Rectangle rectangle, bool center = false)
        {
            DrawRectangle(color, rectangle.X, rectangle.Y, rectangle.Width, rectangle.Height,center);
        }

        public void StrokeRectangle(Color color, Rectangle rectangle, int strokeSize, bool center = false)
        {
            StrokeRectangle(color, rectangle.X, rectangle.Y, rectangle.Width, rectangle.Height, strokeSize,center);
        }

        public void BoxShadow(Color color, int x, int y, int width, int height, int shadowLength, bool center = false)
        {
            var xnaContextSettings = CurrentSettings();

            currentSpriteBatch.Draw(shapeCache.GetShadowRect(color, width, height, shadowLength), new Vector2(xnaContextSettings.Left + x + (center ? -width / 2 : 0), xnaContextSettings.Top + y + (center ? -height / 2 : 0)), TransparentWhite);
        }


        public void BoxShadow(Color color, Rectangle rectangle, int shadowLength, bool center = false)
        {
            BoxShadow(color, rectangle.X, rectangle.Y, rectangle.Width, rectangle.Height, shadowLength, center);
        }

        public void SetDrawingEffects(DrawingEffects drawingEffects)
        {
            var xnaContextSettings = CurrentSettings();
            xnaContextSettings.DrawingEffects = drawingEffects;
        }

        public void DrawString(IFont font, string text, Point point, Color color, bool center = true)
        {
            DrawString(font,text, point.X, point.Y, color, center);
        }

        public void DrawString(IFont font, string text, int x, int y, bool center = true)
        {
            DrawString(font,text, x, y, TransparentEngineWhite, center);
        }

        public void DrawString(IFont font, string text, int x, int y, Color color, bool center = true)
        {
            var xnaContextSettings = CurrentSettings(); 
            if (font == null) return;
            Vector2 position;
            position = new Vector2((float) (xnaContextSettings.Left + x), (float) (xnaContextSettings.Top + y));
            if (center)
            {
               ((XnaSpriteFont) font).DrawCenteredString(currentSpriteBatch, text, position, new Microsoft.Xna.Framework.Color(color.R, color.G, color.B, color.A), Vector2.Zero);
            }
            else
            {
                ((XnaSpriteFont)font).DrawString(currentSpriteBatch, text, position, new Microsoft.Xna.Framework.Color(color.R, color.G, color.B, color.A), Vector2.Zero);
            }
        }

        public void Clear()
        {
        }

        public PointF MeasureString(IFont font, string text)
        { 
            if (font == null) return null;

            var m = ((XnaSpriteFont)font).MeasureString(text);
            return m;
        }

        public void DrawRectangle(Color color, int x, int y, int width, int height, bool center = false)
        {
            var xnaContextSettings = CurrentSettings();

            currentSpriteBatch.Draw(shapeCache.GetFilledRect(color, width, height), new Vector2(xnaContextSettings.Left + x + (center ? -width/2 : 0), xnaContextSettings.Top + y + (center ? -height/2 : 0)), TransparentWhite);
        }

        public Microsoft.Xna.Framework.Color TransparentWhite
        {
            get
            {
                var currentSettings = CurrentSettings();
                if (currentSettings.Alpha == 1)
                {
                    transparentWhite = Microsoft.Xna.Framework.Color.White;
                }
                else
                {
                    transparentWhite = Microsoft.Xna.Framework.Color.FromNonPremultiplied(255, 255, 255, (int) (currentSettings.Alpha*255));
                }
                return transparentWhite;
            }
        }

        public Color TransparentEngineWhite
        {
            get
            {
                if (transparentEngineWhite == null)
                {
                    transparentEngineWhite = new Color(TransparentWhite.R, TransparentWhite.G, TransparentWhite.B, TransparentWhite.A);
                }
                return transparentEngineWhite;
            }
        }

        public static Texture2D CreateBlurredTexture(Texture2D originalTexture, SpriteEffects effects)
        {
            var device = originalTexture.GraphicsDevice;
            var rt4 = new RenderTarget2D(device, originalTexture.Width/4, originalTexture.Height/4);

            using (var rt2 = new RenderTarget2D(device, originalTexture.Width*3/2, originalTexture.Height*3/2))
            using (var rt3 = new RenderTarget2D(device, originalTexture.Width/2, originalTexture.Height/2))
            using (var spriteBatch = new SpriteBatch(device))
            {
                device.SetRenderTarget(rt2);
                device.Clear(Microsoft.Xna.Framework.Color.Transparent);
                spriteBatch.Begin();
                spriteBatch.Draw(originalTexture, new Microsoft.Xna.Framework.Rectangle(0, 0, rt2.Width, rt2.Height), null, Microsoft.Xna.Framework.Color.White, 0, Vector2.Zero, effects, 0f);
                spriteBatch.End();
                device.SetRenderTarget(rt3);
                device.Clear(Microsoft.Xna.Framework.Color.Transparent);
                spriteBatch.Begin();
                spriteBatch.Draw(rt2, new Microsoft.Xna.Framework.Rectangle(0, 0, rt3.Width, rt3.Height), Microsoft.Xna.Framework.Color.White);
                spriteBatch.End();
                device.SetRenderTarget(rt4);
                device.Clear(Microsoft.Xna.Framework.Color.Transparent);
                spriteBatch.Begin();
                spriteBatch.Draw(rt3, new Microsoft.Xna.Framework.Rectangle(0, 0, rt4.Width, rt4.Height), Microsoft.Xna.Framework.Color.White);
                spriteBatch.End();
                device.SetRenderTarget(null);
            }

            return rt4;
        }
    }
}