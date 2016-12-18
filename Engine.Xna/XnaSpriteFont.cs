using System.Collections.Generic;
using Engine.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Engine.Xna
{
    public class XnaSpriteFont:IFont
    {
        public Texture2D Font { get; set; }
        public fontMetrics FontMetrics { get; set; }

        public XnaSpriteFont(Texture2D font, fontMetrics fontMetrics)
        {
            Font = font;
            FontMetrics = fontMetrics;

            FontDict = new Dictionary<char, Microsoft.Xna.Framework.Rectangle>();
            foreach (var characters in fontMetrics.character)
            {
                FontDict.Add((char) characters.character, new Microsoft.Xna.Framework.Rectangle()
                {
                    X = characters.x,
                    Y = characters.y,
                    Width = characters.width,
                    Height = characters.height,
                });
            }
        }

        public Dictionary<char, Microsoft.Xna.Framework.Rectangle> FontDict { get; set; }

        public PointF MeasureString(string text)
        {
            var width = 0;
            var highestWidth = 0;
            var height = 0;
            var fontDict = FontDict;
            var tallestFontThisLine = 0;
            foreach (var @char in text)
            {
                if (@char == '\n')
                {
                    width = 0;
                    height += tallestFontThisLine;
                    tallestFontThisLine = 0;
                    continue;
                }

                width += fontDict[@char].Width + kerning;
                tallestFontThisLine = tallestFontThisLine > fontDict[@char].Height ? tallestFontThisLine : fontDict[@char].Height;
                if (width > highestWidth)
                {
                    highestWidth = width;
                }
            }
            height += tallestFontThisLine;
            return new PointF(width, height);
        }

        public void DrawString(SpriteBatch currentSpriteBatch, string text, Vector2 position, Microsoft.Xna.Framework.Color color, Vector2 offset)
        {
            var fontDict = FontDict;
            var tallestFontThisLine = 0;
            foreach (var @char in text)
            {
                if (@char == '\n')
                {
                    position.X = 0;
                    position.Y += tallestFontThisLine + kerning;
                    tallestFontThisLine = 0;
                    continue;
                }
                tallestFontThisLine = tallestFontThisLine > fontDict[@char].Height ? tallestFontThisLine : fontDict[@char].Height;


                var rectangle = fontDict[@char];
                currentSpriteBatch.Draw(Font, position, rectangle, color);
                position.X += rectangle.Width + kerning;
            }
        }

        public void DrawCenteredString(SpriteBatch currentSpriteBatch, string text, Vector2 position, Microsoft.Xna.Framework.Color color, Vector2 offset)
        {
            var measures = text.Split('\n').Select(MeasureString).ToArray();
            var totalMeasure = MeasureString(text);
            var fontDict = FontDict;
            var positionX = 0;
            var currentMeasure = 0;
            position.Y -= (float) totalMeasure.Y/2;
            var measure = measures[currentMeasure];
            positionX = (int) -measure.X/2;
            foreach (var @char in text)
            {
                if (@char == '\n')
                {
                    currentMeasure++;

                    measure = measures[currentMeasure];
                    position.Y += (float) measure.Y;

                    positionX = (int) -measure.X/2;
                    continue;
                }

                var rectangle = fontDict[@char];
                currentSpriteBatch.Draw(Font, new Vector2(position.X + positionX, (float) (position.Y)), rectangle, color);
                positionX += rectangle.Width + kerning;
            }
        }

        private int kerning = 0;
    }
}