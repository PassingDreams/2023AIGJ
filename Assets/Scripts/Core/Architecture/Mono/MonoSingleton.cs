using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonoSingleton<T> : MonoBehaviour where T:MonoSingleton<T>,new()
{
        private static T instance;
    
        public static T Instance
        {
            get
            {
                if (instance == null)
                {
                    Debug.LogError("Empty MonoSingleton:" + typeof(T));
                }
    
                return instance;
            }
        }
    
        public MonoSingleton()
        {
            instance = (T) this;
        }
}
