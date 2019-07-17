using Assets.Scripts.Interface;
using Assets.Scripts.Messaging;
using Assets.Scripts.Model;
using Assets.Scripts.Util;
using UnityEngine;
using UnityEngine.EventSystems;
namespace Assets.Scripts.Controller
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField]
        private LayerMask shootMask;
        [SerializeField]
        private float shootSphereCastWidth = .3f;
        [SerializeField]
        private float shootShake =.1f;
        [SerializeField]
        private float killShake = .5f;
        [SerializeField]
        private float focusShakePerKill;
        [SerializeField]
        private float shakeSpeed;
        [SerializeField]
        private float shakeSpeedPerKill;

        private GunController gunController;
        private LevelController levelController;
        private new Camera camera;
        private Animator animator;

        public bool HasProtection = false;

        public AudioSource BounceAudioSource;
        public AudioSource HitAudioSource;

        private int enemiesKilledInFocus = 0;

        private bool dead;
        public bool Dead
        {
            get { return dead; }
            private set { dead = value; }
        }
        public bool CanDraw = false;

        // Use this for initialization
        void Start()
        {
            gunController = GetComponentInChildren<GunController>();
            levelController = Utils.getSingleton<LevelController>();
            camera = GetComponentInChildren<Camera>();
            animator = GetComponent<Animator>();
        }
        public void GetShot()
        {
            if (HasProtection)
            {
                //SendDrawEvent();
                HasProtection = false;
                BounceAudioSource.Play();
            }
            else if (true)
            {
                Dead = true;
                levelController.OnPlayerDeath();
                animator.SetBool("Dead", true);
                HitAudioSource.Play();
            }
        }
        // Update is called once per frame
        void Update()
        {
            var cursorController = Utils.getSingleton<CursorController>();
            Ray ray = camera.ScreenPointToRay(cursorController.ScreenPosition);
            //Raycasting
            RaycastHit hit;
            var didRaycastHitEnemy = Physics.SphereCast(ray.origin, shootSphereCastWidth, ray.direction, out hit, Mathf.Infinity, shootMask);
            cursorController.isOverEnemy = didRaycastHitEnemy;
            var timeController = Utils.getSingleton<TimeController>();
            if (Input.GetMouseButtonDown(0) && CanDraw && !Dead && !Utils.getSingleton<TimeController>().isTransitioning)
            {
                if (!gunController.Drawn)
                {
                    Draw();
                }
                else if (gunController.CanShoot)
                {
                    //if (!levelController.FightActive)
                    //    levelController.StartAction();
                    gunController.Shoot();
                    
                    if (didRaycastHitEnemy)
                    {
                        var objectHit = hit.collider.gameObject;
                        var entity = objectHit.GetComponent<IDamageableEntity>();
                        if (entity != null)
                        {
                            entity.ReceiveShot(new Shot(gunController.Damage, hit.point, transform.position));
                        }

                        if (!timeController.IsFocusing)
                            enemiesKilledInFocus = 0;
                        CameraShake.instance.shakeSpeed = shakeSpeed * (shakeSpeedPerKill * (float)enemiesKilledInFocus);
                        CameraShake.instance.setScreenShake(killShake + (focusShakePerKill * (float)enemiesKilledInFocus));

                        if (timeController.IsFocusing)
                            enemiesKilledInFocus++;

                        //Debug.Log(hit.point);
                        //gunController.LookAt(hit.point);
                    }
                    else
                    {
                        CameraShake.instance.shakeSpeed = shakeSpeed;
                        CameraShake.instance.setScreenShake(shootShake);
                    }
                    Utils.getSingleton<TimeController>().RegisterShot(didRaycastHitEnemy);
                    Utils.getSingleton<PlayerMovement>().TriggerGunRecoil();
                    animator.SetTrigger("Fire");
                }

            }
            if (!dead && gunController.Drawn)
            {
                Physics.SphereCast(ray.origin, shootSphereCastWidth, ray.direction, out hit, Mathf.Infinity);
                gunController.LookAt(hit.point);
            }

            if (!timeController.IsFocusing)
                enemiesKilledInFocus = 0;
        }

        public void Draw()
        {
            levelController.StartAction();
            gunController.StartDrawing();
        }

        void OnTriggerEnter(Collider other)
        {
            if (dead || !levelController.FightActive)
                return;

            if (other.tag.Equals("Projectile"))
            {
                GetShot();
            }
        }

        //private void SendDrawEvent()
        //{
        //    ExecuteEvents.Execute<IDrawShootMessageTarget>(Utils.getSingleton<GameController>().gameObject, null, (x, y) => x.PlayerDrawed());
        //}
    }
}
