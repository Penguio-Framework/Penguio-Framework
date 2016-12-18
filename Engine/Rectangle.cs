namespace Engine
{
    public class Rectangle
    {
        public Rectangle()
        {
        }

        public int X { get; set; }
        public int Y { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public Point Center { get; set; }

        public Rectangle(int x, int y, int width, int height, bool pointIsCenter = false)
        {
            if (pointIsCenter)
            {
                X = x - width/2;
                Y = y - height/2;
            }
            else
            {
                X = x;
                Y = y;
            }
            Width = width;
            Height = height;
            Center = new Point(x + width/2, y + height/2);
        }

        public Rectangle(int x, int y, Size size)
        {
            X = x;
            Y = y;
            Width = size.Width;
            Height = size.Height;
        }

        public Rectangle(Point position, int width, int height)
        {
            X = position.X;
            Y = position.Y;
            Width = width;
            Height = height;
        }

        public Rectangle(Point position, Size size)
        {
            X = position.X;
            Y = position.Y;
            Width = size.Width;
            Height = size.Height;
        }

        public bool Contains(Point point)
        {
            return Contains(point.X, point.Y);
        }

        public bool Contains(int x, int y)
        {
            return X < x && Y < y && X + Width > x && Y + Height > y;
        }
    }
}