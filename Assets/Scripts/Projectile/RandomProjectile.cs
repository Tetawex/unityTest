using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Util;
using Assets.Scripts.Controller;

namespace Assets.Scripts.Projectile
{
    public class RandomProjectile : MonoBehaviour
    {
        [SerializeField]
        private GameObject shot;
        [SerializeField]
        private GameObject targetIcon;
        [SerializeField]
        private float crosshairFrontDistance;
        [SerializeField]
        private Vector2 shootAtBounds;
        [SerializeField]
        private float bulletSpeed;

        private MoveRigidbodyTowards bullet;

        void Start()
        {
            bullet = Instantiate(shot, transform.position, Quaternion.identity).GetComponent<MoveRigidbodyTowards>();
            var player = Utils.getSingleton<PlayerController>();
            var aimAt = player.transform.position;
            aimAt = new Vector3(MathHelper.randomRangeFromVector(shootAtBounds), player.transform.position.y,
                player.transform.position.z - crosshairFrontDistance);
            bullet.Direction = aimAt - transform.position;
            bullet.Multiplier = bulletSpeed;

            targetIcon = Instantiate(targetIcon, aimAt, Quaternion.identity);
        }

        void Update()
        {
            if (bullet.transform.position.z < targetIcon.transform.position.z
                //(Utils.getSingleton<PlayerController>().transform.position.z)
                || !Utils.getSingleton<LevelController>().FightActive)
            {
                Destroy(targetIcon);
                enabled = false;
            }
        }
    }
}