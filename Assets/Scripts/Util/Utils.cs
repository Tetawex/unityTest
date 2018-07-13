using UnityEngine;
using System.Collections.Generic;

namespace Assets.Scripts.Util
{
    public class Utils
    {

        //public static GameObject gameController = null;
        //public static GameObject GetGameController()
        //{
        //    if (gameController == null)
        //        gameController = GameObject.Find("GameController");
        //    return gameController;
        //}
        
        static Dictionary<System.Type, object> cachedSingletons = new Dictionary<System.Type, object>();
        public static T getSingleton<T>() where T : UnityEngine.Object
        {
            var type = typeof(T);
            if (!cachedSingletons.ContainsKey(type) || (UnityEngine.Object)cachedSingletons[type] == null)
            {
                Debug.Log("Not Cached");
                cachedSingletons[type] = GameObject.FindObjectOfType<T>();
            }
            else
            {
                Debug.Log("Cached");
            }
            return (T)cachedSingletons[type];
        }
    }
}
