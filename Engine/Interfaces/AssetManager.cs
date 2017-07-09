using System.Collections.Generic;

namespace Engine.Interfaces
{
    public class AssetManager
    {
        public IRenderer Renderer { get; set; }
        public BaseClient Client { get; set; }

        public AssetManager(IRenderer renderer, BaseClient client)
        {
            Renderer = renderer;
            Client = client;
        }

        public IImage CreateImage(string imagePath)
        {
            return Renderer.CreateImage(imagePath);
        }

        public ISoundEffect CreateSoundEffect(string soundPath)
        {
            return Client.CreateSoundEffect(soundPath);
        }

        public ISong CreateSong(string songPath)
        {
            return Client.CreateSong(songPath);
        }

        public Dictionary<int, IImage> CreateImage(string imagePath, int[] arguments)
        {
            Dictionary<int, IImage> images = new Dictionary<int, IImage>();

            foreach (var o in arguments)
            {
                images.Add(o, Renderer.CreateImage(string.Format(imagePath, o)));
            }
            return images;
        }
        public Dictionary<int, Dictionary<int, IImage>> CreateImage(string imagePath, int[] outerArgs, int[] innerArgs)
        {
            Dictionary<int, Dictionary<int, IImage>> images = new Dictionary<int, Dictionary<int, IImage>>();

            foreach (var outer in outerArgs)
            {
                Dictionary<int, IImage> innerImages = new Dictionary<int, IImage>();
                foreach (var inner in innerArgs)
                {
                    innerImages.Add(inner, Renderer.CreateImage(string.Format(imagePath, outer, inner)));
                }
                images.Add(outer, innerImages);


            }
            return images;
        }

        public IFont CreateFont(string fontPath)
        {
            return Renderer.CreateFont(fontPath);
        }
        public Dictionary<object, IFont> CreateFont(IEnumerable<object> arguments, string fontPath)
        {

            Dictionary<object, IFont> fonts = new Dictionary<object, IFont>();

            foreach (var o in arguments)
            {
                fonts.Add(o, Renderer.CreateFont(string.Format(fontPath, o)));

            }
            return fonts;
        }
    }
}