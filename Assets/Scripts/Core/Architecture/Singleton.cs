using UnityEngine;

public class Singleton<T> : object where T: Singleton<T>,new()
{
        private static T instance;
    
        public static T Instance
        {
            get
            {
                if (instance == null)
                {
                    Debug.LogError("Empty Singleton:" + typeof(T));
                }
    
                return instance;
            }
        }
    
        public Singleton()
        {
            instance = (T) this;
        }
}
