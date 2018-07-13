using Assets.Scripts;
using Assets.Scripts.Messaging;
using Assets.Scripts.Model;
using Assets.Scripts.Util;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
namespace Assets.Scripts.Interface
{
    public class EnemyController : MonoBehaviour, IDrawer, IShooter, IMortalEntity, IBaseStats
    {
        public AudioSource receiveDamageAudioSource;
        public AudioSource drawAudioSource;
        public AudioSource shootAudioSource;
        public AudioSource bounceAudioSource;

        public GameObject bloodSplatter;
        public GameObject bounceSpark;

        private Animator animator;

        private GameObject gameController;

        public float ReactionTime = 0.1f;

        private bool dead = false;
        private bool useBrutalDeathAnimation = false;

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
            gameController = Utils.GetGameController();
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
            ExecuteEvents.Execute<IDrawShootMessageTarget>(gameController, null, (x, y) => x.EnemyDrawed());
        }

        public void Shoot()
        {
            if (Dead)
                return;
            shootAudioSource.Play();
            ExecuteEvents.Execute<IDrawShootMessageTarget>(gameController, null, (x, y) => x.EnemyShotPlayer());
        }

        private void StartDrawing()
        {
            drawAudioSource.Play();
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
                receiveDamageAudioSource.Play();
                Instantiate(bloodSplatter, shot.ImpactPoint, Quaternion.Euler(rotation));
            }
            else
            {
                bounceAudioSource.Play();
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
