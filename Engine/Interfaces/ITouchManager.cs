namespace Engine.Interfaces
{
    public interface ITouchManager
    {
        void Init();
        TouchRect PushClickRect(TouchRect touchRect);
        TouchRect PushSwipeRect(TouchRect touchRect);
        void RemoveClickRect(TouchRect touchRect);
        void RemoveSwipeRect(TouchRect touchRect);
        void ClearClickRect();
        void ClearSwipeRect();
        void ProcessTouchEvent(TouchType touchType, int x, int y);
        void Render(ILayer layer);
    }
}