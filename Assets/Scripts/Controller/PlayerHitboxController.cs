using Assets.Scripts.Interface;
using Assets.Scripts.Messaging;
using Assets.Scripts.Util;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Scripts.Controller
{
    public class PlayerHitboxController : MonoBehaviour, IDamageablePlayer
    {
        //[SerializeField]
        //private PlayerController playerController;
        private GameObject gameController;

        // Use this for initialization
        void Start()
        {
            gameController = Utils.GetGameController();
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void ReceiveHit()
        {
            ExecuteEvents.Execute<IDrawShootMessageTarget>(gameController, null, (x, y) => x.EnemyShotPlayer());
        }

    }
}