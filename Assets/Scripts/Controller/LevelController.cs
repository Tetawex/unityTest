using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Util;
using UnityEngine.EventSystems;
using Assets.Scripts.Messaging;
using System.Linq;

namespace Assets.Scripts.Controller
{
    public class LevelController : MonoBehaviour
    {
        static float audioTime;

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
        [SerializeField]
        private AudioClip enemyWhistleClip;
        [SerializeField]
        private float[] enemyFleeIncreases;
        [SerializeField]
        private float fleeGoal = 1f;
        
        public delegate void RoundCompletedEventHandler(int roundNumber);
        public event RoundCompletedEventHandler RoundCompletedEvent;
        private bool musicStarted;
        private float fleeLevel = 0f;


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
                var aliveEnemies = roundContainer.CurrentEnemies
                    .Where(a => !((EnemyController)a).Dead)
                    .ToList();
                roundContainer.SpawnNextRound(aliveEnemies);
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
            if (!fightActive)
                return;
            if (!musicStarted || Application.platform == RuntimePlatform.WebGLPlayer)
            {
                fightMusic.time = audioTime;
                fightMusic.Play();
                musicStarted = true;
            }
            else
                fightMusic.UnPause();
        }

        private void Update()
        {
            if (fightActive && !playerController.Dead && enemiesRemaining > 0 && roundContainer.HasMoreRounds)
            {
                int index = enemiesRemaining - 1;
                if (index < enemyFleeIncreases.Length)
                {
                    fleeLevel += enemyFleeIncreases[index] * Time.deltaTime;
                    if (fleeLevel >= fleeGoal)
                    {
                        EndAction();
                        sfxSource.PlayOneShot(enemyWhistleClip);
                    }
                }
            }
        }

        public void EndAction()
        {
            fightActive = false;
            audioTime = fightMusic.time;
            fleeLevel = 0f;


            if (Application.platform != RuntimePlatform.WebGLPlayer)
                fightMusic.Pause();
            else
                fightMusic.Stop();

            //enemiesRemaining = 0;
            gunController.Holster();
            playerController.CanDraw = false;
            playerMovement.EnableMovement = false;

            Invoke("StartNewRound", nextRoundTime);
        }


        public void OnPlayerDeath()
        {
            audioTime = fightMusic.time;
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