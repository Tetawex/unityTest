using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
namespace Assets.Scripts.UI
{
    public class PlayMenuController : MonoBehaviour
    {

        private Button nextButton, restartButton, quitButton;
        private Text levelCompletedText;
        private GameObject levelCompleteWindow;

        // Use this for initialization
        void Start()
        {
            BindViews();
            BindListeners();
        }

        // Update is called once per frame
        void Update()
        {

        }
        //Init
        private void BindViews()
        {
            try
            {
                nextButton = GameObject.Find("NextButton").GetComponent<Button>();
                restartButton = GameObject.Find("RestartButton").GetComponent<Button>();
                quitButton = GameObject.Find("QuitButton").GetComponent<Button>();
                levelCompletedText = GameObject.Find("LevelCompletedText").GetComponent<Text>();
            }
            catch
            {
                Debug.LogError("Failed to bind views");
            }
        }
        private void BindListeners()
        {
            restartButton.onClick.AddListener(() => { SceneManager.LoadScene(SceneManager.GetActiveScene().name); });
            quitButton.onClick.AddListener(() => { SceneManager.LoadScene("MainMenu", LoadSceneMode.Single); });
        }
        //...
        public void SetLevelCompletedWindowVisibility(bool visibility = true)
        {
            levelCompleteWindow.SetActive(visibility);
        }
        public void SetToggleUIVisibility(bool visibility = true)
        {
            gameObject.SetActive(visibility);
        }
        public void ShowLevelCompletedWindow(bool win)
        {
            if (!win)
            {
                nextButton.gameObject.SetActive(false);
            }
        }
    }
}
