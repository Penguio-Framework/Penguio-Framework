namespace Engine
{
    public class Point
    {
        public int X { get; set; }
        public int Y { get; set; }


        public int Width
        {
            get { return X; }
        }

        public int Height
        {
            get { return Y; }
        }

        public override string ToString()
        {
            return string.Format("X: {0}, Y: {1}", X, Y);
        }

        public Point(int x, int y)
        {
            X = x;
            Y = y;
        }

        public Point(Point pos) : this(pos.X, pos.Y)
        {
        }

        public Point(PointF pos) : this((int) pos.X, (int) pos.Y)
        {
        }

        public static Point operator -(Point one, Point two)
        {
            return new Point(one.X - two.X, one.Y - two.Y);
        }

        public static Point operator +(Point one, Point two)
        {
            return new Point(one.X + two.X, one.Y + two.Y);
        }

        public static Point operator /(Point one, double m)
        {
            return new Point((int)(one.X / m), (int)(one.Y / m));
        }
        public static implicit operator PointF(Point one)
        {
            return new PointF(one.X,one.Y);
        }
    }
}