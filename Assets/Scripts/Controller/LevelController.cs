using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Util;
using UnityEngine.EventSystems;
using Assets.Scripts.Messaging;

namespace Assets.Scripts.Controller
{
    public class LevelController : MonoBehaviour
    {
        [SerializeField]
        private float nextRoundTime;
        [SerializeField]
        private AudioSource standOffMusic;
        [SerializeField]
        private AudioSource fightMusic;
        [SerializeField]
        private float fightMusicStartDelay;

        public delegate void RoundCompletedEventHandler(int roundNumber);
        public event RoundCompletedEventHandler RoundCompletedEvent;


        public delegate void PlayerStartedActionEventHandler();
        public event PlayerStartedActionEventHandler PlayerStartedActionEvent;

        bool fightActive = false;
        public bool FightActive
        {
            get { return fightActive; }
        }

        int enemiesRemaining = 0;//mb make it private?
        public int EnemiesRemaining
        {
            get { return enemiesRemaining; }
            set
            {
                enemiesRemaining = value;
                if (value <= 0)
                    EndAction();
            }
        }

        PlayerController playerController;
        PlayerMovement playerMovement;
        GunController gunController;
        RoundContainer roundContainer;
        GameController gameController;

        private void Start()
        {
            playerController = Utils.getSingleton<PlayerController>();
            playerController.CanDraw = false;
            playerMovement = Utils.getSingleton<PlayerMovement>();
            playerMovement.EnableMovement = false;
            gunController = Utils.getSingleton<GunController>();
            roundContainer = Utils.getSingleton<RoundContainer>();
            gameController = Utils.getSingleton<GameController>();

            Invoke("StartStandoff", 1f); //TODO Serialize or trigger via animationevent when enemies land
        }

        public void StartNewRound()
        {
            if (roundContainer.HasMoreRounds)
            {
                roundContainer.SpawnNextRound();
                Invoke("StartStandoff", 1f); //TODO Serialize or trigger via animationevent when enemies land

                RoundCompletedEvent.Invoke(roundContainer.CurrentRound);
            }
            else
                gameController.InvokeLevelCompletedEvent(true);
        }

        public void OnEnemiesHitGround()
        {
            //standOffMusic.Play();
        }

        public void StartStandoff()
        {
            standOffMusic.Play();
            playerController.CanDraw = true;
            playerMovement.EnableMovement = true;
            gunController.ResetGun();
        }

        public void StartAction()
        {
            PlayerStartedActionEvent.Invoke();
            fightActive = true;
            roundContainer.CurrentEnemies.ForEach((enemy) => { enemy.Draw(); });
            standOffMusic.Stop();
            fightMusic.PlayScheduled(AudioSettings.dspTime + fightMusicStartDelay);
        }

        public void EndAction()
        {
            fightActive = false;
            fightMusic.Stop();

            enemiesRemaining = 0;
            gunController.Holster();
            playerController.CanDraw = false;
            playerMovement.EnableMovement = false;

            Invoke("StartNewRound", nextRoundTime);
        }


        public void OnPlayerDeath()
        {
            fightMusic.Stop();
            playerMovement.EnableMovement = false;
            ExecuteEvents.Execute<IDrawShootMessageTarget>(gameController.gameObject, null, (x, y) => x.EnemyShotPlayer());
        }

        //public void PlayerDrawed()
        //{
        //    StartAction();
        //}
    }
}