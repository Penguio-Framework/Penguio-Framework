using System;
using System.Collections.Generic;

namespace Engine.Animation
{
    public class AnimationMotion : IMotion
    {
        public int EndX { get; set; }
        public int EndY { get; set; }
        public int MsDuration { get; set; }
        public AnimationEasing Easing { get; set; }


        public Action OnComplete { get; set; }
        public double PercentDone { get; set; }

        public Point Reverse(Point point)
        {
            var ret = new Point(EndX, EndY);

            EndX = point.X;
            EndY = point.Y;

            return ret;
        }

        private PointF startPosition;
        private double msElapsed = 0;
        private TimeSpan lastTime;
        private MotionManager motionManager;

        public AnimationMotion(int endX, int endY, int msDuration, AnimationEasing easing)
        {
            EndX = endX;
            EndY = endY;
            MsDuration = msDuration;
            Easing = easing;
        }

        public AnimationMotion(Point end, int msDuration, AnimationEasing easing)
            : this(end.X, end.Y, msDuration, easing)
        {
        }


        public void Init(MotionManager motionManager)
        {
            this.motionManager = motionManager;
        }

        public void Start()
        {
            startPosition = new PointF(motionManager.CurrentPosition);
            msElapsed = 0;
            lastTime = new TimeSpan(0);
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
                return;
            }
            motionManager.SetCurrentPosition(ease(startPosition.X, EndX - startPosition.X), motionManager.CurrentPosition.Y = ease(startPosition.Y, EndY - startPosition.Y));
            PercentDone = (double) msElapsed/(double) MsDuration;
        }


        private double ease(double b, double c)
        {
            return easingLibrary[Easing](msElapsed, b, c, MsDuration);
        }

        private static readonly Dictionary<AnimationEasing, Func<double, double, double, double, double>> easingLibrary =
            new Dictionary<AnimationEasing, Func<double, double, double, double, double>>()
            {
                {AnimationEasing.Linear, Linear},
                {AnimationEasing.QuadEaseOut, QuadEaseOut},
                {AnimationEasing.QuadEaseIn, QuadEaseIn},
                {AnimationEasing.QuadEaseInOut, QuadEaseInOut},
                {AnimationEasing.QuadEaseOutIn, QuadEaseOutIn},
                {AnimationEasing.ExpoEaseOut, ExpoEaseOut},
                {AnimationEasing.ExpoEaseIn, ExpoEaseIn},
                {AnimationEasing.ExpoEaseInOut, ExpoEaseInOut},
                {AnimationEasing.ExpoEaseOutIn, ExpoEaseOutIn},
                {AnimationEasing.CubicEaseOut, CubicEaseOut},
                {AnimationEasing.CubicEaseIn, CubicEaseIn},
                {AnimationEasing.CubicEaseInOut, CubicEaseInOut},
                {AnimationEasing.CubicEaseOutIn, CubicEaseOutIn},
                {AnimationEasing.QuartEaseOut, QuartEaseOut},
                {AnimationEasing.QuartEaseIn, QuartEaseIn},
                {AnimationEasing.QuartEaseInOut, QuartEaseInOut},
                {AnimationEasing.QuartEaseOutIn, QuartEaseOutIn},
                {AnimationEasing.QuintEaseOut, QuintEaseOut},
                {AnimationEasing.QuintEaseIn, QuintEaseIn},
                {AnimationEasing.QuintEaseInOut, QuintEaseInOut},
                {AnimationEasing.QuintEaseOutIn, QuintEaseOutIn},
                {AnimationEasing.CircEaseOut, CircEaseOut},
                {AnimationEasing.CircEaseIn, CircEaseIn},
                {AnimationEasing.CircEaseInOut, CircEaseInOut},
                {AnimationEasing.CircEaseOutIn, CircEaseOutIn},
                {AnimationEasing.SineEaseOut, SineEaseOut},
                {AnimationEasing.SineEaseIn, SineEaseIn},
                {AnimationEasing.SineEaseInOut, SineEaseInOut},
                {AnimationEasing.SineEaseOutIn, SineEaseOutIn},
                {AnimationEasing.ElasticEaseOut, ElasticEaseOut},
                {AnimationEasing.ElasticEaseIn, ElasticEaseIn},
                {AnimationEasing.ElasticEaseInOut, ElasticEaseInOut},
                {AnimationEasing.ElasticEaseOutIn, ElasticEaseOutIn},
                {AnimationEasing.BounceEaseOut, BounceEaseOut},
                {AnimationEasing.BounceEaseIn, BounceEaseIn},
                {AnimationEasing.BounceEaseInOut, BounceEaseInOut},
                {AnimationEasing.BounceEaseOutIn, BounceEaseOutIn},
                {AnimationEasing.BackEaseOut, BackEaseOut},
                {AnimationEasing.BackEaseIn, BackEaseIn},
                {AnimationEasing.BackEaseInOut, BackEaseInOut},
                {AnimationEasing.BackEaseOutIn, BackEaseOutIn},
            };

        #region Equations

        #region Linear

        /// <summary>
        ///     Easing equation function for a simple linear tweening, with no easing.
        /// </summary>
        /// <param name="t">Current time in seconds.</param>
        /// <param name="b">Starting value.</param>
        /// <param name="c">Final value.</param>
        /// <param name="d">Duration of animation.</param>
        /// <returns>The correct value.</returns>
        public static double Linear(double t, double b, double c, double d)
        {
            return c*t/d + b;
        }

        #endregion

        #region Expo

        /// <summary>
        ///     Easing equation function for an exponential (2^t) easing out:
        ///     decelerating from zero velocity.
        /// </summary>
        /// <param name="t">Current time in seconds.</param>
        /// <param name="b">Starting value.</param>
        /// <param name="c">Final value.</param>
        /// <param name="d">Duration of animation.</param>
        /// <returns>The correct value.</returns>
        public static double ExpoEaseOut(double t, double b, double c, double d)
        {
            return (t == d) ? b + c : c*(-Math.Pow(2, -10*t/d) + 1) + b;
        }

        /// <summary>
        ///     Easing equation function for an exponential (2^t) easing in:
        ///     accelerating from zero velocity.
        /// </summary>
        /// <param name="t">Current time in seconds.</param>
        /// <param name="b">Starting value.</param>
        /// <param name="c">Final value.</param>
        /// <param name="d">Duration of animation.</param>
        /// <returns>The correct value.</returns>
        public static double ExpoEaseIn(double t, double b, double c, double d)
        {
            return (t == 0) ? b : c*Math.Pow(2, 10*(t/d - 1)) + b;
        }

        /// <summary>
        ///     Easing equation function for an exponential (2^t) easing in/out:
        ///     acceleration until halfway, then deceleration.
        /// </summary>
        /// <param name="t">Current time in seconds.</param>
        /// <param name="b">Starting value.</param>
        /// <param name="c">Final value.</param>
        /// <param name="d">Duration of animation.</param>
        /// <returns>The correct value.</returns>
        public static double ExpoEaseInOut(double t, double b, double c, double d)
        {
            if (t == 0)
                return b;

            if (t == d)
                return b + c;

            if ((t /= d/2) < 1)
                return c/2*Math.Pow(2, 10*(t - 1)) + b;

            return c/2*(-Math.Pow(2, -10*--t) + 2) + b;
        }

        /// <summary>
        ///     Easing equation function for an exponential (2^t) easing out/in:
        ///     deceleration until halfway, then acceleration.
        /// </summary>
        /// <param name="t">Current time in seconds.</param>
        /// <param name="b">Starting value.</param>
        /// <param name="c">Final value.</param>
        /// <param name="d">Duration of animation.</param>
        /// <returns>The correct value.</returns>
        public static double ExpoEaseOutIn(double t, double b, double c, double d)
        {
            if (t < d/2)
                return ExpoEaseOut(t*2, b, c/2, d);

            return ExpoEaseIn((t*2) - d, b + c/2, c/2, d);
        }

        #endregion

        #region Circular

        /// <summary>
        ///     Easing equation function for a circular (sqrt(1-t^2)) easing out:
        ///     decelerating from zero velocity.
        /// </summary>
        /// <param name="t">Current time in seconds.</param>
        /// <param name="b">Starting value.</param>
        /// <param name="c">Final value.</param>
        /// <param name="d">Duration of animation.</param>
        /// <returns>The correct value.</returns>
        public static double CircEaseOut(double t, double b, double c, double d)
        {
            return c*Math.Sqrt(1 - (t = t/d - 1)*t) + b;
        }

        /// <summary>
        ///     Easing equation function for a circular (sqrt(1-t^2)) easing in:
        ///     accelerating from zero velocity.
        /// </summary>
        /// <param name="t">Current time in seconds.</param>
        /// <param name="b">Starting value.</param>
        /// <param name="c">Final value.</param>
        /// <param name="d">Duration of animation.</param>
        /// <returns>The correct value.</returns>
        public static double CircEaseIn(double t, double b, double c, double d)
        {
            return -c*(Math.Sqrt(1 - (t /= d)*t) - 1) + b;
        }

        /// <summary>
        ///     Easing equation function for a circular (sqrt(1-t^2)) easing in/out:
        ///     acceleration until halfway, then deceleration.
        /// </summary>
        /// <param name="t">Current time in seconds.</param>
        /// <param name="b">Starting value.</param>
        /// <param name="c">Final value.</param>
        /// <param name="d">Duration of animation.</param>
        /// <returns>The correct value.</returns>
        public static double CircEaseInOut(double t, double b, double c, double d)
        {
            if ((t /= d/2) < 1)
                return -c/2*(Math.Sqrt(1 - t*t) - 1) + b;

            return c/2*(Math.Sqrt(1 - (t -= 2)*t) + 1) + b;
        }

        /// <summary>
        ///     Easing equation function for a circular (sqrt(1-t^2)) easing in/out:
        ///     acceleration until halfway, then deceleration.
        /// </summary>
        /// <param name="t">Current time in seconds.</param>
        /// <param name="b">Starting value.</param>
        /// <param name="c">Final value.</param>
        /// <param name="d">Duration of animation.</param>
        /// <returns>The correct value.</returns>
        public static double CircEaseOutIn(double t, double b, double c, double d)
        {
            if (t < d/2)
                return CircEaseOut(t*2, b, c/2, d);

            return CircEaseIn((t*2) - d, b + c/2, c/2, d);
        }

        #endregion

        #region Quad

        /// <summary>
        ///     Easing equation function for a quadratic (t^2) easing out:
        ///     decelerating from zero velocity.
        /// </summary>
        /// <param name="t">Current time in seconds.</param>
        /// <param name="b">Starting value.</param>
        /// <param name="c">Final value.</param>
        /// <param name="d">Duration of animation.</param>
        /// <returns>The correct value.</returns>
        public static double QuadEaseOut(double t, double b, double c, double d)
        {
            return -c*(t /= d)*(t - 2) + b;
        }

        /// <summary>
        ///     Easing equation function for a quadratic (t^2) easing in:
        ///     accelerating from zero velocity.
        /// </summary>
        /// <param name="t">Current time in seconds.</param>
        /// <param name="b">Starting value.</param>
        /// <param name="c">Final value.</param>
        /// <param name="d">Duration of animation.</param>
        /// <returns>The correct value.</returns>
        public static double QuadEaseIn(double t, double b, double c, double d)
        {
            return c*(t /= d)*t + b;
        }

        /// <summary>
        ///     Easing equation function for a quadratic (t^2) easing in/out:
        ///     acceleration until halfway, then deceleration.
        /// </summary>
        /// <param name="t">Current time in seconds.</param>
        /// <param name="b">Starting value.</param>
        /// <param name="c">Final value.</param>
        /// <param name="d">Duration of animation.</param>
        /// <returns>The correct value.</returns>
        public static double QuadEaseInOut(double t, double b, double c, double d)
        {
            if ((t /= d/2) < 1)
                return c/2*t*t + b;

            return -c/2*((--t)*(t - 2) - 1) + b;
        }

        /// <summary>
        ///     Easing equation function for a quadratic (t^2) easing out/in:
        ///     deceleration until halfway, then acceleration.
        /// </summary>
        /// <param name="t">Current time in seconds.</param>
        /// <param name="b">Starting value.</param>
        /// <param name="c">Final value.</param>
        /// <param name="d">Duration of animation.</param>
        /// <returns>The correct value.</returns>
        public static double QuadEaseOutIn(double t, double b, double c, double d)
        {
            if (t < d/2)
                return QuadEaseOut(t*2, b, c/2, d);

            return QuadEaseIn((t*2) - d, b + c/2, c/2, d);
        }

        #endregion

        #region Sine

        /// <summary>
        ///     Easing equation function for a sinusoidal (sin(t)) easing out:
        ///     decelerating from zero velocity.
        /// </summary>
        /// <param name="t">Current time in seconds.</param>
        /// <param name="b">Starting value.</param>
        /// <param name="c">Final value.</param>
        /// <param name="d">Duration of animation.</param>
        /// <returns>The correct value.</returns>
        public static double SineEaseOut(double t, double b, double c, double d)
        {
            return c*Math.Sin(t/d*(Math.PI/2)) + b;
        }

        /// <summary>
        ///     Easing equation function for a sinusoidal (sin(t)) easing in:
        ///     accelerating from zero velocity.
        /// </summary>
        /// <param name="t">Current time in seconds.</param>
        /// <param name="b">Starting value.</param>
        /// <param name="c">Final value.</param>
        /// <param name="d">Duration of animation.</param>
        /// <returns>The correct value.</returns>
        public static double SineEaseIn(double t, double b, double c, double d)
        {
            return -c*Math.Cos(t/d*(Math.PI/2)) + c + b;
        }

        /// <summary>
        ///     Easing equation function for a sinusoidal (sin(t)) easing in/out:
        ///     acceleration until halfway, then deceleration.
        /// </summary>
        /// <param name="t">Current time in seconds.</param>
        /// <param name="b">Starting value.</param>
        /// <param name="c">Final value.</param>
        /// <param name="d">Duration of animation.</param>
        /// <returns>The correct value.</returns>
        public static double SineEaseInOut(double t, double b, double c, double d)
        {
            if ((t /= d/2) < 1)
                return c/2*(Math.Sin(Math.PI*t/2)) + b;

            return -c/2*(Math.Cos(Math.PI*--t/2) - 2) + b;
        }

        /// <summary>
        ///     Easing equation function for a sinusoidal (sin(t)) easing in/out:
        ///     deceleration until halfway, then acceleration.
        /// </summary>
        /// <param name="t">Current time in seconds.</param>
        /// <param name="b">Starting value.</param>
        /// <param name="c">Final value.</param>
        /// <param name="d">Duration of animation.</param>
        /// <returns>The correct value.</returns>
        public static double SineEaseOutIn(double t, double b, double c, double d)
        {
            if (t < d/2)
                return SineEaseOut(t*2, b, c/2, d);

            return SineEaseIn((t*2) - d, b + c/2, c/2, d);
        }

        #endregion

        #region Cubic

        /// <summary>
        ///     Easing equation function for a cubic (t^3) easing out:
        ///     decelerating from zero velocity.
        /// </summary>
        /// <param name="t">Current time in seconds.</param>
        /// <param name="b">Starting value.</param>
        /// <param name="c">Final value.</param>
        /// <param name="d">Duration of animation.</param>
        /// <returns>The correct value.</returns>
        public static double CubicEaseOut(double t, double b, double c, double d)
        {
            return c*((t = t/d - 1)*t*t + 1) + b;
        }

        /// <summary>
        ///     Easing equation function for a cubic (t^3) easing in:
        ///     accelerating from zero velocity.
        /// </summary>
        /// <param name="t">Current time in seconds.</param>
        /// <param name="b">Starting value.</param>
        /// <param name="c">Final value.</param>
        /// <param name="d">Duration of animation.</param>
        /// <returns>The correct value.</returns>
        public static double CubicEaseIn(double t, double b, double c, double d)
        {
            return c*(t /= d)*t*t + b;
        }

        /// <summary>
        ///     Easing equation function for a cubic (t^3) easing in/out:
        ///     acceleration until halfway, then deceleration.
        /// </summary>
        /// <param name="t">Current time in seconds.</param>
        /// <param name="b">Starting value.</param>
        /// <param name="c">Final value.</param>
        /// <param name="d">Duration of animation.</param>
        /// <returns>The correct value.</returns>
        public static double CubicEaseInOut(double t, double b, double c, double d)
        {
            if ((t /= d/2) < 1)
                return c/2*t*t*t + b;

            return c/2*((t -= 2)*t*t + 2) + b;
        }

        /// <summary>
        ///     Easing equation function for a cubic (t^3) easing out/in:
        ///     deceleration until halfway, then acceleration.
        /// </summary>
        /// <param name="t">Current time in seconds.</param>
        /// <param name="b">Starting value.</param>
        /// <param name="c">Final value.</param>
        /// <param name="d">Duration of animation.</param>
        /// <returns>The correct value.</returns>
        public static double CubicEaseOutIn(double t, double b, double c, double d)
        {
            if (t < d/2)
                return CubicEaseOut(t*2, b, c/2, d);

            return CubicEaseIn((t*2) - d, b + c/2, c/2, d);
        }

        #endregion

        #region Quartic

        /// <summary>
        ///     Easing equation function for a quartic (t^4) easing out:
        ///     decelerating from zero velocity.
        /// </summary>
        /// <param name="t">Current time in seconds.</param>
        /// <param name="b">Starting value.</param>
        /// <param name="c">Final value.</param>
        /// <param name="d">Duration of animation.</param>
        /// <returns>The correct value.</returns>
        public static double QuartEaseOut(double t, double b, double c, double d)
        {
            return -c*((t = t/d - 1)*t*t*t - 1) + b;
        }

        /// <summary>
        ///     Easing equation function for a quartic (t^4) easing in:
        ///     accelerating from zero velocity.
        /// </summary>
        /// <param name="t">Current time in seconds.</param>
        /// <param name="b">Starting value.</param>
        /// <param name="c">Final value.</param>
        /// <param name="d">Duration of animation.</param>
        /// <returns>The correct value.</returns>
        public static double QuartEaseIn(double t, double b, double c, double d)
        {
            return c*(t /= d)*t*t*t + b;
        }

        /// <summary>
        ///     Easing equation function for a quartic (t^4) easing in/out:
        ///     acceleration until halfway, then deceleration.
        /// </summary>
        /// <param name="t">Current time in seconds.</param>
        /// <param name="b">Starting value.</param>
        /// <param name="c">Final value.</param>
        /// <param name="d">Duration of animation.</param>
        /// <returns>The correct value.</returns>
        public static double QuartEaseInOut(double t, double b, double c, double d)
        {
            if ((t /= d/2) < 1)
                return c/2*t*t*t*t + b;

            return -c/2*((t -= 2)*t*t*t - 2) + b;
        }

        /// <summary>
        ///     Easing equation function for a quartic (t^4) easing out/in:
        ///     deceleration until halfway, then acceleration.
        /// </summary>
        /// <param name="t">Current time in seconds.</param>
        /// <param name="b">Starting value.</param>
        /// <param name="c">Final value.</param>
        /// <param name="d">Duration of animation.</param>
        /// <returns>The correct value.</returns>
        public static double QuartEaseOutIn(double t, double b, double c, double d)
        {
            if (t < d/2)
                return QuartEaseOut(t*2, b, c/2, d);

            return QuartEaseIn((t*2) - d, b + c/2, c/2, d);
        }

        #endregion

        #region Quintic

        /// <summary>
        ///     Easing equation function for a quintic (t^5) easing out:
        ///     decelerating from zero velocity.
        /// </summary>
        /// <param name="t">Current time in seconds.</param>
        /// <param name="b">Starting value.</param>
        /// <param name="c">Final value.</param>
        /// <param name="d">Duration of animation.</param>
        /// <returns>The correct value.</returns>
        public static double QuintEaseOut(double t, double b, double c, double d)
        {
            return c*((t = t/d - 1)*t*t*t*t + 1) + b;
        }

        /// <summary>
        ///     Easing equation function for a quintic (t^5) easing in:
        ///     accelerating from zero velocity.
        /// </summary>
        /// <param name="t">Current time in seconds.</param>
        /// <param name="b">Starting value.</param>
        /// <param name="c">Final value.</param>
        /// <param name="d">Duration of animation.</param>
        /// <returns>The correct value.</returns>
        public static double QuintEaseIn(double t, double b, double c, double d)
        {
            return c*(t /= d)*t*t*t*t + b;
        }

        /// <summary>
        ///     Easing equation function for a quintic (t^5) easing in/out:
        ///     acceleration until halfway, then deceleration.
        /// </summary>
        /// <param name="t">Current time in seconds.</param>
        /// <param name="b">Starting value.</param>
        /// <param name="c">Final value.</param>
        /// <param name="d">Duration of animation.</param>
        /// <returns>The correct value.</returns>
        public static double QuintEaseInOut(double t, double b, double c, double d)
        {
            if ((t /= d/2) < 1)
                return c/2*t*t*t*t*t + b;
            return c/2*((t -= 2)*t*t*t*t + 2) + b;
        }

        /// <summary>
        ///     Easing equation function for a quintic (t^5) easing in/out:
        ///     acceleration until halfway, then deceleration.
        /// </summary>
        /// <param name="t">Current time in seconds.</param>
        /// <param name="b">Starting value.</param>
        /// <param name="c">Final value.</param>
        /// <param name="d">Duration of animation.</param>
        /// <returns>The correct value.</returns>
        public static double QuintEaseOutIn(double t, double b, double c, double d)
        {
            if (t < d/2)
                return QuintEaseOut(t*2, b, c/2, d);
            return QuintEaseIn((t*2) - d, b + c/2, c/2, d);
        }

        #endregion

        #region Elastic

        /// <summary>
        ///     Easing equation function for an elastic (exponentially decaying sine wave) easing out:
        ///     decelerating from zero velocity.
        /// </summary>
        /// <param name="t">Current time in seconds.</param>
        /// <param name="b">Starting value.</param>
        /// <param name="c">Final value.</param>
        /// <param name="d">Duration of animation.</param>
        /// <returns>The correct value.</returns>
        public static double ElasticEaseOut(double t, double b, double c, double d)
        {
            if ((t /= d) == 1)
                return b + c;

            var p = d*.3;
            var s = p/4;

            return (c*Math.Pow(2, -10*t)*Math.Sin((t*d - s)*(2*Math.PI)/p) + c + b);
        }

        /// <summary>
        ///     Easing equation function for an elastic (exponentially decaying sine wave) easing in:
        ///     accelerating from zero velocity.
        /// </summary>
        /// <param name="t">Current time in seconds.</param>
        /// <param name="b">Starting value.</param>
        /// <param name="c">Final value.</param>
        /// <param name="d">Duration of animation.</param>
        /// <returns>The correct value.</returns>
        public static double ElasticEaseIn(double t, double b, double c, double d)
        {
            if ((t /= d) == 1)
                return b + c;

            var p = d*.3;
            var s = p/4;

            return -(c*Math.Pow(2, 10*(t -= 1))*Math.Sin((t*d - s)*(2*Math.PI)/p)) + b;
        }

        /// <summary>
        ///     Easing equation function for an elastic (exponentially decaying sine wave) easing in/out:
        ///     acceleration until halfway, then deceleration.
        /// </summary>
        /// <param name="t">Current time in seconds.</param>
        /// <param name="b">Starting value.</param>
        /// <param name="c">Final value.</param>
        /// <param name="d">Duration of animation.</param>
        /// <returns>The correct value.</returns>
        public static double ElasticEaseInOut(double t, double b, double c, double d)
        {
            if ((t /= d/2) == 2)
                return b + c;

            var p = d*(.3*1.5);
            var s = p/4;

            if (t < 1)
                return -.5*(c*Math.Pow(2, 10*(t -= 1))*Math.Sin((t*d - s)*(2*Math.PI)/p)) + b;
            return c*Math.Pow(2, -10*(t -= 1))*Math.Sin((t*d - s)*(2*Math.PI)/p)*.5 + c + b;
        }

        /// <summary>
        ///     Easing equation function for an elastic (exponentially decaying sine wave) easing out/in:
        ///     deceleration until halfway, then acceleration.
        /// </summary>
        /// <param name="t">Current time in seconds.</param>
        /// <param name="b">Starting value.</param>
        /// <param name="c">Final value.</param>
        /// <param name="d">Duration of animation.</param>
        /// <returns>The correct value.</returns>
        public static double ElasticEaseOutIn(double t, double b, double c, double d)
        {
            if (t < d/2)
                return ElasticEaseOut(t*2, b, c/2, d);
            return ElasticEaseIn((t*2) - d, b + c/2, c/2, d);
        }

        #endregion

        #region Bounce

        /// <summary>
        ///     Easing equation function for a bounce (exponentially decaying parabolic bounce) easing out:
        ///     decelerating from zero velocity.
        /// </summary>
        /// <param name="t">Current time in seconds.</param>
        /// <param name="b">Starting value.</param>
        /// <param name="c">Final value.</param>
        /// <param name="d">Duration of animation.</param>
        /// <returns>The correct value.</returns>
        public static double BounceEaseOut(double t, double b, double c, double d)
        {
            if ((t /= d) < (1/2.75))
                return c*(7.5625*t*t) + b;
            else if (t < (2/2.75))
                return c*(7.5625*(t -= (1.5/2.75))*t + .75) + b;
            else if (t < (2.5/2.75))
                return c*(7.5625*(t -= (2.25/2.75))*t + .9375) + b;
            else
                return c*(7.5625*(t -= (2.625/2.75))*t + .984375) + b;
        }


        /// <summary>
        ///     Easing equation function for a bounce (exponentially decaying parabolic bounce) easing in:
        ///     accelerating from zero velocity.
        /// </summary>
        /// <param name="t">Current time in seconds.</param>
        /// <param name="b">Starting value.</param>
        /// <param name="c">Final value.</param>
        /// <param name="d">Duration of animation.</param>
        /// <returns>The correct value.</returns>
        public static double BounceEaseIn(double t, double b, double c, double d)
        {
            return c - BounceEaseOut(d - t, 0, c, d) + b;
        }

        /// <summary>
        ///     Easing equation function for a bounce (exponentially decaying parabolic bounce) easing in/out:
        ///     acceleration until halfway, then deceleration.
        /// </summary>
        /// <param name="t">Current time in seconds.</param>
        /// <param name="b">Starting value.</param>
        /// <param name="c">Final value.</param>
        /// <param name="d">Duration of animation.</param>
        /// <returns>The correct value.</returns>
        public static double BounceEaseInOut(double t, double b, double c, double d)
        {
            if (t < d/2)
                return BounceEaseIn(t*2, 0, c, d)*.5 + b;
            else
                return BounceEaseOut(t*2 - d, 0, c, d)*.5 + c*.5 + b;
        }

        /// <summary>
        ///     Easing equation function for a bounce (exponentially decaying parabolic bounce) easing out/in:
        ///     deceleration until halfway, then acceleration.
        /// </summary>
        /// <param name="t">Current time in seconds.</param>
        /// <param name="b">Starting value.</param>
        /// <param name="c">Final value.</param>
        /// <param name="d">Duration of animation.</param>
        /// <returns>The correct value.</returns>
        public static double BounceEaseOutIn(double t, double b, double c, double d)
        {
            if (t < d/2)
                return BounceEaseOut(t*2, b, c/2, d);
            return BounceEaseIn((t*2) - d, b + c/2, c/2, d);
        }

        #endregion

        #region Back

        /// <summary>
        ///     Easing equation function for a back (overshooting cubic easing: (s+1)*t^3 - s*t^2) easing out:
        ///     decelerating from zero velocity.
        /// </summary>
        /// <param name="t">Current time in seconds.</param>
        /// <param name="b">Starting value.</param>
        /// <param name="c">Final value.</param>
        /// <param name="d">Duration of animation.</param>
        /// <returns>The correct value.</returns>
        public static double BackEaseOut(double t, double b, double c, double d)
        {
            return c*((t = t/d - 1)*t*((1.70158 + 1)*t + 1.70158) + 1) + b;
        }

        /// <summary>
        ///     Easing equation function for a back (overshooting cubic easing: (s+1)*t^3 - s*t^2) easing in:
        ///     accelerating from zero velocity.
        /// </summary>
        /// <param name="t">Current time in seconds.</param>
        /// <param name="b">Starting value.</param>
        /// <param name="c">Final value.</param>
        /// <param name="d">Duration of animation.</param>
        /// <returns>The correct value.</returns>
        public static double BackEaseIn(double t, double b, double c, double d)
        {
            return c*(t /= d)*t*((1.70158 + 1)*t - 1.70158) + b;
        }

        /// <summary>
        ///     Easing equation function for a back (overshooting cubic easing: (s+1)*t^3 - s*t^2) easing in/out:
        ///     acceleration until halfway, then deceleration.
        /// </summary>
        /// <param name="t">Current time in seconds.</param>
        /// <param name="b">Starting value.</param>
        /// <param name="c">Final value.</param>
        /// <param name="d">Duration of animation.</param>
        /// <returns>The correct value.</returns>
        public static double BackEaseInOut(double t, double b, double c, double d)
        {
            var s = 1.70158;
            if ((t /= d/2) < 1)
                return c/2*(t*t*(((s *= (1.525)) + 1)*t - s)) + b;
            return c/2*((t -= 2)*t*(((s *= (1.525)) + 1)*t + s) + 2) + b;
        }

        /// <summary>
        ///     Easing equation function for a back (overshooting cubic easing: (s+1)*t^3 - s*t^2) easing out/in:
        ///     deceleration until halfway, then acceleration.
        /// </summary>
        /// <param name="t">Current time in seconds.</param>
        /// <param name="b">Starting value.</param>
        /// <param name="c">Final value.</param>
        /// <param name="d">Duration of animation.</param>
        /// <returns>The correct value.</returns>
        public static double BackEaseOutIn(double t, double b, double c, double d)
        {
            if (t < d/2)
                return BackEaseOut(t*2, b, c/2, d);
            return BackEaseIn((t*2) - d, b + c/2, c/2, d);
        }

        #endregion

        #endregion
    }
}