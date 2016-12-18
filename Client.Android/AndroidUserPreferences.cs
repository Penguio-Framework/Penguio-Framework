using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Remoting.Channels;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Content.Res;
using Android.InputMethodServices;
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


    public class AndroidUserPreferences : IUserPreferences
    { 
            private static ISharedPreferences SharedPreferences { get; set; }
            private static ISharedPreferencesEditor SharedPreferencesEditor { get; set; }
            private readonly object locker = new object();

            public AndroidUserPreferences()
            {
                SharedPreferences = PreferenceManager.GetDefaultSharedPreferences(Application.Context);
                SharedPreferencesEditor = SharedPreferences.Edit();

            }
 
            public T GetValueOrDefault<T>(string key, T defaultValue = default(T))
            {
                lock (locker)
                {
                    Type typeOf = typeof(T);
                    if (typeOf.IsGenericType && typeOf.GetGenericTypeDefinition() == typeof(Nullable<>))
                    {
                        typeOf = Nullable.GetUnderlyingType(typeOf);
                    }
                    object value = null;
                    var typeCode = Type.GetTypeCode(typeOf);
                    switch (typeCode)
                    {
                        case TypeCode.Boolean:
                            value = SharedPreferences.GetBoolean(key, Convert.ToBoolean(defaultValue));
                            break;
                        case TypeCode.Int64:
                            value = SharedPreferences.GetLong(key, Convert.ToInt64(defaultValue));
                            break;
                        case TypeCode.String:
                            value = SharedPreferences.GetString(key, Convert.ToString(defaultValue));
                            break;
                        case TypeCode.Int32:
                            value = SharedPreferences.GetInt(key, Convert.ToInt32(defaultValue));
                            break;
                        case TypeCode.Single:
                            value = SharedPreferences.GetFloat(key, Convert.ToSingle(defaultValue));
                            break;
                        case TypeCode.DateTime:
                            var ticks = SharedPreferences.GetLong(key, -1);
                            if (ticks == -1)
                                value = defaultValue;
                            else
                                value = new DateTime(ticks);
                            break;
                    }

                    return null != value ? (T)value : defaultValue;
                }
            }
     
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
                    switch (typeCode)
                    {
                        case TypeCode.Boolean:
                            SharedPreferencesEditor.PutBoolean(key, Convert.ToBoolean(value));
                            break;
                        case TypeCode.Int64:
                            SharedPreferencesEditor.PutLong(key, Convert.ToInt64(value));
                            break;
                        case TypeCode.String:
                            SharedPreferencesEditor.PutString(key, Convert.ToString(value));
                            break;
                        case TypeCode.Int32:
                            SharedPreferencesEditor.PutInt(key, Convert.ToInt32(value));
                            break;
                        case TypeCode.Single:
                            SharedPreferencesEditor.PutFloat(key, Convert.ToSingle(value));
                            break;
                        case TypeCode.DateTime:
                            SharedPreferencesEditor.PutLong(key, ((DateTime)(object)value).Ticks);
                            break;
                    }
                }
                Save();
                return true;
            }

            /// <summary>
            /// Saves out all current settings
            /// </summary>
            public void Save()
            {
                lock (locker)
                {
                    SharedPreferencesEditor.Commit();
                }
            }

        }
}