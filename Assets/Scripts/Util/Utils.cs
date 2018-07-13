using UnityEngine;

namespace Assets.Scripts.Util
{
    public class Utils
    {
        public static GameObject GetGameController()
        {
            return GameObject.Find("GameController");
        }
    }
}
