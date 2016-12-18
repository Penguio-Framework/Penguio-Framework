//#define DRAWTOUCH 
using System;
using System.Collections.Generic;
using Engine.Interfaces;

namespace Engine
{
    public class TouchManager : ITouchManager
    {
        private readonly IClient _client;
        public List<TouchRect> touchRects { get; set; }
        public List<TouchRect> swipeRects { get; set; }

        public TouchManager(IClient client)
        {
            _client = client;
            touchRects = new List<TouchRect>();
            swipeRects = new List<TouchRect>();

            client.Interval(swipeMonitor, 16);
        }

        private static readonly int MaxSwipeLengthMS = 180;
        private static readonly int minSwipeLength = 20;

        private void swipeMonitor()
        {
            var now = DateTime.Now;
            var removeCaches = new List<TouchCache>();

            var moveCaches = new List<TouchCache>();

            for (var index = 0; index < touchCaches.Count; index++)
            {
                var touchCache = touchCaches[index];
                if (touchCache.TouchType == TouchType.TouchUp)
                {
                    if (moveCaches.Count == 0)
                    {
//                        touchCache.Process();
                        removeCaches.Add(touchCache);
                        continue;
                    }


                    var start = new Point(moveCaches[0].X, moveCaches[0].Y);
                    var end = new Point(touchCache.X, touchCache.Y);

                    var distance = Math.Sqrt(Math.Pow(end.X - start.X, 2) + Math.Pow(end.Y - start.Y, 2));
                    if (distance > minSwipeLength)
                    {
                        ProcessSwipe(moveCaches[0], distance, end - start);
                    }
                    else
                    {
                        foreach (var moveTouchCache in moveCaches)
                        {
//                            moveTouchCache.Process();
                        }
//                        touchCache.Process();
                    }

                    removeCaches.AddRange(moveCaches);
                    removeCaches.Add(touchCache);
                }
                else
                {
                    if (now > touchCache.Time)
                    {
//                        Console.WriteLine(DateTime.Now + "Touch  Cutoff");
                        removeCaches.Add(touchCache);
//                        touchCache.Process();
                    }
                    else
                    {
                        if (touchCache.TouchType == TouchType.TouchDown)
                        {
//                            Console.WriteLine(DateTime.Now + "Touch Down");
                            foreach (var touchCach in moveCaches)
                            {
                                removeCaches.Add(touchCach);
//                                touchCach.Process();
                            }

                            moveCaches.Clear();
                        }
                        moveCaches.Add(touchCache);
                    }
                }
            }

            foreach (var removeCach in removeCaches)
            {
                touchCaches.Remove(removeCach);
            }
        }

        private void ProcessSwipe(TouchCache start, double distance, Point direction)
        {
            foreach (var swipeRect in swipeRects)
            {
                if (swipeRect.Collides(start.X, start.Y))
                {
                    if (!swipeRect.SwipeEventToTrigger(swipeRect, start.X - swipeRect.X, start.Y - swipeRect.Y, direction, distance))
                    {
                        break;
                    }
                }
            }
        }

        public void Init()
        {
        }

        public TouchRect PushClickRect(TouchRect touchRect)
        {
            touchRects.Add(touchRect);
            return touchRect;
        }

        public TouchRect PushSwipeRect(TouchRect touchRect)
        {
            swipeRects.Add(touchRect);
            return touchRect;
        }

        public void RemoveClickRect(TouchRect touchRect)
        {
            touchRects.Remove(touchRect);
        }

        public void RemoveSwipeRect(TouchRect touchRect)
        {
            swipeRects.Remove(touchRect);
        }

        public void ClearClickRect()
        {
            touchRects.Clear();
        }

        public void ClearSwipeRect()
        {
            swipeRects.Clear();
        }

        public void ProcessTouchEvent(TouchType touchType, int x, int y)
        {
            var touchCache = new TouchCache(DateTime.Now.AddMilliseconds(MaxSwipeLengthMS), touchType, x, y, touchRects);
            touchCache.Process();
            touchCaches.Add(touchCache); //ignore result for mouseup
        }

        public void Render(ILayer layer)
        {
#if DRAWTOUCH
            foreach (var touchRect in touchRects)
            {
                layer.DrawRectangle(new Color(22, 40, 120, 150), touchRect.X, touchRect.Y, touchRect.Width, touchRect.Height);
            }
#endif
        }

        private readonly List<TouchCache> touchCaches = new List<TouchCache>();


        private class TouchCache
        {
            public TouchCache(DateTime time, TouchType touchType, int x, int y, List<TouchRect> touchRects)
            {
                Time = time;
                TouchType = touchType;
                X = x;
                Y = y;
                TouchRects = touchRects;
            }

            public List<TouchRect> TouchRects { get; set; }

            public DateTime Time { get; set; }
            public TouchType TouchType { get; set; }
            public int X { get; set; }
            public int Y { get; set; }

            public void Process()
            {
                switch (TouchType)
                {
                    case TouchType.TouchUp:
                        foreach (var clickRect in TouchRects)
                        {
                            clickRect.TouchEventToTrigger(TouchType.TouchUp, clickRect, X - clickRect.X, Y - clickRect.Y, clickRect.Collides(X, Y)); //ignore result for mouseup
                        }
                        break;
                    case TouchType.TouchMove:
                    case TouchType.TouchDown:

                        foreach (var clickRect in TouchRects)
                        {
                            if (!clickRect.Collides(X, Y)) continue;
                            if (!clickRect.TouchEventToTrigger(TouchType, clickRect, X - clickRect.X,
                                Y - clickRect.Y, true))
                            {
                                break;
                            }
                        }
                        break;
                }
            }

            public override string ToString()
            {
                return string.Format("Time: {1}, TouchType: {2}, X: {3}, Y: {4}", 0, Time, TouchType, X, Y);
            }
        }
    }
}