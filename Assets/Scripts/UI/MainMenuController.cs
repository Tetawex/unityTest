using Assets.Scripts.Controller;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
namespace Assets.Scripts.UI
{
    public class MainMenuController : MonoBehaviour
    {

        private Button playButton, ironButton, quitButton;

        [SerializeField]
        private Sprite ironSprite;

        // Use this for initialization
        void Start()
        {
            GameController.IRON_MODE_ENABLED = false;
            BindViews();
            BindListeners();
        }

        // Update is called once per frame
        void Update()
        {

        }

        private void BindViews()
        {
            try
            {
                playButton = GameObject.Find("PlayButton").GetComponent<Button>();
                ironButton = GameObject.Find("IronButton").GetComponent<Button>();
                //settingsButton = GameObject.Find("SettingsButton").GetComponent<Button>();
                quitButton = GameObject.Find("QuitButton").GetComponent<Button>();

                //iron check
                bool useSpecialtexture = PlayerPrefs.GetInt("iron", 0) == 1;

                if (useSpecialtexture)
                    ironButton.image.overrideSprite = ironSprite;
            }
            catch
            {
                Debug.LogError("Failed to bind Views");
            }
        }
        private void BindListeners()
        {
            playButton.onClick.AddListener(() => { SceneManager.LoadScene("LevelSelectMenu", LoadSceneMode.Single); });
            quitButton.onClick.AddListener(() => { Application.Quit(); });
            ironButton.onClick.AddListener(() =>
            {
                GameController.IRON_MODE_ENABLED = true;
                SceneManager.LoadScene("Level1", LoadSceneMode.Single);
            });
        }
    }
}
