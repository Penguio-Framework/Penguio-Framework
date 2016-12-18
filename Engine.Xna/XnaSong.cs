using Engine.Interfaces;
using Microsoft.Xna.Framework.Media;

namespace Engine.Xna
{
    public class XnaSong : ISong
    {
        public Song Song { get; set; }

        public XnaSong(Song song)
        {
            Song = song;
        }
    }
}