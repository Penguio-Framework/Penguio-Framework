namespace Engine
{
    public class TouchRect
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public TouchTrigger TouchEventToTrigger { get; set; }
        public SwipeTrigger SwipeEventToTrigger { get; set; }
        public object State { get; set; }

        public TouchRect(Rectangle rect, TouchTrigger eventToTrigger, bool pointIsCenter = false, object state = null)
            : this(rect.X, rect.Y, rect.Width, rect.Height, eventToTrigger, pointIsCenter, state)
        {
        }

        public TouchRect(Point p, int width, int height, TouchTrigger eventToTrigger, bool pointIsCenter = false, object state = null)
            : this(p.X, p.Y, width, height, eventToTrigger, pointIsCenter, state)
        {
        }

        public TouchRect(Point p, Point size, TouchTrigger eventToTrigger, bool pointIsCenter = false, object state = null)
            : this(p.X, p.Y, size.X, size.Y, eventToTrigger, pointIsCenter, state)
        {
        }

        public TouchRect(Rectangle rect, SwipeTrigger eventToTrigger, bool pointIsCenter = false, object state = null)
            : this(rect.X, rect.Y, rect.Width, rect.Height, eventToTrigger, pointIsCenter, state)
        {
        }

        public TouchRect(Point p, int width, int height, SwipeTrigger eventToTrigger, bool pointIsCenter = false, object state = null)
            : this(p.X, p.Y, width, height, eventToTrigger, pointIsCenter, state)
        {
        }

        public TouchRect(Point p, Point size, SwipeTrigger eventToTrigger, bool pointIsCenter = false, object state = null)
            : this(p.X, p.Y, size.X, size.Y, eventToTrigger, pointIsCenter, state)
        {
        }

        public TouchRect(int x, int y, int width, int height, TouchTrigger eventToTrigger, bool pointIsCenter = false, object state = null)
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
            TouchEventToTrigger = eventToTrigger;
            State = state;
        }

        public TouchRect(int x, int y, int width, int height, SwipeTrigger eventToTrigger, bool pointIsCenter = false, object state = null)
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
            SwipeEventToTrigger = eventToTrigger;
            State = state;
        }

        public bool Collides(int x, int y)
        {
            return X < x &&
                   X + Width > x &&
                   Y < y &&
                   Y + Height > y;
        }
    }
}