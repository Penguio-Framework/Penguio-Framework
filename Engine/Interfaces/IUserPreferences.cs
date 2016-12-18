namespace Engine.Interfaces
{
    public interface IUserPreferences
    {
        T GetValueOrDefault<T>(string key, T defaultValue = default(T));
        bool AddOrUpdateValue(string key, object value);
    }
}