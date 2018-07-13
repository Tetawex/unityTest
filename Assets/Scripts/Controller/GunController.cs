using UnityEngine;
namespace Assets.Scripts.Controller
{
    public class GunController : MonoBehaviour
    {

        private Animator animator;

        public AudioSource shootAudioSource;
        public AudioSource drawAudioSource;


        private float currentCooldown = 0f;
        public float Cooldown = 0.5f;

        public float Damage = 100;

        public float TurnRate = 0.5f;
        public float turnState = 0f;

        public Quaternion fromRotation;
        public Quaternion toRotation;

        private bool startedDrawing;
        private bool drawn;
        public bool Drawn
        {
            get { return drawn; }
            private set { drawn = value; }
        }

        public bool CanShoot
        {
            get { return currentCooldown >= Cooldown; }
            private set { }
        }

        // Use this for initialization
        void Start()
        {
            animator = GetComponent<Animator>();
        }

        // Update is called once per frame
        void Update()
        {
            //Manage cooldown
            currentCooldown += Time.deltaTime;
            if (currentCooldown > Cooldown)
                currentCooldown = Cooldown;

            //Perform smooth turning
            turnState += Time.deltaTime * TurnRate;
            if (turnState > 1f)
                turnState = 1f;
            transform.rotation = Quaternion.Slerp(fromRotation, toRotation, turnState);
        }

        public void Shoot()
        {
            //if (animator.GetCurrentAnimatorStateInfo(0).IsName("Shoot"))
            //return;
            //Fancy effects
            currentCooldown = 0f;
            shootAudioSource.Play();
            animator.Play("Idle", -1, 0);
            animator.SetTrigger("Shoot");
        }
        public void StartDrawing()
        {
            if (startedDrawing)
                return;

            startedDrawing = true;
            animator.SetTrigger("Draw");
            drawAudioSource.Play();
        }
        public void FinishDrawing()
        {
            Drawn = true;
        }
        public void LookAt(Vector3 point)
        {
            toRotation = Quaternion.LookRotation(point - transform.position, Vector3.up);
            fromRotation = transform.rotation;
            turnState = 0f;
        }
    }
}
