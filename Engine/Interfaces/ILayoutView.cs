using System;

namespace Engine.Interfaces
{
    public interface ILayoutView
    {
        void InitLayoutView();
        void TickLayoutView(TimeSpan elapsedGameTime);
        ITouchManager TouchManager { get; }
        ILayout Layout { get; set; }
        void Render(TimeSpan elapsedGameTime);
        void Destroy();
    }

    public interface ISubLayoutView
    {
        void InitLayoutView(ITouchManager touchManager);
        void TickLayoutView(TimeSpan elapsedGameTime);
        ITouchManager TouchManager { get; }
        ILayout Layout { get; set; }
        void Render(ILayer mainLayer);
        void Destroy();
    }
}