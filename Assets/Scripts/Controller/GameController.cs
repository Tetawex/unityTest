using Assets.Scripts.Messaging;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Assets.Scripts.Controller;
using Assets.Scripts.Interface;
using Assets.Scripts.Util;

namespace Assets.Scripts.Controller

{
    public class GameController : MonoBehaviour, IDrawShootMessageTarget
    {
        public static bool IRON_MODE_ENABLED = false;

        [SerializeField]
        public int levelNumber = 0;//TODO assign dynamically somehow

        private float timeBeforeFirstDraw;
        private float elapsedTime = 0f;
        private bool drawed = false;

        public float TimeBeforeFirstDrawUpperBound = 6f;
        public float TimeBeforeFirstDrawLowerBound = 2f;

        private GameObject enemyContainer;

        private IDrawer firstShooter;
        private PlayerController player;

        public delegate void LevelCompletedEventHandler(bool win);
        public event LevelCompletedEventHandler LevelCompletedEvent;

        // Use this for initialization
        void Start()
        {
            player = GameObject.Find("Player").GetComponent<PlayerController>();
            enemyContainer = GameObject.Find("Enemies");

            timeBeforeFirstDraw = Random.Range(TimeBeforeFirstDrawLowerBound, TimeBeforeFirstDrawUpperBound);
        }

        // Update is called once per frame
        void Update()
        {
            //if (elapsedTime < timeBeforeFirstDraw)
            //    elapsedTime += Time.deltaTime;
            //else if (!drawed)
            //{
            //    firstShooter.Draw();
            //    drawed = true;
            //}
        }

        public void EnemyShotPlayer()
        {
            //player.GetShot();
            //Debug.Log("???");

            //Invoke | || || |__ event
            InvokeLevelCompletedEvent(false);
        }
        public void InvokeLevelCompletedEvent(bool win)
        {
            if (win)
                LevelCompleter.UnlockLevel(levelNumber + 1);
            LevelCompletedEvent.Invoke(win);
        }
    }
}
