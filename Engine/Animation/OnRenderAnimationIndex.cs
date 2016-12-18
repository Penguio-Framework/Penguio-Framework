using Engine.Interfaces;

namespace Engine.Animation
{
    public delegate void OnRenderAnimationIndex(ILayer layer, double posX, double posY, int animationIndex, double percent);
}