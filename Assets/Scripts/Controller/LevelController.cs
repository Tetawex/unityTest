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
            playerMovement.enabled = false;
            gunController = Utils.getSingleton<GunController>();
            roundContainer = Utils.getSingleton<RoundContainer>();
            gameController = Utils.getSingleton<GameController>();

            StartStandoff();
        }

        public void StartNewRound()
        {
            if (roundContainer.HasMoreRounds)
            {
                roundContainer.SpawnNextRound();
                Invoke("StartStandoff", 1f); //TODO Serialize or trigger via animationevent when enemies land
            }
            else
                gameController.InvokeLevelCompletedEvent(true);
        }

        public void StartStandoff()
        {
            playerController.CanDraw = true;
            playerMovement.enabled = true;
            gunController.ResetGun();
        }

        public void StartAction()
        {
            fightActive = true;
            roundContainer.CurrentEnemies.ForEach((enemy) => { enemy.Draw(); });
        }

        public void EndAction()
        {
            fightActive = false;

            enemiesRemaining = 0;
            gunController.Holster();
            playerController.CanDraw = false;
            playerMovement.enabled = false;

            Invoke("StartNewRound", nextRoundTime);
        }


        public void OnPlayerDeath()
        {
            playerMovement.enabled = false;
            ExecuteEvents.Execute<IDrawShootMessageTarget>(gameController.gameObject, null, (x, y) => x.EnemyShotPlayer());
        }

        //public void PlayerDrawed()
        //{
        //    StartAction();
        //}
    }
}