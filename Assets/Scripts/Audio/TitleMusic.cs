using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleMusic : MonoBehaviour
{
    public static TitleMusic instance;
    
	void Awake()
    {
        if (instance != null)
            Destroy(gameObject);
        else
        {
            DontDestroyOnLoad(gameObject);
            instance = this;
        }
    }
	
	void Update()
    {
        var sceneName = SceneManager.GetActiveScene().name;
        if (sceneName.StartsWith("Level", System.StringComparison.OrdinalIgnoreCase)
            && sceneName.Length == 6)
        {
            instance = null;
            Destroy(gameObject);
        }

	}
}
