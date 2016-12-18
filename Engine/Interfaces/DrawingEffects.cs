using System;

namespace Engine.Interfaces
{
    [Flags]
    public enum DrawingEffects
    {
        None = 0,
        FlipHorizontally = 1,
        FlipVertically = 2,
    }
}