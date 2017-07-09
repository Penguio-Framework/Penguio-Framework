using System;

namespace Engine.Animation
{
    public class FinishMotion : IMotion
    {
        public void Start()
        {
        }

        public void Init(MotionManager motionManager)
        {
        }


        public int MsDuration { get; set; }

        public FinishMotion()
        {
            MsDuration = 1000*60*60*24;
        }

        public void Tick(TimeSpan elapsedTime)
        {
            
            PercentDone = 0;
        }

        public Action OnComplete { get; set; }
        public double PercentDone { get; set; }

        public Point Reverse(Point point)
        {
            return point;
        }

    }
}