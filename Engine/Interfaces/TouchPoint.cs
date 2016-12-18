namespace Engine.Interfaces
{
    public class TouchPoint
    {
        public TouchPoint(TouchType touchMove, int x, int y)
        {
            this.touchMove = touchMove;
            X = x;
            Y = y;
        }

        public TouchType touchMove { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
    }
}