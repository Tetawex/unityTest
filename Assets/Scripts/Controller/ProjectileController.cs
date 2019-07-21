using Assets.Scripts.Interface;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Assets.Scripts.Controller
{
    public class ProjectileController : MonoBehaviour
    {
        public EnemyController enemy;
        public bool harmless = false;

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        void OnCollisionEnter(Collision collision)
        {
            var other = collision.gameObject;

            IDamageablePlayer playerHitbox = other.GetComponentInChildren<IDamageablePlayer>();
            if (playerHitbox != null)
            {
                playerHitbox.ReceiveHit();

                Debug.Log("hit sth");
            }
        }
    }
}
