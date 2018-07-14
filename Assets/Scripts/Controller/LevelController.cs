using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Util;

namespace Assets.Scripts.Controller
{
    public class LevelController : MonoBehaviour
    {
        bool fightActive = false;
        public bool FightActive
        {
            get { return fightActive; }
        }

        int enemiesRemaining = 0;
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

        private void Start()
        {
            playerController = Utils.getSingleton<PlayerController>();
            playerController.CanDraw = false;
            playerMovement = Utils.getSingleton<PlayerMovement>();
            playerMovement.enabled = false;
            gunController = Utils.getSingleton<GunController>();

            StartStandoff();
        }

        void StartStandoff()
        {
            playerController.CanDraw = true;
            playerMovement.enabled = true;
        }

        public void StartAction()
        {
            fightActive = true;
        }

        public void EndAction()
        {
            fightActive = false;

            enemiesRemaining = 0;
            gunController.Holster();
            playerController.CanDraw = false;
            playerMovement.enabled = false;
        }

        public void OnPlayerDeath()
        {
            playerMovement.enabled = false;
        }
    }
}