using System;

namespace Engine.Interfaces
{
    public interface IClientSettings
    {
        bool OneLayoutAtATime { get; set; }
        Action<Action<string>> GetKeyboardInput { get; set; }
        T LoadXmlFile<T>(string filename);
        Action OpenAppStore { get; set; }
        Action RateApp { get; set; }
        SendEmail SendEmail { get; set; }
    }

    public delegate void SendEmail(string to, string subject, string message);
}