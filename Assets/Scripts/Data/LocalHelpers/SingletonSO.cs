using UnityEngine;

namespace GreatDevResh.Data.LocalHelpers
{
    public class SingletonSO<T> : ScriptableObject where T : ScriptableObject
    {
        private static T _instance;

        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    T[] results = Resources.FindObjectsOfTypeAll<T>();
                    if (results.Length == 0)
                    {
                        Debug.LogError("Singleton of type <" + typeof(T) + "> not found.");
                        return null;
                    }

                    if (results.Length > 1)
                    {
                        Debug.LogError("Singleton of type <" + typeof(T) + "> found too many. Amount: " + results.Length);
                        return null;
                    }

                    _instance = results[0];
                    _instance.hideFlags = HideFlags.DontUnloadUnusedAsset;
                }

                return _instance;
            }
        }
    }
}