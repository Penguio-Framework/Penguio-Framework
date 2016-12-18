//#define FASTMOTION
using System;
using System.Collections.Generic;
using Engine.Interfaces;

namespace Engine.Animation
{
    public class MotionManager
    {
        private int positionX;
        private int positionY;
        private readonly List<IMotion> motions = new List<IMotion>();
        private Action completed;
        private IMotion currentMotion;
        public PointF CurrentPosition { get; set; }
        public bool Completed { get; set; }

        private MotionManager(int positionX, int positionY)
        {
            this.positionX = positionX;
            this.positionY = positionY;
            CurrentPosition = new PointF(positionX, positionY);
        }

        public static MotionManager StartMotion(int positionX, int positionY)
        {
            var mm = new MotionManager(positionX, positionY);
            return mm;
        }

        public static MotionManager StartMotion(Point point)
        {
            return StartMotion(point.X, point.Y);
        }

        public static MotionManager StartMotion(int positionX, int positionY, IMotion motion)
        {
            var mm = new MotionManager(positionX, positionY);
            mm.Motion(motion);

            return mm;
        }

        public static MotionManager StartMotion(Point point, IMotion motion)
        {
            return StartMotion(point.X, point.Y, motion);
        }

        public void Reverse()
        {
            var curPos = new Point(positionX, positionY);
            foreach (var motion in motions)
            {
                curPos = motion.Reverse(curPos);
            }
            positionX = curPos.X;
            positionY = curPos.X;
            motions.Reverse();
        }

        public void Restart()
        {
            CurrentPosition = new PointF(positionX, positionY);
            foreach (var motion in motions)
            {
                motion.Init(this);
            }
            Completed = false;
            currentMotion = motions[0];
            currentMotion.Start();
        }


        public MotionManager Motion(IMotion motion)
        {
            motion.Init(this);

            if (currentMotion == null)
            {
                currentMotion = motion;
                currentMotion.Start();
            }

            motion.OnComplete += () =>
            {
                if (motions.IndexOf(currentMotion) == motions.Count - 1)
                {
                    Completed = true;
                    if (completed != null)
                    {
                        completed();
                    }
                }
                else
                {
                    currentMotion = motions[motions.IndexOf(currentMotion) + 1];
                    currentMotion.Start();
                }
            };
            motions.Add(motion);
            return this;
        }

        public MotionManager OnComplete(Action complete)
        {
            completed = complete;
            return this;
        }

        public void Render(ILayer layer)
        {
            if (Completed) return;
            RenderAnimation(layer, CurrentPosition.X, CurrentPosition.Y, motions.IndexOf(currentMotion), currentMotion.PercentDone);
        }

        public void Tick(TimeSpan elapsedTime)
        {
            if (Completed) return;
#if FASTMOTION
           currentMotion. OnComplete();
            return;
#endif
            currentMotion.Tick(elapsedTime);
        }

        public MotionManager OnRender(OnRenderAnimationIndex action)
        {
            RenderAnimation = action;
            return this;
        }

        public MotionManager OnRender(OnRender renderAnimation)
        {
            RenderAnimation = (layer, x, y, index, percent) => renderAnimation(layer, x, y);
            return this;
        }

        private OnRenderAnimationIndex RenderAnimation { get; set; }

        public void SetCurrentPosition(double x, double y)
        {
            CurrentPosition.X = x;
            CurrentPosition.Y = y;
        }

        public MotionManager DontStart()
        {
            Completed = true;
            return this;
        }
    }
}