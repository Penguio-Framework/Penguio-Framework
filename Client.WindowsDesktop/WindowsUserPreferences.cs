using Engine.Interfaces;

namespace Client
{
    public class WindowsUserPreferences : IUserPreferences
    {
        /// <summary>
        /// Gets the current value or the default that you specify.
        /// </summary>
        /// <typeparam name="T">Vaue of t (bool, int, float, long, string)</typeparam>
        /// <param name="key">Key for settings</param>
        /// <param name="defaultValue">default value if not set</param>
        /// <returns>Value or default</returns>
        public T GetValueOrDefault<T>(string key, T defaultValue = default(T))
        {
            return defaultValue;
        }

        /// <summary>
        /// Adds or updates the value 
        /// </summary>
        /// <param name="key">Key for settting</param>
        /// <param name="value">Value to set</param>
        /// <returns>True of was added or updated and you need to save it.</returns>
        public bool AddOrUpdateValue(string key, object value)
        {

            return false;
        }

        /// <summary>
        /// Saves any changes out.
        /// </summary>
        public void Save()
        {
        }
    }
}