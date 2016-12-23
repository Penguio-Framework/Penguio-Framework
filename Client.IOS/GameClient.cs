using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;  
using Engine;
using Engine.Interfaces;
using Engine.Xna;
using Foundation;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input.Touch;

namespace Client.IOS
{
    public class GameClient : Game
    {
        GraphicsDeviceManager graphics;
        private IClient client;
        private IRenderer renderer;

        public GameClient()
        {
            graphics = new GraphicsDeviceManager(this)
            {
                SupportedOrientations = DisplayOrientation.Portrait
            };
            graphics.PreferMultiSampling = true;
            graphics.ApplyChanges();

            TouchPanel.EnableMouseGestures = true;
            TouchPanel.EnabledGestures = GestureType.Flick;

            Content.RootDirectory = "Content";

            graphics.IsFullScreen = true;
        }

        protected override void Initialize()
        {
            base.Initialize();
        }


        protected override void LoadContent()
        {


            bool opened = false;
            client = new XnaClient({{{projectName}}}, new XnaClientSettings(oneLayoutAtATime: true,
               getKeyboardInput: (callback) =>
                {
                    return;
                },

            
                loadFile: (filename) =>
                {
                    try
                    {
                        return new StreamReader("Assets/" + filename);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex);
                        throw ex;
                    }
                },
                rateApp: () =>
                {

                    // iRate.SharedInstance.PromptIfNetworkAvailable();
                },
                openAppStore: () =>
                {
                },
                sendEmail: (to, subject, message) =>
                {

                }), new IOsUserPreferences(), Content);

            renderer = new XnaRenderer(GraphicsDevice, Content, graphics, client);
            client.LoadAssets(renderer);

            client.Init(renderer);
        }


        protected override void Update(GameTime gameTime)
        {
            var layoutManager = client.ScreenManager.CurrentScreen;

            while (TouchPanel.IsGestureAvailable)
            {
                var gest = TouchPanel.ReadGesture();
                switch (gest.GestureType)
                {
                    case GestureType.Flick:
                        const int tolerance = 4000;
                        if (gest.Delta.X > tolerance)
                        {
                            layoutManager.ChangeLayout(Direction.Left);
                        }
                        if (gest.Delta.X < -tolerance)
                        {
                            layoutManager.ChangeLayout(Direction.Right);
                        }
                        if (gest.Delta.Y > tolerance)
                        {
                            layoutManager.ChangeLayout(Direction.Up);
                        }
                        if (gest.Delta.Y < -tolerance)
                        {
                            layoutManager.ChangeLayout(Direction.Down);

                        }
                        break;
                }
            }


            TouchCollection touchCollection = TouchPanel.GetState();


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
                        client.TouchEvent(TouchType.TouchUp, (int)touch.Position.X, (int)touch.Position.Y);
                        break;
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
        public class IOsUserPreferences : IUserPreferences
        {

            private readonly object locker = new object();

            /// <summary>
            /// Gets the current value or the default that you specify.
            /// </summary>
            /// <typeparam name="T">Vaue of t (bool, int, float, long, string)</typeparam>
            /// <param name="key">Key for settings</param>
            /// <param name="defaultValue">default value if not set</param>
            /// <returns>Value or default</returns>
            public T GetValueOrDefault<T>(string key, T defaultValue = default(T))
            {
                lock (locker)
                {
                    if (NSUserDefaults.StandardUserDefaults[key] == null)
                        return defaultValue;

                    Type typeOf = typeof(T);
                    if (typeOf.IsGenericType && typeOf.GetGenericTypeDefinition() == typeof(Nullable<>))
                    {
                        typeOf = Nullable.GetUnderlyingType(typeOf);
                    }
                    object value = null;
                    var typeCode = Type.GetTypeCode(typeOf);
                    var defaults = NSUserDefaults.StandardUserDefaults;
                    switch (typeCode)
                    {
                        case TypeCode.Boolean:
                            value = defaults.BoolForKey(key);
                            break;
                        case TypeCode.Int64:
                            var savedval = defaults.StringForKey(key);
                            value = Convert.ToInt64(savedval);
                            break;
                        case TypeCode.Double:
                            value = defaults.DoubleForKey(key);
                            break;
                        case TypeCode.String:
                            value = defaults.StringForKey(key);
                            break;
                        case TypeCode.Int32:
                            value = defaults.IntForKey(key);
                            break;
                        case TypeCode.Single:
                            value = defaults.FloatForKey(key);
                            break;

                        case TypeCode.DateTime:
                            var savedTime = defaults.StringForKey(key);
                            var ticks = string.IsNullOrWhiteSpace(savedTime) ? -1 : Convert.ToInt64(savedTime);
                            if (ticks == -1)
                                value = defaultValue;
                            else
                                value = new DateTime(ticks);
                            break;
                    }


                    return null != value ? (T)value : defaultValue;
                }
            }

            /// <summary>
            /// Adds or updates the value 
            /// </summary>
            /// <param name="key">Key for settting</param>
            /// <param name="value">Value to set</param>
            /// <returns>True of was added or updated and you need to save it.</returns>
            public bool AddOrUpdateValue(string key, object value)
            {
                lock (locker)
                {
                    Type typeOf = value.GetType();
                    if (typeOf.IsGenericType && typeOf.GetGenericTypeDefinition() == typeof(Nullable<>))
                    {
                        typeOf = Nullable.GetUnderlyingType(typeOf);
                    }
                    var typeCode = Type.GetTypeCode(typeOf);
                    var defaults = NSUserDefaults.StandardUserDefaults;
                    switch (typeCode)
                    {
                        case TypeCode.Boolean:
                            defaults.SetBool(Convert.ToBoolean(value), key);
                            break;
                        case TypeCode.Int64:
                            defaults.SetString(Convert.ToString(value), key);
                            break;
                        case TypeCode.Double:
                            defaults.SetDouble(Convert.ToDouble(value), key);
                            break;
                        case TypeCode.String:
                            defaults.SetString(Convert.ToString(value), key);
                            break;
                        case TypeCode.Int32:
                            defaults.SetInt(Convert.ToInt32(value), key);
                            break;
                        case TypeCode.Single:
                            defaults.SetFloat(Convert.ToSingle(value), key);
                            break;
                        case TypeCode.DateTime:
                            defaults.SetString(Convert.ToString(((DateTime)(object)value).Ticks), key);
                            break;
                    }
                    Save();
                }

                return true;
            }

            /// <summary>
            /// Saves all currents settings outs.
            /// </summary>
            public void Save()
            {
                try
                {
                    lock (locker)
                    {
                        var defaults = NSUserDefaults.StandardUserDefaults;
                        defaults.Synchronize();
                    }
                }
                catch (Exception)
                {
                    //TODO: log stuff here
                }
            }

        }
    }