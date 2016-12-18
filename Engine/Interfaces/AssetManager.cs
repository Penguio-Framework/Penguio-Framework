using System.Collections.Generic;

namespace Engine.Interfaces
{
    public class AssetManager
    {
        public IRenderer Renderer { get; set; }
        public IClient Client { get; set; }

        public AssetManager(IRenderer renderer, IClient client)
        {
            Renderer = renderer;
            Client = client;
        }

        public IImage CreateImage(string image, string imagePath)
        {
            return Renderer.CreateImage(image, imagePath);
        }

        public ISoundEffect CreateSoundEffect(string soundName, string soundPath)
        {
            return Client.CreateSoundEffect(soundName, soundPath);
        }

        public ISong CreateSong(string songName, string songPath)
        {
            return Client.CreateSong(songName, songPath);
        }

        public IEnumerable<IImage> CreateImages(string image, IEnumerable<object> arguments, string imagePath)
        {
            foreach (var o in arguments)
            {
                yield return Renderer.CreateImage(string.Format("{0}.{1}", image, o), string.Format(imagePath, o));
            }
        }

        public Dictionary<object, IImage> CreateImage(string image, IEnumerable<object> arguments, string imagePath)
        {
            Dictionary<object, IImage> images = new Dictionary<object, IImage>();

            foreach (var o in arguments)
            {
                images.Add(o, Renderer.CreateImage(string.Format("{0}.{1}", image, o), string.Format(imagePath, o)));

            }
            return images;
        }

        public IImage GetImage(string image, object argument = null)
        {
            if (argument == null)
            {
                return Renderer.GetImage(image);
            }
            else
            {
                return Renderer.GetImage(string.Format("{0}.{1}", image, argument));
            }
        }
        public IFont GetFont(string image)
        {
            return Renderer.GetFont(image);

        }
        public IFont CreateFont(string fontName, string fontPath)
        {
            return Renderer.CreateFont(fontName, fontPath);
        }
        public Dictionary<object,IFont> CreateFont(string fontName, IEnumerable<object> arguments, string fontPath)
        {

            Dictionary<object, IFont> fonts = new Dictionary<object, IFont>();

            foreach (var o in arguments)
            {
                fonts.Add(o, Renderer.CreateFont(string.Format("{0}.{1}", fontName, o), string.Format(fontPath, o)));

            }
            return fonts;
        }
    }
}