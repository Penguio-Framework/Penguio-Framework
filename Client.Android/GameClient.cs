using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Remoting.Channels;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Content.Res;
using Android.InputMethodServices;
using Android.Opengl;
using Android.OS;
using Android.Preferences;
using Android.Views;
using Android.Views.InputMethods;
using Engine;
using Engine.Interfaces;
using Engine.Xna;
using Java.IO;
using Java.Security.Cert;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using AssetManager = Android.Content.Res.AssetManager;
using Point = Engine.Point;
using Uri = Android.Net.Uri;

namespace Client.Android
{
    public class GameClient : Microsoft.Xna.Framework.Game
    {
        public AssetManager Assets { get; set; }
        public Bundle Bundle { get; set; }
        private GraphicsDeviceManager graphics;
        private IClient client;
        private IRenderer renderer;


        public GameClient(AssetManager assets)
        {

            Assets = assets;
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferMultiSampling = true;
            graphics.ApplyChanges();
            Content.RootDirectory = "Content";
            graphics.SupportedOrientations = DisplayOrientation.Portrait | DisplayOrientation.PortraitDown;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        protected override void LoadContent()
        {
            bool opened = false;

            client = new XnaClient({{{projectName}}}, new XnaClientSettings(getKeyboardInput: (callback) =>
            {
                if (opened) return;
                opened = true;
                /*
                                    Guide.BeginShowKeyboardInput(PlayerIndex.One, "", "", "", (asyncer) =>
                                    {
                                        var endShowKeyboardInput = Guide.EndShowKeyboardInput(asyncer);
                                        opened = false;
                                        callback(endShowKeyboardInput);
                                    }, null);
                */
            },
               loadFile: (filename) =>
               {
                   var stream = Assets.Open(filename);
                   return new StreamReader(stream);
               },
               oneLayoutAtATime: true,
               openAppStore: () =>
               {
                   Intent intent = new Intent("android.intent.action.VIEW", Uri.Parse("market://details?id=" + Application.Context.PackageName));
                   intent.SetFlags(ActivityFlags.NewTask);
                   Application.Context.StartActivity(intent);
               },
               rateApp: () =>
               {
                   Intent intent = new Intent("android.intent.action.VIEW", Uri.Parse("market://details?id=" + Application.Context.PackageName));
                   intent.SetFlags(ActivityFlags.NewTask);
                   Application.Context.StartActivity(intent);
               },
                sendEmail: (to, subject, message) =>
                {
                    Intent intent = new Intent("android.intent.action.VIEW", Uri.Parse(string.Format("mailto://{0}?&subject={1}&body={2}", to, Uri.Encode(subject), Uri.Encode(message))));
                    intent.SetFlags(ActivityFlags.NewTask);
                    Application.Context.StartActivity(intent);
                }), new AndroidUserPreferences(), Content);

            renderer = new XnaRenderer(GraphicsDevice, Content, graphics, client);
            client.LoadAssets(renderer);
            client.Init(renderer);

        }


        protected override void Update(GameTime gameTime)
        {

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
            {
                Exit();
            }


            var layoutManager = client.ScreenManager.CurrentScreen;


            var dragGesture = client.DragDragGestureManager.GetGeture();
            if (dragGesture != null)
            {
                if (layoutManager.HasLayout(dragGesture.Direction))
                {
                    if (dragGesture.Distance > DragGestureManager.TriggerDistance)
                    {
                        layoutManager.ChangeLayout(dragGesture.Direction);
                        client.DragDragGestureManager.ClearDataPointsTillUp();
                    }
                }
            }

            TouchCollection touchCollection = TouchPanel.GetState();

            if (touchCollection.Count == 2)
            {
                client.DragDragGestureManager.AddDataPoint(TouchType.TouchMove, (int)touchCollection[0].Position.X, (int)touchCollection[0].Position.Y);
            }
            else
            {
                client.DragDragGestureManager.ClearDataPoints();

                for (int index = 0; index < touchCollection.Count; index++)
                {
                    var touch = touchCollection[index];

                    switch (touch.State)
                    {
                        case TouchLocationState.Moved:
                            client.TouchEvent(TouchType.TouchMove, (int)touch.Position.X, (int)touch.Position.Y);
                            break;
                        case TouchLocationState.Pressed:
                            client.TouchEvent(TouchType.TouchDown, (int)touch.Position.X, (int)touch.Position.Y);
                            break;
                        case TouchLocationState.Released:
                            client.DragDragGestureManager.TouchUp();
                            client.TouchEvent(TouchType.TouchUp, (int)touch.Position.X, (int)touch.Position.Y);
                            break;
                    }
                }
            }

            client.Tick(gameTime.TotalGameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            client.Draw(gameTime.TotalGameTime);
            base.Draw(gameTime);
        }
    }
}
