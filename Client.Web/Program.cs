using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Bridge.Html5;
using Engine.Web;
using Engine.Interfaces;

namespace Client.Web
{
    class Program
    {
        WebRenderer renderer;
        WebClient client;

        public Program()
        {
            client = new WebClient({{{projectName}}}, new WebClientSettings()
            {
                OneLayoutAtATime = false,
                GetKeyboardInput = (callback) =>
                {

                }
            },new WebUserPreferences());

            renderer = new WebRenderer(client, finishedLoadingImages);
            client.LoadImages(renderer);
            renderer.DoneLoadingAssets();

            int index = 0;
            Document.OnKeyPress = (e) =>
            {

                index = index == 0 ? 1 : 0;

                var i = 0;
                foreach (var screen in client.ScreenManager.Screens)
                {

                    if (i == index)
                    {
                        client.ScreenManager.ChangeScreen(screen);
                        break;
                    }
                    i++;
                }
            };
        }

        private void finishedLoadingImages()
        {

            client.Init(renderer);
            var startTickTime = DateTime.Now;
            var startRenderTime = DateTime.Now;
            Window.SetInterval(() =>
            {
                client.Tick((DateTime.Now - startTickTime));
            }, 1000 / 60);
            Window.SetInterval(() =>
            {
                client.Draw(DateTime.Now - startRenderTime);
            }, 1000 / 60);
        }

       public static void Main()
        {
            new Program();
        }
    }

}
