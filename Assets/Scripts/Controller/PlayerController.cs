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
        private GunController gunController;
        private LevelController levelController;
        private new Camera camera;
        private Animator animator;

        public bool HasProtection = false;

        public AudioSource BounceAudioSource;
        public AudioSource HitAudioSource;
        

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
            Ray ray = camera.ScreenPointToRay(Input.mousePosition);
            //Raycasting
            RaycastHit hit;
            if (Input.GetMouseButtonDown(0) && CanDraw && !Dead)
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


                    if (Physics.Raycast(ray, out hit, Mathf.Infinity, shootMask))
                    {
                        var objectHit = hit.collider.gameObject;
                        var entity = objectHit.GetComponent<IDamageableEntity>();
                        if (entity != null)
                        {
                            entity.ReceiveShot(new Shot(gunController.Damage, hit.point, transform.position));
                        }

                        //Debug.Log(hit.point);
                        //gunController.LookAt(hit.point);
                    }
                    else
                    {
                    }
                }

            }
            if (!dead)
            {
                Physics.Raycast(ray, out hit);
                gunController.LookAt(hit.point);
            }
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
