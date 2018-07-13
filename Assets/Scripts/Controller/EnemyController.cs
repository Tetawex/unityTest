using Assets.Scripts;
using Assets.Scripts.Messaging;
using Assets.Scripts.Model;
using Assets.Scripts.Util;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Assets.Scripts.Interface;

namespace Assets.Scripts.Controller
{
    public class EnemyController : MonoBehaviour, IDrawer, IShooter, IMortalEntity, IBaseStats
    {
        [SerializeField]
        private GameObject bloodSplatter;
        [SerializeField]
        private GameObject bounceSpark;
        [SerializeField]
        private float ReactionTime = 0.1f;

        private Animator animator;
        private GameController gameController;
        private EnemySoundPlayer enemySoundPlayer;

        private bool useBrutalDeathAnimation = false;

        private bool dead = false;
        public bool Dead
        {
            get
            {
                return dead;
            }
            private set
            {
                dead = value;

            }
        }
        [SerializeField]//TODO: remove
        private float health = 100;
        public float Health
        {
            get
            {
                return health;
            }
            set
            {
                health = value;
                if (health <= 0)
                {
                    Die();
                }
            }
        }
        // Use this for initialization
        void Start()
        {
            animator = GetComponent<Animator>();
            enemySoundPlayer = GetComponent<EnemySoundPlayer>();
            gameController = Utils.getSingleton<GameController>();
        }

        // Update is called once per frame
        void Update()
        {

        }
        //Starts drawing after a delay
        public void Draw()
        {
            if (Dead)
                return;
            Invoke("StartDrawing", ReactionTime + Random.Range(0f, 0.1f));
            ExecuteEvents.Execute<IDrawShootMessageTarget>(gameController.gameObject, null, (x, y) => x.EnemyDrawed());
        }

        public void Shoot()
        {
            if (Dead)
                return;
            enemySoundPlayer.PlayShootSound();
            ExecuteEvents.Execute<IDrawShootMessageTarget>(gameController.gameObject, null, (x, y) => x.EnemyShotPlayer());
        }

        private void StartDrawing()
        {
            enemySoundPlayer.PlayDrawSound();
            animator.SetTrigger("Draw");
        }

        public void ReceiveShot(LocationalShot shot)
        {
            if (Dead) return;

            if (shot.BodyPart == BodyPart.HEAD)
                useBrutalDeathAnimation = true;

            var rotation = Quaternion.LookRotation(shot.OriginPoint).eulerAngles;
            rotation.x = 0;
            if (!(shot.BodyPart == BodyPart.ARMORED_TORSO))
            {
                enemySoundPlayer.PlayHitSound();
                Instantiate(bloodSplatter, shot.ImpactPoint, Quaternion.Euler(rotation));
            }
            else
            {
                enemySoundPlayer.PlayBounceSound();
                Instantiate(bounceSpark, shot.ImpactPoint, Quaternion.Euler(rotation));
            }

            if (shot.BodyPart == BodyPart.HEAD)
                useBrutalDeathAnimation = true;

            Health = Health - shot.Damage;

            animator.Play("Idle");
            Draw();
        }

        public void Die()
        {
            if (Dead)
                return;
            Dead = true;
            gameObject.GetComponentsInChildren<Collider>().ToList().ForEach(collider => { enabled = false; });
            animator.Play("Idle");

            if (!useBrutalDeathAnimation)
                animator.SetBool("Dead", true);
            else
                animator.SetBool("BrutallyDead", true);
        }
    }
}
