using Assets.Scripts;
using Assets.Scripts.Messaging;
using Assets.Scripts.Model;
using Assets.Scripts.Util;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Assets.Scripts.Interface;
using Assets.Scripts.Aesthetic;

namespace Assets.Scripts.Controller
{
    public class EnemyController : MonoBehaviour, IDrawer, IShooter, IMortalEntity, IBaseStats
    {
        private const float bulletFireRateMult = .8f;

        [SerializeField]
        private Vector2 drawDelayRange;
        [SerializeField]
        private Vector2 initialFireDelayRange;
        [SerializeField]
        private Vector2 refireDelayRange;
        [SerializeField]
        private GameObject projectilePrefab;
        [SerializeField]
        private Transform fireLocation;
        [SerializeField]
        private GameObject bloodSplatter;
        [SerializeField]
        private GameObject bounceSpark;
        [SerializeField]
        private float fleeDelay = .5f;
        [SerializeField]
        private float fleeSpeed = 25f;

        private float deathAnimationFocusSpeed = .4f;

        private Animator animator;
        private GameController gameController;
        private EnemySoundPlayer enemySoundPlayer;

        private bool useBrutalDeathAnimation = false;
        private TimeController timeController;
        private LevelController levelController;
        private bool hasStartedAttacking = false;
        private bool isFleeing;
        private bool fleeRight;

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
            Utils.getSingleton<LevelController>().EnemiesRemaining++;
            animator = GetComponent<Animator>();
            enemySoundPlayer = GetComponent<EnemySoundPlayer>();
            gameController = Utils.getSingleton<GameController>();
            timeController = Utils.getSingleton<TimeController>();
            levelController = Utils.getSingleton<LevelController>();
        }

        // Update is called once per frame
        void Update()
        {
            if (isFleeing)
            {
                fleeDelay -= Time.deltaTime;
                if (fleeDelay < 0f)
                    transform.position += Vector3.right * (fleeRight ? 1f : -1f);
            }
            else if (dead && !timeController.IsFocusing && animator.GetFloat("DeathSpeed") == deathAnimationFocusSpeed)
                animator.SetFloat("DeathSpeed", 1f);
            else if (hasStartedAttacking && !dead && !levelController.FightActive && !Utils.getSingleton<PlayerController>().Dead)
            {
                //isFleeing = true;
                //fleeRight = MathHelper.randomBool();
                animator.SetTrigger("Flee");
                hasStartedAttacking = false;
            }
        }

        //Starts drawing after a delay
        public void Draw()
        {
            if (Dead)
                return;
            hasStartedAttacking = true;
            CancelInvoke();
            Invoke("StartDrawing", MathHelper.randomRangeFromVector(drawDelayRange));
            //ExecuteEvents.Execute<IDrawShootMessageTarget>(gameController.gameObject, null, (x, y) => x.EnemyDrawed());
        }

        public void Shoot()
        {
            if (Dead || Utils.getSingleton<PlayerController>().Dead || !levelController.FightActive)
                return;
            Invoke("Shoot", MathHelper.randomRangeFromVector(refireDelayRange) / bulletFireRateMult);

            enemySoundPlayer.PlayShootSound();
            animator.SetTrigger("Shoot");

            var controller = Instantiate(projectilePrefab, fireLocation.transform.position, Quaternion.identity).GetComponent<ProjectileController>();
            controller.enemy = this;


            //ExecuteEvents.Execute<IDrawShootMessageTarget>(gameController.gameObject, null, (x, y) => x.EnemyShotPlayer());
        }

        private void StartDrawing()
        {
            Invoke("Shoot", MathHelper.randomRangeFromVector(initialFireDelayRange) / 2f);

            enemySoundPlayer.PlayDrawSound();
            animator.SetTrigger("Draw");
        }

        public void ReceiveShot(LocationalShot shot)
        {
            if (Dead) return;

            if (shot.BodyPart == BodyPart.HEAD)
                useBrutalDeathAnimation = false;

            var rotation = Quaternion.LookRotation(shot.OriginPoint).eulerAngles;
            rotation.x = 0;
            if ((Health - 4 * shot.Damage) > 0)
            {
                enemySoundPlayer.PlayHitSound();
                Instantiate(bounceSpark, shot.ImpactPoint, Quaternion.Euler(rotation));
            }
            else
            {
                enemySoundPlayer.PlayHitSound();
                Instantiate(bloodSplatter, shot.ImpactPoint, Quaternion.Euler(rotation));
            }

            Health = Health - 4 * shot.Damage;
            //Health = 0f;

            //animator.Play("Idle");
            //Draw();
        }

        public void Die()
        {
            if (Dead)
                return;
            var levelController = Utils.getSingleton<LevelController>();
            levelController.EnemiesRemaining--;
            ShotAnimation.instance.shoot();
            CancelInvoke();
            Dead = true;
            gameObject.GetComponentsInChildren<Collider>().ToList().ForEach(collider => { collider.enabled = false; });
            //animator.Play("Idle");
            if (timeController.IsFocusing)
                animator.SetFloat("DeathSpeed", deathAnimationFocusSpeed);
            if (!useBrutalDeathAnimation)
                animator.SetBool("Dead", true);
            else
                animator.SetBool("BrutallyDead", true);

            GetComponent<FacePlayer>().enabled = false;
            transform.position += Vector3.up * Random.Range(-.01f, .01f);
        }
    }
}
