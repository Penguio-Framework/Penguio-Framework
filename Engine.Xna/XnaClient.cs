//#define Mute

using System;
using System.Collections.Generic;
using Engine.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;

namespace Engine.Xna
{
    public class XnaClient : IClient
    {
        public IGame Game { get; set; }
        public XnaRenderer Renderer { get; set; }
        public IScreenManager ScreenManager { get; set; }
        public IClientSettings ClientSettings { get; set; }
        public ContentManager ContentManager { get; set; }
        public DragGestureManager DragDragGestureManager { get; set; }
        private readonly Dictionary<string, ISoundEffect> soundEffects = new Dictionary<string, ISoundEffect>();
        private readonly Dictionary<string, ISong> songs = new Dictionary<string, ISong>();
        private bool soundEnabled;

        public IUserPreferences UserPreferences { get; private set; }


        private float preSound;

        public bool SoundEnabled
        {
            get { return soundEnabled; }
            set
            {
                soundEnabled = value;
                if (soundEnabled)
                {
                    MediaPlayer.Volume = preSound;
                }
                else
                {
                    preSound = MediaPlayer.Volume;
                    MediaPlayer.Volume = 0;
                }
            }
        }

        public XnaClient(IGame game, IClientSettings clientSettings, IUserPreferences userPreferences, ContentManager contentManager)
        {
            preSound = MediaPlayer.Volume;
            SoundEnabled =
#if Mute
                false
#else
                true
#endif
                ;
            ClientSettings = clientSettings;
            UserPreferences = userPreferences;
            ContentManager = contentManager;
            Game = game;
            Game.Client = this;
        }

        public void LoadAssets(IRenderer renderer)
        {
            Game.AssetManager = new AssetManager(renderer, this);
            Game.LoadAssets(renderer);
        }

        public void Init(IRenderer renderer)
        {
            Renderer = (XnaRenderer)renderer;
            ScreenManager = new XnaScreenManager(Renderer, this);
            Game.InitScreens(renderer, ScreenManager);
            DragDragGestureManager = new DragGestureManager();

            overlaySpriteBatch = new SpriteBatch(Renderer.graphicsDevice);

            letterboxSpriteBatch = new SpriteBatch(Renderer.graphicsDevice);
            //            renderer.CreateImage("overlay.arrow", "images/overlays/arrow.png");


            /*
                        var size = screen.GetLayoutSize();

                        Renderer.graphics.PreferredBackBufferWidth = size.Width;
                        Renderer.graphics.PreferredBackBufferHeight = size.Height;*/
        }


        private SpriteBatch overlaySpriteBatch;
        private SpriteBatch letterboxSpriteBatch;

        public void Draw(TimeSpan elapsedGameTime)
        {
            Game.BeforeDraw();
            ScreenManager.Draw(elapsedGameTime);


            var layoutManager = ScreenManager.CurrentScreen;

            var dragGesture = DragDragGestureManager.GetGeture();
            if (dragGesture != null)
            {
                if (layoutManager.HasLayout(dragGesture.Direction))
                {
                    var scaleMatrix = Renderer.GetScaleMatrix();
                    overlaySpriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, null, null, null, null, scaleMatrix);

                    var location = new Vector2();
                    var quarter = Math.PI * 2 / 4;
                    var distance = (float)dragGesture.Distance / (float)DragGestureManager.TriggerDistance;
                    float angle = 0;
                    var size = ScreenManager.CurrentScreen.GetLayoutSize();
                    switch (dragGesture.Direction)
                    {
                        case Direction.Left:
                            angle = (float)(quarter * 3);
                            location.X = 50;
                            location.Y = size.Height / 2;
                            break;
                        case Direction.Right:
                            angle = (float)(quarter * 1);
                            location.X = size.Width - 50;
                            location.Y = size.Height / 2;
                            break;
                        case Direction.Up:
                            angle = 0;
                            location.X = size.Width / 2;
                            location.Y = 50;
                            break;
                        case Direction.Down:
                            angle = (float)(quarter * 2);
                            location.X = size.Width / 2;
                            location.Y = size.Height - 50;
                            break;
                    }

                    var xnaImage = ((XnaImage)Renderer.GetImage("overlay.arrow"));
                    overlaySpriteBatch.Draw(xnaImage.Texture, location, null, Microsoft.Xna.Framework.Color.White * distance, angle, new Vector2((float)xnaImage.Center.X, (float)xnaImage.Center.Y), 1, SpriteEffects.None, 1);
                    overlaySpriteBatch.End();
                }
            }


            DrawLetterbox();
            Game.AfterDraw();
        }

        public void TouchEvent(TouchType touchType, int x, int y)
        {
            var matrix = Renderer.GetScaleMatrix();

            var screenSize = Renderer.GetScreenSize();
            var point = Renderer.GetOffset();

            if (y < point.Y)
            {
                return;
            }
            if (y > screenSize.Y + point.Y)
            {
                return;
            }

            if (x < point.X)
            {
                return;
            }
            if (x > screenSize.X + point.X)
            {
                return;
            }

            x -= point.X;
            y -= point.Y;


            x = (int)(x / matrix.Right.Length());
            y = (int)(y / matrix.Down.Length());


            ScreenManager.TouchEvent(touchType, x, y);
        }

        public void DrawLetterbox()
        {
            var screenSize = Renderer.GetScreenSize();
            var point = Renderer.GetOffset();

            if (point.X == 0 && point.Y == 0)
                return; //no offset to center the image, so we do not need a letterbox

            letterboxSpriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, null, null, null, null);
            letterboxSpriteBatch.Draw(LetterboxTexture, new Vector2(0, 0), null, Microsoft.Xna.Framework.Color.White);
            letterboxSpriteBatch.Draw(LetterboxTexture, new Vector2(point.X, point.Y + screenSize.Y), null, Microsoft.Xna.Framework.Color.White);
            letterboxSpriteBatch.End();
        }

        private Texture2D letterboxTexture;

        public Texture2D LetterboxTexture
        {
            get
            {
                if (letterboxTexture == null)
                {
                    var size = Renderer.GetScreenSize();
                    var point = Renderer.GetOffset();
                    var width = (point.X == 0 ? size.X : point.X) + 6;
                    var height = (point.Y == 0 ? size.Y : point.Y) + 4;
                    letterboxTexture = new Texture2D(Renderer.graphicsDevice, width, height);
                    var xnaColor = new Microsoft.Xna.Framework.Color(0, 0, 0);
                    var data = new Microsoft.Xna.Framework.Color[width * height];
                    for (var i = 0; i < data.Length; ++i) data[i] = xnaColor;
                    letterboxTexture.SetData(data);
                }
                return letterboxTexture;
            }
        }


        public void Timeout(Action callback, int ms)
        {
            ScreenManager.Timeout(callback, ms);
        }

        public void Interval(Action callback, int ms)
        {
            ScreenManager.Interval(callback, ms);
        }

        public void PlaySong(ISong isong)
        {
            MediaPlayer.Stop();
            var song = ((XnaSong)isong).Song;
            MediaPlayer.IsRepeating = true;
            MediaPlayer.Play(song);
        }

        public ISoundEffect PlaySoundEffect(ISoundEffect isfx, bool repeat = false)
        {
            var sfx = ((XnaSoundEffect)isfx);
            if (SoundEnabled)
            {
                sfx.Play(repeat);
            }
            return sfx;
        }



        public ISoundEffect CreateSoundEffect(string soundName, string soundPath)
        {
            var se = ContentManager.Load<SoundEffect>(soundPath);
            return soundEffects[soundName] = new XnaSoundEffect(se);
        }

        public ISong CreateSong(string songName, string songPath)
        {
            var se = ContentManager.Load<Song>(songPath);
            return songs[songName] = new XnaSong(se);
        }


        public void Tick(TimeSpan elapsedGameTime)
        {
            Game.BeforeTick();

            ScreenManager.Tick(elapsedGameTime);
            Game.AfterTick();
        }
    }
}