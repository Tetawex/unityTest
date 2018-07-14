using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.Controller
{
    public class RestartKey : MonoBehaviour
    {
        [SerializeField]
        private KeyCode restartKey = KeyCode.R;

        void Update()
        {
            if (Input.GetKeyDown(restartKey))
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
