﻿using Assets.Scripts.Messaging;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Assets.Scripts.Controller;
using Assets.Scripts.Interface;

namespace Assets.Scripts.Controller

{
    public class GameController : MonoBehaviour, IDrawShootMessageTarget
    {
        private float timeBeforeFirstDraw;
        private float elapsedTime = 0f;
        private bool drawed = false;

        public float TimeBeforeFirstDrawUpperBound = 6f;
        public float TimeBeforeFirstDrawLowerBound = 2f;

        private GameObject enemyContainer;

        private IDrawer firstShooter;
        private PlayerController player;

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
            player.GetShot();
        }
    }
}
