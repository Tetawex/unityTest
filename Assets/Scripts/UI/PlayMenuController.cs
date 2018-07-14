using Assets.Scripts.Controller;
using Assets.Scripts.Model;
using Assets.Scripts.Util;
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

        private GameController gameController;

        [SerializeField]
        private float clickCooldown = 1f;

        // Use this for initialization
        void Start()
        {
            gameController = Utils.getSingleton<GameController>();
            gameController.LevelCompletedEvent += ShowLevelCompletedWindow;

            BindViews();
            BindListeners();

            levelCompleteWindow.SetActive(false);
            SetButtonsEnabled(false);
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
                levelCompleteWindow = GameObject.Find("WindowWithVerticalLayout");
            }
            catch
            {
                Debug.LogError("Failed to bind views");
            }
        }
        private void BindListeners()
        {
            nextButton.onClick.AddListener(() => { SceneManager.LoadScene("Level" + (gameController.levelNumber + 1)); });
            restartButton.onClick.AddListener(() => { SceneManager.LoadScene(SceneManager.GetActiveScene().name); });
            quitButton.onClick.AddListener(() => { SceneManager.LoadScene("LevelSelectMenu", LoadSceneMode.Single); });
        }
        //...
        public void EnableButtons()
        {
            SetButtonsEnabled();
        }
        public void SetButtonsEnabled(bool enabled = true)
        {
            restartButton.interactable = enabled;
            nextButton.interactable = enabled;
            quitButton.interactable = enabled;
        }
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
            SetLevelCompletedWindowVisibility(true);
            Invoke("EnableButtons", clickCooldown);
            if (win)
            {
                nextButton.gameObject.SetActive(true);
                levelCompletedText.text = Strings.LevelCompletedSuccess;
            }
            else
            {
                nextButton.gameObject.SetActive(false);
                levelCompletedText.text = Strings.LevelCompletedFailure;
            }
        }
    }
}
