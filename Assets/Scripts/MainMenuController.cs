using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{

    private Button playButton, settingsButton, quitButton;

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

    private void BindViews()
    {
        try
        {
            playButton = GameObject.Find("PlayButton").GetComponent<Button>();
            settingsButton = GameObject.Find("SettingsButton").GetComponent<Button>();
            quitButton = GameObject.Find("QuitButton").GetComponent<Button>();
        }
        catch
        {
            Debug.LogError("Failed to bind Views");
        }
    }
    private void BindListeners()
    {
        playButton.onClick.AddListener(() => { SceneManager.LoadScene("SampleScene", LoadSceneMode.Single); });
        quitButton.onClick.AddListener(() => { Application.Quit(); });
    }
}
