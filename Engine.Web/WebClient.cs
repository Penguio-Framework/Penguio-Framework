using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using Bridge.Html5;
using Engine.Interfaces;

namespace Engine.Web
{
    public class WebClient : BaseClient
    {
        public ISocketManager SocketManager { get; set; }
        public override bool SoundEnabled { get; set; }

        public WebRenderer Renderer { get; set; }

        public WebClient(BaseGame game, IClientSettings clientSettings, IUserPreferences userPreferences):base(game,clientSettings,userPreferences)
        {
            Game.Client = this;
        }

        public override void Init(IRenderer renderer)
        {

            Renderer = (WebRenderer)renderer;
            ScreenManager = new WebScreenManager(Renderer, this);
            Game.ScreenManager = ScreenManager;
            Game.InitScreens(renderer);

            /*         SocketManager = new WebSocketManager();
                     Game.InitSocketManager(SocketManager);*/
        }


        public override void Draw(TimeSpan elapsedGameTime)
        {
            Game.BeforeDraw();
            ScreenManager.Draw(elapsedGameTime);
            Game.AfterDraw();
        }

        public override void TouchEvent(TouchType touchType, int x, int y)
        {
            ScreenManager.TouchEvent(touchType, x, y);
        }

        public override void Tick(TimeSpan elapsedGameTime)
        {
            Game.BeforeTick();

            ScreenManager.Tick(elapsedGameTime);
            Game.AfterTick();
        }

        public override void SetCustomLetterbox(IImage image)
        {

        }
        public override void Timeout(Action callback, int ms)
        {
            Window.SetTimeout(callback, ms);
        }

        public override void Interval(Action callback, int ms)
        {
            Window.SetInterval(callback, ms);
        }

        public override void PlaySong(ISong isong)
        {
            //            MediaPlayer.Stop();
            //            var song = ((XnaSong)songs[songName]).Song;
            //            MediaPlayer.IsRepeating = true;
            //            MediaPlayer.Play(song);
        }

        public override ISoundEffect PlaySoundEffect(ISoundEffect sfx, bool repeat = false)
        {

            //            var sfx = ((XnaSoundEffect)soundEffects[soundEffectName]);
            //            if (SoundEnabled)
            //            {
            //                sfx.Play(repeat);
            //            }
            //            return sfx;
            return sfx;
        }
        


        public override ISoundEffect CreateSoundEffect(string soundPath)
        {
            //            var se = ContentManager.Load<SoundEffect>(soundPath);
            //            return soundEffects[soundName] = new XnaSoundEffect(se);
            return null;
        }

        public override ISong CreateSong(string songPath)
        {
            //            var se = ContentManager.Load<Song>(songPath);
            //            return songs[songName] = new XnaSong(se);
            return null;
        }


        public override void DrawLetterbox()
        {
            //            throw new NotImplementedException();
        }

        public void ShowKeyboard()
        {
            //            throw new NotImplementedException();
        }

        public override void LoadAssets(IRenderer renderer)
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