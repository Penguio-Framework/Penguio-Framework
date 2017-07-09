using System;

namespace Engine.Interfaces
{

    public abstract class BaseLayoutView
    {
        public virtual void InitLayoutView() { }
        public virtual void TickLayoutView(TimeSpan elapsedGameTime) { }
        public ITouchManager TouchManager { get; set; }
        public BaseLayout Layout { get; set; }
        public virtual void Render(TimeSpan elapsedGameTime) { }
        public virtual void Destroy() { }
    }

    public interface ISubLayoutView
    {
        void InitLayoutView(ITouchManager touchManager);
        void TickLayoutView(TimeSpan elapsedGameTime);
        ITouchManager TouchManager { get; }
        BaseLayout Layout { get; set; }
        void Render(ILayer mainLayer);
        void Destroy();
    }
}