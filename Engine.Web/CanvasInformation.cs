
using System.Runtime.CompilerServices;
using Bridge.Html5;

namespace Engine.Web
{
    public class CanvasInformation
    {
        private static HTMLCanvasElement blackPixel;
        public CanvasRenderingContext2D Context { get; set; }
        public HTMLCanvasElement Canvas { get; set; }
        public static HTMLCanvasElement BlackPixel
        {
            get
            {
                if (blackPixel == null)
                {
                    var m = Create(0, 0);

                    m.Context.FillStyle = "black";
                    m.Context.FillRect(0, 0, 1, 1);

                    blackPixel = m.Canvas;
                }
                return blackPixel;
            }
        }

        public CanvasInformation(CanvasRenderingContext2D context, HTMLCanvasElement domCanvas)
        {
            Context = context;
            Canvas = domCanvas;
        }

        public static CanvasInformation Create(int w, int h)
        {
            var canvas = (HTMLCanvasElement)Document.CreateElement("canvas");
            return Create(canvas, w, h);
        }

        public static CanvasInformation Create(HTMLCanvasElement canvas, int w, int h)
        {
            if (w == 0) w = 1;
            if (h == 0) h = 1;
            canvas.Width =  w;
            canvas.Height =  h;

            var ctx = (CanvasRenderingContext2D)canvas.GetContext("2d");
            return new CanvasInformation(ctx, canvas);
        }
    }
}