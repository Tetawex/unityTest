using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Util;
using Assets.Scripts.Controller;

namespace Assets.Scripts.Aesthetic
{
    public class FacePlayer : MonoBehaviour
    {
        [SerializeField]
        private bool lockX = false;

        //PlayerMovement playerMovement;
        PlayerController playerController;

        private void Start()
        {
            //playerMovement = Utils.getSingleton<PlayerMovement>();
            playerController = Utils.getSingleton<PlayerController>();
        }

        void LateUpdate()
        {
            if (!playerController.Dead)
            {
                var newRotation = CameraShake.instance.transform.eulerAngles;
                if (lockX)
                    newRotation.x = transform.eulerAngles.x;
                transform.eulerAngles = newRotation;
            }
        }
    }
}