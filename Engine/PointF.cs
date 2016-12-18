namespace Engine
{
    public class PointF
    {
        public double X { get; set; }
        public double Y { get; set; }
        public static Point Zero = new Point(0, 0);

        public PointF(double x, double y)
        {
            X = x;
            Y = y;
        }

        public PointF(PointF currentPosition)
        {
            X = currentPosition.X;
            Y = currentPosition.Y;
        }

        public PointF(Point currentPosition) : this(currentPosition.X, currentPosition.Y)
        {
        }
    }
}