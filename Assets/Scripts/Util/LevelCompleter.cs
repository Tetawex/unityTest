using Assets.Scripts.Model;
using UnityEngine;

namespace Assets.Scripts.Util
{
    public class LevelCompleter
    {
        public static void UnlockLevel(int levelCode)
        {
            //Unlock the next level
            Debug.Log("Unlocking" + LevelStateData.LevelCodePrefsKey + (levelCode));
            PlayerPrefs.SetInt(LevelStateData.LevelCodePrefsKey + (levelCode), 1);
            PlayerPrefs.Save();
        }
    }
}
