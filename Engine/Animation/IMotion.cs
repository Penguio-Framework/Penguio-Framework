using System;

namespace Engine.Animation
{
    public interface IMotion
    {
        void Tick(TimeSpan elapsedTime);
        void Init(MotionManager motionManager);
        void Start();
        int MsDuration { get; set; }
        Action OnComplete { get; set; }
        double PercentDone { get; set; }
        Point Reverse(Point point);
    }
}