using Engine.Interfaces;

namespace Engine.Xna
{
    public class XnaContextSettings
    {
        public XnaContextSettings()
        {
            Alpha = 1;
        }

        public int Left { get; set; }
        public int Top { get; set; }
        public DrawingEffects DrawingEffects { get; set; }
        public double Alpha { get; set; }

        public XnaContextSettings Clone()
        {
            return new XnaContextSettings()
            {
                Left = Left,
                Top = Top,
                Alpha = Alpha,
                DrawingEffects = DrawingEffects
            };
        }
    }
}