using System;

namespace Engine.Animation
{
    public class WaitMotion : IMotion
    {
        public void Start()
        {
            msElapsed = 0;
            lastTime = new TimeSpan();
        }

        public void Init(MotionManager motionManager)
        {
        }


        public int MsDuration { get; set; }
        private double msElapsed = 0;
        private TimeSpan lastTime;

        public WaitMotion(int msDuration)
        {
            MsDuration = msDuration;
        }

        public void Tick(TimeSpan elapsedTime)
        {
            if (lastTime.TotalMilliseconds == 0)
            {
                lastTime = elapsedTime;
            }
            msElapsed += (elapsedTime - lastTime).TotalMilliseconds;
            lastTime = elapsedTime;
            if (msElapsed >= MsDuration)
            {
                OnComplete();
            }
            PercentDone = (double) msElapsed/(double) MsDuration;
        }

        public Action OnComplete { get; set; }
        public double PercentDone { get; set; }

        public Point Reverse(Point point)
        {
            return point;
        }

        public void Reverse()
        {
        }
    }
}