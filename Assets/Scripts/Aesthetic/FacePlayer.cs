using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Util;
using Assets.Scripts.Controller;

namespace Assets.Scripts.Aesthetic
{
    public class FacePlayer : MonoBehaviour
    {
        PlayerMovement playerMovement;
        PlayerController playerController;

        private void Start()
        {
            playerMovement = Utils.getSingleton<PlayerMovement>();
            playerController = Utils.getSingleton<PlayerController>();
        }

        void LateUpdate()
        {
            if (!playerController.Dead)
                transform.rotation = playerMovement.transform.rotation;
        }
    }
}