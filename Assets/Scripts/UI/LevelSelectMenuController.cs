﻿using Assets.Scripts.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
namespace Assets.Scripts.UI
{
    public class LevelSelectMenuController : MonoBehaviour
    {
        [SerializeField]
        private GameObject buttonPrefab;
        [SerializeField]
        private int levelCount;//should assign dynamically by counting the amount of files in Levels folder

        private GameObject gridLayout;
        private Button quitButton;

        // Use this for initialization
        void Start()
        {
            //PlayerPrefs.DeleteAll();
            BindViews();
            BindListeners();
        }

        // Update is called once per frame
        void Update()
        {

        }

        private void BindViews()
        {
            quitButton = GameObject.Find("QuitButton").GetComponent<Button>();
            gridLayout = GameObject.Find("GridLayout");
            FillGrid();
        }
        private void BindListeners()
        {
            quitButton.onClick.AddListener(() => { SceneManager.LoadScene("MainMenu", LoadSceneMode.Single); });
        }
        private void FillGrid()
        {
            for (int i = 1; i <= levelCount; i++)
            {
                var buttonGo = Instantiate(buttonPrefab, Vector3.zero, Quaternion.identity, gridLayout.transform);

                var button = buttonGo.GetComponentInChildren<Button>();

                //gah just unlock everything instead
                //if (!(i == 1 || (PlayerPrefs.GetInt(LevelStateData.LevelCodePrefsKey + i, 0)) == 1))
                //button.interactable = false;//disable the button

                var textbox = buttonGo.GetComponentInChildren<Text>();
                textbox.text = (i).ToString();

                int final_i = i;
                button.onClick.AddListener(() =>
                {
                    SceneManager.LoadScene("Level" + (final_i));
                });
            }
        }
    }
}
