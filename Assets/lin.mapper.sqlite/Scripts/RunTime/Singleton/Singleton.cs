using System;
using UnityEngine;

namespace Assets.SQLite.Scripts.Singleton
{
    public class Singleton<T> where T : class, new()
    {
        private static T instance;
        private static System.Object locker = new object();
        public static T Instance()
        {
            lock (locker)
            {
                if (instance == null)
                {
                    instance = Activator.CreateInstance<T>();
                    if (instance == null)
                    {
                        Debug.LogError("Failed to create the instance of " + typeof(T) + " as singleton!");
                    }
                }
                return instance;
            }                      
        }
        public static void Release()
        {
            if (instance != null)
            {
               instance = (T)((object)null);
            }
        }
    }

    public class SingleMono<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T instance;
        private static GameObject container;
        private static System.Object locker=new object();
        public static T Instance
        {
            get
            {
                lock (locker)
                {
                    if (!instance)
                    {
                        container = new GameObject {name = typeof (T).ToString()};
                        instance = container.AddComponent(typeof(T)) as T;
                    }
                    return instance;
                }
            }
        }
    }
}