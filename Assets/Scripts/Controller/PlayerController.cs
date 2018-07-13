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
        private GunController gunController;
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
            camera = GetComponentInChildren<Camera>();
            animator = GetComponent<Animator>();
        }
        public void GetShot()
        {
            if (HasProtection)
            {
                SendDrawEvent();
                HasProtection = false;
                BounceAudioSource.Play();
            }
            else if (true)
            {
                Dead = true;
                animator.SetBool("Dead", true);
                HitAudioSource.Play();
            }
        }
        // Update is called once per frame
        void Update()
        {
            if (Input.GetMouseButtonDown(0) && CanDraw && !Dead)
            {
                if (!gunController.Drawn)
                {
                    gunController.StartDrawing();
                    SendDrawEvent();

                }
                else if (gunController.CanShoot)
                {
                    gunController.Shoot();

                    //Raycasting
                    RaycastHit hit;
                    Ray ray = camera.ScreenPointToRay(Input.mousePosition);

                    if (Physics.Raycast(ray, out hit))
                    {
                        var objectHit = hit.collider.gameObject;
                        var entity = objectHit.GetComponent<IDamageableEntity>();
                        if (entity != null)
                        {
                            entity.ReceiveShot(new Shot(gunController.Damage, hit.point, transform.position));
                        }

                        Debug.Log(hit.point);
                        gunController.LookAt(hit.point);
                    }
                }

            }
        }
        private void SendDrawEvent()
        {
            ExecuteEvents.Execute<IDrawShootMessageTarget>(Utils.getSingleton<GameController>().gameObject, null, (x, y) => x.PlayerDrawed());
        }
    }
}
