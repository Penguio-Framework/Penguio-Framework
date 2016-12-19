#define CATCHERROR


using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Engine;
using Engine.Interfaces;
using Engine.Xna;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using ButtonState = Microsoft.Xna.Framework.Input.ButtonState;
using Keys = Microsoft.Xna.Framework.Input.Keys;
using Point = Microsoft.Xna.Framework.Point;

namespace Client.WindowsGame
{
    public class GameClient : Game
    {
        SpriteBatch spriteBatch;
        GraphicsDeviceManager graphics;
        private IClient client;
        private IRenderer renderer;

        public GameClient()
            : base()
        {
            graphics = new GraphicsDeviceManager(this);

            graphics.PreferMultiSampling = true;
            graphics.ApplyChanges();
            var c = new ContentManager(this.Services);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            TouchPanel.EnableMouseTouchPoint = true;

            graphics.PreferredBackBufferWidth = 1536/2;
            graphics.PreferredBackBufferHeight = 2048/2;
        }

        protected override void Initialize()
        {
            base.Initialize();
        }

        private void ExceptionOccured(Exception exc)
        {
            Form form = new Form();
            form.Width = 1000;
            form.Height = 500;
            TextBox txt = new TextBox();
            txt.Multiline = true;
            txt.Font = new Font(txt.Font.FontFamily, 15);
            txt.WordWrap = false;
            txt.ScrollBars = ScrollBars.Both;
            txt.Left = txt.Top = 0;
            txt.Width = form.Width - 50;
            txt.Height = form.Height - 50;
            txt.Text = exc.ToString();
            form.Controls.Add(txt);
            form.FormClosed += (sender, args) => this.Exit();
            form.Show();
            failed = true;
        }

        protected override void LoadContent()
        {
            if (failed) return;
#if CATCHERROR
            try
            {
#endif
                spriteBatch = new SpriteBatch(GraphicsDevice);
                client = new XnaClient({{{projectName}}}, new XnaClientSettings(getKeyboardInput: (callback) =>
                {
                    callback("");
                },
                    oneLayoutAtATime: false,
                    loadFile: s => new StreamReader("Content/" + s),
                    openAppStore: () => { },
                    rateApp: () => { },
                    sendEmail: (to, subject, message) => { })
                {

                }, new WindowsUserPreferences(), Content);

                renderer = new XnaRenderer(GraphicsDevice, Content, graphics, client);
                client.LoadImages(renderer);

                client.Init(renderer);
#if CATCHERROR
            }
            catch (Exception ex)
            {
                ExceptionOccured(ex);
            }
#endif
        }

        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        private int mouseX;
        private int mouseY;
        private bool mouseIsDown;
        //        private int currentIndex = 0;
        protected override void Update(GameTime gameTime)
        {
            if (failed) return;
#if CATCHERROR
            try
            {
#endif
                if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                    Exit();



                if (Keyboard.GetState().IsKeyDown(Keys.Space))
                {
                    //                currentIndex = currentIndex == 0 ? 1 : 0;
                    //                var screen = client.ScreenManager.Screens.ElementAt(currentIndex);
                    //                client.ScreenManager.ChangeScreen(screen);
                }


                var layoutManager = client.ScreenManager.CurrentScreen;

                if (Keyboard.GetState().IsKeyDown(Keys.Left))
                {
                    layoutManager.ChangeLayout(Direction.Left);
                }
                if (Keyboard.GetState().IsKeyDown(Keys.Right))
                {
                    layoutManager.ChangeLayout(Direction.Right);
                }
                if (Keyboard.GetState().IsKeyDown(Keys.Up))
                {
                    layoutManager.ChangeLayout(Direction.Up);
                }
                if (Keyboard.GetState().IsKeyDown(Keys.Down))
                {
                    layoutManager.ChangeLayout(Direction.Down);
                }

                MouseState mouseState = Mouse.GetState();
                if (mouseX != mouseState.X || mouseY != mouseState.Y)
                {
                    mouseX = mouseState.X;
                    mouseY = mouseState.Y;
                    client.TouchEvent(TouchType.TouchMove, (int)mouseState.X, (int)mouseState.Y);
                }


                /*
                            switch (mouseState.LeftButton)
                            {
                                case ButtonState.Pressed:
                                    if (!mouseIsDown)
                                    {
                                        client.TouchEvent(TouchType.TouchDown, (int)mouseState.X, (int)mouseState.Y);
                                    }
                                    mouseIsDown = true;
                                    break;
                                case ButtonState.Released:
                                    if (mouseIsDown)
                                    {
                                        client.TouchEvent(TouchType.TouchUp, (int)mouseState.X, (int)mouseState.Y);
                                        mouseIsDown = false;
                                    }
                                    break;
                            }*/

                TouchCollection touchCollection = TouchPanel.GetState();
                foreach (var touch in touchCollection)
                {
                    switch (touch.State)
                    {
                        case TouchLocationState.Moved:
                            client.TouchEvent(TouchType.TouchMove, (int)touch.Position.X, (int)touch.Position.Y);
                            break;
                        case TouchLocationState.Pressed:
                            client.TouchEvent(TouchType.TouchDown, (int)touch.Position.X, (int)touch.Position.Y);
                            break;
                        case TouchLocationState.Released:
                            client.TouchEvent(TouchType.TouchUp, (int)touch.Position.X, (int)touch.Position.Y);
                            break;
                    }
                }

                client.Tick(gameTime.TotalGameTime);

                base.Update(gameTime);
#if CATCHERROR
            }
            catch (Exception ex)
            {
                ExceptionOccured(ex);
            }
#endif

        }

        private bool failed = false;
        protected override void Draw(GameTime gameTime)
        {
            if (failed) return;
#if CATCHERROR
            try
            {
#endif
                GraphicsDevice.Clear(Microsoft.Xna.Framework.Color.CornflowerBlue);
                client.Draw(gameTime.TotalGameTime);
                client.DrawLetterbox();
                base.Draw(gameTime);
#if CATCHERROR
            }
            catch (Exception ex)
            {
                ExceptionOccured(ex);
            }
#endif

        }
    }
}