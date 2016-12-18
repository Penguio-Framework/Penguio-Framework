using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;

namespace Engine.Xna
{
    public class XnaShapeCache
    {
        private readonly GraphicsDevice _graphicDevice;

        public XnaShapeCache(GraphicsDevice graphicDevice)
        {
            _graphicDevice = graphicDevice;
        }

        private readonly Dictionary<string, Texture2D> caches = new Dictionary<string, Texture2D>();

        public Texture2D GetFilledRect(Color color, int width, int height)
        {
            var m = getKey(color, width, height, 0, false);
            Texture2D texture;
            if (caches.TryGetValue(m, out texture))
            {
                return texture;
            }

            var rect = new Texture2D(_graphicDevice, width, height);
            var xnaColor = new Microsoft.Xna.Framework.Color(color.R, color.G, color.B, color.A);
            var data = new Microsoft.Xna.Framework.Color[width * height];
            for (var i = 0; i < data.Length; ++i) data[i] = xnaColor;
            rect.SetData(data);


            caches.Add(m, rect);
            return rect;
        }
        public Texture2D GetStrokeRect(Color color, int width, int height, int strokeSize)
        {
            var m = getKey(color, width, height, strokeSize, false);
            Texture2D texture;
            if (caches.TryGetValue(m, out texture))
            {
                return texture;
            }

            var realWidth = width + strokeSize  ;
            var realHeight = height + strokeSize  ;

            var rect = new Texture2D(_graphicDevice, realWidth, realHeight);
            var xnaColor = new Microsoft.Xna.Framework.Color(color.R, color.G, color.B, color.A);
            var transparentColor = new Microsoft.Xna.Framework.Color(0, 0, 0, 0);
            var data = new Microsoft.Xna.Framework.Color[realWidth * realHeight];
            for (var i = 0; i < data.Length; ++i) data[i] = transparentColor;
/*

            for (int j = 0; j < strokeSize; j++)
                DrawLine(data, realWidth, xnaColor, 0, j, realWidth, j);

            for (int j = 0; j < strokeSize; j++)
                DrawLine(data, realWidth, xnaColor, 0, realHeight - 1 - j, realWidth, realHeight - 1 - j);

            for (int j = 0; j < strokeSize; j++)
                DrawLine(data, realWidth, xnaColor, j, 0, j, realHeight - 1);

            for (int j = 0; j < strokeSize; j++)
                DrawLine(data, realWidth, xnaColor, realWidth - 1 - j, 0, realWidth - 1 - j, realHeight);
*/


            for (var i = 0; i < realWidth; ++i)
                for (int j = 0; j < strokeSize; j++)
                    data[i + j * realWidth] = xnaColor;

            for (var i = 0; i < realWidth; ++i)
                for (int j = 0; j < strokeSize; j++)
                    data[i + (realHeight - 1 - j) * realWidth] = xnaColor;


            for (var i = 0; i < realHeight; ++i)
                for (int j = 0; j < strokeSize; j++)
                    data[j + i * realWidth] = xnaColor;

            for (var i = 0; i < realHeight; ++i)
                for (int j = 0; j < strokeSize; j++)
                    data[(realWidth - 1 - j) + (i) * realWidth] = xnaColor;


            rect.SetData(data);


            caches.Add(m, rect);
            return rect;
        }

        private string getKey(Color color, int width, int height, int strokeSize, bool shadow)
        {
            return string.Format("{0}-{1}-{2}-{3}-{4}-{5}-{6}-{7}", color.R, color.G, color.B, color.A, width, height, strokeSize, shadow);
        }

        public Texture2D GetShadowRect(Color color, int width, int height, int shadowLength)
        {
            var m = getKey(color, width, height, shadowLength, true);
            Texture2D texture;
            if (caches.TryGetValue(m, out texture))
            {
                return texture;
            }
            var realWidth = width + shadowLength * 2;
            var realHeight = height + shadowLength * 2;

            var rect = new Texture2D(_graphicDevice, realWidth, realHeight);
            var xnaColor = new Microsoft.Xna.Framework.Color[shadowLength];
            var aOffset = color.A / shadowLength;
            for (int i = 0; i < shadowLength; i++)
            {
                xnaColor[i] = new Microsoft.Xna.Framework.Color(color.R, color.G, color.B, color.A - aOffset * i);
            }
            var transparentColor = new Microsoft.Xna.Framework.Color(0, 0, 0, 0);
            var data = new Microsoft.Xna.Framework.Color[realWidth * realHeight];
            for (var i = 0; i < data.Length; ++i) data[i] = transparentColor;


            for (int j = 0; j < shadowLength; j++)
                DrawLine(data, realWidth, xnaColor[shadowLength - 1 - j], shadowLength, j, realWidth - shadowLength, j);

            for (int j = 0; j < shadowLength; j++)
                DrawLine(data, realWidth, xnaColor[shadowLength - 1 - j], shadowLength, realHeight - 1 - j, realWidth - shadowLength, realHeight - 1 - j);

            for (int j = 0; j < shadowLength; j++)
                DrawLine(data, realWidth, xnaColor[shadowLength - 1 - j], j, shadowLength, j, realHeight - 1 - shadowLength);

            for (int j = 0; j < shadowLength; j++)
                DrawLine(data, realWidth, xnaColor[shadowLength - 1 - j], realWidth - 1 - j, shadowLength, realWidth - 1 - j, realHeight - 1 - shadowLength);


            for (int j = 0; j < shadowLength; j++)
                DrawLine(data, realWidth, xnaColor[shadowLength - 1 - j], 0, j, j, 0);


            //            for (var i = 0; i < shadowLength; ++i)
            //                for (var j = 0; j < i; ++j)
            //                data[(j) + (j) * realWidth] = xnaColor[shadowLength - 1 - j];


            rect.SetData(data);


            caches.Add(m, rect);
            return rect;

        }

        public static void DrawLine(Microsoft.Xna.Framework.Color[] block, int blockWidth, Microsoft.Xna.Framework.Color color, int x0, int y0, int x1, int y1)
        {

            var dx = Math.Abs(x1 - x0);
            var dy = Math.Abs(y1 - y0);
            int sx, sy;
            if (x0 < x1) sx = 1;
            else sx = -1;
            if (y0 < y1) sy = 1;
            else sy = -1;

            var err = dx - dy;

            while (true)
            {
                block[x0 + y0 * blockWidth] = color;

                if (x0 == x1 && y0 == y1) return;
                var e2 = 2 * err;
                if (e2 > -dy)
                {
                    err = err - dy;
                    x0 = x0 + sx;
                }
                if (e2 < dx)
                {
                    err = err + dx;
                    y0 = y0 + sy;
                }
            }
        }
    }
}