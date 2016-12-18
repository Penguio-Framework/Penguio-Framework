using Engine.Interfaces;
using Microsoft.Xna.Framework.Audio;

namespace Engine.Xna
{
    public class XnaSoundEffect : ISoundEffect
    {
        public SoundEffect SoundEffect { get; set; }

        public XnaSoundEffect(SoundEffect soundEffect)
        {
            SoundEffect = soundEffect;
        }

        private SoundEffectInstance currentInstance;

        public void Play(bool repeat=false)
        {
            if (currentInstance != null)
            {
                if(currentInstance.State!=SoundState.Stopped)
                    currentInstance.Stop(true);
            }
            currentInstance = SoundEffect.CreateInstance();
            currentInstance.IsLooped = repeat;
            currentInstance.Play();
        }

        public void Stop()
        {
            if (currentInstance != null)
            {
                currentInstance.Stop();
            }
        }   
    }
}