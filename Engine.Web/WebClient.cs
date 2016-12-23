using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using Bridge.Html5;
using Engine.Interfaces;

namespace Engine.Web
{
    public class WebClient : IClient
    {
        public IScreenManager ScreenManager { get; set; }
        public ISocketManager SocketManager { get; set; }
        public IClientSettings ClientSettings { get; set; }
        public DragGestureManager DragDragGestureManager { get; set; }
        public bool SoundEnabled { get; set; }


        public IGame Game { get; set; }
        public WebRenderer Renderer { get; set; }

        public WebClient(IGame game, IClientSettings clientSettings, IUserPreferences userPreferences)
        {
            Game = game;
            ClientSettings = clientSettings;
            UserPreferences = userPreferences;
            Game.Client = this;

        }
        public void Init(IRenderer renderer)
        {

            Renderer = (WebRenderer)renderer;
            ScreenManager = new WebScreenManager(Renderer, this);
            Game.InitScreens(renderer, ScreenManager);

            /*         SocketManager = new WebSocketManager();
                     Game.InitSocketManager(SocketManager);*/
        }


        public void Draw(TimeSpan elapsedGameTime)
        {
            Game.BeforeDraw();
            ScreenManager.Draw(elapsedGameTime);
            Game.AfterDraw();
        }

        public void TouchEvent(TouchType touchType, int x, int y)
        {
            ScreenManager.TouchEvent(touchType, x, y);
        }

        public void Tick(TimeSpan elapsedGameTime)
        {
            Game.BeforeTick();

            ScreenManager.Tick(elapsedGameTime);
            Game.AfterTick();
        }

        public void SetCustomLetterbox(IImage image)
        {

        }
        public void Timeout(Action callback, int ms)
        {
            Window.SetTimeout(callback, ms);
        }

        public void Interval(Action callback, int ms)
        {
            Window.SetInterval(callback, ms);
        }

        public void PlaySong(ISong isong)
        {
            //            MediaPlayer.Stop();
            //            var song = ((XnaSong)songs[songName]).Song;
            //            MediaPlayer.IsRepeating = true;
            //            MediaPlayer.Play(song);
        }

        public ISoundEffect PlaySoundEffect(ISoundEffect sfx, bool repeat = false)
        {

            //            var sfx = ((XnaSoundEffect)soundEffects[soundEffectName]);
            //            if (SoundEnabled)
            //            {
            //                sfx.Play(repeat);
            //            }
            //            return sfx;
            return sfx;
        }

        private readonly Dictionary<string, ISoundEffect> soundEffects = new Dictionary<string, ISoundEffect>();
        private readonly Dictionary<string, ISong> songs = new Dictionary<string, ISong>();

   


        public ISoundEffect CreateSoundEffect(string soundName, string soundPath)
        {
            //            var se = ContentManager.Load<SoundEffect>(soundPath);
            //            return soundEffects[soundName] = new XnaSoundEffect(se);
            return null;
        }

        public ISong CreateSong(string songName, string songPath)
        {
            //            var se = ContentManager.Load<Song>(songPath);
            //            return songs[songName] = new XnaSong(se);
            return null;
        }


        public void DrawLetterbox()
        {
            //            throw new NotImplementedException();
        }

        public void ShowKeyboard()
        {
            //            throw new NotImplementedException();
        }

        public IUserPreferences UserPreferences { get; private set; }

        public void LoadAssets(IRenderer renderer)
        {
            Game.AssetManager=new AssetManager(renderer,this);
            Game.LoadAssets(renderer);
        }
    }
    public class WebClientSettings : IClientSettings
    {
        public bool OneLayoutAtATime { get; set; }
        public Action<Action<string>> GetKeyboardInput { get; set; }
        public T LoadXmlFile<T>(string filename)
        {
            return loadJSON<T>(filename.Replace(".xml", ".js"));
        }

        public Action OpenAppStore { get; set; }
        public Action RateApp { get; set; }
        public SendEmail SendEmail { get; set; }

        private T loadJSON<T>(string filePath)
        {
            var json = loadTextFileAjaxSync(filePath, "application/json");
            var loadJson = JSON.Parse<T>(json);
            return loadJson;
            /*
                        foreach (var cc in  loadJson)
                        {
                            return Script.Reinterpret<T>(cc.Value);
                        }

                        return default(T);
            */

        }

        private string loadTextFileAjaxSync(string filePath, string mimeType)
        {
            var xmlhttp = new XMLHttpRequest();
            xmlhttp.Open("GET", filePath, false);
            if (mimeType != null)
            {
                xmlhttp.OverrideMimeType(mimeType);
            }
            xmlhttp.Send();
            if (xmlhttp.Status == 200)
            {
                return xmlhttp.ResponseText;
            }
            else
            {
                // TODO Throw exception
                return null;
            }

        }

        /* 

        // Load text with Ajax synchronously: takes path to file and optional MIME type
        function loadTextFileAjaxSync(filePath, mimeType)
        {
     
        }*/
    }


}