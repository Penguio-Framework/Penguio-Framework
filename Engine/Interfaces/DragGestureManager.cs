using System;
using System.Collections.Generic;

namespace Engine.Interfaces
{
    public class DragGestureManager
    {
        private readonly List<TouchPoint> points = new List<TouchPoint>();
        public const int TriggerDistance = 300;
        private bool tillUp = false;

        public void TouchUp()
        {
            tillUp = false;
        }

        public void AddDataPoint(TouchType touchType, int x, int y)
        {
            if (!tillUp)
            {
                if (touchType == TouchType.TouchUp)
                {
                    points.Clear();
                    return;
                }
                points.Add(new TouchPoint(touchType, x, y));
            }
        }

        public DragGesture GetGeture()
        {
            if (points.Count < 2)
            {
                return null;
            }

            var touchPoint = points[points.Count - 1];

            var point = new Point(touchPoint.X - points[0].X, touchPoint.Y - points[0].Y);
            if (Math.Abs(point.X) > Math.Abs(point.Y))
            {
                return new DragGesture(Math.Abs(point.X), point.X < 0 ? Direction.Right : Direction.Left);
            }
            else
            {
                return new DragGesture(Math.Abs(point.Y), point.Y < 0 ? Direction.Down : Direction.Up);
            }
        }

        public void ClearDataPoints()
        {
            points.Clear();
        }

        public void ClearDataPointsTillUp()
        {
            tillUp = true;
            ClearDataPoints();
        }
    }
}