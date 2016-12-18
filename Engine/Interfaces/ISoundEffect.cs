namespace Engine.Interfaces
{
    public interface ISoundEffect
    {
        void Play(bool repeat=false);
        void Stop();
    }
}