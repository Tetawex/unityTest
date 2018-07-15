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
        private float standoffStartTime;
        [SerializeField]
        private AudioSource standOffMusic;
        [SerializeField]
        private AudioSource fightMusic;
        [SerializeField]
        private float fightMusicStartDelay;
        [SerializeField]
        private float standoffMusicStartDelay;
        [SerializeField]
        private AudioClip enemyLandClip;

        public delegate void RoundCompletedEventHandler(int roundNumber);
        public event RoundCompletedEventHandler RoundCompletedEvent;
        private bool musicStarted;


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
        AudioSource sfxSource;

        private void Start()
        {
            playerController = Utils.getSingleton<PlayerController>();
            playerController.CanDraw = false;
            playerMovement = Utils.getSingleton<PlayerMovement>();
            playerMovement.EnableMovement = false;
            gunController = Utils.getSingleton<GunController>();
            roundContainer = Utils.getSingleton<RoundContainer>();
            gameController = Utils.getSingleton<GameController>();
            sfxSource = GetComponent<AudioSource>();

            Invoke("StartStandoff", standoffStartTime);
        }

        public void StartNewRound()
        {
            if (roundContainer.HasMoreRounds)
            {
                roundContainer.SpawnNextRound();
                Invoke("StartStandoff", standoffStartTime);

                RoundCompletedEvent.Invoke(roundContainer.CurrentRound);
            }
            else
                gameController.InvokeLevelCompletedEvent(true);
        }

        public void OnEnemiesHitGround()
        {
            sfxSource.PlayOneShot(enemyLandClip);
        }

        public void StartStandoff()
        {
            standOffMusic.PlayScheduled(AudioSettings.dspTime + standoffMusicStartDelay);
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
            Invoke("PlayFightMusic", fightMusicStartDelay);
        }

        void PlayFightMusic()
        {
            if (!musicStarted)
            {
                fightMusic.Play();
                musicStarted = true;
            }
            else
                fightMusic.UnPause();
        }

        public void EndAction()
        {
            fightActive = false;
            fightMusic.Pause();

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