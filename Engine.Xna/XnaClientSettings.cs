using System;
using System.IO;
using System.Xml.Serialization;
using Engine.Interfaces;

namespace Engine.Xna
{
    //todo make this a class that client has to implement, not xna. that way we can get rid of this action nonesense
    public class XnaClientSettings : IClientSettings
    {
        public XnaClientSettings(bool oneLayoutAtATime, Action<Action<string>> getKeyboardInput, Func<string, StreamReader> loadFile, Action openAppStore, Action rateApp, SendEmail sendEmail)
        {
            OneLayoutAtATime = oneLayoutAtATime;
            GetKeyboardInput = getKeyboardInput;
            LoadFile = loadFile;
            OpenAppStore = openAppStore;
            RateApp = rateApp;
            SendEmail = sendEmail;
        }

        public bool OneLayoutAtATime { get; set; }
        public Action<Action<string>> GetKeyboardInput { get; set; }
        public Func<string, StreamReader> LoadFile { get; set; }
        public Action OpenAppStore { get; set; }
        public Action RateApp { get; set; }
        public SendEmail SendEmail { get; set; }

        public T LoadXmlFile<T>(string filename)
        {
            var streamReader = LoadFile(filename);
            if (streamReader == null) return default(T);
            using (var stream = streamReader)
            {
                var ser = new XmlSerializer(typeof (T));
                return (T) ser.Deserialize(stream);
            }
        }
    }
}